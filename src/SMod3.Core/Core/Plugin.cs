using SMod3.Core.Logging;

namespace SMod3.Core
{
    public abstract class Plugin : BaseLogger
    {
        #region Properties

        public PluginInfoAttribute Definer
        {
            get;
            internal set;
        }

        public PluginStatus Status
        {
            get;
            internal set;
        }

        #endregion

        #region Plugin Events

        /// <summary>
        ///		Called first when loading the plugin.
        ///		<para>
        ///			It recommend registering your commands and events here.
        ///		</para>
        /// </summary>
        protected virtual void Awake() { }

        /// <summary>
        ///		Called when starting the plugin.
        /// </summary>
        protected virtual void OnEnable() { }

        /// <summary>
        ///		Called when disabling the plugin.
        ///		<para>
        ///			We strongly recommend unregister all external things, such as Harmony and so on.
        ///		</para>
        /// </summary>
        protected virtual void OnDisable() { }

        /// <summary>
        ///		Called when destroying the plugin.
        ///		<para>
        ///			Before this method is called <see cref="OnDisable"/> to delete all registered items, be careful.
        ///		</para>
        /// </summary>
        protected virtual void OnDestroy() { }

        #endregion

        //#region Reflection Event Region

        //public void InvokeEvent(string eventName) => InvokeEvent(eventName, null);
        //public void InvokeEvent(string eventName, params object[] args) => InvokeExternalEvent($"{Definer.Id}.{eventName}", args);

        //public void InvokeExternalEvent(string fullName) => InvokeExternalEvent(fullName, null);
        //public void InvokeExternalEvent(string fullName, params object[] args)
        //{
        //    if (string.IsNullOrEmpty(fullName))
        //    {
        //        throw new ArgumentNullException(nameof(fullName));
        //    }

        //    PipeManager.Manager.InvokeEvent(fullName, Definer.Id, args);
        //}

        //#endregion

        #region Base

        public override string LoggingTag => Definer.Id;

        internal void CallAwake()
        {
            //PluginManager.Debug($"Invoking {nameof(Awake)}: {this}");
            Awake();
        }

        internal void CallOnEnable()
        {
            //PluginManager.Debug($"Invoking {nameof(OnEnable)}: {this}");
            OnEnable();
        }

        internal void CallOnDisable()
        {
            //PluginManager.Debug($"Invoking {nameof(OnDisable)}: {this}");
            OnDisable();
        }

        internal void CallOnDestroy()
        {
            //PluginManager.Debug($"Invoking {nameof(OnDestroy)}: {this}");
            OnDestroy();
        }

        #endregion
    }
}
