$(document).ready(function () {

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
})