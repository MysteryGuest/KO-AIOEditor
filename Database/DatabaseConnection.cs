using System;
using System.Data.Common;

namespace MysteryGuest_INC
{
    class DatabaseConnection : IDisposable
    {
        protected DbConnection _dbConnection;

        public DatabaseConnection()
        {
        }

        public void Open()
        {
            _dbConnection.Open();
        }

        public void Close()
        {
            _dbConnection.Close();
        }

        public void Dispose()
        {
            if (_dbConnection != null)
                Close();
        }
    }
}