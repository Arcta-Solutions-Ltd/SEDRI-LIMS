namespace arc.common.Utils
{
    using arc.common.ExtensionMethods;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Contains set of static methods to help with object manipulation.
    /// </summary>
    public static class ObjectHelper
    {
        public static T PopulateFromDelimitedString<T>(string delimitedString, string delimiter) where T : new()
        {
            T obj = new T();
            string[] elements = delimitedString.Split(new string[] { delimiter }, StringSplitOptions.None);
            elements = elements.RemoveFirstElement();

            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            if (elements.Length != properties.Length)
            {
                throw new ArgumentException("The input string does not match the number of properties in the object.");
            }

            for (int i = 0; i < properties.Length; i++)
            {
                PropertyInfo property = properties[i];
                Type propertyType = property.PropertyType;

                // Convert the string element to the property's type
                object value = Convert.ChangeType(elements[i], propertyType);

                property.SetValue(obj, value);
            }

            return obj;
        }

        /// <summary>
        /// Populates an object with the contents of a delimited string.
        /// <param name="modelToCopy">Object defining which model to copy the data into</param>
        /// <param name="delimitedString">Delimited string to load into the object</param>
        /// <param name="delim">Delimiter to separate the items in the string</param>
        /// <returns>Object with the delimited values loaded sequentially into the object properties</returns>
        public static object GetPopulatedObjectFromDelimitedString(KeyValuePair<Type, string> modelToCopy, string delimitedString, string delimiter)
        {
            MethodInfo method = typeof(ObjectHelper).GetMethod("PopulateFromDelimitedString").MakeGenericMethod(modelToCopy.Key);

            object result = method.Invoke(null, new object[] { delimitedString, delimiter });

            return result;
        }
    }
}
