using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace DVLD_BuisnessLayer
{
    public class clsLocalDrivingLicenseApplication:clsApplication
    {
        public new enum enMode { AddNew=0,Update=1};
        public new enMode Mode;
        public int LocalDrivingLicenseApplicationID { get; set;}
        public short LicenseClassID {  get; set;}
        public clsLicenseClass LicenseClass;
        public clsLocalDrivingLicenseApplication():base()
        {
            LocalDrivingLicenseApplicationID = -1;
            LicenseClassID = -1;
            Mode = enMode.AddNew;

        }
        private clsLocalDrivingLicenseApplication
            (int localDrivingLicenseApplicationID, int applicationID, int applicantPersonID, DateTime applicationDate,
            int applicationTypeID, enApplicationStatus applicationStatus, DateTime lastStatus,
            decimal paidFees, int userID, short licenseClassID)
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
            LocalDrivingLicenseApplicationID = localDrivingLicenseApplicationID;
            LicenseClassID = licenseClassID;
            LicenseClass=clsLicenseClass._GetLicenseClassByClassID(licenseClassID);
        }
        public static DataTable _GetAllLocalDrivingLicenseApplications()
        {
            return clsLocalDrivingLicenseApplicationDataAccess.GetAllLocalDrivingLicenseApplications();
        }
        public new  bool _AddNew()
        {
           this.LocalDrivingLicenseApplicationID= clsLocalDrivingLicenseApplicationDataAccess.AddNewLocalDrivingLicenseApplication
                (this.ApplicationID,this.LicenseClassID);
            return this.LocalDrivingLicenseApplicationID!=-1;
        }
        public new bool _Update()
        {
            return clsLocalDrivingLicenseApplicationDataAccess.UpdateLocalDrivingLicenseApplication(this.LocalDrivingLicenseApplicationID, this.ApplicationID, this.LicenseClassID);
        }
        public new bool Save()
        {
            base.Mode = (clsApplication.enMode)Mode;
            if(!base.Save())
                return false;
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
        public static clsLocalDrivingLicenseApplication _GetLocalDrivingLicenseApplicatioInfoByID(int localDrivingLicenseApplicationID)
        {
            int applicationID = -1;
            short licenseClassID = -1;
            
            bool isFound= clsLocalDrivingLicenseApplicationDataAccess.GetLocalDrivingLicenseApplicatioInfoByID(localDrivingLicenseApplicationID,ref applicationID,ref licenseClassID);
            if (isFound)
            {
                clsApplication application =clsApplication._GetApplicationInfoByID(applicationID);
                return new clsLocalDrivingLicenseApplication
                    (localDrivingLicenseApplicationID,  applicationID,application.ApplicantPersonID,
                    application.ApplicationDate,  application.ApplicationTypeID,application.ApplicationStatus,
                    application.LastStatus,application.PaidFees,application.UserID,licenseClassID);
            }
            else
                return null;
        }
       public new bool _Delete()
        {
            if (!clsLocalDrivingLicenseApplicationDataAccess.DeleteLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationID))
                return false;
            return base._Delete();
        }
        public static bool _IsLocalDrivingLicneseApplicationExist(int  localDrivingLicenseApplicationID)
        {
          return clsLocalDrivingLicenseApplicationDataAccess.IsLocalDrivingLicneseApplicationExist(localDrivingLicenseApplicationID);
        }
        public  int _GetPassedTestCount()
        {
            return clsTestDataAccess.GetPassedTestCount(this.LocalDrivingLicenseApplicationID);
        }
        public int _GetNumberOfTrials(clsTestType.enTestType testType)
        {
            return clsTestDataAccess.GetNumberOfTrials(this.LocalDrivingLicenseApplicationID,(int)testType);
        }

        public  bool _DoesHaveActiveTestAppointment(clsTestType.enTestType testType)
        {
            return clsLocalDrivingLicenseApplicationDataAccess.DoesHaveActiveTestAppointment(this.LocalDrivingLicenseApplicationID,(int)testType);
        }
        public bool DoesPassTest(clsTestType.enTestType testType)
        {
            return clsTest._DoesPassTest(this.LocalDrivingLicenseApplicationID,testType);
        }
        public bool DoesMakeTest(clsTestType.enTestType testType)
        {
            return _GetNumberOfTrials(testType) > 0;
        }
        public int _GetActiveLicense()
        {
            return clsLicense._GetActiveLicense(this.ApplicantPersonID, this.LicenseClassID);
        }
        public int IssueLicenseForTheFirstTime(int CreatedByUserID,string Notes)
        {
            int DriverID = -1;
            clsDriver driver = clsDriver._GetDriverInfoByPersonID(this.ApplicantPersonID);
            if (driver == null)
            {
                driver = new clsDriver();
                driver.PersonID = this.ApplicantPersonID;
                driver.UserID = CreatedByUserID;
                if (driver.Save())
                {
                    DriverID = driver.DriverID;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                DriverID = driver.DriverID;
            }
            clsLicense License = new clsLicense();
            License.ApplicationID = this.ApplicationID;
            License.DriverID = DriverID;
            License.LicenseClass = this.LicenseClassID;
            License.IssueDate = DateTime.Now;
            License.ExpirationDate = DateTime.Now.AddYears(this.LicenseClass.DefaultValidityLength);
            License.Notes = Notes;
            License.PaidFees = this.LicenseClass.ClassFees;
            License.IsActive = true;
            License.IssueReason = clsLicense.enIssueReason.FirstTime;
            License.CreatedByUserID = CreatedByUserID;
            if(License.Save())
            {
                this._UpdateApplicationStatus(clsApplication.enApplicationStatus.Completed);
                return License.LicenseID;
            }
            else
            {
                return -1;
            }
        }
    }
}
