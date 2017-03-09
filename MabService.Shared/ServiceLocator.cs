namespace MabService.Shared
{
    /// <summary>
    /// Service Locator
    /// </summary>
    public static class ServiceLocator
    {
        /// <summary>
        /// The application setting
        /// </summary>
        private static IMabAppSetting appSetting = new MabAppSetting();

        /// <summary>
        /// Initializes the specified setting.
        /// </summary>
        /// <param name="setting">The setting.</param>
        public static void Init(IMabAppSetting setting)
        {
            appSetting = setting;
        }

        /// <summary>
        /// Gets the application setting.
        /// </summary>
        /// <value>
        /// The application setting.
        /// </value>
        public static IMabAppSetting AppSetting
        {
            get
            {
                return appSetting;
            }
        }
    }
}
