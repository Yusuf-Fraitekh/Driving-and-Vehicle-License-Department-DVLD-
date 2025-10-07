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

namespace DVLD.User
{
    public partial class frmChangePassword : Form
    {
        private int _UserID = -1;
        private clsUser User;
        public frmChangePassword(int UserID )
        {
            InitializeComponent();
            _UserID=UserID;
        }

        public void ResetData()
        {
            ctrlUserCard1._ResetData();
            txtCurrentPassword.Text="";
            txtNewPassword.Text="";
            txtConfirmPassword.Text="";
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            ResetData();
            User = clsUser._GetUserInfoBy(_UserID);
            if (User == null)
            {
                MessageBox.Show($"User with ID :{_UserID} is not Found");
                this.Close();
                return;
            }
            ctrlUserCard1.LoadData(_UserID);

        }

        private void txtCurrentPassword_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtCurrentPassword.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtCurrentPassword, "Current Password Cant Be blank");
            }
            else
            {
                errorProvider1.SetError(txtCurrentPassword,null);
                if (txtCurrentPassword.Text != User.Password)
                {
                    e.Cancel = true;
                    errorProvider1.SetError(txtCurrentPassword, "Current Password is wrong!");
                }
                else
                {
                    e.Cancel = false;
                }
            }
            
        }

        private void txtNewPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtNewPassword.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNewPassword, "Password Cant Be blank");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtNewPassword, null);
            }
        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (!clsValidation.PasswordValidation(txtNewPassword.Text, txtConfirmPassword.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmPassword, "Password Confirmation does not match New Password!");
            }
            else
            {
                errorProvider1.SetError(txtCurrentPassword,null);
                e.Cancel = false;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
          User.Password = txtConfirmPassword.Text;
            if(User.Save())
            {
                MessageBox.Show("Password Changed Successfully.",
                    "Saved.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetData();
               
            }
            else
            {
                MessageBox.Show("An Erro Occured, Password did not change.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
