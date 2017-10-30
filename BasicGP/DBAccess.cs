﻿// Author: Ryan Alderton
// SID: 1609275
using System;
using System.Data;
using System.Data.SqlClient;

namespace BasicGP
{
    class DBAccess
    {
        private static string conStr = Properties.Settings.Default.OverGPDBConnectionString;
        private static SqlConnection DBConnection;
        private static SqlDataAdapter dataAdapter;


        /// <summary>
        /// Opens the connection based on the string passed in DatabaseConnection
        /// </summary>
        public static void OpenConnection()
        {
            DBConnection = new SqlConnection(conStr);
            DBConnection.Open();
        }

        /// <summary>
        /// Closes the DB Connection
        /// </summary>
        public static void CloseConnection()
        {
            DBConnection.Close();
        }
        
        /// <summary>
        /// Get the data generated by the SQL Statment
        /// </summary>
        /// <param name="sqlQuery">The SQL query to passed in</param>
        /// <returns></returns>
        public static DataSet CheckLogin(string username, string password)
        {
            DataSet dataSet;
            //Open a connection to the database inside DBAccess
            OpenConnection();
            //create the object dataAdapter to manipulate a table from the database specified by DBConnection
            dataAdapter = new SqlDataAdapter($"SELECT * FROM users WHERE userName = '{username}' AND password = '{password}'", DBConnection);
            //Creat the dataSet
            dataSet = new DataSet();
            dataAdapter.Fill(dataSet);
            //close the DB Connections
            CloseConnection();

            return dataSet;
        }

        /// <summary>
        /// Gets specified data from server
        /// </summary>
        public static DataSet getData(String[] args)
        {
            DataSet dataSet;

            OpenConnection();
            
            switch(args[0])
            {
                case "findPatient":
                    //dataAdapter = new SqlDataAdapter($"SELECT * FROM patients WHERE patientID = '{args[1]}'", DBConnection);
                    Console.WriteLine(args[0]);
                    break;
                case "patientAppointments":
                    break;
                case "testResults":
                    break;
                case "patientPresciptions":
                    break;
                case "availability":
                    break;
                case "duty":
                    break;
                default:
                    dataSet = null;
                    break;
            }

            dataSet = new DataSet();
            dataAdapter.Fill(dataSet);

            return dataSet;
        }

    }
}
