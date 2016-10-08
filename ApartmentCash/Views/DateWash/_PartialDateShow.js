///<reference path='/js.html'/>


(function ($) {
    $.fn.dateWash = function (options) {
        var opts = $.extend({}, $.fn.dateWash.defaults, options);


        var $container = $(this);

        var week = ['日', '一', '二', '三', '四', '五', '六'];
        var $weekHead = $("<div/>", { class: 'dateWash-row dateWash-head' });
        week.forEach(function (item) { 
            $weekHead.append('<div class="dateWash-day">' + item + '</div>');
        });
        $container.append($weekHead);


        var date = new Date(opts.year, opts.month - 1, opts.day);
        var monthFirstDate = new Date(opts.year, opts.month - 1, 1);
        var preMonthDate = new Date(opts.year, opts.month - 2, opts.day);
        var preMonthDayCount = getMonthCount(preMonthDate);


        var firstDateWeek = monthFirstDate.getDay();//本月1号是一周第几天

        var dateStart = getDayNext(monthFirstDate, -firstDateWeek);

        for (var i = 0; i < 6; i++) {
            var $row = $("<div class='dateWash-row'></div>");
            for (var j = 0; j < 7; j++) {
                var month = dateStart.getMonth() + 1;
                var day = dateStart.getDate();
                var extClass = "";
                if (month < opts.month) {
                    extClass = "preMonth";
                }
                else if (month > opts.month) {
                    extClass = "nextMonth";
                }
                else if (month == opts.month && day == opts.day) {
                    extClass = "curDay";
                }
                var $day = $("<div class='dateWash-day " + extClass + "' date='" + dateStart.Format("yyyy-MM-dd") + "'><div class='date-day'>" + dateStart.getDate() + "</div></div>");
                $day.appendTo($row);
                dateStart=getDayNext(dateStart);
            }
            $row.appendTo($container);
        }

        if (opts.data) {
            $container.find('.dateWash-day[date]').each(function () {
                var dateStr = $(this).attr('date');
                var filter = opts.data.filter(function (item) {
                    return item.DateStr == dateStr;
                });
                if(filter.length>0)
                {
                    $(this).attr(filter[0]).append("<span class='dateWashName'>" + (filter[0].UserName||"") + "</span>");;
                }
            });
        }

        var defaultCss = $.fn.dateWash.css;
        $("head").append(defaultCss);
        $container.css({
            'background-color': 'rgb(83,84,84)',
            display: 'inline-block'
        });

        var needWidth = $container.find('.dateWash-day:eq(0)').outerWidth(true) * 7;
        var nowWidth = $container.width();
        if (needWidth > nowWidth)
        {
            //$container.width(needWidth).css({
            //    'transform': 'scale(' + nowWidth / needWidth + ')',
            //    'transform-origin': 'top left'
            //});
            $container.width(needWidth).css({
                'zoom':nowWidth / needWidth,
            });
        }
        return this;
    }

    $.fn.dateWash.defaults = {
        container: "",
        year: "",
        month: "",
        day: "",
        data:null
    };

    $.fn.dateWash.css = " \
        <style type='text/css'>\r\n\
            .dateWash-day {\
                display:inline-block;\
                box-sizing:border-box;\
                border:3px solid rgb(83,84,84);\
                margin:2px;\
                width:60px;\
                height:60px;\
                font-family:'Microsoft YaHei';\
                text-align:center;\
                color:rgb(253,253,253);\
                vertical-align:top;\
                cursor:default;\
            }\
            .dateWash-day:hover {\
                border:3px solid rgb(153,153,153);\
            }\
            .dateWash-row.dateWash-head .dateWash-day{\
                line-height:54px;\
                color:rgb(236,236,236);\
            }\
            .dateWash-row.dateWash-head  .dateWash-day:hover {\
                border:3px solid rgb(83,84,84);\
            }\
            .dateWash-row .dateWash-day.preMonth,.dateWash-row .dateWash-day.nextMonth {\
                color:rgb(140,140,140);\
            }\
            .dateWash-row .dateWash-day .date-day {\
                padding-top: 4px;\
                padding-bottom: 4px;\
            }\
            .dateWash-row .dateWash-day .dateWashName {\
                display:block;\
                font-size:13px;\
            }\
            .dateWash-row .dateWash-day.curDay{\
                background-color:rgb(0,120,215);\
            }\
            .dateWash-row .dateWash-day.curDay .date-day {\
                padding-top: 4px;\
                padding-bottom: 4px;\
            }\
            .dateWash-row .dateWash-day.curDay .dateWashName {\
                display:block;\
                font-size:13px;\
            }\
        </style>";


    function getMonthCount(d)
    {
       return new Date(d.getFullYear(), (d.getMonth() + 1), 0).getDate();
    }

    
    function getDayNext(t, diff) {
        diff = diff || 1;
        var tm = new Date(t.getFullYear(), t.getMonth(), t.getDate() + diff);
        return tm;
    }
    Date.prototype.Format = function (fmt) { //author: meizz 
        var o = {
            "M+": this.getMonth() + 1, //月份 
            "d+": this.getDate(), //日 
            "h+": this.getHours(), //小时 
            "m+": this.getMinutes(), //分 
            "s+": this.getSeconds(), //秒 
            "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
            "S": this.getMilliseconds() //毫秒 
        };
        if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
        for (var k in o)
            if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
        return fmt;
    }
})(jQuery);