using System;
using System.Reflection;
using ConsoleBase.Common.Attributes;

namespace ArgsMapValidator.Mapping
{
    public static class ObjectMapper
    {
        public static T MapFromParameters<T>(string[] parameters) where T : new()
        {
            var obj = new T();
            var properties = typeof(T).GetProperties();

            foreach (var prop in properties)
            {
                var columnAttr = prop.GetCustomAttribute<ColumnAttribute>();
                if (columnAttr != null)
                {
                    var index = columnAttr.Index;

                    if (index < parameters.Length)
                    {
                        var parameterValue = parameters[index];
                        try
                        {
                            var convertedValue = Convert.ChangeType(parameterValue, prop.PropertyType);
                            prop.SetValue(obj, convertedValue);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"Error converting value '{parameterValue}' for property '{prop.Name}': {ex.Message}");
                        }
                    }
                }
            }

            return obj;
        }
    }
}

