using DVLD.DriverLicense;
using DVLD.Licenses.Controls;
using DVLD.Licenses.International_License;
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

namespace DVLD.Applications.Rlease_Detained_License
{
    public partial class frmReleaseDetainedLicenseApplication : Form
    {
        int LicenseIDToBeReleased = -1;
        clsLicense DetainedLicense;
        public frmReleaseDetainedLicenseApplication()
        {
            InitializeComponent();
        }
        public frmReleaseDetainedLicenseApplication(int LicenseID)
        {
            InitializeComponent();
            LicenseIDToBeReleased = LicenseID;
            ctrlDriverLicenseInfoWithFilter1.LoadData(LicenseID);
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void FillData()
        {
            lblDetainID.Text = DetainedLicense.DetainedLicenseInfo.DetainID.ToString();
            lblDetainDate.Text = clsFormat.DateToShort(DetainedLicense.DetainedLicenseInfo.DetainDate);
            lblApplicationFees.Text = Convert.ToSingle(clsApplicationType._GetApplicationTypeByApplicationTypeID((int)clsApplication.enApplicationType.ReleaseDetainedDrivingLicsense).ApplicationFees).ToString();
            lblFineFees.Text = Convert.ToSingle(DetainedLicense.DetainedLicenseInfo.FineFees).ToString();
            lblTotalFees.Text= (Convert.ToSingle(lblApplicationFees.Text) + Convert.ToSingle(lblFineFees.Text)).ToString();
            lblApplicationID.Text = DetainedLicense.DetainedLicenseInfo.ReleaseApplicationID.ToString();
            lblCreatedByUser.Text = clsGlopal.LoggedInUser.UserName;
        }
        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            LicenseIDToBeReleased = obj;
            if (LicenseIDToBeReleased == -1)
                return;
            DetainedLicense = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo;
            llShowLicenseHistory.Enabled = (LicenseIDToBeReleased != -1);
            lblLicenseID.Text = LicenseIDToBeReleased.ToString();
            if(!DetainedLicense.IsLicenseDetained)
            {
                MessageBox.Show("Selected License is not detained, choose another one.", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnRelease.Enabled = false;
                return;
            }
            FillData();
            btnRelease.Enabled = true;
        }

        private void frmReleaseDetainedLicenseApplication_Load(object sender, EventArgs e)
        {

        }
        
        private void btnRelease_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to release the detained license", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            int ApplicationID = -1;
            bool IsReleased = DetainedLicense.ReleaseDetainedLicense(clsGlopal.LoggedInUser.UserID,ref ApplicationID);
            lblApplicationID.Text = ApplicationID.ToString();
            if(!IsReleased)
            {
                MessageBox.Show("Faild to Release the License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
           
            MessageBox.Show($"Detained License Released Successfully ", "Detained License Released", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnRelease.Enabled = false;
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
            llShowLicenseInfo.Enabled = true;
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLicenseInfo frm = new frmShowLicenseInfo(LicenseIDToBeReleased);
            frm.ShowDialog();
        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(DetainedLicense.DriverInfo.PersonID);
            frm.ShowDialog();
        }
    }
}
