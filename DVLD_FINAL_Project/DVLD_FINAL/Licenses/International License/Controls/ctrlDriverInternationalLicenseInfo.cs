using DVLD_FINAL.Properties;
using DVLD_BuisnessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using DVLD_FINAL.Settings;

namespace DVLD.Licenses.International_Licenses.Controls
{
    public partial class ctrlDriverInternationalLicenseInfo : UserControl
    {
        int _InternationalLicenseID = -1;
        clsInternationalDrivingLicenseApplication internationalLicense;
        public int InternationalLicenseID
        {
            get
            {
                return _InternationalLicenseID;
            }
        }
        public clsInternationalDrivingLicenseApplication SelectedInternationalLicense
        {
            get
            {
                return internationalLicense;
            }
        }
        public ctrlDriverInternationalLicenseInfo()
        {
            InitializeComponent();
        }
        public void LoadData(int internationalLicenseID)
        {
            _InternationalLicenseID = internationalLicenseID;
            internationalLicense = clsInternationalDrivingLicenseApplication._GetInternationalDrivingLicenseApplicatioInfoByID(internationalLicenseID);
            if (internationalLicense == null)
            {

                MessageBox.Show("Could not find License ID = " + _InternationalLicenseID.ToString(),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _InternationalLicenseID = -1;
                return;
            }
            FillData();
        }
        private void HandlePersonImage()
        {
            string ImagePath = internationalLicense.DriverInfo.PersonInfo.ImagePath;
            if (ImagePath != "")
            {
                pbPersonImage.Load(ImagePath);
            }
            else
            {
                pbPersonImage.Image = internationalLicense.DriverInfo.PersonInfo.Gendor == 0 ?
                    DVLD_FINAL.Properties.Resources.Male_512 : DVLD_FINAL.Properties.Resources.Female_512;
            }
        }
        private void FillData()
        {
            lblFullName.Text = internationalLicense.DriverInfo.PersonInfo.FullName;
            lblInternationalLicenseID.Text = internationalLicense.InernationalDrivingLicenseApplicationID.ToString();
            lblLocalLicenseID.Text = internationalLicense.LicenseID.ToString();
            lblNationalNo.Text = internationalLicense.DriverInfo.PersonInfo.NationalNo.ToString();
            lblGendor.Text = internationalLicense.DriverInfo.PersonInfo.Gendor == 1 ? "FeMale" : "Male";
            lblIssueDate.Text = clsFormat.DateToShort(internationalLicense.IssueDate);
            lblApplicationID.Text = internationalLicense.ApplicationID.ToString();
            lblIsActive.Text = internationalLicense.IsActive == true ? "Yes" : "No";
            
            lblDateOfBirth.Text = clsFormat.DateToShort(internationalLicense.DriverInfo.PersonInfo.DateOfBirth);
            lblDriverID.Text = internationalLicense.DriverID.ToString();
            lblExpirationDate.Text = clsFormat.DateToShort(internationalLicense.ExpirationDate);
            HandlePersonImage();
        }


       

       

        

     
    }
}
