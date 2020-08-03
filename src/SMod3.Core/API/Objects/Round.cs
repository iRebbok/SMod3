using System;

namespace SMod3.API
{
    public abstract class Round
    {
        public abstract IRoundStats Stats { get; }
        public abstract TimeSpan Duration { get; }
        public DateTime StartedTime => DateTime.Now - Duration;
        public abstract bool RoundInProgress { get; }
        public abstract bool LobbyLock { get; set; }
        public abstract bool RoundLock { get; set; }

        /// <summary>
        ///     Forcibly starts the round if it has not started yet.
        /// </summary>
        /// <returns>
        ///     true if the round has been started; otherwise, false;
        /// </returns>
        public abstract bool ForceStartRound();
        /// <summary>
        ///     Forcibly restarts the round.
        /// </summary>
        /// <returns>
        ///     true if the round has been restarted; otherwise, false;
        /// </returns>
        public abstract bool ForceRestartRound();
        /// <summary>
        ///     Forcibly ends the round.
        /// </summary>
        /// <param name="ending">
        ///     Determines if the round is ending.
        /// </param>
        public abstract void ForceEndRound(bool ending = true);
    }
}
