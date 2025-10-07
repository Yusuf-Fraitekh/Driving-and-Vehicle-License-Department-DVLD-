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

namespace DVLD.Applications
{
    public partial class frmListApplicationTypes : Form
    {
        DataTable table;
     
        public frmListApplicationTypes()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmListApplicationTypes_Load(object sender, EventArgs e)
        {
            _Load();
        }
        private void _Load()
        {
            table = clsApplicationType._GetAllApplicationTypes();
            dgvApplicationTypes.DataSource = table;
            lblRecordsCount.Text = dgvApplicationTypes.Rows.Count.ToString();
            if(dgvApplicationTypes.Rows.Count>0)
            {
                dgvApplicationTypes.Columns[0].HeaderText = "ID";
                dgvApplicationTypes.Columns[0].Width = 110;
                dgvApplicationTypes.Columns[1].HeaderText = "Title";
                dgvApplicationTypes.Columns[1].Width = 400;
                dgvApplicationTypes.Columns[2].HeaderText = "Fees";
                dgvApplicationTypes.Columns[2].Width = 100;
            }
           
            
        }


       
        int rowSelected = -1;
        private void dgvApplicationTypes_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
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
                int ID= SelectedID;
                frmEditApplicationType frm = new frmEditApplicationType(ID);
                frm.ShowDialog();
                _Load();
            }
        }
    }
}
