using SMod3.Core;
using SMod3.Core.Meta;
using System.Collections.Generic;

namespace SMod3.Module.Commands.Meta
{
    /// <summary>
    ///		The main wrapper to keep all the commands in the command manager.
    /// </summary>
    /// <typeparam name="T">
    ///		Type of command handler.
    /// </typeparam>
    public sealed class CommandWrapper<T> : BaseWrapper where T : BaseCommandHandler
    {
        /// <summary>
        ///		Handler for the commands specified in <see cref="Commands"/>.
        /// </summary>
        public T Handler { get; }
        /// <summary>
        ///		Commands that trigger the handler.
        /// </summary>
        public HashSet<string> Commands { get; }

        public CommandWrapper(Plugin owner, T handler, IEnumerable<string> commands) : base(owner)
        {
            Handler = handler;
            Commands = new HashSet<string>(commands);
        }
    }
}
