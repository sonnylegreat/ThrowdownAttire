var currentType = null;
var $seriesClone;
var $shirtModals;

function backToNormal() {
    $("#series").remove();
    $seriesClone.insertAfter("#navbar");
    $seriesClone = $seriesClone.clone();

    $(".shirtContainer").each(function(index, element){
        $(element).slideDown(1000);
    });

    createListeners();
}

function show($type) {
    if (currentType === $type) {
        return;
    }
    currentType = $type;
    var $shirtContainer = $(".shirtContainer");
    if ($type === "all") {
        backToNormal();
    }
    else {
        var $lastShirt = null; // The last successfully matched shirt.
        var siblingCount = 0;

        $shirtContainer.each(function (index, element) {
            var $element = $(element);

            if ($element.find(".shirtModal").data("type") != $type) {
                $element.slideUp("slow");
                $element.css("position", "absolute");
            }
            else {
                $element.css("position", "relative");
                $element.slideDown("slow");

                var $dest;

                if ($lastShirt != null && siblingCount < 3) {
                    $element.insertAfter($lastShirt);
                    siblingCount += 1;
                }
                else {
                    siblingCount = 0;
                }

                $lastShirt = $element;
            }
        });
    }
}

function createListeners() {
    $(".modal-footer, .modal-header").css("visibility", "hidden");

    $(".shirtModal").mouseenter(function () {
        $(this).find(".modal-footer, .modal-header").css("visibility", "visible");
    }).mouseleave(function (event) {
        $(this).find(".modal-footer, .modal-header").css("visibility", "hidden");
    });
}

$(document).ready(function () {
    $seriesClone = $("#series").clone();

    $("li.series").click(function (event) {
        var $type = $(this).attr("value");
        show($type);
        event.preventDefault();
    });

    createListeners();
});