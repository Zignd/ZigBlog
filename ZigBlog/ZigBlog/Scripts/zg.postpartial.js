/// <reference path="~/Scripts/jquery-2.1.4.js"/>

'use strict';

function onSuccessLikePost(data) {
    var counter = $(".zg-postpartial-panel-heading-like-counter[data-zg-postid='" + data.PostId + "']");
    var button = $(".zg-postpartial-panel-heading-like-button[data-zg-postid='" + data.PostId + "']");

    if (data.UserLikes) {
        button.addClass('zg-likes');
    } else {
        button.removeClass('zg-likes');
    }

    counter.text(data.LikesCount);
}

function onFailureLikePost(data) {
    if (data.responseJSON.ErrorMessage)
        alert(data.responseJSON.ErrorMessage);
}