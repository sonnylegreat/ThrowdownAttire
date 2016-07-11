$(document).ready(function () {
    $("#admin-table").tablesorter();

    $(".delete").click(function () {
        $.post("/Admin/DeleteShirt", { "id": $(this).data("id") });
        $(this).parents("tr").remove();
    });

    $("#add-series").click(function () {
        $(".newseries-container").show();
        $(".series-container").hide();
    });

    $('input[type="checkbox"]').click(function(){
        var $this = $(this);
        if ($this.val().toLowerCase() == "true") {
            $this.val("false");
        } else {
            $this.val("true");
        }

        if($this.hasClass("display-check")){
            $.post("/Admin/SetDisplay",
            {
                "id": $this.data("id"),
                "display": $this.val()
            });
        }
    });
});