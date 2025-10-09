using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DVLD_DataAccessLayerLastVersion
{
    public class clsInternationalDrivingLicenseApplicationDataAccess
    {
        public static int AddNewInternationalLicense(int ApplicationID, int DrivierID, int LicenseID,DateTime IssueDate,DateTime ExpirationDate,bool isActive,int UserID)
        {
            int InterNationalDrivingLicenseApplicationID = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"Update InternationalLicenses
                             Set IsActive=0
                             where DriverID=@DriverID;

                            INSERT INTO InternationalLicenses 
                            (ApplicationID,DriverID,IssuedUsingLocalLicenseID,IssueDate,ExpirationDate,IsActive,CreatedByUserID)
                             VALUES (@ApplicationID, @DriverID,@IssuedUsingLocalLicenseID
                                    ,@IssueDate,@ExpirationDate,@IsActive,@CreatedByUserID);
                             SELECT SCOPE_IDENTITY();";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@DriverID", DrivierID);
            command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", LicenseID);
            command.Parameters.AddWithValue("@IssueDate", IssueDate);
            command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
            command.Parameters.AddWithValue("@IsActive", isActive);
            command.Parameters.AddWithValue("@CreatedByUserID", UserID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    InterNationalDrivingLicenseApplicationID = insertedID;
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
            return InterNationalDrivingLicenseApplicationID;
        }
        public static bool UpdateInternationalLicense(int InterNationalDrivingLicenseApplicationID, int ApplicationID, int DrivierID, int LicenseID, DateTime IssueDate, DateTime ExpirationDate, bool isActive, int UserID)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"Update InternationalLicenses
                            Set ApplicationID=@ApplicationID,
                                DrivierID=@DrivierID,
                                IssuedUsingLocalLicenseID=@IssuedUsingLocalLicenseID,
                                IssueDate=@IssueDate,
                                ExpirationDate=@ExpirationDate,
                                IsActive=@IsActive,
                                CreatedByUserID=@CreatedByUserID
                                Where InternationalLicenseID=@InternationalLicenseID;";
            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@InternationalLicenseID", InterNationalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@DrivierID", DrivierID);
            command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", LicenseID);
            command.Parameters.AddWithValue("@IssueDate", IssueDate);
            command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
            command.Parameters.AddWithValue("@IsActive", isActive);
            command.Parameters.AddWithValue("@CreatedByUserID", UserID);
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
        public static bool DeleteInternationalLicense(int InterNationalDrivingLicenseApplicationID)
        {
            int rowsAffected = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"Delete From InternationalLicenses
                          Where InternationalLicenseID=@InternationalLicenseID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@InternationalLicenseID", InterNationalDrivingLicenseApplicationID);
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
        public static DataTable GetAllInternationalDrivingLicenseApplications()
        {
            DataTable table = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"SELECT    InternationalLicenseID, ApplicationID,DriverID,
		                IssuedUsingLocalLicenseID , IssueDate, 
                        ExpirationDate, IsActive
		                 from InternationalLicenses 
                         order by IsActive, ExpirationDate desc";
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
        public static DataTable GetDriverInternationalDrivingLicenseApplications(int DriverID)
        {
            DataTable table = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"SELECT    InternationalLicenseID, ApplicationID,
		                IssuedUsingLocalLicenseID , IssueDate, 
                        ExpirationDate, IsActive
		                 from InternationalLicenses 
                         Where DriverID=@DriverID
                         order by ExpirationDate desc";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DriverID", DriverID);
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

        public static bool GetInternationalDrivingLicenseApplicatioInfoByID
            (int InterNationalDrivingLicenseApplicationID,ref int ApplicationID,ref int DrivierID,
           ref int LicenseID,ref DateTime IssueDate,ref DateTime ExpirationDate,ref bool isActive,ref int UserID)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "SELECT * FROM InternationalLicenses WHERE InternationalLicenseID = @InternationalLicenseID;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@InternationalLicenseID", InterNationalDrivingLicenseApplicationID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    ApplicationID = Convert.ToInt32(reader["ApplicationID"]);
                    DrivierID = Convert.ToInt32(reader["DriverID"]);
                    LicenseID = Convert.ToInt32(reader["IssuedUsingLocalLicenseID"]);
                    IssueDate = Convert.ToDateTime(reader["IssueDate"]);
                    ExpirationDate = Convert.ToDateTime(reader["ExpirationDate"]);
                    isActive = Convert.ToBoolean(reader["IsActive"]);
                    UserID = Convert.ToInt32(reader["CreatedByUserID"]);
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
         public static int IsInternationalDrivingLicenseExist(int DriverID)
        {
            int InterNationalDrivingLicenseApplicationID = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"SELECT InternationalLicenseID FROM InternationalLicenses
                           Where DriverID=@DriverID AND GETDATE() between IssueDate and ExpirationDate;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("DriverID", @DriverID);
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    InterNationalDrivingLicenseApplicationID = insertedID;
                }
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                connection.Close();
            }
            return InterNationalDrivingLicenseApplicationID;
        }





    }
}
