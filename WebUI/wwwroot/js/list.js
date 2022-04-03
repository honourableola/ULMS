const notSetText = "Not Set";


const fixedOptions = {
    YesNo: [
        {
            value: 0,
            text: "No"
        },
        {
            value: 1,
            text: "Yes"
        }
    ],
    Title: [
        {
            value: 0,
            text: "Choose Title"
        },
        {
            value: 1,
            text: "Mr."
        },
        {
            value: 2,
            text: "Mrs."
        },
    ],
    Status: [
        {
            value: 1,
            text: "Active"
        },
        {
            value: 2,
            text: "Inactive"
        },
        {
            value: 3,
            text: "Discharged"
        },
        {
            value: 4,
            text: "Disabled"
        },
        {
            value: 5,
            text: "Retrenched"
        },
        {
            value: 6,
            text: "Resigned"
        }
    ],
    EmployeeCategory: [
        {
            value: 0,
            text: "Choose a category"
        },
        {
            value: 1,
            text: "Administration"
        },
        {
            value: 2,
            text: "Support"
        },
        {
            value: 3,
            text: "Growth Provider"
        },
        {
            value: 4,
            text: "Service Provider"
        },
    ],
    EmployeeCosting: [
        {
            value: 1,
            text: "OPEX"
        },
        {
            value: 2,
            text: "Cost of Sales"
        }
    ],
    TerminationGrounds: [
        {
            value: 0,
            text: "Choose Termination Ground"
        },
        {
            value: 1,
            text: "Resignation"
        },
        {
            value: 2,
            text: "Non-Renewal of contract"
        },
        {
            value: 3,
            text: "Dismissal"
        },
        {
            value: 4,
            text: "Retirement"
        },
        {
            value: 5,
            text: "Deceased"
        },
        {
            value: 6,
            text: "Termination"
        },
        {
            value: 7,
            text: "Contract Ended"
        },
        {
            value: 8,
            text: "Abandonment"
        },
        {
            value: 9,
            text: "Appt. Revoked"
        },
        {
            value: 10,
            text: "Contract Termination"
        },
        {
            value: 11,
            text: "Retrenchment"
        },
        {
            value: 12,
            text: "Other"
        },
    ],
    LeaveStatus: [
        {
            value: 0,
            text: "Inactive"
        },
        {
            value: 1,
            text: "Active"
        },
    ],
    Gender: [
        {
            value: 0,
            text: "Choose Gender"
        },
        {
            value: 1,
            text: "Male"
        },
        {
            value: 2,
            text: "Female"
        },
    ],
    MaritalStatus: [
        {
            value: 0,
            text: "Choose Marital Status"
        },
        {
            value: 1,
            text: "Single"
        },
        {
            value: 2,
            text: "Married"
        }, {
            value: 3,
            text: "Seperated"
        }, {
            value: 4,
            text: "Divorced"
        }, {
            value: 5,
            text: "Widow(er)"
        }, {
            value: 6,
            text: "Other"
        },
    ]
}

async function fetchData(url) {
    let response = await axios.get(url);
    return response.data.data;
}

function createOption(value, text, selectedValue = "") {
    let option = document.createElement('option');
    option.value = value;
    option.textContent = text;
    option.selected = value === selectedValue;
    return option;
}

//For Departments
async function mapDepartments(selectedValue = "") {
    const url = `${appConfig.apiBaseUrl}/lookups/departments`;
    const placeholder = "Choose Department";
    const departmentSelect = document.querySelector("#department");
    let departments = await fetchData(url);

    departments.forEach(department =>
        departmentSelect.appendChild(createOption(department.code, department.name, selectedValue)))
}

//For Bank Structures
async function mapBankStructures(selectedValue = "") {
    const url = `${appConfig.apiBaseUrl}/lookups/bank-structures`;
    const placeholder = "Choose Bank";
    const bankStructureSelect = document.querySelector("#bankStructure");
    let bankStructures = await fetchData(url);

    bankStructures.forEach(bankStructure =>
        bankStructureSelect.appendChild(createOption(bankStructure.code, bankStructure.name, selectedValue)))
}

async function mapMainBank(selectedValue = "") {
    const url = `${appConfig.apiBaseUrl}/lookups/bank-structures`;
    const placeholder = "Choose Main Bank";
    const bankStructureSelect = document.querySelector("#mainBank");
    let bankStructures = await fetchData(url);

    bankStructures.forEach(bankStructure =>
        bankStructureSelect.appendChild(createOption(bankStructure.bankCode, bankStructure.bankName, selectedValue)))
}

