using DVLD.Controls.ApplicationControls;
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
using static DVLD_BuisnessLayer.clsTestType;

namespace DVLD.Tests
{
    public partial class frmListTestAppointments: Form
    {
        private int _localDrivingLicenseApplicationID = -1;
        private DataTable testAppointmentTable;
        private clsTestType.enTestType _testType=clsTestType.enTestType.VisionTest;

        public frmListTestAppointments(int localDrivingLicenseApplicationID,clsTestType.enTestType testType)
        {
            _testType=testType;
            _localDrivingLicenseApplicationID = localDrivingLicenseApplicationID;
            InitializeComponent();
        }
        private void _LoadData()
        {
            testAppointmentTable = clsTestAppointment._GetAllTestAppointmentsPerTestType(_localDrivingLicenseApplicationID, _testType);
            dgvLicenseTestAppointments.DataSource = testAppointmentTable;
            lblRecordsCount.Text = dgvLicenseTestAppointments.Rows.Count.ToString();
        }
        private void ChangeTitle(clsTestType.enTestType testType)
        {
            switch(testType)
            {
                case clsTestType.enTestType.VisionTest:
                    lblTitle.Text= testType.ToString();
                    break;
                case clsTestType.enTestType.WrittenTest:
                    lblTitle.Text = testType.ToString();
                    break;
                case clsTestType.enTestType.StreetTest:
                    lblTitle.Text = testType.ToString();
                    break;
            }
        }
        private void frmListTestAppointments_Load(object sender, EventArgs e)
        {
            ChangeTitle(_testType);
            ctrlDrivingLicenseApplicationInfo1.LoadData(_localDrivingLicenseApplicationID);
            testAppointmentTable= clsTestAppointment._GetAllTestAppointmentsPerTestType(_localDrivingLicenseApplicationID, _testType);
            dgvLicenseTestAppointments.DataSource = testAppointmentTable;
            lblRecordsCount.Text = dgvLicenseTestAppointments.Rows.Count.ToString();
            if (dgvLicenseTestAppointments.Rows.Count > 0)
            {
                dgvLicenseTestAppointments.Columns[0].HeaderText = "Appointment ID";
                dgvLicenseTestAppointments.Columns[0].Width = 150;

                dgvLicenseTestAppointments.Columns[1].HeaderText = "Appointment Date";
                dgvLicenseTestAppointments.Columns[1].Width = 200;

                dgvLicenseTestAppointments.Columns[2].HeaderText = "Paid Fees";
                dgvLicenseTestAppointments.Columns[2].Width = 150;
      
                dgvLicenseTestAppointments.Columns[3].HeaderText = "Is Locked";
                dgvLicenseTestAppointments.Columns[3].Width = 100;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddNewAppointment_Click(object sender, EventArgs e)
        {
            clsLocalDrivingLicenseApplication localDrivingLicenseApplication= clsLocalDrivingLicenseApplication._GetLocalDrivingLicenseApplicatioInfoByID(_localDrivingLicenseApplicationID);
            if (localDrivingLicenseApplication._DoesHaveActiveTestAppointment(_testType)) 
            {
                MessageBox.Show("Person Already have an active appointment for this test,You cannot add new appointment",
                    "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
           if(localDrivingLicenseApplication.DoesPassTest(_testType))
            {
                MessageBox.Show("Person Already Passed this test,You cannot add new appointment",
                                    "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            frmScheduleTest frm = new frmScheduleTest(_localDrivingLicenseApplicationID, _testType);
            frm.ShowDialog();
            frmListTestAppointments_Load(null,null);

        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int AppointmentID= (int)dgvLicenseTestAppointments.CurrentRow.Cells[0].Value;
            frmScheduleTest frm =new frmScheduleTest(_localDrivingLicenseApplicationID, _testType, AppointmentID);
            frm.ShowDialog();
            frmListTestAppointments_Load(null, null);
        }

        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            int AppointmentID = (int)dgvLicenseTestAppointments.CurrentRow.Cells[0].Value;
            frmTakeTest frm = new frmTakeTest(AppointmentID, _testType);
            frm.ShowDialog();
            frmListTestAppointments_Load(null, null);
        }

        private void dgvLicenseTestAppointments_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
