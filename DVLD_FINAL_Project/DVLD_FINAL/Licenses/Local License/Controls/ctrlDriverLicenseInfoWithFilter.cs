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

namespace DVLD.Licenses.Controls
{
    public partial class ctrlDriverLicenseInfoWithFilter : UserControl
    {
        public event Action<int> OnLicenseSelected;
        protected virtual void LicenseSelected(int LicenseID)
        {
            Action<int> handler = OnLicenseSelected;
            if(handler != null)
            {
                handler(LicenseID);
            }
        }
        int _licenseID = -1;
        public int LicenseID
        {
            get
            {
                return ctrlDriverLicenseInfo1.LicenseID;
            }
        }
        public clsLicense SelectedLicenseInfo
        {
            get 
            {
                return ctrlDriverLicenseInfo1.SelectedLicenseInfo;
            }
        }
        private bool _FilterEnabled = true;
        public bool FilterEnabled
        {
            get
            {
                return _FilterEnabled;
            }
            set
            {
                _FilterEnabled = value;
                gbFilters.Enabled = _FilterEnabled;
            }
        }
        public ctrlDriverLicenseInfoWithFilter()
        {
            InitializeComponent();
        }
        private void txtLicenseID_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar);
            if (e.KeyChar == (char)13)
            {
                btnFind.PerformClick();
            }
           
        }
        private void btnFind_Click(object sender, EventArgs e)
        {
            Find();
        }
        private void Find()
        {
            _licenseID = Convert.ToInt32(txtLicenseID.Text);
            ctrlDriverLicenseInfo1.LoadData(_licenseID);
            if(OnLicenseSelected != null && FilterEnabled)
            OnLicenseSelected(ctrlDriverLicenseInfo1.LicenseID);
        }
        public void LoadData(int LicenseID)
        {
           
            txtLicenseID.Text = LicenseID.ToString();
            ctrlDriverLicenseInfo1.LoadData(LicenseID);
            _licenseID = ctrlDriverLicenseInfo1.LicenseID;
            if (OnLicenseSelected != null && FilterEnabled)
                OnLicenseSelected(_licenseID);
        }

        private void txtLicenseID_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtLicenseID.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtLicenseID, "This field is required!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtLicenseID, "");
            }
        }
        public void txtLicenseIDFocus()
        {
            txtLicenseID.Focus();
        }
    }
}
