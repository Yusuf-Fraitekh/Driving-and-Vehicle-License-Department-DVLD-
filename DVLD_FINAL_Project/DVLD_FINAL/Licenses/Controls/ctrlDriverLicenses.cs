using DVLD.Controls;
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
using DVLD;
using DVLD.Licenses.International_Licenses;
using DVLD.DriverLicense;

namespace DVLD.Licenses.Local_Licenses.Controls
{
    public partial class ctrlDriverLicenses : UserControl
    {
        private DataTable _dtDriverLocalLicensesHistory;
        private DataTable _dtDriverInternationalLicensesHistory;
        private clsDriver driver;
        private int _DriverID = -1;
        public ctrlDriverLicenses()
        {
            InitializeComponent();
        }
        private void LoadLocalLicenses()
        {
            _dtDriverLocalLicensesHistory = clsLicense._GetDriverLicenses(_DriverID);
            dgvLocalLicensesHistory.DataSource = _dtDriverLocalLicensesHistory;
            lblLocalLicensesRecords.Text = dgvLocalLicensesHistory.Rows.Count.ToString();

            if (dgvLocalLicensesHistory.Rows.Count > 0)
            {
                dgvLocalLicensesHistory.Columns[0].HeaderText = "Lic.ID";
                dgvLocalLicensesHistory.Columns[0].Width = 110;

                dgvLocalLicensesHistory.Columns[1].HeaderText = "App.ID";
                dgvLocalLicensesHistory.Columns[1].Width = 110;

                dgvLocalLicensesHistory.Columns[2].HeaderText = "Class Name";
                dgvLocalLicensesHistory.Columns[2].Width = 270;

                dgvLocalLicensesHistory.Columns[3].HeaderText = "Issue Date";
                dgvLocalLicensesHistory.Columns[3].Width = 170;

                dgvLocalLicensesHistory.Columns[4].HeaderText = "Expiration Date";
                dgvLocalLicensesHistory.Columns[4].Width = 170;

                dgvLocalLicensesHistory.Columns[5].HeaderText = "Is Active";
                dgvLocalLicensesHistory.Columns[5].Width = 110;

            }
        }
        private void LoadInternationalLicenses()
        {
            _dtDriverInternationalLicensesHistory = clsInternationalDrivingLicenseApplication._GetDriverInternationalDrivingLicenseApplications(_DriverID);
            dgvInternationalLicensesHistory.DataSource = _dtDriverInternationalLicensesHistory;
            lblInternationalLicensesRecords.Text = dgvInternationalLicensesHistory.Rows.Count.ToString();

            if (dgvInternationalLicensesHistory.Rows.Count > 0)
            {
                dgvInternationalLicensesHistory.Columns[0].HeaderText = "Int.License ID";
                dgvInternationalLicensesHistory.Columns[0].Width = 160;

                dgvInternationalLicensesHistory.Columns[1].HeaderText = "Application ID";
                dgvInternationalLicensesHistory.Columns[1].Width = 130;

                dgvInternationalLicensesHistory.Columns[2].HeaderText = "L.License ID";
                dgvInternationalLicensesHistory.Columns[2].Width = 130;

                dgvInternationalLicensesHistory.Columns[3].HeaderText = "Issue Date";
                dgvInternationalLicensesHistory.Columns[3].Width = 180;

                dgvInternationalLicensesHistory.Columns[4].HeaderText = "Expiration Date";
                dgvInternationalLicensesHistory.Columns[4].Width = 180;

                dgvInternationalLicensesHistory.Columns[5].HeaderText = "Is Active";
                dgvInternationalLicensesHistory.Columns[5].Width = 120;

            }
        }
        public void LoadDataByDriverID(int DriverID)
        {
            driver = clsDriver._GetDriverInfoByDriverID(DriverID);
            if (driver == null)
            {
                MessageBox.Show("There is no driver with id ="+DriverID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _DriverID = driver.DriverID;
            LoadLocalLicenses();
            LoadInternationalLicenses();
        }
        public void LoadData(int PersonID)
        {
            driver = clsDriver._GetDriverInfoByPersonID(PersonID);
            if(driver == null)
            {
                MessageBox.Show("There is no driver linked with person with id = "+PersonID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _DriverID = driver.DriverID;
            LoadLocalLicenses();
            LoadInternationalLicenses();
         
        }

        private void InternationalLicenseHistorytoolStripMenuItem_Click(object sender, EventArgs e)
        {
            int InternationalLicenseID= (int)dgvInternationalLicensesHistory.CurrentRow.Cells[0].Value;
            frmShowInternationalLicenseInfo frm = new frmShowInternationalLicenseInfo(InternationalLicenseID);
            frm.ShowDialog();
        }

        private void showLicenseInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID= (int)dgvLocalLicensesHistory.CurrentRow.Cells[0].Value;
            frmShowLicenseInfo frm = new frmShowLicenseInfo(LicenseID);
            frm.ShowDialog();
        }
        public void CLear()
        {
            _dtDriverInternationalLicensesHistory.Clear();
            _dtDriverLocalLicensesHistory.Clear();
        }
    }
}
