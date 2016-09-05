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

    $(".faq-add").on("click", function () {
        $faqCreate = $(this).siblings(".faq-create");
        if($faqCreate.find(".question").last().val() == "" || 
            $faqCreate.find(".answer").last().val() == "") {
            return;
        }

        $faqCreate.last().after($faqClone);
        setClone($faqClone, $faqCreate.parent().data("cnum"), $faqCreate.last().data("qnum"));
        $faqClone = $(this).siblings(".faq-create").last().clone(true, true);
    });

    $("#category-add").click(function () {
        $categoryCreate = $(this).siblings(".category-container").last();
        if ($categoryCreate.find(".question").last().val() == "" ||
            $categoryCreate.find(".answer").last().val() == "") {
            return;
        }

        $categoryCreate.after($categoryClone);
        setCatClone($categoryClone, $categoryCreate.data("cnum"));
        $categoryClone = $categoryClone.clone(true, true);

    });

    $(".faq-delete").click(function () {
        var keySplit = $(this).data("key").split("_");
        $.post("/Admin/DeleteFAQ",
            {
                "cat": keySplit[0],
                "q": keySplit[1]
            });
        $(this).parents(".faq-container").remove();
    });

    $(".category-delete").click(function () {
        $.post("/Admin/DeleteCategory", { "cat": $(this).data("key") });
        $(this).parents(".category-container").remove();
    })

    var $faqClone = $(".faq-create").last().clone(true, true);
    var $categoryClone = $(".category-container").last().clone(true, true);

    $("#faq-form").submit(function (e) {
        e.preventDefault();

        var json = {};
        var faqs = [];

        $(this).find(".category-container").each(function () {
            var $this = $(this);

            var faq = {};
            var questions = [];
            var answers = [];

            var category = $this.find(".category").val();

            if (category.length > 0) {
                faq["category"] = category;
            }

            $this.find(".question").each(function(){
                var $this = $(this);
                var question = $this.val();

                if (question.length > 0) {
                    questions.push(question);
                }

                var iSplit = $this.attr("id").split("_");

                var selector = "#a" + "_" + iSplit[1] + "_" + iSplit[2];

                var answer = $(selector).val();
                if (answer.length > 0) {
                    answers.push(answer);
                }
            });

            faq["questions"] = questions;
            faq["answers"] = answers;
            faqs.push(faq);
        });

        json["faqs"] = faqs;

        $.post("/Admin/FAQ", { "json": JSON.stringify(json) }, function () { window.location.reload(); });
    });
});

function setClone($clone, cString, qString) {
    var cnum = parseInt(cString);
    var qnum = parseInt(qString);

    $clone.find("label").each(function () {
        var forVal = $(this).attr("for").split('_');
        $(this).attr("for", forVal[0] + "_" + cnum + "_" + (qnum + 1));
    });

    $clone.find("input").each(function () {
        var idVal = $(this).attr("id").split('_');

        $(this).attr({
            id: idVal[0] + "_" + cnum + "_" + (qnum + 1),
            name: idVal[0] + "_" + cnum + "_" + (qnum + 1)
        });
    });

    $clone.data("qnum", qnum + 1);
}

function setCatClone($clone, cString) {
    var cnum = parseInt(cString);

    var $categoryInput = $clone.find(".category").first();
    $categoryInput.attr("id", "category_" + (cnum + 1));
    $categoryInput.val("Category " + (cnum + 1));

    setClone($clone.find(".faq-create"), cnum + 1, -1);

    $clone.data("cnum", cnum + 1);
}