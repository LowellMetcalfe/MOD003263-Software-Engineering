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
        private static SqlCommand sqlCommand;


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
            int pID;

            // Open the DB Connection
            OpenConnection();
            
            // Switch statement based on what is in data[0]
            switch(data[0])
            {
                case "findPatient":

                    // Switch based on data[1] which is ID or name and DOB
                    switch (data[1])
                    {
                        case "id":
                            // Try to parse data[2] (ID) to an int32 and output as pID
                            Int32.TryParse(data[2], out pID);
                            // Instantiate an sqlCommand on the DBConnection
                            sqlCommand = new SqlCommand("SELECT * FROM patients WHERE NationalHealthNumber = @id", DBConnection);
                            // add parameters to the sql command (Prevents again SQLI)
                            sqlCommand.Parameters.AddWithValue("@id", pID);
                            break;
                        case "name&dob":
                            Console.WriteLine(data[3]);
                            DateTime dateOfBirth = DateTime.Parse(data[3]);

                            string name = data[2];
                            sqlCommand = new SqlCommand("SELECT * FROM patients WHERE name = @name AND DOB = @DOB", DBConnection);
                            sqlCommand.Parameters.AddWithValue("@name", name);
                            sqlCommand.Parameters.AddWithValue("@DOB", dateOfBirth);
                            break;
                        default:
                            //dataSet = null;
                            break;
                    }
                    // Add the value of the sqlCommand to the sqlDataAdapter
                    dataAdapter = new SqlDataAdapter(sqlCommand);
                    break;
                case "patientAppointments":
                    // Try to parse data[1] to an int32 and output as pID
                    Int32.TryParse(data[1], out pID);
                    // Instantiate an sqlCommand on the DBConnection
                    sqlCommand = new SqlCommand("SELECT * FROM appointment WHERE NationalHealthNumber = @id", DBConnection);
                    // add parameters to the sql command (Prevents again SQLI)
                    sqlCommand.Parameters.AddWithValue("@id", pID);
                    // Add the value of the sqlCommand to the sqlDataAdapter
                    dataAdapter = new SqlDataAdapter(sqlCommand);
                    break;
                // TODO:  Implement this case (TestResults)
                case "testResults":
                    // Try to parse data[1] to an int32 and output as pID
                    Int32.TryParse(data[1], out pID);
                    // Instantiate an sqlCommand on the DBConnection
                    sqlCommand = new SqlCommand("SELECT * FROM testresults WHERE NationalHealthNumber = @id", DBConnection);
                    // add parameters to the sql command (Prevents again SQLI)
                    sqlCommand.Parameters.AddWithValue("@id", pID);
                    // Add the value of the sqlCommand to the sqlDataAdapter
                    dataAdapter = new SqlDataAdapter(sqlCommand);
                    break;
                // TODO:  Implement this case (PatientPresciptions)
                case "patientPresciptions":
                    // Try to parse data[1] to an int32 and output as pID
                    Int32.TryParse(data[1], out pID);
                    // Instantiate an sqlCommand on the DBConnection
                    sqlCommand = new SqlCommand("SELECT * FROM prescriptions WHERE NationalHealthNumber = @id", DBConnection);
                    // add parameters to the sql command (Prevents again SQLI)
                    sqlCommand.Parameters.AddWithValue("@id", pID);
                    // Add the value of the sqlCommand to the sqlDataAdapter
                    dataAdapter = new SqlDataAdapter(sqlCommand);
                    break;
                // TODO:  Implement this case (Availability)
                case "availability":
                    // Try to parse data[1] to an int32 and output as pID
                    Int32.TryParse(data[1], out pID);
                    // Instantiate an sqlCommand on the DBConnection
                    sqlCommand = new SqlCommand("SELECT * FROM availability WHERE NationalHealthNumber = @id", DBConnection);
                    // add parameters to the sql command (Prevents again SQLI)
                    sqlCommand.Parameters.AddWithValue("@id", pID);
                    // Add the value of the sqlCommand to the sqlDataAdapter
                    dataAdapter = new SqlDataAdapter(sqlCommand);
                    break;
                // TODO: Implement this case (Duty
                case "duty":
                    // Try to parse data[1] to an int32 and output as pID
                    Int32.TryParse(data[1], out pID);
                    // Instantiate an sqlCommand on the DBConnection
                    sqlCommand = new SqlCommand("SELECT * FROM duty WHERE NationalHealthNumber = @id", DBConnection);
                    // add parameters to the sql command (Prevents again SQLI)
                    sqlCommand.Parameters.AddWithValue("@id", pID);
                    // Add the value of the sqlCommand to the sqlDataAdapter
                    dataAdapter = new SqlDataAdapter(sqlCommand);
                    break;
                case "prescriptionDuration":
                    int prescriptionID;
                    // Try to parse data[1] to an int32 and output as prescriptionID
                    Int32.TryParse(data[1], out prescriptionID);
                    // Instantiate an sqlCommand on the DBConnection
                    sqlCommand = new SqlCommand("SELECT duration FROM prescriptions WHERE prescriptionID = @id", DBConnection);
                    // add parameters to the sql command (Prevents again SQLI)
                    sqlCommand.Parameters.AddWithValue("@id", prescriptionID);
                    // Add the value of the sqlCommand to the sqlDataAdapter
                    dataAdapter = new SqlDataAdapter(sqlCommand);
                    break;
                // TODO: Figure out how to defualt this
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
            int count;
            DataSet dataSet;

            dataSet = null;
            dataAdapter = null;
            OpenConnection();

            switch (data[0])
            {
                case "registerPatient":
                    // Instantiate an sqlCommand on the DBConnection
                    // TODO: Concat Address
                    SqlCommand sqlCommand = new SqlCommand("INSERT INTO patients (NationalHealthNumber, Name, Title, DOB, PhoneNumber, Address, Diabetes, Smoker, Asthma, Allergies) " +
                        "VALUES (@NHNumber, @name, @title, @DOB, @phoneNumber, @address, @diabetes, @smoker, @asthma, @allergies)", DBConnection);
                    // add parameters to the sql command (Prevents again SQLI)
                    sqlCommand.Parameters.AddWithValue("@NHNumber", data[1]);
                    sqlCommand.Parameters.AddWithValue("@Name", data[2]);
                    sqlCommand.Parameters.AddWithValue("@Title", data[3]);
                    
                    sqlCommand.Parameters.AddWithValue("@DOB", DateTime.Parse(data[4]));
                    sqlCommand.Parameters.AddWithValue("@PhoneNumber", data[5]);
                    sqlCommand.Parameters.AddWithValue("@Address", data[6]);
                    sqlCommand.Parameters.AddWithValue("@Allergies", data[7]);
                    sqlCommand.Parameters.AddWithValue("@Diabetes", data[8]);
                    sqlCommand.Parameters.AddWithValue("@Smoker", data[9]);
                    sqlCommand.Parameters.AddWithValue("@Asthma", data[10]);

                    count = sqlCommand.ExecuteNonQuery();

                    if(count > 0)
                    {
                        RegisterForm.showMessage("Success!", "Patient was added successfully!");
                    } else
                    {
                        RegisterForm.showMessage("Error!", "There was an error and the patient was not added.");
                    }
                    count = 0;
                    break;
                case "newAppointment":
                    sqlCommand = new SqlCommand("INSERT INTO appointment (AppointmentID, EmployeeID, NationalHealthNumber, Date, Time, Description" +
                        "Status) VALUES (@AppointmentID, @EmployeeID, @NHNumber, @Date, @Time, @Description, @Status)", DBConnection);
                    // Add params to the above SQL query (Prevents against SQLI)
                    sqlCommand.Parameters.AddWithValue("@AppointmentID", data[1]);
                    sqlCommand.Parameters.AddWithValue("@EmployeeID", data[2]);
                    sqlCommand.Parameters.AddWithValue("@NHNumber", data[3]);
                    sqlCommand.Parameters.AddWithValue("@Date", data[4]);
                    sqlCommand.Parameters.AddWithValue("@Time", data[5]);
                    sqlCommand.Parameters.AddWithValue("@Description", data[6]);
                    sqlCommand.Parameters.AddWithValue("@Status", data[7]);

                    count = sqlCommand.ExecuteNonQuery();

                    if(count > 0)
                    {
                        RegisterForm.showMessage("Success!", "Patient was added successfully!");
                    } else
                    {
                        RegisterForm.showMessage("Error!", "There was an error and the patient was not added.");
                    }

                    count = 0;
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
