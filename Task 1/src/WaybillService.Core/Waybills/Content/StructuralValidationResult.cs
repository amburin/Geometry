using System.Collections.Generic;
using System.Linq;

namespace WaybillService.Core.Waybills.Content
{
    public abstract class StructuralValidationResult
    {
        public bool HasErrors => ErrorCount != 0;
        public int ErrorCount => EnumerateAllErrors().Count(x => x);

        public IEnumerable<bool> EnumerateAllErrors() =>
            EnumerateErrorFlags()
                .Concat(EnumerateInnerValidationResults().SelectMany(x => x.EnumerateAllErrors()));
        
        protected abstract IEnumerable<bool> EnumerateErrorFlags();
        protected abstract IEnumerable<StructuralValidationResult> EnumerateInnerValidationResults();
    }
}