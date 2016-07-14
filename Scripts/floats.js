$(document).ready(function () {
    var fadeTime = 200;
    $(".modal-body").mouseenter(function () {
        var $this = $(this);
        $this.find(".front-pic").first().fadeOut(fadeTime);
        $this.find(".back-pic").first().fadeIn(fadeTime);
    }).mouseleave(function () {
        var $this = $(this);
        $this.find(".back-pic").first().fadeOut(fadeTime);
        $this.find(".front-pic").first().fadeIn(fadeTime);
    });
});