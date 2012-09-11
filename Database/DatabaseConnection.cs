using System;
using System.Data.Common;
using System.Data;

namespace MysteryGuest_INC
{
    abstract class DatabaseConnection : IDisposable
    {
        protected const int CommandTimeout = 5;

        protected DbConnection _dbConnection;
        protected string _serverName, _user, _pass, _databaseName;

        public DatabaseConnection(string serverName, string user, string pass, string databaseName)
        {
            _serverName = serverName;
            _user = user;
            _pass = pass;
            _databaseName = databaseName;
        }

        public DbConnection GetBaseConnection()
        {
            return _dbConnection;
        }

        public bool isOpen()
        {
            return _dbConnection.State != ConnectionState.Closed 
                && _dbConnection.State != ConnectionState.Broken;
        }

        public void Open()
        {
            _dbConnection.Open();
            if (_dbConnection.State != ConnectionState.Open)
                return;

            if (!String.IsNullOrEmpty(_databaseName))
                _dbConnection.ChangeDatabase(_databaseName);
        }


        public int Execute(string stmt, params DBParameter[] args)
        {
            if (!isOpen())
                Open();

            using (var dbCommand = _dbConnection.CreateCommand())
            {
                dbCommand.CommandText = stmt;
                dbCommand.CommandTimeout = CommandTimeout;

                foreach (var arg in args)
                {
                    var param = dbCommand.CreateParameter();
                    if (!arg.CopyParameter(ref param)) // invalid argument, to-do: error handling
                        continue;

                    dbCommand.Parameters.Add(param);
                }

                return dbCommand.ExecuteNonQuery();
            }
        }

        public DbDataReader Lookup(string stmt, params DBParameter[] args)
        {
            if (!isOpen())
                Open();

            using (var dbCommand = _dbConnection.CreateCommand())
            {
                dbCommand.CommandText = stmt;
                dbCommand.CommandTimeout = CommandTimeout;

                foreach (var arg in args)
                {
                    var param = dbCommand.CreateParameter();
                    if (!arg.CopyParameter(ref param)) // invalid argument, to-do: error handling
                        continue;

                    dbCommand.Parameters.Add(param);
                }

                return dbCommand.ExecuteReader();
            }
        }
        
        public void Close()
        {
            try
            {
                _dbConnection.Close();
            }
            catch
            {
                // we really don't care if it can't close.
            }
        }

        public void Dispose()
        {
            if (_dbConnection != null)
                Close();
        }

        ~DatabaseConnection()
        {
            this.Dispose();
        }
    }
}