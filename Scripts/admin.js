$(document).ready(function () {
    var $faqClone = $(".faq-create").last().clone();
    setClone($faqClone);

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

    $("#faq-add").click(function () {
        $faqCreate = $(".faq-create");
        if($faqCreate.find(".question").first().val() == "" || 
            $faqCreate.find(".answer").first().val() == "") {
            return;
        }

        $faqCreate.last().after($faqClone);
        $faqClone = $(".faq-create").last().clone();
        setClone($faqClone);
    });

    $("#faq-form").submit(function (e) {
        e.preventDefault();

        var json = {};

        $(this).find(".question").each(function () {
            var $this = $(this);
            var selector = "#a" + "_" + $this.attr("id").split("_")[1];
            var answer = $(selector).val();

            json[$this.val()] = answer;
        });

        $.post("/Admin/FAQ", { "json": JSON.stringify(json) }, function () { window.location.reload(); });
    });
});

function setClone($clone) {
    $clone.find("label").each(function () {
        var forVal = $(this).attr("for").split('_');
        $(this).attr("for", forVal[0] + "_" + (parseInt(forVal[1]) + 1));
    });

    $clone.find("input").each(function () {
        var idVal = $(this).attr("id").split('_');

        $(this).attr({
            id: idVal[0] + "_" + (parseInt(idVal[1]) + 1),
            name: idVal[0] + "_" + (parseInt(idVal[1]) + 1)
        });
    });
}