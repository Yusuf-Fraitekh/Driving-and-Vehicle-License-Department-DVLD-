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


    public partial class frmEditTestType: Form
    {
        private clsTestType TestType;
        private clsTestType.enTestType _TestTypeID = clsTestType.enTestType.VisionTest;
        private decimal value = -1;
        public frmEditTestType(clsTestType.enTestType ID)
        {
            InitializeComponent();
            _TestTypeID = ID;
        }
        
        private void _FillData()
        {
            TestType = clsTestType._GetTestTypeByTestTypeID(_TestTypeID);
            if (TestType == null)
            {
                MessageBox.Show($"TestType with ID :{(int)_TestTypeID} is not Found");
                this.Close();
                return;
            }
            lblTestTypeID.Text = ((int)TestType.TestTypeID).ToString();
            txtTitle.Text = TestType.TestTypeTitle.ToString();
            txtFees.Text = TestType.TestTypeFees.ToString();
            txtDescription.Text= TestType.TestTypeDescription.ToString();

        }

        private void txtTitle_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtTitle.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtTitle, "Title Cant Be blank");
            }
            else
            {
                e.Cancel = false;
            }
        }

        private void txtFees_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFees.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFees, "Fees Cant Be blank");
            }
            else
            {
                if (decimal.TryParse(txtFees.Text, out value))
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

        

        private void txtDescription_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtTitle.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtTitle, "Description Cant Be blank");
            }
            else
            {
                e.Cancel = false;
            }
        }

        private void frmEditTestType_Load_1(object sender, EventArgs e)
        {
            _FillData();
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            TestType.TestTypeTitle = txtTitle.Text;
            TestType.TestTypeFees = value;
            TestType.TestTypeDescription = txtDescription.Text;
            if (TestType._UpdateTest())
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
