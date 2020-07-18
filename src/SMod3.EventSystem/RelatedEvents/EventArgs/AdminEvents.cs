using SMod3.API;
using SMod3.Module.EventSystem.Background;

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

namespace SMod3.Module.EventSystem.Events
{
    public sealed class AdminQueryEvent : AdminEventArg
    {
        public string Query { get; set; }
        /// <remarks>
        ///     Sends anyways.
        /// </remarks>
        public string? Message { get; set; }
        /// <summary>
        ///     Blocks query processing,
        ///     sends <see cref="Message"/> if isn't null or empty.
        /// </summary>
        public bool Allow { get; set; }

        internal override void Reset()
        {
            Query = null;
            Message = null;
            Allow = true;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<AdminQueryEvent>(other);
            target.Query = Query;
            target.Message = Message;
            target.Allow = Allow;
            target.CommandSender = CommandSender;
        }
    }

    public sealed class BanEvent : BanEventArg, IAllowable
    {
        public Player Target { get; set; }
        /// <summary>
        ///     Ban duration, measured in minutes.
        /// </summary>
        public uint Duration { get; set; }
        public string Reason { get; set; }
        public bool Allow { get; set; }
        /// <summary>
        ///     The global ban is different
        ///     that ban exclusively by IP.
        /// </summary>
        public bool IsGlobalBan { get; set; }

        internal override void Reset()
        {
            Target = default;
            Duration = default;
            Reason = default;
            Allow = true; // don't use default here
            IsGlobalBan = default;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<BanEvent>(other);
            target.Target = Target;
            target.Issuer = Issuer;
            target.Duration = Duration;
            target.Reason = Reason;
            target.Allow = Allow;
            target.IsGlobalBan = IsGlobalBan;
        }
    }

    public sealed class OfflineBanEvent : BanEventArg, IAllowable
    {
        public string TargetNickname { get; set; }
        public string TargetId { get; internal set; }
        public string Reason { get; set; }
        /// <summary>
        ///     <inheritdoc cref="BanEvent.Duration"/>
        ///     Although the source is ticks, we convert it to minutes.
        /// </summary>
        public uint Duration { get; set; }
        public BanType Type { get; internal set; }
        public bool Allow { get; set; }

        internal override void Reset()
        {
            TargetNickname = default;
            TargetId = default;
            Issuer = default;
            Reason = default;
            Duration = default;
            Type = default;
            Allow = true;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<OfflineBanEvent>(other);
            target.TargetNickname = TargetNickname;
            target.TargetId = TargetId;
            target.Issuer = Issuer;
            target.Reason = Reason;
            target.Duration = Duration;
            target.Type = Type;
            target.Allow = Allow;
        }
    }
}
