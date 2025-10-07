using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccessLayer
{
    public class clsTestAppointmentDataAccess
    {

        public static int AddNewTestAppointment
            (int TestTypeID, int LocalDrivingLicenseApplicationID, DateTime AppointmentDate,
            decimal PaidFees,int UserID,bool IsLocked,int RetakeTestApplicationID)
        {
            int TestAppointmentID = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"INSERT Into TestAppointments 
                            (TestTypeID,LocalDrivingLicenseApplicationID,AppointmentDate,PaidFees,
                            CreatedByUserID,IsLocked,RetakeTestApplicationID)
                            VALUES(@TestTypeID,@LocalDrivingLicenseApplicationID,@AppointmentDate,@PaidFees,
                            @CreatedByUserID,@IsLocked,@RetakeTestApplicationID);
                             SELECT SCOPE_IDENTITY();";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
            command.Parameters.AddWithValue("@PaidFees", PaidFees);
            command.Parameters.AddWithValue("@IsLocked", IsLocked);
            command.Parameters.AddWithValue("@CreatedByUserID", UserID);
            if(RetakeTestApplicationID != -1 )
            command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID);
            else
            {
                command.Parameters.AddWithValue("@RetakeTestApplicationID", System.DBNull.Value);
            }

                try
                {
                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int insertedID))
                    {
                        TestAppointmentID = insertedID;
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
            return TestAppointmentID;

        }
        public static bool UpdateTestAppointment
            (int TestAppointmentID, int TestTypeID, int LocalDrivingLicenseApplicationID, DateTime AppointmentDate,
            decimal PaidFees, int UserID, bool isLocked, int RetakeTestApplicationID)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"Update TestAppointments
                            Set TestTypeID=@TestTypeID,
                                LocalDrivingLicenseApplicationID=@LocalDrivingLicenseApplicationID,
                                AppointmentDate=@AppointmentDate,
                                PaidFees=@PaidFees,
                                CreatedByUserID=@CreatedByUserID,
                                isLocked=@isLocked,
                                RetakeTestApplicationID=@RetakeTestApplicationID
                                Where TestAppointmentID=@TestAppointmentID;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
            command.Parameters.AddWithValue("@PaidFees", PaidFees);
            command.Parameters.AddWithValue("@CreatedByUserID", UserID);
            command.Parameters.AddWithValue("@isLocked", isLocked);
            if (RetakeTestApplicationID != -1)
                command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID);
            else
            {
                command.Parameters.AddWithValue("@RetakeTestApplicationID", System.DBNull.Value);
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
        public static bool DeleteTestAppointment(int TestAppointmentID)
        {
            int rowsAffected = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"Delete From TestAppointments
                          Where TestAppointmentID=@TestAppointmentID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
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
        public static DataTable GetAllTestAppointments()
        {
            DataTable table = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"SELECT * FROM TestAppointments_View
                             order by AppointmentDate Desc;";
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
        public static DataTable GetAllTestAppointmentsPerTestType(int localDrivingLicenseApplicationID,int testTypeID)
        {
            DataTable table = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"SELECT TestAppointmentID,AppointmentDate,PaidFees,IsLocked
                             FROM TestAppointments
                             Where LocalDrivingLicenseApplicationID=@LocalDrivingLicenseApplicationID 
                             AND TestTypeID=@TestTypeID
                                 Order by TestAppointmentID Desc;";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", localDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@TestTypeID", testTypeID);

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

        public static bool GetTestAppointmentInfoByID
            (int TestAppointmentID,ref int TestTypeID,ref int LocalDrivingLicenseApplicationID,ref DateTime AppointmentDate,
            ref decimal PaidFees,ref int UserID,ref bool isLocked,ref int RetakeTestApplicationID)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "SELECT * FROM TestAppointments WHERE TestAppointmentID = @TestAppointmentID;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    TestTypeID = Convert.ToInt32(reader["TestTypeID"]);
                    LocalDrivingLicenseApplicationID = Convert.ToInt32(reader["LocalDrivingLicenseApplicationID"]);
                    AppointmentDate = Convert.ToDateTime(reader["AppointmentDate"]);
                    PaidFees = Convert.ToDecimal(reader["PaidFees"]);
                    UserID = Convert.ToInt32(reader["CreatedByUserID"]);
                    isLocked = Convert.ToBoolean(reader["isLocked"]);
                    if(reader["RetakeTestApplicationID"]!=System.DBNull.Value)
                    {
                        RetakeTestApplicationID = Convert.ToInt32(reader["RetakeTestApplicationID"]);
                    }
                    else
                    {
                        RetakeTestApplicationID = -1;
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
        public static bool GetTestAppointmentInfoByLocalDrivingLicenseApplicationID
            (ref int TestAppointmentID, ref int TestTypeID,  int LocalDrivingLicenseApplicationID, ref DateTime AppointmentDate,
            ref decimal PaidFees, ref int UserID, ref bool isLocked, ref int RetakeTestApplicationID)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "SELECT * FROM TestAppointments WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    TestAppointmentID = Convert.ToInt32(reader["TestAppointmentID"]);
                    TestTypeID = Convert.ToInt32(reader["TestTypeID"]);
                    AppointmentDate = Convert.ToDateTime(reader["AppointmentDate"]);
                    PaidFees = Convert.ToDecimal(reader["PaidFees"]);
                    UserID = Convert.ToInt32(reader["CreatedByUserID"]);
                    isLocked = Convert.ToBoolean(reader["isLocked"]);
                    if (reader["RetakeTestApplicationID"] != System.DBNull.Value)
                    {
                        RetakeTestApplicationID = Convert.ToInt32(reader["RetakeTestApplicationID"]);
                    }
                    else
                    {
                        RetakeTestApplicationID = -1;
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


    }
}
