using DVLD_DataAccessLayerLastVersion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BuisnessLayer
{
    public  class clsApplicationType
    {
        public int ApplicationTypeID {  get; set; }
        public string ApplicationTypeTitle { get; set; }
        public decimal ApplicationFees {  get; set; }
        private clsApplicationType(int applicationTypeID, string applicationTypeTitle, decimal applicationFees)
        {
            ApplicationTypeID = applicationTypeID;
            ApplicationTypeTitle = applicationTypeTitle;
            ApplicationFees = applicationFees;
        }
        public static DataTable _GetAllApplicationTypes()
        {
            return clsApplicationTypeDataAccess.GetAllApplicationTypes();
        }
        public static clsApplicationType _GetApplicationTypeByApplicationTypeID(int applicationTypeID)
        {
            bool isFound=false;
            string applicationTypeTitle = "";
            decimal applicationFees = -1m;
            isFound= clsApplicationTypeDataAccess.GetApplicationTypeByApplicationTypeID(applicationTypeID,ref applicationTypeTitle,ref applicationFees);
            if(isFound)
            {
                return new clsApplicationType(applicationTypeID,applicationTypeTitle,applicationFees);
            }
            else
            {
                return null;
            }

        }
        public static clsApplicationType _GetApplicationTypeByApplicationTypeTitle(string ApplicationTypeTitle)
        {
            bool isFound = false;
            int applicationTypeID = -1;
            decimal applicationFees = -1m;
            isFound = clsApplicationTypeDataAccess.GetApplicationTypeByApplicationTypeTitle(ref applicationTypeID, ApplicationTypeTitle, ref applicationFees);
            if (isFound)
            {
                return new clsApplicationType(applicationTypeID, ApplicationTypeTitle, applicationFees);
            }
            else
            {
                return null;
            }

        }
        public  bool _UpdateApplication()
        {
            return clsApplicationTypeDataAccess.UpdateApplication(this.ApplicationTypeID,this.ApplicationFees,this.ApplicationTypeTitle);
        }
        

    }
}
