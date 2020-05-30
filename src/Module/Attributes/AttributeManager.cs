using SMod3.Core;
using SMod3.Module.Attributes.Meta;
using SMod3.Module.Config;
using SMod3.Module.Config.Attributes;
using SMod3.Module.Lang;
using SMod3.Module.Lang.Attributes;
using SMod3.Module.Piping.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SMod3.Module.Attributes
{
    public sealed class AttributeManager
    {
        private AttributeManager() { }

        /// <summary>
        ///		The main instance of the class, do not create other instances.
        /// </summary>
        public static AttributeManager Manager { get; } = new AttributeManager();

        /// <summary>
        ///		Refreshes all the registered by the plugin attributes.
        /// </summary>
        public void RefreshPluginAttributes(Plugin plugin)
        {
            ConfigManager.Manager.RefreshAttributes(plugin);
            LangManager.Manager.RefreshAttributes(plugin);
        }

        /// <summary>
        ///		Gets Map of attributes and FieldInfo.
        /// </summary>
        /// <typeparam name="TAttribute">
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
        public IDictionary<TAttribute, FieldInfo> PullAttributes<TAttribute, TInstance>(TInstance instance) where TAttribute : BaseAttribute
        {
            if (instance == null)
            {
                return null;
            }

            return ProcessPullingAttributes<TAttribute, TInstance>(instance);
        }

        private IDictionary<TAttribute, FieldInfo> ProcessPullingAttributes<TAttribute, TInstance>(TInstance instance) where TAttribute : BaseAttribute
        {
            Dictionary<TAttribute, FieldInfo> successfulAttributes = new Dictionary<TAttribute, FieldInfo>();
            FieldInfo[] fields = instance.GetType().GetFields(PluginManager.ALL_BINDING_FLAGS);

            foreach (FieldInfo field in fields)
            {
                Attribute attribute = field.GetCustomAttribute(typeof(TAttribute), false);
                if (attribute != null && TryPullAttributeParent<TAttribute>(attribute, out TAttribute result))
                {
                    successfulAttributes.Add(result, field);
                }
            }

            return successfulAttributes.Keys.Count != 0 ? successfulAttributes : null;
        }

        private bool TryPullAttributeParent<TAttribute>(Attribute attrb, out TAttribute result) where TAttribute : BaseAttribute
        {
            Type tMemberType = typeof(TAttribute);

            #region Config

            if (tMemberType.IsAssignableFrom(typeof(ConfigOptionAttribute)))
            {
                ConfigOptionAttribute configOption = (ConfigOptionAttribute)attrb;
                if (configOption != null)
                {
                    result = (TAttribute)(BaseAttribute)configOption;
                    return true;
                }
            }

            #endregion

            #region Lang

            if (tMemberType.IsAssignableFrom(typeof(LangOptionAttribute)))
            {
                LangOptionAttribute langOption = (LangOptionAttribute)attrb;
                if (langOption != null)
                {
                    result = (TAttribute)(BaseAttribute)langOption;
                    return true;
                }
            }

            #endregion

            #region Piping

            if (tMemberType.IsAssignableFrom(typeof(PipeEventAttribute)))
            {
                PipeEventAttribute pipeEvent = (PipeEventAttribute)attrb;
                if (pipeEvent != null)
                {
                    result = (TAttribute)(BaseAttribute)attrb;
                    return true;
                }
            }

            if (tMemberType.IsAssignableFrom(typeof(PipeFieldAttribute)))
            {
                PipeFieldAttribute pipeField = (PipeFieldAttribute)attrb;
                if (pipeField != null)
                {
                    result = (TAttribute)(BaseAttribute)attrb;
                    return true;
                }
            }

            if (tMemberType.IsAssignableFrom(typeof(PipeLinkAttribute)))
            {
                PipeLinkAttribute pipeLink = (PipeLinkAttribute)attrb;
                if (pipeLink != null)
                {
                    result = (TAttribute)(BaseAttribute)attrb;
                    return true;
                }
            }

            if (tMemberType.IsAssignableFrom(typeof(PipeMethodAttribute)))
            {
                PipeMethodAttribute pipeMethod = (PipeMethodAttribute)attrb;
                if (pipeMethod != null)
                {
                    result = (TAttribute)(BaseAttribute)attrb;
                    return true;
                }
            }

            if (tMemberType.IsAssignableFrom(typeof(PipePropertyAttribute)))
            {
                PipePropertyAttribute pipeProperty = (PipePropertyAttribute)attrb;
                if (pipeProperty != null)
                {
                    result = (TAttribute)(BaseAttribute)attrb;
                    return true;
                }
            }

            #endregion

            result = default(TAttribute);
            return false;
        }
    }
}
