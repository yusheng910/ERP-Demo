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

    // Spinner
    var spinner = function () {
        setTimeout(function () {
            if ($('#spinner').length > 0) {
                $('#spinner').removeClass('show');
            }
        }, 1);
    };
    spinner();


    // Back to top button
    $(window).scroll(function () {
        if ($(this).scrollTop() > 300) {
            $('.back-to-top').fadeIn('slow');
        } else {
            $('.back-to-top').fadeOut('slow');
        }
    });
    $('.back-to-top').click(function () {
        $('html, body').animate({ scrollTop: 0 }, 1500, 'easeInOutExpo');
        return false;
    });


    // Sidebar Toggler
    $('.sidebar-toggler').click(function () {
        $('.sidebar, .content').toggleClass("open");
        return false;
    });


    // Progress Bar
    $('.pg-bar').waypoint(function () {
        $('.progress .progress-bar').each(function () {
            $(this).css("width", $(this).attr("aria-valuenow") + '%');
        });
    }, { offset: '80%' });


    // Calender
    $('#calender').datetimepicker({
        inline: true,
        format: 'L'
    });


    // Testimonials carousel
    $(".testimonial-carousel").owlCarousel({
        autoplay: true,
        smartSpeed: 1000,
        items: 1,
        dots: true,
        loop: true,
        nav: false
    });


    // Worldwide Sales Chart
    var ctx1 = $("#worldwide-sales").get(0).getContext("2d");
    var myChart1 = new Chart(ctx1, {
        type: "bar",
        data: {
            labels: ["2016", "2017", "2018", "2019", "2020", "2021", "2022"],
            datasets: [{
                label: "USA",
                data: [15, 30, 55, 65, 60, 80, 95],
                backgroundColor: "rgba(0, 156, 255, .7)"
            },
            {
                label: "UK",
                data: [8, 35, 40, 60, 70, 55, 75],
                backgroundColor: "rgba(0, 156, 255, .5)"
            },
            {
                label: "AU",
                data: [12, 25, 45, 55, 65, 70, 60],
                backgroundColor: "rgba(0, 156, 255, .3)"
            }
            ]
        },
        options: {
            responsive: true
        }
    });


    // Sales & Revenue Chart
    var result = {};

    $.ajax({
        url: "/api/ChartAPI/YearlySales",
        type: "GET",
        success: function (response) {
            response.forEach(item => {
                var month = item.month;
                var categorySales = item.categorySales;

                for (const category in categorySales) {
                    if (!result[category]) {
                        result[category] = Array(12).fill(0);
                    }
                    result[category][month - 1] = categorySales[category];
                }
            });
            
            console.log(result);
            console.log(result.CPU);
            createLineChart(result);
        },
        error: function (xhr) {
            var errorObj = JSON.parse(xhr.responseText);
            alert("Error: " + errorObj.error);
        }
    });

    function createLineChart(data) {
        var ctx2 = $("#salse-revenue").get(0).getContext("2d");
        var myChart2 = new Chart(ctx2, {
            type: "line",
            data: {
                labels: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
                datasets: [{
                    label: "CPU",
                    data: data.CPU,
                    backgroundColor: "rgba(92, 93, 192, .5)",
                    fill: true
                },
                {
                    label: "GPU",
                    data: data.GPU,
                    backgroundColor: "rgba(255, 239, 95, .5)",
                    fill: true
                }, {
                    label: "Motherboards",
                    data: data.Motherboards,
                    backgroundColor: "rgba(60, 179, 113, .5)",
                    fill: true
                }, {
                    label: "RAM",
                    data: data.RAM,
                    backgroundColor: "rgba(255, 114, 118, .5)",
                    fill: true
                }, {
                    label: "Storage",
                    data: data.Storage,
                    backgroundColor: "rgba(228, 190, 239, .5)",
                    fill: true
                }]
            },
            options: {
                responsive: true
            }
        });
    }

    // Single Line Chart
    var ctx3 = $("#line-chart").get(0).getContext("2d");
    var myChart3 = new Chart(ctx3, {
        type: "line",
        data: {
            labels: [50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150],
            datasets: [{
                label: "Salse",
                fill: false,
                backgroundColor: "rgba(0, 156, 255, .3)",
                data: [7, 8, 8, 9, 9, 9, 10, 11, 14, 14, 15]
            }]
        },
        options: {
            responsive: true
        }
    });


    // Single Bar Chart
    var ctx4 = $("#bar-chart").get(0).getContext("2d");
    var myChart4 = new Chart(ctx4, {
        type: "bar",
        data: {
            labels: ["Italy", "France", "Spain", "USA", "Argentina"],
            datasets: [{
                backgroundColor: [
                    "rgba(0, 156, 255, .7)",
                    "rgba(0, 156, 255, .6)",
                    "rgba(0, 156, 255, .5)",
                    "rgba(0, 156, 255, .4)",
                    "rgba(0, 156, 255, .3)"
                ],
                data: [55, 49, 44, 24, 15]
            }]
        },
        options: {
            responsive: true
        }
    });


    // Pie Chart
    var ctx5 = $("#pie-chart").get(0).getContext("2d");
    var myChart5 = new Chart(ctx5, {
        type: "pie",
        data: {
            labels: ["Italy", "France", "Spain", "USA", "Argentina"],
            datasets: [{
                backgroundColor: [
                    "rgba(0, 156, 255, .7)",
                    "rgba(0, 156, 255, .6)",
                    "rgba(0, 156, 255, .5)",
                    "rgba(0, 156, 255, .4)",
                    "rgba(0, 156, 255, .3)"
                ],
                data: [55, 49, 44, 24, 15]
            }]
        },
        options: {
            responsive: true
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

