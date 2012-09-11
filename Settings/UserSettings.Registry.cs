using Microsoft.Win32;
using System;

namespace MysteryGuest_INC
{
    class RegistryUserSettings : UserSettings
    {
        private const string BasePath = @"Software\MysteryGuest inc\Beta001\";

        public RegistryUserSettings()
        {
            RegistryKey keyCreatesql = Registry.CurrentUser.CreateSubKey(@"Software\MysterGuest inc\Beta001\SQL\");

        }

        public override int GetInt(string key, int defaultValue = -1)
        {
            bool result;
            var value = base.GetInt(key, out result, defaultValue);
            if (result)
                return value;

            return (int)GetRegistryValue(key, defaultValue);
        }

        public override void SetInt(string key, int value)
        {
            SetRegistryValue(key, value);
            UpdateSetting(key, value);
        }

        public override string GetStringFromUserConfig(string key, string defaultValue = "")
        {
            var value = (string)GetRegistryValue(key, defaultValue);
            UpdateSetting(key, value);
            return value;
        }

        public override string GetString(string key, string defaultValue = "")
        {
            bool result;
            var value = base.GetString(key, out result, defaultValue);
            if (result)
                return value;

            return GetStringFromUserConfig(key, defaultValue);
        }

        public override void SetString(string key, string value)
        {
            SetRegistryValue(key, value);
            UpdateSetting(key, value);
        }

        private object GetRegistryValue(string key, object defaultValue)
        {
            try
            {
                var regKey = Registry.CurrentUser.CreateSubKey(BasePath + key);
                var result = regKey.GetValue("value", defaultValue);

                regKey.Close();

                if (result == null)
                    return defaultValue;

                return result;
            }
            catch (Exception)
            {
                // error handling
                return null;
            }
        }

        private bool SetRegistryValue(string key, object value)
        {
            try
            {
                var regKey = Registry.CurrentUser.CreateSubKey(BasePath + key);
                regKey.SetValue("value", value);

                regKey.Close();
                return true;
            }
            catch (Exception)
            {
                // error handling
                return false;
            }
        }

    }
}