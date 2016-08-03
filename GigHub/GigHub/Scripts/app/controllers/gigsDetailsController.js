var GigsDetailsController = function (followingService) {
    var followButton;

    var followeeId;

    var init = function () {
        $(".js-toggle-follow").click(toggleFollowing);
    };

    var toggleFollowing = function (e) {
        followButton = $(e.target);
        followeeId = followButton.attr("data-user-id");

        if (followButton.hasClass("btn-default"))
            followingService.createFollowing(followeeId, done, fail);
        else
            followingService.deleteFollowing(followeeId, done, fail);
    };

    var fail = function () {
        alert("Fail");
    };

    var done = function () {
        var text = followButton.text() === "Follow" ? "Following" : "Follow";

        followButton
            .toggleClass("btn-info")
            .toggleClass("btn-default")
            .text(text);
    };

    return {
        init: init
    };
}(FollowingService);
