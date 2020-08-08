namespace SMod3.API
{
    /// <summary>
    ///     Remote admin player group.
    /// </summary>
    public interface IUserGroup
    {
        public abstract string Name { get; }
        public abstract ColorType Color { get; }
        public abstract string BadgeText { get; }
        public abstract RemoteAdminPermissions Permissions { get; }
        public abstract bool Cover { get; }
        public abstract bool HiddenByDefault { get; }
        public abstract byte KickPower { get; }
        public abstract byte RequiredKickPower { get; }
        public abstract bool Shared { get; }
    }
}
