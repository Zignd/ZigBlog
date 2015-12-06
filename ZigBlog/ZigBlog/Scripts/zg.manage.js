/// <reference path="~/Scripts/jquery-2.1.4.js"/>

'use strict';

function onSuccessUpdate(data, a, b, c) {
    console.log("onSuccessUpdate");
    console.log(data);
    console.log(a);
    console.log(b);
    console.log(c);
}

function onFailureUpdate(data, a, b, c) {
    console.log("onFailureUpdate");
    console.log(data);
    console.log(a);
    console.log(b);
    console.log(c);

    if (data.responseJSON.ErrorMessage)
        alert(data.responseJSON.ErrorMessage);
}

function onBeginUpdate(data, a, b, c) {
    console.log("onBeginUpdate");
    console.log(data);
    console.log(a);
    console.log(b);
    console.log(c);
}

function onCompleteUpdate(data, a, b, c) {
    console.log("onCompleteUpdate");
    console.log(data);
    console.log(a);
    console.log(b);
    console.log(c);
}

$('.zg-role-checkbox').click(function () {
    var $checkbox = $(this);
    $checkbox.parents('form').submit();
});