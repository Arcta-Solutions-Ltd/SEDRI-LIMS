using arc.common.Models;
using System;
using System.Collections.Generic;
using System.Linq;


namespace arc.common.ExtensionMethods
{
    /// <summary>
    /// Contains set of static extension methods to help with object manipulation.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Converts an object's properties to a delimited string, removing tab characters from each property value.
        /// </summary>
        /// <param name="obj">The object whose properties are to be converted.</param>
        /// <param name="delim">The delimiter to be used.</param>
        /// <returns>A string containing the object's properties separated by the specified delimiter, with tabs removed.</returns>
        public static string ToDelimitedString(this object obj, string delim)
        {
            var properties = obj.GetType().GetProperties();
            var values = properties.Select(prop => prop.GetValue(obj, null)?.ToString().RemoveTabs().RemoveEndOfLineCharacters() ?? string.Empty);
            return delim + string.Join(delim, values);
        }


        /// <summary>
        /// Maps properties with the same name and type from the current object to the target object.
        /// </summary>
        /// <param name="obj">Object containing the model to translate</param>
        /// <typeparam name="TTarget">The type of the target object.</typeparam>
        public static TTarget Map<TTarget>(this object obj) where TTarget : new()
        {
            var sourceProperties = obj.GetType().GetProperties();
            var targetProperties = typeof(TTarget).GetProperties();
            var returnTarget = new TTarget();

            foreach (var sourceProperty in sourceProperties)
            {
                foreach (var targetProperty in targetProperties)
                {
                    if (sourceProperty.Name == targetProperty.Name &&
                        sourceProperty.PropertyType == targetProperty.PropertyType)
                    {
                        var value = sourceProperty.GetValue(obj);
                        if (targetProperty.CanWrite)
                        {
                            targetProperty.SetValue(returnTarget, value);
                        }
                    }
                }
            }
            return returnTarget;
        }

        /// <summary>
        /// Checks if a property exists in the specified model.
        /// </summary>
        /// <param name="obj">Object containing the model to translate</param>
        /// <param name="propertyName">The name of the property to check.</param>
        /// <returns>True if the property exists; otherwise, false.</returns>
        public static bool HasProperty(this object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName) != null;
        }

        /// <summary>
        /// Retrieves the value of a specified property from an entity.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="entity">The entity from which to retrieve the property value.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The value of the specified property.</returns>
        /// <exception cref="ArgumentException">Thrown when the specified property is not found on the entity.</exception>
        public static object GetPropertyValue<T>(this T entity, string propertyName)
        {
            var propertyInfo = typeof(T).GetProperty(propertyName);
            if (propertyInfo == null)
            {
                throw new ArgumentException($"Property '{propertyName}' not found on type '{typeof(T)}'");
            }
            return propertyInfo.GetValue(entity);
        }

        public static List<KeyValueModel> GetQueryValues<T>(this T entity)
        {
            var queryValues = new List<KeyValueModel>();

            foreach (var prop in typeof(T).GetProperties())
            {
                var value = prop.GetValue(entity);
                if (value != null)
                {
                    queryValues.Add(new KeyValueModel
                    {
                        Key = prop.Name,
                        Value = value.ToString()
                    });
                }
            }

            return queryValues;
        }

    }
}





