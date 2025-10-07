using DVLD_BuisnessLayer;
using DVLD_FINAL.Settings;
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
    public partial class frmEditApplicationType : Form
    {
        private clsApplicationType applicationType;
        private int _ApplicationTypeID = -1;
        private decimal value = -1;
        public frmEditApplicationType(int applicationTypeID )
        {
            InitializeComponent();
            _ApplicationTypeID = applicationTypeID;
        }

        private void frmEditApplicationType_Load(object sender, EventArgs e)
        {
            _FillData();
        }
        private void _FillData()
        {
            applicationType=clsApplicationType._GetApplicationTypeByApplicationTypeID(_ApplicationTypeID);
            if(applicationType == null)
            {
                MessageBox.Show($"ApplicationType with ID :{_ApplicationTypeID} is not Found");
                this.Close();
                return;
            }
            lblApplicationTypeID.Text= applicationType.ApplicationTypeID.ToString();
            txtTitle.Text= applicationType.ApplicationTypeTitle.ToString();
            txtFees.Text= applicationType.ApplicationFees.ToString();

        }

        private void txtTitle_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtTitle.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtTitle, "Title Cant Be blank");
            }
            else
            {
                e.Cancel= false;
            }
        }

        private void txtFees_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFees.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFees, "Fees Cant Be empty");
            }
            else
            {
                if(decimal.TryParse(txtFees.Text,out value))
                {
                    e.Cancel = false;
                    errorProvider1.SetError(txtFees, null);
                }
                else
                {
                    errorProvider1.SetError(txtFees, "Fees Must be A number");
                    e.Cancel = true;
                }

            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            applicationType.ApplicationTypeTitle = txtTitle.Text;
            applicationType.ApplicationFees=value;
            if (applicationType._UpdateApplication())
            {
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                this.Close();
            }
            else
            {
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
