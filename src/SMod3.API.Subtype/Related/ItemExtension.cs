using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SMod3.API
{
    public static partial class Extension
    {
        /// <summary>
        ///     Backward compatible string permissions for the item.
        ///     Used to convert item permissions to enum and vice versa.
        /// </summary>
        public static readonly IReadOnlyDictionary<string, AccessRequirements> BackwardsCompatibleItemPermissions = new ReadOnlyDictionary<string, AccessRequirements>(new Dictionary<string, AccessRequirements>
        {
            ["CONT_LVL_1"] = AccessRequirements.ContainmentLevelOne,
            ["CONT_LVL_2"] = AccessRequirements.ContainmentLevelTwo,
            ["CONT_LVL_3"] = AccessRequirements.ContainmentLevelThree,

            ["ARMORY_LVL_1"] = AccessRequirements.ArmoryLevelOne,
            ["ARMORY_LVL_2"] = AccessRequirements.ArmoryLevelTwo,
            ["ARMORY_LVL_3"] = AccessRequirements.ArmoryLevelThree,

            ["INCOM_ACC"] = AccessRequirements.Intercom,
            ["CHCKPOINT_ACC"] = AccessRequirements.Checkpoints,
            ["EXIT_ACC"] = AccessRequirements.Gates
        });

        /// <summary>
        ///     Returns converted permissions to string.
        /// </summary>
        public static IEnumerable<string> AccessRequirementsToString(AccessRequirements access)
        {
            foreach (AccessRequirements value in Enum.GetValues(typeof(AccessRequirements)))
                if (access.HasFlag(value))
                {
                    // There is no such permission as Unaccessible, so we filter it
                    var stringPerm = BackwardsCompatibleItemPermissions.FirstOrDefault(a => a.Value == value).Key;
                    if (!string.IsNullOrEmpty(stringPerm))
                        yield return stringPerm;
                }
        }

        /// <summary>
        ///     Converts permissions from a string.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Permission is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     Could not find any permissions.
        /// </exception>
        public static AccessRequirements StringToAccessRequirements(IEnumerable<string> permission)
        {
            if (permission is null)
                throw new ArgumentNullException("Permissions cannot be null", nameof(permission));

            AccessRequirements? access = null;
            foreach (var perm in permission)
            {
                if (string.IsNullOrEmpty(perm) || !BackwardsCompatibleItemPermissions.TryGetValue(perm, out var result))
                    continue;

                access |= result;
            }

            return access!.Value;
        }
    }
}
