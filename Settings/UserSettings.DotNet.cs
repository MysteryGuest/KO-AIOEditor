using System.Configuration;
using System;
namespace MysteryGuest_INC
{
    class DotNetUserSettings : UserSettings
    {
        public DotNetUserSettings()
        {
        }

        public override int GetInt(string key, int defaultValue = -1)
        {
            bool result;
            var value = base.GetInt(key, out result, defaultValue);
            if (result)
                return value;

            var tmp = Properties.Settings.Default.PropertyValues[key];
            value = (tmp == null ? defaultValue : (int)tmp.PropertyValue);
            base.UpdateSetting(key, value);

            return value;
        }

        public override void SetInt(string key, int value)
        {
            UpdateSetting(key, value);
        }

        public override string GetString(string key, string defaultValue = "")
        {
            bool result;
            var value = base.GetString(key, out result, defaultValue);
            if (result)
                return value;

            var tmp = Properties.Settings.Default.PropertyValues[key];
            value = (tmp == null ? defaultValue : (string)tmp.PropertyValue);
            base.UpdateSetting(key, value);

            return value;
        }

        public override void SetString(string key, string value)
        {
            UpdateSetting(key, value);
        }

        protected override void UpdateSetting(string key, object value)
        {
            var property = new SettingsProperty(key);
            var propertyValue = new SettingsPropertyValue(property);
            propertyValue.PropertyValue = value;

            Properties.Settings.Default.PropertyValues.Add(propertyValue);
            Save();

            base.UpdateSetting(key, value);
        }

        private void Save()
        {
            Properties.Settings.Default.Save();
        }
     }
}