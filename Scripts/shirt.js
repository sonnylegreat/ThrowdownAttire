$(document).ready(function () {
    $("#owlCarousel").owlCarousel({
        items: 1,
        paginationNumbers: true
    });

    $("#size-selector").change(function () {
        var $buybutton = $("#buybutton");

        $buybutton.data("variant_id", $(this).val());
        $buybutton.find("iframe").attr("src", "https://widgets.shopifyapps.com/embed/product?eid=0&variant_id=" + $(this).val() + "&next_page_button_text=Next%20page&product_title_color=000000&product_modal=false&button_text_color=4d4a4a&button_background_color=e9f2ed&buy_button_product_unavailable_text=Unavailable&buy_button_out_of_stock_text=Out%20of%20Stock&buy_button_text=Add%20to%20cart&redirect_to=cart&has_image=false&product_handle=" + $buybutton.data("product_handle") + "&product_name=" + $buybutton.data("product_name").replace(" ", "%20") + "&shop=throwdown-attire.myshopify.com&embed_type=product&r=");
    });
});