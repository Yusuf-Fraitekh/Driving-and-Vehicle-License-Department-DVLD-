using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.DriverLicense
{
    public partial class frmShowLicenseInfo : Form
    {
        int _licenseID = -1;
        public frmShowLicenseInfo(int licenseID)
        {
            InitializeComponent();
            _licenseID = licenseID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmShowLicenseInfo_Load(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfo1.LoadData(_licenseID);
        }
        
    }
}
