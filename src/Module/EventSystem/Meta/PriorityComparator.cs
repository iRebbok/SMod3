using System.Collections.Generic;

namespace SMod3.Module.EventSystem.Meta
{
	public sealed class PriorityComparator : IComparer<EventHandlerWrapper>
	{
		public int Compare(EventHandlerWrapper x, EventHandlerWrapper y)
		{
			// order by descending
			return y.Priority.CompareTo(x.Priority);
		}
	}
}
