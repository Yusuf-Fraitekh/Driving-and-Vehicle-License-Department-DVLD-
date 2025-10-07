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
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace DVLD_FINAL
{
    public partial class frmListPeople : Form
    {
        public frmListPeople()
        {
            InitializeComponent();
            
        }
        private static DataTable table= clsPerson._GetAllPeople();
        private DataTable PeopleDataTable= table.DefaultView.ToTable(false, "PersonID", "NationalNo",
                                                       "FirstName", "SecondName", "ThirdName", "LastName",
                                                       "GendorCaption", "DateOfBirth", "CountryName",
                                                       "Phone", "Email");
        private void _LoadData()
        {
            table= clsPerson._GetAllPeople();
            PeopleDataTable = table.DefaultView.ToTable(false, "PersonID", "NationalNo",
                                                       "FirstName", "SecondName", "ThirdName", "LastName",
                                                       "GendorCaption", "DateOfBirth", "CountryName",
                                                       "Phone", "Email"); 
            dgvPeople.DataSource= PeopleDataTable;
            lblRecordsCount.Text = dgvPeople.Rows.Count.ToString();
        }
       

        private void frmListPeople_Load(object sender, EventArgs e)
        {
            dgvPeople.DataSource = PeopleDataTable;
            lblRecordsCount.Text= dgvPeople.Rows.Count.ToString();
            cbFilterBy.Text = cbFilterBy.Items[0].ToString();
            txtFilterValue.Visible = false;
            if (dgvPeople.Rows.Count > 0)
            {

                dgvPeople.Columns[0].HeaderText = "Person ID";
                dgvPeople.Columns[0].Width = 110;

                dgvPeople.Columns[1].HeaderText = "National No.";
                dgvPeople.Columns[1].Width = 120;


                dgvPeople.Columns[2].HeaderText = "First Name";
                dgvPeople.Columns[2].Width = 120;

                dgvPeople.Columns[3].HeaderText = "Second Name";
                dgvPeople.Columns[3].Width = 140;


                dgvPeople.Columns[4].HeaderText = "Third Name";
                dgvPeople.Columns[4].Width = 120;

                dgvPeople.Columns[5].HeaderText = "Last Name";
                dgvPeople.Columns[5].Width = 120;

                dgvPeople.Columns[6].HeaderText = "Gendor";
                dgvPeople.Columns[6].Width = 120;

                dgvPeople.Columns[7].HeaderText = "Date Of Birth";
                dgvPeople.Columns[7].Width = 140;

                dgvPeople.Columns[8].HeaderText = "Nationality";
                dgvPeople.Columns[8].Width = 120;


                dgvPeople.Columns[9].HeaderText = "Phone";
                dgvPeople.Columns[9].Width = 120;


                dgvPeople.Columns[10].HeaderText = "Email";
                dgvPeople.Columns[10].Width = 170;
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbFilterBy.Text!="None")
            {
                txtFilterValue.Visible = true;
            }
            else
            {
                txtFilterValue.Visible = false;
            }
            txtFilterValue.Clear(); 
                

        }
       

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(cbFilterBy.Text =="Person ID")
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
            else
            {
                e.Handled= false;
            }
        }

  
        private void Filteration(object sender, EventArgs e)
        {
           
            string FilterColumn = "";
            switch (cbFilterBy.Text)
            {

                case "Person ID":
                    FilterColumn = "PersonID";
                    break;

                case "National No.":
                    FilterColumn = "NationalNo";
                    break;

                case "First Name":
                    FilterColumn = "FirstName";
                    break;

                case "Second Name":
                    FilterColumn = "SecondName";
                    break;

                case "Third Name":
                    FilterColumn = "ThirdName";
                    break;

                case "Last Name":
                    FilterColumn = "LastName";
                    break;

                case "Nationality":
                    FilterColumn = "CountryName";
                    break;

                case "Gendor":
                    FilterColumn = "GendorCaption";
                    break;

                case "Phone":
                    FilterColumn = "Phone";
                    break;

                case "Email":
                    FilterColumn = "Email";
                    break;

                default:
                    FilterColumn = "None";
                    break;

            }
            if (txtFilterValue.Text.Trim()==""|| FilterColumn == "None")
            {
                PeopleDataTable.DefaultView.RowFilter = "";
                lblRecordsCount.Text = dgvPeople.Rows.Count.ToString();
                return;
            }

            if (FilterColumn == "PersonID")
            
                PeopleDataTable.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterValue.Text.Trim());
           
            else
            
                PeopleDataTable.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilterValue.Text.Trim());
   
            lblRecordsCount.Text = dgvPeople.Rows.Count.ToString();


        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            ShowAddUpdateForm();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ShowAddUpdateForm();
        }
        private void ShowAddUpdateForm()
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson();
            frm.ShowDialog();
            _LoadData();
        }
        int rowSelected = -1;
        private void dgvPeople_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if((e.Button == MouseButtons.Left || e.Button == MouseButtons.Right) &&e.RowIndex>=0)
            {
                rowSelected=e.RowIndex;
            }
        }



        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rowSelected == -1)
            {
                MessageBox.Show($"Please Select a row", "Error Row Selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int.TryParse(PeopleDataTable.Rows[rowSelected][0].ToString(), out int SelectedPerson);
                int PersonID = SelectedPerson;
                frmAddUpdatePerson frm = new frmAddUpdatePerson(PersonID);
                frm.ShowDialog();
                _LoadData();
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
                int.TryParse(PeopleDataTable.Rows[rowSelected][0].ToString(), out int SelectedPerson);
                int PersonID = SelectedPerson;
                if ((MessageBox.Show($"Are you sure you want to delete Person [{PersonID}]", "Confirm Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)) == DialogResult.OK)
                {
                    if (clsPerson._DeletePerson(PersonID))
                    {
                        MessageBox.Show("Person Deleted Successfully.", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Person was not deleted because it has data linked to it.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                int.TryParse(PeopleDataTable.Rows[rowSelected][0].ToString(), out int SelectedPerson);
                int PersonID = SelectedPerson;
                Form frm = new frmShowPersonInfo(PersonID);
                frm.ShowDialog();
            }
                
        }

        
    }
}
