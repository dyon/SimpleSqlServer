using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using SimpleSqlServer.Models;

namespace SimpleSqlServer.Services
{
    public static class ClientHelper
    {
        public static string GetConnectionString(ConnectionInfo connectionInfo)
        {
            return string.Format("server={0};database={1};user id={2};password={3}",
                                 connectionInfo.Server, connectionInfo.Database, connectionInfo.Username, connectionInfo.Password);
        }

        public static Parameter GetParameter(string parameter)
        {
            // Check if parameter format is correct
            if (!Regex.IsMatch(parameter, @"^\w+:.*$"))
            {
                return null;
            }

            string[] keyValue = parameter.Split(new char[] { ':' }, 2);
            var result = new Parameter
                             {
                                 Name = keyValue[0],
                                 Value = keyValue[1]
                             };

            return result;
        }
    }
}
