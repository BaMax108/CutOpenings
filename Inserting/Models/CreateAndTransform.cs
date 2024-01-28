using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using CutOpeningsPlugin.Inserting.Types;
using CutOpeningsPlugin.Inserting.Presenters;
using CutOpeningsPlugin.Inserting.Data;
using CutOpeningsPlugin.Inserting.Data.Enums;
using CutOpeningsPlugin.Inserting.Data.Interfaces;
using CutOpeningsPlugin.Other;

namespace CutOpeningsPlugin.Inserting.Models
{
    /// <summary></summary>
    public class CreateAndTransform
    {
        readonly private InsertingPresenter Presenter;
        private XYZ CurrentBasePoint { get; set; }
        private FamilySymbol CurrentFamily { get; set; }
        private RepositoryFSymbols Repository { get; set; }
        private ActionsWithParameters ChangingParameters { get; set; }

        /// <summary></summary>
        public CreateAndTransform(InsertingPresenter _presenter, List<string> _ofsets, FamilySymbol _currentFamily)
        {
            Presenter = _presenter;
            Repository = new RepositoryFSymbols();
            ChangingParameters = new ActionsWithParameters(Presenter.Doc, _ofsets);
            CurrentBasePoint = BasePoint.GetProjectBasePoint(Presenter.Doc).Position;
            CurrentFamily = _currentFamily;
        }

        /// <summary>
        /// Размещение отверстий в пространстве модели.
        /// </summary>
        public void InsertingOpenings(List<ElementId> groupIds, List<ElementId> localGroudIds, XYZ linkOrigin, List<Element> openingsFromLink)
        {
            string name;
            foreach (var element in openingsFromLink)
            {
                //if (element.Id.IntegerValue != 2991026) continue;

                name = element.get_Parameter(BuiltInParameter.ELEM_FAMILY_PARAM).AsValueString();

                if (Repository.OpeningsDictionary[ShapeType.Rechtangle].Select(x => x.Name).Contains(name) ||
                    Repository.OpeningsDictionary[ShapeType.Circle].Select(x => x.Name).Contains(name))
                {
                    CreateInstance(element, name, groupIds, localGroudIds, linkOrigin);
                }
            }
        }

        private LevelInfo[] GetLevels()
        { 
            var levels = new FilteredElementCollector(Presenter.Doc)
                .OfCategory(BuiltInCategory.OST_Levels)
                .WhereElementIsNotElementType()
                .ToElements();
            
            List<LevelInfo> levelInfos = new List<LevelInfo>();
            foreach (var l in levels)
            {
                levelInfos.Add(new LevelInfo(l.Id, l.Name, (l as Level).Elevation));
            }

            return levelInfos.OrderBy(x=>x.Elevation).ToArray();
        }

        private LevelInfo GetHostLevel(Element element)
        {
            var levels = GetLevels();

            Element linkLevelElement = Presenter.CurrentLinkInstance.GetLinkDocument().GetElement(element.LevelId);
            if (linkLevelElement == null) return GetZeroLevel(levels);
            Level linkLevel = linkLevelElement as Level;
            LevelInfo result = levels.FirstOrDefault(x=>x.Name == linkLevel.Name);
            
            return result ?? GetZeroLevel(levels);
        }

        private LevelInfo GetZeroLevel(LevelInfo[] array)
        {
            foreach (var level in array)
            {
                if (level.Elevation >= 0)
                {
                    return level;
                }
            }
            return null;
        }

