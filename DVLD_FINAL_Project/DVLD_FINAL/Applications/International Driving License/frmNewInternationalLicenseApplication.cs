
using DVLD.DriverLicense;
using DVLD.Licenses.International_Licenses;
using DVLD_BuisnessLayer;
using DVLD_FINAL.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DVLD_BuisnessLayer.clsApplication;

namespace DVLD.Applications.International_License
{
    public partial class frmNewInternationalLicenseApplication : Form
    {
        clsLicense License;
        int InternationalLicenseID = -1;
        clsInternationalDrivingLicenseApplication InternationalLicense;
        public frmNewInternationalLicenseApplication()
        {
            InitializeComponent();
        }

        private void frmNewInternationalLicenseApplication_Load(object sender, EventArgs e)
        {
            lblApplicationDate.Text = clsFormat.DateToShort(DateTime.Now);
            lblIssueDate.Text= clsFormat.DateToShort(DateTime.Now);
            lblFees.Text = clsApplicationType._GetApplicationTypeByApplicationTypeID((int)clsApplication.enApplicationType.NewInternationalLicense).ApplicationFees.ToString();
            lblCreatedByUser.Text = clsGlopal.LoggedInUser.UserName;
            lblExpirationDate.Text = clsFormat.DateToShort(DateTime.Now.AddYears(1));
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            int LicenseID = obj;
            if (LicenseID == -1)
                return;
            License = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo;
            llShowLicenseHistory.Enabled = (LicenseID != -1);
            lblLocalLicenseID.Text = LicenseID.ToString();
            if(License.LicenseClass !=(short) clsLicenseClass.enClassName.OrdinaryDriving)
            {
                MessageBox.Show($"Selected License Should be Class 3 Select another one", "Not Allowd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnIssueLicense.Enabled = false;
                llShowLicenseInfo.Enabled = false;
                return;
            }
            if (License.isLicenseExpired())
            {
                MessageBox.Show($"Selected License is  expired" , "Not Allowd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnIssueLicense.Enabled = false;
                llShowLicenseInfo.Enabled = false;
                return;
            }
            if (!License.IsActive)
            {
                MessageBox.Show("Selected License is not Not Active, choose an active license."
                    , "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                llShowLicenseInfo.Enabled = false;
                btnIssueLicense.Enabled = false;
                return;
            }
            int ActivatedInternationalLicenseID = clsInternationalDrivingLicenseApplication._IsInternationalLicenseExist(License.DriverID);
            if (ActivatedInternationalLicenseID !=-1)
            {
                MessageBox.Show("Person already have an active international license with ID = " + ActivatedInternationalLicenseID.ToString(), "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                llShowLicenseInfo.Enabled = true;
                lblInternationalLicenseID.Text = ActivatedInternationalLicenseID.ToString();
                InternationalLicenseID = ActivatedInternationalLicenseID;
                btnIssueLicense.Enabled = false;
                return;
            }
            btnIssueLicense.Enabled = true;
        }
        

        private void btnIssueLicense_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to issue the license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }
            InternationalLicense = new clsInternationalDrivingLicenseApplication();
            InternationalLicense.ApplicantPersonID = License.DriverInfo.PersonInfo.PersonID;
            InternationalLicense.ApplicationDate = DateTime.Now;
            InternationalLicense.ApplicationTypeID = (int)clsApplication.enApplicationType.NewInternationalLicense;
            InternationalLicense.ApplicationStatus=clsApplication.enApplicationStatus.Completed;
            InternationalLicense.DriverID = License.DriverID;
            InternationalLicense.LicenseID = License.LicenseID;
            InternationalLicense.IssueDate = DateTime.Now;
            InternationalLicense.ExpirationDate = (DateTime.Now.AddYears(1));
            InternationalLicense.IsActive = true;
            InternationalLicense.UserID = clsGlopal.LoggedInUser.UserID;
            if(!InternationalLicense.Save())
            {
                MessageBox.Show("Faild to Issue International License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            InternationalLicenseID = InternationalLicense.InernationalDrivingLicenseApplicationID;
            lblApplicationID.Text = InternationalLicense.ApplicationID.ToString();
            lblInternationalLicenseID.Text = InternationalLicenseID.ToString();
            MessageBox.Show("International License Issued Successfully with ID=" + InternationalLicenseID.ToString(), "License Issued", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnIssueLicense.Enabled = false;
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
            llShowLicenseInfo.Enabled = true;

        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
           frmShowInternationalLicenseInfo frm = new frmShowInternationalLicenseInfo(InternationalLicenseID);
           frm.ShowDialog();
            frmNewInternationalLicenseApplication_Load(null, null);
        }
    }
}
