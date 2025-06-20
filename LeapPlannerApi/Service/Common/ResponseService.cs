using System;
using System.Reflection;

namespace LeapPlannerApi.Service.Common
{
    public static class ResponseService
    {
        public static T UpdateSucess<T>()
        {
            var data = (T)Activator.CreateInstance(typeof(T), new object[] { });
            var updateData = new System.Collections.Generic.Dictionary<string, object>
                {
                    { "Error", false },
                    { "Status", "Sucess" },
                    { "ErrorMessage", null }
                };
            Type type = typeof(T);
            foreach (var prop in updateData)
            {
                PropertyInfo propertyInfo = type.GetProperty(prop.Key);

                if (propertyInfo != null && propertyInfo.CanWrite)
                {
                    try
                    {
                        object value = Convert.ChangeType(prop.Value, propertyInfo.PropertyType);
                        propertyInfo.SetValue(data, value, null);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error setting property {prop.Key}: {ex.Message}");
                    }
                }
            }
            return data;       
        }

        public static T UpdateErrorMessage<T>(string error)
        {
            var data = (T)Activator.CreateInstance(typeof(T), new object[] { });
            var updateData = new System.Collections.Generic.Dictionary<string, object>
                {
                    { "Error", true },
                    { "Status", "Failed" },
                    { "ErrorMessage", error }
                };
            Type type = typeof(T);
            foreach (var prop in updateData)
            {
                PropertyInfo propertyInfo = type.GetProperty(prop.Key);

                if (propertyInfo != null && propertyInfo.CanWrite)
                {
                    try
                    {
                        object value = Convert.ChangeType(prop.Value, propertyInfo.PropertyType);
                        propertyInfo.SetValue(data, value, null);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error setting property {prop.Key}: {ex.Message}");
                    }
                }
            }
            return data;
        }
    }

    public enum ErrorMessage
    {
        BadRequest,
        InternalServerError,
        InvalidProcess
    }
}
