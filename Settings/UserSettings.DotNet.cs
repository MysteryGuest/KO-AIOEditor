using System.Configuration;
using System;
namespace MysteryGuest_INC
{
    class DotNetUserSettings : UserSettings
    {
        public DotNetUserSettings()
        {
        }

        private string NormaliseKey(string key)
        {
            return key.Replace('\\', '_');
        }

        public override int GetInt(string key, int defaultValue = -1)
        {
            bool result;
            var value = base.GetInt(key, out result, defaultValue);
            if (result)
                return value;

            var newKey = NormaliseKey(key);
            var tmp = Properties.Settings.Default[newKey];
            value = (tmp == null ? defaultValue : Int32.Parse((string)tmp));

            base.UpdateSetting(key, value.ToString());

            return value;
        }

        public override void SetInt(string key, int value)
        {
            UpdateSetting(key, value.ToString());
        }

        public override string GetStringFromUserConfig(string key, string defaultValue = "")
        {
            var newKey = NormaliseKey(key);
            var tmp = Properties.Settings.Default[newKey];
            var value = (tmp == null ? defaultValue : (string)tmp);

            base.UpdateSetting(key, value);

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
            UpdateSetting(key, value);
        }

        protected override void UpdateSetting(string key, object value)
        {
            base.UpdateSetting(key, value);

            key = NormaliseKey(key);
            Properties.Settings.Default[key] = (string)value;
            Properties.Settings.Default.Save();
        }
     }
}