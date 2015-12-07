/// <reference path="~/Scripts/jquery-2.1.4.js"/>

'use strict';

function onSuccessUpdate(data, a, b, c) {
    console.log("onSuccessUpdate");
}

function onFailureUpdate(data, a, b, c) {
    console.log("onFailureUpdate");

    if (data.responseJSON.ErrorMessage)
        alert(data.responseJSON.ErrorMessage);
}

function onBeginUpdate(data, a, b, c) {
    console.log("onBeginUpdate");
}

function onCompleteUpdate(data, a, b, c) {
    console.log("onCompleteUpdate");
}

$('.zg-role-checkbox').click(function () {
    var $checkbox = $(this);
    var username = $checkbox.attr('data-zg-username');
    var role = $checkbox.attr('data-zg-role');

    var $loading = $('span[data-zg-username="' + username + '"][data-zg-role="' + role + '"]');

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
            console.log('begin complete');
            console.log(jqXHR);
            console.log(textStatus);
            $checkbox.show();
            $loading.hide();
            console.log('end complete');
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        $checkbox[0].checked = !$checkbox[0].checked;

        if (jqXHR.responseJSON.ErrorMessage)
            alert(jqXHR.responseJSON.ErrorMessage);
    });
});