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

namespace DVLD.Controls
{
    public partial class ctrlUserCard : UserControl
    {
        private int _UserID=-1;
        private clsUser User;
        public int UserID
        {
            get { return _UserID; }
        }
        public ctrlUserCard()
        {
            InitializeComponent();
        }
        public void _ResetData()
        {
            ctrlPersonCard1.ResetData();
            lblUserID.Text = "???";
            lblUserName.Text = "???";
            lblIsActive.Text = "???";
        }
        public void FillData()
        {
            ctrlPersonCard1.LoadData(User.PersonID);
            lblUserID.Text=User.UserID.ToString();
            lblUserName.Text = User.UserName.ToString();
            lblIsActive.Text = User.IsActive == 1 ? "Yes" : "No";
        }
        public void LoadData(int UserID)
        {
            _UserID=UserID;
            User=clsUser._GetUserInfoBy(_UserID);
            if(User==null)
            {
                _ResetData();
                MessageBox.Show("No User with UserID = " + UserID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            FillData();


        }

        
    }
}
