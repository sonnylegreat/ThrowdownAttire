$(document).ready(function () {
    $("body").css("background", "url(../../Content/background.jpg) 0 fixed");

    $("#size-selector").change(function () {
        var $buybutton = $("#buybutton");
        var title = $buybutton.data("item-name").split("-");

        var urlList = $buybutton.data("item-url").split('/');
        urlList.pop();
        var url = urlList.join('/');

        $buybutton.data("item-id", $(this).val());
        $buybutton.data("item-name", title[0] + "- " + $("#size-selector :selected").text().trim());
        $buybutton.data("item-url", url + "/" + $(this).val());

        $buybutton.attr("data-item-id", $(this).val());
        $buybutton.attr("data-item-name", title[0] + "- " + $("#size-selector :selected").text().trim());
        $buybutton.attr("data-item-url", url + "/" + $(this).val());
    });

    $(".modal-trigger").click(function () {
        var pic = $(this).find("img").first().attr("src");
        $("#modal-pic").attr("src", pic);
    });
});