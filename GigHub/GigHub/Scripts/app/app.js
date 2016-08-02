var GigsController = function () {

    var button;

    var gigId;

    var init = function () {
        $(".js-toggle-attendance").click(toggleAttendance);
    };

    var toggleAttendance = function (e) {
        button = $(e.target);
        gigId = button.attr("data-gig-id");

        if (button.hasClass('btn-default'))
            createAttendance();
        else
            deleteAttendance();
    };

    var fail = function () {
        alert("Fail");
    };

    var done = function () {
        var text = button.text() === 'Going' ? 'Going?' : 'Going';

        button
            .toggleClass('btn-info')
            .toggleClass('btn-default')
            .text(text);
    };

    var createAttendance = function () {
        var gigDto = { gigId: gigId };
        $.post("/api/attendances", gigDto)
            .done(done)
            .fail(fail);
    };

    var deleteAttendance = function () {
        $.ajax({
            url: '/api/attendances/' + gigId,
            method: 'DELETE'
        })
            .done(done)
            .fail(fail);
    };

    return {
        init: init
    };

}();
