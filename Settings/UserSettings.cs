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
                // setting name                             default value
                new [] { "GameDatabase\\ConnectionMethod",    "ODBC"      },     // MySQL/MSSQL/ODBC
                new [] { "GameDatabase\\Server",              "KN_online" },     // Server name/host or datasource name
                new [] { "GameDatabase\\User",                "knight"    },     // Username
                new [] { "GameDatabase\\Password",            "knight"    },     // Password
                new [] { "GameDatabase\\DatabaseName",        ""          },     // Database name
                new [] { "GameDatabase\\TestPassed",          "0"         },     // Has it successfully connected using these details

                new [] { "AccountDatabase\\ConnectionMethod", "ODBC"      },     // MySQL/MSSQL/ODBC
                new [] { "AccountDatabase\\Server",           "KN_accounts" },   // Server name/host or datasource name
                new [] { "AccountDatabase\\User",             "knight"     },    // Username
                new [] { "AccountDatabase\\Password",         "knight"     },    // Password
                new [] { "AccountDatabase\\DatabaseName",     ""           },    // Database name
                new [] { "AccountDatabase\\TestPassed",       "0"          },    // Has it successfully connected using these details

            };

            foreach (var setting in settings)
            {
                try
                {
                    GetStringFromUserConfig(setting[0], setting[1]);
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
            return value;
        }

        public virtual void SetInt(string key, int value)
        {
            // dummy
        }

        public virtual string GetStringFromUserConfig(string key, string defaultValue = "")
        {
            // dummy
            return "";
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