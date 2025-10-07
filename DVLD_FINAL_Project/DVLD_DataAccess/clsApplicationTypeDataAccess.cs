using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccessLayer
{
    public static class clsApplicationTypeDataAccess
    {
        public static DataTable GetAllApplicationTypes()
        {
            DataTable table=new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"SELECT * FROM ApplicationTypes";
            SqlCommand command= new SqlCommand(query, connection);
            try
            {
                connection.Open();
                SqlDataReader reader=command.ExecuteReader();
                if(reader.HasRows)
                {
                    table.Load(reader);
                }
                reader.Close();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return table;

        }
        public static bool GetApplicationTypeByApplicationTypeID(int ApplicationTypeID,ref string ApplicationTypeTitle,ref decimal ApplicationFees)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"SELECT * FROM ApplicationTypes
                             WHERE ApplicationTypeID=@ApplicationTypeID;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            try
            {
                connection.Open();
                SqlDataReader reader=command.ExecuteReader();
                if( reader.Read())
                {
                    isFound = true;
                    ApplicationTypeTitle = Convert.ToString(reader["ApplicationTypeTitle"]);
                    ApplicationFees = Convert.ToDecimal(reader["ApplicationFees"]);
                }
                else
                {
                    isFound = false;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                isFound=false;
            }
            finally
            {
                connection.Close();
            }
            return isFound;
        }
        public static bool GetApplicationTypeByApplicationTypeTitle(ref int ApplicationTypeID, string ApplicationTypeTitle, ref decimal ApplicationFees)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"SELECT * FROM ApplicationTypes
                             WHERE ApplicationTypeTitle=@ApplicationTypeTitle;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicationTypeTitle", ApplicationTypeTitle);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    isFound = true;
                    ApplicationTypeID = Convert.ToInt32(reader["ApplicationTypeID"]);
                    ApplicationFees = Convert.ToDecimal(reader["ApplicationFees"]);
                }
                else
                {
                    isFound = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }
            return isFound;
        }
        public static bool UpdateApplication(int ApplicationTypeID, decimal ApplicationFees, string ApplicationTypeTitle)
        {
            int rowsAffected = -1;
            SqlConnection connection=new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"Update ApplicationTypes 
                            Set ApplicationFees=@ApplicationFees,
                                ApplicationTypeTitle=@ApplicationTypeTitle
                             WHERE ApplicationTypeID=@ApplicationTypeID;";
            SqlCommand command=new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            command.Parameters.AddWithValue("@ApplicationFees", ApplicationFees);
            command.Parameters.AddWithValue("@ApplicationTypeTitle", ApplicationTypeTitle);
            try
            {
                connection.Open();
                rowsAffected=command.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return rowsAffected > 0;

        }
        
    }
}
