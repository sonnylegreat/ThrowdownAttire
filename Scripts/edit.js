$(document).ready(function () {
    $("#thumb-container").sortable();
    $("#thumb-container").disableSelection();

    $("#save").click(function () {
        $("#editform").submit();
    });

    $("#pictures").change(function () {
        var formData = new FormData($('#photo-upload')[0]);

        $.ajax({
            url: '/Admin/UploadPhoto',
            type: 'Post',
            beforeSend: function () { },
            success: function (result) {
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

        $("#photo-upload")[0].reset();
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