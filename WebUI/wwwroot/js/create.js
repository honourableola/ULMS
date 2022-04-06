async function handleCreateInstructorForm() {

    let form = document.getElementById('createInstructorForm');

    form.addEventListener('submit', async e => {

        e.preventDefault();

        const submitButton = $('createInstructorBtn');

        spinButton(submitButton);

        const url = `${appConfig.apiBaseUrl}/instructor/AddInstructor`;

        const firstName = $('#createInstructorForm input[name=firstName]').val();
        const lastName = $('#createInstructorForm input[name=lastName]').val();
        const email = $('#createInstructorForm input[name=email]').val();
        const phoneNumber = $('#createInstructorForm input[name=phoneNumber]').val();

        if (!firstName) {

            notifyDataRequired('Kindly complete the form correctly.');
            stopButtonSpin(submitButton);
            form.reset();

        } else {

            const registrationInfo = {
                firstName, lastName, email, phoneNumber
            }

            await register(registrationInfo, url).then((result) => {

                stopButtonSpin(submitButton);
                form.reset();
            });

            stopButtonSpin(submitButton);
            form.reset();
        }

    });
}

async function handleCreateGradeForm() {

    let form = document.getElementById('createGradeForm');

    form.addEventListener('submit', async e => {

        e.preventDefault();

        const submitButton = $('createGradeBtn');

        spinButton(submitButton);

        const url = `${appConfig.apiBaseUrl}/grades`;

        const name = $('#createGradeForm input[name=name]').val();

        if (!name) {

            notifyDataRequired('Kindly complete the form correctly.');
            stopButtonSpin(submitButton);
            form.reset();

        } else {

            const gradeInfo = {

                name
            }

            await register(gradeInfo, url).then((result) => {

                stopButtonSpin(submitButton);
                form.reset();

            });

            stopButtonSpin(submitButton);
            form.reset();
        }

    });
}

async function handleCountryForm() {

    let form = document.getElementById('createCountryForm');

    form.addEventListener('submit', async e => {

        e.preventDefault();

        const submitButton = $('countryBtn');

        spinButton(submitButton);

        const url = `${appConfig.apiBaseUrl}/countries`;

        const name = $('#createCountryForm input[name=name]').val();

        if (!name) {

            notifyDataRequired('Kindly complete the form correctly.');
            stopButtonSpin(submitButton);
            form.reset();

        } else {

            const registrationInfo = {

                name
            }

            await register(registrationInfo, url).then((result) => {

                stopButtonSpin(submitButton);
                form.reset();

            });

            stopButtonSpin(submitButton);
            form.reset();
        }

    });
}

async function handleRoleForm() {

    let form = document.getElementById('createRoleForm');

    form.addEventListener('submit', async e => {

        e.preventDefault();

        const submitButton = $('createRoleBtn');

        spinButton(submitButton);

        const url = `${appConfig.apiBaseUrl}/roles`;

        const name = $('#createRoleForm input[name=name]').val();
        const description = $('#createRoleForm input[name=description]').val();

        if (!name || !description) {

            notifyDataRequired('Kindly complete the form correctly.');
            stopButtonSpin(submitButton);
            form.reset();

        } else {

            const registerRoleInfo = {

                name, description
            }

            await register(registerRoleInfo, url).then((result) => {

                stopButtonSpin(submitButton);
                form.reset();

            });

            stopButtonSpin(submitButton);
            form.reset();
        }

    });
}


async function handleCreateReligionForm() {

    let form = document.getElementById('createReligionForm');

    form.addEventListener('submit', async e => {

        e.preventDefault();

        const submitButton = $('createReligionBtn');

        spinButton(submitButton);

        const url = `${appConfig.apiBaseUrl}/religions`;

        const name = $('#createReligionForm input[name=name]').val();

        if (!name) {

            notifyDataRequired('Kindly complete the form correctly.');
            stopButtonSpin(submitButton);
            form.reset();

        } else {

            const registrationInfo = {

                name
            }

            await register(registrationInfo, url).then((result) => {

                stopButtonSpin(submitButton);
                form.reset();

            });

            stopButtonSpin(submitButton);
            form.reset();
        }

    });
}

