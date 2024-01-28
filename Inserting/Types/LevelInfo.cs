using Autodesk.Revit.DB;

namespace CutOpeningsPlugin.Inserting.Types
{
    /// <summary></summary>
    public class LevelInfo
    {
        /// <summary></summary>
        public LevelInfo(ElementId _id, string _name, double _elevation)
        { 
            ElementId = _id;
            Name = _name;
            Elevation = _elevation;
        }

        /// <summary></summary>
        public ElementId ElementId { get;}
        /// <summary></summary>
        public string Name { get;}
        /// <summary></summary>
        public double Elevation { get;}
    }
}
