namespace SMod3.API
{
    public enum GrenadeType
    {
        FRAG_GRENADE = 0,
        FLASHBANG = 1
    }

    public enum ItemCategory
    {
        NOCATEGORY = 0,
        KEYCARD = 1,
        MEDICAL = 2,
        RADIO = 3,
        WEAPON = 4,
        GRENADE = 5,
        SCPITEM = 6, // SCP-018, SCP-500, SCP-207, SCP-268
        MICROHID = 7
    }

    public enum ItemType
    {
        NONE = -1,
        KEYCARD_JANITOR = 0,
        KEYCARD_SCIENTIST = 1,
        KEYCARD_SCIENTIST_MAJOR = 2,
        KEYCARD_ZONE_MANAGER = 3,
        KEYCARD_GUARD = 4,
        KEYCARD_SENIOR_GUARD = 5,
        KEYCARD_CONTAINMENT_ENGINEER = 6,
        KEYCARD_NTF_LIEUTENANT = 7,
        KEYCARD_NTF_COMMANDER = 8,
        KEYCARD_FACILITY_MANAGER = 9,
        KEYCARD_CHAOS_INSURGENCY = 10,
        KEYCARD_O5 = 11,
        RADIO = 12,
        GUN_COM15 = 13,
        MEDKIT = 14,
        FLASHLIGHT = 15,
        MICRO_HID = 16,
        SCP500 = 17,
        SCP207 = 18,
        WEAPON_MANAGER_TABLET = 19,
        GUN_E11_SR = 20,
        GUN_PROJECT90 = 21,
        AMMO556 = 22,
        GUN_MP7 = 23,
        GUN_LOGICER = 24,
        GRENADE_FRAG = 25,
        GRENADE_FLASH = 26,
        DISARMER = 27,
        AMMO762 = 28,
        AMMO9MM = 29,
        GUN_USP = 30,
        SCP018 = 31,
        SCP268 = 32,
        ADRENALINE = 33,
        PAINKILLERS = 34,
        COIN = 35,
    }
}