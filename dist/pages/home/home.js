/// <reference path="../../references.ts"/>
var TodoWinTs;
(function (TodoWinTs) {
    "use strict";
    WinJS.UI.Pages.define("dist/pages/home/home.html", {
        // This function is called whenever a user navigates to this page. It
        // populates the page elements with the app's data.
        ready: function (element, options) {
            // TODO: Initialize the page here.
            WinJS.UI.processAll();
        }
    });
})(TodoWinTs || (TodoWinTs = {}));
