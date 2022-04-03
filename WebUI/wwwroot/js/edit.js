async function editUserForm(id) {

    if (isIdhAdmin()) {
        getCompanyProgrammesAndCountries('companies', 'programmes', 'countries');
    }
    let accessAllProgrammes = false;
    let accessAllProduces = false;
    let accessAllCountries = false;
    let accessAllCompanies = false;

    $('#editRegisterForm select[name="roles[]"]').on('change', function () {
        var role = $(this).find("option:selected").text();
        if (role.toLowerCase() === permissionConfig.idhAdmin.toLowerCase()) {
            $('#registerCompany').hide();
            $('#registerCountry').hide();
            $('#registerProduce').hide();
            $('#registerProgramme').hide();
            accessAllCompanies = accessAllCountries = accessAllProduces = accessAllProgrammes = true;
        }
        else if (role.toLowerCase() === permissionConfig.ipAdmin.toLowerCase() && isIdhAdmin()) {
            $('#registerCompany').show();
            $('#registerCountry').hide();
            $('#registerProduce').hide();
            $('#registerProgramme').hide();
            accessAllCompanies = accessAllCountries = accessAllProduces = accessAllProgrammes = true;
        }
        else if (role.toLowerCase() === permissionConfig.ipAdmin.toLowerCase() && isIpAdmin()) {
            $('#registerCompany').hide();
            $('#registerCountry').hide();
            $('#registerProduce').hide();
            $('#registerProgramme').hide();
            accessAllCompanies = accessAllCountries = accessAllProduces = accessAllProgrammes = true;
        }
        else if (role.toLowerCase() === permissionConfig.dataCollector.toLowerCase() && isIpAdmin()) {
            $('#registerCompany').hide();
            $('#registerCountry').show();
            $('#registerProduce').show();
            $('#registerProgramme').show();
            accessAllCompanies = accessAllCountries = accessAllProduces = accessAllProgrammes = false;
        }
        else {
            $('#registerCompany').show();
            $('#registerCountry').show();
            $('#registerProduce').show();
            $('#registerProgramme').show();
            accessAllCompanies = accessAllCountries = accessAllProduces = accessAllProgrammes = false;
        }
    });


    axios.get(`${appConfig.apiBaseUrl}/users/user/${id}`).then(obj => {


        $("#userFirstNameInput").val(obj.data.data.firstName);
        $("#userLastNameInput").val(obj.data.data.lastName);
        $("#userEmailInput").val(obj.data.data.email);
        $("#userPhoneInput").val(obj.data.data.phone);
        $("#userEnumeratorCodeInput").val(obj.data.data.enumeratorCode);

        var produce = obj.data.data.produces.map(function (e) {
            return e.name;
        }).join(', ');
        document.getElementById("produce").innerHTML = produce;



        var role = obj.data.data.roles.map(function (e) {
            return e.name;
        }).join(', ');
        document.getElementById("role").innerHTML = role;


        var country = obj.data.data.countries.map(function (e) {
            return e.name;
        }).join(', ');

        document.getElementById("country").innerHTML = country;

        var company = obj.data.data.companies.map(function (e) {
            return e.name;
        }).join(', ');

        document.getElementById("company").innerHTML = company;


        var program = obj.data.data.programmes.map(function (e) {
            return e.name;
        }).join(', ');

        document.getElementById("Program").innerHTML = program;
    });

    let form = document.getElementById('editRegisterForm');

    form.addEventListener('submit', async e => {

        e.preventDefault();

        let submitButton = $('#editUserSubmit');

        submitButton.attr('disabled', 'disabled');

        //const url = `/account/register/${id}`;
        const url = `${appConfig.apiBaseUrl}/users/${id}`;

        const firstName = $('#editRegisterForm input[name=firstname]').val();

        const lastName = $('#editRegisterForm input[name=lastname]').val();

        const phone = $('#editRegisterForm input[name=phone]').val();

        const enumeratorCode = $('#editRegisterForm input[name=enumeratorCode]').val();

        const roles = getSelectFieldValues($('#editRegisterForm select[name="roles[]"]'));

        const companies = getSelectFieldValues($('#editRegisterForm select[name="companyId[]"]'));

        const countries = getSelectFieldValues($('#editRegisterForm select[name="countries[]"]'));

        const produces = getSelectFieldValues($('#editRegisterForm select[name="produces[]"]'));

        const programmes = getSelectFieldValues($('#editRegisterForm select[name="programmes[]"]'));


        if (!firstName || !lastName) {

            notifyDataRequired('Kindly complete the form correctly.');
            submitButton.removeAttr('disabled');
            //form.reset();

            } else {
            let companyId = emptyGuid();
            if (companies && companies.length > 0) {
                companyId = companies[0];
            }
            const updateInfo = {

              phone, firstName, lastName, roles, companyId, countries, produces, programmes, enumeratorCode,
                accessAllCompanies, accessAllCountries, accessAllProduces, accessAllProgrammes
            }

            await update(updateInfo, url).then((result) => {

                submitButton.removeAttr('disabled');
                form.reset();
            });
        }

        submitButton.removeAttr('disabled');
        form.reset();
    });
}

