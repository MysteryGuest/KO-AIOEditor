using System.Data.Odbc;
namespace MysteryGuest_INC
{
    class OdbcDatabaseConnection : DatabaseConnection
    {
        public OdbcDatabaseConnection()
        {
            _dbConnection = new OdbcConnection();
        }
    }
}