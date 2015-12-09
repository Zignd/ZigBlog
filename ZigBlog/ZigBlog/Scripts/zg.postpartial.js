/// <reference path="~/Scripts/jquery-2.1.4.js"/>

'use strict';

function onSuccessLikePost(data) {
    var $counter = $(".zg-postpartial-panel-heading-like-counter[data-zg-postid='" + data.PostId + "']");
    var $button = $(".zg-postpartial-panel-heading-like-button[data-zg-postid='" + data.PostId + "']");

    if (data.UserLikes) {
        $button.addClass('zg-likes');
    } else {
        $button.removeClass('zg-likes');
    }

    $counter.text(data.LikesCount);
}

function onFailureLikePost(data) {
    if (data.responseJSON.ErrorMessage)
        alert(data.responseJSON.ErrorMessage);
}

$('.zg-postpartial-button-expand').click(function () {
    var $button = $(this);
    var id = $button.attr('data-zg-postid');

    var $buttonCollapse = $('.zg-postpartial-button-collapse[data-zg-postid="' + id + '"]');

    $button.hide();
    $buttonCollapse.show();

    $('#content' + id).removeClass('zg-postpartial-content-collapsed');
    $('#contentShadow' + id).removeClass('zg-postpartial-content-shadow');
});

$('.zg-postpartial-button-collapse').click(function () {
    var $button = $(this);
    var id = $button.attr('data-zg-postid');

    var $buttonExpand = $('.zg-postpartial-button-expand[data-zg-postid="' + id + '"]');

    $button.hide();
    $buttonExpand.show();

    $('#content' + id).addClass('zg-postpartial-content-collapsed');
    $('#contentShadow' + id).addClass('zg-postpartial-content-shadow');
});