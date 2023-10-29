$(document).ready(function () {

    getData();

    $("#searchButton").click(function () {
        getData(); 
    });

    function getData()
    {
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
                                <a href="#" data-ID='${product.productID}'>Edit</a> |
                                <a href="#" data-ID='${product.productID}'>Delete</a>
                            </td>
                        </tr>
                        `
                    );
                });
            }
        });
    }
});
