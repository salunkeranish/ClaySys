// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//future dates disabled
function futuredate() {
    var todayDate = new Date();
    var month = todayDate.getMonth() + 1;
    var year = todayDate.getFullYear();
    var tdate = todayDate.getDate();
    if (month < 10) {
        month = "10" + month
    }
    if (tdate < 10) {
        tdate = "0" + tdate;
    }
    var maxDate = year + "-" + month + "-" + tdate;
    document.getElementById("dob").setAttribute("max", maxDate);
}


//past date disabled for events
function pastdate() {
    var date = new Date();
    var tdate = date.getDate();
    var month = date.getMonth() + 1;


    if (tdate < 10) {
        tdate = '0' + tdate;
    }
    if (month < 10) {
        month = '0' + month;
    }
    var year = date.getUTCFullYear();
    var minDate = year + "-" + month + "-" + tdate;
    document.getElementById("startDate").setAttribute('min', minDate);

}
//register form validation

document.addEventListener('DOMContentLoaded', function () {
    const dob = document.getElementById('dob');
    const phone = document.getElementById('phone');
    const dobError = document.getElementById('dob-error');
    const phoneError = document.getElementById('phone-error');
    const password = document.getElementById('password');
    const passwordError = document.getElementById('password-error');
    dob.addEventListener('input', validateDob);
    phone.addEventListener('input', validatePhone);
    password.addEventListener('input', validatePassword);

    function validateDob() {
        dobError.textContent = '';

        const today = new Date();
        const birthDate = new Date(dob.value);
        let age = today.getFullYear() - birthDate.getFullYear();
        const m = today.getMonth() - birthDate.getMonth();
        if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
            age--;
        }
        if (age < 18) {
            dobError.textContent = "You must be at least 18 years old.";
        }
    }

    function validatePhone() {
        phoneError.textContent = '';

        const phonePattern = /^[6-9]\d{9}$/;
        if (!phonePattern.test(phone.value)) {
            phoneError.textContent = "Phone number must start with 6, 7, 8, or 9 and be exactly 10 digits long.";
        }
    }
    function validatePassword() {
        passwordError.textContent = '';
        if (password.value.length < 8) {
            passwordError.textContent = "Password must be at least 8 characters long.";
        }
    }
});

//login form
document.addEventListener('DOMContentLoaded', function () {
    const username = document.getElementById('username');
    const password = document.getElementById('password');
    const usernameError = document.getElementById('username-error');
    const passwordError = document.getElementById('password-error');

    username.addEventListener('input', validateUsername);
    password.addEventListener('input', validatePassword);

    function validateUsername() {
        usernameError.textContent = '';

        if (username.value.trim() === '') {
            usernameError.textContent = "Username is required.";
        }
    }

    function validatePassword() {
        passwordError.textContent = '';

        if (password.value.trim() === '') {
            passwordError.textContent = "Password is required";
        }
    }
});




