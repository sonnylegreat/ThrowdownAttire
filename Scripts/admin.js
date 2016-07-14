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

    $(".sliderform").submit(function (e) {
        e.preventDefault();

        var formData = new FormData();
        var $this = $(this);
        var type = $this.data("type");

        formData.append("type", type);
        formData.append("image", $this.find("input")[0].files[0]);

        var $btn = $($this.find("button").first())
        $btn.button("loading");

        $.ajax({
            url: '/Admin/UpdateSlider',
            type: 'Post',
            beforeSend: function () { },
            success: function (result) {
                var src = result.src;
                $("#" + type).attr("src", src);

                $this[0].reset();
                $btn.button("reset");
            },
            xhr: function () {  // Custom XMLHttpRequest
                var myXhr = $.ajaxSettings.xhr();
                if (myXhr.upload) { // Check if upload property exists
                    // Progress code if you want
                }
                return myXhr;
            },
            error: function () { },
            data: formData,
            cache: false,
            contentType: false,
            processData: false
        });
    });
});