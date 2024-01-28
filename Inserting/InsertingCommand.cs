using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.Attributes;
using System;
using CutOpeningsPlugin.Inserting.Presenters;

namespace CutOpeningsPlugin
{
    /// <summary></summary>
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class InsertingCommand : IExternalCommand
    {
        /// <summary>
        /// Overload this method to implement and external command within Revit.
        /// </summary>
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            InsertingPresenter presenter = new InsertingPresenter(
                new UIApplication(commandData.Application.Application).ActiveUIDocument.Document);

            presenter.Run();

            GC.Collect();
            GC.WaitForPendingFinalizers();
            return Result.Succeeded;
        }
    }
}
