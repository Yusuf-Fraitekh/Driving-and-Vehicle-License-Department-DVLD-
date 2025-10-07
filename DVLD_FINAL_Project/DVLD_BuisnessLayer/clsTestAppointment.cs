using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DVLD_BuisnessLayer.clsApplication;
namespace DVLD_BuisnessLayer
{
    public class clsTestAppointment
    {
        public enum enMode { AddNew=0,Update=1};
        enMode Mode;
        public int TestAppointmentID {  get; set; }
        public clsTestType.enTestType TestTypeID {  get; set; }
        public int LocalDrivingLicenseApplicationID {  get; set; }
        public DateTime AppointmentDate {  get; set; }
        public decimal PaidFees {  get; set; }
        public int CreatedByUserID {  get; set; }
        public bool IsLocked {  get; set; }
        public int RetakeTestApplicationID {  get; set; }
        public clsUser _UserInfo;
        public clsApplication ApplicationInfo;
        public clsLocalDrivingLicenseApplication LocalDrivingLicenseApplicationInfo;
        public int TestID
        {
            get { return GetTestID(); }
        }
        public clsTestAppointment()
        {
            TestAppointmentID = -1;
            TestTypeID = clsTestType.enTestType.VisionTest;
            LocalDrivingLicenseApplicationID = -1;
            AppointmentDate = DateTime.Now;
            PaidFees = -1m;
            CreatedByUserID = -1;
            IsLocked = false;
            RetakeTestApplicationID= -1;
            Mode=enMode.AddNew;
        }
        private clsTestAppointment
            (int testAppointmentID, clsTestType.enTestType testTypeID, int localDrivingLicenseApplicationID,
            DateTime appointmentDate, decimal paidFees, int createdByUserID, bool isLocked, int retakeTestApplicationID)
        {
            Mode = enMode.Update;
            TestAppointmentID = testAppointmentID;
            TestTypeID = testTypeID;
            LocalDrivingLicenseApplicationID = localDrivingLicenseApplicationID;
            AppointmentDate = appointmentDate;
            PaidFees = paidFees;
            CreatedByUserID = createdByUserID;
            IsLocked = isLocked;
            RetakeTestApplicationID = retakeTestApplicationID;
            _UserInfo = clsUser._GetUserInfoBy(CreatedByUserID);
            ApplicationInfo = clsApplication._GetApplicationInfoByID(RetakeTestApplicationID);
            LocalDrivingLicenseApplicationInfo = clsLocalDrivingLicenseApplication._GetLocalDrivingLicenseApplicatioInfoByID(LocalDrivingLicenseApplicationID);
        }
        public bool _Update()
        {
            return clsTestAppointmentDataAccess.UpdateTestAppointment
                (this.TestAppointmentID, (short)this.TestTypeID, this.LocalDrivingLicenseApplicationID,
                this.AppointmentDate, this.PaidFees, this.CreatedByUserID,
                this.IsLocked, this.RetakeTestApplicationID);
        }
        public bool _Add()
        {
            this.TestAppointmentID = clsTestAppointmentDataAccess.AddNewTestAppointment
                ((short)this.TestTypeID, this.LocalDrivingLicenseApplicationID,
                this.AppointmentDate, this.PaidFees, this.CreatedByUserID,
                 this.IsLocked,this.RetakeTestApplicationID);
            return this.TestAppointmentID != -1;
        }
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_Add())
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
        public static bool _Delete(int TestAppointmentID)
        {
            return clsTestAppointmentDataAccess.DeleteTestAppointment(TestAppointmentID);
        }
        public static DataTable _GetAllTestAppointments()
        {
            return clsTestAppointmentDataAccess.GetAllTestAppointments();
        }
        public static DataTable _GetAllTestAppointmentsPerTestType
            (int localDrivingLicenseApplicationID,clsTestType.enTestType testType)
        {
            return clsTestAppointmentDataAccess.GetAllTestAppointmentsPerTestType(localDrivingLicenseApplicationID, (int)testType);
        }
        public static clsTestAppointment _GetTestAppointmntInfoByID(int TestAppointmentID)
        {
            int testTypeID=-1, localDrivingLicenseApplicationID = -1, createdByUserID = -1, retakeTestApplicationID = -1;
            bool islocked =false;
            decimal paidfees = -1;
            DateTime appointmentDate = DateTime.Now;
            bool isFound = clsTestAppointmentDataAccess.GetTestAppointmentInfoByID(TestAppointmentID, ref testTypeID, ref localDrivingLicenseApplicationID, ref appointmentDate, ref paidfees, ref createdByUserID, ref islocked, ref retakeTestApplicationID);
            if(isFound)
            {
                return new clsTestAppointment(TestAppointmentID, (clsTestType.enTestType)testTypeID, localDrivingLicenseApplicationID, appointmentDate, paidfees, createdByUserID, islocked, retakeTestApplicationID);
            }
            else
            {
                return null;
            }

        }
        public static clsTestAppointment _GetTestAppointmentInfoByLocalDrivingLicenseApplicationID(int LocalDrivingLicenseApplicationID)
        {
            int testTypeID = -1, testAppointmentID = -1, createdByUserID = -1, retakeTestApplicationID = -1;
            bool islocked = false;
            decimal paidfees = -1;
            DateTime appointmentDate = DateTime.Now;
            bool isFound = clsTestAppointmentDataAccess.GetTestAppointmentInfoByLocalDrivingLicenseApplicationID(ref testAppointmentID, ref testTypeID, LocalDrivingLicenseApplicationID, ref appointmentDate, ref paidfees, ref createdByUserID, ref islocked, ref retakeTestApplicationID);
            if (isFound)
            {
                return new clsTestAppointment(testAppointmentID, (clsTestType.enTestType)testTypeID, LocalDrivingLicenseApplicationID, appointmentDate, paidfees, createdByUserID, islocked, retakeTestApplicationID);
            }
            else
            {
                return null;
            }

        }
        public int GetTestID()
        {
           return clsTestDataAccess.GetTestID(this.TestAppointmentID);
        }


    }
}
