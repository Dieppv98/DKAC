/*
 * Yiven
 * 
 * jQuery ScrollPagination
 * 2020/02/17
 */

var pageSizeChatMessage;
var pageIndexChatMessage = 2;
var toId = -1;
var typeUser;

; (function ($) {
    pageSizeChatMessage = 15;
    var defaults = {
        'url': null,
        'autoload': true,
        'data': {
            'page': 1,
            'size': pageSizeChatMessage,
            'toId': toId,
        },
        'before': function () {
            // Before load function, you can display a preloader div
            setTimeout(function () {
                $(this.loading).fadeIn();
            }, 3000);
        },
        'after': function (elementsLoaded) {
            // After loading content, you can use this function to animate your new elements
            setTimeout(function () {
                $(this.loading).fadeOut();
                $(elementsLoaded).fadeInWithDelay();
            }, 3000);
        },
        'scroller': $('#popupBody'), // Who gonna scroll? default is the full window
        'heightOffset': 20,
        'loading': '#loading',
        'loadingText': 'Đang tải...',
        'loadingNomoreText': 'Không còn tin nhắn nào!',
        'manuallyText': 'click me to loading',
    };

    $.fn.scrollPaginationChatMessage = function (options) {
        var opts = $.extend(defaults, options);
        var target = opts.scroller;
        return this.each(function () {
            $.fn.scrollPaginationChatMessage.init($(this), opts);
        });
    };

    $.fn.stopScrollPaginationChatMessage = function (obj = null, opts = null) {
        if (obj == null) {
            return this.each(function () {
                $(this).attr('scrollPagination', 'disabled');
            });
        } else {
            $(opts.loading).text(opts.loadingNomoreText).fadeIn();
            $(obj).attr('scrollPagination', 'disabled');
        }
    };

    $.fn.scrollPaginationChatMessage.loadContent = function (obj, opts) {
        var target = opts.scroller;
        opts.data.size = pageSizeChatMessage;
        opts.data.page = pageIndexChatMessage;
        if (typeUser == 3) toId = $('#idReceiverMessage').val() * 1;
        else {
            toId = -1;
        }
        opts.data.toId = toId;
        // do before
        if (opts.before != null) {
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
            success: function (rs) {
                var html = "";
                if (rs.length > 0) {
                    $(opts.loading).text(opts.loadingText);
                    for (var i = 0; i < rs.length; i++) {
                        if (rs[i].CreatedDate != null && rs[i].CreatedDate != "") {
                            var datetime = new Date(parseInt(rs[i].CreatedDate.substr(6))).toLocaleString('vi', { year: 'numeric', month: '2-digit', day: '2-digit' }).replace(/(\d+)\/(\d+)\/(\d+)/, '$1/$2/$3');
                            //nếu như những tin nhắn của các ngày khác đã có
                            $(`#lstMessages #messageDateTime_${datetime.replaceAll('/', '')}`).remove();
                            html += `<div id ='messageDateTime_${datetime.replaceAll('/', '')}' class="text-center"><div class="timestamp">${datetime}</div></div>`;

                            for (var j = 0; j < rs[i].lstChat.length; j++) {

                                var dateConvert = new Date(parseInt(rs[i].lstChat[j].CreatedDate.substr(6))) //convert định dạng
                                var date = ("0" + dateConvert.getDate()).slice(-2) + "/" + ("0" + (dateConvert.getMonth() + 1)).slice(-2) + "/" + dateConvert.getFullYear() + " " + ("0" + dateConvert.getHours()).slice(-2) + ":" + ("0" + dateConvert.getMinutes()).slice(-2) + ":" + ("0" + dateConvert.getSeconds()).slice(-2);

                                if (typeUser == 3) {
                                    if (rs[i].lstChat[j].SenderId == -1) {
                                        html += `<div class='direct-chat-msg pull-right message-right' title='${date}'>${rs[i].lstChat[j].Message}</div>`;
                                    } else {
                                        html += `<div class='direct-chat-msg pull-left message-left' title='${date}'>${rs[i].lstChat[j].Message}</div>`;
                                    }
                                } else {
                                    if (rs[i].lstChat[j].SenderId == userId) {
                                        html += `<div class='direct-chat-msg pull-right message-right' title='${date}'>${rs[i].lstChat[j].Message}</div>`;
                                    } else {
                                        html += `<div class='direct-chat-msg pull-left message-left' title='${date}'>${rs[i].lstChat[j].Message}</div>`;
                                    }
                                }
                            }
                        } else {
                            //nếu như những tin nhắn của các ngày khác đã có
                            var today = $('#messageToday');
                            if (today == null || today == undefined || today.length == 0) {
                                html += `<div id='messageToday' class="text-center"><div class="timestamp">Hôm nay</div></div>`;
                            } else { $('#messageToday').remove(); }
                            for (var j = 0; j < rs[i].lstChatHour.length; j++) {
                                html += `<div id='messageTodayHour_${rs[i].lstChatHour[j].Hour.substr(0, 2)}' class="text-center"><div class="timestamp">${rs[i].lstChatHour[j].Hour}</div></div>`
                                for (var k = 0; k < rs[i].lstChatHour[j].lstChat.length; k++) {

                                    var dateConvert = new Date(parseInt(rs[i].lstChatHour[j].lstChat[k].CreatedDate.substr(6))) //convert định dạng
                                    var date = ("0" + dateConvert.getDate()).slice(-2) + "/" + ("0" + (dateConvert.getMonth() + 1)).slice(-2) + "/" + dateConvert.getFullYear() + " " + ("0" + dateConvert.getHours()).slice(-2) + ":" + ("0" + dateConvert.getMinutes()).slice(-2) + ":" + ("0" + dateConvert.getSeconds()).slice(-2);

                                    if (typeUser == 3) {
                                        if (rs[i].lstChatHour[j].lstChat[k].SenderId == -1) {
                                            html += `<div class='direct-chat-msg pull-right message-right' title='${date}'>${rs[i].lstChatHour[j].lstChat[k].Message}</div>`;
                                        } else {
                                            html += `<div class='direct-chat-msg pull-left message-left' title='${date}'>${rs[i].lstChatHour[j].lstChat[k].Message}</div>`;
                                        }
                                    } else {
                                        if (rs[i].lstChatHour[j].lstChat[k].SenderId == userId) {
                                            html += `<div class='direct-chat-msg pull-right message-right' title='${date}'>${rs[i].lstChatHour[j].lstChat[k].Message}</div>`;
                                        } else {
                                            html += `<div class='direct-chat-msg pull-left message-left' title='${date}'>${rs[i].lstChatHour[j].lstChat[k].Message}</div>`;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    pageIndexChatMessage = pageIndexChatMessage + 1;
                    $('#lstMessages').prepend(html);//thêm vào đầu của html
                } else {
                    html += `<div style='display:none;' id='nothingMessage'></div>`;
                    $('#lstMessages').prepend(html);
                }
                var objectsRendered = $(obj).children('[rel!=loaded]');
                // do after
                if (opts.after != null) {
                    opts.after(objectsRendered);
                }
            },
            timeout: 2000,
        });
    };

    $.fn.scrollPaginationChatMessage.init = function (obj, opts) {
        var target = opts.scroller;
        $(obj).attr('scrollPagination', 'enabled');
        if ($(obj).children().length === 0) {
            $.fn.scrollPaginationChatMessage.loadContent(obj, opts);
        }
        if (opts.autoload === true) {
            $(target).scroll(function (event) {
                if ($(obj).attr('scrollPagination') == 'enabled') {
                    var mayLoadContent = (Math.ceil($(target).scrollTop())) <= 50; //scroll popup chat lên trên thì load thêm
                    if (mayLoadContent) {
                        var nothingMessage = $('#lstMessages #nothingMessage');
                        if (nothingMessage == null || nothingMessage == undefined || nothingMessage.length <= 0) {
                            $.fn.scrollPaginationChatMessage.loadContent(obj, opts);

                            console.log('load more called !');
                        }
                    }
                }
            });
        }
    };

    // code for fade in element by element
    $.fn.fadeInWithDelay = function () {
        var delay = 0;
        return this.each(function () {
            $(this).delay(delay).animate({ opacity: 1 }, 3000);
            delay += 3000;
        });
    };
})(jQuery);

