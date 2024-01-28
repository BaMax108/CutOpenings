using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CutOpeningsPlugin.Cutting.Presenters;
using System;

namespace CutOpeningsPlugin
{
    /// <summary></summary>
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class CuttingCommand : IExternalCommand
    {
        /// <summary>
        /// Overload this method to implement and external command within Revit.
        /// </summary>
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uIApp = new UIApplication(commandData.Application.Application);
            UIDocument uIDoc = uIApp.ActiveUIDocument;
            Document doc = uIDoc.Document;

            CuttingPresenter presenter = new CuttingPresenter();
            presenter.Start(uIDoc, doc);
            presenter.StartCutting(doc);

            GC.Collect();
            GC.WaitForPendingFinalizers();

            return Result.Succeeded;
        }
    }
}
