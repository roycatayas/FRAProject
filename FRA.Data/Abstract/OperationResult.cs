using System.Collections.Generic;
using System.Linq;

namespace FRA.Data.Abstract
{
    public class OperationResult
    {
        private static readonly OperationResult _success = new OperationResult { Succeeded = true };
        private List<OperationResultError> _errors = new List<OperationResultError>();

        public bool Succeeded { get; protected set; }

        public IEnumerable<OperationResultError> Errors => _errors;
        public static OperationResult Success => _success;

        public static OperationResult Failed(params OperationResultError[] errors)
        {
            var result = new OperationResult { Succeeded = false };
            if (errors != null)
            {
                result._errors.AddRange(errors);
            }
            return result;
        }

        public override string ToString()
        {
            return Succeeded ?
                "Succeeded" :
                string.Format("{0} : {1}", "Failed", string.Join(",", Errors.Select(x => x.Code).ToList()));
        }
    }
}