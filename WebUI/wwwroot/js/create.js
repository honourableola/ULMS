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
        const instructorPhoto = $('#createInstructorForm input[name=instructorPhoto]').val();

        if (!firstName || !lastName || !email || phoneNumber || !instructorPhoto) {
            notifyDataRequired('Kindly complete the form correctly.');
            stopButtonSpin(submitButton);
            form.reset();

        } else {

            const registrationInfo = {
                firstName, lastName, email, phoneNumber, instructorPhoto
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

async function handleCreateLearnerForm() {

    let form = document.getElementById('createLearnerForm');

    form.addEventListener('submit', async e => {

        e.preventDefault();

        const submitButton = $('createLearnerBtn');

        spinButton(submitButton);

        const url = `${appConfig.apiBaseUrl}/learner/AddLearner`;

        const firstName = $('#createLearnerForm input[name=firstName]').val();
        const lastName = $('#createLearnerForm input[name=lastName]').val();
        const email = $('#createLearnerForm input[name=email]').val();
        const phoneNumber = $('#createLearnerForm input[name=phoneNumber]').val();
        const learnerPhoto = $('#createLearnerForm input[name=learnerPhoto]').val();

        if (!firstName || !lastName || !email || !phoneNumber) {
            notifyDataRequired('Kindly complete the form correctly.');
            stopButtonSpin(submitButton);
            form.reset();

        } else {

            const registrationInfo = {
                firstName, lastName, email, phoneNumber, learnerPhoto
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

async function handleCreateAdminForm() {

    let form = document.getElementById('createAdminForm');

    form.addEventListener('submit', async e => {

        e.preventDefault();

        const submitButton = $('createAdminBtn');

        spinButton(submitButton);

        const url = `${appConfig.apiBaseUrl}/admin/AddAdmin`;

        const firstName = $('#createAdminForm input[name=firstName]').val();
        const lastName = $('#createAdminForm input[name=lastName]').val();
        const email = $('#createAdminForm input[name=email]').val();
        const phoneNumber = $('#createAdminForm input[name=phoneNumber]').val();
        const adminPhoto = $('#createAdminForm input[name=adminPhoto]').val();

        if (!firstName || !lastName || !email || !phoneNumber) {
            notifyDataRequired('Kindly complete the form correctly.');
            stopButtonSpin(submitButton);
            form.reset();

        } else {

            const registrationInfo = {
                firstName, lastName, email, phoneNumber, adminPhoto
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

async function handleCreateCategoryForm() {

    let form = document.getElementById('createCategoryForm');

    form.addEventListener('submit', async e => {

        e.preventDefault();

        const submitButton = $('createCategoryBtn');

        spinButton(submitButton);

        const url = `${appConfig.apiBaseUrl}/category/Addcategory`;

        const name = $('#createCategoryForm input[name=name]').val();

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

async function handleCreateCourseForm() {

    let form = document.getElementById('createCourseForm');

    form.addEventListener('submit', async e => {

        e.preventDefault();

        const submitButton = $('createCourseBtn');

        spinButton(submitButton);

        const url = `${appConfig.apiBaseUrl}/course/AddCourse`;

        const name = $('#createCourseForm input[name=name]').val();
        const description = $('#createCourseForm input[name=description]').val();
        const categoryId = $('#createCourseForm select[name=categoryId]').val();
     

        if (!name || !description || !categoryId ) {
            notifyDataRequired('Kindly complete the form correctly.');
            stopButtonSpin(submitButton);
            form.reset();

        } else {

            const courseInfo = {
                name, description, categoryId
            }

            await register(courseInfo, url).then((result) => {

                stopButtonSpin(submitButton);
                form.reset();
            });

            stopButtonSpin(submitButton);
            form.reset();
        }

    });
}

async function handleCreateSettingsForm() {

    let form = document.getElementById('createSettingsForm');

    form.addEventListener('submit', async e => {

        e.preventDefault();

        const submitButton = $('createSettingsBtn');

        spinButton(submitButton);

        const url = `${appConfig.apiBaseUrl}/courseConstant/AddCourseConstant`;

        const maximumNoOfMajorCourses = $('#createCourseConstantForm input[name=maximumNoOfMajorCourses]').val();
        const maximumNoOfAdditionalCourses = $('#createCourseConstantForm input[name=maximumNoOfAdditionalCourses]').val();
        const noOfAssessmentQuestions = $('#createCourseConstantForm input[name=noOfAssessmentQuestions]').val();
        const durationOfAssessment = $('#createCourseConstantForm input[name=durationOfAssessment]').val();

        if (!maximumNoOfMajorCourses) {

            notifyDataRequired('Kindly complete the form correctly.');
            stopButtonSpin(submitButton);
            form.reset();

        } else {

            const registrationInfo = {

                maximumNoOfAdditionalCourses, maximumNoOfMajorCourses, noOfAssessmentQuestions, durationOfAssessment
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

async function handleCreateModuleForm() {

    let form = document.getElementById('createModuleForm');

    form.addEventListener('submit', async e => {

        e.preventDefault();

        const submitButton = $('createModuleBtn');

        spinButton(submitButton);

        const url = `${appConfig.apiBaseUrl}/module/AddModule`;

        const name = $('#createModuleForm input[name=name]').val();
        const description = $('#createModuleForm input[name=description]').val();
        const content = $('#createModuleForm input[name=content]').val();
        const courseId = $('#createModuleForm select[name=courseId]').val();
        const moduleImage1 = $('#createModuleForm input[name=moduleImage1]').val();
        const moduleImage2 = $('#createModuleForm input[name=moduleImage2]').val();
        const modulePDF1 = $('#createModuleForm input[name=modulePDF1]').val();
        const modulePDF2 = $('#createModuleForm input[name=modulePDF2]').val();
        const moduleVideo1 = $('#createModuleForm input[name=moduleVideo1]').val();
        const moduleVideo2 = $('#createModuleForm input[name=moduleVideo2]').val();

        if (!name || !description || !content || !courseId) {

            notifyDataRequired('Kindly complete the form correctly.');
            stopButtonSpin(submitButton);
            form.reset();

        } else {

            const registrationInfo = {

                name, description, content, courseId, moduleImage1, moduleImage2, modulePDF1, modulePDF2, moduleVideo1, moduleVideo2
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

async function handleCreateTopicForm() {

    let form = document.getElementById('createTopicForm');

    form.addEventListener('submit', async e => {

        e.preventDefault();

        const submitButton = $('createTopicBtn');

        spinButton(submitButton);

        const url = `${appConfig.apiBaseUrl}/topic/AddTopic`;

        const title = $('#createCourseConstantForm input[name=title]').val();
        const content = $('#createCourseConstantForm input[name=content]').val();
        const moduleId = $('#createCourseConstantForm select[name=moduleId]').val();

        if (!title || !content || !moduleId ) {

            notifyDataRequired('Kindly complete the form correctly.');
            stopButtonSpin(submitButton);
            form.reset();

        } else {

            const registrationInfo = {

                title, content, moduleId
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

async function handleCreateQuestionForm() {

    let form = document.getElementById('createQuestionForm');

    form.addEventListener('submit', async e => {

        e.preventDefault();

        const submitButton = $('createQuestionBtn');

        spinButton(submitButton);

        const url = `${appConfig.apiBaseUrl}/question/AddQuestion`;

        const questionText = $('#createCourseConstantForm input[name=questionText]').val();
        const points = $('#createCourseConstantForm input[name=points]').val();
        const moduleId = $('#createCourseConstantForm select[name=moduleId]').val();

        if (!points || !questionText || !moduleId) {

            notifyDataRequired('Kindly complete the form correctly.');
            stopButtonSpin(submitButton);
            form.reset();

        } else {

            const registrationInfo = {

                questionText, points, moduleId
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

async function handleCreateOptionForm() {

    let form = document.getElementById('createOptionForm');

    form.addEventListener('submit', async e => {

        e.preventDefault();

        const submitButton = $('createOptionBtn');

        spinButton(submitButton);

        const url = `${appConfig.apiBaseUrl}/option/AddOption`;

        const label = $('#createCourseConstantForm input[name=label]').val();
        const optionText = $('#createCourseConstantForm input[name=optionText]').val();
        const status = $('#createCourseConstantForm select[name=status]').val();
        const questionId = $('#createCourseConstantForm select[name=questionId]').val();

        if (!label || !optionText || !status | questionId) {

            notifyDataRequired('Kindly complete the form correctly.');
            stopButtonSpin(submitButton);
            form.reset();

        } else {

            const registrationInfo = {

                label, optionText, status, questionId
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


async function handleCreateRoleForm() {

    let form = document.getElementById('createRoleForm');

    form.addEventListener('submit', async e => {

        e.preventDefault();

        const submitButton = $('createRoleBtn');

        spinButton(submitButton);

        const url = `${appConfig.apiBaseUrl}/role/AddRole`;

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
