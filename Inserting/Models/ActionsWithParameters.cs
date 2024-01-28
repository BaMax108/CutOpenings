using Autodesk.Revit.DB;
using CutOpeningsPlugin.Inserting.Data;
using CutOpeningsPlugin.Inserting.Data.Enums;
using CutOpeningsPlugin.Inserting.Data.Interfaces;
using CutOpeningsPlugin.Inserting.Types;
using CutOpeningsPlugin.Other;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CutOpeningsPlugin.Inserting.Models
{
    /// <summary></summary>
    public class ActionsWithParameters
    {
        private enum EnumOffset
        {
            WallOffsetA = 0,
            WallOffsetB = 1,
            FloorOffsetA = 2,
            FloorOffsetB = 3
        }
        
        private enum SurfaceType
        {
            Horizontal,
            Vertical
        }

        readonly private Document Doc;

        /// <summary></summary>
        public List<string> ListOffsets { get; private set; }

        /// <summary></summary>
        public ActionsWithParameters(Document _doc, List<string> _offsets)
        {
            Doc = _doc;
            ListOffsets = _offsets;
        }

        #region Изменение параметров и угла поворота
        private void ChangingPropertiesTransaction(FamilyInstance newInstance, Element element, IOpening familyDataParams, SurfaceType surface)
        {
            using (Transaction t = new Transaction(Doc, "ChangingProperties"))
            {
                t.SetFailureHandlingOptions(t.GetFailureHandlingOptions()
                            .SetFailuresPreprocessor(new WarningDiscard()));
                t.Start("ChangingProperties");

                EditSurfaceAndShape(newInstance, surface, familyDataParams.ShapeType);

                switch (familyDataParams.ShapeType)
                {
                    case ShapeType.Rechtangle:
                        SetRectangularParams(newInstance, familyDataParams as IOpeningRechtangular, element); break;
                    case ShapeType.Circle:
                        SetCircleParams(newInstance, familyDataParams as IOpeningCircular, element); break;
                    default: break;
                }

                SetInstanceOffset(newInstance,
                    ListOffsets[(int)EnumOffset.WallOffsetA],
                    ListOffsets[(int)EnumOffset.WallOffsetB]);

                SetDiscriptions(newInstance, (element as FamilyInstance).Symbol.FamilyName);

                t.Commit();
            }
        }

        private void RotatingTransaction(FamilyInstance newInstance, Element element, Transform transform)
        {
            using (Transaction t = new Transaction(Doc, "Rotating"))
            {
                t.SetFailureHandlingOptions(t.GetFailureHandlingOptions()
                            .SetFailuresPreprocessor(new WarningDiscard()));
                t.Start("Rotating");
                InstanceRotating(newInstance, SetAngle(element), transform,element);
                t.Commit();
            }
        }
        private void RotatingTransaction(FamilyInstance newInstance, Element element)
        {
            using (Transaction t = new Transaction(Doc, "Rotating"))
            {
                t.SetFailureHandlingOptions(t.GetFailureHandlingOptions()
                            .SetFailuresPreprocessor(new WarningDiscard()));
                t.Start("Rotating");
                InstanceRotating(newInstance, SetAngle(element));
                t.Commit();
            }
        }

        /// <summary>
        /// Изменение угла поворота и заполнение параметров отверстий с прямоугольным сечением в стенах.
        /// </summary>
        public void EditWallRectangleOpenings(FamilyInstance newInstance, Element element, IOpening fParams, Transform transform)
        {
            ChangingPropertiesTransaction(newInstance, element, fParams, SurfaceType.Vertical);

            RotatingTransaction(newInstance, element, transform);
        }


        /// <summary>
        /// Изменение угла поворота и заполнение параметров отверстий с круглым сечением в стенах.
        /// </summary>
        public void EditWallCircleOpenings(FamilyInstance newInstance, Element element, IOpening fParams)
        {
            ChangingPropertiesTransaction(newInstance, element, fParams, SurfaceType.Vertical);

            RotatingTransaction(newInstance, element);
        }

        /// <summary>
        /// Изменение угла поворота и заполнение параметров отверстий с прямоугольным сечением в плитах.
        /// </summary>
        public void EditFloorRectangleOpenings(FamilyInstance newInstance, Element element, IOpening fParams)
        {
            var instance = element as FamilyInstance;

            if (instance.Symbol.FamilyName == "231_Отверстие прямоуг без основы (ОбщМод_Ур)" || 
                instance.Symbol.FamilyName == "231_Отверстие_прямоуг_без_основы_УК_ОбщМод_Ур")
            {
                ChangingPropertiesTransaction(newInstance, element, fParams, SurfaceType.Vertical);
            }
            else
            {
                ChangingPropertiesTransaction(newInstance, element, fParams, SurfaceType.Horizontal);
            }

            RotatingTransaction(newInstance, element);
        }

        /// <summary>
        /// Изменение угла поворота и заполнение параметров отверстий с круглым сечением в плитах.
        /// </summary>
        public void EditFloorCircleOpenings(FamilyInstance newInstance, Element element, IOpening fParams)
        {
            ChangingPropertiesTransaction(newInstance, element, fParams, SurfaceType.Horizontal);
            RotatingTransaction(newInstance, element);
        }

        /// <summary>
        /// Изменение параметров отверстий с прямоугольным сечением.
        /// </summary>
        private void SetRectangularParams(FamilyInstance newInstance, IOpeningRechtangular fParams, Element element)
        {
            Parameter height = GetParameterAtGuid(newInstance, "da753fe3-ecfa-465b-9a2c-02f55d0c2ff1");
            Parameter width = GetParameterAtGuid(newInstance, "8f2e4f93-9472-4941-a65d-0ac468fd6a5d");
            Parameter thickness = GetParameterAtGuid(newInstance, "398ea174-e0ef-4619-8db9-0b2c598d5206");

            Parameter h = null;
            Parameter w = null;
            Parameter thick = null;

            if (fParams != null)
            {
                FamilySymbol symbol = (element as FamilyInstance).Symbol;

                if (fParams.IsInstanceHeigh)
                    h = GetParameterAtName(element, fParams.Heigh);
                else
                    h = GetParameterAtName(symbol, fParams.Heigh);

                if (fParams.IsInstanceWidth)
                    w = GetParameterAtName(element, fParams.Width);
                else
                    w = GetParameterAtName(symbol, fParams.Width);

                if (fParams.IsInstanceThickness)
                    thick = GetParameterAtName(element, fParams.Thickness);
                else
                    thick = GetParameterAtName(symbol, fParams.Thickness);

                if (w == null || h == null || thick == null) return;

                SetParams(height, h.AsValueString());
                SetParams(width, w.AsValueString());
                SetParams(thickness, thick.AsValueString());
            }
            else
            {
                FamilyParameters parameters = new FamilyParameters();

                foreach (string name in parameters.HeightList)
                {
                    if (IsExistParameter(element, name, out h))
                    {
                        SetParams(height, h.AsValueString());
                    }
                }

                foreach (string name in parameters.WidthList)
                {
                    if (IsExistParameter(element, name, out w))
                    {
                        SetParams(width, w.AsValueString());
                    }
                }

                foreach (string name in parameters.ThicknessList)
                {
                    if (IsExistParameter(element, name, out thick))
                    {
                        SetParams(thickness, thick.AsValueString());
                    }
                }

                if (w == null || h == null || thick == null) return;
            }
        }

        /// <summary>
        /// Изменение параметров отверстий с круглым сечением.
        /// </summary>
        private void SetCircleParams(FamilyInstance newInstance, IOpeningCircular fParams, Element element)
        {
            Parameter diameter = GetParameterAtGuid(newInstance, "9b679ab7-ea2e-49ce-90ab-0549d5aa36ff");
            Parameter thickness = GetParameterAtGuid(newInstance, "398ea174-e0ef-4619-8db9-0b2c598d5206");

            if (fParams != null)
            {
                var symbol = (element as FamilyInstance).Symbol;

                if (fParams.IsInstancDiameter)
                    SetParams(diameter, GetParameterAtName(element, fParams.Diameter).AsValueString());
                else
                    SetParams(diameter, GetParameterAtName(symbol, fParams.Diameter).AsValueString());

                if (fParams.IsInstanceThickness)
                    SetParams(thickness, GetParameterAtName(element, fParams.Thickness).AsValueString());
                else
                    SetParams(thickness, GetParameterAtName(symbol, fParams.Thickness).AsValueString());

            }
            else
            {
                FamilyParameters parameters = new FamilyParameters();
                Parameter diam = null;
                Parameter thick = null;

                foreach (string name in parameters.DiameterList)
                {
                    if (IsExistParameter(element, name, out diam))
                    {
                        SetParams(diameter, diam.AsValueString());
                    }
                }

                foreach (string name in parameters.ThicknessList)
                {
                    if (IsExistParameter(element, name, out thick))
                    {
                        SetParams(thickness, thick.AsValueString());
                    }
                }

                if (diam == null || thick == null) return;
            }
        }

        private bool IsExistParameter(Element element, string paramName, out Parameter param)
        {
            param = GetParameterAtName(element, paramName);
            return param != null;
        }

        /// <summary>
        /// Изменение угла поворота.
        /// </summary>
        private void InstanceRotating(FamilyInstance newInstance, double angle, Transform transform, Element element)
        {
            var fOr = (element as FamilyInstance).FacingOrientation;
            XYZ mid = (newInstance.get_BoundingBox(Doc.ActiveView).Max + newInstance.get_BoundingBox(Doc.ActiveView).Min) / 2;
            
            var qwe = transform.OfVector(fOr).AngleTo(XYZ.BasisX);
            Debug.WriteLine(qwe);
            Debug.WriteLine(transform.OfVector(fOr).AngleTo(XYZ.BasisY));

            Debug.WriteLine(angle);
            var hOr = (element as FamilyInstance).HandOrientation;
            Debug.WriteLine(hOr);

            var qwe2 = transform.OfVector(hOr).AngleTo(XYZ.BasisX);
            Debug.WriteLine(qwe2);
            Debug.WriteLine(transform.OfVector(hOr).AngleTo(XYZ.BasisY));

            newInstance.Location.Rotate(Line.CreateBound(mid, mid.Add(XYZ.BasisZ)), qwe2);
        }
        private void InstanceRotating(FamilyInstance newInstance, double angle)
        {
            XYZ mid = (newInstance.get_BoundingBox(Doc.ActiveView).Max + newInstance.get_BoundingBox(Doc.ActiveView).Min) / 2;
            var qwe = newInstance.FacingOrientation.AngleTo(XYZ.BasisX);
            newInstance.Location.Rotate(Line.CreateBound(mid, mid.Add(XYZ.BasisZ)), qwe);
        }

        /// <summary>
        /// Получение угла поворота элемента в зависимости от типа хоста.
        /// </summary>
        private double SetAngle(Element element)
        {
            var temp = (element as FamilyInstance).Location;
            double angle;
            if (temp is LocationPoint lPoint)
            {
                angle = lPoint.Rotation;
            }
            else if (temp is LocationCurve lCurve)
            { 
                angle = 0;
            }
            else
            {
                angle = 0;
            }
            angle += Math.PI / 2;

            Debug.WriteLine(angle);

            return angle - Math.PI / 2;
            //return (element as FamilyInstance).GetTransform().BasisX.AngleTo(XYZ.BasisX);
        }

        /// <summary>
        /// Изменение отступов.
        /// </summary>
        private void SetInstanceOffset(FamilyInstance newInstance, string a, string b)
        {
            newInstance.LookupParameter("OffsetA").SetValueString(a);
            newInstance.LookupParameter("OffsetB").SetValueString(b);
        }

        /// <summary>
        /// Заполнение параметра ADSK_Примечание
        /// </summary>
        private void SetDiscriptions(FamilyInstance newInstance, string value)
        {
            var p1 = newInstance.get_Parameter(new Guid("a85b7661-26b0-412f-979c-66af80b4b2c3"));
            p1.Set(value);
        }

        /// <summary>
        /// Изменение поверхности размещения и формы сечения отверстия.
        /// </summary>
        private void EditSurfaceAndShape(FamilyInstance newInstance, SurfaceType surface, ShapeType shape)
        {
            newInstance.LookupParameter("isVertical").Set((int)surface);
            newInstance.LookupParameter("isCircle").Set((int)shape);
        }

        #endregion

        #region Получение/Изменение параметров

        /// <summary>
        /// Получение параметра по имени
        /// </summary>
        private Parameter GetParameterAtName(Element newInstance, string str) => newInstance.LookupParameter(str);
        /// <summary>
        /// Получение параметра по имени
        /// </summary>
        private Parameter GetParameterAtName(FamilySymbol newInstance, string str) => newInstance.LookupParameter(str);

        /// <summary>
        /// Получение параметра по GUID
        /// </summary>
        private Parameter GetParameterAtGuid(FamilyInstance newInstance, string guid) => newInstance.get_Parameter(new Guid(guid));

        /// <summary>
        /// Изменение значения параметра
        /// </summary>
        private void SetParams(Parameter param, string str)
        {
            if (param == null) return;
            param.SetValueString(str);
        }

        #endregion
    }
}
