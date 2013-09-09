using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using SimpleSqlServer.Models;

namespace SimpleSqlServer
{
    public class Client : IDisposable
    {
        private static Client _instance;
        private ConnectionInfo _connectionInfo;
        private static SqlConnection _connection;

        private Client(ConnectionInfo connectionInfo)
        {
            _connectionInfo = connectionInfo;
        }

        public static Client ConnectTo(ConnectionInfo connectionInfo)
        {
            if (_instance == null)
            {
                _instance = new Client(connectionInfo);
            }

            // Connect to the database
            // TODO: Handle exceptions
            _connection = _instance.Connect();
            _connection.Open();

            return _instance;
        }

        public static Client ConnectTo(string server, string username, string password, string database)
        {
            var connection = new ConnectionInfo
                                 {
                                     Server = server,
                                     Username = username,
                                     Password = password,
                                     Database = database
                                 };

            return ConnectTo(connection);
        }

        private SqlConnection Connect()
        {
            try
            {
                return new SqlConnection(Services.ClientHelper.GetConnectionString(_connectionInfo));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return null;
        }

        public void Disconnect()
        {
            try
            {
                _connection.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public void Dispose()
        {
            if (_connection != null && _connection.State != ConnectionState.Closed)
            {
                Disconnect();
            }
        }

        private static SqlCommand PrepareCommand(string query, string[] parameters)
        {
            var command = new SqlCommand(query, _connection) {CommandType = CommandType.Text};

            if (parameters != null && parameters.Length > 0)
            {
                foreach (string param in parameters)
                {
                    Parameter parameter = Services.ClientHelper.GetParameter(param);

                    if (parameter != null)
                    {
                        command.Parameters.AddWithValue(string.Format("@{0}", parameter.Name), parameter.Value);
                    }
                }
            }

            return command;
        }

        public object ExecuteScalar(string query, params string[] parameters)
        {
            var command = PrepareCommand(query, parameters);

            object result = command.ExecuteScalar();

            return result;
        }

        public int ExecuteNonQuery(string query, params string[] parameters)
        {
            var command = PrepareCommand(query, parameters);

            int affectedRows = command.ExecuteNonQuery();

            return affectedRows;
        }

        public DataTable RecordSet(string query, params string[] parameters)
        {
            var command = PrepareCommand(query, parameters);
            var adapter = new SqlDataAdapter(command);
            var dataTable = new DataTable();

            adapter.Fill(dataTable);

            return dataTable;
        }
    }
}
