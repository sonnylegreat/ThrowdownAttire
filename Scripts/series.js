var currentType = null;

function animateAppend($source, $target) {
    var $dest = $source.clone().appendTo($target);
    var destOffset = $dest.offset();

    var sourceOffset = $source.offset();

    var $temp = $source.clone().appendTo("body");

    $temp.css("position", "absolute")
    .css("left", sourceOffset.left)
    .css("top", sourceOffset.top)
    .css("z-index", 1000);

    $dest.hide();
    $source.hide();

    $temp.animate({
        "top": destOffset.top,
        "left": destOffset.left
    },
    "slow",
    function () {
        $dest.show();
        $source.remove();
        $temp.remove();
    });
}

function backToNormal($shirtContainer) {
    $shirtContainer.css("position", "relative");
    $shirtContainer.show();

    stealthBackToNormal($shirtContainer);
}

function stealthBackToNormal($shirtContainer) {
    $shirtContainer.parent().each(function (index, element) {

        var $element = $(element);
        var length = $element.children().length;

        if (length > 4) {
            for (i = length; i > 4; i--) {
                $($element.children().last()).
                prependTo($element.next())
            }
        }
    });
}

function show($type) {
    if (currentType === $type) {
        return;
    }
    currentType = $type;
    var $shirtContainer = $(".shirtContainer");
    if ($type == "all") {
        backToNormal($shirtContainer);
    }
    else {
        stealthBackToNormal($shirtContainer);

        var $currentParent = $shirtContainer.first().parent(); // the current row to be filled.
        var emptyCount = 0; // The number of empty spaces in the current row.

        $shirtContainer.each(function (index, element) {
            var $element = $(element);

            if ($element.find(".shirtModal").data("type") != $type) {
                $element.hide();
                $element.css("position", "absolute");

                if (index % 4 == 0) {
                    emptyCount = 0;
                    $currentParent = $element.parent();
                }
                emptyCount += 1;
            }
            else {
                $element.css("position", "relative");
                $element.show();

                if ($currentParent[0] == $element.parent()[0]) {
                    return;
                }
                if (emptyCount > 0) {
                    animateAppend($element, $currentParent);
                    emptyCount -= 1;
                }
            }
        });
    }
}
$(document).ready(function () {
    $("li.series").click(function (event) {
        var $type = $(this).attr("value");
        show($type);
        event.preventDefault();
    });
});