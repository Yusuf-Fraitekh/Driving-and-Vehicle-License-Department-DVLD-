using System;
using System.Data;
using DVLD_DataAccessLayer;


namespace DVLD_BuisnessLayer
{
    public class clsPerson
    {
        public enum Mode { AddNew = 0, Update = 1 }
        public Mode enMode = Mode.AddNew;
        public int PersonID { get; set; }
        public string NationalNo { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get
            {
                return FirstName + ' ' + SecondName + ' ' + ThirdName;
            }
        }
        public DateTime DateOfBirth { get; set; }
        public int Gendor { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int CountryID { get; set; }
        public clsCountry CountryInfo;
        public string ImagePath { get; set; }
        private clsPerson(int personID, string nationalNo, string firstName, string secondName,
            string thirdName, string lastName, DateTime dateOfBirth,
            int gendor, string address, string phone, string email,
            int countryID, string imagePath)
        {

            PersonID = personID;
            NationalNo = nationalNo;
            FirstName = firstName;
            SecondName = secondName;
            ThirdName = thirdName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Gendor = gendor;
            Address = address;
            Phone = phone;
            Email = email;
            CountryID = countryID;
            CountryInfo = clsCountry._GetCountryInfoBy(countryID);
            ImagePath = imagePath;
            enMode = Mode.Update;
        }
        public clsPerson()
        {
            NationalNo = "";
            FirstName = "";
            SecondName = "";
            ThirdName = "";
            LastName = "";
            DateOfBirth = DateTime.Now;
            Gendor = -1;
            Address = "";
            Phone = "";
            Email = "";
            CountryID = -1;
            ImagePath = "";
            enMode = Mode.AddNew;
        }
        public static clsPerson _GetPersonInfo(int personID)
        {
            string NationalNo = "", FirstName = "", SecondName = "", ThirdName = "", LastName = "", Address = "", Phone = "", Email = "", ImagePath = "";
            DateTime DateOfBirth = DateTime.Now;
            int Gendor = -1, CountryID = 0;
            bool isFound = clsPersonDataAccess.GetPersonInfoByID(
                personID, ref NationalNo, ref FirstName, ref SecondName,
                ref ThirdName, ref LastName, ref DateOfBirth, ref Gendor,
                ref Address, ref Phone, ref Email, ref CountryID, ref ImagePath
                                                                );
            if (isFound)
                return new clsPerson(
                    personID, NationalNo, FirstName, SecondName,
                    ThirdName, LastName, DateOfBirth, Gendor,
                    Address, Phone, Email, CountryID, ImagePath
                                    );
            else
                return null;
        }
        public static clsPerson _GetPersonInfo(string NationalNo)
        {
            string FirstName = "", SecondName = "", ThirdName = "", LastName = "", Address = "", Phone = "", Email = "", ImagePath = "";
            DateTime DateOfBirth = DateTime.Now;
            int Gendor = -1, CountryID = 0, PersonID = -1;
            bool isFound = clsPersonDataAccess.GetPersonInfoByNationalNumber(
                ref PersonID, NationalNo, ref FirstName, ref SecondName,
                ref ThirdName, ref LastName, ref DateOfBirth, ref Gendor,
                ref Address, ref Phone, ref Email, ref CountryID, ref ImagePath
                                                                             );
            if (isFound)
                return new clsPerson(
                    PersonID, NationalNo, FirstName, SecondName,
                    ThirdName, LastName, DateOfBirth, Gendor,
                    Address, Phone, Email, CountryID, ImagePath
                                    );
            else
                return null;
        }
        public static bool _DeletePerson(int PersonID)
        {
            return clsPersonDataAccess.DeletePerson(PersonID);
        }
        public static bool _IsPersonExist(int PersonID)
        {
            return clsPersonDataAccess.IsPersonExist(PersonID);
        }
        public static bool _IsPersonExist(string NationalNo)
        {
            return clsPersonDataAccess.IsPersonExist(NationalNo);
        }
        public static DataTable _GetAllPeople()
        {
            return clsPersonDataAccess.GetAllPeople();
        }
        public bool _AddPerson()
        {
            this.PersonID = clsPersonDataAccess.Add(
                this.NationalNo, this.FirstName, this.SecondName, this.ThirdName
                , this.LastName, this.DateOfBirth, this.Gendor, this.Address,
                this.Phone, this.Email, this.CountryID, this.ImagePath
                                                   );
            return this.PersonID != -1;
        }
        public bool _UpdatePerson()
        {
            return clsPersonDataAccess.UpdatePerson(
                this.PersonID, this.NationalNo, this.FirstName, this.SecondName,
                this.ThirdName, this.LastName, this.DateOfBirth, this.Gendor,
                this.Address, this.Phone, this.Email,
                this.CountryID, this.ImagePath
                                                   );

        }
        public bool Save()
        {
            switch (enMode)
            {
                case Mode.AddNew:
                    if (this._AddPerson())
                    {
                        this.enMode = Mode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case Mode.Update:
                    return this._UpdatePerson();
            }
            return false;
        }
    }
}
