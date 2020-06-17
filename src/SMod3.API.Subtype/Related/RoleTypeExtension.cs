using System;
using System.Collections.Generic;

namespace SMod3.API
{
    public static partial class Extension
    {
        /// <summary>
        ///     Playable role types.
        /// </summary>
        public static IReadOnlyList<RoleType> RolePlayables { get; } = Array.AsReadOnly<RoleType>(new RoleType[17]
        {
            RoleType.SCP_049,
            RoleType.SCP_049_2,
            RoleType.SCP_079,
            RoleType.SCP_096,
            RoleType.SCP_106,
            RoleType.SCP_173,
            RoleType.SCP_939_53,
            RoleType.SCP_939_89,
            RoleType.CLASSD,
            RoleType.CHAOS_INSURGENCY,
            RoleType.SCIENTIST,
            RoleType.FACILITY_GUARD,
            RoleType.NTF_CADET,
            RoleType.NTF_LIEUTENANT,
            RoleType.NTF_COMMANDER,
            RoleType.NTF_SCIENTIST,
            RoleType.TUTORIAL
        });

        /// <summary>
        ///     SCP roles.
        /// </summary>
        public static IReadOnlyList<RoleType> RoleSCPs { get; } = Array.AsReadOnly<RoleType>(new RoleType[8]
        {
            RoleType.SCP_049,
            RoleType.SCP_049_2,
            RoleType.SCP_079,
            RoleType.SCP_096,
            RoleType.SCP_106,
            RoleType.SCP_173,
            RoleType.SCP_939_53,
            RoleType.SCP_939_89
        });

        /// <summary>
        ///     SCP roles that are available to block during spawn.
        /// </summary>
        public static IReadOnlyList<RoleType> RoleBannableSCPs { get; } = Array.AsReadOnly<RoleType>(new RoleType[7]
        {
            RoleType.SCP_049,
            RoleType.SCP_079,
            RoleType.SCP_096,
            RoleType.SCP_106,
            RoleType.SCP_173,
            RoleType.SCP_939_53,
            RoleType.SCP_939_89
        });

        /// <summary>
        ///     SCP roles that are available to pickup.
        /// </summary>
        public static IReadOnlyList<RoleType> RolePickableSCPs { get; } = Array.AsReadOnly<RoleType>(new RoleType[7]
        {
            RoleType.SCP_049,
            RoleType.SCP_079,
            RoleType.SCP_096,
            RoleType.SCP_106,
            RoleType.SCP_173,
            RoleType.SCP_939_53,
            RoleType.SCP_939_89
        });
    }
}
