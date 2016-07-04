$(document).ready(function () {
    $(".shirtModal").mouseenter(function () {
        $(this).find(".modal-footer, .modal-header").css({ opacity: 0, visibility: "visible" }).animate({ opacity: 1 }, 400);
    }).mouseleave(function (event) {
        $(this).find(".modal-footer, .modal-header").css({ opacity: 1 }).animate({ opacity: 0 }, 400);
    });
});