async function editOrganizationForm(id) {
    axios.get(`${appConfig.apiBaseUrl}/companies/${id}`).then(obj => {

        $("#companyNameInput").val(obj.data.data.name);
        $("#companyCodeInput").val(obj.data.data.code);

    });

    let form = document.getElementById('editOrgForm');

    form.addEventListener('submit', async e => {

        e.preventDefault();

        const submitButton = $('editOrganizationBtn');

        spinButton(submitButton);

        const url = `${appConfig.apiBaseUrl}/companies/${id}`;

        const name = $('#editOrgForm input[name=name]').val();

        const code = $('#editOrgForm input[name=code]').val();

        const countries = getSelectFieldValues($('#editOrgForm select[name="countries[]"]'));

        const programmes = getSelectFieldValues($('#editOrgForm select[name="programmeid[]"]'));

        const produces = getSelectFieldValues($('#editOrgForm select[name="produces[]"]'));

        if (!name || !code) {

            notifyDataRequired('Kindly complete the form correctly.');
            stopButtonSpin(submitButton);
            form.reset();

            } else {

            const updateInfo = {
                name, code, countries, produces, programmes
            }

            await update(updateInfo, url).then((result) => {

                stopButtonSpin(submitButton);
                form.reset();

            });

            stopButtonSpin(submitButton);
            form.reset();
        }

    });
}

async function editProduceForm(id) {

    const response = await axios.get(`${appConfig.apiBaseUrl}/produces/${id}`);
    $("#produceNameInput").val(response.data.data.name);
    $("#produceCodeInput").val(response.data.data.code);
    let pg = $('#editProduceForm select[name="programmeId"]');

    let form = document.getElementById('editProduceForm');

    form.addEventListener('submit', async e => {

        e.preventDefault();

        const submitButton = $('editProduceBtn');

        spinButton(submitButton);

        const url = `${appConfig.apiBaseUrl}/produces/${id}`;

        const name = $('#editProduceForm input[name=name]').val();

        const code = $('#editProduceForm input[name=code]').val();

        const programmeId = $('#editProduceForm select[name="programmeId"]').val();

        if (!name || !code) {

            notifyDataRequired('Kindly complete the form correctly.');
            stopButtonSpin(submitButton);
            form.reset();

        } else {

            const updateInfo = {

                name, code, programmeId
            }

            await update(updateInfo, url).then((result) => {

                stopButtonSpin(submitButton);
                form.reset();

            });

            stopButtonSpin(submitButton);
            form.reset();
        }

    });
}

async function editCountryForm(id) {
    axios.get(`${appConfig.apiBaseUrl}/countries/${id}`).then(obj => {

        $("#countryNameInput").val(obj.data.data.name);
    });

    let form = document.getElementById('editCountryForm');

    form.addEventListener('submit', async e => {

        e.preventDefault();

        const submitButton = $('editCountryBtn');

        spinButton(submitButton);

        const url = `${appConfig.apiBaseUrl}/countries/${id}`;

        const name = $('#editCountryForm input[name=name]').val();

        if (!name) {

            notifyDataRequired('Kindly complete the form correctly.');
            stopButtonSpin(submitButton);
            form.reset();

        } else {

            const updateInfo = {

                name
            }

            await update(updateInfo, url).then((result) => {

                stopButtonSpin(submitButton);
                form.reset();

            });

            stopButtonSpin(submitButton);
            form.reset();
        }

    });
}






async function editDepartmentForm(id) {
    axios.get(`${appConfig.apiBaseUrl}/departments/${id}`).then(obj => {

        $("#departmentNameInput").val(obj.data.data.name);
        $("#departmentCodeInput").val(obj.data.data.departmentCode);

    });

    let form = document.getElementById('editDepartmentForm');

    form.addEventListener('submit', async e => {

        e.preventDefault();

        const submitButton = $('editDepartmentBtn');

        spinButton(submitButton);

        const url = `${appConfig.apiBaseUrl}/departments/${id}`;

        const name = $('#editDepartmentForm input[name=name]').val();
        const departmentCode = $('#editDepartmentForm input[name=departmentCode]').val();

        if (!name || !departmentCode) {

            notifyDataRequired('Kindly complete the form correctly.');
            stopButtonSpin(submitButton);
            //form.reset();

        } else {

            const updateInfo = {

                name, departmentCode
            }

            await update(updateInfo, url).then((result) => {

                stopButtonSpin(submitButton);
                form.reset();

            });

            stopButtonSpin(submitButton);
            form.reset();
        }

    });
}

async function editGradeForm(id) {
    axios.get(`${appConfig.apiBaseUrl}/grades/${id}`).then(obj => {

        $("#gradeNameInput").val(obj.data.data.name);
    });

    let form = document.getElementById('editGradeForm');

    form.addEventListener('submit', async e => {

        e.preventDefault();

        const submitButton = $('editGradeBtn');

        spinButton(submitButton);

        const url = `${appConfig.apiBaseUrl}/grades/${id}`;

        const name = $('#editGradeForm input[name=name]').val();

        if (!name) {

            notifyDataRequired('Kindly complete the form correctly.');
            stopButtonSpin(submitButton);
            //form.reset();

        } else {

            const updateInfo = {
                name
            }

            await update(updateInfo, url).then((result) => {

                stopButtonSpin(submitButton);
                //form.reset();

            });

            stopButtonSpin(submitButton);
            //form.reset();
        }

    });
}

