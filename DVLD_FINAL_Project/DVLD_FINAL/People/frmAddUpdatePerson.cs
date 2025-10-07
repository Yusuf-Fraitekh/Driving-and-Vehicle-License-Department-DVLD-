using DVLD_BuisnessLayer;
using DVLD_FINAL.Properties;
using DVLD_FINAL.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace DVLD.People
{
    public partial class frmAddUpdatePerson : Form
    {
        private int _PersonID = -1;
        public delegate void DataBackEventHandler(object sender, int PersonID);
        public event DataBackEventHandler DataBack;
        private enum enMode {AddNew=0,Update=1 };
        private enum enGendor {Male=0,Female=1};
        private enMode Mode= enMode.AddNew;
        
        private enGendor Gendor;
        private clsPerson Person;
        public frmAddUpdatePerson()
        {
            InitializeComponent();
            Mode= enMode.AddNew;
        }
        public frmAddUpdatePerson(int PersonID)
        {
            InitializeComponent();
            Mode = enMode.Update;
            _PersonID=PersonID;
        }
        private void ResetData()
        {
            FillCountries();
             if(Mode ==enMode.AddNew)
            {
                lblTitle.Text = "Add New Person";
                Person = new clsPerson();
            }
            else
            {
                lblTitle.Text = "Update Person";
            }
            if (rbMale.Checked)
                pbPersonImage.Image = Resources.Male_512;
            else
                pbPersonImage.Image = Resources.Female_512;
            txtFirstName.Text = "";
            txtSecondName.Text = "";
            txtThirdName.Text="";
            txtLastName.Text="";
            txtNationalNo.Text="";
            rbMale.Checked = true;
            txtPhone.Text = "";
            txtEmail.Text = "";
            cbCountry.SelectedItem = "Jordan";
            txtAddress.Text = "";
            lblPersonID.Text = "";
            llRemoveImage.Visible = (pbPersonImage.ImageLocation != null);
            dtpDateOfBirth.MinDate = DateTime.Today.AddYears(-100);
            dtpDateOfBirth.MaxDate = DateTime.Today.AddYears(-18);
            dtpDateOfBirth.Value= dtpDateOfBirth.MaxDate;

        }
        private void frmAddUpdatePerson_Load(object sender, EventArgs e)
        {
          ResetData();
          if(Mode == enMode.Update)
             FillData();
                    
        }
        
       
        private void FillData()
        {
           
            Person = clsPerson._GetPersonInfo(_PersonID);
            if (Person == null)
            {
                MessageBox.Show($"Person with ID :{_PersonID} is not Found");
                this.Close();
                return;
            }
                txtFirstName.Text = Person.FirstName;
                txtSecondName.Text = Person.SecondName;
                txtThirdName.Text = Person.ThirdName;
                txtLastName.Text = Person.LastName;
                txtNationalNo.Text = Person.NationalNo;
                dtpDateOfBirth.Value = Person.DateOfBirth;
                if (Gendor == 0)
                {
                    rbMale.Checked = true; 
                }
                else
                {
                    rbFemale.Checked = true;  
                }
                Gendor = (enGendor)Person.Gendor;
                txtPhone.Text = Person.Phone;
                txtEmail.Text = Person.Email;
                string CountryName = clsCountry._GetCountryInfoBy(Person.CountryID).CountryName;
                cbCountry.Text = CountryName;
                txtAddress.Text = Person.Address;
                lblPersonID.Text = Person.PersonID.ToString();
            if (Person.ImagePath != "")
            {
                pbPersonImage.ImageLocation = Person.ImagePath;
            }
            llRemoveImage.Visible = (Person.ImagePath != "");
        }
        
        private void FillCountries()
        {
            cbCountry.Items.Clear();
            DataTable Countries=new DataTable();
            Countries=clsCountry._GetAllCountries();
            foreach(DataRow row in Countries.Rows)
            {
                cbCountry.Items.Add(row["CountryName"]);

            }
            
        }
        private bool _HandlePersonImage()
        {
            if (Person.ImagePath != pbPersonImage.ImageLocation)
            {
                if (Person.ImagePath != "")
                {
                  

                    try
                    {
                        File.Delete(Person.ImagePath);
                    }
                    catch (IOException)
                    {
                       
                    }
                }

                if (pbPersonImage.ImageLocation != null)
                {
                   
                    string SourceImageFile = pbPersonImage.ImageLocation.ToString();

                    if (clsUtil.CopyImageToProjectImagesFolder(ref SourceImageFile))
                    {
                        pbPersonImage.ImageLocation = SourceImageFile;
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Error Copying Image File", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

            }
            return true;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
           if (!_HandlePersonImage())
                return;
            Person.FirstName = txtFirstName.Text;
            Person.SecondName = txtSecondName.Text;
            Person.ThirdName = txtThirdName.Text;
            Person.LastName = txtLastName.Text;
            Person.NationalNo = txtNationalNo.Text;
            Person.DateOfBirth = dtpDateOfBirth.Value;
            if (rbMale.Checked)
            {
                Person.Gendor = (int) enGendor.Male; 
            }
            else
            {
                Person.Gendor =(int) enGendor.Female;
 
            }
            
            Person.Phone = txtPhone.Text;
            Person.Email = txtEmail.Text;
            int CountryID = clsCountry._GetCountryInfoBy(cbCountry.Text).CountryID;
            Person.CountryID = CountryID;
            Person.Address = txtAddress.Text;
            if (pbPersonImage.ImageLocation != null)
                Person.ImagePath = pbPersonImage.ImageLocation;
            else
                Person.ImagePath = "";

            if (Person.Save())
            {
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                lblPersonID.Text=Person.PersonID.ToString();
                Mode = enMode.Update;
                lblTitle.Text = "Update Person";
                DataBack?.Invoke(this, Person.PersonID);
            }
            else
            {
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
            //if(DataBack!=null)
            //DataBack(sender,_PersonID);
        }

        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtNationalNo.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "Please Enter National Number");
            }
            else
            if (txtNationalNo.Text.Trim() != Person.NationalNo && clsPerson._IsPersonExist(txtNationalNo.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "National Number is used for another Person!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtNationalNo, null);
            }
        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            string email;
            email = txtEmail.Text.Trim();
            if (string.IsNullOrEmpty(email))
            {
                e.Cancel = false;
                return;
            }
  
            if (clsValidation.EmailValidation(email))
            {

                e.Cancel = false;
                errorProvider1.SetError(txtEmail, null);
            }
            else
            {
                e.Cancel = true;
                errorProvider1.SetError(txtEmail, "Invalid Email!");
            }  
        }

        private void rbFemale_CheckedChanged(object sender, EventArgs e)
        {
            if(pbPersonImage.ImageLocation == null)
            pbPersonImage.Image = DVLD_FINAL.Properties.Resources.Female_512;
        }

        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {
            if (pbPersonImage.ImageLocation == null)
                pbPersonImage.Image = DVLD_FINAL.Properties.Resources.Male_512;
        }
        private void TextBoxValidation(object sender, CancelEventArgs e)
        {
            TextBox box=(TextBox)sender;
            if(string.IsNullOrEmpty(box.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(box, "Cant Be Empty");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(box, null);
            }

        }

        private void llSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                
                string selectedFilePath = openFileDialog1.FileName;
                pbPersonImage.Load(selectedFilePath);
                llRemoveImage.Visible = true;
                
            }
        }

        private void llRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pbPersonImage.ImageLocation = null;

            if (rbMale.Checked)
                pbPersonImage.Image = Resources.Male_512;
            else
                pbPersonImage.Image = Resources.Female_512;

            llRemoveImage.Visible = false;
        }
    }


}
