$(document).ready(function () {
    $("#admin-table").tablesorter();

    $(".delete").click(function () {
        $.post("/Admin/DeleteShirt", { "id": $(this).data("id") });
        $(this).parents("tr").remove();
    });
});