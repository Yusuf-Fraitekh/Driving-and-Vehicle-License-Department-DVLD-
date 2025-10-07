using DVLD.Applications;
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

namespace DVLD.Tests
{
    public partial class frmListTestTypes : Form
    {
        DataTable table;
        public frmListTestTypes()
        {
            InitializeComponent();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void frmListTestTypes_Load(object sender, EventArgs e)
        {
            _Load();
        }
        private void _Load()
        {
            table = clsTestType._GetAllTestTypes();
            dgvTestTypes.DataSource = table;
            lblRecordsCount.Text = dgvTestTypes.Rows.Count.ToString();
            if(dgvTestTypes.Rows.Count>0)
            {
                dgvTestTypes.Columns[0].HeaderText = "ID";
                dgvTestTypes.Columns[0].Width = 120;
                dgvTestTypes.Columns[1].HeaderText = "Title";
                dgvTestTypes.Columns[1].Width = 200;
                dgvTestTypes.Columns[2].HeaderText = "Description";
                dgvTestTypes.Columns[2].Width = 400;
                dgvTestTypes.Columns[3].HeaderText = "Fees";
                dgvTestTypes.Columns[3].Width = 100;
            }
            

        }
        int rowSelected = -1;
        private void dgvTestTypes_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.Button == MouseButtons.Left || e.Button == MouseButtons.Right) && e.RowIndex >= 0)
            {
                rowSelected = e.RowIndex;
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
                int.TryParse(table.Rows[rowSelected][0].ToString(), out int SelectedID);
                int ID = SelectedID;
                frmEditTestType frm = new frmEditTestType((clsTestType.enTestType)ID);
                frm.ShowDialog();
                _Load();
            }
        }
    }
}
