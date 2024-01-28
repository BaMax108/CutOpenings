using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace CutOpeningsPlugin.Cutting.Interfaces
{
    /// <summary></summary>
    public interface IPresenterCut
    {
        /// <summary></summary>
        void Start(UIDocument uIDoc, Document doc);

        /// <summary></summary>
        void StartCutting(Document doc);
    }
}
