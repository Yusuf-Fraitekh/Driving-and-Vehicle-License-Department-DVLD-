using DVLD_BuisnessLayer;
using DVLD_FINAL.Properties;
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
    public partial class frmScheduleTest: Form
    {
        private int _appointmentID = -1;
        private int _localDrivingLicenseApplicationID = -1;
        clsTestType.enTestType _testType= clsTestType.enTestType.VisionTest;
   
        public frmScheduleTest(int localDrivingLicenseApplicationID,clsTestType.enTestType testType,int AppointmentID = -1)
        {
            _localDrivingLicenseApplicationID = localDrivingLicenseApplicationID;
            _appointmentID = AppointmentID;
            _testType = testType;
            InitializeComponent();
        }

        private void frmScheduleTest_Load(object sender, EventArgs e)
        {
            ctrlScheduleTest1.testType = _testType;
            ctrlScheduleTest1.LoadData(_localDrivingLicenseApplicationID, _appointmentID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
