using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace DVLD_DataAccessLayer
{
    public class clsTestDataAccess
    {
        public static int AddNewTest
            (int TestAppointmentID, bool TestResult, string Notes,int UserID)
        {
            int TestID = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"INSERT Into Tests 
                            (TestAppointmentID,TestResult,Notes,CreatedByUserID)
                            VALUES(@TestAppointmentID,@TestResult,@Notes,@CreatedByUserID);
                             UPDATE TestAppointments 
                                SET IsLocked=1 where TestAppointmentID = @TestAppointmentID;
                             SELECT SCOPE_IDENTITY();";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            command.Parameters.AddWithValue("@TestResult", TestResult);
            command.Parameters.AddWithValue("@CreatedByUserID", UserID);
            if (Notes != "" && Notes !=null)
                command.Parameters.AddWithValue("@Notes", Notes);
            else
            {
                command.Parameters.AddWithValue("@Notes", System.DBNull.Value);
            }

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    TestID = insertedID;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return TestID;



        }
        public static bool UpdateTest
           (int TestID,int TestAppointmentID, bool TestResult, string Notes, int UserID)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"Update Tests
                            Set TestAppointmentID=@TestAppointmentID,
                                TestResult=@TestResult,
                                Notes=@Notes,
                                CreatedByUserID=@CreatedByUserID,
                                Where TestAppointmentID=@TestAppointmentID;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestID", TestID);
            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            command.Parameters.AddWithValue("@TestResult", TestResult);
            command.Parameters.AddWithValue("@CreatedByUserID", UserID);
            if (Notes != "" && Notes != null)
                command.Parameters.AddWithValue("@Notes", Notes);
            else
            {
                command.Parameters.AddWithValue("@Notes", System.DBNull.Value);
            }
            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);

            }

            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }
        public static bool DeleteTest(int TestID)
        {
            int rowsAffected = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"Delete From Tests
                          Where TestID=@TestID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestID", TestID);
            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return rowsAffected > 0;
        }
        public static DataTable GetAllTests()
        {
            DataTable table = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"SELECT * FROM Tests
                             Order by TestID;";
            SqlCommand command = new SqlCommand(query, connection);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    table.Load(reader);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return table;
        }
        public static bool GetTestInfoByID
            (int TestID, ref int TestAppointmentID,ref bool TestResult,ref string Notes,ref int UserID)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "SELECT * FROM Tests WHERE TestID = @TestID;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestID", TestID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    
                    TestAppointmentID = Convert.ToInt32(reader["TestAppointmentID"]);
                    TestResult = Convert.ToBoolean(reader["TestResult"]);
                    UserID = Convert.ToInt32(reader["CreatedByUserID"]);
                   
                    if (reader["Notes"] != System.DBNull.Value)
                    {
                        Notes = Convert.ToString(reader["Notes"]);
                    }
                    else
                    {
                        Notes ="";
                    }
                    isFound = true;
                }
                else
                {
                    isFound = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }
            return isFound;
        }
        public static int GetPassedTestCount(int LocalDrivingLicenseApplicationID)
        {
            int PassedTestCount = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"SELECT PassedTestCount=count(LocalDrivingLicenseApplicationID) 
                            FROM Tests Join TestAppointments 
                            ON Tests.TestAppointmentID = TestAppointments.TestAppointmentID
                            Where LocalDrivingLicenseApplicationID=@LocalDrivingLicenseApplicationID AND TestResult=1;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int passedTestCount))
                {
                    PassedTestCount = passedTestCount;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return PassedTestCount;
        }
        public static int GetNumberOfTrials(int LocalDrivingLicenseApplicationID,int testTypeID)
        {
            int PassedTestCount = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"SELECT NumberOfTrial=count(LocalDrivingLicenseApplicationID) 
                            FROM Tests Join TestAppointments 
                            ON Tests.TestAppointmentID = TestAppointments.TestAppointmentID
                            Where LocalDrivingLicenseApplicationID=@LocalDrivingLicenseApplicationID AND
                            TestTypeID=@TestTypeID;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@TestTypeID", testTypeID);
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int passedTestCount))
                {
                    PassedTestCount = passedTestCount;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return PassedTestCount;
        }
        public static int GetTestID(int TestAppointmentID)
        {
            int TestID = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "SELECT Tests.TestID From Tests Where TestAppointmentID=@TestAppointmentID;";
                           
                           
                           
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int testID))
                {
                    TestID = testID;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return TestID;
        }
        public static bool DoesPassTest(int LocalDrivingLicenseApplicationID,int testTypeID)
        {
            bool isFound=false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"SELECT Found=1 
                            FROM TestAppointments Join Tests
                            ON TestAppointments.TestAppointmentID=Tests.TestAppointmentID
                            where LocalDrivingLicenseApplicationID=@LocalDrivingLicenseApplicationID
                            AND TestResult=1 AND TestTypeID=@testTypeID;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@testTypeID", testTypeID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    isFound = true;
                }
                reader.Close();
            }
            catch(Exception ex)
            {
                isFound = false;
            }
            finally
            {
                connection.Close();
            }
            return isFound;
        }
        public static bool DoesMakeTest(int LocalDrivingLicenseApplicationID, int testTypeID)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"SELECT Found=1 
                            FROM TestAppointments Join Tests
                            ON TestAppointments.TestAppointmentID=Tests.TestAppointmentID
                            where LocalDrivingLicenseApplicationID=@LocalDrivingLicenseApplicationID
                            AND TestTypeID=@testTypeID;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@testTypeID", testTypeID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    isFound = true;
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                isFound = false;
            }
            finally
            {
                connection.Close();
            }
            return isFound;
        }

    }
}
