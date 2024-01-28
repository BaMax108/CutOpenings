using Autodesk.Revit.DB;
using CutOpeningsPlugin.Inserting.Data.Enums;
using CutOpeningsPlugin.Inserting.Data.Interfaces;

namespace CutOpeningsPlugin.Inserting.Data.OpeningsRechtangular
{
    /// <summary>
    /// 231_Отверстие_прямоуг_без_основы_УК_ОбщМод_Ур
    /// </summary>
    internal class GmLvl_231RechtangularOpeningSansHostUk : IOpeningRechtangular
    {
        #region Свойства

        public ValidOpenings ValidOpenings { get; } = ValidOpenings.RechtangularOpeningSansHost_UK_GenericModelLevel_231;

        /// <summary>
        /// Название семейства.
        /// </summary>
        public string Name { get; } = "231_Отверстие_прямоуг_без_основы_УК_ОбщМод_Ур";

        /// <summary>
        /// Название параметра, отвечающего за высоту экземпляра.
        /// </summary>
        public string Heigh { get; } = "Рзм.Высота";

        /// <summary>
        /// Является ли параметр параметром экземпляра.
        /// </summary>
        public bool IsInstanceHeigh { get; } = true;

        /// <summary>
        /// Название параметра, отвечающего за ширину экземпляра.
        /// </summary>
        public string Width { get; } = "Рзм.Ширина";

        /// <summary>
        /// Является ли параметр параметром экземпляра.
        /// </summary>
        public bool IsInstanceWidth { get; } = true;

        /// <summary>
        /// Название параметра, отвечающего за толщину основы экземпляра.
        /// </summary>
        public string Thickness { get; } = "Рзм.Глубина";

        /// <summary>
        /// Является ли параметр параметром экземпляра.
        /// </summary>
        public bool IsInstanceThickness { get; } = true;

        /// <summary>
        /// Геометрия сечения.
        /// </summary>
        public ShapeType ShapeType { get; } = ShapeType.Rechtangle;

        #endregion

        /// <summary>
        /// Вычисление точки вставки для нового экземпляра семейства.
        /// </summary>
        public XYZ SetLocationPoint(Document doc, XYZ basePoint, Element element, XYZ linkPosition)
        {
            Parameter param = element.LookupParameter(this.Heigh);
            if (param == null) return default;
            
            BoundingBoxXYZ bb = element.get_BoundingBox(doc.ActiveView);
            XYZ newInstanceLocation = (bb.Max + bb.Min) / 2 + basePoint + linkPosition -
                new XYZ(0, 0, param.AsDouble() / 2);

            return newInstanceLocation;
        }
    }
}
