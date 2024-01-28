using Autodesk.Revit.DB;
using CutOpeningsPlugin.Inserting.Data.Enums;
using CutOpeningsPlugin.Inserting.Data.Interfaces;

namespace CutOpeningsPlugin.Inserting.Data.OpeningsRechtangular
{
    /// <summary>
    /// 231_Рен_Узел ввода СС без вырезания арматуры (ОбщМод_Грань)
    /// </summary>
    internal class GmEdg_231SsInputUnitSansCuttingReinforcement : IOpeningRechtangular
    {
        #region Свойства

        public ValidOpenings ValidOpenings { get; } = ValidOpenings.SsInputUnitSansCuttingReinforcement_GenericModelEdge_231;

        /// <summary>
        /// Название семейства.
        /// </summary>
        public string Name { get; } = "231_Рен_Узел ввода СС без вырезания арматуры (ОбщМод_Грань)";

        /// <summary>
        /// Название параметра, отвечающего за высоту экземпляра.
        /// </summary>
        public string Heigh { get; } = "РасчетДлинЛист";

        /// <summary>
        /// Является ли параметр параметром экземпляра.
        /// </summary>
        public bool IsInstanceHeigh { get; } = true;

        /// <summary>
        /// Название параметра, отвечающего за ширину экземпляра.
        /// </summary>
        public string Width { get; } = "РасчетШирЛист";

        /// <summary>
        /// Является ли параметр параметром экземпляра.
        /// </summary>
        public bool IsInstanceWidth { get; } = true;

        /// <summary>
        /// Название параметра, отвечающего за толщину основы экземпляра.
        /// </summary>
        public string Thickness { get; } = "Рзм.ТолщинаСтены";

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
            if ((element as FamilyInstance).Location == null)
            {
                BoundingBoxXYZ bb = element.get_BoundingBox(doc.ActiveView);
                newInstanceLocation += (bb.Max + bb.Min) / 2;
            }
            else
            {
                newInstanceLocation += (element.Location as LocationPoint).Point;
            }

            return newInstanceLocation -= new XYZ(0, 0, param.AsDouble() / 2);
        }
    }
}

