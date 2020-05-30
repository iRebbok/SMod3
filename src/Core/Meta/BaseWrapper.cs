namespace SMod3.Core.Meta
{
	public class BaseWrapper
	{
		/// <summary>
		///		The owner of this wrapper.
		/// </summary>
		public Plugin Owner { get; }

		public BaseWrapper(Plugin owner)
		{
			Owner = owner;
		}
	}
}