async function handleCreateDepartmentForm() {

    let form = document.getElementById('createDepartmentForm');

    form.addEventListener('submit', async e => {

        e.preventDefault();

        const submitButton = $('createDepartmentBtn');

        spinButton(submitButton);

        const url = `${appConfig.apiBaseUrl}/departments`;

        const name = $('#createDepartmentForm input[name=name]').val();

        const departmentCode = $('#createDepartmentForm input[name=departmentCode]').val();



        if (!name || !departmentCode) {

            notifyDataRequired('Kindly complete the form correctly.');
            stopButtonSpin(submitButton);
            form.reset();

        } else {

            const formInfo = {

                name, departmentCode
            }

            await register(formInfo, url).then((result) => {

                stopButtonSpin(submitButton);
                form.reset();

            });

            stopButtonSpin(submitButton);
            form.reset();
        }

    });
}

async function handleEmployeeForm() {

    let form = document.getElementById('createEmployeeForm');

    form.addEventListener('submit', async e => {

        e.preventDefault();

        const submitButton = $('createEmployeeBtn');

        spinButton(submitButton);

        const url = `${appConfig.apiBaseUrl}/users/employee`;

        const userName = $('#createEmployeeForm input[name=userName]').val();
        const password = $('#createEmployeeForm input[name=password]').val();



        if (!userName || !password) {

            notifyDataRequired('Kindly complete the form correctly.');
            stopButtonSpin(submitButton);


        } else {

            const registerRoleInfo = {
                userName, password
            }
            await register(registerRoleInfo, url).then((result) => {

                stopButtonSpin(submitButton);
                form.reset();

            });

            stopButtonSpin(submitButton);
            //form.reset();
        }

    });
}



function spinButton(btn) {
    btn.addClass("kt-spinner kt-spinner--right kt-spinner--sm kt-spinner--light").attr("disabled", true);
}

function stopButtonSpin(btn) {
    btn.removeClass("kt-spinner kt-spinner--right kt-spinner--sm kt-spinner--light").attr("disabled", false);
}

async function handleCreateLeaveTypeForm() {

    let form = document.getElementById('createLeaveTypeForm');

    form.addEventListener('submit', async e => {

        e.preventDefault();

        const submitButton = $('createLeaveTypeBtn');

        spinButton(submitButton);

        const url = `${appConfig.apiBaseUrl}/leaveTypes`;

        const name = $('#createLeaveTypeForm input[name=name]').val();

        if (!name) {

            notifyDataRequired('Kindly complete the form correctly.');
            stopButtonSpin(submitButton);
            form.reset();

        } else {

            const leaveTypeInfo = {

                name
            }

            await register(leaveTypeInfo, url).then((result) => {

                stopButtonSpin(submitButton);
                form.reset();

            });

            stopButtonSpin(submitButton);
            form.reset();
        }

    });
}

async function handleEmployeeLeaveForm() {

    let form = document.getElementById('createEmployeeLeaveForm');

    form.addEventListener('submit', async e => {

        e.preventDefault();

        const submitButton = $('createEmployeeLeaveBtn');

        spinButton(submitButton);

        const url = `${appConfig.apiBaseUrl}/leaves/create-leave`;

        const employeeId = $('#createEmployeeLeaveForm select[name=employeeId]').val();
        const year = parseInt($('#createEmployeeLeaveForm input[name=year]').val());
        const employeeLeaveTypeId = $('#createEmployeeLeaveForm select[name=employeeLeaveTypeId]').val();
        const proceededDate = $('#createEmployeeLeaveForm input[name=proceededDate]').val();
        const appliedDate = $('#createEmployeeLeaveForm input[name=appliedDate]').val();
        //const approvedDate = $('#createEmployeeLeaveForm input[name=approvedDate]').val();
        const returnedDate = $('#createEmployeeLeaveForm input[name=returnedDate]').val();
        //const duration = parseInt($('#createEmployeeLeaveForm input[name=duration]').val());
        //const noOfDays = parseInt($('#createEmployeeLeaveForm input[name=noOfDays]').val());
        const leaveStatus = parseInt($('#createEmployeeLeaveForm select[name=leaveStatus]').val());
        const relieverName = $('#createEmployeeLeaveForm input[name=relieverName]').val();
        const employeeLeaveAllowanceRequest = Boolean($('#createEmployeeLeaveForm input[name=employeeLeaveAllowanceRequest]').val());


        if (!employeeId || !year || !employeeLeaveTypeId || !proceededDate || !appliedDate || !returnedDate || !leaveStatus || !relieverName) {

            notifyDataRequired('Kindly complete the form correctly.');
            stopButtonSpin(submitButton);

        } else {

            const registerLeaveInfo = {
                employeeId, year, employeeLeaveTypeId,
                proceededDate, returnedDate, appliedDate,
                leaveStatus, relieverName, employeeLeaveAllowanceRequest
            }
            await register(registerLeaveInfo, url).then((result) => {

                stopButtonSpin(submitButton);
                form.reset();

            });

            stopButtonSpin(submitButton);
            //form.reset();
        }

    });
}

