let authConfig = {
    authTokenkey: "authToken",
    authExpirykey: "authExpiry",
    userInfo: "userInfo",
    //selectedValues: "selectedValues"
}

let permissionConfig = {
    "admin": "Admin",
    "employee": "Employee"
}


function isAuthenticated() {
    const token = localStorage.getItem(authConfig.authTokenkey);
    const expiry = localStorage.getItem(authConfig.authExpirykey);

    if (!token || !expiry) {
        return false;
    }
    else if (new Date(parseInt(expiry)).getTime() < Date.now()) {

        return false;
    }
    else {
        return true;
    }
}

function getAuthToken() {
    const token = localStorage.getItem(authConfig.authTokenkey);
    return token;
}

function getUserInfo() {
    if (isAuthenticated()) {
        const userInfo = JSON.parse(localStorage.getItem(authConfig.userInfo));
        return userInfo;
    } else {
        return null;
    }
}

function setAuthInfo(authToken, authExpiry, userInfo) {
    localStorage.setItem(authConfig.authTokenkey, authToken);
    localStorage.setItem(authConfig.authExpirykey, authExpiry);
    const userInfoString = JSON.stringify(userInfo) || "";
    localStorage.setItem(authConfig.userInfo, userInfoString);
}

function clearAuthInfo() {
    localStorage.removeItem(authConfig.authTokenkey);
    localStorage.removeItem(authConfig.authExpirykey);
    localStorage.removeItem(authConfig.userInfo);
    localStorage.removeItem('selectedvalues');
}

async function login(userName, password) {
    try {
        const data = { userName: userName, password };
        const response = await axios.post('/users/token', data);
        const token = response.headers.token;
        const tokenExpiry = response.headers.tokenexpiry;
        const userInfo = {
            userId: response.data.data.userId,
            userName: response.data.data.userName,
            roles: response.data.data.roles
        }
        setAuthInfo(token, tokenExpiry, userInfo);
        updateUserInfoDisplay();
        notifyLoginSuccess();
        return true;

    } catch (error) {
        notifyLoginError(error.response.data.message);
        return false;
    }
}

async function register(registrationInfo, url) {
    try {
        const data = registrationInfo;
        const response = await axios.post(url, data, {
            headers: { Tenant: 'delta' }
        });
        if (response) {
            notifyRegistrationSuccess(response.data.message);
        }
    } catch (error) {
        notifyGeneralError(error.response.data.message);
    }
}

async function update(updateInfo, url, queryParams = null) {
    if (queryParams !== null) {
        try {
            const data = updateInfo;

            const response = await axios.put(url, queryParams, data);
            if (response) {
                notifyRegistrationSuccess(response.data.message);
            }
        } catch (error) {
            notifyGeneralError(error.response.data.message);
        }
    }
    else {

        try {
            const data = updateInfo;
            const response = await axios.put(url, data);
            if (response) {
                notifyRegistrationSuccess(response.data.message);
            }
        } catch (error) {
            notifyGeneralError(error.response.data.message);
        }
    }
}



function logout() {
    clearAuthInfo();
    window.location.href = appConfig.loginUrl;
}

/*function getDataList(obj, element, placeholder) {
    element.append(`<option value="">${placeholder}</option>`);
    $.each(obj, function (key, entry) {
        element.append($('<option></option>').attr('value', entry.id).text(entry.name));
    });
}*/

function updateUserInfoDisplay() {
    if (isAuthenticated()) {
        $("#userInfo").show();
        $("#userInfo").text(getUserInfo().userName);
        $("#roleUser").text(`${getUserInfo().roles}`);
        $("#logout").show();
    }
    else {
        $("#logout").hide();
        $("#userInfo").hide();
    }

}

function setSelectedValues(elementId, values) {
    if (values && values.length > 0) {
        $(`#${elementId}`).val(values);
    }
}

function updateSideMenu() {
    if (isAdmin()) {
        $("#security-section").show();
        $("#user-section-menu").show();
        $("#role-section-menu").show();
        $("#employee-section").show();
        $("#employee-section-menu").show();
        $("#employees-section").show();
        $("#employees-section-menu").show();
        $("#system-section").show();
        $("#department-section-menu").show();
        $("#grade-section-menu").show();
        $("#job-section-menu").show();
        $("#religion-section-menu").show();
    }
    else {
        $("#security-section").hide();
        $("#user-section-menu").hide();
        $("#role-section-menu").hide();
        $("#employee-section").hide();
        $("#employee-section-menu").hide();
        $("#employees-section").hide();
        $("#employees-section-menu").hide();
        $("#system-section").hide();
        $("#department-section-menu").hide();
        $("#grade-section-menu").hide();
        $("#job-section-menu").hide();
        $("#religion-section-menu").hide();
        $("#profile-section-menu").show();
    }

    /*if (isAdmin() || isEmployee()) {
        $("#user-section").show();
        $("#user-section-menu").show();
    } else {
        $("#user-section").hide();
        $("#user-section-menu").hide();
    }*/
}

