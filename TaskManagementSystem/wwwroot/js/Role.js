document.addEventListener("DOMContentLoaded", function () {
    document.getElementById("saveRoleBtn").addEventListener("click", function () {

        const isNew = ($("#roleId").val() === "" || $("#roleId").val() === "0");

        const roleData = {
            id: $("#roleId").val(),
            name: $("#roleName").val().trim(),
            isActive: $("#isActive").is(":checked"),
            concurrencyStamp: $("#ConcurrencyStamp").val()
        };
        const Path = isNew ? "/Role/CreateRole" : "/Role/SaveUpdate";
        console.log("Path:", Path);
        console.log("Role Data Sent:", roleData);

        fetch(Path, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Authorization": "Bearer YOUR_TOKEN_HERE" 
            },
            body: JSON.stringify(roleData)
        })

            .then(async response => {
                const text = await response.text(); 
                const data = text ? JSON.parse(text) : {}; 

                if (!response.ok) {
                    let errors = (data.errors || [data.message || "Something went wrong"]).map(e => `${e}<br>`).join("");
                    $("#nameError").html(errors);
                    throw new Error(errors);
                }

                return data; 
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



//Delete
document.addEventListener("DOMContentLoaded", function () {
    document.querySelectorAll(".delete-role-btn").forEach(button => {
        button.addEventListener("click", function () {
            const id = this.getAttribute("data-id");

            Swal.fire({
                text: 'Are you sure you want to delete this role?',
                icon: 'warning',
                showCancelButton: true,
                confirmButtonText: 'Yes, delete it!',
                cancelButtonText: 'Cancel',
            }).then((result) => {
                if (result.isConfirmed) {
                    deleteRole(id);
                }
            });
        });
    });

    function deleteRole(id) {
        fetch("/Role/Delete?id=" + id, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Authorization": "Bearer YOUR_TOKEN_HERE"
            }
        })
            .then(async response => {
                const text = await response.text();
                const data = text ? JSON.parse(text) : {};

                if (!response.ok) {
                    let errors = (data.errors || [data.message || "Something went wrong"]).map(e => `${e}<br>`).join("");
                    $("#nameError").html(errors);
                    throw new Error(errors);
                }

                return data;
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
    }

    function showMessage(type, message) {
        Swal.fire({
            icon: type,
            title: message,
            showConfirmButton: false,
            timer: 3000
        });
    }
});

//Show Details
document.addEventListener("DOMContentLoaded", function () {
    document.querySelectorAll(".details-role-btn").forEach(button => {
        button.addEventListener("click", function () {
            const id = this.getAttribute("info-id");
            console.log("The id is:", id);

            if (!id) {
                showMessage("error", "Invalid ID");
                return;
            }

            ShowDetails(id);
        });
    });
});

function ShowDetails(id) {
    fetch("/Role/Details?id=" + id, {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            "Authorization": "Bearer YOUR_TOKEN_HERE"
        }
    })
        .then(async response => {
            const text = await response.text();
            const data = text ? JSON.parse(text) : {};


            if (!response.ok) {
                let errors = (data.errors || [data.message || "Something went wrong"]).map(e => `${e}<br>`).join("");
                $("#nameError").html(errors);
                throw new Error(errors);
            }

            return data;
        })
        .then(data => {
            if (data.success) {
                const role = data.data; 

             

                Swal.fire({
                    title: 'Role Details', 
                    html: `
                <p><strong>Name:</strong> ${role.name}</p>
                <p><strong>Status:</strong> ${role.isActive ? 'Active' : 'Inactive'}</p>
            `,
                    icon: 'info', 
                    confirmButtonText: 'Close'
                });

            } else {
                showMessage("error", data.message);
            }
        })
        .catch(err => {
            showMessage("error", err.message);
        });


}

function showMessage(type, message) {
    Swal.fire({
        icon: type,
        title: message,
        showConfirmButton: false,
        timer: 3000
    });
}