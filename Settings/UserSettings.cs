using System.Collections;
using System;
namespace MysteryGuest_INC
{
    abstract class UserSettings
    {
        protected Hashtable _settings;

        public UserSettings()
        {
            _settings = new Hashtable();
            Load();
        }

        private void Load()
        {
            string[][] settings = 
            {
                         // setting name                default value
                new [] { "Database\\ConnectionMethod", "ODBC" },        // MySQL/MSSQL/ODBC
                new [] { "Database\\Server",           "KN_online" },   // Server name/host or datasource name
                new [] { "Database\\Driver",           "SQL server" },  // Name of driver to use with MSSQL connections
                new [] { "Database\\User",             "knight" },      // Username
                new [] { "Database\\Password",         "knight" },      // Password
                new [] { "Database\\DatabaseName",     ""       },      // Database name
            };

            foreach (var setting in settings)
            {
                try
                {
                    GetString(setting[0], setting[1]);
                }
                catch (Exception)
                {
                    // to-do: error reporting
                }
            }
        }

        public virtual int GetInt(string key, int defaultValue = -1)
        {
            bool result;
            return GetInt(key, out result, defaultValue); 
        }

        protected int GetInt(string key, out bool result, int defaultValue = -1)
        {
            int value;

            result = false;
            if (!_settings.ContainsKey(key))
                return defaultValue;

            var rawValue = (string)_settings[key];
            if (!Int32.TryParse(rawValue, out value))
                return defaultValue;

            result = true;
            return defaultValue;
        }

        public virtual void SetInt(string key, int value)
        {
            // dummy
        }

        public virtual string GetString(string key, string defaultValue = "")
        {
            bool result;
            return GetString(key, out result, defaultValue);
        }

        protected string GetString(string key, out bool result, string defaultValue = "")
        {
            result = false;
            if (!_settings.ContainsKey(key))
                return "";

            result = true;
            return (string)_settings[key];
        }

        public virtual void SetString(string key, string value)
        {
            // dummy
        }

        protected virtual void UpdateSetting(string key, object value)
        {
            if (_settings.ContainsKey(key))
                _settings[key] = value;
            else
                _settings.Add(key, value);
        }
    }
}