async function editJobForm(id) {
    axios.get(`${appConfig.apiBaseUrl}/jobs/${id}`).then(obj => {

        $("#jobTitleInput").val(obj.data.data.jobTitle);
        $("#jobDescriptionInput").val(obj.data.data.jobDescription);

    });

    let form = document.getElementById('editJobForm');

    form.addEventListener('submit', async e => {

        e.preventDefault();

        const submitButton = $('editJobBtn');

        spinButton(submitButton);

        const url = `${appConfig.apiBaseUrl}/jobs/${id}`;

        const jobTitle = $('#editJobForm input[name=jobTitle]').val();
        const jobDescription = $('#editJobForm input[name=jobDescription]').val();

        if (!jobTitle || !jobDescription) {

            notifyDataRequired('Kindly complete the form correctly.');
            stopButtonSpin(submitButton);
            //form.reset();

        } else {

            const updateInfo = {

                jobTitle, jobDescription
            }

            await update(updateInfo, url).then((result) => {

                stopButtonSpin(submitButton);
                form.reset();

            });

            stopButtonSpin(submitButton);
            form.reset();
        }

    });
}

async function editReligionForm(id) {
    axios.get(`${appConfig.apiBaseUrl}/religions/${id}`).then(obj => {

        $("#religionNameInput").val(obj.data.data.name);
    });

    let form = document.getElementById('editRiligionForm');

    form.addEventListener('submit', async e => {

        e.preventDefault();

        const submitButton = $('editRiligionBtn');

        spinButton(submitButton);

        const url = `${appConfig.apiBaseUrl}/religions/${id}`;

        const name = $('#editRiligionForm input[name=name]').val();

        if (!name) {

            notifyDataRequired('Kindly complete the form correctly.');
            stopButtonSpin(submitButton);
            //form.reset();

        } else {

            const updateInfo = {
                name
            }

            await update(updateInfo, url).then((result) => {

                stopButtonSpin(submitButton);
                form.reset();

            });

            stopButtonSpin(submitButton);
            form.reset();
        }

    });
}

async function editRoleForm(id) {
    axios.get(`${appConfig.apiBaseUrl}/roles/${id}`).then(obj => {

        $("#roleNameInput").val(obj.data.data.name);
        $("#roleDescriptionInput").val(obj.data.data.description);

    });

    let form = document.getElementById('editRoleForm');

    form.addEventListener('submit', async e => {

        e.preventDefault();

        const submitButton = $('editRoleBtn');

        spinButton(submitButton);

        const url = `${appConfig.apiBaseUrl}/roles/${id}`;

        const name = $('#editRoleForm input[name=name]').val();
        const description = $('#editRoleForm input[name=description]').val();

        if (!name || !description) {

            notifyDataRequired('Kindly complete the form correctly.');
            stopButtonSpin(submitButton);
            //form.reset();

        } else {

            const updateInfo = {

                name, description
            }

            await update(updateInfo, url).then((result) => {

                stopButtonSpin(submitButton);
                form.reset();

            });

            stopButtonSpin(submitButton);
            form.reset();
        }

    });
}






async function editProgrammeForm(id) {
    axios.get(`${appConfig.apiBaseUrl}/programmes/${id}`).then(obj => {

        $("#programNameInput").val(obj.data.data.name);
        $("#programCodeInput").val(obj.data.data.code);
        $("#programStartInput").val(obj.data.data.programmeStart);
        $("#programEndInput").val(obj.data.data.programmeEnd);

        obj.data.data.forms.map(function (e) {
            let radio = $(`input[type="radio"][value="${e.id}"]`)
            radio.prop("checked", true)
        });

    });

    let form = document.getElementById('editProgrammeForm');

    form.addEventListener('submit', async e => {

        e.preventDefault();

        const submitButton = $('editProgrammeBtn');

        spinButton(submitButton);

        const url = `${appConfig.apiBaseUrl}/programmes/${id}`;

        const name = $('#editProgrammeForm input[name=name]').val();
        const code = $('#editProgrammeForm input[name=code]').val();
        const programmeStart = $('#editProgrammeForm input[name=programmeStart]').val();
        const programmeEnd = $('#editProgrammeForm input[name=programmeEnd]').val();
        const radios = document.querySelectorAll('input[type="radio"]:checked') || [];
        const forms = [];
        radios.forEach(r => {
            forms.push(r.value);
        });

        if (!name || !code || !programmeStart || !programmeEnd || forms.length < 1) {

            notifyDataRequired('Kindly complete the form correctly.');
            stopButtonSpin(submitButton);
            form.reset();

        } else {

            const updateInfo = {

                name, code, programmeStart, programmeEnd, forms
            }

            await update(updateInfo, url).then((result) => {

                stopButtonSpin(submitButton);
                form.reset();

            });

            stopButtonSpin(submitButton);
            form.reset();
        }

    });
}

