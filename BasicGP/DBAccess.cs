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
        private static SqlDataAdapter loginDataAdapter;
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
            DataSet loginDataSet;
            //Open a connection to the database inside DBAccess
            OpenConnection();
            //create the object dataAdapter to manipulate a table from the database specified by DBConnection
            loginDataAdapter = new SqlDataAdapter($"SELECT * FROM users WHERE userName = '{username}' AND password = '{password}'", DBConnection);
            //Creat the dataSet
            loginDataSet = new DataSet();
            loginDataAdapter.Fill(loginDataSet);
            //close the DB Connections
            CloseConnection();

            return loginDataSet;
        }

        /// <summary>
        /// Fetches data from the server
        /// </summary>
        /// <param name="data">Data to be passed to the server - first element should always be the function</param>
        public static DataSet getData(params string[] data)
        {
            // Create a dataset called dataSet
            DataSet dataSet;
            // Clean the dataSet and the dataAdapter
            dataSet = null;
            dataAdapter = null;

            // Open the DB Connection
            OpenConnection();
            
            // Switch statement based on what is in data[0]
            switch(data[0])
            {
                case "findPatient":
                    // Try to parse data[1] to an int32 and output as pID
                    Int32.TryParse(data[1], out int pID);
                    // Instantiate an sqlCommand on the DBConnection
                    SqlCommand sqlCommand = new SqlCommand("SELECT * FROM patients WHERE NationalHealthNumber = @id", DBConnection);
                    // add parameters to the sql command (Prevents again SQLI)
                    sqlCommand.Parameters.AddWithValue("@id", pID);
                    // Add the value of the sqlCommand to the sqlDataAdapter
                    dataAdapter = new SqlDataAdapter(sqlCommand);
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
                    Console.WriteLine("default");
                    break;
            }

            // create a data set
            dataSet = new DataSet();
            // Fill the dataAdapter with the data from the dataSet
            dataAdapter.Fill(dataSet);
            // Close the DB Connection
            CloseConnection();
            // Return the dataset
            return dataSet;
        }

        /// <summary>
        /// Posts Data to server
        /// </summary>
        /// <param name="data">Data to be passed to the server - first element should always be the function</param>
        public static DataSet postData(params string[] data)
        {
            DataSet dataSet;

            dataSet = null;
            dataAdapter = null;
            OpenConnection();

            switch (data[0])
            {
                case "registerPatient":
                    // Instantiate an sqlCommand on the DBConnection
                    // TODO: Concat Address
                    SqlCommand sqlCommand = new SqlCommand("INSERT INTO patient (NationalHealthNumber, Name, Title, DOB, PhoneNumber, Address, Diabetes, Smoker, Asthma, Allergies) " +
                        "VALUES (@NHNumber, @name, @title, @DOB, @phoneNumber, @address, @diabetes, @smoker, @asthma, @allergies)", DBConnection);
                    // add parameters to the sql command (Prevents again SQLI)
                    sqlCommand.Parameters.AddWithValue("@NHNumber", data[1]);
                    sqlCommand.Parameters.AddWithValue("@Name", data[2]);
                    sqlCommand.Parameters.AddWithValue("@Title", data[3]);
                    sqlCommand.Parameters.AddWithValue("@DOB", data[4]);
                    sqlCommand.Parameters.AddWithValue("@PhoneNumber", data[5]);
                    sqlCommand.Parameters.AddWithValue("@Address", data[6]);
                    sqlCommand.Parameters.AddWithValue("@Diabetes", data[7]);
                    sqlCommand.Parameters.AddWithValue("@Smoker", data[8]);
                    sqlCommand.Parameters.AddWithValue("@Asthma", data[9]);
                    sqlCommand.Parameters.AddWithValue("@Allergies", data[10]);
                    // Add the value of the sqlCommand to the sqlDataAdapter
                    dataAdapter = new SqlDataAdapter(sqlCommand);
                    break;
                case "newAppointment":
                    // TODO: Create Appointments table and make this SQLCommand valid
                    //SqlCommand sqlCommand = new SqlCommand("INSERT INTO appointments (TABLEPARAMS) VALUES (VALUES)");
                    break;
                default:
                    dataSet = null;
                    Console.WriteLine("default");
                    break;
            }
            
            return dataSet;
        }

    }
}
