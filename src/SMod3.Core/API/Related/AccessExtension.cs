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
            ["CONT_LVL_1"] = DoorAccessRequirements.CONTAINMENT_LEVEL_ONE,
            ["CONT_LVL_2"] = DoorAccessRequirements.CONTAINMENT_LEVEL_TWO,
            ["CONT_LVL_3"] = DoorAccessRequirements.CONTAINMENT_LEVEL_THREE,

            ["ARMORY_LVL_1"] = DoorAccessRequirements.CONTAINMENT_LEVEL_ONE,
            ["ARMORY_LVL_2"] = DoorAccessRequirements.CONTAINMENT_LEVEL_TWO,
            ["ARMORY_LVL_3"] = DoorAccessRequirements.CONTAINMENT_LEVEL_THREE,

            ["INCOM_ACC"] = DoorAccessRequirements.INTERCOM,
            ["CHCKPOINT_ACC"] = DoorAccessRequirements.CHECKPOINTS,
            ["EXIT_ACC"] = DoorAccessRequirements.GATES
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
