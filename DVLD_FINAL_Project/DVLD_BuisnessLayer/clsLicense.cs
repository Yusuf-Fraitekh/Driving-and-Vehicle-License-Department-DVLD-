using DVLD_DataAccessLayerLastVersion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_BuisnessLayer
{
    public class clsLicense
    {
        public enum enMode { AddNew=0,Update=1};
        public enum enIssueReason { FirstTime=1,ReNew=2,ReplacementForDamaged,ReplacementForLost=4};
        public enMode Mode = enMode.AddNew;
        public int LicenseID { get; set; }
        public int ApplicationID { get; set; }
        public int DriverID { get; set; }
        public short LicenseClass { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Notes { get; set; }
        public decimal PaidFees { get; set; }
        public bool IsActive { get; set; }
        public enIssueReason IssueReason;
        public int CreatedByUserID { get; set; }
        public clsUser UserInfo;
        public clsLicenseClass LicenseClassInfo;
        public clsApplication ApplicationInfo;
        public clsDriver DriverInfo;
        public  string GetIssueReasonText()
        {
            switch(IssueReason)
            {
                case enIssueReason.FirstTime:
                    return "First Time";
                case enIssueReason.ReNew:
                    return "ReNew";
                case enIssueReason.ReplacementForDamaged:
                    return "Replacement For Damaged";
                case enIssueReason.ReplacementForLost:
                    return "Replacement For Lost";
                default:
                    return "First Time";
            }
           
        }
        public bool IsLicenseDetained
        {
            get 
            { 
                return clsDetainedLicense._IsLicenseDetained(this.LicenseID);
            }
        }
        public clsDetainedLicense DetainedLicenseInfo { get; set; }
        
        public clsLicense()
        {
            Mode = enMode.AddNew;
            IssueReason = enIssueReason.FirstTime;
            ApplicationID = -1;
            DriverID = -1;
            LicenseClass = -1;
            IssueDate = DateTime.Now;
            ExpirationDate = DateTime.Now;
            Notes = "";
            PaidFees = -1;
            IsActive = true;
            CreatedByUserID = -1;
        }
        private clsLicense
            (int LicenseID, int ApplicationID, int DriverID, short LicenseClass, DateTime IssueDate, DateTime ExpirationDate,
            string Notes, decimal PaidFees, bool isActive, enIssueReason IssueReason, int CreatedByUserID)
        {
            Mode = enMode.Update;
            this.LicenseID = LicenseID;
            this.ApplicationID = ApplicationID;
            this.DriverID = DriverID;
            this.LicenseClass = LicenseClass;
            this.IssueDate = IssueDate;
            this.ExpirationDate = ExpirationDate;
            this.Notes = Notes;
            this.PaidFees = PaidFees;
            this.IsActive = isActive;
            this.IssueReason = IssueReason;
            this.CreatedByUserID = CreatedByUserID;
            UserInfo= clsUser._GetUserInfoBy(CreatedByUserID);
            LicenseClassInfo = clsLicenseClass._GetLicenseClassByClassID(LicenseClass);
            ApplicationInfo = clsApplication._GetApplicationInfoByID(ApplicationID);
            DriverInfo = clsDriver._GetDriverInfoByDriverID(DriverID);
            DetainedLicenseInfo = clsDetainedLicense._GetDetainedLicenseInfoByLicenseID(LicenseID);
        }
        public bool _AddNewLicense()
        {
            this.LicenseID = clsLicenseDataAccess.AddNewLicense(this.ApplicationID, this.DriverID, this.LicenseClass, this.IssueDate,
                this.ExpirationDate, this.Notes, this.PaidFees, this.IsActive, (short)this.IssueReason, this.CreatedByUserID);
            return this.LicenseID != -1;
        }
        public bool _UpdateLicense()
        {
            return clsLicenseDataAccess.UpdateLicense(this.LicenseID, this.ApplicationID, this.DriverID, this.LicenseClass,
                this.IssueDate,this.ExpirationDate, this.Notes, this.PaidFees,
                this.IsActive, (short)this.IssueReason, this.CreatedByUserID);
        }
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewLicense())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return _UpdateLicense();
            }

            return false;
        }
        public static bool _DeleteLicense(int LicenseID)
        {
            return clsLicenseDataAccess.DeleteLicense(LicenseID);
        }
        public static bool _IsLicenseExist(int LicenseID)
        {
            return clsLicenseDataAccess.IsLicenseExist(LicenseID);
        }
        public static bool _IsLicenseExistByPerosn(int PersonID,int LicenseClassID)
        {
            return (clsLicense._GetActiveLicense(PersonID, LicenseClassID) != -1);
        }
        public static DataTable _GetAllLicenses()
        {
            return clsLicenseDataAccess.GetAllLicenses();
        }
        public static DataTable _GetDriverLicenses(int DriverID)
        {
            return clsLicenseDataAccess.GetDriverLicenses(DriverID);
        }
        public static clsLicense _GetLicenseInfoByLicenseID(int LicenseID)
        {
            int applicationID = -1, driverID = -1, userID = -1;
            short licenseClass = -1, issueReason = -1;
            bool isActive = false;
            decimal paidFees = -1m;
            DateTime issueDate = DateTime.Now, expirationDate = DateTime.Now;
            string notes = "";
            bool isFound = clsLicenseDataAccess.GetLicenseInfoByLicenseID
                (LicenseID, ref applicationID,ref driverID,
                 ref licenseClass, ref issueDate, ref expirationDate, ref notes,
                 ref paidFees, ref isActive, ref issueReason, ref userID);
            if(isFound)
            {
                return new clsLicense(LicenseID, applicationID, driverID,
                  licenseClass, issueDate, expirationDate, notes,
                  paidFees, isActive, (enIssueReason)issueReason, userID);
            }
            else
            {
                return null;
            }
        }
        public static clsLicense _GetLicenseInfoByDriverID(int DriverID)
        {
            int applicationID = -1, licenseID = -1, userID = -1;
            short licenseClass = -1, issueReason = -1;
            bool isActive = false;
            decimal paidFees = -1m;
            DateTime issueDate = DateTime.Now, expirationDate = DateTime.Now;
            string notes = "";
            bool isFound = clsLicenseDataAccess.GetLicenseInfoByDriverID
                (ref licenseID, ref applicationID, DriverID,
                 ref licenseClass, ref issueDate, ref expirationDate, ref notes,
                 ref paidFees, ref isActive, ref issueReason, ref userID);
            if (isFound)
            {
                return new clsLicense(licenseID, applicationID, DriverID,
                  licenseClass, issueDate, expirationDate, notes,
                  paidFees, isActive,(enIssueReason)issueReason, userID);
            }
            else
            {
                return null;
            }
        }
        public static int _GetActiveLicense(int PersonID,int LicenseClass)
        {
            return clsLicenseDataAccess.GetActiveLicense(PersonID, LicenseClass);
        }
        public bool isLicenseExpired()
        {
            return (this.ExpirationDate < DateTime.Now);
        }
        
        public bool _DeactivateOldLicense()
        {
          return clsLicenseDataAccess.DeactivateOldLicense(this.LicenseID);
        }
        public clsLicense RenewLicense(int UserID,string Notes)
        {
            clsApplication RenewApplication = new clsApplication();
            RenewApplication.ApplicantPersonID = this.DriverInfo.PersonID;
            RenewApplication.ApplicationDate = DateTime.Now;
            RenewApplication.ApplicationStatus = clsApplication.enApplicationStatus.New;
            RenewApplication.ApplicationTypeID = (int)clsApplication.enApplicationType.RenewDrivingLicense;
            RenewApplication.LastStatus = DateTime.Now;
            RenewApplication.PaidFees = clsApplicationType._GetApplicationTypeByApplicationTypeID((int)clsApplication.enApplicationType.RenewDrivingLicense).ApplicationFees;
            RenewApplication.UserID = UserID;
            if (!RenewApplication.Save())
            {
                return null;
            }
            clsLicense NewLicense = new clsLicense();
            NewLicense.ApplicationID = RenewApplication.ApplicationID;
            int DefaultValidityLength = this.LicenseClassInfo.DefaultValidityLength;
            NewLicense.DriverID = this.DriverID;
            NewLicense.LicenseClass = this.LicenseClass;
            NewLicense.IssueDate = DateTime.Now;
            NewLicense.ExpirationDate = DateTime.Now.AddYears(DefaultValidityLength);
            NewLicense.Notes = Notes;
            NewLicense.PaidFees = this.LicenseClassInfo.ClassFees;
            NewLicense.IsActive = true;
            NewLicense.IssueReason = clsLicense.enIssueReason.ReNew;
            NewLicense.CreatedByUserID = UserID;
            if(!NewLicense.Save())
            {
                return null;
            }
            this._DeactivateOldLicense();
            return NewLicense;
        }
        public clsLicense Replace(enIssueReason IssueReason,int UserID)
        {
            clsApplication ReplaceApplication = new clsApplication();
            ReplaceApplication.ApplicantPersonID = this.DriverInfo.PersonID;
            ReplaceApplication.ApplicationDate = DateTime.Now;
            ReplaceApplication.ApplicationStatus = clsApplication.enApplicationStatus.New;
            ReplaceApplication.ApplicationTypeID = IssueReason == enIssueReason.ReplacementForLost ? (int)clsApplication.enApplicationType.ReplaceLostDrivingLicense : (int)clsApplication.enApplicationType.ReplaceDamagedDrivingLicense;
            ReplaceApplication.LastStatus = DateTime.Now;
            ReplaceApplication.PaidFees = clsApplicationType._GetApplicationTypeByApplicationTypeID((int)clsApplication.enApplicationType.RenewDrivingLicense).ApplicationFees;
            ReplaceApplication.UserID = UserID;
            if (!ReplaceApplication.Save())
            {
                return null;
            }
            clsLicense NewLicense = new clsLicense();
            NewLicense.ApplicationID = ReplaceApplication.ApplicationID;
            
            NewLicense.DriverID = this.DriverID;
            NewLicense.LicenseClass = this.LicenseClass;
            NewLicense.IssueDate = DateTime.Now;
            NewLicense.ExpirationDate = this.ExpirationDate;
            NewLicense.Notes = this.Notes;
            NewLicense.PaidFees = 0;
            NewLicense.IsActive = true;
            NewLicense.IssueReason =IssueReason;
            NewLicense.CreatedByUserID = UserID;
            if (!NewLicense.Save())
            {
                return null;
            }
            this._DeactivateOldLicense();
            return NewLicense;
        }
        public int Detain(string FineFeesTxt,int UserID)
        {
            clsDetainedLicense DetainedLicense = new clsDetainedLicense();
            DetainedLicense.DetainDate = DateTime.Now;
            DetainedLicense.FineFees = Convert.ToDecimal(FineFeesTxt);
            DetainedLicense.IsReleased = false;
            DetainedLicense.CreatedByUserID = UserID;
            DetainedLicense.LicenseID = LicenseID;
            if(DetainedLicense.Save())
                return DetainedLicense.DetainID;
            else
                return -1;
            
        }
        public bool ReleaseDetainedLicense(int UserID,ref int ApplicationID)
        {
            clsApplication ReleaseApplication = new clsApplication();

            ReleaseApplication.ApplicantPersonID = this.DriverInfo.PersonID;
            ReleaseApplication.ApplicationDate = DateTime.Now;
            ReleaseApplication.ApplicationStatus = clsApplication.enApplicationStatus.New;
            ReleaseApplication.ApplicationTypeID = (int)clsApplication.enApplicationType.ReleaseDetainedDrivingLicsense;
            ReleaseApplication.LastStatus = DateTime.Now;
            ReleaseApplication.PaidFees = clsApplicationType._GetApplicationTypeByApplicationTypeID((int)clsApplication.enApplicationType.ReleaseDetainedDrivingLicsense).ApplicationFees;
            ReleaseApplication.UserID = UserID;
            ReleaseApplication.CreatedByUserIDInfo = this.UserInfo;
            
            if (!ReleaseApplication.Save())
            {
                ApplicationID = -1;
                return false;
            }
            ApplicationID = ReleaseApplication.ApplicationID;
            return this.DetainedLicenseInfo.ReleaseDetainedLicenseByLicenseID(this.LicenseID,UserID,ApplicationID);
           
        }
    }
}
