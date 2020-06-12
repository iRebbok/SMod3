using System;
using System.Collections.Generic;
using System.Reflection;

using SMod3.Core;

namespace SMod3.Imbedded.Attribute
{
    /// <summary>
    ///     Attribute interaction helper.
    /// </summary>
    public static class AttributeHelper
    {
        /// <summary>
        ///		Gets Map of attributes and FieldInfo.
        /// </summary>
        /// <typeparam name="TInstance">
        ///		An object of a class or structure whose attributes need to be registered.
        /// </typeparam>
        /// <typeparam name="TAttribute">
        ///		Type of attribute to register.
        ///	</typeparam>
        /// <param name="instance">
        ///		An instance of an object.
        ///	</param>
        /// <returns>
        ///		Attributes or null if none are registered.
        /// </returns>
        public static IDictionary<TAttribute, FieldInfo>? PullAttributes<TAttribute, TInstance>(TInstance instance) where TAttribute : System.Attribute
        {
            if (instance is null)
                return null;

            return ProcessPullingAttributes<TAttribute, TInstance>(instance);
        }

        private static IDictionary<TAttribute, FieldInfo>? ProcessPullingAttributes<TAttribute, TInstance>(TInstance instance) where TAttribute : System.Attribute
        {
            Dictionary<TAttribute, FieldInfo> successfulAttributes = new Dictionary<TAttribute, FieldInfo>();
            FieldInfo[] fields = instance!.GetType().GetFields(PluginManager.ALL_BINDING_FLAGS);

            foreach (FieldInfo field in fields)
            {
                var attribute = field.GetCustomAttribute(typeof(TAttribute), false);
                if (attribute != null && TryPullAttributeParent<TAttribute>(attribute, out TAttribute? result))
                {
                    successfulAttributes.Add(result!, field);
                }
            }

            return successfulAttributes.Keys.Count != 0 ? successfulAttributes : null;
        }

        private static bool TryPullAttributeParent<TAttribute>(System.Attribute attrb, out TAttribute? result) where TAttribute : System.Attribute
        {
            Type tMemberType = typeof(TAttribute);


            result = default;
            return false;
        }
    }
}
