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
using static DVLD_BuisnessLayer.clsTestType;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD.Tests
{

    public partial class ctrlScheduleTest : UserControl
    {
        private enum enMode {AddNew=0,Update=1};
        private enum enCreationMode { NewTest=0,RetakeTest=1};
        private enCreationMode CreationMode = enCreationMode.NewTest;
        private enMode Mode = enMode.AddNew;
        private int _appointmentID = -1;
        private int _localDrivingLicenseApplicationID = -1;
        clsTestAppointment testAppointment;
        clsTestType.enTestType _testType = clsTestType.enTestType.VisionTest;
        clsLocalDrivingLicenseApplication localDrivingLicenseApplication;
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
        public ctrlScheduleTest()
        {
            InitializeComponent();
        }
        private bool HandleLockedConstraint()
        {
            if (testAppointment.IsLocked)
            {
                lblUserMessage.Visible = true;
                lblUserMessage.Text = "Person already sat for the test, appointment is locked";
                lblTitle.Text = "Schedule Retake Test";
                btnSave.Enabled = false;
                dtpTestDate.Enabled = false;
                gbRetakeTestInfo.Enabled = true;
                return false;
            }
            else
            {
                lblUserMessage.Visible = false;
            }
            return true;
        }
       private bool HandleActiveTestAppointmentConstriant()
        {
            if(Mode == enMode.AddNew && localDrivingLicenseApplication._DoesHaveActiveTestAppointment(testType))
            {
                lblUserMessage.Text = "Person Already have an active appointment for this test";
                btnSave.Enabled = false;
                dtpTestDate.Enabled = false;
                return false;
            }
            return true;
        }
        private bool HandlePreviousTestConstriant()
        {
            switch(testType)
            {
                case clsTestType.enTestType.VisionTest:
                    if(localDrivingLicenseApplication.DoesPassTest(testType))
                    {
                        lblUserMessage.Visible = true;
                        lblUserMessage.Text = "Cannot Sechule, Person already passed Vision Test";
                        btnSave.Enabled = false;
                        dtpTestDate.Enabled = false;
                        return false;
                    }
                    else
                    {
                        lblUserMessage.Visible = false;
                    }
                    return true;
                case clsTestType.enTestType.WrittenTest:
                    if(localDrivingLicenseApplication.DoesPassTest(testType))
                    {
                        lblUserMessage.Text = "Cannot Sechule,  Person already passed Written Test";
                        lblUserMessage.Visible = true;
                        btnSave.Enabled = false;
                        dtpTestDate.Enabled = false;
                        return false;
                    }
                    if(!localDrivingLicenseApplication.DoesPassTest(clsTestType.enTestType.VisionTest))
                    {
                        lblUserMessage.Text = "Cannot Sechule, Vision Test should be passed first";
                        lblUserMessage.Visible = true;
                        btnSave.Enabled = false;
                        dtpTestDate.Enabled = false;
                        return false;
                    }
                    else
                    {
                        lblUserMessage.Visible = false;
                        btnSave.Enabled = true;
                        dtpTestDate.Enabled = true;
                    }
                    return true;
                case clsTestType.enTestType.StreetTest:
                    if (localDrivingLicenseApplication.DoesPassTest(testType))
                    {
                        lblUserMessage.Text = "Cannot Sechule,  Person already passed StreetTest Test";
                        lblUserMessage.Visible = true;
                        btnSave.Enabled = false;
                        dtpTestDate.Enabled = false;
                        return false;
                    }
                    if (!localDrivingLicenseApplication.DoesPassTest(clsTestType.enTestType.WrittenTest))
                    {
                        lblUserMessage.Text = "Cannot Sechule, Vision Test should be passed first";
                        lblUserMessage.Visible = true;
                        btnSave.Enabled = false;
                        dtpTestDate.Enabled = false;
                        return false;
                    }
                    else
                    {
                        lblUserMessage.Visible = false;
                        btnSave.Enabled = true;
                        dtpTestDate.Enabled = true;
                    }
                    return true;

            }
            return true;
        }
        public void LoadData(int localDrivingLicenseApplicationID, int AppointmentID = -1)
        {
            _appointmentID = AppointmentID;
            if (_appointmentID != -1)
                Mode = enMode.Update;
            else
                Mode = enMode.AddNew;

            _localDrivingLicenseApplicationID = localDrivingLicenseApplicationID;
            if (!FillData())
                return;

            if (localDrivingLicenseApplication.DoesMakeTest(_testType))
                CreationMode = enCreationMode.RetakeTest;
            else
                CreationMode = enCreationMode.NewTest;

            if(CreationMode == enCreationMode.RetakeTest)
            {
                lblUserMessage.Visible = false;
                lblTitle.Text = "Schedule Retake Test";
                gbRetakeTestInfo.Enabled = true;
                lblRetakeAppFees.Text = clsApplicationType._GetApplicationTypeByApplicationTypeID((int)clsApplication.enApplicationType.RetakeTest).ApplicationFees.ToString();
                lblRetakeTestAppID.Text = "N/A";         
            }
            else
            {
                gbRetakeTestInfo.Enabled = false;
                lblUserMessage.Visible = false;
                lblTitle.Text = "Schedule Test";
                lblRetakeAppFees.Text = "0";
                lblRetakeTestAppID.Text = "N/A";
            }
            if (Mode == enMode.Update)
            {
                
                if (!FillAppointmentData())
                    return;
            }
            else
            {
                dtpTestDate.MinDate = DateTime.Now;
                lblFees.Text = ((int)clsTestType._GetTestTypeByTestTypeID(_testType).TestTypeFees).ToString();
                testAppointment = new clsTestAppointment();
            }
            lblTotalFees.Text = (Convert.ToSingle(lblFees.Text) + Convert.ToSingle(lblRetakeAppFees.Text)).ToString();
            if (!HandleActiveTestAppointmentConstriant())
                return;
            if (!HandlePreviousTestConstriant())
                return;
            if (!HandleLockedConstraint())
                return;
        }
        
        private bool FillAppointmentData()
        {
            testAppointment = clsTestAppointment._GetTestAppointmntInfoByID(_appointmentID);
            if(testAppointment == null)
            {
                MessageBox.Show("Error: No Appointment with ID = " + _appointmentID.ToString(),
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return false;
            }
            dtpTestDate.Value = testAppointment.AppointmentDate;
            dtpTestDate.MinDate = DateTime.Now;
            lblFees.Text = Convert.ToSingle(testAppointment.PaidFees).ToString();
            if(testAppointment.RetakeTestApplicationID!=-1)
            {
                lblRetakeTestAppID.Text = testAppointment.RetakeTestApplicationID.ToString();
                lblRetakeAppFees.Text = Convert.ToSingle(testAppointment.ApplicationInfo.PaidFees).ToString();
                gbRetakeTestInfo.Enabled = true;
                lblTitle.Text = "Schedule Retake Test";
            }
            else
            {
                lblRetakeAppFees.Text = "0";
                lblRetakeTestAppID.Text = "N/A";
            }
                return true;
        }
        private bool FillData()
        {
            localDrivingLicenseApplication = clsLocalDrivingLicenseApplication._GetLocalDrivingLicenseApplicatioInfoByID(_localDrivingLicenseApplicationID);
            if (localDrivingLicenseApplication == null)
            {
                MessageBox.Show("Error: No Local Driving License Application with ID = " + _localDrivingLicenseApplicationID.ToString(),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return false;
            }
            lblLocalDrivingLicenseAppID.Text = localDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
            lblDrivingClass.Text = localDrivingLicenseApplication.LicenseClass.ClassName.ToString();
            lblFullName.Text = localDrivingLicenseApplication.PersonInfo.FullName.ToString();
            lblTrial.Text = localDrivingLicenseApplication._GetNumberOfTrials(_testType).ToString();
            return true;
        }
        private bool HandleRetakeTest()
        {
            if(Mode == enMode.AddNew && CreationMode == enCreationMode.RetakeTest)
            {
                clsApplication RetakeTestApplication = new clsApplication();
                RetakeTestApplication.ApplicantPersonID = localDrivingLicenseApplication.ApplicantPersonID;
                RetakeTestApplication.ApplicationDate = DateTime.Now;
                RetakeTestApplication.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
                RetakeTestApplication.ApplicationTypeID = (int)clsApplication.enApplicationType.RetakeTest;
                RetakeTestApplication.LastStatus= DateTime.Now;
                RetakeTestApplication.PaidFees = clsApplicationType._GetApplicationTypeByApplicationTypeID((int)clsApplication.enApplicationType.RetakeTest).ApplicationFees;
                RetakeTestApplication.CreatedByUserIDInfo = clsGlopal.LoggedInUser;
                RetakeTestApplication.UserID = clsGlopal.LoggedInUser.UserID;
                if (!RetakeTestApplication.Save())
                {
                    testAppointment.RetakeTestApplicationID = -1;
                    MessageBox.Show("Faild to Create application", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                    
                }
                testAppointment.RetakeTestApplicationID = RetakeTestApplication.ApplicationID;
                lblRetakeTestAppID.Text = RetakeTestApplication.ApplicationID.ToString();
                
            }
            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!HandleRetakeTest())
                return;
            testAppointment.AppointmentDate = dtpTestDate.Value;
            testAppointment.TestTypeID = _testType;
            testAppointment.LocalDrivingLicenseApplicationID = localDrivingLicenseApplication.LocalDrivingLicenseApplicationID;
            testAppointment.PaidFees = Convert.ToDecimal(lblFees.Text);
            testAppointment.CreatedByUserID = clsGlopal.LoggedInUser.UserID;
            testAppointment.IsLocked = false;
            if (testAppointment.Save())
            {
                Mode = enMode.Update;
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
