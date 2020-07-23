using System;

namespace SMod3.API
{
    public static class ItemTypeExtension
    {
        /// <summary>
        ///     Converts <see cref="MedicalItemType"/> to <see cref="ItemType"/>.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     No matching override.
        /// </exception>
        public static ItemType ConvertFromMedicalItem(this MedicalItemType medicalItem)
        {
            return medicalItem switch
            {
                MedicalItemType.MEDKIT => ItemType.MEDKIT,
                MedicalItemType.SCP500 => ItemType.SCP500,
                MedicalItemType.SCP207 => ItemType.SCP207,
                MedicalItemType.SCP268 => ItemType.SCP268,
                MedicalItemType.PAINKILLERS => ItemType.PAINKILLERS,
                MedicalItemType.ADRENALINE => ItemType.ADRENALINE,
                _ => throw new ArgumentOutOfRangeException(),
            };
        }
    }
}
