var AttendanceService = function () {
    var createAttenance = function (gigId, done, fail) {
        var gigDto = { gigId: gigId };
        $.post("/api/attendances", gigDto)
            .done(done)
            .fail(fail);
    };

    var deleteAttendance = function (gigId, done, fail) {
        $.ajax({
            url: "/api/attendances/" + gigId,
            method: 'DELETE'
        })
            .done(done)
            .fail(fail);
    };

    return {
        createAttenance: createAttenance,
        deleteAttendance: deleteAttendance
    };
}();
