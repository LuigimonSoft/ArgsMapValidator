using System.Collections.Generic;

namespace ArgsMapValidator.Validation.ErrorMessages
{
    public static class ErrorMessageStore
    {
        public static Dictionary<int, string> Messages = new Dictionary<int, string>
        {
            
        };

        public static string GetMessage(int errorCode)
        {
            return Messages.ContainsKey(errorCode) ? Messages[errorCode] : $"Error code {errorCode} not found.";
        }
    }
}
