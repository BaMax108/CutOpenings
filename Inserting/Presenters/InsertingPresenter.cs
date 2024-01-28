using Autodesk.Revit.DB;
using CutOpeningsPlugin.Inserting.Data;
using System.Collections.Generic;
using CutOpeningsPlugin.Inserting.Models;
using CutOpeningsPlugin.Inserting.Views;
using Document = Autodesk.Revit.DB.Document;
using CutOpeningsPlugin.Other;
using System.Diagnostics;

namespace CutOpeningsPlugin.Inserting.Presenters
{
    /// <summary></summary>
    public class InsertingPresenter
    {
        readonly private InsertingPresenter Presenter;
        
        private CreateAndTransform Creation;
        private RepositoryErrors Errors { get; }
        private InitialData InitData { get; }
        private ActionsWithGroups GroupActions { get; }
        private RemoveUnnecessaryInstances UnnecessaryInstances { get; }

        private string SelectedLink { get; set; }
        private List<string> ListOffsets { get; set; }
        private List<ElementId> GroupIds { get; set; }
        private List<Group> OpeningsGroups { get; set; }
        /// <summary></summary>
        public Document Doc { get; private set; }
        /// <summary></summary>
        public RevitLinkInstance CurrentLinkInstance { get; private set; }

        /// <summary></summary>
        public InsertingPresenter(Document _doc)
        {
            Presenter = this;
            Doc = _doc;
            Errors = new RepositoryErrors();
            InitData = new InitialData(Doc);
            GroupActions = new ActionsWithGroups(Doc);
            UnnecessaryInstances = new RemoveUnnecessaryInstances(Presenter);

            GroupIds = new List<ElementId>();
            OpeningsGroups = new List<Group>();
            Debug.WriteLine("");
        }

        /// <summary></summary>
        public List<FamilyInstance> GetNewOpenings() => InitData.NewOpenings;

        /// <summary></summary>
        public string GetOpeningsName() => InitData.OpeningsName;

        /// <summary></summary>
        public void AddOpenings(FamilyInstance fi) => InitData.NewOpenings.Add(fi);

        /// <summary></summary>
        public void Run()
        {
            if (Errors.IsValidView(Doc.ActiveView)) return;

            ShowUserInteface();
            if (SelectedLink == null) return;
            if (Errors.IsNull(InitData.CurrentFamily)) return;
            if (Errors.IsNull(InitData.LinkInstances)) return;

            TimeChecker tc = new TimeChecker();
            tc.Start();

            Creation = new CreateAndTransform(Presenter, ListOffsets, InitData.CurrentFamily);

            InsertOpenings();

            UnnecessaryInstances.Remove();

            tc.Stop();
        }

        private void ShowUserInteface()
        {
            UserWindow View = new UserWindow(InitData.Getlinks());
            View.ShowDialog();
            if (View.SelectedLink == null) return;
            
            SelectedLink = View.SelectedLink;
            ListOffsets = new List<string>
            {
                View.WallOffsetA,
                View.WallOffsetB,
                View.FloorOffsetA,
                View.FloorOffsetB
            };
            InitData.CurrentFamily = InitData.GetFamilyInstance(); 
            InitData.LinkInstances = InitData.GetLinkInstances(SelectedLink); 
            View.Close();
        }
        public RevitLinkInstance RvtLink { get; set; }
        private void InsertOpenings()
        {
            List<ElementId> subGroupIds;
            Document linkDoc;
            foreach (var link in InitData.LinkInstances)
            {
                RvtLink = link;
                CurrentLinkInstance = link;
                linkDoc = link.GetLinkDocument();
                if (linkDoc == null) continue;

                InitData.GetOpeningsFromLink(linkDoc);
                if (InitData.OpeningsFromLink.Count == 0) continue;

                subGroupIds = new List<ElementId>();
                Creation.InsertingOpenings(GroupIds, subGroupIds, link.GetTotalTransform().Origin, InitData.OpeningsFromLink);

                //var group = GroupActions.CreateGroup(subGroupIds);
                //if (group == null) continue;
                //OpeningsGroups.Add(group);

                //GroupActions.RotateUngroupDelete(group, link);
            }
        }
    }
}
