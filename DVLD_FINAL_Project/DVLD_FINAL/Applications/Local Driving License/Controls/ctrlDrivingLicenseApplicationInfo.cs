using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD_BuisnessLayer;
using static System.Net.Mime.MediaTypeNames;
using DVLD.Tests;
using DVLD.DriverLicense;

namespace DVLD.Controls.ApplicationControls
{
    public partial class ctrlDrivingLicenseApplicationInfo: UserControl
    {
        private int _LicenseID = -1;
        private clsLocalDrivingLicenseApplication localDrivingLicenseApplication;
        private int _localDrivingLicenseApplicationID=-1;
        public int LocalDrivingLicenseApplicationID
        {
            get
            {
                return _localDrivingLicenseApplicationID;
            }
        }
       
        public ctrlDrivingLicenseApplicationInfo()
        {
            InitializeComponent();
        }
        public void ResetData()
        {
            llShowLicenceInfo.Enabled = false;
            lblLocalDrivingLicenseApplicationID.Text = "[???]";
            lblAppliedFor.Text = "[???]";
            lblPassedTests.Text = "0";
            ctrlApplicationBasicInfo1.ResetData();
        }
        public void LoadData(int localDrivingLicenseApplicationID)
        {
            
            localDrivingLicenseApplication= clsLocalDrivingLicenseApplication._GetLocalDrivingLicenseApplicatioInfoByID(localDrivingLicenseApplicationID);
            if(localDrivingLicenseApplication == null)
            {
                ResetData();
                MessageBox.Show("No Application with ApplicationID = " + _localDrivingLicenseApplicationID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            FillData();
        }
        public void FillData()
        {
            _LicenseID=localDrivingLicenseApplication._GetActiveLicense();
            llShowLicenceInfo.Enabled = (_LicenseID != -1);
            _localDrivingLicenseApplicationID = localDrivingLicenseApplication.LocalDrivingLicenseApplicationID;
            lblLocalDrivingLicenseApplicationID.Text= _localDrivingLicenseApplicationID.ToString();
            lblAppliedFor.Text= localDrivingLicenseApplication.LicenseClass.ClassName.ToString();
            lblPassedTests.Text= localDrivingLicenseApplication._GetPassedTestCount().ToString() + "/3";
            ctrlApplicationBasicInfo1.LoadData(localDrivingLicenseApplication.ApplicationID);
        }

        private void llShowLicenceInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLicenseInfo frm = new frmShowLicenseInfo(_LicenseID);
            frm.ShowDialog();
        }
    }
}
