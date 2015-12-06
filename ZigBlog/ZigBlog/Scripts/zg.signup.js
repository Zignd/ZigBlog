/// <reference path="~/Scripts/jquery-2.1.4.js"/>
/// <reference path="~/Scripts/jquery.validate.js"/>
/// <reference path="~/Scripts/jquery.validate.unobtrusive.js"/>

'use strict';

if ($.validator && $.validator.unobtrusive) {
    $.validator.unobtrusive.adapters.add('password', ['requiredigit', 'requiredlength', 'requirelowercase', 'requirenonletterordigit', 'requireuppercase'], function (options) {
        options.rules['password'] = options.params;
        options.messages['password'] = options.message;
    });

    $.validator.addMethod('password', function (value, element, params) {
        if (params.requiredigit === 'True' && /\d+/.exec(value.toString()) === null)
            return false;

        if (params.requiredlength !== 0 && value.toString().length < params.requiredlength)
            return false;

        if (params.requirelowercase === 'True' && /[a-z]/.exec(value.toString()) === null)
            return false;

        if (params.requireuppercase === 'True' && /[A-Z]/.exec(value.toString()) === null)
            return false;

        return true;
    });

}