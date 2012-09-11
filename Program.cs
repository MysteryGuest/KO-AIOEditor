using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MysteryGuest_INC
{
    static class Program
    {
        enum SettingsType
        {
            SettingsTypeDotNet,
            SettingsTypeRegistry
        };

        private const SettingsType UseSettings = SettingsType.SettingsTypeDotNet;
        public static UserSettings Settings;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            switch (UseSettings)
            {
                case SettingsType.SettingsTypeDotNet:
                    Settings = new DotNetUserSettings();
                    break;

                case SettingsType.SettingsTypeRegistry:
                    Settings = new RegistryUserSettings();
                    break;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }
    }
}
