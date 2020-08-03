namespace SMod3.API
{
    public interface IRoundStats
    {
        int NTFAlive { get; }
        int CIAlive { get; }

        int ScientistsAlive { get; }
        int ScientistsEscaped { get; }
        int ScientistsDead { get; }
        int ScientistsStart { get; }

        int ClassDEscaped { get; }
        int ClassDDead { get; }
        int ClassDAlive { get; }
        int ClassDStart { get; }

        int Zombies { get; }
        int GrenadeKills { get; }

        int SCPDead { get; }
        int SCPKills { get; }
        int SCPAlive { get; }
        int SCPStart { get; }

        bool WarheadDetonated { get; }
    }
}
