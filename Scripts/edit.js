$(document).ready(function () {
    $("#thumb-container").sortable();
    $("#thumb-container").disableSelection();

    $("#save").click(function () {
        $("#editform").submit();
    });

    $("#pictures").change(function () {
        var formData = new FormData($('#photo-upload')[0]);
        $("#success-text").show();

        $.ajax({
            url: '/Admin/UploadPhoto',
            type: 'Post',
            beforeSend: function () { },
            success: function (result) {
                for (var i = 0; i < result.sources.length; i++) {
                    var src = result.sources[i];

                    var $dragClone = $(".ui-state-default").last().clone(true, true);
                    $dragClone.children("a").last().attr("href", src);
                    $dragClone.find("img").first().attr("src", src);
                    $dragClone.find("input").first().val(src);

                    $("#thumb-container").append($dragClone);
                }

                $("#success-text").text("Upload Successful").show(0).delay(1500).hide(0);
                $("#photo-upload")[0].reset();
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

    $(".pic-delete").click(function () {
        $.post("/Admin/DeletePhoto",
            {
                "src": $(this).siblings("a").children("img").attr("src"),
                "id": $("#id").val()
            });
        $(this).parents("li").remove();
    });

    $("#back").click(function () {
        window.location.href = "../../Auth/Admin";
    });
});