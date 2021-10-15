using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Tools.Connection.Database
{
    public class Connection : IConnection
    {
        private readonly DbProviderFactory _factory;
        private readonly string _connectionString;

        public Connection(DbProviderFactory factory, string connectionString)
        {
            if (factory is null)
                throw new ArgumentNullException();

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("Invalid ConnectionString");

            _factory = factory;
            _connectionString = connectionString;

            using (DbConnection dbConnection = CreateConnection(_factory, _connectionString))
            {
                dbConnection.Open();
            }
        }

        public int ExecuteNonQuery(Command command)
        {
            using (DbConnection dbConnection = CreateConnection(_factory, _connectionString))
            {
                using (DbCommand dbCommand = CreateCommand(dbConnection, command))
                {
                    dbConnection.Open();
                    return dbCommand.ExecuteNonQuery();
                }
            }
        }



        public object ExecuteScalar(Command command)
        {
            using (DbConnection dbConnection = CreateConnection(_factory, _connectionString))
            {
                using (DbCommand dbCommand = CreateCommand(dbConnection, command))
                {
                    dbConnection.Open();
                    object result = dbCommand.ExecuteNonQuery();
                    return result is DBNull ? null : result;
                }
            }
        }

        public IEnumerable<TResult> ExecuteReader<TResult>(Command command, Func<IDataRecord, TResult> selector, bool executeImmediately = false)
        {
            IEnumerable<TResult> results = ExecuteReader(command, selector);
            return executeImmediately ? results.ToList() : results;
        }

        private IEnumerable<TResult> ExecuteReader<TResult>(Command command, Func<IDataRecord, TResult> selector)
        {
            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            using (DbConnection dbConnection = CreateConnection(_factory, _connectionString))
            {
                using (DbCommand dbCommand = CreateCommand(dbConnection, command))
                {
                    dbConnection.Open();
                    using (IDataReader dataReader = dbCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            yield return selector(dataReader);
                        }
                    }
                }
            }
        }

        private static DbCommand CreateCommand(DbConnection dbConnection, Command command)
        {
            DbCommand dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText = command.Query;

            if (command.IsStoredProcedure)
                dbCommand.CommandType = CommandType.StoredProcedure;

            foreach (KeyValuePair<string, object> kvp in command.Parameters)
            {
                DbParameter dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = kvp.Key;
                dbParameter.Value = kvp.Value;

                dbCommand.Parameters.Add(dbParameter);
            }

            return dbCommand;
        }

        private static DbConnection CreateConnection(DbProviderFactory factory, string connectionString)
        {
            DbConnection dbConnection = factory.CreateConnection();
            dbConnection.ConnectionString = connectionString;

            return dbConnection;
        }
    }
}
