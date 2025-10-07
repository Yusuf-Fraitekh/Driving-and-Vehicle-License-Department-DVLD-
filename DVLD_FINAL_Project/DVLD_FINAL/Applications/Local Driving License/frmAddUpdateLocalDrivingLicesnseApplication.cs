using DVLD.People;
using DVLD_BuisnessLayer;
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
using static System.Net.Mime.MediaTypeNames;

namespace DVLD.Applications
{

    public partial class frmAddUpdateLocalDrivingLicesnseApplication: Form
    {
        int SelectedPersonID = -1;
        enum enMode { AddNew = 0, Update = 1 };
        enMode Mode;
        int _localDrivingLicenseApplicationID = -1;
        short LicenseClassID = -1;
        clsLocalDrivingLicenseApplication localDrivingLicenseApplication;
        public frmAddUpdateLocalDrivingLicesnseApplication()
        {
            InitializeComponent();
            Mode= enMode.AddNew;
         
        }
        public frmAddUpdateLocalDrivingLicesnseApplication(int localDrivingLicenseID)
        {
            InitializeComponent();
            _localDrivingLicenseApplicationID=localDrivingLicenseID;
            Mode=enMode.Update;
        }
        public void FillLicenseClasses()
        {
            cbLicenseClass.Items.Clear();
            DataTable LicenseClasses = new DataTable();
            LicenseClasses = clsLicenseClass._GetAllLicenseClasses();
            foreach (DataRow row in LicenseClasses.Rows)
            {
                cbLicenseClass.Items.Add(row["ClassName"]);

            }
        }
        public void _ResetData()
        {
            FillLicenseClasses();
            if(Mode == enMode.AddNew)
            {
                localDrivingLicenseApplication = new clsLocalDrivingLicenseApplication();
                lblTitle.Text = "New Local Driving License Application";
                this.Text = "New Local Driving License Application";
                btnSave.Enabled = false;
                tpApplicationInfo.Enabled = false;  
                lblApplicationDate.Text = clsFormat.DateToShort(DateTime.Now);
                cbLicenseClass.SelectedIndex = 2;
                lblCreatedByUser.Text = clsGlopal.LoggedInUser.UserName.ToString(); ;
                lblFees.Text = Convert.ToSingle(clsApplicationType._GetApplicationTypeByApplicationTypeID((int)clsApplication.enApplicationType.NewDrivingLicense).ApplicationFees).ToString();
            }
            else
            {
                lblTitle.Text = "Update Local Driving License Application";
                this.Text = "Update Local Driving License Application";
                btnSave.Enabled = true;
                tpApplicationInfo.Enabled = true;
            }
        }
        public void FillData()
        {  
            ctrlPersonCardWithFilter1.FilterEnabled = false;
            localDrivingLicenseApplication = clsLocalDrivingLicenseApplication._GetLocalDrivingLicenseApplicatioInfoByID(_localDrivingLicenseApplicationID);
            if (localDrivingLicenseApplication == null)
            {
                MessageBox.Show("No Application with ID = " + _localDrivingLicenseApplicationID, "Application Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
                return;
            }
                ctrlPersonCardWithFilter1.LoadData(localDrivingLicenseApplication.ApplicantPersonID);
                lblLocalDrivingLicebseApplicationID.Text = localDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
                lblApplicationDate.Text = clsFormat.DateToShort(localDrivingLicenseApplication.ApplicationDate);
                lblFees.Text = Convert.ToSingle(localDrivingLicenseApplication.PaidFees).ToString();
                lblCreatedByUser.Text = localDrivingLicenseApplication.CreatedByUserIDInfo.UserName.ToString();
                cbLicenseClass.SelectedIndex = cbLicenseClass.FindString( clsLicenseClass._GetLicenseClassByClassID(localDrivingLicenseApplication.LicenseClassID).ClassName);
        }

        private void frmAddUpdateLocalDrivingLicesnseApplication_Load(object sender, EventArgs e)
        {
            _ResetData();
            if(Mode == enMode.Update)
                FillData();
        }

        private void btnApplicationInfoNext_Click(object sender, EventArgs e)
        {
            //if (Mode == enMode.Update)
            //{
            //    tpApplicationInfo.Enabled = true;
            //    btnSave.Enabled = true;
            //    tcApplicationInfo.SelectedTab = tpApplicationInfo;
            //    return;
            //}
            if (Mode == enMode.Update || ctrlPersonCardWithFilter1.PersonID != -1)
            {
                tpApplicationInfo.Enabled = true;
                btnSave.Enabled = true;
                tcApplicationInfo.SelectedTab = tpApplicationInfo;
            }
            else
            {
                MessageBox.Show("Please Select a person", "Select a Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
            }
        }
        private bool CheckActiveApplicationExist()
        {
            
            int ActiveApplicationID = clsApplication._GetActiveApplicationIDForLicenseClass(ctrlPersonCardWithFilter1.PersonID, LicenseClassID, clsApplication.enApplicationType.NewDrivingLicense);
            if (ActiveApplicationID != -1)
            {
                MessageBox.Show("Choose another License Class, the selected Person Already have an active application for the selected class with id=" + ActiveApplicationID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        private bool CheckLicenseExist()
        {
            int LicenseID = clsLicense._GetActiveLicense(ctrlPersonCardWithFilter1.PersonID, LicenseClassID);
            if (LicenseID != -1)
            {
                MessageBox.Show("Person already have a license with the same applied driving class with id=" + LicenseID + "Choose diffrent driving class", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        private bool CheckPersonAge()
        {
            int MinimumAllowedAge = clsLicenseClass._GetLicenseClassByClassName(cbLicenseClass.Text).MinimumAllowedAge;
            int PersonAge = (DateTime.Now.Year) - (clsPerson._GetPersonInfo(ctrlPersonCardWithFilter1.PersonID).DateOfBirth.Year);
            if (MinimumAllowedAge > PersonAge)
            {
                MessageBox.Show($"Person is not allowed for this Driving License Class, it requires a {MinimumAllowedAge} years old and above", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            LicenseClassID = clsLicenseClass._GetLicenseClassByClassName(cbLicenseClass.Text).LicenseClassID;
            if (!CheckActiveApplicationExist())
                return;
            if (!CheckLicenseExist())
                return;
            if (!CheckPersonAge())
                return;
            localDrivingLicenseApplication.ApplicantPersonID = ctrlPersonCardWithFilter1.PersonID; 
            localDrivingLicenseApplication.ApplicationDate = DateTime.Now;
            localDrivingLicenseApplication.ApplicationTypeID = (int)clsApplication.enApplicationType.NewDrivingLicense;
            localDrivingLicenseApplication.ApplicationStatus = clsApplication.enApplicationStatus.New;
            localDrivingLicenseApplication.LastStatus = DateTime.Now;
            localDrivingLicenseApplication.PaidFees = Convert.ToDecimal(lblFees.Text);
            localDrivingLicenseApplication.UserID = clsGlopal.LoggedInUser.UserID;
            localDrivingLicenseApplication.LicenseClassID =LicenseClassID;

            if(localDrivingLicenseApplication.Save())
            {
                lblLocalDrivingLicebseApplicationID.Text = localDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
                Mode = enMode.Update;
                lblTitle.Text = "Update Local Driving License Application";
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void frmAddUpdateLocalDrivingLicesnseApplication_Activated(object sender, EventArgs e)
        {
            ctrlPersonCardWithFilter1.TextBoxFocus();
        }

        private void ctrlPersonCardWithFilter1_OnPersonSelected(int obj)
        {
            SelectedPersonID = obj;
            
        }
    }
}
