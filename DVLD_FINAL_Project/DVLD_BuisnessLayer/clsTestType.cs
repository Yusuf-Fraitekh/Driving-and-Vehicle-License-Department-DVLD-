using DVLD_DataAccessLayerLastVersion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BuisnessLayer
{
    public class clsTestType
    {
        public enum enTestType { VisionTest = 1, WrittenTest = 2, StreetTest = 3 };
        public enTestType TestTypeID { get; set; }
        public string TestTypeTitle { get; set; }
        public decimal TestTypeFees { get; set; }
        public string TestTypeDescription { get; set; }
        private clsTestType(clsTestType.enTestType testType, string testTypeTitle, decimal testTypeFees,string testTypeDescription)
        {
            TestTypeID = testType;
            TestTypeTitle = testTypeTitle;
            TestTypeFees = testTypeFees;
            TestTypeDescription= testTypeDescription;
        }
        public static DataTable _GetAllTestTypes()
        {
            return clsTestTypeDataAccess.GetAllTestTypes();
        }
        public static clsTestType _GetTestTypeByTestTypeID(clsTestType.enTestType testTypeID)
        {
            bool isFound = false;
            string testTypeTitle="" , testTypeDescription="";
            decimal testTypeFees = -1m;
            isFound = clsTestTypeDataAccess.GetTestTypeByTestTypeID((int)testTypeID, ref testTypeTitle, ref testTypeDescription, ref testTypeFees);
            if (isFound)
            {
                return new clsTestType( testTypeID,  testTypeTitle,  testTypeFees,  testTypeDescription);
            }
            else
            {
                return null;
            }

        }
        public static clsTestType _GetTestTypeByTestTypeTitle(string testTypeTitle)
        {
            bool isFound = false;
            string  testTypeDescription = "";
            decimal testTypeFees = -1m;
            int testTypeID = -1;
            isFound = clsTestTypeDataAccess.GetTestTypeByTestTypeTitle(ref testTypeID, testTypeTitle, ref testTypeDescription,ref testTypeFees);
            if (isFound)
            {
                return new clsTestType((clsTestType.enTestType)testTypeID, testTypeTitle, testTypeFees, testTypeDescription);
            }
            else
            {
                return null;
            }

        }
        public bool _UpdateTest()
        {
            return clsTestTypeDataAccess.UpdateTest((int)this.TestTypeID, this.TestTypeTitle,this.TestTypeDescription,this.TestTypeFees);
        }


    }
}