        /// <summary>
        /// Создание нового экземпляра семейства
        /// </summary>
        /// <returns></returns>
        private void CreateInstance(Element element, string name, List<ElementId> groupIds, List<ElementId> localGroudIds, XYZ linkOrigin)
        {
            FamilyInstance newInstance = null;
            IOpening fParams = GetFamilyParams(name);
            if (fParams == null) return;
            //var localPoint = fParams.SetLocationPoint(Presenter.Doc, CurrentBasePoint, element, linkOrigin - CurrentBasePoint);

            
            var location = (element as FamilyInstance).Location;
            
            var localPoint = fParams.SetLocationPoint(Presenter.Doc, CurrentBasePoint, element, linkOrigin - CurrentBasePoint);
            
            if (localPoint == null) return;
            XYZ newCoords = Presenter.RvtLink.GetTotalTransform().OfPoint(localPoint);
            var levelHost = GetHostLevel(element);
            if (levelHost == null) return;

            using (Transaction t = new Transaction(Presenter.Doc, "InsertFamily"))
            {
                t.SetFailureHandlingOptions(t.GetFailureHandlingOptions()
                            .SetFailuresPreprocessor(new WarningDiscard()));
                t.Start("InsertFamily");
                if (!CurrentFamily.IsActive) { CurrentFamily.Activate(); Presenter.Doc.Regenerate(); }
               
                newInstance = Presenter.Doc.Create.NewFamilyInstance(
                    newCoords,
                    CurrentFamily,
                    Presenter.Doc.GetElement(levelHost.ElementId),
                    StructuralType.NonStructural);

                try { t.Commit(); }
                catch (Exception)
                {
                    return;
                }
            }

            Presenter.AddOpenings(newInstance);
            groupIds.Add(newInstance.Id);
            localGroudIds.Add(newInstance.Id);

            Element host = (element as FamilyInstance).Host;
            if (host != null)
            {
                if (host.Category.Id == new ElementId(-2000011)) // -2000011 Walls
                {
                    SearchingFamilyInstanceForWall(name, newInstance, element, fParams);
                }
                else
                {
                    SearchingFamilyInstanceForFloor(name, newInstance, element, fParams);
                }
            }
            else
            {
                switch (Math.Round((element as FamilyInstance).FacingOrientation.Z))
                {
                    case 1D:
                        SearchingFamilyInstanceForWall(name, newInstance, element, fParams);
                        break;
                    case 0D:
                        SearchingFamilyInstanceForFloor(name, newInstance, element, fParams);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Получение списка параметров.
        /// </summary>
        private IOpening GetFamilyParams(string name)
        {
            IOpening opening = default;
            foreach (var list in Repository.OpeningsDictionary)
            {
                opening = list.Value.FirstOrDefault(x => name.Contains(x.Name));
                if (opening != null) break;
            }

            return opening;
        }

        /// <summary>
        /// Поиск семейств в репозитории
        /// </summary>
        private void SearchingFamilyInstanceForWall(string name, FamilyInstance newInstance, Element element, IOpening fParams)
        {
            if (Repository.OpeningsDictionary[ShapeType.Rechtangle].FirstOrDefault(e => name.Contains(e.Name)) != null)
            {
                ChangingParameters.EditWallRectangleOpenings(newInstance, element, fParams, Presenter.RvtLink.GetTotalTransform());
            }
            else if (Repository.OpeningsDictionary[ShapeType.Circle].FirstOrDefault(e => name.Contains(e.Name)) != null)
            {
                ChangingParameters.EditWallCircleOpenings(newInstance, element, fParams);
            }
        }

        /// <summary>
        /// Поиск семейств в репозитории
        /// </summary>
        private void SearchingFamilyInstanceForFloor(string name, FamilyInstance newInstance, Element element, IOpening fParams)
        {
            if (Repository.OpeningsDictionary[ShapeType.Rechtangle].FirstOrDefault(e => name.Contains(e.Name)) != null)
            {
                ChangingParameters.EditFloorRectangleOpenings(newInstance, element, fParams);
            }
            else if (Repository.OpeningsDictionary[ShapeType.Circle].FirstOrDefault(e => name.Contains(e.Name)) != null)
            {
                ChangingParameters.EditFloorCircleOpenings(newInstance, element, fParams);
            }
        }

        
    }
}
