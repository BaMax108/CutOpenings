using Autodesk.Revit.DB;
using CutOpeningsPlugin.Cutting.Data;
using CutOpeningsPlugin.Inserting.Data;
using CutOpeningsPlugin.Other;
using System.Collections.Generic;
using System.Linq;
using Document = Autodesk.Revit.DB.Document;
using RepositoryErrors = CutOpeningsPlugin.Cutting.Data.RepositoryErrors;

namespace CutOpeningsPlugin.Models
{
    /// <summary></summary>
    public class ModelCut
    {
        /// <summary>
        /// An object that represents an open Autodesk Revit project.
        /// </summary>
        private readonly Document Doc;
        private readonly RepositoryErrors Errors;
        private InitialData InitialData;
        /// <summary></summary>
        public ModelCut(Document _doc)
        {
            Doc = _doc;
            Errors = new RepositoryErrors();
            InitialData = new InitialData(_doc);
        }

        /// <summary>
        /// Вырезание геометрии в выбранных элементах.
        /// </summary>
        public void Cut(List<ElementId> selection)
        {
            if (Errors.IsNull(selection)) return;
            BoundingBoxXYZ bb;
            Element element;
            List<Element> intersections;
            List<FamilyInstance> familyInstances;
            foreach (ElementId id in selection)
            {
                element = Doc.GetElement(id);

                bb = element.get_BoundingBox(Doc.ActiveView);
                intersections = new FilteredElementCollector(Doc)
                    .OfCategory(BuiltInCategory.OST_TelephoneDevices)
                    .WherePasses(new BoundingBoxIntersectsFilter(new Outline(bb.Min, bb.Max), false))
                    .ToList();
                if (intersections.Count == 0) continue;

                familyInstances = intersections
                    .Cast<FamilyInstance>()
                    .Where(x => x.Symbol.FamilyName == InitialData.OpeningsName)
                    .ToList();
                if (Errors.IsNull(familyInstances.Count, InitialData.OpeningsName)) continue;

                foreach (FamilyInstance instance in familyInstances)
                {
                    ValidationElementsForCutting(instance, element);
                }
            }
        }

        /// <summary>
        /// Вырезание геометрии в выбранных категориях.
        /// </summary>
        public void Cut(Dictionary<string, Category> selectedCategores)
        {
            List<FamilyInstance> familyInstances = GetFamilyInstances();
            if (Errors.IsNull(familyInstances.Count, InitialData.OpeningsName)) return;

            if (selectedCategores.Count == 0)
                CutBoundingBoxes(familyInstances);
            else
                CutBoundingBoxes(familyInstances, selectedCategores);
        }

        /// <summary>
        /// Получение экземпляров семейства из пространства модели.
        /// </summary>
        private List<FamilyInstance> GetFamilyInstances()
        {
            return new FilteredElementCollector(Doc)
                .WherePasses(new ElementClassFilter(typeof(FamilyInstance)))
                .Where(z => z.Name == InitialData.OpeningsName)
                .Cast<FamilyInstance>()
                .ToList();
        }

        /// <summary>
        /// Вырезание геометрии по пересечениям с Bounding boxes.
        /// </summary>
        private void CutBoundingBoxes(List<FamilyInstance> familyInstances)
        {
            BoundingBoxXYZ bb;
            foreach (FamilyInstance instance in familyInstances)
            {
                bb = instance.get_BoundingBox(Doc.ActiveView);
                switch (instance.LookupParameter("isVertical").AsInteger())
                {
                    case 1:
                        ValidationElementsForCutting(instance,
                            GetIntersects<Wall>(new Outline(bb.Min, bb.Max)));
                        break;
                    case 0:
                        ValidationElementsForCutting(instance,
                            GetIntersects<Floor>(new Outline(bb.Min, bb.Max)));
                        break;
                    default: break;
                }
            }
        }

        /// <summary>
        /// Вырезание геометрии по пересечениям с Bounding boxes.
        /// </summary>
        private void CutBoundingBoxes(List<FamilyInstance> familyInstances, Dictionary<string, Category> selectedCategores)
        {
            BoundingBoxXYZ bb;
            foreach (KeyValuePair<string, Category> category in selectedCategores)
            {
                foreach (FamilyInstance instance in familyInstances)
                {
                    bb = instance.get_BoundingBox(Doc.ActiveView);

                    ValidationElementsForCutting(instance,
                        GetIntersects(category.Value, new Outline(bb.Min, bb.Max)));
                }
            }
        }

        /// <summary>
        /// Получение пересечений.
        /// </summary>
        private IList<Element> GetIntersects(Category catategory, Outline myOutLn)
        {
            return new FilteredElementCollector(Doc)
                .OfCategoryId(catategory.Id)
                .WherePasses(new BoundingBoxIntersectsFilter(myOutLn, false))
                .ToElements();
        }
        /// <summary>
        /// Получение пересечений.
        /// </summary>
        private IList<Element> GetIntersects<T>(Outline myOutLn)
        {
            return new FilteredElementCollector(Doc)
                .OfClass(typeof(T))
                .WherePasses(new BoundingBoxIntersectsFilter(myOutLn, false))
                .ToElements();
        }

        /// <summary>
        /// Транзакция вырезания геометрии
        /// </summary>
        /// <param name="instance">Геометрия для вырезания.</param>
        /// <param name="intersects">Список элементов пересекающихся с instance.</param>
        private void ValidationElementsForCutting(FamilyInstance instance, IList<Element> intersects)
        {
            if (intersects.Count == 0) return;
            foreach (Element element in intersects)
            {
                if (!ValidDesignOptions(instance, element)) continue;
                if (InstanceVoidCutUtils.CanBeCutWithVoid(element))
                {
                    CutTransaction(instance, element);
                }
            }
        }

        private void ValidationElementsForCutting(FamilyInstance instance, Element element)
        {
            if (element == null) return;
            if (!ValidDesignOptions(instance, element)) return;
            if (InstanceVoidCutUtils.CanBeCutWithVoid(element))
            {
                CutTransaction(instance, element);
            }
        }

        private void CutTransaction(FamilyInstance instance, Element element)
        {
            using (Transaction t = new Transaction(Doc, "Cut transaction"))
            {
                t.SetFailureHandlingOptions(t.GetFailureHandlingOptions()
                        .SetFailuresPreprocessor(new WarningDiscard()));
                t.Start("Cut transaction");

                InstanceVoidCutUtils.AddInstanceVoidCut(Doc, element, instance);

                t.Commit();
            }
        }

        private bool ValidDesignOptions(FamilyInstance instance, Element element)
        {
            Parameter instanceDesignOption = instance.get_Parameter(BuiltInParameter.DESIGN_OPTION_ID);
            Parameter elementDesignOption = element.get_Parameter(BuiltInParameter.DESIGN_OPTION_ID);

            if (elementDesignOption == null || instanceDesignOption == null) return false;

            return instanceDesignOption.AsElementId() == elementDesignOption.AsElementId();
        }
    }
}
