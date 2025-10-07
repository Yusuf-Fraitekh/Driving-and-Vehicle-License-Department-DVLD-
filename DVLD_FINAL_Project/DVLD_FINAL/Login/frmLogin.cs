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
using DVLD_FINAL.Properties;

namespace DVLD.Login
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            bool isActive = false;
            clsUser User = clsUser._GetUserInfoBy(txtUserName.Text, txtPassword.Text);
           
            if(User != null)
            {
                 isActive = User.IsActive == 1 ? true : false;
                if (isActive)
                {
                    if(chkRememberMe.Checked)
                    {
                        DVLD_FINAL.Properties.Settings.Default.UserName= txtUserName.Text;
                        DVLD_FINAL.Properties.Settings.Default.Password= txtPassword.Text;
                        DVLD_FINAL.Properties.Settings.Default.RememberMe = chkRememberMe.Checked;

                    }
                    else
                    {
                        DVLD_FINAL.Properties.Settings.Default.UserName ="";
                        DVLD_FINAL.Properties.Settings.Default.Password = "";
                        DVLD_FINAL.Properties.Settings.Default.RememberMe = chkRememberMe.Checked;
                    }
                    DVLD_FINAL.Properties.Settings.Default.Save();
                    clsGlopal.LoggedInUser = User;
                    frmMain frm =new frmMain();
                    frm.ShowDialog();  
                    this.Close();
                   
                }
                else
                {
                    MessageBox.Show("This User is DeActivated please contact your admin","DeActive User",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Invalid UserName/Password", "Wrong Credintials", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        

        private void frmLogin_Load(object sender, EventArgs e)
        {
            if(DVLD_FINAL.Properties.Settings.Default.RememberMe)
            {
                txtUserName.Text = DVLD_FINAL.Properties.Settings.Default.UserName;
                txtPassword.Text = DVLD_FINAL.Properties.Settings.Default.Password;
                chkRememberMe.Checked = true;
            }
            else
            {
                txtUserName.Text = "";
                txtPassword.Text = "";
                chkRememberMe.Checked= false;
            }
        }
    }
}
