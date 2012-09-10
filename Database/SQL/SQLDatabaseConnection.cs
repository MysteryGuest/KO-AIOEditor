using System.Data.SqlClient;

namespace MysteryGuest_INC
{
    class SQLDatabaseConnection : DatabaseConnection
    {
        public SQLDatabaseConnection()
        {
            _dbConnection = new SqlConnection();
        }
    }
}