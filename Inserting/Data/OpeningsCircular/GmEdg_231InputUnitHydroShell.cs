using Autodesk.Revit.DB;
using CutOpeningsPlugin.Inserting.Data.Enums;
using CutOpeningsPlugin.Inserting.Data.Interfaces;

namespace CutOpeningsPlugin.Inserting.Data.OpeningsCircular
{
    /// <summary>
    /// 231_Узел ввода гидрогильза (ОбщМод_Грань)
    /// </summary>
    internal class GmEdg_231InputUnitHydroShell : IOpeningCircular
    {
        #region Свойства

        public ValidOpenings ValidOpenings { get; } = ValidOpenings.InputUnitHydroShell_GenericModelEdge_231;

        /// <summary>
        /// Название семейства.
        /// </summary>
        public string Name { get; } = "231_Узел ввода гидрогильза (ОбщМод_Грань)";

        /// <summary>
        /// Название параметра, отвечающего за толщину основы экземпляра.
        /// </summary>
        public string Thickness { get; } = "Рзм.ТолщинаОсновы";

        /// <summary>
        /// Является ли параметр параметром экземпляра.
        /// </summary>
        public bool IsInstanceThickness { get; } = true;

        /// <summary>
        /// Название параметра, отвечающего за диаметр экземпляра.
        /// </summary>
        public string Diameter { get; } = "d уплотн кольца";

        /// <summary>
        /// Является ли параметр параметром экземпляра.
        /// </summary>
        public bool IsInstancDiameter { get; } = false;

        /// <summary>
        /// Геометрия сечения.
        /// </summary>
        public ShapeType ShapeType { get; } = ShapeType.Circle;

        #endregion

        /// <summary>
        /// Вычисление точки вставки для нового экземпляра семейства.
        /// </summary>
        public XYZ SetLocationPoint(Document doc, XYZ basePoint, Element element, XYZ linkPosition)
        {
            BoundingBoxXYZ bb = element.get_BoundingBox(doc.ActiveView);
            Parameter param = (element as FamilyInstance).Symbol.LookupParameter(this.Diameter);
            
            return param == null ? default : (bb.Max + bb.Min) / 2 + basePoint + linkPosition - new XYZ(0, 0, param.AsDouble());
        }
    }
}
