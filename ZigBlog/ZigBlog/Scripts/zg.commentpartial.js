/// <reference path="~/Scripts/jquery-2.1.4.js"/>

'use strict';

function onSuccessPostComment(data) {
    var id = data.IsTopLevel ? 'TopLevel' : data.ParentId;

    // Appends the new comment to the aproprieate comments root
    $('#commentsRoot' + id).append(data.PartialView);

    // Register an event for the "Reply to comment" button of this new comment
    $('.reply-to-comment[data-zg-id="' + data.Id + '"]').click(function () {
        var $comment = $(this).parents('.zg-comment');
        var $form = $comment.find('#form' + $(this).attr('data-zg-id'));
        $form.toggleClass('hidden');
    });

    // Clears the textarea that originated this new comment
    $('#form' + id + " textarea").val(null);

    // Look the form reposible for this comment submission using the id of the parent comment
    var $validationMessage = $('#form' + id + ' [data-valmsg-for="Content"]');

    // Clears the error message if any
    $validationMessage
        .removeClass('field-validation-error')
        .addClass('field-validation-valid')
        .html(null);

    // Goes to the new comment on the page
    location.href = location.origin + location.pathname + '#comment' + data.ParentId;
}

function onFailurePostComment(data) {
    if (data.responseJSON.ErrorMessage) {
        alert(data.responseJSON.ErrorMessage);
        return;
    }

    // Retrieves the value of a property that states if it's top level comment
    var isTopLevel = $.grep(data.responseJSON, function (e) {
        return e.Key === 'IsTopLevel';
    })[0].Value[0];

    // Retrieves the id for the parent comment
    var parentId = $.grep(data.responseJSON, function (e) {
        return e.Key === 'ParentId';
    })[0].Value[0];

    // Retrieves the error message
    var contentError = $.grep(data.responseJSON, function (e) {
        return e.Key === 'Content';
    })[0].Errors[0];

    // Look the form reposible for this comment submission using the id retrieved above
    var $validationMessage = $('#form' + (isTopLevel === 'True' ? 'TopLevel' : parentId) + ' [data-valmsg-for="Content"]');

    // Displays the validation error message
    $validationMessage
        .removeClass('field-validation-valid')
        .addClass('field-validation-error')
        .html('<p>' + contentError + '</p>');
}

$(function () {
    var $buttons = $('.reply-to-comment');

    for (var button in $buttons.toArray()) {
        var $button = $($buttons[button]);

        // Shows or hides the new comments submission form
        $button.click(function () {
            var $comment = $(this).parents('.zg-comment');
            var $form = $comment.find('#form' + $(this).attr('data-zg-id'));
            $form.toggleClass('hidden');
            $form.find('textarea').focus();
        });
    }
});