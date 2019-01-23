var CalendarData = new Array(20);
var madd = new Array(12);
var TheDate = new Date();
var tgString = "鐢蹭箼涓欎竵鎴婂繁搴氳緵澹櫢";
var dzString = "瀛愪笐瀵呭嵂杈板烦鍗堟湭鐢抽厜鎴屼亥";
var numString = "涓€浜屼笁鍥涗簲鍏竷鍏節鍗�";
var monString = "姝ｄ簩涓夊洓浜斿叚涓冨叓涔濆崄鍐厞";
var weekString = "鏃ヤ竴浜屼笁鍥涗簲鍏�";
var sx = "榧犵墰铏庡厰榫欒泧椹緤鐚撮浮鐙楃尓";
var cYear;
var cMonth;
var cDay;
var cHour;
var cDateString;
var DateString;
var Browser = navigator.appName;

function init() {
    CalendarData[0] = 0x41A95;
    CalendarData[1] = 0xD4A;
    CalendarData[2] = 0xDA5;
    CalendarData[3] = 0x20B55;
    CalendarData[4] = 0x56A;
    CalendarData[5] = 0x7155B;
    CalendarData[6] = 0x25D;
    CalendarData[7] = 0x92D;
    CalendarData[8] = 0x5192B;
    CalendarData[9] = 0xA95;
    CalendarData[10] = 0xB4A;
    CalendarData[11] = 0x416AA;
    CalendarData[12] = 0xAD5;
    CalendarData[13] = 0x90AB5;
    CalendarData[14] = 0x4BA;
    CalendarData[15] = 0xA5B;
    CalendarData[16] = 0x60A57;
    CalendarData[17] = 0x52B;
    CalendarData[18] = 0xA93;
    CalendarData[19] = 0x40E95;
    madd[0] = 0;
    madd[1] = 31;
    madd[2] = 59;
    madd[3] = 90;
    madd[4] = 120;
    madd[5] = 151;
    madd[6] = 181;
    madd[7] = 212;
    madd[8] = 243;
    madd[9] = 273;
    madd[10] = 304;
    madd[11] = 334;
}

function GetBit(m, n) {
    return (m >> n) & 1;
}

function e2c() {
    var totalmnk;
    var isEnd = false;
    var tmp = TheDate.getYear();
    if (tmp < 1900) tmp += 1900;
    total = (tmp - 2001) * 365
        + Math.floor((tmp - 2001) / 4)
        + madd[TheDate.getMonth()]
        + TheDate.getDate()
        - 23;
    if (TheDate.getYear() % 4 == 0 && TheDate.getMonth() > 1)
        total++;
    for (m = 0; ; m++) {
        k = (CalendarData[m] < 0xfff) ? 11 : 12;
        for (n = k; n >= 0; n--) {
            if (total <= 29 + GetBit(CalendarData[m], n)) {
                isEnd = true;
                break;
            }
            total = total - 29 - GetBit(CalendarData[m], n);
        }
        if (isEnd) break;
    }
    cYear = 2001 + m;
    cMonth = k - n + 1;
    cDay = total;
    if (k == 12) {
        if (cMonth == Math.floor(CalendarData[m] / 0x10000) + 1)
            cMonth = 1 - cMonth;
        if (cMonth > Math.floor(CalendarData[m] / 0x10000) + 1)
            cMonth--;
    }
    cHour = Math.floor((TheDate.getHours() + 3) / 2);
}

function GetcDateString() {
    var tmp = "";
    tmp += tgString.charAt((cYear - 4) % 10);       //骞村共
    tmp += dzString.charAt((cYear - 4) % 12);       //骞存敮
    tmp += "骞�(";
    tmp += sx.charAt((cYear - 4) % 12);
    tmp += ")   ";
    if (cMonth < 1) {
        tmp += "闂�";
        tmp += monString.charAt(-cMonth - 1);
    }
    else
        tmp += monString.charAt(cMonth - 1);
    tmp += "鏈�";
    tmp += (cDay < 11) ? "鍒�" : ((cDay < 20) ? "鍗�" : ((cDay < 30) ? "寤�" : "鍗�"));
    if (cDay % 10 != 0 || cDay == 10)
        tmp += numString.charAt((cDay - 1) % 10);
    tmp += "    ";
    if (cHour == 13) tmp += "澶�";
    //tmp+=dzString.charAt((cHour-1)%12);
    //tmp+="鏃�";
    cDateString = tmp;
    return tmp;
}

function GetDateString() {
    var tmp = "";
    var t1 = TheDate.getYear();
    if (t1 < 1900) t1 += 1900;
    tmp += t1
        + "骞�"
        + (TheDate.getMonth() + 1) + "鏈�"
        + TheDate.getDate() + "鏃�   "
        //+TheDate.getHours()+":"
        //+((TheDate.getMinutes()<10)?"0":"")
        //+TheDate.getMinutes()
        + " 鏄熸湡" + weekString.charAt(TheDate.getDay());
    DateString = tmp;
    return tmp;
}

init();
e2c();
GetDateString();
GetcDateString();
document.write(DateString + " " + cDateString);
var c_doing = !1, _c_t, readMore = function () {
    function e() {
        r ? ($(".article_content").height("").css({
            overflow: "hidden"
        }), $(".readall_box").show().addClass("readall_box_nobg"), $(".readall_box").hide().addClass("readall_box_nobg"), r = !1) : ($(".article_content").height(2 * t).css({
            overflow: "hidden"
        }), $(".readall_box").show().removeClass("readall_box_nobg"), r = !0)
    }
    var t = $(window).height(),
        n = $(".article_content").height();
    !1;
    if (n > 2 * t) {
        $(".article_content").height(2 * t - 680).css({
            overflow: "hidden"
        });
        var r = !0;
        $(".read_more_btn").on("click", e)
    } else r = !0,
        $(".article_content").removeClass("article_Hide"),
        $(".readall_box").hide().addClass("readall_box_nobg")
};