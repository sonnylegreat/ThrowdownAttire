$(document).ready(function () {
    $("#owlCarousel").owlCarousel({
        items: 1,
        paginationNumbers: true
    });

    $("#size-selector").change(function () {
        var $buybutton = $("#buybutton");
        var title = $buybutton.data("item-name").split("-");

        $buybutton.data("item-id", $(this).val());
        $buybutton.data("item-name", title[0] + "-" + $("#size-selector :selected").text());
    });
});