var GigsController = function (attendanceService) {
    var button;

    var gigId;

    var init = function (container) {
        $(container).on("click", ".js-toggle-attendance", toggleAttendance);
    };

    var toggleAttendance = function (e) {
        button = $(e.target);
        gigId = button.attr("data-gig-id");

        if (button.hasClass("btn-default"))
            attendanceService.createAttenance(gigId, done, fail);
        else
            attendanceService.deleteAttendance(gigId, done, fail);
    };

    var fail = function () {
        alert("Fail");
    };

    var done = function () {
        var text = button.text() === "Going" ? "Going?" : "Going";

        button
            .toggleClass("btn-info")
            .toggleClass("btn-default")
            .text(text);
    };

    return {
        init: init
    };
}(AttendanceService);
