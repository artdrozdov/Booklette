/**
 * Created by artem_000 on 10.05.2014.
 */


$(document).on('click', 'a[data-container]', function(event) {
    var container = $(this).attr("data-container");
    $.pjax.click(event,  container)
});

$(document).on('click', 'a', function (event) {
    var container = '#container';
    $.pjax.click(event, container)
})

$(document).on('pjax:start', function() {
    NProgress.start();
});

$(document).on('pjax:end', function () {
    ApplyHandlers();
    NProgress.done();
});

$(document).on('submit', 'form[data-container]', function(event) {
    $.pjax.submit(event, $(this).attr("data-container"))
});

function ApplyHandlers() {

    $('.navbar-nav li').hover(
        function () {
            $(this).addClass('active');
        },
        function () {
            $(this).removeClass('active');
        }
    );

    Holder.run({ images: ".thumbnail img" })

}

ApplyHandlers();