async function mapBranchBank(selectedValue = "") {
    const url = `${appConfig.apiBaseUrl}/lookups/bank-structures`;
    const placeholder = "Choose Branch Bank";
    const bankStructureSelect = document.querySelector("#branchBank");
    let bankStructures = await fetchData(url);

    bankStructures.forEach(bankStructure =>
        bankStructureSelect.appendChild(createOption(bankStructure.branchCode, bankStructure.branchName, selectedValue)))
}


async function mapCountryRegions(selectedValue = "") {
    const url = `${appConfig.apiBaseUrl}/lookups/country-regions`;
    const placeholder = "Choose Country Region";
    const countryRegionSelect = document.querySelector("#citizenship");
    let countryRegions = await fetchData(url);

    countryRegions.forEach(countryRegion =>
        countryRegionSelect.appendChild(createOption(countryRegion.code, countryRegion.name, selectedValue)))
}

async function mapBonusGroups(selectedValue = "") {
    const url = `${appConfig.apiBaseUrl}/lookups/bonus-groups`;
    const placeholder = "Choose Bonus Group";
    const bonusGroupSelect = document.querySelector("#groupBonusCode");
    let bonusGroups = await fetchData(url);

    bonusGroups.forEach(bonusGroup =>
        bonusGroupSelect.appendChild(createOption(bonusGroup.group, bonusGroup.bonusGroupDescription, selectedValue)))
}

async function mapJobs(selectedValue = "") {
    const url = `${appConfig.apiBaseUrl}/lookups/jobs`;
    const placeholder = "Choose Job";
    const jobSelect = document.querySelector("#jobTitle");
    let jobs = await fetchData(url);

    console.log("Jobs", jobs)

    jobs.forEach(job =>
        jobSelect.appendChild(createOption(job.jobId, job.jobPosition, selectedValue)))
}

async function mapLeaveTypes(selectedValue = "") {
    const url = `${appConfig.apiBaseUrl}/lookups/leave-types`;
    const placeholder = "Choose Leave Type";
    const leaveTypeSelect = document.querySelector("#leaveTypeFilter");
    let leaveTypes = await fetchData(url);

    leaveTypes.forEach(leaveType =>
        leaveTypeSelect.appendChild(createOption(leaveType.code, leaveType.description, selectedValue)))
}

async function mapPostCodes(selectedValue = "") {
    const url = `${appConfig.apiBaseUrl}/lookups/post-codes`;
    const placeholder = "Choose Post Code";
    const postCodeSelect = document.querySelector("#postCode");
    let postCodes = await fetchData(url);

    postCodes.forEach(postCode =>
        postCodeSelect.appendChild(createOption(postCode.code, postCode.code, selectedValue)))
}

async function mapPostingGroups(selectedValue = "") {
    const url = `${appConfig.apiBaseUrl}/lookups/posting-groups`;
    const placeholder = "Choose Posting Group";
    const postingGroupSelect = document.querySelector("#postingGroup");
    let postingGroups = await fetchData(url);

    postingGroups.forEach(postingGroup =>
        postingGroupSelect.appendChild(createOption(postingGroup.code, postingGroup.description, selectedValue)))
}

async function mapCountries(selectedValue = "") {
    const url = `${appConfig.apiBaseUrl}/lookups/country`;
    const placeholder = "Choose Country";
    const countrySelect = document.querySelector("#county");
    let countries = await fetchData(url);

    countries.forEach(country =>
        countrySelect.appendChild(createOption(country.code, country.description, selectedValue)))
}

async function mapFirstLanguages(selectedValue = "") {
    const url = `${appConfig.apiBaseUrl}/lookups/language`;
    const placeholder = "Choose Language";
    const languageSelect = document.querySelector("#firstLanguageRWS");
    let languages = await fetchData(url);

    languages.forEach(language =>
        languageSelect.appendChild(createOption(language.code, language.description, selectedValue)))
}

async function mapSecondLanguages(selectedValue = "") {
    const url = `${appConfig.apiBaseUrl}/lookups/language`;
    const placeholder = "Choose Language";
    const languageSelect = document.querySelector("#secondLanguageRWS");
    let languages = await fetchData(url);

    languages.forEach(language =>
        languageSelect.appendChild(createOption(language.code, language.description, selectedValue)))
}

