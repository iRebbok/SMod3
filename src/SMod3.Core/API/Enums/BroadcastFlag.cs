using System;

namespace SMod3.API
{
    [Flags]
    public enum BroadcastFlag : byte
    {
        Normal = 0,
        Monospaced = 1,
        AdminChat = 2
    }
}
