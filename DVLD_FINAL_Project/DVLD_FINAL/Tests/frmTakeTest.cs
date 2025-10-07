using DVLD_BuisnessLayer;
using DVLD_FINAL.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Tests
{
    public partial class frmTakeTest: Form
    {
        private clsTest _test;
        private int _testID;
        private int _appointmentID = -1;
        clsTestType.enTestType _testType = clsTestType.enTestType.VisionTest;


        public frmTakeTest(int AppointmentID,clsTestType.enTestType TestType )
        {
            InitializeComponent();
            _appointmentID = AppointmentID;
            _testType = TestType;
        }

        private void frmTakeTest_Load(object sender, EventArgs e)
        {
            ctrlSecheduledTest1.testType = _testType;
            ctrlSecheduledTest1.LoadData(_appointmentID);
            if (_appointmentID == -1)
                btnSave.Enabled = false;
            else
                btnSave.Enabled = true;
              int testID = ctrlSecheduledTest1.TestID;
            if (testID != -1)
            {
                _test=clsTest._GetTestInfoByID(ctrlSecheduledTest1.TestID);
                    txtNotes.Text = _test.Notes;
                    if(_test.TestResult)
                    {
                        rbPass.Checked = true;
                    }
                    else
                    {
                       rbFail.Checked = true;
                    }
                lblUserMessage.Visible = true;
                rbPass.Enabled = false;
                rbFail.Enabled = false;
                txtNotes.Enabled = false;
                btnSave.Enabled = false;
            }
            else
            {
                _test = new clsTest();
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to save", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }
            _test.TestAppointmentID = _appointmentID;
            _test.Notes = txtNotes.Text.Trim();
            _test.TestResult = rbPass.Checked;
            _test.CreatedByUserID= clsGlopal.LoggedInUser.UserID;
             if(_test.Save())
             {
                 MessageBox.Show("Data Saved Successfully", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                 btnSave.Enabled = false;
                 this.Close();
             }
            else
            {
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