async function mapReligions(selectedValue = "") {
    const url = `${appConfig.apiBaseUrl}/lookups/religion`;
    const placeholder = "Choose Religion";
    const religionSelect = document.querySelector("#religion");
    let religions = await fetchData(url);

    religions.forEach(religion =>
        religionSelect.appendChild(createOption(religion.code, religion.description, selectedValue)))
}

async function mapSalaryNotches(selectedValue = "") {
    const url = `${appConfig.apiBaseUrl}/lookups/salarynotches`;
    const placeholder = "Choose Salary Step";
    const salaryNotchSelect = document.querySelector("#salaryStep");
    let salaryNotches = await fetchData(url);

    salaryNotches.forEach(salaryNotch =>
        salaryNotchSelect.appendChild(createOption(salaryNotch.salaryNotch, salaryNotch.description, selectedValue)))
}

async function mapDimensionValues(selectedValue = "") {
    const url = `${appConfig.apiBaseUrl}/lookups/dimension-values`;
    const placeholder = "Choose Division Code";
    const dimensionValueSelect = document.querySelector("#globalDimension2");
    let dimensionValues = await fetchData(url);
    dimensionValues.forEach(dimensionValue =>
        dimensionValueSelect.appendChild(createOption(dimensionValue.code, dimensionValue.name, selectedValue)))
}

async function mapStaffGrades(selectedValue = "") {
    const url = `${appConfig.apiBaseUrl}/lookups/staff-grade`;
    const placeholder = "Choose Staff Grade";
    const staffGradeSelect = document.querySelector("#staffGrade");
    let staffGrades = await fetchData(url);

    staffGrades.forEach(staffGrade =>
        staffGradeSelect.appendChild(createOption(staffGrade.code, staffGrade.description, selectedValue)))
}

async function mapContractTypes(selectedValue = "") {
    const url = `${appConfig.apiBaseUrl}/lookups/contract-type`;
    const placeholder = "Choose Contract Type";
    const contractTypeSelect = document.querySelector("#contractType");
    let contractTypes = await fetchData(url);

    contractTypes.forEach(contractType =>
        contractTypeSelect.appendChild(createOption(contractType.code, contractType.description, selectedValue)))
}

async function mapInstituionalMembership(selectedValue = "") {
    const url = `${appConfig.apiBaseUrl}/lookups/institutionalmemberships`;
    const placeholder = "Choose Pension Fund Administrator";
    const institutionalMembershipSelect = document.querySelector("#pensionFundAdministrator");
    let institutionalMemberships = await fetchData(url);

    institutionalMemberships.forEach(institutionalMembership =>
        institutionalMembershipSelect.appendChild(createOption(institutionalMembership.groupNo, institutionalMembership.description, selectedValue)))
}

//FOR DYNAMIC SELECTION ON THE EMPLOYEE DETAILS PAGE
async function selectCountryRegion(selectedValue = "") {
    const url = `${appConfig.apiBaseUrl}/lookups/country-regions`;
    const countryRegionSpan = document.querySelector("#citizenship");
    let countryRegions = await fetchData(url);
    let selected = countryRegions.find(c => c.code == selectedValue);
    countryRegionSpan.textContent = selected ? selected.name : notSetText;
}

async function selectDepartment(selectedValue = "") {
    const url = `${appConfig.apiBaseUrl}/lookups/departments`;
    const departmentSpan = document.querySelector("#department");
    let departments = await fetchData(url);
    let selected = departments.find(c => c.code == selectedValue);
    departmentSpan.textContent = selected ? selected.name : notSetText;
}

async function selectDivisionCode(selectedValue = "") {
    const url = `${appConfig.apiBaseUrl}/lookups/dimension-values`;
    const divisionCodeSpan = document.querySelector("#globalDimension2");
    let dimensionValues = await fetchData(url);
    let selected = dimensionValues.find(c => c.code == selectedValue);
    divisionCodeSpan.textContent = selected ? selected.name : notSetText;
}

async function selectPostCode(selectedValue = "") {
    const url = `${appConfig.apiBaseUrl}/lookups/post-codes`;
    const postCodeSpan = document.querySelector("#postCode");
    let postCodes = await fetchData(url);
    let selected = postCodes.find(c => c.code == selectedValue);
    postCodeSpan.textContent = selected ? selected.code : notSetText;
}