async function handleCreateLoanTypeForm() {

    let form = document.getElementById('createLoanTypeForm');

    form.addEventListener('submit', async e => {

        e.preventDefault();

        const submitButton = $('createLoanTypeBtn');

        spinButton(submitButton);

        const url = `${appConfig.apiBaseUrl}/loanTypes`;

        const name = $('#createLoanTypeForm input[name=name]').val();

        const description = $('#createLoanTypeForm input[name=description]').val();

        if (!name || !description) {

            notifyDataRequired('Kindly complete the form correctly.');
            stopButtonSpin(submitButton);
            form.reset();

        } else {

            const loanTypeInfo = {

                name, description
            }

            await register(loanTypeInfo, url).then((result) => {

                stopButtonSpin(submitButton);
                form.reset();

            });

            stopButtonSpin(submitButton);
            form.reset();
        }

    });
}

async function handleEmployeeLoanForm() {

    let form = document.getElementById('createEmployeeLoanForm');

    form.addEventListener('submit', async e => {

        e.preventDefault();

        const submitButton = $('createEmployeeLoanBtn');

        spinButton(submitButton);

        const url = `${appConfig.apiBaseUrl}/loans/create-loan`;

        const employeeId = $('#createEmployeeLoanForm select[name=employeeId]').val();
        const employeeLoanTypeId = $('#createEmployeeLoanForm select[name=employeeLoanTypeId]').val();
        const requestedAmount = parseFloat($('#createEmployeeLoanForm input[name=requestedAmount]').val());
        const interestPercentage = parseInt($('#createEmployeeLoanForm input[name=interestPercentage]').val());
        const installmentNumber = parseInt($('#createEmployeeLoanForm input[name=installmentNumber]').val());

        if (!employeeId || !employeeLoanTypeId || !requestedAmount || !interestPercentage || !installmentNumber) {

            notifyDataRequired('Kindly complete the form correctly!');
            stopButtonSpin(submitButton);

        } else {

            const registerLoanInfo = {
                employeeId, employeeLoanTypeId, requestedAmount,
                interestPercentage, installmentNumber
            }
            await register(registerLoanInfo, url).then((result) => {

                stopButtonSpin(submitButton);
                form.reset();

            });

            stopButtonSpin(submitButton);
        }

    });
}


async function handleEmployeeLoanInstallmentForm() {

    let form = document.getElementById('createEmployeeLoanInstallmentForm');

    form.addEventListener('submit', async e => {

        e.preventDefault();

        const submitButton = $('createEmployeeLoanInstallmentBtn');

        spinButton(submitButton);

        const url = `${appConfig.apiBaseUrl}/loan-installments/create-loan-installment`;

        const employeeId = $('#editLoanInstallmentForm select[name=employeeId]').val();
        const installmentAmount = parseInt($('#editLoanInstallmentForm input[name=installmentAmount]').val());
        const paidAmount = parseInt($('#editLoanInstallmentForm input[name=paidAmount]').val());



        if (!employeeId || !employeeLoanId || !installmentAmount || !paidAmount) {

            notifyDataRequired('Kindly complete the form correctly.');
            stopButtonSpin(submitButton);
            //form.reset();

        } else {

            const registerLoanInfo = {
                employeeId, employeeLoanId,
                installmentAmount, paidAmount
            }

            await register(registerLoanInfo, url).then((result) => {

                stopButtonSpin(submitButton);
                //form.reset();

            });

            stopButtonSpin(submitButton);
            //form.reset();
        }

    });
}
