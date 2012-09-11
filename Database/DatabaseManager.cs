using System.Collections.Generic;
using System;

namespace MysteryGuest_INC
{
    public enum ConnectionType
    {
        ConnTypeODBC,
        ConnTypeSQL,
        ConnTypeUnknown
    }

    public enum DatabaseType
    {
        DatabaseTypeAccount,
        DatabaseTypeGame
    }

    public struct K_MONSTER
    {
        public int sSid;
        public string strName;
        public int sItem;
    }

    public struct ITEM
    {
        public int Num;
        public string strName;
    }

    static class DatabaseManager
    {
        public static DatabaseConnection AccountDB;
        public static DatabaseConnection GameDB;

        private static Dictionary<int, K_MONSTER> m_monsterTable;
        private static Dictionary<int, ITEM> m_itemTable;

        public static void Init()
        {
            CreateConnection(DatabaseType.DatabaseTypeAccount);
            CreateConnection(DatabaseType.DatabaseTypeGame);
        }

        public static DatabaseConnection CreateConnection(DatabaseType databaseType)
        {
            var prefix = (databaseType == DatabaseType.DatabaseTypeAccount ? "Account" : "Game") 
                + "Database\\"; 

            var connectionMethod = Program.Settings.GetString(prefix + "ConnectionMethod");
            var serverName = Program.Settings.GetString(prefix + "Server");
            var driverName = Program.Settings.GetString(prefix + "Driver");
            var user = Program.Settings.GetString(prefix + "User");
            var pass = Program.Settings.GetString(prefix + "Password");
            var databaseName = Program.Settings.GetString(prefix + "DatabaseName");

            var result = CreateConnection(ConnectionTypeFromString(connectionMethod), serverName, user, pass, databaseName, driverName);
            if (result != null)
            {
                if (databaseType == DatabaseType.DatabaseTypeAccount)
                    AccountDB = result;
                else
                    GameDB = result;
            }
            return result;
        }

        public static DatabaseConnection CreateConnection(ConnectionType connectionType, string serverName, string user, string pass, string databaseName = null, string driverName = null)
        {
            switch (connectionType)
            {
                case ConnectionType.ConnTypeODBC:
                    return new OdbcDatabaseConnection(serverName, user, pass, databaseName);

                case ConnectionType.ConnTypeSQL:
                    return new SQLDatabaseConnection(serverName, user, pass, databaseName);
            }

            return null; // no matching connection method
        }

        public static ConnectionType ConnectionTypeFromString(string method)
        {
            switch (method.ToUpper())
            {
                case "ODBC":
                    return ConnectionType.ConnTypeODBC;

                case "SQL":
                    return ConnectionType.ConnTypeSQL;
            }

            return ConnectionType.ConnTypeUnknown;
        }

        public static Dictionary<int, K_MONSTER> GetMonsterTable()
        {
            if (m_monsterTable == null)
            {
                m_monsterTable = new Dictionary<int, K_MONSTER>();
                LoadMonsterTable();
            }

            return m_monsterTable;
        }

        public static bool LoadMonsterTable()
        {
            // clear existing table data
            m_monsterTable.Clear();

            if (!GameDB.isOpen())
                return false;

            try
            {
                using (var dbReader = GameDB.Lookup("SELECT DISTINCT sSid, strName, sItem FROM K_MONSTER"))
                {
                    while (dbReader.Read())
                    {
                        var row = new K_MONSTER();

                        row.sSid = dbReader.GetInt32(0);
                        row.strName = dbReader.GetString(1);
                        row.sItem = dbReader.GetInt32(2);

                        m_monsterTable.Add(row.sSid, row);
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static Dictionary<int, ITEM> GetItemTable()
        {
            if (m_itemTable == null)
            {
                m_itemTable = new Dictionary<int, ITEM>();
                LoadItemTable();
            }

            return m_itemTable;
        }

        public static bool LoadItemTable()
        {
            // clear existing table data
            m_itemTable.Clear();

            if (!GameDB.isOpen())
                return false;

            try
            {
                using (var dbReader = GameDB.Lookup("SELECT DISTINCT Num, strName FROM ITEM"))
                {
                    while (dbReader.Read())
                    {
                        var row = new ITEM();

                        row.Num = dbReader.GetInt32(0);
                        row.strName = dbReader.GetString(1);

                        m_itemTable.Add(row.Num, row);
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}