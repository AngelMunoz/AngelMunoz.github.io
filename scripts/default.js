var TodoWinTs;
(function (TodoWinTs) {
    "use strict";
    var app = WinJS.Application;
    var nav = WinJS.Navigation;
    var sched = WinJS.Utilities.Scheduler;
    var ui = WinJS.UI;
    app.addEventListener("ready", function (args) {
        nav.history = app.sessionState.history || {};
        nav.history.current.initialPlaceholder = true;
        ui.disableAnimations();
        var p = ui.processAll().then(function () {
            return nav.navigate(nav.location || Application.navigator.home, nav.state);
        }).then(function () {
            return sched.requestDrain(sched.Priority.aboveNormal + 1);
        }).then(function () {
            ui.enableAnimations();
        });
        args.setPromise(p);
    });
    app.oncheckpoint = function (args) {
        app.sessionState.history = nav.history;
    };
    app.start();
})(TodoWinTs || (TodoWinTs = {}));
//# sourceMappingURL=default.js.map