$(document).ready(function(){
    var margin = ["left"];
    var $shirtModals = $(".shirtModal");
    var $oldParent;

    $(".modal-footer, .modal-header").css("visibility", "hidden");
    
    function hideInfo($elem){
        $elem.addClass("hvr-grow")
            .removeClass("expandedModal")
            .appendTo($oldParent);
            
        $(".info").hide();
    }
    
    function hoverChange($elem, position, value){
        $elem.css("position", position)
        .css("left", 0)
        .css("z-index", 1);

        $elem.animate({ marginTop: value });
    }
    
    function createShirtPage($elem){
        $oldParent = $elem.parent();
        var series = $oldParent.parent().parent().data("series");
            
        $elem.removeClass("hvr-grow")
        .addClass("expandedModal")
        .appendTo($("#shirtPage" + series));
            
        $(".info").show();
    }
    
    function snapScroll($anchor){
        $("button.series").removeAttr("disabled");
        $("button" + $anchor.replace('#', '.')).attr("disabled", "true");
        
        $('html, body').stop().animate({
                scrollLeft: $($anchor).offset().left,
                scrollTop: $($anchor).offset().top
            }, 1000);
        event.preventDefault();
    }
    
    $shirtModals.mouseenter(function () {

        var $this = $(this);
        $this.find(".modal-footer, .modal-header").css("visibility", "visible");

    }).mouseleave(function (event) {

        var $this = $(this);
        $this.find(".modal-footer, .modal-header").css("visibility", "hidden");
        
    }).click(function (event) {
        var $this = $(this);
        
        if(event.target.className === "info close"){
            
            hideInfo($this);
        }
        else if(!($this.hasClass("expandedModal"))){
            createShirtPage($this);
        }
    });
    $("a, li").mouseup(function(){
        $(this).blur();
    });

    $("#owlCarousel").owlCarousel({
        items: 1
    })
});