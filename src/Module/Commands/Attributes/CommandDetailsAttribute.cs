using SMod3.Module.Attributes.Meta;
using System;

namespace SMod3.Module.Commands.Attributes
{
    /// <summary>
    ///		Attribute used to define the command.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CommandDetailsAttribute : BaseAttribute
    {
        /// <summary>
        ///		The aliases that the handler will trigger.
        /// </summary>
        public string[] Aliases { get; set; }
        /// <summary>
        ///		Description of what this command is for and how it works.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        ///		Example of using the command.
        ///		<para>
        ///			Describe all sorts of necessary/unnecessary args, etc.
        ///		</para>
        /// </summary>
        public string Usage { get; set; }
    }
}
