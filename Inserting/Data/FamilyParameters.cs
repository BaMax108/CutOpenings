using System.Collections.Generic;

namespace CutOpeningsPlugin.Inserting.Data
{
    /// <summary></summary>
    public class FamilyParameters
    {
        /// <summary></summary>
        public readonly List<string> HeightList = new List<string>()
        {
            "Длина",
            "Рзм.Длина",
            "Высота",
            "Рзм.Высота",
            "ADSK_Отверстие_Высота",
            "ADSK_Размер_Высота",
            "CGN_Высота"
        };

        /// <summary></summary>
        public readonly List<string> WidthList = new List<string>()
        {
            "Ширина",
            "Рзм.Ширина",
            "ADSK_Отверстие_Ширина",
            "ADSK_Размер_Ширина",
            "CGN_Ширина"
        };

        /// <summary></summary>
        public readonly List<string> DiameterList = new List<string>()
        {
            "Рзм.Диаметр",
            "ADSK_Размер_Диаметр",
            "ADSK_Диаметр",
            "Размер_Диаметр",
            "Диаметр"
        };

        /// <summary></summary>
        public readonly List<string> ThicknessList = new List<string>()
        {
            "Рзм.ТолщинаОсновы",
            "ADSK_Размер_Толщина основы",
            "ADSK_Толщина основы",
            "Размер_Толщина основы"
        };
    }
}