async function editFormForm(id) {
    axios.get(`${appConfig.apiBaseUrl}/forms/${id}`).then(obj => {

        $("#formNameInput").val(obj.data.data.name);
        $("#formDescriptionInput").val(obj.data.data.description);
        obj.data.data.formTypes.map(function (e) {
            let select = $(`input[type="select"][value="${e.name}"]`)
            select.show(true)
        });
    });

    let form = document.getElementById('editFormForm');

    form.addEventListener('submit', async e => {

        e.preventDefault();

        const submitButton = $('editFormBtn');

        spinButton(submitButton);

        const url = `${appConfig.apiBaseUrl}/forms/${id}`;

        const name = $('#editFormForm input[name=name]').val();

        const description = $('#editFormForm input[name=description]').val();

        const formTypeId = $('#editFormForm select[name="formTypeId"]').val();
        //const formTypes = getSelectFieldValues($('#createFormForm select[name="formtypes[]"]'));

        if (!name || !description) {

            notifyDataRequired('Kindly complete the form correctly.');
            stopButtonSpin(submitButton);
            form.reset();

        } else {

            const updateInfo = {

                name, description, formTypeId
            }

            await update(updateInfo, url).then((result) => {

                stopButtonSpin(submitButton);
                form.reset();

            });

            stopButtonSpin(submitButton);
            form.reset();
        }

    });
}


async function editFormTypeForm(id) {
    axios.get(`${appConfig.apiBaseUrl}/formtypes/${id}`).then(obj => {

        $("#formTypeNameInput").val(obj.data.data.name);
    });

    let form = document.getElementById('editFormTypeForm');

    form.addEventListener('submit', async e => {

        e.preventDefault();

        const submitButton = $('editFormTypeBtn');

        spinButton(submitButton);

        const url = `${appConfig.apiBaseUrl}/formtypes/${id}`;

        const name = $('#editFormTypeForm input[name=name]').val();

        if (!name) {

            notifyDataRequired('Kindly complete the form correctly.');
            stopButtonSpin(submitButton);
            form.reset();

        } else {

            const updateInfo = {

                name
            }

            await update(updateInfo, url).then((result) => {

                stopButtonSpin(submitButton);
                form.reset();

            });

            stopButtonSpin(submitButton);
            form.reset();
        }

    });
}

function spinButton(btn) {
    btn.addClass("kt-spinner kt-spinner--right kt-spinner--sm kt-spinner--light").attr("disabled", true);
}

function stopButtonSpin(btn) {
    btn.removeClass("kt-spinner kt-spinner--right kt-spinner--sm kt-spinner--light").attr("disabled", false);
}

async function editLeaveTypeForm(id) {
    axios.get(`${appConfig.apiBaseUrl}/leaveTypes/leave-type/${id}`).then(obj => {

        $("#leaveTypeNameInput").val(obj.data.data.name);
    });

    let form = document.getElementById('editLeaveTypeForm');

    form.addEventListener('submit', async e => {

        e.preventDefault();

        const submitButton = $('editLeaveTypeBtn');

        spinButton(submitButton);

        const url = `${appConfig.apiBaseUrl}/leaveTypes/${id}`;

        const name = $('#editLeaveTypeForm input[name=name]').val();

        if (!name) {

            notifyDataRequired('Kindly complete the form correctly.');
            stopButtonSpin(submitButton);
            //form.reset();

        } else {

            const updateInfo = {
                name
            }

            await update(updateInfo, url).then((result) => {

                stopButtonSpin(submitButton);
                //form.reset();

            });

            stopButtonSpin(submitButton);
            //form.reset();
        }

    });
}




