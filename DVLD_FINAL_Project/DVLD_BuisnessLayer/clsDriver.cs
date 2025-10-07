using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BuisnessLayer
{
    public class clsDriver
    {
        public enum Mode { AddNew = 0, Update = 1 };
        public Mode enMode = Mode.AddNew;
        public int DriverID { get; set; }
        public int PersonID { get; set; }
        public int UserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public clsPerson PersonInfo;
        
        private clsDriver(int driverID,int personID, int userID,DateTime createdDate)
        {
            enMode = Mode.Update;
            DriverID = driverID;
            UserID = userID;
            PersonID = personID;
            CreatedDate = createdDate;
            PersonInfo = clsPerson._GetPersonInfo(personID);
        }
        public clsDriver()
        {
            enMode = Mode.AddNew;
            DriverID = -1;
            UserID = -1;
            PersonID = -1;
            CreatedDate = DateTime.Now;
        }
        public static clsDriver _GetDriverInfoByDriverID(int DriverID)
        {
            int personID = -1,userID=-1;
            DateTime date = DateTime.Now;
            
            bool isFound = clsDriverDataAccess.GetDriverInfoByDriverID(DriverID, ref personID, ref userID, ref date);
            if (isFound)
            {
                return new clsDriver(DriverID,personID,userID,date);
            }
            else
            {
                return null;
            }

        }
        public static clsDriver _GetDriverInfoByPersonID(int personID)
        {
            int driverID = -1, userID = -1;
            DateTime date = DateTime.Now;

            bool isFound = clsDriverDataAccess.GetDriverInfoByPersonID(ref driverID,  personID, ref userID, ref date);
            if (isFound)
            {
                return new clsDriver(driverID, personID, userID, date);
            }
            else
            {
                return null;
            }

        }
       
        public static bool _DeleteDriver(int driverID)
        {
            return clsDriverDataAccess.DeleteDriver(driverID);
        }
        public static bool _IsDriverExistByDriver(int driverID)
        {
            return clsDriverDataAccess.IsDriverExistByDriverID(driverID);
        }
        public static bool _IsDriverExistByPerson(int personID)
        {
            return clsDriverDataAccess.IsDriverExistByPersonID(personID);
        }
        public static DataTable _GetAllDrivers()
        {
            return clsDriverDataAccess.GetAllDrivers();
        }
        public bool _AddNew()
        {
            this.DriverID = clsDriverDataAccess.AddNewDriver( this.PersonID, this.UserID);
            return this.DriverID != -1;

        }
        public bool _Update()
        {
            return clsDriverDataAccess.UpdateDriver(this.DriverID, this.PersonID, this.UserID, this.CreatedDate);


        }
        public bool Save()
        {
            switch (enMode)
            {
                case Mode.AddNew:
                    if (_AddNew())
                    {
                        enMode = Mode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case Mode.Update:
                    return _Update();
            }

            return false;
        }
    }
}