async function selectCountry(selectedValue = "") {
    const url = `${appConfig.apiBaseUrl}/lookups/country`;
    const countrySpan = document.querySelector("#county");
    let countries = await fetchData(url);
    let selected = countries.find(c => c.code == selectedValue);
    countrySpan.textContent = selected ? selected.description : notSetText;
}

async function selectBonusGroup(selectedValue = "") {
    const url = `${appConfig.apiBaseUrl}/lookups/bonus-groups`;
    const bonusGroupSelect = document.querySelector("#groupBonusCode");
    let bonusGroups = await fetchData(url);
    let selected = bonusGroups.find(c => c.group == selectedValue);
    bonusGroupSelect.textContent = selected ? selected.bonusGroupDescription : notSetText;
}

async function selectMainBank(selectedValue = "") {
    const url = `${appConfig.apiBaseUrl}/lookups/bank-structures`;
    const mainBankSelect = document.querySelector("#mainBank");
    let mainBanks = await fetchData(url);
    let selected = mainBanks.find(c => c.bankCode == selectedValue);
    mainBankSelect.textContent = selected ? selected.bankName : notSetText;
}

async function selectBranchBank(selectedValue = "") {
    const url = `${appConfig.apiBaseUrl}/lookups/bank-structures`;
    const mainBankSelect = document.querySelector("#branchBank");
    let mainBanks = await fetchData(url);
    let selected = mainBanks.find(c => c.branchCode == selectedValue);
    mainBankSelect.textContent = selected ? selected.branchName : notSetText;
}

async function selectJob(selectedValue = "") {
    const url = `${appConfig.apiBaseUrl}/lookups/jobs`;
    const jobSelect = document.querySelector("#jobTitle");
    let jobs = await fetchData(url);
    let selected = jobs.find(c => c.jobId == selectedValue);
    jobSelect.textContent = selected ? selected.jobPosition : notSetText;
}

async function selectStaffGrade(selectedValue = "") {
    const url = `${appConfig.apiBaseUrl}/lookups/staff-grade`;
    const gradeSpan = document.querySelector("#staffGrade");
    let grades = await fetchData(url);
    let selected = grades.find(c => c.code == selectedValue);
    gradeSpan.textContent = selected ? selected.description : notSetText;
}

async function selectSalaryStep(selectedValue = "") {
    const url = `${appConfig.apiBaseUrl}/lookups/salarynotches`;
    const stepSpan = document.querySelector("#salaryStep");
    let steps = await fetchData(url);
    let selected = steps.find(c => c.salaryNotch == selectedValue);
    stepSpan.textContent = selected ? selected.description : notSetText;
}

async function selectPostingGroup(selectedValue = "") {
    const url = `${appConfig.apiBaseUrl}/lookups/posting-groups`;
    const groupSpan = document.querySelector("#postingGroup");
    let groups = await fetchData(url);
    let selected = groups.find(c => c.code == selectedValue);
    groupSpan.textContent = selected ? selected.description : notSetText;
}

async function selectContractType(selectedValue = "") {
    const url = `${appConfig.apiBaseUrl}/lookups/contract-type`;
    const contractTypeSpan = document.querySelector("#contractType");
    let contractTypes = await fetchData(url);
    let selected = contractTypes.find(c => c.code == selectedValue);
    contractTypeSpan.textContent = selected ? selected.description : notSetText;
}

async function selectPensionFundAdministrator(selectedValue = "") {
    const url = `${appConfig.apiBaseUrl}/lookups/institutionalmemberships`;
    const membershipSpan = document.querySelector("#pensionFundAdministrator");
    let memberships = await fetchData(url);
    let selected = memberships.find(c => c.groupNo == selectedValue);
    membershipSpan.textContent = selected ? selected.description : notSetText;
}

async function selectLeaveType(selectedValue = "") {
    const url = `${appConfig.apiBaseUrl}/lookups/leave-types`;
    const leaveTypeSpan = document.querySelector("#leaveTypeFilter");
    let leaveTypes = await fetchData(url);
    let selected = leaveTypes.find(c => c.code == selectedValue);
    leaveTypeSpan.textContent = selected ? selected.description : notSetText;
}

