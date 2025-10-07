using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BuisnessLayer
{
    public  class clsLicenseClass
    {
        public enum enClassName {SmallMotorCycle=1,HeaveMotorCycle=2,OrdinaryDriving=3,Commercial=4,Agriculture=5,SmallAndMeduimBus=6,TruckAndHeavyVehicle=7};
        public short LicenseClassID { get; set; }
        public string ClassName { get; set; }
        public string ClassDescription { get; set; }
        public short MinimumAllowedAge {  get; set; }
        public short DefaultValidityLength {  get; set; }
        public decimal ClassFees { get; set; }
        private clsLicenseClass(short licenseClassID, string className, string classDescription, short minimumAllowedAge, short defaultValidityLength, decimal classFees)
        {
            LicenseClassID = licenseClassID;
            ClassName = className;
            ClassDescription = classDescription;
            MinimumAllowedAge = minimumAllowedAge;
            DefaultValidityLength = defaultValidityLength;
            ClassFees = classFees;
        }

        public static DataTable _GetAllLicenseClasses()
        {
            return clsLicenseClassDataAccess.GetAllLicenseClasses();
        }
        public static clsLicenseClass _GetLicenseClassByClassID(short licenseClassID)
        {
            bool isFound = false;
            string className = "", classDescription = "";
            decimal classFees = -1m;
            short minimumAllowedAge=-1, defaultValidityLength = -1;
            isFound = clsLicenseClassDataAccess.GetLicenseClassByClassID(licenseClassID, ref className, ref classDescription, ref minimumAllowedAge,ref defaultValidityLength,ref classFees);
            if (isFound)
            {
                return new clsLicenseClass( licenseClassID,  className,  classDescription,  minimumAllowedAge,  defaultValidityLength,  classFees);
            }
            else
            {
                return null;
            }

        }
        public static clsLicenseClass _GetLicenseClassByClassName(string className)
        {
            bool isFound = false;
            short licenseClassID = -1;
            string classDescription = "";
            decimal classFees = -1m;
            short minimumAllowedAge = -1, defaultValidityLength = -1;
            isFound = clsLicenseClassDataAccess.GetLicenseClassByClassName(ref licenseClassID,  className, ref classDescription, ref minimumAllowedAge, ref defaultValidityLength, ref classFees);
            if (isFound)
            {
                return new clsLicenseClass(licenseClassID, className, classDescription, minimumAllowedAge, defaultValidityLength, classFees);
            }
            else
            {
                return null;
            }

        }
        public  bool _UpdateApplication(short licenseClassID,string className,string classDescription,short minimumAllowedAge,short defaultValidityLength,decimal classFees)
        {
            return clsLicenseClassDataAccess.UpdateLicenseClass(licenseClassID, className, classDescription, minimumAllowedAge, defaultValidityLength, classFees);
        }
    }
}
