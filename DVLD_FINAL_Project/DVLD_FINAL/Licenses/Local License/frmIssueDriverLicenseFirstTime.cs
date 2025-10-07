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

namespace DVLD.DriverLicense
{
    public partial class frmIssueDriverLicenseFirstTime : Form
    {
        int _localDrivingLicenseApplicationID = -1;
        clsLocalDrivingLicenseApplication localDrivingLicenseApplication;

        public frmIssueDriverLicenseFirstTime(int localDrivingLicenseApplicationID)
        {
            InitializeComponent();
            _localDrivingLicenseApplicationID = localDrivingLicenseApplicationID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void frmIssueDriverLicenseFirstTime_Load(object sender, EventArgs e)
        {
            txtNotes.Focus();
            localDrivingLicenseApplication = clsLocalDrivingLicenseApplication._GetLocalDrivingLicenseApplicatioInfoByID(_localDrivingLicenseApplicationID);
            if(localDrivingLicenseApplication == null)
            {
                MessageBox.Show("No Applicaiton with ID=" + _localDrivingLicenseApplicationID.ToString(), "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            if(localDrivingLicenseApplication._GetPassedTestCount() != 3)
            {
                MessageBox.Show("Person Should Pass All Tests First.", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            int LicenseID = localDrivingLicenseApplication._GetActiveLicense();
            if(LicenseID !=-1)
            {
                MessageBox.Show("Person already has License before with License ID=" + LicenseID.ToString(), "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;

            }
            ctrlDrivingLicenseApplicationInfo1.LoadData(_localDrivingLicenseApplicationID);
        }

        private void btnIssueLicense_Click(object sender, EventArgs e)
        {
            int LicenseID = localDrivingLicenseApplication.IssueLicenseForTheFirstTime(clsGlopal.LoggedInUser.UserID,txtNotes.Text.Trim());

            if (LicenseID != -1)
            {
                MessageBox.Show("License Issued Successfully with License ID = " + LicenseID.ToString(),
                    "Succeeded", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("License Was not Issued ! ",
                 "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
