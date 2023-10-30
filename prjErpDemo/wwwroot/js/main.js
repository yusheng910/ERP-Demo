(function ($) {
    "use strict";

    $(".date").each(function (index, element) {
        var dateStringStored = $(element).text().trim();

        if (dateStringStored === "") {
            $(element).text('N/A');

        } else {
            var dateStored = new Date(dateStringStored);

            const options = { year: 'numeric', month: '2-digit', day: '2-digit', hour: '2-digit', minute: '2-digit', second: '2-digit' };
            var dateShown = dateStored.toLocaleDateString('zh-TW', options).replace(/\//g, '-');
            $(element).text(`${dateShown}`);
        }
    });

    // DoughnutChart
    const categories = [];
    const totalSales = [];

    $.ajax({
        url: "/api/ChartAPI/MonthlySales",
        type: "GET",
        success: function (response) {
            response.forEach(item => {
                categories.push(item.category);
                totalSales.push(parseInt(item.totalSale));
            });

            createDoughnutChart(categories, totalSales);
        },
        error: function (xhr) {
            var errorObj = JSON.parse(xhr.responseText);
            alert("Error: " + errorObj.error);
        }
    });

    function createDoughnutChart(categories, totalSales) {
        var ctx6 = $("#doughnut-chart").get(0).getContext("2d");
        var myChart6 = new Chart(ctx6, {
            type: "doughnut",
            data: {
                labels: categories,
                datasets: [{
                    backgroundColor: [
                        "rgba(92, 93, 192, .5)",
                        "rgba(255, 239, 95, .5)",
                        "rgba(60, 179, 113, .5)",
                        "rgba(255, 114, 118, .5)",
                        "rgba(228, 190, 239, .5)"
                    ],
                    data: totalSales
                }]
            },
            options: {
                responsive: true
            }
        });
    }

})(jQuery);

