$(document).ready(function () {

    // delete customer
    $(".table").on("click", ".anchor-delete-supplier", function (event) {
        event.preventDefault();
        var $this = $(this);
        var supplierId = $this.data("id");

        var confirmDelete = confirm("Are you sure you want to delete this supplier?");
        if (confirmDelete) {
            $.ajax({
                url: "/api/SupplierAPI/" + supplierId,
                type: "DELETE",
                success: function (response) {
                    if (response.status === "Supplier deleted") {
                        alert("Supplier deleted successfully.");
                        $this.closest("tr").remove();
                    } else {
                        alert("Please reload and check if the supplier was deleted.");
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
