namespace SMod3.API
{
    public abstract class Round
    {
        public abstract IRoundStats Stats { get; }
        public abstract int Duration { get; }

        public abstract void AddNTFUnit(string unit);
        public abstract void MTFRespawn(bool isCI);
        public abstract void RestartRound();
        public abstract void EndRound();
    }
}
