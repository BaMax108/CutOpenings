using Autodesk.Revit.DB;
using CutOpeningsPlugin.Inserting.Data.Enums;

namespace CutOpeningsPlugin.Inserting.Data.Interfaces
{
    /// <summary></summary>
    public interface IOpening
    {
        /// <summary>
        /// Название семейства.
        /// </summary>
        string Name { get; }

        /// <summary></summary>
        ValidOpenings ValidOpenings { get; }

        /// <summary>
        /// Геометрия сечения.
        /// </summary>
        ShapeType ShapeType { get; }

        /// <summary>
        /// Определение точки вставки.
        /// </summary>
        XYZ SetLocationPoint(Document doc, XYZ basePoint, Element element, XYZ linkPosition);
    }
}
