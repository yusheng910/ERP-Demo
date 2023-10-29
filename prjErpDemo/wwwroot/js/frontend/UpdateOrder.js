$(document).ready(function () {

    let orderDateOriginalForm = $(".order-date").val();
    let shippedDateOriginalForm = $(".shipped-date").val();

    const options = { year: 'numeric', month: '2-digit', day: '2-digit', hour: '2-digit', minute: '2-digit', second: '2-digit' };

    let orderDateFormat = new Date(orderDateOriginalForm);
    let orderDateShown = orderDateFormat.toLocaleDateString('zh-TW', options).replace(/\//g, '-');
    $("#order-date").text("Order date: " + orderDateShown)

    if (shippedDateOriginalForm !== '') {
        let shippedDateDormat = new Date(shippedDateOriginalForm);
        let shippedDateShown = shippedDateDormat.toLocaleDateString('zh-TW', options).replace(/\//g, '-');
        $("#shipped-date").text("Shipped date: " + shippedDateShown)
    } else {
        $("#shipped-date").text("Shipped date: N/A");
    }

    // check order status before save to db
    $("#confirm-change").click(function (event) {
        // get current order status
        var currentStatus = $("#current-order-status").val();
        // get selected order status
        var newStatus = $("select[name='orderStatusID']").val();

        if ((currentStatus === "1" && newStatus === "4") || (currentStatus === "2" && newStatus === "4")) {
            // confirm the "Cancel Order" action
            if (!confirm("This action (cancel order) cannot be undone, do you want to proceed?")) {
                event.preventDefault();
            }
        } else if (currentStatus === "2" && newStatus === "3") {
            // confirm the "Complete Order" action
            if (!confirm("This action (complete order) cannot be undone, do you want to proceed?")) {
                event.preventDefault();
            }
        } else if ((currentStatus === "1" && newStatus === "3") || currentStatus === "3" || currentStatus === "4") {
            // alert for other cases
            event.preventDefault();
            if (currentStatus === "1" && newStatus === "3") {
                alert("Order should be shipped before completion.");
            } else {
                alert("The order is already " + (currentStatus === "3" ? "completed" : "canceled"));
            }
        }
    });
})