﻿$(document).ready(function () {
    var $imgText = $("#img-text");

    $imgText.mouseenter(function () {
        $("#img-msg").fadeIn(200);
        $("#series-carousel .carousel-inner div.item img").css(setGreyScale());
    }).mouseleave(function () {
        $("#img-msg").fadeOut(200);
    });

    $(".carousel").mouseenter(function () {
        var $this = $(this);
        $this.carousel("pause");
        var text = $($this.find(".active").children().first()).data("text");
        if (text == "Quote") {
            text = "Quotes";
        }
        $imgText.text(text);
        $imgText.fadeIn(200);

        $("#series-carousel .carousel-inner div.item img").css(setGreyScale());
    }).mouseleave(function () {
        $(this).carousel("cycle");
        $imgText.fadeOut(200);
        $("#series-carousel .carousel-inner div.item img").css(clearGreyScale());
    }).on("slid.bs.carousel", function () {
        var current = $($(".carousel").find(".active").first()).data("text");
        $(".img-link").first().attr("href", "/Series/" + current.replace(new RegExp(" ", "g"), ''));
        if (current == "Quote") {
            current = "Quotes";
        }
        $imgText.text(current);
    });
});

function setGreyScale(){
    return {
        "filter": "gray", /* IE5+ */
        "-webkit-filter": "grayscale(1)", /* Webkit Nightlies & Chrome Canary */
        "-webkit-transform": "scale(1.01)"
    };
}
function clearGreyScale() {
    return {
        "filter": "none",
        "-webkit-filter": "grayscale(0)",
        "-webkit-transition": "all .5s ease-in-out"
    };
}