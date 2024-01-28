namespace CutOpeningsPlugin.Inserting.Data.Enums
{
    /// <summary></summary>
    public enum ValidOpenings
    {
        /*Прямоугольное сечение.*/
        /// <summary>
        /// 231_Армированное отверстие без вырезания арматуры (ОбщМод_Грань)
        /// </summary>
        ReinforcedOpeningSansCuttingReinforcement_GenericModelEdge_231,
        /// <summary>
        /// 231_Кассета из гильз (Окно_Стена)
        /// </summary>
        ShellCaseCassette_WindowWall_231,
        /// <summary>
        /// 231_Окно с вариантами армирования (Окно_Стена)
        /// </summary>
        WindowWithReinforcementOptions_WindowWall_231,
        /// <summary>
        /// 231_Отверстие прямоуг без вырезания арм (ОбщМод_Грань)
        /// </summary>
        RechtangularOpeningSansCuttingReinforcement_GenericodelEdge_231,
        /// <summary>
        /// 231_Отверстие прямоуг без основы (ОбщМод_Ур)
        /// </summary>
        RechtangularOpeningSansHost_GenericModelLevel_231,
        /// <summary>
        /// 231_Отверстие прямоуг с армированием (Окно_Стена)
        /// </summary>
        RechtangularOpeningWithReinforcement_WindowWall_231,
        /// <summary>
        /// 231_Отверстие прямоугольное (Окно_Стена)
        /// </summary>
        RectangularOpening_WindowWall_231,
        /// <summary>
        /// 231_Проем прямоуг (Окно_Стена)
        /// </summary>
        RectAperture_WindowWall_231,
        /// <summary>
        /// 231_Проем прямоугольный (Окно_Стена)
        /// </summary>
        RectangularAperture_WindowWall_231,
        /// <summary>
        /// 232_Проем прямоугольный (Окно_Стена)
        /// </summary>
        RectangularAperture_WindowWall_232,
        /// <summary>
        /// 232_Проем прямоугольный (Окно_Перекр)
        /// </summary>
        RectangularAperture_WindowFloor_232,
        /// <summary>
        /// 231_Отверстие_прямоуг_без_основы_УК_ОбщМод_Ур
        /// </summary>
        RechtangularOpeningSansHost_UK_GenericModelLevel_231,
        /// <summary>
        /// 231_Проем с армированием (Окно_Стена)
        /// </summary>
        ApertureWithReinforcement_WindowFloor_231,
        /// <summary>
        /// 232_Армированное отверстие без вырезания арматуры (ОбщМод_Грань)
        /// </summary>
        ReinforcedOpeningSansCuttingReinforcement_GenericModelEdge_232,
        /// <summary>
        /// 232_Отверстие в плите без вырезания арм (ОбщМод_Грань)
        /// </summary>
        OpeningInFloorSansCuttingReinforcement_GenericModelEdge_232,
        /// <summary>
        /// 232_Отверстие в плите прямоугольное (Окно_Плита)
        /// </summary>
        RechtangularOpeningIfFloor_WindowFloor_232,
        /// <summary>
        /// 232_Приямок ниша (Фунд_Плита)
        /// </summary>
        PitAlcove_FoundationFloor_232,
        /// <summary>
        /// 232_Проем временный (Окно_Плита)
        /// </summary>
        TemporaryOpening_WindowFloor_232,
        /// <summary>
        /// 232_Проем прямоуг с армированием (Окно_Плита)
        /// </summary>
        RechtangularOpeningWithReinforcement_WindowFloor_232,
        /// <summary>
        /// 231_Рен_Узел ввода СС без вырезания арматуры (ОбщМод_Грань)
        /// </summary>
        SsInputUnitSansCuttingReinforcement_GenericModelEdge_231,

        /*Круглое сечение.*/
        /// <summary>
        /// 231_Отверстие круглое с гильзой и уплотн- кольцом (Окно_Стена)
        /// </summary>
        CircleOpeningWithShellAndSealingRing_WindowWall_231,
        /// <summary>
        /// 231_Отверстие круглое без вырезания арм (ОбщМод_Грань)
        /// </summary>
        CircleOpeningSansCuttingReinforcement_GenericModelEdge_231,
        /// <summary>
        /// 231_Отверстие круглое (Окно_Стена)
        /// </summary>
        CircleOpening_WindowWall_231,
        /// <summary>
        /// 231_Сальник набивной ТМ (ОбщМод_Стена)
        /// </summary>
        StuffingBoxTm_GenericModelWall_231,
        /// <summary>
        /// 231_Сальник набивной ТМ без вырезания арм (ОбщМод_Стена)
        /// </summary>
        StuffingBoxTmSansCuttingReinforcement_GenericModelWall_231,
        /// <summary>
        /// 232_Круглое отверстие в плите без вырезания арм (ОбщМод_Грань)
        /// </summary>
        CircleOpeningInFloorSansCuttingReinforcement_GenericModelEdge_232,
        /// <summary>
        /// 232_Отверстие круглое (Окно_Перекр)
        /// </summary>
        CircleOpening_WindowFloor_232,
        /// <summary>
        /// 231_Узел ввода ОВ и ВК без вырезания арматуры (ОбщМод_Грань)
        /// </summary>
        OvVkInputUnitSansCuttingReinforcement_GenericModelEdge_231,
        /// <summary>
        /// 231_Узел ввода гидрогильза (ОбщМод_Грань)
        /// </summary>
        InputUnitHydroShell_GenericModelEdge_231,
        /// <summary>
        /// 231_Отверстие круглое с гильзой и уплотн. кольцом (Окно_Стена)
        /// </summary>
        CircleOpeningWithBushsingAndSealingRing_WindowWall_231
    }
}
