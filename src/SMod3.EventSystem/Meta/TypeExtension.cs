using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using SMod3.Module.EventSystem.Background;

namespace SMod3.Module.EventSystem.Meta
{
    public sealed class CustomParameterEqualityComparer : IEqualityComparer<ParameterInfo>
    {
        public bool Equals(ParameterInfo x, ParameterInfo y)
        {
            // Since we have delegates that have generic parameters as event arguments,
            // we need to figure out that we can eventually use them.
            // We don't need to do this with `y`, because it shouldn't have generic parameters.
            return (x.ParameterType.IsGenericParameter ?
                x.ParameterType.BaseType.IsAssignableFrom(y.ParameterType) :
                x.ParameterType == y.ParameterType) &&
                    x.IsOut == y.IsOut &&
                    x.IsIn == y.IsIn &&
                    x.Position == y.Position;
        }

        public int GetHashCode(ParameterInfo obj)
        {
            return obj.GetHashCode();
        }
    }

    public static class TypeExtension
    {
        /// <summary>
        ///     Flags by which methods are searched. Doesn't include private and static search.
        /// </summary>
        public const BindingFlags METHOD_SEARCH_FLAGS = BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod;

        /// <summary>
        ///     Same as <see cref="METHOD_SEARCH_FLAGS"/>, but include private search.
        /// </summary>
        public const BindingFlags METHOD_SEARCH_FLAGS_INCLUDE_PRIVATE = METHOD_SEARCH_FLAGS | BindingFlags.NonPublic;

        /// <summary>
        ///     Same as <see cref="METHOD_SEARCH_FLAGS_INCLUDE_PRIVATE"/>, but include static search.
        /// </summary>
        public const BindingFlags METHOD_SEARCH_FLAGS_INCLUDE_STATIC = METHOD_SEARCH_FLAGS_INCLUDE_PRIVATE | BindingFlags.Static;

        public static readonly CustomParameterEqualityComparer CustomParameterInfoComparer = new CustomParameterEqualityComparer();

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

            var delegateSignature = delegateType.GetMethod("Invoke", METHOD_SEARCH_FLAGS);
            var parametersEqual = delegateSignature.GetParameters().SequenceEqual(methodInfo.GetParameters(), CustomParameterInfoComparer);

            return parametersEqual && delegateSignature.ReturnType == methodInfo.ReturnType;
        }

        /// <summary>
        ///     Compares the method for being able to use as a handler method.
        ///     It doesn't compare arguments if <paramref name="method"/> doesn't have them,
        ///     but is sensitive to the presence of arguments if the handler method doesn't have them.
        /// </summary>
        /// <param name="eventArgType">
        ///     The argument that the handler supplies,
        ///     can be null if the handler doesn't provide an argument.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     Type isn't a handler (inherited from <see cref="IEventHandler"/>)
        ///     or couldn't find methods.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Type provides more than one method.
        /// </exception>
        public static bool IsMethodCompatibleWithEventHandler(Type handler, MethodInfo method, out Type? eventArgType)
        {
            CheckTypeIsEventHandler(handler);

            var handlerMethods = handler.GetMethods(METHOD_SEARCH_FLAGS);
            if (handlerMethods.Length > 1)
                throw new ArgumentOutOfRangeException(nameof(handler), "The handler contains more than one method - doesn't follow the standard");
            else if (handlerMethods.Length != 1)
                throw new ArgumentException("Failed to get any methods, the handler doesn't contain any methods?", nameof(handler));

            var handlerMethod = handlerMethods[0];
            var handlerMethodParameters = handlerMethod.GetParameters();

            var hasOneArg = handlerMethodParameters.Length == 1;
            if (!hasOneArg && handlerMethodParameters.Length != 0)
            {
                throw new ArgumentException("Handler method contains more than one arg - doesn't follow the standard");
            }

            eventArgType = null;
            if (hasOneArg)
            {
                eventArgType = handlerMethodParameters[0].ParameterType;
            }

            var methodPrameters = method.GetParameters();

            // Method isn't generic
            // 1: Method has no parameters
            // 2: Both have the same number of params & they are equal
            return !method.IsGenericMethod && ((methodPrameters.Length == 0) ||
                (handlerMethodParameters.Length == methodPrameters.Length && handlerMethodParameters.SequenceEqual(methodPrameters, CustomParameterInfoComparer)));
        }

        /// <summary>
        ///     Checks if the type is one of the event handlers.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Type is null.
        /// </exception>
        /// <returns>
        ///     true if the type is an event handler,
        ///     an interface, and not the <see cref="IEventHandler"/> itself;
        ///     otherwise, false.
        /// </returns>
        public static bool TypeIsEventHandler(Type type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            return type.IsInterface && type != typeof(IEventHandler) && typeof(IEventHandler).IsAssignableFrom(type);
        }

        /// <summary>
        ///     Checks and throws an exception if the type isn't one of the event handlers.
        /// </summary>
        /// <exception cref="ArgumentNullException"><inheritdoc cref="TypeIsEventHandler(Type)" /></exception>
        /// <exception cref="ArgumentException">
        ///     The type isn't one of the event handlers.
        /// </exception>
        public static void CheckTypeIsEventHandler(Type type)
        {
            if (!TypeIsEventHandler(type))
                throw new ArgumentException("Type isn't an event handler");
        }
    }
}
