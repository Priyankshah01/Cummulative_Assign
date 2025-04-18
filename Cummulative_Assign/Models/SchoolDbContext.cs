﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace Cummulative1_schooldb.Models
{
    public class SchoolDbContext
    {
        // Credentials for accessing the database
        private static string User { get { return "root"; } }       // Username
        private static string Password { get { return ""; } }   // Password
        private static string Database { get { return "cummulative_assign3"; } } // Database name
        private static string Server { get { return "localhost"; } } // Server address
        private static string Port { get { return "3306"; } }       // Port number

        // Connection string used to establish a connection to the database
        protected static string ConnectionString
        {
            get
            {
                return "server=" + Server
                    + ";user=" + User
                    + ";database=" + Database
                    + ";port=" + Port
                    + ";password=" + Password
                    + ";convert zero datetime=True";
            }
        }

        /// <summary>
        /// Returns a connection to the school database.
        /// </summary>
        /// <returns>A MySqlConnection object representing the connection to the database.</returns>
        public MySqlConnection AccessDatabase()
        {
            // Instantiate a MySqlConnection object with the connection string
            // This object represents a specific connection to the school database on localhost:3306
            return new MySqlConnection(ConnectionString);
        }
    }
}