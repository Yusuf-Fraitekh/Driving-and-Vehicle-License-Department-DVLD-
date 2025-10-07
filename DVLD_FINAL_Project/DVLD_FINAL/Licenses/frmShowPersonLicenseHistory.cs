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

namespace DVLD.Licenses.International_License
{
    public partial class frmShowPersonLicenseHistory : Form
    {
        int _PersonID = -1;
       
        public frmShowPersonLicenseHistory()
        {
            InitializeComponent();
        }
        public frmShowPersonLicenseHistory(int PersonID)
        {
            _PersonID = PersonID;
            InitializeComponent();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmShowPersonLicenseHistory_Load(object sender, EventArgs e)
        {
            
            if(_PersonID !=-1)
            {
                ctrlPersonCardWithFilter1.LoadData(_PersonID);
                ctrlPersonCardWithFilter1.FilterEnabled = false;
                ctrlDriverLicenses1.LoadData(_PersonID);
            }
            else
            {
                ctrlPersonCardWithFilter1.FilterEnabled = true;
                ctrlPersonCardWithFilter1.TextBoxFocus();
            }
        }

        private void ctrlPersonCardWithFilter1_OnPersonSelected(int obj)
        {
            _PersonID = obj;
              if(_PersonID != -1)
              {
                ctrlDriverLicenses1.LoadData(_PersonID);
              }
            else
            {
                ctrlDriverLicenses1.CLear();
            }
        }
    }
}
