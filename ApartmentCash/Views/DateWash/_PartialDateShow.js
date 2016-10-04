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
                var $day = $("<div class='dateWash-day " + extClass + "' date='" + dateStart.Format("yyyy-MM-dd") + "'>" + dateStart.getDate() + "</div>");
                $day.appendTo($row);
                dateStart=getDayNext(dateStart);
            }
            $row.appendTo($container);
        }



        var defaultCss = $.fn.dateWash.css;
        $("head").append(defaultCss);
        return this;
    }

    $.fn.dateWash.defaults = {
        container: "",
        year: "",
        month: "",
        day: ""
    };

    $.fn.dateWash.css = " \
                <style type='text/css'>\r\n\
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