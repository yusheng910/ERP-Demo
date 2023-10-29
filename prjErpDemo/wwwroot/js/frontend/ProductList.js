$(document).ready(function () {

    $("#searchButton").click(function () {
        getData();
    });

    function getData() {
        var keyword = $("#searchInput").val();
        $.ajax({
            url: "/api/productapi",
            data: { keyword: keyword },
            dataType: "json",
            success: function (data) {
                // clear table
                $("table tbody").empty();
                $.each(data, function (index, product) {
                    $("table tbody").append(
                        `            
                        <tr>
                            <td>
                                ${product.productName}
                            </td>
                            <td>
                                ${product.price}
                            </td>
                            <td>
                                ${product.stock}
                            </td>
                            <td>
                                ${product.category}
                            </td>
                            <td>
                                ${product.supplier}
                            </td>
                            <td>
                                <a href="/product/updateproduct/${product.productID}">Edit</a> |
                                <a class="anchor-delete-product" data-id="${product.productID}" href="/product/updateproduct/${product.productID}">Delete</a>
                            </td>
                        </tr>
                        `
                    );
                });
            }
        });
    }


    // delete product
    $(".table").on("click", ".anchor-delete-product", function (event) {
        event.preventDefault();
        var $this = $(this);
        var productId = $this.data("id");

        var confirmDelete = confirm("Are you sure you want to delete this product?");
        if (confirmDelete) {
            $.ajax({
                url: "/api/ProductAPI/" + productId,
                type: "DELETE",
                success: function (response) {
                    if (response.status === "Product deleted") {
                        alert("Product deleted successfully.");
                        $this.closest("tr").remove(); 
                    } else {
                        alert("Please reload and check if the product was deleted.");
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
