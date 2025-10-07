
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
using DVLD.Controls;
using DVLD_FINAL.Settings;

namespace DVLD.DriverLicense
{
    public partial class ctrlDriverLicenseInfo : UserControl
    {
        int _licenseID = -1;
        clsLicense license;
        public clsLicense SelectedLicenseInfo
        {
            get
            {
                return license;
            }
        }
       
        public int LicenseID
        {
            get
            {
                return _licenseID;
            }
        }
        public ctrlDriverLicenseInfo()
        {
            InitializeComponent();
        }
        public void LoadData(int LicenseID)
        {
            _licenseID = LicenseID;
            license = clsLicense._GetLicenseInfoByLicenseID(_licenseID);
            if (license == null)
            {

                MessageBox.Show("Could not find License ID = " + _licenseID.ToString(),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _licenseID = -1;
                return;
            }
            FillData();
        }
        private void FillData()
        {

            lblClass.Text = license.LicenseClassInfo.ClassName;
            lblFullName.Text = license.DriverInfo.PersonInfo.FullName;
            lblLicenseID.Text = license.LicenseID.ToString();
            lblNationalNo.Text = license.DriverInfo.PersonInfo.NationalNo.ToString();
            lblGendor.Text = license.DriverInfo.PersonInfo.Gendor == 1 ? "FeMale" : "Male";
            lblIssueDate.Text = clsFormat.DateToShort(license.IssueDate);
            lblIssueReason.Text = license.GetIssueReasonText();
            lblNotes.Text = license.Notes == "" ? "No Notes" : license.Notes.ToString();
            lblIsActive.Text = license.IsActive ? "Yes" : "No";
            lblDateOfBirth.Text = clsFormat.DateToShort(license.DriverInfo.PersonInfo.DateOfBirth);
            lblDriverID.Text = license.DriverID.ToString();
            lblExpirationDate.Text = clsFormat.DateToShort(license.ExpirationDate);
            lblIsDetained.Text = license.IsLicenseDetained ? "Yes" : "No";
            LoadPersonImage();
        }
        private void LoadPersonImage()
        {
            string ImagePath = license.DriverInfo.PersonInfo.ImagePath;
            if (ImagePath != "" )
            {
                if(File.Exists(ImagePath))
                pbPersonImage.Load(ImagePath);
                else
                    MessageBox.Show("Could not find this image: = " + ImagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                pbPersonImage.Image= license.DriverInfo.PersonInfo.Gendor == 0 ? 
                    DVLD_FINAL.Properties.Resources.Male_512 :  DVLD_FINAL.Properties.Resources.Female_512;
            }
        }
    }
}
