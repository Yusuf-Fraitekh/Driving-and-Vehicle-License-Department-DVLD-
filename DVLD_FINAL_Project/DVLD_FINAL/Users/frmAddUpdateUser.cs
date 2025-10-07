using DVLD.Controls;
using DVLD_BuisnessLayer;
using DVLD_FINAL.Properties;
using DVLD_FINAL.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.User
{
    public partial class frmAddUpdateUser: Form
    {
        enum enMode { AddNew=0, Update=1};
        enMode Mode;
        private clsUser User;
        private int _UserID=-1;
       
        public frmAddUpdateUser()
        {
            InitializeComponent();
            Mode = enMode.AddNew;
        }
        public frmAddUpdateUser(int UserID)
        {
            InitializeComponent();
            Mode = enMode.Update;
            _UserID = UserID;
        }
        public void _ResetData()
        {
            if(Mode == enMode.AddNew)
            {
                lblTitle.Text = "Add New User";
                this.Text = "Add New User";
                User = new clsUser();
                tpLoginInfo.Enabled = false;
            }
            else
            {
                lblTitle.Text = "Update User";
                this.Text = "Update User";
                tpLoginInfo.Enabled = true;
                btnSave.Enabled = true;
            }

            
            lblUserID.Text = "???";
            txtUserName.Text= "";
            txtPassword.Text= "";
            txtConfirmPassword.Text = "";
            chkIsActive.Checked = true;
        }
        public void FillData()
        {
            User = clsUser._GetUserInfoBy(_UserID);
            ctrlPersonCardWithFilter1.FilterEnabled = false;
            if (User == null )
            {
                MessageBox.Show($"User with ID :{_UserID} is not Found");
                this.Close();
                return;
            }
      
            ctrlPersonCardWithFilter1.LoadData(User.PersonID);
            lblUserID.Text = User.UserID.ToString(); 
            txtUserName.Text = User.UserName;
            txtPassword.Text = User.Password;
            txtConfirmPassword.Text = User.Password;
            chkIsActive.Checked = User.IsActive == 1 ? true : false;
            
                
        }
        private void frmAddUpdateUser_Load(object sender, EventArgs e)
        {
            
            _ResetData();
            if (Mode== enMode.Update)
                 FillData();
                   
            
        }
        

        private void btnPersonInfoNext_Click(object sender, EventArgs e)
        {
            if(Mode == enMode.Update)
            {
                tpLoginInfo.Enabled = true;
                btnSave.Enabled = true;
                tcUserInfo.SelectedTab = tpLoginInfo;
                return;
            }
            if (ctrlPersonCardWithFilter1.PersonID != -1)
            {

                if (clsUser._IsUserExistForPersonID(ctrlPersonCardWithFilter1.PersonID))
                {
                    MessageBox.Show("Selected Person already has a user,choose another one", "Select another Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnSave.Enabled = false;
                }

                else
                {
                    tpLoginInfo.Enabled = true;
                    btnSave.Enabled = true;
                    tcUserInfo.SelectedTab = tpLoginInfo;
                }
            }
            else
            {
                tpLoginInfo.Enabled = false;
                MessageBox.Show("Please Select a person", "Select a Person",MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
            }
        }
        
        private void btnSave_Click(object sender, EventArgs e)
        {

         
            User.PersonID= ctrlPersonCardWithFilter1.PersonID;
            User.Person=clsPerson._GetPersonInfo(ctrlPersonCardWithFilter1.PersonID);
            User.UserName = txtUserName.Text.Trim();
            if(chkIsActive.Checked==true)
            {
                User.IsActive = 1;
            }
            else
            {
                User.IsActive = 0;
            }
            User.Password=txtConfirmPassword.Text.Trim();

            if (User.Save())
            {
                lblUserID.Text = User.UserID.ToString();
                Mode = enMode.Update;
                lblTitle.Text = "Update User";
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
               
            }
            else
            {
               
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
               
            }


        }

        private void TextBox_Validating(object sender, CancelEventArgs e)
        {
  
            if(string.IsNullOrEmpty(txtPassword.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtPassword, "Password Cant Be blank");
            }
            else
            {
                e.Cancel= false;
                errorProvider1.SetError(txtPassword, null);
            }
        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (!clsValidation.PasswordValidation(txtPassword.Text, txtConfirmPassword.Text))
            {
                e.Cancel=true;
                errorProvider1.SetError(txtConfirmPassword, "Password Confirmation does not match Password!");
            }
            else
            {
                e.Cancel=false;
            }
        }

        private void txtUserName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUserName.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtUserName, "UserName Cant Be blank");
                return;
            }
            if (Mode == enMode.AddNew)
            {
                    if(clsUser._IsUserExist(txtUserName.Text))
                    {
                        e.Cancel = true;
                        errorProvider1.SetError(txtUserName, "UserName is Already Taken");
                    }
                    else
                    {
                        e.Cancel= false;
                    }
                
            }
            else
            {
                if(User.UserName != txtUserName.Text.Trim())
                {
                    if (clsUser._IsUserExist(txtUserName.Text))
                    {
                        e.Cancel = true;
                        errorProvider1.SetError(txtUserName, "UserName is Already Taken");
                    }
                    else
                    {
                        e.Cancel= false;
                    }
                }

            }
        }
    }
}
