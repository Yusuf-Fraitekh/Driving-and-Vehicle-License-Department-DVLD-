using DVLD.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.People
{
    public partial class frmShowPersonInfo : Form
    {
        private int _PersonID = -1;
        public frmShowPersonInfo(int PersonID)
        {
            InitializeComponent();
            _PersonID = PersonID;
            ctrlPersonCard1.LoadData(_PersonID);
        }
        public frmShowPersonInfo(string NationalNumber)
        {
            
            InitializeComponent();
            string _NationalNumber = NationalNumber;
            ctrlPersonCard1.LoadData(_NationalNumber);
        }

        

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
