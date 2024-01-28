using Autodesk.Revit.DB;
using CutOpeningsPlugin.Inserting.Data.Enums;
using CutOpeningsPlugin.Inserting.Data.Interfaces;

namespace CutOpeningsPlugin.Inserting.Data.OpeningsRechtangular
{
    /// <summary>
    /// 232_Отверстие в плите без вырезания арм (ОбщМод_Грань)
    /// </summary>
    internal class GmEdg_232OpeningInFloorSansCuttingReinforcement : IOpeningRechtangular
    {
        #region Свойства

        public ValidOpenings ValidOpenings { get; } = ValidOpenings.OpeningInFloorSansCuttingReinforcement_GenericModelEdge_232;

        /// <summary>
        /// Название семейства.
        /// </summary>
        public string Name { get; } = "232_Отверстие в плите без вырезания арм (ОбщМод_Грань)";

        /// <summary>
        /// Название параметра, отвечающего за высоту экземпляра.
        /// </summary>
        public string Heigh { get; } = "Длина";

        /// <summary>
        /// Является ли параметр параметром экземпляра.
        /// </summary>
        public bool IsInstanceHeigh { get; } = true;

        /// <summary>
        /// Название параметра, отвечающего за ширину экземпляра.
        /// </summary>
        public string Width { get; } = "Ширина";

        /// <summary>
        /// Является ли параметр параметром экземпляра.
        /// </summary>
        public bool IsInstanceWidth { get; } = true;

        /// <summary>
        /// Название параметра, отвечающего за толщину основы экземпляра.
        /// </summary>
        public string Thickness { get; } = "Рзм.ТолщинаОсновы";

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

            XYZ newInstanceLocation = basePoint + linkPosition;
            if (element is FamilyInstance instance)
            { 
                if (instance.Location == null)
                {
                    BoundingBoxXYZ bb = element.get_BoundingBox(doc.ActiveView);
                    newInstanceLocation += (bb.Max + bb.Min) / 2;
                }
                else
                {
                    newInstanceLocation += (instance.Location as LocationPoint).Point;
                }

                if (instance.Host is Wall)
                    newInstanceLocation -= new XYZ(0, 0, param.AsDouble() / 2);
            }

            return newInstanceLocation;
        }
    }
}
