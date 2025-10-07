using DVLD.DriverLicense;
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

namespace DVLD.Licenses
{
    public partial class frmRenewLocalDrivingLicenseApplication: Form
    {
        int NewLicenseID = -1;
        int OldLicenseID = -1;
        clsLicense OldLicense;
        clsLicense NewLicense;


        public frmRenewLocalDrivingLicenseApplication()
        {
            InitializeComponent();
        }

        private void frmRenewLocalDrivingLicenseApplication_Load(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfoWithFilter1.txtLicenseIDFocus();
            lblExpirationDate.Text = "???";
            lblCreatedByUser.Text = clsGlopal.LoggedInUser.UserName.ToString();
            lblApplicationDate.Text = clsFormat.DateToShort(DateTime.Now);
            lblIssueDate.Text = clsFormat.DateToShort(DateTime.Now);
            lblApplicationFees.Text = Convert.ToSingle(clsApplicationType._GetApplicationTypeByApplicationTypeID((int)clsApplication.enApplicationType.RenewDrivingLicense).ApplicationFees).ToString();
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            OldLicenseID = obj;
            if (OldLicenseID == -1)
                return;
            OldLicense = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo;
            llShowLicenseHistory.Enabled = (OldLicenseID != -1);
            LoadApplicationInfo();
            if (!OldLicense.isLicenseExpired())
            {
                MessageBox.Show($"Selected License is not yet expired,it will expire on:" +
                    $"{clsFormat.DateToShort(OldLicense.ExpirationDate)}", "Not Allowd", MessageBoxButtons.OK);
                btnRenewLicense.Enabled = false;
                llShowLicenseInfo.Enabled = false;
                return;
            }
            if(!OldLicense.IsActive)
            {
                MessageBox.Show("Selected License is not Not Active, choose an active license."
                    , "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                llShowLicenseInfo.Enabled = false;
                btnRenewLicense.Enabled = false;
                return;
            }
            btnRenewLicense.Enabled = true;
           

        }
        private void LoadApplicationInfo()
        {
            int DefaultValidityLength = OldLicense.LicenseClassInfo.DefaultValidityLength;
            lblLicenseFees.Text = Convert.ToSingle(OldLicense.LicenseClassInfo.ClassFees).ToString();
            lblTotalFees.Text = (Convert.ToSingle(lblApplicationFees.Text) + Convert.ToSingle(lblLicenseFees.Text)).ToString();
            lblExpirationDate.Text = clsFormat.DateToShort(DateTime.Now.AddYears(DefaultValidityLength)).ToString();
            lblOldLicenseID.Text = OldLicenseID.ToString();
            txtNotes.Text = OldLicense.Notes;
        }
        

        private void btnRenewLicense_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to Renew the license","Confirm",MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }
            NewLicense = OldLicense.RenewLicense(clsGlopal.LoggedInUser.UserID, txtNotes.Text);
            if(NewLicense == null)
            {
                MessageBox.Show("Faild to Renew the License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
                lblApplicationID.Text = NewLicense.ApplicationID.ToString();
                NewLicenseID = NewLicense.LicenseID;
                lblRenewedLicenseID.Text = NewLicenseID.ToString();
                MessageBox.Show($"License Renewed Successfully with ID={NewLicense.LicenseID}", "License Issued", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnRenewLicense.Enabled = false;
                ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
                llShowLicenseInfo.Enabled = true;

        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLicenseInfo frm = new frmShowLicenseInfo(NewLicenseID);
            frm.ShowDialog();
        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(OldLicense.DriverInfo.PersonID);
            frm.ShowDialog();
        }
    }
}
