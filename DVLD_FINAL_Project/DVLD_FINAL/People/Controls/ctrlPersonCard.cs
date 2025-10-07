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
using System.Xml.Linq;
using System.IO;
using DVLD.People;

namespace DVLD.Controls
{
    public partial class ctrlPersonCard : UserControl
    {
        private clsPerson Person;
        private int _PersonID = -1;
        public int PersonID 
        {
            get 
            { 
                return _PersonID;
            } 
        }
        public clsPerson SelectedPersonInfo 
        {
            get { return Person; }
        }
        public ctrlPersonCard()
        {
            InitializeComponent();
        }
        public void ResetData()
        {
            _PersonID = -1;
            lblPersonID.Text = "[????]";
            lblNationalNo.Text = "[????]";
            lblFullName.Text = "[????]";
            pbGendor.Image = Resources.Man_32;
            lblGendor.Text = "[????]";
            lblEmail.Text = "[????]";
            lblPhone.Text = "[????]";
            lblDateOfBirth.Text = "[????]";
            lblCountry.Text = "[????]";
            lblAddress.Text = "[????]";
            pbPersonImage.Image = Resources.Male_512;
        }
        
        public void LoadData(int PersonID)
        {
            Person = clsPerson._GetPersonInfo(PersonID);
            if(Person == null)
            {
                ResetData();
                MessageBox.Show("No Person with PersonID = " + PersonID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            FillData();
        }
        public void LoadData(string NationalNo)
        {
            Person = clsPerson._GetPersonInfo(NationalNo);
            if (Person == null)
            {
                ResetData();
                MessageBox.Show("No Person with NationalNo = " + NationalNo, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            FillData();
        }
        private void _LoadPersonImage()
        {
            if(Person.Gendor == 0)
                pbPersonImage.Image = Resources.Male_512;
            else
                pbPersonImage.Image=Resources.Female_512;
            string ImagePath = Person.ImagePath;
            if(ImagePath != "")
                if(File.Exists(ImagePath))
                    pbPersonImage.ImageLocation = ImagePath;
                else
                    MessageBox.Show("Could not find this image: =" + ImagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
        }
        public void FillData()
        {
            llEditPersonInfo.Enabled = true;
            _PersonID = Person.PersonID;
            lblPersonID.Text = Person.PersonID.ToString();
            lblNationalNo.Text = Person.NationalNo;
            lblFullName.Text = Person.FullName;
            lblGendor.Text = Person.Gendor == 0 ? "Male" : "Female";
            lblEmail.Text = Person.Email;
            lblPhone.Text = Person.Phone;
            lblDateOfBirth.Text = Person.DateOfBirth.ToShortDateString();
            lblCountry.Text = clsCountry._GetCountryInfoBy(Person.CountryID).CountryName;
            lblAddress.Text = Person.Address;
            _LoadPersonImage();
        }

        private void llEditPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAddUpdatePerson frmAddUpdatePerson = new frmAddUpdatePerson(_PersonID);
            frmAddUpdatePerson.ShowDialog();
             LoadData(PersonID);
            
        }
       
    }
}
