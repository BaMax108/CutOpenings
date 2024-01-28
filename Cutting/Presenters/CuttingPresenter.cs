using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CutOpeningsPlugin.Cutting.Data;
using CutOpeningsPlugin.Cutting.Interfaces;
using CutOpeningsPlugin.Cutting.Views;
using CutOpeningsPlugin.Models;
using CutOpeningsPlugin.Other;
using System.Collections.Generic;

namespace CutOpeningsPlugin.Cutting.Presenters
{
    /// <summary></summary>
    public class CuttingPresenter : IPresenterCut
    {
        private List<ElementId> Selection { get; set; }

        private Dictionary<string, Category> SelectionCategories { get; set; }

        private readonly RepositoryErrors Errors;

        /// <summary></summary>
        public CuttingPresenter() => Errors = new RepositoryErrors();

        /// <summary></summary>
        public void Start(UIDocument uIDoc, Document doc)
        {
            if (Errors.IsNull(uIDoc) || Errors.IsNull(doc)) return;

            WindowCutSettings window = new WindowCutSettings(uIDoc, doc);
            window.ShowDialog();
            Selection = window.SelectionElementIds;
            SelectionCategories = window.SelectionCategories;
            window.Close();
        }

        /// <summary></summary>
        public void StartCutting(Document doc)
        {
            if (Errors.IsNull(doc)) return;

            TimeChecker tc = new TimeChecker();
            tc.Start();

            if (SelectionCategories != null)
                new ModelCut(doc).Cut(SelectionCategories);
            else if (Selection != null)
                new ModelCut(doc).Cut(Selection);

            tc.Stop();
        }
    }
}
