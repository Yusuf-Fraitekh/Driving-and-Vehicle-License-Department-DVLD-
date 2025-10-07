using DVLD.People;
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

namespace DVLD.Controls
{
    public partial class ctrlPersonCardWithFilter : UserControl
    {
        public event Action<int> OnPersonSelected;
        protected virtual void PersonSelected(int PersonID)
        {
            Action<int> handler = OnPersonSelected;
            if (handler != null)
            {
                handler(PersonID);
            }
        }
        private int _PersonID=-1;
        public int PersonID 
        { 
            get { return ctrlPersonCard1.PersonID; }
        }
        public clsPerson SelectedPersonInfo 
        { 
            get { return ctrlPersonCard1.SelectedPersonInfo; }
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
                _FilterEnabled=value;
                gbFilters.Enabled = _FilterEnabled;
            }
        }
        public ctrlPersonCardWithFilter()
        {
            InitializeComponent();
        }

        private void ctrlPersonCardWithFilter_Load(object sender, EventArgs e)
        {
            
            cbFilterBy.SelectedIndex = 0;
            ctrlPersonCard1.ResetData();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)13)
            {
                btnFind.PerformClick();
            }
            if(cbFilterBy.Text=="Person ID")
            {
                if((!char.IsControl(e.KeyChar)) && (!char.IsDigit(e.KeyChar)))
                {
                    e.Handled = true;
                }
                else
                { 
                    e.Handled = false;
                }
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            Find();
        }
        private void Find()
        {
            switch (cbFilterBy.Text)
            {
                case "Person ID":
                    ctrlPersonCard1.LoadData(Convert.ToInt32(txtFilterValue.Text.Trim()));
                    break;
                case "National No.":
                    ctrlPersonCard1.LoadData(txtFilterValue.Text.Trim());
                    break;

                default:  
                    break;
                    
            }
            if (OnPersonSelected != null && FilterEnabled)
                OnPersonSelected(ctrlPersonCard1.PersonID);
            _PersonID = ctrlPersonCard1.PersonID;
        }
        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frmAddUpdatePerson = new frmAddUpdatePerson();
            frmAddUpdatePerson.DataBack += DataBackHandle;
            frmAddUpdatePerson.ShowDialog();
        }
        private void DataBackHandle(object sender, int PersonID)
        {
            txtFilterValue.Text=PersonID.ToString();
            cbFilterBy.SelectedIndex = 1;
            ctrlPersonCard1.LoadData(PersonID);
        }
        public void LoadData(int PersonID)
        {
            cbFilterBy.SelectedIndex = 1;
            txtFilterValue.Text = PersonID.ToString();
            gbFilters.Enabled = false;
            Find();
        }
        public void TextBoxFocus()
        {
            txtFilterValue.Focus();
        }

       
    }
}
