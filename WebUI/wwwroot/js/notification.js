function notifyLoginError(message) {

    toastr.error(message, "Login Error!");
}

function notifyLoginSuccess() {
    
    toastr.success("Login successful!");
}

function notifyRegistrationSuccess(message) {

    toastr.success(message);
}

function notifyDataRequired(message) {
    
    toastr.warning(message, "Details Required!");
}

function notifyUnAuthenticatedError() {
   
    toastr.error("You are not authenticated. kindly log in to continue.", "Login Required!");
}

function notifyUnAuthorizedError() {
   
    toastr.warning("You do not have enough permission to perform this action.", "Access Denied!");
}

function notifyGeneralError(message) {

    toastr.error(message, "Error!");
}



function notifyValueNotEqual(message) {

    toastr.error(message);
}