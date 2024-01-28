using Autodesk.Revit.DB;
using CutOpeningsPlugin.Inserting.Presenters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CutOpeningsPlugin.Inserting.Models
{
    /// <summary></summary>
    public class RemoveUnnecessaryInstances
    {
        readonly private InsertingPresenter Presenter;
        private List<FamilyInstance> NewOpenings { get; set; }
        private List<ElementId> NewOpeningsIds { get; set; }

        /// <summary></summary>
        public RemoveUnnecessaryInstances(InsertingPresenter _presenter) => Presenter = _presenter;

        /// <summary></summary>
        public void Remove()
        {
            List<ElementId> intersections = new List<ElementId>();

            NewOpenings = Presenter.GetNewOpenings();
            NewOpeningsIds = NewOpenings.Select(x => x.Id).ToList();

            if (NewOpenings == null) return;
            foreach (var openings in NewOpenings)
            {
                if (IntersectsByCategory(BuiltInCategory.OST_Windows, openings).Count > 0 ||
                    IntersectsByCategory(BuiltInCategory.OST_Doors, openings).Count > 0)
                    intersections.Add(openings.Id);
            }

            RemoveTransaction(intersections);
            RemoveTransaction(IntersectsBetweenOpenings());
        }

        private ICollection<ElementId> IntersectsByCategory(BuiltInCategory bic, FamilyInstance fInstance)
        {
            BoundingBoxXYZ bb = fInstance.get_BoundingBox(Presenter.Doc.ActiveView);

            return new FilteredElementCollector(Presenter.Doc)
                .OfCategory(bic)
                .WhereElementIsNotElementType()
                .WherePasses(new BoundingBoxIntersectsFilter(new Outline(bb.Min, bb.Max), false))
                .ToElementIds();
        }

        private List<ElementId> IntersectsBetweenOpenings()
        {
            IEnumerable<Element> intersects;
            List<ElementId> result = new List<ElementId>();

            ElementId[] allOpenings =  new FilteredElementCollector(Presenter.Doc)
                .OfCategory(BuiltInCategory.OST_TelephoneDevices)
                .WhereElementIsNotElementType()
                .Cast<FamilyInstance>()
                .Where(x => x.Symbol.FamilyName == Presenter.GetOpeningsName())
                .Select(x=>x.Id)
                .ToArray();
            BoundingBoxXYZ bb;

            foreach (var newOp in NewOpeningsIds)
            {
                if (!allOpenings.Contains(newOp)) continue;
                bb = Presenter.Doc.GetElement(newOp).get_BoundingBox(Presenter.Doc.ActiveView);
                intersects = new FilteredElementCollector(Presenter.Doc)
                    .WherePasses(new BoundingBoxIntersectsFilter(new Outline(bb.Min, bb.Max)))
                    .OfCategory(BuiltInCategory.OST_TelephoneDevices)
                    .Where(x => (x as FamilyInstance).Symbol.FamilyName == Presenter.GetOpeningsName() & x.Id != newOp);

                if (intersects.Count() > 0)
                    result.Add(newOp);
            }

            return result;
        }

        private void RemoveTransaction(List<ElementId> ids)
        {
            foreach (var id in ids)
            {
                using (Transaction t = new Transaction(Presenter.Doc, "Remove transaction"))
                {
                    t.Start("Remove transaction");
                    try
                    {
                        Presenter.Doc.Delete(id);
                        NewOpeningsIds.Remove(id);
                    }
                    catch (Autodesk.Revit.Exceptions.InvalidObjectException e) 
                    { 
                        Debug.WriteLine(e.Message); 
                    }
                    catch (Exception e) 
                    { 
                        Debug.WriteLine(e); 
                    }
                    t.Commit();
                    
                }
            }
        }
    }
}
