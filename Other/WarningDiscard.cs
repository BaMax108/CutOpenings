using Autodesk.Revit.DB;
using System.Collections.Generic;

namespace CutOpeningsPlugin.Other
{
    /// <summary>
    /// Подавление ошибок.
    /// </summary>
    public class WarningDiscard : IFailuresPreprocessor
    {
        /// <summary></summary>
        public FailureProcessingResult PreprocessFailures(FailuresAccessor a)
        {
            IList<FailureMessageAccessor> failures = a.GetFailureMessages();

            foreach (FailureMessageAccessor f in failures)
            {
                FailureSeverity failureSeverity = a.GetSeverity();

                if (failureSeverity == FailureSeverity.Warning)
                {
                    a.DeleteWarning(f);
                }
                else
                {
                    return FailureProcessingResult.ProceedWithRollBack;
                }
            }
            return FailureProcessingResult.Continue;
        }
    }
}
