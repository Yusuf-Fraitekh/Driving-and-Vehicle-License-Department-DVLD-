using DVLD_DataAccessLayerLastVersion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BuisnessLayer
{
    public class clsInternationalDrivingLicenseApplication:clsApplication
    {
        public new enum enMode { AddNew=0,Update=1};
        public new enMode Mode = enMode.AddNew;
        public int InernationalDrivingLicenseApplicationID { get; set; }
        public int DriverID { get; set; }
        public int LicenseID { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }
        public clsUser UserInfo;
        public clsDriver DriverInfo;
        public clsInternationalDrivingLicenseApplication():base()
        {
            Mode = enMode.AddNew;
            InernationalDrivingLicenseApplicationID = -1;
            DriverID = -1;
            LicenseID = -1;
            IssueDate = DateTime.Now;
            ExpirationDate = DateTime.Now;
            IsActive = true;
            ApplicationTypeID =(int) clsApplication.enApplicationType.NewInternationalLicense;
        }
        private clsInternationalDrivingLicenseApplication
            (int InernationalDrivingLicenseApplicationID,int DriverID,int LicenseID,
            DateTime IssueDate,DateTime ExpirationDate,bool IsActive,int UserID,
            int ApplicationID, int ApplicantPersonID, DateTime ApplicationDate
            , enApplicationStatus ApplicationStatus, DateTime LastStatus,
            decimal PaidFees)
        {
            this.InernationalDrivingLicenseApplicationID = InernationalDrivingLicenseApplicationID;
            this.DriverID = DriverID;
            this.LicenseID = LicenseID;
            this.IssueDate = IssueDate;
            this.ExpirationDate = ExpirationDate;
            this.IsActive = IsActive;

            base.UserID = UserID;
            base.ApplicationID = ApplicationID;
            base.ApplicantPersonID = ApplicantPersonID;
            base.ApplicationDate = ApplicationDate;
            base.ApplicationTypeID = (int)clsApplication.enApplicationType.NewInternationalLicense;
            base.ApplicationStatus = ApplicationStatus;
            base.LastStatus = LastStatus;
            base.PaidFees = PaidFees;
            UserInfo = clsUser._GetUserInfoBy(UserID);
            DriverInfo = clsDriver._GetDriverInfoByDriverID(DriverID);
            Mode = enMode.Update;
        }

        public static DataTable _GetAllInternationalDrivingLicenseApplications()
        {
            return clsInternationalDrivingLicenseApplicationDataAccess.GetAllInternationalDrivingLicenseApplications();
        }
        public static DataTable _GetDriverInternationalDrivingLicenseApplications(int DriverID)
        {
            return clsInternationalDrivingLicenseApplicationDataAccess.GetDriverInternationalDrivingLicenseApplications(DriverID);
        }
        public new bool _AddNew()
        {
            this.InernationalDrivingLicenseApplicationID = clsInternationalDrivingLicenseApplicationDataAccess.AddNewInternationalLicense
                (this.ApplicationID, this.DriverID, this.LicenseID, this.IssueDate,
                this.ExpirationDate, this.IsActive, this.UserID);

            return this.InernationalDrivingLicenseApplicationID != -1;
        }
        public new bool _Update()
        {
            return clsInternationalDrivingLicenseApplicationDataAccess.UpdateInternationalLicense
                (this.InernationalDrivingLicenseApplicationID, this.ApplicationID, this.DriverID,
                this.LicenseID, this.IssueDate, this.ExpirationDate, this.IsActive, this.UserID);
        }
        public new bool Save()
        {
            base.Mode = (clsApplication.enMode)Mode;
            if (!base.Save())
                return false;
            switch(Mode)
            {
                case enMode.AddNew:
                    if(_AddNew())
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
        public static clsInternationalDrivingLicenseApplication _GetInternationalDrivingLicenseApplicatioInfoByID(int InterNationalDrivingLicenseApplicationID)
        {
            int applicationID = -1, driverID = -1, licenseID = -1, userID = -1;
            bool isActive = false;
            DateTime issueDate = DateTime.Now, expirationDate = DateTime.Now;
            bool isFound = clsInternationalDrivingLicenseApplicationDataAccess.GetInternationalDrivingLicenseApplicatioInfoByID
                (InterNationalDrivingLicenseApplicationID, ref applicationID, ref driverID,
                ref licenseID, ref issueDate, ref expirationDate, ref isActive, ref userID);
            if(isFound)
            {
                clsApplication application = clsApplication._GetApplicationInfoByID(applicationID);
                return new clsInternationalDrivingLicenseApplication
                    (InterNationalDrivingLicenseApplicationID, driverID, licenseID,
                    issueDate, expirationDate, isActive, userID, application.ApplicationID,
                    application.ApplicantPersonID, application.ApplicationDate,
                    application.ApplicationStatus, application.LastStatus, application.PaidFees);
            }
            else
            {
                return null;
            }
        }
        public static int _IsInternationalLicenseExist(int DriverID)
        {
            return clsInternationalDrivingLicenseApplicationDataAccess.IsInternationalDrivingLicenseExist(DriverID);
        }
    }
}