async function editLeaveForm(id) {
    axios.get(`${appConfig.apiBaseUrl}/leaves/${id}`).then(obj => {
       
        $("#yearInput").val(obj.data.data.year);
        $("#proceededDateInput").val(obj.data.data.proceededDate);
        $("#appliedDateInput").val(obj.data.data.appliedDate);
        $("#approvedDateInput").val(obj.data.data.approvedDate);
        $("#returnedDateInput").val(obj.data.data.returnedDate);
        $("#durationInput").val(obj.data.data.duration);
        $("#noOfDaysInput").val(obj.data.data.noOfDays);
        $("#leaveStatusInput").val(obj.data.data.leaveStatus);
        $("#relieverNameInput").val(obj.data.data.relieverName);
        $("#employeeLeaveAllowanceRequestInput").prop("checked", obj.data.data.employeeLeaveAllowanceRequest);

        if (obj.data.data.fullName !== null) {
            var newOption = new Option(obj.data.data.fullName, obj.data.data.employeeId, true, true);
            $("#employees").append(newOption).trigger('change');
        }

        if (obj.data.data.employeeLeaveTypeName !== null) {
            var newOption = new Option(obj.data.data.employeeLeaveTypeName, obj.data.data.employeeLeaveTypeId, true, true);
            $("#leavetypes").append(newOption).trigger('change');
        }
    });

    let form = document.getElementById('editLeaveForm');

    form.addEventListener('submit', async e => {

        e.preventDefault();

        const submitButton = $('editLeaveBtn');

        spinButton(submitButton);

        const url = `${appConfig.apiBaseUrl}/leaves/edit-leave/${id}`;

        const employeeId = $('#editLeaveForm select[name=employeeId]').val();
        const year = parseInt($('#editLeaveForm input[name=year]').val());
        const employeeLeaveTypeId = $('#editLeaveForm select[name=employeeLeaveTypeId]').val();
        const proceededDate = $('#editLeaveForm input[name=proceededDate]').val();
        const appliedDate = $('#editLeaveForm input[name=appliedDate]').val();
        const approvedDate = $('#editLeaveForm input[name=approvedDate]').val();
        const returnedDate = $('#editLeaveForm input[name=returnedDate]').val();
        const duration = parseInt($('#editLeaveForm input[name=duration]').val());
        const noOfDays = parseInt($('#editLeaveForm input[name=noOfDays]').val());
        const leaveStatus = parseInt($('#editLeaveForm select[name=leaveStatus]').val());
        const relieverName = $('#editLeaveForm input[name=relieverName]').val();
        const employeeLeaveAllowanceRequest = Boolean($('#editLeaveForm input[name=employeeLeaveAllowanceRequest]').val());

        if (!employeeId || !year || !employeeLeaveTypeId || !proceededDate || !appliedDate || !returnedDate || !leaveStatus || !relieverName || !employeeLeaveAllowanceRequest)
        {
            notifyDataRequired('Kindly complete the form correctly.');
            stopButtonSpin(submitButton);

        } else {

            const updateInfo = {
                employeeId, year, employeeLeaveTypeId,
                proceededDate, appliedDate, approvedDate,
                returnedDate, duration, noOfDays,
                leaveStatus, relieverName, employeeLeaveAllowanceRequest
            }

            await update(updateInfo, url).then((result) => {

                stopButtonSpin(submitButton);
                //form.reset();

            });

            stopButtonSpin(submitButton);
            //form.reset();
        }

    });
}



async function editLoanTypeForm(id) {
    axios.get(`${appConfig.apiBaseUrl}/loanTypes/loan-type/${id}`).then(obj => {

        $("#loanTypeNameInput").val(obj.data.data.name);

        $("#loanTypeDescriptionInput").val(obj.data.data.description);
    });

    let form = document.getElementById('editLoanTypeForm');

    form.addEventListener('submit', async e => {

        e.preventDefault();

        const submitButton = $('editLoanTypeBtn');

        spinButton(submitButton);

        const url = `${appConfig.apiBaseUrl}/loanTypes/${id}`;

        const name = $('#editLoanTypeForm input[name=name]').val();

        const description = $('#editLoanTypeForm input[name=description]').val();

        if (!name || !description) {

            notifyDataRequired('Kindly complete the form correctly.');
            stopButtonSpin(submitButton);
            //form.reset();

        } else {

            const updateInfo = {
                name, description
            }

            await update(updateInfo, url).then((result) => {

                stopButtonSpin(submitButton);
                //form.reset();

            });

            stopButtonSpin(submitButton);
            //form.reset();
        }

    });
}

async function editLoanForm(id) {
    axios.get(`${appConfig.apiBaseUrl}/loans/${id}`).then(obj => {

        if (obj.data.data.fullName !== null) {
            let empOption = new Option(obj.data.data.fullName, obj.data.data.employeeId, true, true);
            $("#employees").append(empOption).trigger('change');
        }

        if (obj.data.data.employeeLoanTypeName !== null) {
            let loanOption = new Option(obj.data.data.employeeLoanTypeName, obj.data.data.employeeLoanTypeId, true, true);
            $("#loantypes").append(loanOption).trigger('change');
        }

        $("#requestedAmountInput").val(obj.data.data.requestedAmount);

        $("#interestPercentageInput").val(obj.data.data.interestPercentage);

        $("#installmentNumberInput").val(obj.data.data.installmentNumber);

        $("#loanRequestStatusInput").val(obj.data.data.loanRequestStatus);

        $("#loanStatusInput").val(obj.data.data.loanStatusText);

    });

    let form = document.getElementById('editLoanForm');

    form.addEventListener('submit', async e => {

        e.preventDefault();

        const submitButton = $('editEmployeeLoanBtn');

        spinButton(submitButton);

        const url = `${appConfig.apiBaseUrl}/loans/edit-loan/${id}`;

        const employeeId = $('#editLoanForm select[name=employeeId]').val();

        const employeeLoanTypeId = $('#editLoanForm select[name=employeeLoanTypeId]').val();

        const requestedAmount = parseFloat($('#editLoanForm input[name=requestedAmount]').val());

        const interestPercentage = parseInt($('#editLoanForm input[name=interestPercentage]').val());

        const installmentNumber = parseInt($('#editLoanForm input[name=installmentNumber]').val());

        const loanRequestStatus = parseInt($('#editLoanForm select[name=loanRequestStatus]').val());

        if (!employeeId || !employeeLoanTypeId || !requestedAmount || !interestPercentage || !installmentNumber) {

            notifyDataRequired('Kindly complete the form correctly.');
            stopButtonSpin(submitButton);

        } else {

            const updateInfo = {
                employeeId, employeeLoanTypeId, requestedAmount, interestPercentage, installmentNumber, loanRequestStatus
            }

            await update(updateInfo, url).then((result) => {
                stopButtonSpin(submitButton);
            });

            stopButtonSpin(submitButton);
        }

    });
}

