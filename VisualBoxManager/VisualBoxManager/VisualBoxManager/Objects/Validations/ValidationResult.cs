using System;
using System.Collections.Generic;
using System.Text;

namespace VisualBoxManager.Objects.Validations
{
    public class ValidationResult
    {
        public ValidationResult(bool error)
        {
            Error = error;
            Message = "";
        }
        public ValidationResult(bool error, string message)
        {
            Error = error;
            Message = message;
        }

        public string Message { private set; get; }

        public bool Error { private set; get; }

    }
}
