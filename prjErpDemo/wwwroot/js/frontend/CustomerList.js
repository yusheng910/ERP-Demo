$(document).ready(function () {

    // delete customer
    $(".table").on("click", ".anchor-delete-customer", function (event) {
        event.preventDefault();
        var $this = $(this);
        var customerId = $this.data("id");

        var confirmDelete = confirm("Are you sure you want to delete this customer?");
        if (confirmDelete) {
            $.ajax({
                url: "/api/CustomerAPI/" + customerId,
                type: "DELETE",
                success: function (response) {
                    if (response.status === "Customer deleted") {
                        alert("Customer deleted successfully.");
                        $this.closest("tr").remove();
                    } else {
                        alert("Please reload and check if the customer was deleted.");
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
