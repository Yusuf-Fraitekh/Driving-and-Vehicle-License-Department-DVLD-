using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_DataAccessLayer;
using System.Data;
namespace DVLD_BuisnessLayer
{
    public class clsApplication
    {
        public enum enMode { AddNew=0,Update=1};
        public enum enApplicationStatus {New=1,Cancelled=2,Completed=3};
        public enum enApplicationType {NewDrivingLicense = 1, RenewDrivingLicense = 2, ReplaceLostDrivingLicense = 3,
        ReplaceDamagedDrivingLicense = 4, ReleaseDetainedDrivingLicsense = 5, NewInternationalLicense = 6, RetakeTest = 7 }
        public enMode Mode;
        public int ApplicationID { get; set; }
        public int ApplicantPersonID {  get; set; }
        public DateTime ApplicationDate {  get; set; }
        public int ApplicationTypeID {  get; set; }
        public enApplicationStatus ApplicationStatus {  get; set; }
        public DateTime LastStatus {  get; set; }
        public decimal PaidFees {  get; set; }
        public int UserID { get; set; }
        public clsUser CreatedByUserIDInfo;
        public clsApplicationType ApplicationTypeInfo;
        public clsPerson PersonInfo;
        public clsApplication()
        {
            ApplicationID = -1;
            ApplicantPersonID = -1;
            ApplicationDate = DateTime.Now;
            ApplicationTypeID = -1;
            ApplicationStatus= enApplicationStatus.New;
            LastStatus=DateTime.Now;
            PaidFees = 0m;
            UserID = -1;
            Mode = enMode.AddNew;
        }
    
         private clsApplication
            (int applicationID, int applicantPersonID, DateTime applicationDate, int applicationTypeID,
             enApplicationStatus applicationStatus, DateTime lastStatus, decimal paidFees, int userID)
         {
            Mode=enMode.Update;
            ApplicationID = applicationID;
            ApplicantPersonID = applicantPersonID;
            ApplicationDate = applicationDate;
            ApplicationTypeID = applicationTypeID;
            ApplicationStatus = applicationStatus;
            LastStatus = lastStatus;
            PaidFees = paidFees;
            UserID = userID;
            CreatedByUserIDInfo = clsUser._GetUserInfoBy(UserID);
            ApplicationTypeInfo = clsApplicationType._GetApplicationTypeByApplicationTypeID(ApplicationTypeID);
            PersonInfo = clsPerson._GetPersonInfo(ApplicantPersonID);
        }
        public bool _AddNew()
        {
            this.ApplicationID= clsApplicationDataAccess.AddNewApplication
                (this.ApplicantPersonID,this.ApplicationDate,this.ApplicationTypeID,
                (short)this.ApplicationStatus,this.LastStatus,this.PaidFees,this.UserID);
            return this.ApplicationID != -1;
        }
        public bool _Update()
        {
            return clsApplicationDataAccess.UpdateApplication
                (this.ApplicationID, this.ApplicantPersonID, this.ApplicationDate, this.ApplicationTypeID,
                (short)this.ApplicationStatus, this.LastStatus, this.PaidFees, this.UserID);
        }
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNew())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return _Update();
            }

            return false;
        }
        public static clsApplication _GetApplicationInfoByID(int applicationID)
        {
            int applicantPersonID = -1, applicationTypeID = -1, userID = -1;
            short applicationStatus = -1;
            DateTime applicationDate= DateTime.Now, lastStatusDate= DateTime.Now;
            decimal paidFees= 0m;
            bool isFound = clsApplicationDataAccess.GetApplicationInfoByID
                (applicationID, ref applicantPersonID, ref applicationDate,ref applicationTypeID,
                ref applicationStatus, ref lastStatusDate, ref paidFees, ref userID);
            if(isFound)
            {
                return new clsApplication
                    (applicationID, applicantPersonID, applicationDate, applicationTypeID,
                    (enApplicationStatus)applicationStatus, lastStatusDate, paidFees, userID);
            }
            else
                return null;
        }
        public static DataTable _GetAllApplications()
        {
            return clsApplicationDataAccess.GetAllApllications();
        }
        public static bool _IsApplicationExist(int applicationID)
        {
            return clsApplicationDataAccess.IsApplicationExist(applicationID);
        }
        public bool _Delete()
        {
            return clsApplicationDataAccess.DeleteApplication(ApplicationID);
        }
        public  bool _UpdateApplicationStatus(clsApplication.enApplicationStatus newApplicationStatus)
        {
            return clsApplicationDataAccess.UpdateApplicationStatus(this.ApplicationID,(short)newApplicationStatus);
        }
        public static  int _GetActiveApplicationIDForLicenseClass(int PersonID,int LicenseClassID,clsApplication.enApplicationType ApplicationTypeID)
        {
          return  clsApplicationDataAccess.GetActiveApplicationIDForLicenseClass(PersonID, LicenseClassID, (int)ApplicationTypeID);
        }

    }
}
