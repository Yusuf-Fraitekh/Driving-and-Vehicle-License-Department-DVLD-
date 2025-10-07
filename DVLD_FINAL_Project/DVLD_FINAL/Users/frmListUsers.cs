using DVLD.People;
using DVLD_BuisnessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.User
{
    public partial class frmListUsers : Form
    {
        
        private static DataTable _dtAllUsers ;
        
        public frmListUsers()
        {
            InitializeComponent();
        }
        private void _Load()
        {
            _dtAllUsers = clsUser._GetAllUsers();
            dgvUsers.DataSource = _dtAllUsers;
            if(dgvUsers.Rows.Count>0)
            {
                dgvUsers.Columns[0].HeaderText = "User ID";
                dgvUsers.Columns[0].Width = 110;

                dgvUsers.Columns[1].HeaderText = "Person ID";
                dgvUsers.Columns[1].Width = 120;

                dgvUsers.Columns[2].HeaderText = "Full Name";
                dgvUsers.Columns[2].Width = 350;

                dgvUsers.Columns[3].HeaderText = "UserName";
                dgvUsers.Columns[3].Width = 120;

                dgvUsers.Columns[4].HeaderText = "Is Active";
                dgvUsers.Columns[4].Width = 120;
            }
            lblRecordsCount.Text = dgvUsers.Rows.Count.ToString();

        }

        private void frmListUsers_Load(object sender, EventArgs e)
        {
            cbFilterBy.SelectedIndex = 0;
            _Load();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilterBy.Text == "Is Active")
            {
                txtFilterValue.Visible = false;
                cbIsActive.Visible = true;
                cbIsActive.Focus();
                cbIsActive.SelectedIndex = 0;
            }

            else
            {
                txtFilterValue.Visible = (cbFilterBy.Text != "None");
                cbIsActive.Visible = false;
                txtFilterValue.Text = "";
                txtFilterValue.Focus();
            }

            _Load();

        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if((cbFilterBy.Text == "User ID") || (cbFilterBy.Text == "Person ID"))
            {
                if(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
                else
                {
                    e.Handled = false;
                }
            }
            else
            {
                e.Handled = false;
            }
        }
       

        private void Filteration(object sender, EventArgs e)
        {
            string Filter = "";
            switch (cbFilterBy.Text)
            {
                case "User ID":
                    Filter = "UserID";
                    break;
                case "UserName":
                    Filter = "UserName";
                    break;
                case "Person ID":
                    Filter = "PersonID";
                    break;
                case "Full Name":
                    Filter = "FullName";
                    break;
                default:
                    Filter = "None";
                    break;
            }
            
            if (txtFilterValue.Text.Trim() == "" || Filter == "None")
            {
                _dtAllUsers.DefaultView.RowFilter = "";
                lblRecordsCount.Text = dgvUsers.Rows.Count.ToString();
                return;
            }
                if (Filter == "PersonID" || Filter == "UserID")
                {
                _dtAllUsers.DefaultView.RowFilter = string.Format("[{0}] = {1}", Filter, txtFilterValue.Text.Trim());
                }
                else
                {
                _dtAllUsers.DefaultView.RowFilter = string.Format("[{0}] Like '%{1}%'", Filter, txtFilterValue.Text.Trim());
                }
            
            lblRecordsCount.Text=dgvUsers.Rows.Count.ToString();
        }

        private void FilterByIsActive(object sender, EventArgs e)
        {
            string Filter = "IsActive";
            string FilterVal= cbIsActive.Text.Trim();
            switch(FilterVal)
            {
                case "Yes":
                    FilterVal = "1";                   
                    break;
                case "No":
                    FilterVal = "0";                    
                    break;
                default:                  
                    break;
            }
            if (FilterVal == "All")
            {
                _dtAllUsers.DefaultView.RowFilter = "";
                
            }
            else
            {
                _dtAllUsers.DefaultView.RowFilter = string.Format("[{0}] = {1}", Filter, FilterVal);
                
            }
            lblRecordsCount.Text = dgvUsers.Rows.Count.ToString();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void _AddNewUser()
        {
            Form frm = new frmAddUpdateUser();
            frm.ShowDialog();
            _Load();
        }
        private void btnAddUser_Click(object sender, EventArgs e)
        {
            _AddNewUser();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            _AddNewUser();
        }

       
        int rowSelected = -1;
        private void dgvUsers_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.Button == MouseButtons.Left || e.Button == MouseButtons.Right) && e.RowIndex >= 0)
            {
                rowSelected = e.RowIndex;
            }
        }
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
           if(rowSelected == -1)
            {
                MessageBox.Show($"Please Select a row", "Error Row Selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int.TryParse(_dtAllUsers.Rows[rowSelected][0].ToString(), out int SelectedPerson);
                int UserID = SelectedPerson;
                frmAddUpdateUser frm = new frmAddUpdateUser(UserID);
                frm.ShowDialog();
                _Load();
            }
              
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rowSelected == -1)
            {
                MessageBox.Show($"Please Select a row", "Error Row Selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int.TryParse(_dtAllUsers.Rows[rowSelected][0].ToString(), out int SelectedPerson);
                int UserID = SelectedPerson;
                if ((MessageBox.Show($"Are you sure you want to delete User [{UserID}]", "Confirm Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)) == DialogResult.OK)
                {
                    if (clsUser._DeleteUser(UserID))
                    {
                        MessageBox.Show("User Deleted Successfully.", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _Load();
                    }
                    else
                    {
                        MessageBox.Show("User was not deleted because it has data linked to it.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rowSelected == -1)
            {
                MessageBox.Show($"Please Select a row", "Error Row Selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int.TryParse(_dtAllUsers.Rows[rowSelected][0].ToString(), out int SelectedPerson);
                int UserID = SelectedPerson;
                frmUserInfo frm = new frmUserInfo(UserID);
                frm.ShowDialog();
            }
               
        }

        private void ChangePasswordtoolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rowSelected == -1)
            {
                MessageBox.Show($"Please Select a row", "Error Row Selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int.TryParse(_dtAllUsers.Rows[rowSelected][0].ToString(), out int SelectedPerson);
                int UserID = SelectedPerson;
                frmChangePassword frm = new frmChangePassword(UserID);
                frm.ShowDialog();
            }
        }
    }
}
