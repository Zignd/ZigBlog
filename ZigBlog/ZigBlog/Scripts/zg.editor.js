/// <reference path="~/Scripts/jquery-2.1.4.js"/>
/// <reference path="~/Scripts/MarkdownDeep.min.js"/>
/// <reference path="~/Scripts/MarkdownDeepEditor.min.js"/>

'use strict';

$(function () {
    var editor = new MarkdownDeepEditor.Editor(document.getElementById('editor'), document.getElementById('editorOutput'));

    editor.Markdown.OnPrepareImage = function (tag) {
        tag.attributes.class = "zg-post-image-width";
    }

    $('.zg-editor-bar-btn-undo').click(function () { editor.InvokeCommand('undo'); });
    $('.zg-editor-bar-btn-redo').click(function () { editor.InvokeCommand('redo'); });
    $('.zg-editor-bar-btn-bold').click(function () { editor.InvokeCommand('bold'); });
    $('.zg-editor-bar-btn-italic').click(function () { editor.InvokeCommand('italic'); });
    $('.zg-editor-bar-btn-heading').click(function () { editor.InvokeCommand('heading'); });
    $('.zg-editor-bar-btn-code').click(function () { editor.InvokeCommand('code'); });
    $('.zg-editor-bar-btn-ullist').click(function () { editor.InvokeCommand('ullist'); });
    $('.zg-editor-bar-btn-ollist').click(function () { editor.InvokeCommand('ollist'); });
    $('.zg-editor-bar-btn-indent').click(function () { editor.InvokeCommand('indent'); });
    $('.zg-editor-bar-btn-outdent').click(function () { editor.InvokeCommand('outdent'); });
    $('.zg-editor-bar-btn-link').click(function () { editor.InvokeCommand('link'); });
    $('.zg-editor-bar-btn-img').click(function () { editor.InvokeCommand('img'); });
    $('.zg-editor-bar-btn-hr').click(function () { editor.InvokeCommand('hr'); });
    $('.zg-editor-bar-btn-h0').click(function () { editor.InvokeCommand('h0'); });
    $('.zg-editor-bar-btn-h1').click(function () { editor.InvokeCommand('h1'); });
    $('.zg-editor-bar-btn-h2').click(function () { editor.InvokeCommand('h2'); });
    $('.zg-editor-bar-btn-h3').click(function () { editor.InvokeCommand('h3'); });
    $('.zg-editor-bar-btn-h4').click(function () { editor.InvokeCommand('h4'); });
    $('.zg-editor-bar-btn-h5').click(function () { editor.InvokeCommand('h5'); });
    $('.zg-editor-bar-btn-h6').click(function () { editor.InvokeCommand('h6'); });
    $('.zg-editor-bar-btn-tab').click(function () { editor.InvokeCommand('tab'); });
    $('.zg-editor-bar-btn-untab').click(function () { editor.InvokeCommand('untab'); });

    $('.zg-editor-area').focus(function () {
        $(this).parent().addClass('zg-editor-border-focused');
    }).blur(function () {
        $(this).parent().removeClass('zg-editor-border-focused');
    });
});



