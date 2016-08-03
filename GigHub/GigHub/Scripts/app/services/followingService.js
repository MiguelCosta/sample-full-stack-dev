var FollowingService = function () {
    var createFollowing = function (followeeId, done, fail) {
        var followDto = { followeeId: followeeId };
        
        $.post("/api/followings", followDto)
            .done(done).fail(fail);
    };

    var deleteFollowing = function (followeeId, done, fail) {
        $.ajax({
            url: "/api/followings/" + followeeId,
            method: 'DELETE'
        })
            .done(done)
            .fail(fail);
    };

    return {
        createFollowing: createFollowing,
        deleteFollowing: deleteFollowing
    };
}();
