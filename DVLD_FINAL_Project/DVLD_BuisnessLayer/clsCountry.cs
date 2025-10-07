using DVLD_DataAccessLayer;
using System;
using System.Data;


namespace DVLD_BuisnessLayer
{
    public class clsCountry
    {
        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public clsCountry()
        {
            CountryID = -1;
            CountryName = "";
        }
        private clsCountry(int CountryID, string CountryName)
        {
            this.CountryID = CountryID;
            this.CountryName = CountryName;
        }
        public static clsCountry _GetCountryInfoBy(int CountryID)
        {
            string CountryName = "";
            bool isFound = clsCountryDataAccess.GetCountryInfoByID(CountryID, ref CountryName);
            if (isFound)
            {
                return new clsCountry(CountryID, CountryName);
            }
            else
            {
                return null;
            }
        }
        public static clsCountry _GetCountryInfoBy(string CountryName)
        {
            int CountryID = -1;
            bool isFound = clsCountryDataAccess.GetCountryInfoByName(ref CountryID, CountryName);
            if (isFound)
            {
                return new clsCountry(CountryID, CountryName);
            }
            else
            {
                return null;
            }
        }
        public static DataTable _GetAllCountries()
        {
            return clsCountryDataAccess.GetAllCountries();
        }
        public static bool _IsCountryExist(int CountryID)
        {
            return clsCountryDataAccess.IsCountryExist(CountryID);
        }
        public static bool _IsCountryExist(string CountryName)
        {
            return clsCountryDataAccess.IsCountryExist(CountryName);
        }




    }
}
