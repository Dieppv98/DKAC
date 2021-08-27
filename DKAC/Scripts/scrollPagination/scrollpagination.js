/*
 * Yiven
 * 
 * jQuery ScrollPagination
 * 2020/02/17
 */

;(function($){
    var defaults = {
        'url': null,
        'autoload': true,
        'data': {
            'page': 1,
            'size': 15,
        }, 
        'before': function(){
            // Before load function, you can display a preloader div
            setTimeout(function () {
                $(this.loading).fadeIn();
            }, 3000);
        },
        'after': function(elementsLoaded){
            // After loading content, you can use this function to animate your new elements
            setTimeout(function () {
                $(this.loading).fadeOut();
                $(elementsLoaded).fadeInWithDelay();
            }, 3000);
        },
        'scroller': $('#scrollpagination'), // Who gonna scroll? default is the full window
        'heightOffset': 20, 
        'loading': '#loading',
        'loadingText': 'Đang tải...', 
        'loadingNomoreText': 'Không còn thông báo nào!',
        'manuallyText': 'click me to loading',
    };

    $.fn.scrollPagination = function(options) {
        var opts = $.extend(defaults, options);
        var target = opts.scroller;
        return this.each(function() {
            $.fn.scrollPagination.init($(this), opts);
        });
    };

    $.fn.stopScrollPagination = function(obj=null, opts=null){
        if(obj == null){
            return this.each(function() {
                $(this).attr('scrollPagination', 'disabled');
            });
        }else{
            $(opts.loading).text(opts.loadingNomoreText).fadeIn();
            $(obj).attr('scrollPagination', 'disabled');
        }
    };

    $.fn.scrollPagination.loadContent = function(obj, opts){
        var target = opts.scroller;
        // do before
        if (opts.before != null){
            opts.before();
        }
        $(obj).children().attr('rel', 'loaded');
        $.ajax({
            type: 'POST',
            url: opts.url,
            data: opts.data,
            dataType: 'json',
            async: false,
            cache: false,
            success: function(data){
                var html = "";
                if (data.lstNotiNew.length > 0 || data.lstNotiOld.length > 0) {
                    $(opts.loading).text(opts.loadingText);

                    $.each(data.lstNotiNew,function(index, value){
                        html += `<a class='dropdown-item d-flex align-items-center' style='padding-bottom:5px;cursor: pointer;text-transform: none;${value.SeenStatus != 1 ? "background-color:#d7d9db;" : ""}' onclick='${value.TypeNoti == 2 ? "RedirectViewNewDish(" + value.Id + "," + value.DishId + ")" : "RedirectAllDishs(" + value.Id + ")"}' href='#'>
                                     <div class='mr-3'>
                                         <div>
                                             <img src='/Content/image/Dish/${value.TypeNoti == 2 ? value.Image : "favicon.png"}' style='width:35px; height:35px;border-radius:25px;border:none;' />
                                         </div>
                                     </div>
                                     <div class='text-wrap wrapper'>
                                            <span>Bạn có thông báo mới: ${value.ContentNoti.trim()}</span>
                                            <div class='small'>`
                        if (value.Days > 0) {
                            html += `<span style='color:#138ce6;'>${value.Days} ngày trước</span>`
                        }
                        if (value.Days == 0 && value.Hours > 0) {
                            html += `<span style='color:#138ce6;'>Khoảng ${value.Hours} giờ trước</span>`
                        }
                        if (value.Days == 0 && value.Hours == 0 && value.Minutes > 0) {
                            html += `<span style='color:#138ce6;'>Khoảng ${value.Minutes} phút trước</span>`
                        }
                        if (value.Days == 0 && value.Hours == 0 && value.Minutes == 0 && value.Seconds > 0) {
                            html += `<span style='color:#138ce6;'>Khoảng ${value.Seconds} giây trước</span>`
                        }
                        html += `</div></div>`
                        if (value.SeenStatus != 1) {
                            html += `<span class='dot' style='background-color:#0095ff;height:11px;width:16px;border-radius:100%;display:inline-block;'><span>`
                        }
                        html += `</a>`;
                    });

                    var notifyNew = $('#notifyNew');
                    if (notifyNew != null && notifyNew != undefined && notifyNew.length > 0) {
                        var notifyOld = $('#notifyOld');
                        if (notifyOld == null || notifyOld == undefined || notifyOld.length <= 0) {
                            html += `<span id='notifyOld' style='font-size: 13px; padding-left: 25px;'><b>Trước đó</b></span>`;
                        }
                    }
                    
                    $.each(data.lstNotiOld, function (index, value) {
                        html += `<a class='dropdown-item d-flex align-items-center' style='padding-bottom:5px;cursor: pointer;text-transform: none;${value.SeenStatus != 1 ? "background-color:#d7d9db;" : ""}' onclick='${value.TypeNoti == 2 ? "RedirectViewNewDish(" + value.Id + "," + value.DishId + ")" : "RedirectAllDishs(" + value.Id + ")"}' href='#'>
                                     <div class='mr-3'>
                                         <div>
                                             <img src='/Content/image/Dish/${value.TypeNoti == 2 ? value.Image : "favicon.png"}' style='width:35px; height:35px;border-radius:25px;border:none;' />
                                         </div>
                                     </div>
                                     <div class='text-wrap wrapper'>
                                            <span>Bạn có thông báo mới: ${value.ContentNoti.trim()}</span>
                                            <div class='small'>`
                        if (value.Days > 0) {
                            html += `<span style='color:#138ce6;'>${value.Days} ngày trước</span>`
                        }
                        if (value.Days == 0 && value.Hours > 0) {
                            html += `<span style='color:#138ce6;'>Khoảng ${value.Hours} giờ trước</span>`
                        }
                        html += `</div></div>`
                        if (value.SeenStatus != 1) {
                            html += `<span class='dot' style='background-color:#0095ff;height:11px;width:16px;border-radius:100%;display:inline-block;'><span>`
                        }
                        html += `</a>`;
                    });
                    opts.data.page++; //tăng pageIndex lên 1 đơn vị

                    $('#contentLoading').append(html);
                }
                if (data.lstNotiNew.length == 0 && data.lstNotiOld.length == 0) {
                    $.fn.stopScrollPagination(obj, opts);
                    $(obj).attr('scrollPagination', 'disabled');
                }
                var objectsRendered = $(obj).children('[rel!=loaded]');
                // do after
                if (opts.after != null){
                    opts.after(objectsRendered);
                }
            },
            timeout: 2000,
        });
    };

    $.fn.scrollPagination.init = function(obj, opts){
        var target = opts.scroller;
        $(obj).attr('scrollPagination', 'enabled');
        if($(obj).children().length === 0){
            $.fn.scrollPagination.loadContent(obj, opts);
        }
        if(opts.autoload === true){
            $(target).scroll(function(event){
                if ($(obj).attr('scrollPagination') == 'enabled') {
                    var mayLoadContent = (Math.ceil($(target).scrollTop()) + $(target).height()/* + opts.heightOffset*/) > $('#contentLoading').height(); //nếu đã load quá độ dài của thẻ div chứa list các thông báo
                    if(mayLoadContent){
                        $.fn.scrollPagination.loadContent(obj, opts);

                        console.log('load more called !');
                    }
                }else{
                    event.stopPropagation(obj, opts);
                }
            });
            // $.fn.scrollPagination.loadContent(obj, opts);
        }else{
            opts.loadingText = opts.manuallyText;
            $(opts.loading).text(opts.loadingText).fadeIn().on('click', function(event){
                if($(obj).attr('scrollPagination') == 'enabled'){
                    $.fn.scrollPagination.loadContent(obj, opts);
                }else{
                    event.stopPropagation(obj, opts);
                }
            });
            // $.fn.scrollPagination.loadContent(obj, opts);
        }
    };
    
    // code for fade in element by element
    $.fn.fadeInWithDelay = function(){
        var delay = 0;
        return this.each(function(){
            $(this).delay(delay).animate({opacity:1}, 3000);
            delay += 3000;
        });
    };
})(jQuery);

/*
 * example
 * 
    $('#content').scrollPagination({
        'url': 'democontent.html', // the url you are fetching the results
        'data': {}, // these are the variables you can pass to the request, for example: children().size() to know which page you are
        'scroller': $(window), // who gonna scroll? in this example, the full window
        'heightOffset': 10, // it gonna request when scroll is 10 pixels before the page ends
        'before': function(){ // before load function, you can display a preloader div
            $('#loading').fadeIn();
        },
        'after': function(elementsLoaded){ // after loading content, you can use this function to animate your new elements
            $('#loading').fadeOut();
            var i = 0;
            $(elementsLoaded).fadeInWithDelay();
            if ($('#content').children().size() > 100){ // if more than 100 results already loaded, then stop pagination (only for testing)
                $('#nomoreresults').fadeIn();
                $('#content').stopScrollPagination();
            }
        }
    });
 *
 */
