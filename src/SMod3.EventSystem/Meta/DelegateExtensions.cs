using System;
using System.Linq;
using System.Reflection;

namespace SMod3.Module.EventSystem.Meta
{
    public static class DelegateExtension
    {
        /// <summary>
        ///     Compares the method for being able to convert to a delegate.
        /// </summary>
        public static bool IsMethodCompatibleWithDelegate(Type delegateType, MethodInfo methodInfo)
        {
            if (delegateType is null)
                throw new ArgumentNullException(nameof(delegateType));
            else if (methodInfo is null)
                throw new ArgumentNullException(nameof(methodInfo));

            if (!typeof(Delegate).IsAssignableFrom(delegateType))
                throw new ArgumentException("Must be a delegate", nameof(delegateType));

            // Literally a copy of
            // https://stackoverflow.com/a/28354494/13175172

            var delegateSignature = delegateType.GetMethod("Invoke", BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod);
            bool parametersEqual = delegateSignature
                    .GetParameters()
                    .Select(x => x.ParameterType)
                    .SequenceEqual(methodInfo.GetParameters()
                    .Select(x => x.ParameterType));

            return parametersEqual && delegateSignature.ReturnType == methodInfo.ReturnType;
        }
    }
}
