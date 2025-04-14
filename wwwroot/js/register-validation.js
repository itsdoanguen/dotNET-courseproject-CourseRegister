document.addEventListener("DOMContentLoaded", function () {
    const passwordInput = document.getElementById("Password");
    const confirmInput = document.getElementById("ConfirmPassword");
    const strengthBar = document.getElementById("strengthBar");
    const requirementList = document.getElementById("requirementList");
    const confirmMessage = document.getElementById("confirmMessage");
    const form = document.querySelector("form");

    const requirements = [
        { test: pw => pw.length >= 6 && pw.length <= 18, text: "Độ dài từ 6 đến 18 ký tự" },
        { test: pw => /[a-z]/.test(pw), text: "Ít nhất một chữ thường" },
        { test: pw => /[A-Z]/.test(pw), text: "Ít nhất một chữ hoa" },
        { test: pw => /\d/.test(pw), text: "Ít nhất một số" },
        { test: pw => /[^\w\s]/.test(pw), text: "Ít nhất một ký tự đặc biệt" },
        { test: pw => !/\s/.test(pw), text: "Không chứa khoảng trắng" }
    ];

    function updatePasswordFeedback() {
        const password = passwordInput.value;
        let strength = 0;

        requirementList.innerHTML = "";

        requirements.forEach(req => {
            if (req.test(password)) {
                strength++;
            } else {
                const li = document.createElement("li");
                li.textContent = req.text;
                requirementList.appendChild(li);
            }
        });

        // Update strength bar
        strengthBar.value = strength;
        strengthBar.className = "";
        if (strength <= 2) {
            strengthBar.classList.add('weak');
            strengthBar.classList.remove('medium', 'strong');
        } else if (strength <= 4) {
            strengthBar.classList.add('medium');
            strengthBar.classList.remove('weak', 'strong');
        } else {
            strengthBar.classList.add('strong');
            strengthBar.classList.remove('weak', 'medium');
        }
    }

    function validateConfirmPassword() {
        confirmMessage.classList.remove("d-none"); 

        if (confirmInput.value !== passwordInput.value) {
            confirmMessage.textContent = "Mật khẩu xác nhận không khớp!";
            confirmMessage.classList.add("text-danger");
            confirmMessage.classList.remove("text-success");
            return false;
        } else {
            confirmMessage.textContent = "Xác nhận mật khẩu hợp lệ ✔️";
            confirmMessage.classList.remove("text-danger");
            confirmMessage.classList.add("text-success");
            return true;
        }
    }


    passwordInput.addEventListener("input", () => {
        updatePasswordFeedback();
        validateConfirmPassword();
    });
    confirmInput.addEventListener("input", validateConfirmPassword);

    form.addEventListener("submit", function (e) {
        const remaining = requirementList.querySelectorAll("li");
        if (remaining.length > 0 || !validateConfirmPassword()) {
            e.preventDefault();
        }
    });
});
