document.addEventListener("DOMContentLoaded", () => {
  const form = document.querySelector("#registrationForm");
  const firstNameInput = document.querySelector("#firstName");
  const lastNameInput = document.querySelector("#lastName");
  const dobInput = document.querySelector("#dob");
  const emailInput = document.querySelector("#email");
  const phoneInput = document.querySelector("#phone");
  const passwordInput = document.querySelector("#password");
  const confirmPasswordInput = document.querySelector("#confirmPassword");
  const submitButton = form.querySelector("button[type='submit']");

  // Helper functions for showing and hiding errors
  function showError(input, message) {
      let error = input.nextElementSibling;
      if (!error || !error.classList.contains("error-message")) {
          error = document.createElement("span");
          error.classList.add("error-message");
          input.after(error);
      }
      error.textContent = message;
      error.style.color = "red";
      error.style.display = "block";
      input.style.border = "2px solid red";
  }

  function hideError(input) {
      const error = input.nextElementSibling;
      if (error && error.classList.contains("error-message")) {
          error.style.display = "none";
      }
      input.style.border = "1px solid #ccc";
  }

  // Individual Validation functions
  function validateName(input, fieldName) {
      const value = input.value.trim();
      const nameRegex = /^[A-Za-z]+$/;
      if (!value) {
          showError(input, `${fieldName} is required.`);
          return false;
      } else if (!nameRegex.test(value)) {
          showError(input, `${fieldName} must contain only letters.`);
          return false;
      } else {
          hideError(input);
          return true;
      }
  }

  function validateDOB(input) {
      const value = input.value.trim();
      const selectedDate = new Date(value);
      const today = new Date();
      const age = today.getFullYear() - selectedDate.getFullYear();
      const monthDifference = today.getMonth() - selectedDate.getMonth();
      const dayDifference = today.getDate() - selectedDate.getDate();

      const actualAge = (monthDifference > 0 || (monthDifference === 0 && dayDifference >= 0)) ? age : age - 1;

      if (!value) {
          showError(input, "Date of birth is required.");
          return false;
      } else if (selectedDate >= today) {
          showError(input, "Date of birth must be in the past.");
          return false;
      } else if (actualAge < 14) {
          showError(input, "Age must be at least 14 years.");
          return false;
      } else {
          hideError(input);
          return true;
      }
  }

  function validateEmail(input) {
      const value = input.value.trim();
      const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
      if (!value) {
          showError(input, "Email is required.");
          return false;
      } else if (!emailRegex.test(value)) {
          showError(input, "Invalid email format.");
          return false;
      } else {
          hideError(input);
          return true;
      }
  }

  function validatePhone(input) {
      const value = input.value.trim();
      const phoneRegex = /^9\d{9}$/;
      if (!value) {
          showError(input, "Phone number is required.");
          return false;
      } else if (!phoneRegex.test(value)) {
          showError(input, "Phone number must start with 9 and be 10 digits long.");
          return false;
      } else {
          hideError(input);
          return true;
      }
  }

  function validatePassword(input) {
      const value = input.value.trim();
      const passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*(),.?":{}|<>]).{8,}$/;
      if (!value) {
          showError(input, "Password is required.");
          return false;
      } else if (!passwordRegex.test(value)) {
          showError(input, "Password must be at least 8 characters long, contain at least one uppercase letter, one lowercase letter, and one symbol.");
          return false;
      } else {
          hideError(input);
          return true;
      }
  }

  function validateConfirmPassword(input, passwordInput) {
      const value = input.value.trim();
      if (value !== passwordInput.value.trim()) {
          showError(input, "Passwords do not match.");
          return false;
      } else {
          hideError(input);
          return true;
      }
  }

  // Add event listeners to trigger validations only on specific inputs
  firstNameInput.addEventListener("blur", () => validateName(firstNameInput, "First name"));
  lastNameInput.addEventListener("blur", () => validateName(lastNameInput, "Last name"));
  dobInput.addEventListener("blur", () => validateDOB(dobInput));
  emailInput.addEventListener("blur", () => validateEmail(emailInput));
  phoneInput.addEventListener("blur", () => validatePhone(phoneInput));
  passwordInput.addEventListener("blur", () => validatePassword(passwordInput));
  confirmPasswordInput.addEventListener("blur", () => validateConfirmPassword(confirmPasswordInput, passwordInput));

  // Enable submit button only when all validations pass
  form.addEventListener("input", () => {
      const isFirstNameValid = validateName(firstNameInput, "First name");
      const isLastNameValid = validateName(lastNameInput, "Last name");
      const isDOBValid = validateDOB(dobInput);
      const isEmailValid = validateEmail(emailInput);
      const isPhoneValid = validatePhone(phoneInput);
      const isPasswordValid = validatePassword(passwordInput);
      const isConfirmPasswordValid = validateConfirmPassword(confirmPasswordInput, passwordInput);

      submitButton.disabled = !(isFirstNameValid && isLastNameValid && isDOBValid && isEmailValid && isPhoneValid && isPasswordValid && isConfirmPasswordValid);
  });

  // Prevent form submission if there are validation errors
  form.addEventListener("submit", (event) => {
      if (submitButton.disabled) {
          event.preventDefault();
      }
  });
});
