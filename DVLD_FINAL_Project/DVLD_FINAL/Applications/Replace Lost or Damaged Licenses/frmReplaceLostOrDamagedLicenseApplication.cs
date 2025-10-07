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
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DVLD_BuisnessLayer.clsApplication;
using static DVLD_BuisnessLayer.clsLicense;

namespace DVLD.Applications.ReplaceLostOrDamagedLicense
{
    public partial class frmReplaceLostOrDamagedLicenseApplication : Form
    {
      
        clsLicense OldLicense;
        clsLicense NewLicense;
        int OldLicenseID = -1;
        int NewLicenseID = -1;
        public frmReplaceLostOrDamagedLicenseApplication()
        {
            InitializeComponent();
        }

        private enIssueReason GetIssueReason()
        {
            if(rbDamagedLicense.Checked)
            {
                return enIssueReason.ReplacementForDamaged;
            }
            else
            {
                return enIssueReason.ReplacementForLost;
            }
        }
        private enApplicationType GetApplicationType()
        {
            if (rbDamagedLicense.Checked)
            {
                return enApplicationType.ReplaceDamagedDrivingLicense;
            }
            else
            {
                return enApplicationType.ReplaceLostDrivingLicense;
            }
        }
       
        private void frmReplaceLostOrDamagedLicenseApplication_Load(object sender, EventArgs e)
        {
            llShowLicenseHistory.Enabled = false;
            llShowLicenseInfo.Enabled = false;
            btnIssueReplacement.Enabled = false;
            rbDamagedLicense.Checked = true; 
            lblCreatedByUser.Text = clsGlopal.LoggedInUser.UserName.ToString();
            lblApplicationDate.Text = clsFormat.DateToShort(DateTime.Now);   
            
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
            llShowLicenseHistory.Enabled = (OldLicenseID != -1);
            OldLicense = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo;
            lblOldLicenseID.Text = OldLicenseID.ToString();
            if (!OldLicense.IsActive)
            {
                MessageBox.Show("Selected License is not Not Active, choose an active license."
                    , "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            btnIssueReplacement.Enabled = true;
        }
        private void btnIssueReplacement_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Replace the license", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }
            NewLicense = OldLicense.Replace(GetIssueReason(), clsGlopal.LoggedInUser.UserID);
            if(NewLicense == null)
            {
                MessageBox.Show("Faild to Renew the License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
                lblApplicationID.Text = NewLicense.ApplicationID.ToString();
                NewLicenseID = NewLicense.LicenseID;
                lblRreplacedLicenseID.Text = NewLicenseID.ToString();
                MessageBox.Show($"License Renewed Successfully with ID={NewLicense.LicenseID}", "License Issued", MessageBoxButtons.OK, MessageBoxIcon.Information); 
                gbReplacementFor.Enabled = false;
                ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
                llShowLicenseInfo.Enabled = true;
                btnIssueReplacement.Enabled = false;
        }

        private void rbDamagedLicense_CheckedChanged(object sender, EventArgs e)
        {
            lblTitle.Text = "Replacement for Damaged License";
            this.Text = lblTitle.Text;
            lblApplicationFees.Text = Convert.ToSingle(clsApplicationType._GetApplicationTypeByApplicationTypeID((int)GetApplicationType()).ApplicationFees).ToString();
        }

        private void rbLostLicense_CheckedChanged(object sender, EventArgs e)
        {
            lblTitle.Text = "Replacement for Lost License";
            this.Text = lblTitle.Text;
            lblApplicationFees.Text = Convert.ToSingle(clsApplicationType._GetApplicationTypeByApplicationTypeID((int)GetApplicationType()).ApplicationFees).ToString();
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
