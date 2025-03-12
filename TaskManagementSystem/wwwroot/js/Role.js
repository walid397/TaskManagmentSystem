document.addEventListener("DOMContentLoaded", function () {
    document.getElementById("saveRoleBtn").addEventListener("click", function () {

        const roleData = {
            id: $("#roleId").val(),
            name: $("#roleName").val().trim(),
            isActive: $("#isActive").is(":checked"),
            concurrencyStamp: $("#ConcurrencyStamp").val()
        };

        fetch("/Role/SaveUpdate", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(roleData)
        })
            .then(response => {
                if (!response.ok) {
                    return response.json().then(err => {
                        let errors = err.errors.map(e => `${e}<br>`).join("");
                        $("#nameError").html(errors);
                        console.log(response);
                        throw new Error(errors);
                    });
                }
                return response.json();
            })
            .then(data => {
                if (data.success) {
                    showMessage("success", data.message);
                    setTimeout(() => {
                        window.location.href = "/Role/GetRoles";
                    }, 1500);
                } else {
                    showMessage("error", data.message);
                }
            })
            .catch(err => {
                showMessage("error", err.message);
            });
    });

    function showMessage(type, message) {
        Swal.fire({
            icon: type,
            title: message,
            showConfirmButton: false,
            timer: 3000
        });
    }
});
