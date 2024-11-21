document.addEventListener("DOMContentLoaded", () => {
  const form = document.querySelector("#contactForm");
  const nameInput = document.querySelector("#name");
  const emailInput = document.querySelector("#email");
  const subjectInput = document.querySelector("#subject");
  const messageInput = document.querySelector("#message");
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

  // Validation functions
  function validateField(input, fieldName) {
      const value = input.value.trim();
      if (!value) {
          showError(input, `${fieldName} is required.`);
          return false;
      } else {
          hideError(input);
          return true;
      }
  }

  // Add event listeners to trigger validations
  nameInput.addEventListener("blur", () => validateField(nameInput, "Name"));
  emailInput.addEventListener("blur", () => validateField(emailInput, "Email"));
  subjectInput.addEventListener("blur", () => validateField(subjectInput, "Subject"));
  messageInput.addEventListener("blur", () => validateField(messageInput, "Message"));

  // Enable submit button only when all validations pass
  form.addEventListener("input", () => {
      const isNameValid = validateField(nameInput, "Name");
      const isEmailValid = validateField(emailInput, "Email");
      const isSubjectValid = validateField(subjectInput, "Subject");
      const isMessageValid = validateField(messageInput, "Message");

      submitButton.disabled = !(isNameValid && isEmailValid && isSubjectValid && isMessageValid);
  });

  // Prevent form submission if there are validation errors
  form.addEventListener("submit", (event) => {
      if (submitButton.disabled) {
          event.preventDefault();
      }
  });
});