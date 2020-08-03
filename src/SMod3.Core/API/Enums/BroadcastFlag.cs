using System;

namespace SMod3.API
{
    [Flags]
    public enum BroadcastFlag : byte
    {
        NORMAL = 0,
        MONOSPACED = 1,
        ADMIN_CHAT = 2
    }
}
