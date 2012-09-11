using System.Data.Odbc;
using System.Data.Common;

namespace MysteryGuest_INC
{
    class OdbcDatabaseConnection : DatabaseConnection
    {
        public OdbcDatabaseConnection(string serverName, string user, string pass, string databaseName) 
            : base(serverName, user, pass, databaseName)
        {
            _dbConnection = new OdbcConnection(string.Format("DSN={0}; UID={1}; PWD={2}", serverName, user, pass));
        }
    }
}