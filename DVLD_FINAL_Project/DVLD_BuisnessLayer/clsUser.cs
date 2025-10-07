using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_DataAccessLayer;
using System.Data;

namespace DVLD_BuisnessLayer
{
    public class clsUser
    {
        public enum Mode {AddNew=0,Update=1};
        public Mode enMode=Mode.AddNew;
        public int UserID {  get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public short IsActive { get; set; }
        public int PersonID { get; set; }
        public clsPerson Person;
        private clsUser( int userID, string userName, string password, short isActive, int personID)
        {
            enMode=Mode.Update;
            UserID = userID;
            UserName = userName;
            Password = password;
            IsActive = isActive;
            PersonID = personID;
            Person = clsPerson._GetPersonInfo(personID);
            
        }
        public clsUser()
        {
            enMode=Mode.AddNew;
            UserID = -1;
            UserName = "";
            Password = "";
            IsActive = -1;
            PersonID = -1;
        }
        public static clsUser _GetUserInfoBy(int UserID)
        {
            string UserName = "", Password = "";
            int personID = -1;
            short IsActive = -1;
            bool isFound=clsUserDataAccess.GetUserInfoByUserID(UserID,ref personID,ref UserName,ref Password,ref IsActive);
            if(isFound)
            {
                return new clsUser(UserID,UserName,Password, IsActive, personID);
            }
            else
            {
                return null;
            }

        }
        public static clsUser _GetUserInfoBy(string UserName)
        {
            string Password = "";
            int personID = -1,UserID=-1;
            short IsActive = -1;
            bool isFound = clsUserDataAccess.GetUserInfoByUserName( UserName, ref personID,ref UserID, ref Password, ref IsActive);
            if (isFound)
            {
                return new clsUser(UserID, UserName, Password, IsActive, personID);
            }
            else
            {
                return null;
            }

        }
        public static clsUser _GetUserInfoBy(string UserName, string Password)
        {
            
            int personID = -1, UserID = -1;
            short IsActive = -1;
            bool isFound = clsUserDataAccess.GetUserInfoByUserNameAndPassword(UserName, ref personID, ref UserID,  Password, ref IsActive);
            if (isFound)
            {
                return new clsUser(UserID, UserName, Password, IsActive, personID);
            }
            else
            {
                return null;
            }

        }
        public static bool _DeleteUser(int UserID)
        {
            return clsUserDataAccess.DeleteUser(UserID);
        }
        public static bool _IsUserExist(int UserID)
        {
            return clsUserDataAccess.IsUserExist(UserID);
        }
        public static bool _IsUserExist(string UserName)
        {
            return clsUserDataAccess.IsUserExist(UserName);
        }
        //public static bool _IsUserExist(string UserName,string Password)
        //{
        //    return clsUserDataAccess.IsUserExist(UserName,Password);
        //}
        public static bool _IsUserActive(string UserName)
        {
            return clsUserDataAccess.IsUserActive(UserName);
        }

        public bool _ChangePassword(string UserName,string Password)
        {
            return clsUserDataAccess.ChangePassword(UserName, Password);
        }
        public static bool _IsUserExistForPersonID(int PersonID)
        {
            return clsUserDataAccess.IsUserExistForPersonID(PersonID);
        }
        public static DataTable _GetAllUsers()
        {
            return clsUserDataAccess.GetAllUsers();
        }
        public bool _AddNew()
        {
            this.UserID=clsUserDataAccess.AddUser( this.PersonID, this.UserName,this.Password, this.IsActive);
            return this.UserID != -1;
            
        }
        public bool _Update()
        {
            return clsUserDataAccess.UpdateUser(this.UserID,this.PersonID, this.UserName, this.Password, this.IsActive);
        }
        public bool Save()
        {
            switch(enMode)
            {
                case Mode.AddNew:
                    if(_AddNew())
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
