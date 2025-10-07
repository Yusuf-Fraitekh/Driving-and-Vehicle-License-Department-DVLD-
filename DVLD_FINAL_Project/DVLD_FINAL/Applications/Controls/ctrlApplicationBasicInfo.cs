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

namespace DVLD.Controls.ApplicationControls
{
    public partial class ctrlApplicationBasicInfo : UserControl
    {
        clsApplication _application;
        int _applicationID = -1;

        public int ApplicationID
        {
            get

            {
                return _applicationID;
            }
        }
        public ctrlApplicationBasicInfo()
        {
            InitializeComponent();
  
        }
        public void ResetData()
        {
            lblApplicationID.Text = "[???]";
            lblStatus.Text = "[???]";
            lblType.Text = "[???]";
            lblApplicant.Text = "[????]";
            lblDate.Text = "[??/??/????]";
            lblStatusDate.Text = "[??/??/????]";
            lblCreatedByUser.Text = "[????]";
            lblFees.Text = "[$$$]";
        }
        public void LoadData(int applicationID)
        {
            
            _application=clsApplication._GetApplicationInfoByID(applicationID);
            if(_application == null)
            {
                ResetData();
                MessageBox.Show("No Application with ApplicationID = " + applicationID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
           
            FillData();
        }
        public void FillData()
        {
            _applicationID= _application.ApplicationID;
            lblApplicationID.Text= _application.ApplicationID.ToString();
            lblStatus.Text=_application.ApplicationStatus.ToString();
            lblType.Text=_application.ApplicationTypeInfo.ApplicationTypeTitle.ToString();
            lblApplicant.Text=_application.ApplicantPersonID.ToString();
            lblDate.Text= clsFormat.DateToShort(_application.ApplicationDate);
            lblStatusDate.Text = clsFormat.DateToShort(_application.LastStatus);
            lblCreatedByUser.Text=_application.CreatedByUserIDInfo.UserName;
            lblFees.Text = ((int)_application.PaidFees).ToString();
        }

        private void llViewPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonInfo frm=new frmShowPersonInfo(_application.ApplicantPersonID);
            frm.ShowDialog();
            LoadData(_applicationID);

        }
    }
}
