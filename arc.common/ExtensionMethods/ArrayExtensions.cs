using System;

namespace arc.common.ExtensionMethods
{
    /// <summary>
    /// Contains set of static extension methods to help with array manipulation.
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// Removes the first element from an array.
        /// <typeparam name="T">The type of the object that makes up the array.</typeparam>
        /// <param name=array">Array to remove the element from</param>
        /// <returns>Array with teh first element removed</returns>
        public static T[] RemoveFirstElement<T>(this T[] array)
        {
            if (array == null || array.Length == 0)
            {
                throw new ArgumentException("Array cannot be null or empty.");
            }

            T[] newArray = new T[array.Length - 1];
            Array.Copy(array, 1, newArray, 0, array.Length - 1);
            return newArray;
        }
    }
}

