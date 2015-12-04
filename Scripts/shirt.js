$(document).ready(function () {
    $("#owlCarousel").owlCarousel({
        items: 1,
        paginationNumbers: true
    });

    $("#size-selector").change(function () {
        var $buybutton = $("#buybutton");

        $buybutton.data("item-id", $(this).val());
    });
});