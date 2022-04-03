function emptyGuid() {
    return '00000000-0000-0000-0000-000000000000';
}

function getSelectFieldValues(element) {

    return element.map(function () { if ($(this).val() != "") return $(this).val(); }).get();

}

async function handleLoginForm(returnUrl) {

    let form = document.getElementById('loginForm');

    form.addEventListener('submit', async e => {
        e.preventDefault();

        const submitButton = document.getElementById('#loginSubmit');

        $(submitButton).attr('disabled', 'disabled');

        const userName = $('#loginForm input[name=userName]').val();

        const password = $('#loginForm input[name=password]').val();

        if (!userName || !password) {

            notifyDataRequired('Username and password is required');
            $(submitButton).removeAttr('disabled');

        } else {

            const successful = await login(userName, password, returnUrl);
            $(submitButton).removeAttr('disabled');

            if (successful) {
                if (returnUrl) {
                    window.location.href = returnUrl;
                } else {
                    window.location.href = appConfig.appBaseUrl;
                }
            }
        }
    });
}

async function handleRegisterForm() {

    let form = document.getElementById('registerForm');

    form.addEventListener('submit', async e => {

        e.preventDefault();

        let submitButton = $('#registerSubmit');

        submitButton.attr('disabled', 'disabled');

        const url = '/users/register';

        const userName = $('#registerForm input[name=userName]').val();

        const password = $('#registerForm input[name=password]').val();

        const cpassword = $('#registerForm input[name=cpassword]').val();

        const roles = getSelectFieldValues($('#registerForm select[name="roles[]"]'));


        if (!userName || !password) {

            notifyDataRequired('Kindly complete the form correctly.');
            submitButton.removeAttr('disabled');
            //form.reset();

        } else {

            if (cpassword != password) {

                notifyValueNotEqual("Password does not match!");

            } else {
                const registrationInfo = {

                    userName, password, roles
                }

                await register(registrationInfo, url).then((result) => {

                    submitButton.removeAttr('disabled');
                    form.reset();
                });
            }

            submitButton.removeAttr('disabled');
            form.reset();
        }

    });
}



function getSelectedValues(elementId) {
    var val = $(`#${elementId}`).val();
    if (val) {
        return val.split(',');
    }
    return [];
}


