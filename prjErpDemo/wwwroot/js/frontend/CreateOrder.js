$(document).ready(function () {
    // prevent enter keydown to submit form
    $('form').on('keydown', 'input', function (event) {
        if (event.key === "Enter") {
            event.preventDefault();
        }
    });

    // prevent submit empty order
    $("form").submit(function (event) {
        var productRows = $(".product-purchased tr");
        if (productRows.length === 0) {
            alert("You must add at least one product to the order.");
            event.preventDefault(); 
        }
    });

    var addedProducts = {};
    $(".add-product").click(function () {
        var productID = $(this).data("productid");
        var stockTd = $(this).closest("tr").find(".stock-td");
        var stockQuantity = parseInt(stockTd.text());

        // check if there is enough stock
        if (stockQuantity === 0) {
            alert("No stock for this product");
            return;
        }

        // check if product is already on order list
        if (addedProducts[productID]) {
            alert("You have already added the product, set quantity on the order list");
            return;
        }

        stockQuantity -= 1;
        stockTd.text(stockQuantity);

        var productRow = $(this).closest("tr");
        var productName = productRow.find("td:first").text();
        var price = parseFloat(productRow.find("td:eq(1)").text().replace('$', '')); // 價格轉換為浮點數

        // create a new tr element
        var newRow =
            `<tr class="align-middle">
            <td>${productName}</td>
            <td class="unit-price">$${price.toFixed(2)}</td>
            <td>
                <input type="hidden" class="productID" value="${productID}">
                <input type="number" class="quantityPurchased" min="1" max="50" value="1">
            </td>
            <td class="total-amount">$${price.toFixed(2)}</td>
            <td>
                <button type="button" class="btn btn-secondary btn-sm remove-product">X</button>
            </td>
        </tr>`;

        // set addedProducts with productID to avoid adding same product
        addedProducts[productID] = true;

        // add new tr
        $(".product-purchased").append(newRow);

        updateTotal();
        updateInputNamesForVM();
    });

    $(".product-purchased").on("click", ".remove-product", function () {
        var productID = $(this).closest("tr").find(".productID").val();
        var stockTd = $(`#${productID}`).find(".stock-td");
        var stockQuantity = parseInt(stockTd.text());
        var quantityPurchased = parseInt($(this).closest("tr").find(".quantityPurchased").val());

        delete addedProducts[productID];
        $(this).closest("tr").remove();

        stockQuantity += quantityPurchased;
        stockTd.text(stockQuantity);

        updateTotal();
        updateInputNamesForVM();
    });

    $(".product-purchased").on("change", ".quantityPurchased", function () {
        var inputElement = $(this);
        var quantityPurchased = inputElement.val();
        var productID = inputElement.closest("tr").find(".productID").val();

        // use data() to save quantityChanges
        var quantityChanges = inputElement.data('quantityChangeStore') || {};
        var previousQuantity = quantityChanges[productID] || 1;
        var change = quantityPurchased - previousQuantity;

        if (quantityPurchased <= 0) {
            alert("Please set valid quantity");
            inputElement.val(previousQuantity);
            return;
        }

        var stockTd = $(`#${productID}`).find(".stock-td");
        var stockQuantity = parseInt(stockTd.text());

        if (change > stockQuantity) {
            alert("Cannot purchase more than available stock");
            inputElement.val(previousQuantity);
            return;
        } else if (quantityPurchased > 50) {
            alert("Cannot purchase same product more than 50 units");
            inputElement.val(previousQuantity);
            return;
        }

        quantityChanges[productID] = quantityPurchased;
        $(this).data('quantityChangeStore', quantityChanges);

        stockQuantity -= change;
        stockTd.text(stockQuantity);

        // set total amount
        var unitPrice = parseFloat($(this).closest("tr").find(".unit-price").text().replace('$', ''));
        var totalAmount = (quantityPurchased * unitPrice).toFixed(2);
        $(this).closest("tr").find(".total-amount").text('$' + totalAmount);

        updateTotal();
    });

    function updateTotal() {
        var totalAmounts = $(".total-amount");
        var total = 0;

        totalAmounts.each(function () {
            var amountText = $(this).text();
            var amountValue = parseFloat(amountText.replace('$', ''));

            if (!isNaN(amountValue)) {
                total += amountValue;
            }
        });

        $(".total").text('Total Amount: $' + total.toFixed(2));
    }

    function updateInputNamesForVM() {
        $(".product-purchased tr").each(function (index) {
            var productIDInput = $(this).find(".productID");
            var quantityInput = $(this).find(".quantityPurchased");

            productIDInput.attr("name", `productsSelected[${index}].productID`);
            quantityInput.attr("name", `productsSelected[${index}].quantityPurchased`);
        });
    }
});