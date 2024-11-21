document.addEventListener("DOMContentLoaded", () => {
  const loginForm = document.querySelector("#loginForm");
  const emailInput = document.querySelector("#email");
  const passwordInput = document.querySelector("#password");
  const loginButton = loginForm.querySelector("button[type='submit']");

  // Helper functions for showing and hiding errors
  function showError(input, message) {
      const error = input.nextElementSibling;
      error.textContent = message;
      error.style.display = "block";
      error.style.color = "red";
      input.style.border = "2px solid red";
  }

  function hideError(input) {
      const error = input.nextElementSibling;
      error.textContent = "";
      error.style.display = "none";
      input.style.border = "1px solid #ccc"; // Reset border to default
  }

  // Validate individual inputs
  function validateInput(input, message) {
      if (input.value.trim() === "") {
          showError(input, message);
          return false;
      } else {
          hideError(input);
          return true;
      }
  }

  // Function to enable/disable login button
  function toggleButtonState() {
      if (emailInput.value.trim() === "" || passwordInput.value.trim() === "") {
          loginButton.disabled = true;
          loginButton.style.backgroundColor = "#ccc"; // Disabled button style
          loginButton.style.cursor = "not-allowed";
      } else {
          loginButton.disabled = false;
          loginButton.style.backgroundColor = ""; // Reset button style
          loginButton.style.cursor = "pointer";
      }
  }

  // Add event listeners for validation on blur (focus out)
  emailInput.addEventListener("blur", () => validateInput(emailInput, "Email is required."));
  passwordInput.addEventListener("blur", () => validateInput(passwordInput, "Password is required."));

  // Add input event listeners to toggle button state
  emailInput.addEventListener("input", toggleButtonState);
  passwordInput.addEventListener("input", toggleButtonState);

  // Prevent form submission if validation fails
  loginForm.addEventListener("submit", (event) => {
      const isEmailValid = validateInput(emailInput, "Email is required.");
      const isPasswordValid = validateInput(passwordInput, "Password is required.");
      if (!isEmailValid || !isPasswordValid) {
          event.preventDefault(); // Prevent form submission
      }
  });

  // Initialize button state on page load
  toggleButtonState();
});
