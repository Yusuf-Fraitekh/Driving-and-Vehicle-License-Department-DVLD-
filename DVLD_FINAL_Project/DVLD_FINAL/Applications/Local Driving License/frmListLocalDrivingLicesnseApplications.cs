using DVLD.Applications;
using DVLD.DriverLicense;
using DVLD.Licenses.International_License;
using DVLD.User;
using DVLD_BuisnessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DVLD.Tests
{
    public partial class frmListLocalDrivingLicesnseApplications : Form
    {
    
        private DataTable _dtAllLocalDrivingLicenseApplications;
        public frmListLocalDrivingLicesnseApplications()
        {
            InitializeComponent();
        }
        private void _Load()
        {
            _dtAllLocalDrivingLicenseApplications = clsLocalDrivingLicenseApplication._GetAllLocalDrivingLicenseApplications();
            dgvLocalDrivingLicenseApplications.DataSource = _dtAllLocalDrivingLicenseApplications;
            if (dgvLocalDrivingLicenseApplications.Rows.Count > 0)
            {

                dgvLocalDrivingLicenseApplications.Columns[0].HeaderText = "L.D.L.AppID";
                dgvLocalDrivingLicenseApplications.Columns[0].Width = 120;

                dgvLocalDrivingLicenseApplications.Columns[1].HeaderText = "Driving Class";
                dgvLocalDrivingLicenseApplications.Columns[1].Width = 300;

                dgvLocalDrivingLicenseApplications.Columns[2].HeaderText = "National No.";
                dgvLocalDrivingLicenseApplications.Columns[2].Width = 150;

                dgvLocalDrivingLicenseApplications.Columns[3].HeaderText = "Full Name";
                dgvLocalDrivingLicenseApplications.Columns[3].Width = 350;

                dgvLocalDrivingLicenseApplications.Columns[4].HeaderText = "Application Date";
                dgvLocalDrivingLicenseApplications.Columns[4].Width = 170;

                dgvLocalDrivingLicenseApplications.Columns[5].HeaderText = "Passed Tests";
                dgvLocalDrivingLicenseApplications.Columns[5].Width = 150;
            }
            lblRecordsCount.Text = dgvLocalDrivingLicenseApplications.Rows.Count.ToString();
        }
        private void frmListLocalDrivingLicesnseApplications_Load(object sender, EventArgs e)
        {
            cbFilterBy.SelectedIndex = 0;
            _Load();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible = (cbFilterBy.Text != "None");
            if(txtFilterValue.Visible)
            {
              txtFilterValue.Text = "";
              txtFilterValue.Focus();
            }
            //_dtAllLocalDrivingLicenseApplications.DefaultView.RowFilter = "";
            //lblRecordsCount.Text = _dtAllLocalDrivingLicenseApplications.Rows.Count.ToString();

        }
        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text == "L.D.L.AppID")
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
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
                case "L.D.L.AppID":
                    Filter = "LocalDrivingLicenseApplicationID";
                    break;

                case "National No.":
                    Filter = "NationalNo";
                    break;

                case "Full Name":
                    Filter = "FullName";
                    break;
                case "Status":
                    Filter = "Status";
                    break;

                default:
                    Filter = "None";
                    break;

            }
            if (txtFilterValue.Text.Trim() == "" || Filter == "None")
            {
                _dtAllLocalDrivingLicenseApplications.DefaultView.RowFilter = "";
                lblRecordsCount.Text = _dtAllLocalDrivingLicenseApplications.Rows.Count.ToString();
                return;
            }

            if (Filter == "LocalDrivingLicenseApplicationID")
            {
                _dtAllLocalDrivingLicenseApplications.DefaultView.RowFilter = string.Format("[{0}] = {1}", Filter, txtFilterValue.Text.Trim());
            }
            else
            {
                _dtAllLocalDrivingLicenseApplications.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", Filter, txtFilterValue.Text.Trim());
            }

            lblRecordsCount.Text = dgvLocalDrivingLicenseApplications.Rows.Count.ToString();
        }

     

        private void btnAddNewApplication_Click(object sender, EventArgs e)
        {
            frmAddUpdateLocalDrivingLicesnseApplication frm = new frmAddUpdateLocalDrivingLicesnseApplication();
            frm.ShowDialog();
            frmListLocalDrivingLicesnseApplications_Load(null, null);

        }
        
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
             int localDrivingLicenseID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;
             clsLocalDrivingLicenseApplication localDrivingLicenseApplication= clsLocalDrivingLicenseApplication._GetLocalDrivingLicenseApplicatioInfoByID(localDrivingLicenseID);
             frmAddUpdateLocalDrivingLicesnseApplication frm = new frmAddUpdateLocalDrivingLicesnseApplication(localDrivingLicenseID);
             frm.ShowDialog();
            frmListLocalDrivingLicesnseApplications_Load(null, null);
        }

        private void CancelApplicaitonToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
                if(MessageBox.Show("Are you sure you want to cancel this application","Confirm",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.No)
                {
                  return;
                }
           
            int localDrivingLicenseID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value; 
            clsLocalDrivingLicenseApplication localDrivingLicenseApplication = clsLocalDrivingLicenseApplication._GetLocalDrivingLicenseApplicatioInfoByID(localDrivingLicenseID);
            if (localDrivingLicenseApplication != null)
            {
                if (localDrivingLicenseApplication._UpdateApplicationStatus(clsApplication.enApplicationStatus.Cancelled))
                {
                    MessageBox.Show("Application Cancelled Successfully", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else
                {
                    MessageBox.Show("Could not cancel applicatoin.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                frmListLocalDrivingLicesnseApplications_Load(null, null);
            }
        }

        private void DeleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
                if (MessageBox.Show("Are you sure you want to Delete this application", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return; 
                }
                
                int localDrivingLicenseID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;
                clsLocalDrivingLicenseApplication LocalDrivingLicense = clsLocalDrivingLicenseApplication._GetLocalDrivingLicenseApplicatioInfoByID(localDrivingLicenseID);
                if(LocalDrivingLicense != null)
               {
                   if (LocalDrivingLicense._Delete())
                  {
                      MessageBox.Show("Application Deleted Successfully", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                  }
                  else
                  {
                    MessageBox.Show("Could not delete applicatoin.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                frmListLocalDrivingLicesnseApplications_Load(null, null);
            }  
        }
        private void ScheduleTest(clsTestType.enTestType testType)
        {
            int localDrivingLicenseApplicationID= (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;
            frmListTestAppointments frm = new frmListTestAppointments(localDrivingLicenseApplicationID,testType);
            frm.ShowDialog();
            frmListLocalDrivingLicesnseApplications_Load(null, null);
        }

        private void scheduleVisionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScheduleTest(clsTestType.enTestType.VisionTest);
        }

        private void scheduleWrittenTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScheduleTest(clsTestType.enTestType.WrittenTest);
        }

        private void scheduleStreetTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScheduleTest(clsTestType.enTestType.StreetTest);
        }

        private void cmsApplications_Opening(object sender, CancelEventArgs e)
        {
            int localDrivingLicenseApplicationID= (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;
            clsLocalDrivingLicenseApplication localDrivingLicenseApplication = clsLocalDrivingLicenseApplication._GetLocalDrivingLicenseApplicatioInfoByID(localDrivingLicenseApplicationID);
            int LicenseID = localDrivingLicenseApplication._GetActiveLicense();
            int TotalPassedTestCount= localDrivingLicenseApplication._GetPassedTestCount();
            bool PassedVisionTest = localDrivingLicenseApplication.DoesPassTest(clsTestType.enTestType.VisionTest);
            bool PassedWrittenTest = localDrivingLicenseApplication.DoesPassTest(clsTestType.enTestType.WrittenTest);
            bool PassedStreetTest= localDrivingLicenseApplication.DoesPassTest(clsTestType.enTestType.StreetTest);
            ScheduleTestsMenue.Enabled =(!(PassedVisionTest && PassedWrittenTest && PassedStreetTest) && localDrivingLicenseApplication.ApplicationStatus == clsApplication.enApplicationStatus.New);
            editToolStripMenuItem.Enabled = localDrivingLicenseApplication.ApplicationStatus == clsApplication.enApplicationStatus.New;
            CancelApplicaitonToolStripMenuItem.Enabled = DeleteApplicationToolStripMenuItem.Enabled = editToolStripMenuItem.Enabled;
            issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = (TotalPassedTestCount == 3) && (LicenseID == -1);
            showLicenseToolStripMenuItem.Enabled = (LicenseID != -1);

            if(ScheduleTestsMenue.Enabled)
            {
                scheduleVisionTestToolStripMenuItem.Enabled = !PassedVisionTest;
                scheduleWrittenTestToolStripMenuItem.Enabled = PassedVisionTest && !PassedWrittenTest;
                scheduleStreetTestToolStripMenuItem.Enabled = PassedVisionTest && PassedWrittenTest && !PassedStreetTest;
            }
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int localDrivingLicenseApplicationID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;
            frmLocalDrivingLicenseApplicationInfo frm = new frmLocalDrivingLicenseApplicationInfo(localDrivingLicenseApplicationID);
            frm.ShowDialog();
            frmListLocalDrivingLicesnseApplications_Load(null, null);

        }

        private void issueDrivingLicenseFirstTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            int localDrivingLicenseApplicationID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;
            frmIssueDriverLicenseFirstTime frm = new frmIssueDriverLicenseFirstTime(localDrivingLicenseApplicationID);
            frm.ShowDialog();
            frmListLocalDrivingLicesnseApplications_Load(null, null);
        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int localDrivingLicenseApplicationID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;
            
            int LicenseID = clsLocalDrivingLicenseApplication._GetLocalDrivingLicenseApplicatioInfoByID(localDrivingLicenseApplicationID)._GetActiveLicense();
            if(LicenseID != -1)
            {
                frmShowLicenseInfo frm = new frmShowLicenseInfo(LicenseID);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("No License Found!", "No License", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }    
                frmListLocalDrivingLicesnseApplications_Load(null, null);
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;
            clsLocalDrivingLicenseApplication localDrivingLicenseApplication = clsLocalDrivingLicenseApplication._GetLocalDrivingLicenseApplicatioInfoByID(LocalDrivingLicenseApplicationID);
            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(localDrivingLicenseApplication.ApplicantPersonID);
            frm.ShowDialog();
        }
    }
}
