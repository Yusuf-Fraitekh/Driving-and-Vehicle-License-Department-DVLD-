using DVLD.DriverLicense;
using DVLD.Licenses.Controls;
using DVLD.Licenses.International_License;
using DVLD.Licenses.International_Licenses;
using DVLD_BuisnessLayer;
using DVLD_FINAL.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Applications.Detain_License
{
    public partial class frmDetainLicenseApplication : Form
    {
        int LicenseID = -1;
        int DetainID = -1;
        clsLicense LicenseToBeDetained;
        public frmDetainLicenseApplication()
        {
            InitializeComponent();
        }
        public frmDetainLicenseApplication(int licenseID)
        {
            InitializeComponent();
            LicenseID = licenseID;
            ctrlDriverLicenseInfoWithFilter1.LoadData(LicenseID);
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
        }

        private void frmDetainLicenseApplication_Load(object sender, EventArgs e)
        {
            lblCreatedByUser.Text = clsGlopal.LoggedInUser.UserName;
            lblDetainDate.Text = clsFormat.DateToShort(DateTime.Now);
            llShowLicenseInfo.Enabled = false;
            llShowLicenseHistory.Enabled = false;
            btnDetain.Enabled = false;
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            LicenseID = obj;
            if (LicenseID == -1)
            {
                return;
            }
            LicenseToBeDetained = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo;
            llShowLicenseHistory.Enabled = (LicenseID != -1);
            lblLicenseID.Text = LicenseID.ToString();
            if(LicenseToBeDetained.IsLicenseDetained)
            {
                MessageBox.Show("Selected License is already detained, choose another one.", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnDetain.Enabled = false;
                return;
            }
            txtFineFees.Focus();
            btnDetain.Enabled = true;
        }

        private void txtFineFees_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFineFees.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFineFees, "Fine Fees Cant Be empty");
            }
            else
            {
                if (decimal.TryParse(txtFineFees.Text, out decimal value))
                {
                    e.Cancel = false;
                    errorProvider1.SetError(txtFineFees, null);
                }
                else
                {
                    errorProvider1.SetError(txtFineFees, "Fine Fees Must be A number");
                    e.Cancel = true;
                }

            }
        }

        private void btnDetain_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to detain this license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }
            if(!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valid!, put the mouse over the red icon(s) to see the erro", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DetainID = LicenseToBeDetained.Detain(txtFineFees.Text, clsGlopal.LoggedInUser.UserID);

            if (DetainID == -1)
            {
                MessageBox.Show("Faild to Detain License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            lblDetainID.Text =DetainID.ToString();
            MessageBox.Show("License Detained Successfully with ID=" + DetainID.ToString(), "License Detained", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnDetain.Enabled = false;
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
            txtFineFees.Enabled = false;
            llShowLicenseInfo.Enabled = true;
        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(LicenseToBeDetained.DriverInfo.PersonID);
            frm.ShowDialog();
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLicenseInfo frm = new frmShowLicenseInfo(LicenseID);
            frm.ShowDialog();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmDetainLicenseApplication_Shown(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfoWithFilter1.txtLicenseIDFocus();
        }

       
    }
}
