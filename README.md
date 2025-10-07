<h1 align="center" id="title">Driving and Vehicle License Department (DVLD)</h1>



<h2>🚀 Overview</h2>


The Driving & Vehicle License Department (DVLD) is a robust C# Windows Forms desktop application developed using Object-Oriented Programming (OOP) principles and ADO.NET for data access and management.
This system is designed to streamline, modernize, and automate the processes related to the issuance, renewal, detention, and management of both driver and vehicle licenses.

By integrating comprehensive validation, a multi-layered architecture, and adherence to national and international licensing standards, DVLD provides a secure, reliable, and user-friendly experience for managing all licensing operations efficiently.


<h2>⭐ Main Features</h2>

 📝 Here're some of the project's best features:

1. **Issuing a New Driving License:**
   - Allows applicants to apply for a specific license category (e.g., motorbike, car, commercial vehicles).
   - Validates eligibility requirements such as age and prior training.
   - Ensures no duplicate license exists for the same category.
   - Dynamically calculates and applies relevant fees based on the license category.

2. **Reassessment Service:**
   - Enables applicants to retake failed tests (vision, theoretical, or practical).
   - Schedules a new test date upon payment of the applicable fee.
   - Prevents scheduling multiple reassessments for the same test type concurrently.

3. **Renewing a Driving License:**
   - Allows the renewal of expired licenses upon successful completion of vision tests.
   - Requires submission of the expired license.
   - Calculates and applies renewal fees automatically.

4. **Replacing Lost Licenses:**
   - Processes applications for lost license replacements.
   - Ensures the license is not under suspension or previously replaced.
   - Logs all replacement requests with applicable fees.

5. **Replacing Damaged Licenses:**
   - Processes replacement requests for damaged licenses.
   - Requires submission of the damaged license for verification.
   - Records the replacement history and applies applicable fees.

6. **Unblocking a Detained License:**
   - Facilitates unblocking of licenses after penalty payments.
   - Logs the date and reason for license unblocking.

7. **Issuing an International License:**
   - Issues international driving licenses to eligible applicants with valid domestic licenses.
   - Restricts issuance to holders of category-three (car) licenses only.
   - Ensures no active international license exists prior to issuing a new one.
   - Applies a set duration and predefined fee for international licenses.


<h2>🔆 Administrative Services</h2>

1. **Application Services:**
   - **Primary Services Provided in the System:**
     1. Issuing a new driving license: $5 fee per application.
     2. Reassessment services: $5 fee per application.
     3. Renewing a driving license: $5 fee per application.
     4. Issuing a replacement for a lost license: $5 fee per application.
     5. Issuing a replacement for a damaged license: $5 fee per application.
     6. Release detained licenses: $5 fee per application.
     7. Issuing an international driving license: $5 fee per application.
        
2. **User Management:**
   - Add, view, update, or delete user accounts.
   - Suspend or activate user accounts as needed.

3. **Person Management:**
   - Add, view, update, or delete individuals within the system.
   - Ensure no duplicate records exist for the same national ID.

4. **Request Management:**
   - Search requests by request ID.
   - Search requests by the applicant’s national ID.
   - Display a list of all requests.
   - View detailed information for each request, including fees.
   - Update request information as needed.
   - Filter requests by their status.

5. **Test Management:**
   - Configure test fees and settings for vision, theoretical, and practical exams.
   - Schedule tests and maintain test result logs.
   - **Test Workflow:**
     1. **Vision Test:**
        - Scheduled by the system upon payment of a $10 fee.
        - Verifies applicant’s medical and visual ability.
        - Results (pass or fail) are recorded in the system.
        - Failing applicants must correct their vision (e.g., glasses, surgery) and reschedule the test.
     2. **Theoretical Test:**
        - Scheduled manually after passing the vision test and paying a $20 fee.
        - Evaluates knowledge of traffic laws and road safety.
        - Results (pass or fail) are recorded in the system with a score out of 100.
        - Failing applicants can reschedule the test with a new fee payment.
     3. **Practical Driving Test:**
        - Scheduled manually after passing the theoretical test and paying the applicable fee (varies by license category).
        - Assesses driving skills, vehicle control, and traffic rule adherence.
        - Failing applicants can reschedule after paying the test fee again.
     4. **License Issuance:**
        - After passing all tests, a driving license is issued containing:
          - License number
          - Applicant photo
          - National ID
          - Name and birthdate
          - License category
          - Issuance and expiration dates
          - Special conditions (if any)
          - Status (e.g., New, Replacement, Renewal).

6. **License Categories Management:**
   - Adjust fees, validity periods, and age requirements for each license category.
   - Define and update license category descriptions and associated rules.

7. **License Detention Management:**
   - Detain licenses by recording reasons and detention dates.
   - Release licenses once penalties or requirements are completed.


<h2>📄 Additional Information:</h2>

#### License Application Requirements:
- **Eligibility Criteria:**
  - Applicants must meet the minimum age required for the desired license category.
  - Must not already hold a license of the same category.
- **Required Documents:**
  - Valid identification (e.g., passport or national ID).
  - Proof of completion of necessary training courses.
- **Mandatory Tests:**
  - **Vision Test:** Verifies visual ability; applicants failing the test must correct their vision and retake it.
  - **Theoretical Test:** Assesses knowledge of traffic laws; requires a minimum passing score.
  - **Practical Test:** Evaluates real-world driving skills; can be retaken after paying applicable fees if failed.
#### License Categories
| **Category**       | **Description**                | **Fees** | **Validity** | **Minimum Age** |
|---------------------|--------------------------------|----------|--------------|------------------|
| Motorbike (Small)   | Small motorcycles             | $15      | 5 years      | 18 years         |
| Motorbike (Heavy)   | Large, powerful motorcycles   | $30      | 5 years      | 21 years         |
| Car License         | Personal cars                 | $20      | 10 years     | 18 years         |
| Commercial Vehicle  | Taxis/Limousines              | $200     | 10 years     | 21 years         |
| Agricultural        | Tractors and farming vehicles | $50      | 10 years     | 21 years         |
| Buses               | Small and medium buses        | $250     | 10 years     | 21 years         |
| Trucks              | Heavy-duty trucks             | $300     | 10 years     | 21 years         |





<h2>🛠️ Installation Steps:</h2>
<p>
  1. Prerequisites 
  <li> sql server management studio.</li>
  <li> .NET Framework 4.8 </li>
</p>

<p>2. Clone the repository</p>

```
 git clone https://github.com/Yusuf-Fraitekh/Driving-and-Vehicle-License-Department-DVLD-.git
```

<p>3.Place DVLD.bak file on your c:\ drive and Restore DVLD.bak file in your sql server management studio (you will find the file in DatabaseFolder)</p>

```
Restore Database DVLD from disk ='c:\DVLD.bak';
```

<p>5. in the project folder, update the "clsDataAccessSettings.cs" file with your sql server settings (user Id and password)</p>

 <p>6. build the solution and run it </p>
 
<p>7. in login screen set ( user name : Yusuf / password : 1234 ) </p>

  
<h2>🔷 Technologies used in the project</h2>

*   Architecture: 3-Tier Architecture 
*   Programming Language: C#
*   Database: SQL Server
*   Data Access: ADO.NET
*   User Interface: Windows Forms
