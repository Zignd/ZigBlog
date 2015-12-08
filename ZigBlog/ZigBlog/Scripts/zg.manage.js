/// <reference path="~/Scripts/jquery-2.1.4.js"/>

'use strict';

$('.zg-role-checkbox').click(function () {
    var $checkbox = $(this);
    var username = $checkbox.attr('data-zg-username');
    var role = $checkbox.attr('data-zg-role');

    var $loading = $('.zg-role-checkbox-loading[data-zg-username="' + username + '"][data-zg-role="' + role + '"]');

    $checkbox.hide();
    $loading.show();

    $.ajax({
        url: '/user/updaterole',
        method: 'POST',
        data: {
            userName: $checkbox.attr('data-zg-username'),
            role: $checkbox.attr('data-zg-role'),
            isInRole: $checkbox[0].checked
        },
        complete: function (jqXHR, textStatus) {
            $checkbox.show();
            $loading.hide();
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        $checkbox[0].checked = !$checkbox[0].checked;

        if (jqXHR.responseJSON.ErrorMessage)
            alert(jqXHR.responseJSON.ErrorMessage);
    });
});

$('.zg-user-delete').click(function () {
    var $button = $(this);
    var username = $button.attr('data-zg-username');

    var $loading = $('.zg-user-delete-loading[data-zg-username="' + username + '"]');

    $button.hide();
    $loading.show();

    $.ajax({
        url: '/user/delete',
        method: 'POST',
        data: {
            userName: $button.attr('data-zg-username')
        },
        complete: function (jqXHR, textStatus) {
            $button.parents('tr').remove();
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        if (jqXHR.responseJSON.ErrorMessage)
            alert(jqXHR.responseJSON.ErrorMessage);
    });
});