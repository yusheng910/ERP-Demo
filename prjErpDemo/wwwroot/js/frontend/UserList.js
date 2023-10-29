$(document).ready(function () {

    // delete customer
    $(".table").on("click", ".anchor-delete-user", function (event) {
        event.preventDefault();
        var $this = $(this);
        var userId = $this.data("id");

        var confirmDelete = confirm("Are you sure you want to delete this user?");
        if (confirmDelete) {
            $.ajax({
                url: "/api/UserAPI/" + userId,
                type: "DELETE",
                success: function (response) {
                    if (response.status === "User deleted") {
                        alert("User deleted successfully.");
                        $this.closest("tr").remove();
                    } else {
                        alert("Please reload and check if the user was deleted.");
                    }
                },
                error: function (xhr) {
                    var errorObj = JSON.parse(xhr.responseText);
                    alert("Error: " + errorObj.error);
                }
            });
        }
    });
});
