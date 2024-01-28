using Autodesk.Revit.UI;
using System.Diagnostics;

namespace CutOpeningsPlugin.Other
{
    /// <summary>
    /// Реализация класса Stopwatch.
    /// </summary>
    public class TimeChecker
    {
        private Stopwatch Timer { get; set; }

        /// <summary></summary>
        public TimeChecker() => Timer = new Stopwatch();

        /// <summary>
        /// Запускает или возобновляет измерение затраченного времени для интервала.
        /// </summary>
        public void Start()
        {
            Timer.Start();
        }

        /// <summary>
        /// Останавливает измерение затраченного времени для интервала.
        /// </summary>
        public void Stop()
        {
            if (Timer.IsRunning)
            {
                Timer.Stop();
                TaskDialog.Show("Отчет", $"Время выполнения: {Timer.Elapsed.TotalSeconds}");
            }
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса System.Diagnostics.Stopwatch, задает свойство
        /// затраченного времени равным нулю и запускает измерение затраченного времени.
        /// </summary>
        public void StartNew()
        {
            Timer = new Stopwatch();
            Timer.Start();
        }
    }
}

