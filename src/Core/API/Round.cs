namespace SMod3.Core.API
{
    public abstract class Round
    {
        public abstract RoundStats Stats { get; }
        public abstract void EndRound();
        public abstract int Duration { get; }
        public abstract void AddNTFUnit(string unit);
        public abstract void MTFRespawn(bool isCI);
        public abstract void RestartRound();
    }
}