function updateTopBarMenu() {
    if (isAdmin()) {
        $("#security-section").show();
        $("#user-section-menu").show();
        $("#role-section-menu").show();
        $("#employee-section").show();
        $("#employee-section-menu").show();
        $("#employees-section").show();
        $("#employees-section-menu").show();
        $("#system-section").show();
        $("#department-section-menu").show();
        $("#grade-section-menu").show();
        $("#job-section-menu").show();
        $("#religion-section-menu").show();
    }
    else {
        $("#security-section").hide();
        $("#user-section-menu").hide();
        $("#role-section-menu").hide();
        $("#employee-section").hide();
        $("#employee-section-menu").hide();
        $("#system-section").hide();
        $("#department-section-menu").hide();
        $("#grade-section-menu").hide();
        $("#job-section-menu").hide();
        $("#religion-section-menu").hide();
        $("#profile-section-menu").show();
    }

    /*if (isAdmin() || isEmployee()) {
        $("#user-section").show();
        $("#user-section-menu").show();
    } else {
        $("#user-section").hide();
        $("#user-section-menu").hide();
    }*/
}

function isAdmin() {
    const userRoles = getUserInfo().roles || [];
    const isAdmin = userRoles.some(role => role.toLowerCase() === permissionConfig.admin.toLowerCase());
    return isAdmin;
}

function isEmployee() {
    const userRoles = getUserInfo().roles || [];
    const isEmployee = userRoles.some(role => role.toLowerCase() === permissionConfig.employee.toLowerCase());
    return isEmployee;
}


async function viewProfileForm() {


    let form = document.getElementById('changeProfilePasswordForm');

    form.addEventListener('submit', async e => {

        e.preventDefault();

        const submitButton = $('changePasswordBtn');

        spinButton(submitButton);

        const url = `${appConfig.apiBaseUrl}/account/changepassword`;
        const id = getUserInfo().userId
        const currentPassword = $('#changeProfilePasswordForm input[name=currentPassword]').val();
        const newPassword = $('#changeProfilePasswordForm input[name=newPassword]').val();
        const confirmPassword = $('#changeProfilePasswordForm input[name=confirmPassword]').val();


        if (!currentPassword || !newPassword) {

            notifyDataRequired('Kindly complete the form correctly.');
            stopButtonSpin(submitButton);
            form.reset();

        } else {
            if (confirmPassword != newPassword) {

                notifyValueNotEqual("Password does not match!");

            }
            else {

                const changePasswordInfo = {

                    id, currentPassword, newPassword, confirmPassword
                }

                await changePassword(changePasswordInfo, url).then((result) => {

                    stopButtonSpin(submitButton);
                    form.reset();

                });

                stopButtonSpin(submitButton);
                form.reset();
            }
        }

    });
}
async function changePasswordForm() {


    let form = document.getElementById('changeProfilePasswordForm');

    form.addEventListener('submit', async e => {

        e.preventDefault();

        const submitButton = $('changePasswordBtn');

        spinButton(submitButton);

        const url = `${appConfig.apiBaseUrl}/account/changepassword`;
        const id = getUserInfo().userId
        const currentPassword = $('#changeProfilePasswordForm input[name=currentPassword]').val();
        const newPassword = $('#changeProfilePasswordForm input[name=newPassword]').val();
        const confirmPassword = $('#changeProfilePasswordForm input[name=confirmPassword]').val();


        if (!currentPassword || !newPassword) {

            notifyDataRequired('Kindly complete the form correctly.');
            stopButtonSpin(submitButton);
            form.reset();

        } else {
            if (confirmPassword != newPassword) {

                notifyValueNotEqual("Password does not match!");

            }
            else {

                const changePasswordInfo = {

                    id, currentPassword, newPassword, confirmPassword
                }

                await changePassword(changePasswordInfo, url).then((result) => {

                    stopButtonSpin(submitButton);
                    form.reset();

                });

                stopButtonSpin(submitButton);
                form.reset();
            }
        }

    });
}