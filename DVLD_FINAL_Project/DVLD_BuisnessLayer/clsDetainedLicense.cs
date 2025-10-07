using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BuisnessLayer
{
    public class clsDetainedLicense
    {
        public enum enMode { AddNew=0,Update=1};
        public enMode Mode;
        public int DetainID { get; set; }
        public int LicenseID { get; set; }
        public DateTime DetainDate { get; set; }
        public decimal FineFees { get; set; }
        public int CreatedByUserID { get; set; }
        public bool IsReleased { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int ReleasedByUserID { get; set; }
        public int ReleaseApplicationID { get; set; }
        public clsApplication ReleaseApplicationInfo;
        public clsUser CreatedByUserInfo;
        public clsUser RelesedByUserInfo;
        public clsDetainedLicense()
        {
            Mode = enMode.AddNew;
            DetainID = -1;
            LicenseID = -1;
            DetainDate = DateTime.MaxValue;
            FineFees = 0m;
            CreatedByUserID = -1;
            IsReleased = false;
            ReleaseDate = DateTime.MaxValue;
            ReleasedByUserID = -1;
            ReleaseApplicationID = -1;
        }
        private clsDetainedLicense
            (int DetainID,int LicenseID,DateTime DetainDate,decimal FineFees,
            int CreatedByUserID,bool IsReleased,DateTime ReleaseDate,int ReleasedByUserID,int ReleaseApplicationID)
        {
            this.DetainID = DetainID;
            this.LicenseID = LicenseID;
            this.DetainDate = DetainDate;
            this.FineFees = FineFees;
            this.CreatedByUserID = CreatedByUserID;
            this.IsReleased = IsReleased;
            this.ReleaseDate = ReleaseDate;
            this.ReleasedByUserID = ReleasedByUserID;
            this.ReleaseApplicationID = ReleaseApplicationID;
            ReleaseApplicationInfo = clsApplication._GetApplicationInfoByID(ReleaseApplicationID);
            CreatedByUserInfo = clsUser._GetUserInfoBy(CreatedByUserID);
            RelesedByUserInfo = clsUser._GetUserInfoBy(ReleasedByUserID);
            Mode = enMode.Update;
        }
        public bool _AddNew()
        {
            this.DetainID = clsDetainedLicenseDataAccess.AddNewDetainedLicense
               (this.LicenseID,this.DetainDate,this.FineFees,this.CreatedByUserID,this.IsReleased);
            return this.DetainID != -1;
        }
        public bool _Update()
        {
            return clsDetainedLicenseDataAccess.UpdateDetainedLicense(this.DetainID, this.LicenseID, this.DetainDate, this.FineFees, this.CreatedByUserID, this.IsReleased);
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
        public static clsDetainedLicense _GetDetainedLicenseInfoByLicenseID(int LicenseID)
        {
            int detainID = -1, createdByUserID = -1, releasedByUserID=-1, releaseApplicationID = -1;
            DateTime detainDate = DateTime.MaxValue, releaseDate = DateTime.MaxValue;
            bool isReleased = false;
            decimal fineFees = 0;
            bool isFound = clsDetainedLicenseDataAccess.GetDetainedLicenseInfoByLicenseID
                (ref detainID, LicenseID, ref detainDate, ref fineFees,
                ref createdByUserID, ref isReleased, ref releaseDate,
                ref releasedByUserID, ref releaseApplicationID);
            if(isFound)
            {
                return new clsDetainedLicense
                    (detainID, LicenseID, detainDate, fineFees, createdByUserID,
                    isReleased, releaseDate, releasedByUserID, releaseApplicationID);
            }
            else
            {
                return null;
            }
        }
        public static clsDetainedLicense _GetDetainedLicenseInfoByID(int DetainID)
        {
            int licenseID = -1, createdByUserID = -1, releasedByUserID = -1, releaseApplicationID = -1;
            DateTime detainDate = DateTime.MaxValue, releaseDate = DateTime.MaxValue;
            bool isReleased = false;
            decimal fineFees = 0;
            bool isFound = clsDetainedLicenseDataAccess.GetDetainedLicenseInfoByID
                (DetainID, ref licenseID, ref detainDate, ref fineFees,
                ref createdByUserID, ref isReleased, ref releaseDate,
                ref releasedByUserID, ref releaseApplicationID);
            if (isFound)
            {
                return new clsDetainedLicense
                    (DetainID, licenseID, detainDate, fineFees, createdByUserID,
                    isReleased, releaseDate, releasedByUserID, releaseApplicationID);
            }
            else
            {
                return null;
            }
        }
        public static bool _IsLicenseDetained(int LicenseID)
        {
            return clsDetainedLicenseDataAccess.IsLicenseDetained(LicenseID);
        }
        public bool ReleaseDetainedLicenseByDetainID()
        {
            return clsDetainedLicenseDataAccess.ReleaseDetainedLicenseByDetainID
                (this.DetainID, this.ReleasedByUserID, this.ReleaseApplicationID);
        }
        public bool ReleaseDetainedLicenseByLicenseID(int LicenseID,int UserID,int ApplicationID)
        {
            return clsDetainedLicenseDataAccess.ReleaseDetainedLicenseByLicenseID
                (LicenseID, UserID, ApplicationID);
        }
        public static DataTable _GetAllDetainedLicenses()
        {
            return clsDetainedLicenseDataAccess.GetAllDetainedLicenses();
        }
    }
}
