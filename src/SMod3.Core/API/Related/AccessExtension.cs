using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SMod3.API
{
    public static class AccessExtension
    {
        /// <summary>
        ///     Backward compatible string permissions for the item.
        ///     Used to convert item permissions to enum and vice versa.
        /// </summary>
        public static readonly IReadOnlyDictionary<string, DoorAccessRequirements> BackwardsCompatibleItemPermissions = new ReadOnlyDictionary<string, DoorAccessRequirements>(new Dictionary<string, DoorAccessRequirements>
        {
            ["CONT_LVL_1"] = DoorAccessRequirements.ContainmentLevelOne,
            ["CONT_LVL_2"] = DoorAccessRequirements.ContainmentLevelTwo,
            ["CONT_LVL_3"] = DoorAccessRequirements.ContainmentLevelThree,

            ["ARMORY_LVL_1"] = DoorAccessRequirements.ArmoryLevelOne,
            ["ARMORY_LVL_2"] = DoorAccessRequirements.ArmoryLevelTwo,
            ["ARMORY_LVL_3"] = DoorAccessRequirements.ArmoryLevelThree,

            ["INCOM_ACC"] = DoorAccessRequirements.Intercom,
            ["CHCKPOINT_ACC"] = DoorAccessRequirements.Checkpoints,
            ["EXIT_ACC"] = DoorAccessRequirements.Gates
        });

        /// <summary>
        ///     Returns converted permissions to string.
        /// </summary>
        public static IEnumerable<string> AccessRequirementsToString(DoorAccessRequirements access)
        {
            foreach (DoorAccessRequirements value in Enum.GetValues(typeof(DoorAccessRequirements)))
            {
                if ((access & value) != 0)
                {
                    // There is no such permission as Unaccessible, so we filter it
                    var stringPerm = BackwardsCompatibleItemPermissions.FirstOrDefault(a => a.Value == value).Key;
                    if (!string.IsNullOrEmpty(stringPerm))
                        yield return stringPerm;
                }
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
        public static DoorAccessRequirements StringToAccessRequirements(IEnumerable<string> permission)
        {
            if (permission is null)
                throw new ArgumentNullException("Permissions cannot be null", nameof(permission));

            DoorAccessRequirements? access = null;
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
