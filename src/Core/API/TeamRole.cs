namespace SMod3.Core.API
{
    public abstract class TeamRole
    {
        public abstract TeamType Team { get; }
        public abstract RoleType Role { get; }
        public abstract bool RoleDisallowed { get; set; }
        public abstract int MaxHP { get; set; }
        public abstract string Name { get; set; }
        public abstract object GetClass();
    }
}
