using DVLD_BuisnessLayer;
using DVLD_FINAL.Properties;
using DVLD_FINAL.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace DVLD.Tests
{
    public partial class ctrlSecheduledTest: UserControl
    {
        private int _testAppointmentID=-1;
        clsTestAppointment testAppointment;
        clsTestType.enTestType _testType;
        private int _testID=-1;
        public ctrlSecheduledTest()
        {
            InitializeComponent();
        }
        public int TestID
        {
            get
            {
                return _testID;
            }
        }
        public clsTestType.enTestType testType
        {
            set
            {
                _testType = value;
                switch (_testType)
                {
                    case clsTestType.enTestType.VisionTest:
                        gbTestType.Text = "Vision Test";
                        pbTestTypeImage.Image = Resources.Vision_512;
                        break;
                    case clsTestType.enTestType.WrittenTest:
                        gbTestType.Text = "Written Test";
                        pbTestTypeImage.Image = Resources.Written_Test_512;
                        break;
                    case clsTestType.enTestType.StreetTest:
                        gbTestType.Text = "Street Test";
                        pbTestTypeImage.Image = Resources.driving_test_512;
                        break;
                }
            }
            get
            {
                return _testType;
            }
        }
        public clsTestAppointment TestAppointment
        {
            get 
            { 
                return testAppointment;
            }
        }
            
        private void ResetData()
        {
            lblLocalDrivingLicenseAppID.Text = "[??]";
            lblDrivingClass.Text = "[???????]";
            lblFullName.Text = "[???????]";
            lblTrial.Text = "[??]";
            lblDate.Text = "[dd/mm/yyyy]";
            lblFees.Text = "[$$$]";
            lblTestID.Text = "Not Taken Yet";
        }
        public void LoadData(int testAppointmentID)
        {
            testAppointment=clsTestAppointment._GetTestAppointmntInfoByID(testAppointmentID);
            if(testAppointment == null)
            {
                ResetData();
                MessageBox.Show("No Appointment with AppointmentID = " + _testAppointmentID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _testAppointmentID = testAppointmentID;
            _testID = testAppointment.GetTestID();
            FillData();
        }
        private void FillData()
        {
            lblLocalDrivingLicenseAppID.Text=testAppointment.LocalDrivingLicenseApplicationID.ToString();
            lblDrivingClass.Text=testAppointment.LocalDrivingLicenseApplicationInfo.LicenseClass.ClassName.ToString();
            lblFullName.Text= testAppointment.LocalDrivingLicenseApplicationInfo.PersonInfo.FullName.ToString();
            lblTrial.Text = testAppointment.LocalDrivingLicenseApplicationInfo._GetNumberOfTrials(testType).ToString();
            lblDate.Text= clsFormat.DateToShort(testAppointment.AppointmentDate);
            lblFees.Text = Convert.ToSingle(clsTestType._GetTestTypeByTestTypeID(testAppointment.TestTypeID).TestTypeFees).ToString();
            lblTestID.Text = (TestID == -1) ? "Not Taken Yet" : TestID.ToString();
        }
    }
}
