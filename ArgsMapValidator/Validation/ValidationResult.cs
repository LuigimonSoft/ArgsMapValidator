using System.Collections.Generic;

namespace ArgsMapValidator.Validation
{
    public class ValidationResult
    {
        public bool IsValid { get; private set; } = true;
        public List<Error> Errors { get; } = new List<Error>();

        public void AddError(Error error)
        {
            IsValid = false;
            Errors.Add(error);
        }
    }

}