async function updateEmployeeForm(employeeNo) {

    let form = document.getElementById('updateEmployeeForm');

    form.addEventListener('submit', async e => {

        e.preventDefault();

        const submitButton = $('updateEmployeeFormBtn');

        spinButton(submitButton);


        const url = `${appConfig.apiBaseUrl}/employee/${employeeNo}`;


        const firstName = $('#updateEmployeeForm input[name=firstName]').val();
        const lastName = $('#updateEmployeeForm input[name=lastName]').val();
        const middleName = $('#updateEmployeeForm input[name=middleName]').val();
        const passportNumber = $('#updateEmployeeForm input[name=passportNumber]').val();
        const postalAddress = $('#updateEmployeeForm input[name=postalAddress]').val();
        const city = $('#updateEmployeeForm input[name=city]').val();
        const managerSupervisorNo = $('#updateEmployeeForm input[name=managerSupervisorNo]').val();
        const managerSupervisorEmail = $('#updateEmployeeForm input[name=managerEmail]').val();
        const supervisorManager = $('#updateEmployeeForm input[name=managerSupervisor]').val();
        const supervisorSJobTitle = $('#updateEmployeeForm input[name=supervisorSJobTitle]').val();
        const status = parseInt($('#updateEmployeeForm select[name=status]').val());
        const title = parseInt($('#updateEmployeeForm select[name=title]').val());
        const employeeCategory = parseInt($('#updateEmployeeForm select[name=employeeCategory]').val());
        const employeeCosting = parseInt($('#updateEmployeeForm select[name=employeeCosting]').val());
        const isHod = parseInt($('#updateEmployeeForm select[name=isHod]').val());
        const hr = parseInt($('#updateEmployeeForm select[name=hr]').val());
        const county = $('#updateEmployeeForm select[name=county]').val();
        const uploadToPayroll = parseInt($('#updateEmployeeForm select[name=uploadToPayroll]').val());
        const departmentCode = $('#updateEmployeeForm select[name=department]').val();
        const groupBonusCode = $('#updateEmployeeForm select[name=groupBonusCode]').val();
        const citizenship = $('#updateEmployeeForm select[name=citizenship]').val();
        const postCode = $('#updateEmployeeForm select[name=postCode]').val();
        const globalDimension2 = $('#updateEmployeeForm select[name=globalDimension2]').val();

        const branchBank = $('#updateEmployeeForm select[name=branchBank]').val();
        const bankAccountNumber = $('#updateEmployeeForm input[name=bankAccountNumber]').val();
        const mainBank = $('#updateEmployeeForm select[name=mainBank]').val();

        const companyEmail = $('#updateEmployeeForm input[name=companyEmail]').val();
        const homePhoneNumber = $('#updateEmployeeForm input[name=homePhoneNumber]').val();
        const cellPhoneNumber = $('#updateEmployeeForm input[name=cellPhoneNumber]').val();
        const faxNumber = $('#updateEmployeeForm input[name=faxNumber]').val();
        const workPhoneNumber = $('#updateEmployeeForm input[name=workPhoneNumber]').val();
        const ext = $('#updateEmployeeForm input[name=ext]').val();
        const personalEmail = $('#updateEmployeeForm input[name=personalEmail]').val();
        const altAddressCode = $('#updateEmployeeForm input[name=altAddressCode]').val();
        const altAddressStartDate = $('#updateEmployeeForm input[name=altAddressStartDate]').val();
        const altAddressEndDate = $('#updateEmployeeForm input[name=altAddressEndDate]').val();

        const endOfProbationDate = $('#updateEmployeeForm input[name=endOfProbationDate]').val();
        const pensionSchemeJoinDate = $('#updateEmployeeForm input[name=pensionSchemeJoinDate]').val();
        const medicalSchemeJoinDate = $('#updateEmployeeForm input[name=medicalSchemeJoinDate]').val();
        const timeOnMedicalScheme = $('#updateEmployeeForm input[name=timeOnMedicalScheme]').val();
        const dateOfBirth = $('#updateEmployeeForm input[name=dateOfBirth]').val();
        const weddingAnniversary = $('#updateEmployeeForm input[name=weddingAnniversary]').val();
        const age = $('#updateEmployeeForm input[name=age]').val();
        const dateOfJoiningTheCompany = $('#updateEmployeeForm input[name=dateOfJoiningTheCompany]').val();
        const lengthOfService = $('#updateEmployeeForm input[name=lengthOfService]').val();

        const jobTitle = $('#updateEmployeeForm select[name=jobTitle]').val();
        const staffGrade = $('#updateEmployeeForm select[name=staffGrade]').val();
        const salaryStep = $('#updateEmployeeForm select[name=salaryStep]').val();
        const postingGroup = $('#updateEmployeeForm select[name=postingGroup]').val();

        const contractType = $('#updateEmployeeForm select[name=contractType]').val();
        const sendAlertTo = $('#updateEmployeeForm input[name=sendAlertTo]').val();
        const noticePeriod = $('#updateEmployeeForm input[name=noticePeriod]').val();
        const contractEndDate = $('#updateEmployeeForm input[name=contractEndDate]').val();     
        const fullPartTime = parseInt($('#updateEmployeeForm select[name=fullPartTime]').val());

        const pinNumber = $('#updateEmployeeForm input[name=pinNumber]').val();
        const nssfNo = $('#updateEmployeeForm input[name=nssfNo]').val();
        const pensionFundAdministrator = $('#updateEmployeeForm select[name=pensionFundAdministrator]').val();
        const nhifNo = $('#updateEmployeeForm input[name=nhifNo]').val();

        const dateOfLeavingTheCompany = $('#updateEmployeeForm input[name=dateOfLeavingTheCompany]').val();
        const terminationGrounds = parseInt($('#updateEmployeeForm select[name=terminationGrounds]').val());
        const exitInterviewDate = $('#updateEmployeeForm input[name=exitInterviewDate]').val();
        const exitInterviewDoneBy = $('#updateEmployeeForm input[name=exitInterviewDoneBy]').val();
        const reasonForTermination = $('#updateEmployeeForm input[name=reasonForTermination]').val();

        const cashLeaveEarned = parseInt($('#updateEmployeeForm input[name=cashLeaveEarned]').val());
        const totalLeaveDays = parseInt($('#updateEmployeeForm input[name=totalLeaveDays]').val());
        const leaveStatus = parseInt($('#updateEmployeeForm select[name=leaveStatus]').val());
        const leaveTypeFilter = $('#updateEmployeeForm select[name=leaveTypeFilter]').val();
        const leaveBalance = parseInt($('#updateEmployeeForm input[name=leaveBalance]').val());
        const acruedLeaveDays = parseInt($('#updateEmployeeForm input[name=acruedLeaveDays]').val());
        const cashPerLeaveDay = parseInt($('#updateEmployeeForm input[name=cashPerLeaveDay]').val());

        const causeOfInactivityCode = $('#updateEmployeeForm input[name=causeOfInactivityCode]').val();
        const religion = $('#updateEmployeeForm select[name=religion]').val();
        const firstLanguageRWS = $('#updateEmployeeForm select[name=firstLanguageRWS]').val();
        const secondLanguageRWS = $('#updateEmployeeForm select[name=secondLanguageRWS]').val();
        const additionalLanguage = $('#updateEmployeeForm input[name=additionalLanguage]').val();
        const driverLicenseNo = $('#updateEmployeeForm input[name=driverLicenseNo]').val();
        const vehicleRegistrationNumber = $('#updateEmployeeForm input[name=vehicleRegistrationNumber]').val();
        const disabilityDetails = $('#updateEmployeeForm input[name=disabilityDetails]').val();
        const medicalSchemeNo = $('#updateEmployeeForm input[name=medicalSchemeNo]').val();
        const medicalSchemeHeadMember = $('#updateEmployeeForm input[name=medicalSchemeHeadMember]').val();
        const medicalSchemeName = $('#updateEmployeeForm input[name=medicalSchemeName]').val();
        const gender = parseInt($('#updateEmployeeForm select[name=gender]').val());
        const maritalStatus = parseInt($('#updateEmployeeForm select[name=maritalStatus]').val());
        const hasDrivingLicence = parseInt($('#updateEmployeeForm select[name=hasDrivingLicence]').val());
        const disabled = parseInt($('#updateEmployeeForm select[name=disabled]').val());
        const numberOfDependants = parseInt($('#updateEmployeeForm input[name=numberOfDependants]').val());
        const healthAssesmentDate = $('#updateEmployeeForm input[name=healthAssesmentDate]').val();
        const signature = $('#updateEmployeeForm input[name=signature]').val();
        const healthAssesment = parseInt($('#updateEmployeeForm select[name=healthAssesment]').val());
        const firstLanguageRead = $('#updateEmployeeForm input[name=firstLanguageRead]').val();
        const firstLanguageWrite = $('#updateEmployeeForm input[name=firstLanguageWrite]').val();
        const firstLanguageSpeak = $('#updateEmployeeForm input[name=firstLanguageSpeak]').val();
        const secondLanguageRead = $('#updateEmployeeForm input[name=secondLanguageRead]').val();
        const secondLanguageWrite = $('#updateEmployeeForm input[name=secondLanguageWrite]').val();
        const secondLanguageSpeak = $('#updateEmployeeForm input[name=secondLanguageSpeak]').val();
        //const departmentId = $('#updateEmployeeForm select[name=departmentId]').val();
       
        //
       
        
        //const payeNumber = $('#updateEmployeeForm input[name=payeNumber]').val();
        
        
        
        //const gradeId = $('#updateEmployeeForm select[name=gradeId]').val();
        //const jobId = $('#updateEmployeeForm select[name=jobId]').val();
        //const religionId = $('#updateEmployeeForm select[name=religionId]').val();
        //const dateJoinedCompany = $('#updateEmployeeForm input[name=dateJoinedCompany]').val();
   
        /*const applicationNumber = $('#updateEmployeeForm input[name=applicationNumber]').val();
        const applicationDate = $('#updateEmployeeForm input[name=applicationDate]').val();
        const trainingGroupNo = $('#updateEmployeeForm input[name=trainingGroupNo]').val();
        const noOfParticipant = $('#updateEmployeeForm input[name=noOfParticipant]').val();
        const employeeName = $('#updateEmployeeForm input[name=employeeName]').val();*/
        /*
        const divisionCode = $('#updateEmployeeForm input[name=divisionCode]').val();
        const trainingCategory = $('#updateEmployeeForm input[name=trainingCategory]').val();
        const trainingCategory1 = $('#updateEmployeeForm input[name=trainingCategory1]').val();
        const purposeOfTraining = $('#updateEmployeeForm input[name=purposeOfTraining]').val();
        const location = $('#updateEmployeeForm input[name=location]').val();
        const provider = $('#updateEmployeeForm input[name=provider]').val();
        const providerName = $('#updateEmployeeForm input[name=providerName]').val();*/
        /*const responsibilityCenter = $('#updateEmployeeForm input[name=responsibilityCenter]').val();
        const stage = $('#updateEmployeeForm input[name=stage]').val();
        const userId = $('#updateEmployeeForm input[name=userId]').val();
        const nominatorName = $('#updateEmployeeForm input[name=nominatorName]').val();
        const nominatorEmail = $('#updateEmployeeForm input[name=nominatorEmail]').val();
        const nominatorDepartment = $('#updateEmployeeForm input[name=nominatorDepartment]').val();
        const departmentLocation = $('#updateEmployeeForm input[name=departmentLocation]').val();
        
        
        
        const jobCode = $('#updateEmployeeForm select[name=jobCode]').val();
        const jobTitle = $('#updateEmployeeForm input[name=jobTitle]').val();
        const staffGrade = $('#updateEmployeeForm select[name=staffGrade]').val();
        const salaryStep = $('#updateEmployeeForm select[name=salaryStep]').val();
        
        
        const reimbursedLeaveDays = $('#updateEmployeeForm input[name=reimbursedLeaveDays]').val();     
        const allocatedLeaveDays = $('#updateEmployeeForm input[name=allocatedLeaveDays]').val();
        */
        //const userName = $('#createEmployeeForm input[name=userName]').val();
        //const password = $('#createEmployeeForm input[name=password]').val();


        if (!firstName || !lastName) {

            notifyDataRequired('Kindly complete the form correctly.');
            stopButtonSpin(submitButton);
            //form.reset();

        } else {

            const updateInfo = {

                firstName, lastName, middleName, passportNumber, postalAddress, city, managerSupervisorNo, supervisorManager, managerSupervisorEmail, supervisorSJobTitle, title, status, county, isHod, hr, uploadToPayroll, employeeCategory, employeeCosting, postCode, groupBonusCode, citizenship, departmentCode, globalDimension2, mainBank, branchBank, bankAccountNumber, companyEmail, homePhoneNumber, cellPhoneNumber, faxNumber, workPhoneNumber, ext, personalEmail, altAddressCode, altAddressStartDate, altAddressEndDate, dateOfBirth, age, dateOfJoiningTheCompany, endOfProbationDate,
                pensionSchemeJoinDate, medicalSchemeJoinDate, timeOnMedicalScheme, weddingAnniversary, lengthOfService, jobTitle, staffGrade, salaryStep, postingGroup, contractType, sendAlertTo, noticePeriod, contractEndDate, fullPartTime, pensionFundAdministrator, pinNumber, nssfNo, nhifNo, dateOfLeavingTheCompany, terminationGrounds, exitInterviewDate, exitInterviewDoneBy, reasonForTermination, leaveBalance, cashPerLeaveDay, totalLeaveDays, leaveStatus, cashLeaveEarned, acruedLeaveDays, leaveTypeFilter, causeOfInactivityCode, religion, firstLanguageRWS, secondLanguageRWS, additionalLanguage, driverLicenseNo, vehicleRegistrationNumber, disabilityDetails, medicalSchemeNo, medicalSchemeHeadMember, medicalSchemeName, gender, maritalStatus, disabled, hasDrivingLicence, numberOfDependants, healthAssesmentDate, signature, healthAssesment, firstLanguageRead, firstLanguageWrite, firstLanguageSpeak, secondLanguageRead, secondLanguageWrite, secondLanguageSpeak
               
            }

            await update(updateInfo, url).then((result) => {

                stopButtonSpin(submitButton);
               // form.reset();

            });

            stopButtonSpin(submitButton);
            //form.reset();
        }
    });
}
    