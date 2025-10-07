using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BuisnessLayer
{
    public class clsTest
    {
        public enum enMode { AddNew=0,Update=1};
        public enMode Mode;
        public int TestID {  get; set; }
        public int TestAppointmentID {  get; set; }
        public bool TestResult { get; set; }
        public string Notes { get; set; }
        public int CreatedByUserID { get; set; }
        public clsUser UserInfo;
        public clsTestAppointment TestAppointment;
        public clsTest()
        {
            TestID = -1;
            TestAppointmentID = -1;
            TestResult = false;
            Notes = "";
            CreatedByUserID = -1;
            Mode=enMode.AddNew;
        }
        private clsTest( int testID, int testAppointmentID, bool testResult, string notes, int createdByUserID)
        {
            Mode = enMode.Update;
            TestID = testID;
            TestAppointmentID = testAppointmentID;
            TestResult = testResult;
            Notes = notes;
            CreatedByUserID = createdByUserID;
            UserInfo = clsUser._GetUserInfoBy(CreatedByUserID);
            TestAppointment = clsTestAppointment._GetTestAppointmntInfoByID(TestAppointmentID);
        }
        public bool _Update()
        {
            return clsTestDataAccess.UpdateTest
                (this.TestID,this.TestAppointmentID,this.TestResult,this.Notes,this.CreatedByUserID);
        }
        public bool _Add()
        {
            this.TestID = clsTestDataAccess.AddNewTest
                (this.TestAppointmentID, this.TestResult, this.Notes, this.CreatedByUserID);
            return this.TestID != -1;
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
        public static bool _Delete(int TestID)
        {
            return clsTestDataAccess.DeleteTest(TestID);
        }
        public static DataTable _GetAllTests()
        {
            return clsTestDataAccess.GetAllTests();
        }
        public static clsTest _GetTestInfoByID(int TestID)
        {
            int testAppointmentID = -1, createdByUserID = -1;
            bool testResult =false;
            string notes = "";
            
            bool isFound = clsTestDataAccess.GetTestInfoByID(TestID, ref testAppointmentID, ref testResult, ref notes,  ref createdByUserID);
            if (isFound)
            {
                return new clsTest(TestID, testAppointmentID, testResult, notes, createdByUserID);
            }
            else
            {
                return null;
            }

        }
        public static int _PassedTestCount(int LocalDrivingLicenseApplicationID)
        {
            return clsTestDataAccess.GetPassedTestCount(LocalDrivingLicenseApplicationID);
        }
        public static bool _DoesPassTest(int LocalDrivingLicenseApplicationID,clsTestType.enTestType testType)
        {
            return clsTestDataAccess.DoesPassTest(LocalDrivingLicenseApplicationID, (int)testType);
        }

        public static bool _DoesMakeTest(int LocalDrivingLicenseApplicationID, clsTestType.enTestType testType)
        {
            return clsTestDataAccess.DoesMakeTest(LocalDrivingLicenseApplicationID, (int)testType);
        }
    }
}
