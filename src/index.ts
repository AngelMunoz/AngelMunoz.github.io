/// <reference path="./references.ts"/>
module TsBlog {
    "use strict";
    declare var Application;
    var app = WinJS.Application;
    var nav = WinJS.Navigation;
    var sched = WinJS.Utilities.Scheduler;
    var ui = WinJS.UI;

    app.addEventListener("ready", function (args) {
        nav.history = app.sessionState.history || {};
        nav.history.current.initialPlaceholder = true;
        
        // Optimize the load of the application and while the splash screen is shown, execute high priority scheduled work.
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
        // TODO: This application is about to be suspended. Save any state
        // that needs to persist across suspensions here. If you need to
        // complete an asynchronous operation before your application is
        // suspended, call args.setPromise().
        app.sessionState.history = nav.history;
    };

    app.start();
}
