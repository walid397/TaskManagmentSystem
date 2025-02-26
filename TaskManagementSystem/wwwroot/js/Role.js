document.addEventListener("DOMContentLoaded", function () {
    document.getElementById("saveRoleBtn").addEventListener("click", function () {
        const roleId = document.getElementById("roleId").value;
        const roleName = document.getElementById("roleName").value.trim();
        const isActive = document.getElementById("isActive").checked;

        // التحقق من إدخال الاسم
        if (roleName === "") {
            showMessage("danger", "Role name cannot be empty.");
            return;
        }

        const roleData = {
            id: roleId,
            name: roleName,
            isActive: isActive
        };

        fetch("/Role/SaveUpdate", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(roleData)
        })
            .then(response => response.json())
            .then(data => {
                console.log(data); // ✅ تحقق مما يتم إرجاعه من السيرفر
                if (data.success) {
                    showMessage("success", data.message);
                    window.location.href = "/Role/GetRoles"; // 🔄 التنقل مباشرة بدون تأخير
                } else {
                    showMessage("success", data.message);
                }
            })

            .catch(() => {
                showMessage("danger", "Error updating role.");
            });
    });

    function showMessage(type, message) {
        document.getElementById("messageContainer").innerHTML = `
            <div class="alert alert-${type} alert-dismissible fade show" role="alert">
                ${message}
            </div>
        `;
    }
});