async function selectFirstLanguage(selectedValue = "") {
    const url = `${appConfig.apiBaseUrl}/lookups/language`;
    const languageSpan = document.querySelector("#firstLanguageRWS");
    let languages = await fetchData(url);
    let selected = languages.find(c => c.code == selectedValue);
    languageSpan.textContent = selected ? selected.description : notSetText;
}

async function selectSecondLanguage(selectedValue = "") {
    const url = `${appConfig.apiBaseUrl}/lookups/language`;
    const languageSpan = document.querySelector("#secondLanguageRWS");
    let languages = await fetchData(url);
    let selected = languages.find(c => c.code == selectedValue);
    languageSpan.textContent = selected ? selected.description : notSetText;
}

async function selectReligion(selectedValue = "") {
    const url = `${appConfig.apiBaseUrl}/lookups/religion`;
    const religionSpan = document.querySelector("#religion");
    let religions = await fetchData(url);
    let selected = religions.find(c => c.code == selectedValue);
    religionSpan.textContent = selected ? selected.description : notSetText;
}


//
async function getEmployeeList(element, url, placeholder) {

    let response = await axios.get(url);

    let returnedData = response.data.data;

    mapEmployeeResults(returnedData, element);

    initializeSelect(element, placeholder);
}


function mapEmployeeResults(response, element) {

    $.each(response, function (_key, entry) {
        element.append($('<option></option>').attr('value', entry.id).text(entry.fullName));
    });

}


function initializeSelect(element, placeholder) {

    element.select2({

        placeholder: placeholder,

        allowClear: true,

        width: "100%"
    });
}


async function getRoles() {

    const url = `${appConfig.apiBaseUrl}/roles`;
    const placeholder = "Choose Role";
    const roles = $("#roles");
    await getList(roles, url, placeholder);
}


function resetSelect(element) {
    element.select2("destroy");

    element.empty();

    element.prepend("<option></option>");
}

async function setupProgrammeProduce() {
    var produces = $("#produces");
    const programmes = $("#programmes");
    programmes.on("select2:select", async function () {

        let selected = $(this).val();
        if (selected) {
            selected = selected.split(',');
        } else {
            selected = [];
        }

        resetSelect(produces);
        let url = `${appConfig.apiBaseUrl}/produces`;
        url += selected.length > 0 ? "?" : "";
        selected.forEach((p, index) => {
            url += `programmes=${p}`;
            if (index != programmes.length - 1) {
                url += '&';
            }
        });
        if (isIdhAdmin()) {
            var companyId = $("#companies").val();
            if (!companyId) {
                companyId = emptyGuid();
            }
            url += `&companyId=${companyId}`;
        }
        let response = await axios.get(url);

        let returnedData = response.data.data;

        mapResults(returnedData, produces);

        initializeSelect(produces, "Choose produces");

    });

    programmes.on("select2:deselect", function (e) {
        resetSelect(produces);
        initializeSelect(produces, "Choose produces");
    });
    initializeSelect(produces, "Choose Produces");
}

async function getCompanyProgrammesAndCountries(companyElementId, programmeElementId, countryElementId) {

    const companyElement = $(`#${companyElementId}`);
    const programmeElement = $(`#${programmeElementId}`);
    const countryElement = $(`#${countryElementId}`);

    companyElement.on("select2:select", async function () {

        let selected = $(this).val();

        resetSelect(programmeElement);
        resetSelect(countryElement);

        let programmeurl = `${appConfig.apiBaseUrl}/companies/programmes/${selected}`;
        let programmeResponse = await axios.get(programmeurl);
        mapResults(programmeResponse.data.data, programmeElement);
        initializeSelect(programmeElement, "Choose programmes");

        let countryurl = `${appConfig.apiBaseUrl}/companies/countries/${selected}`;
        let countryResponse = await axios.get(countryurl);
        mapResults(countryResponse.data.data, countryElement);
        initializeSelect(countryElement, "Choose Countries");

    });

    companyElement.on("select2:deselect", function (e) {
        resetSelect(programmeElement);
        resetSelect(countryElement);
        initializeSelect(programmeElement, "Choose programmes");
        initializeSelect(countryElement, "Choose Countries");
    });

    initializeSelect(programmeElement, "Choose Programmes");
    initializeSelect(countryElement, "Choose Countries");
}

setupProgrammeProduce();
