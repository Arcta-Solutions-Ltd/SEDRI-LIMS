using System.Reflection;

namespace arc.common.Utils
{
    public class CopyProperties : ICopyProperties
    {
        private const BindingFlags InstancePublic = BindingFlags.Public | BindingFlags.Instance;

        public void CopyAll<S, T>(S source, T target)
        {
            if (source == null || target == null)
                return;

            var sourceType = typeof(S);
            var targetType = typeof(T);

            foreach (var sourceProperty in sourceType.GetProperties(InstancePublic))
            {
                if (sourceProperty.GetIndexParameters().Length > 0)
                    continue;

                var targetProperty = targetType.GetProperty(sourceProperty.Name, InstancePublic);
                if (targetProperty == null || !targetProperty.CanWrite)
                    continue;

                var value = sourceProperty.GetValue(source, null);
                if (value != null && !targetProperty.PropertyType.IsAssignableFrom(value.GetType()))
                    continue;

                targetProperty.SetValue(target, value, null);
            }

            foreach (var sourceField in sourceType.GetFields(InstancePublic))
            {
                var targetField = targetType.GetField(sourceField.Name, InstancePublic);
                if (targetField == null)
                    continue;

                var value = sourceField.GetValue(source);
                if (value != null && !targetField.FieldType.IsAssignableFrom(value.GetType()))
                    continue;

                targetField.SetValue(target, value);
            }
        }
    }
}
