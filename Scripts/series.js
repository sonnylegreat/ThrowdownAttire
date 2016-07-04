$(document).ready(function () {
    var $imgText = $("#img-text");

    $("#img-text").hide();

    $(".carousel").mouseenter(function () {
        var $this = $(this);

        $this.carousel("pause");
        $imgText.text($($this.find(".active").children().first()).data("text"));
        $imgText.fadeIn(200);
    }).mouseleave(function () {
        $(this).carousel("cycle");
        $imgText.fadeOut(200);
    }).on("slid.bs.carousel", function () {
        $imgText.text($($(".carousel").find(".active").first()).data("text"));
    });
});