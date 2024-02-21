// © 2023, Worth Systems.

using System.Reflection;

namespace EventsHandler.Extensions
{
    /// <summary>
    /// Extension methods for different objects and behaviors related to <see cref="System.Reflection"/> mechanic.
    /// </summary>
    internal static class ReflectionExtensions
    {
        /// <summary>
        /// Determines whether the value of the property (<see cref="PropertyInfo"/>) is null.
        /// </summary>
        internal static bool NotInitializedProperty<TInstance>(this TInstance instance, PropertyInfo property)
        {
            return instance.GetPropertyValue(property) == null;
        }

        /// <summary>
        /// Gets the value of the property (<see cref="PropertyInfo"/>) from the given instance.
        /// </summary>
        internal static object? GetPropertyValue<TInstance>(this TInstance instance, PropertyInfo property)
        {
            return property.GetValue(instance);
        }
    }
}