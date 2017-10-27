﻿// Author: Ryan Alderton
// SID: 1609275
using System;
using System.Data;

namespace BasicGP
{
    class DBAccess
    {
        private string connectionString;
        System.Data.SqlClient.SqlConnection DBConnection;
        private System.Data.SqlClient.SqlDataAdapter dataAdapter;


        //TODO: Fix this error - I'm not sure why it's broken (Issue #
        //public DatabaseConnection(string connectionString)
        //{
        //    this.connectionString = connectionString;
        //}

        /// <summary>
        /// Opens the connection based on the string passed in DatabaseConnection
        /// </summary>
        public void OpenConnection()
        {
            DBConnection = new System.Data.SqlClient.SqlConnection(connectionString);
            DBConnection.Open();
        }

        /// <summary>
        /// Closes the DB Connection
        /// </summary>
        public void closeConnection()
        {
            DBConnection.Close();
        }

        /// <summary>
        /// Get the data generated by the SQL Statment
        /// </summary>
        /// <param name="sqlQuery">The SQL query to passed in</param>
        /// <returns></returns>
        public DataSet GetDataSet(string sqlQuery)
        {
            DataSet dataSet;

            //create the object dataAdapter to manipulate a table from the database specified by DBConnection
            dataAdapter = new System.Data.SqlClient.SqlDataAdapter(sqlQuery, DBConnection);
            //Creat the dataSet
            dataSet = new DataSet();
            dataAdapter.Fill(dataSet);

            return dataSet;
        }

    }
}