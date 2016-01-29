$.ConfirmDialog = function (message, title, callbackYes, callbackNo, callbackArgument) {
    if ($("#modalConfirmDialog").length == 0)
        $('body').append('<div id="modalConfirmDialog"></div>');

    var dlg = $("#modalConfirmDialog")
        .html(message)
        .dialog({
            autoOpen: false, // set this to false so we can manually open it
            closeOnEscape: true,
            modal: true,
            title: title,
            buttons: [
                 {
                     text: "Sim",
                     click: function () {
                         if (callbackYes && typeof (callbackYes) === "function") {
                             if (callbackArgument == null) {
                                 callbackYes();
                             } else {
                                 callbackYes(callbackArgument);
                             }
                         }

                         $(this).dialog("close");
                     }
                 },
                 {
                     html:'N&atilde;o',
                     click: function () {
                         if (callbackNo && typeof (callbackNo) === "function") {
                             if (callbackArgument == null) {
                                 callbackNo();
                             } else {
                                 callbackNo(callbackArgument);
                             }
                         }

                         $(this).dialog("close");
                     }
                 }
            ]
        });
    dlg.dialog("open");
};

$.AlertDialog = function (message, title, callbackClose, callbackArgument) {
    if ($("#modalAlertDialog").length == 0)
        $('body').append('<div id="modalAlertDialog"></div>');

    var dlg = $("#modalAlertDialog")
        .html(message)
        .dialog({
            autoOpen: false, // set this to false so we can manually open it
            closeOnEscape: true,
            modal: true,
            title: title,
            close: function () {
                if (callbackClose && typeof (callbackClose) === "function") {
                    if (callbackArgument == null) {
                        callbackClose();
                    } else {
                        callbackClose(callbackArgument);
                    }
                }
            },
            buttons: {
                OK: function () {
                    $(this).dialog("close");
                }
            }
        });
    dlg.dialog("open");
};

$.ErrorDialog = function (message, callbackClose, callbackArgument) {
    if ($("#modalErrorDialog").length == 0)
        $('body').append('<div id="modalErrorDialog"></div>');

    var dlg = $("#modalErrorDialog")
        .html(message)
        .dialog({
            autoOpen: false, // set this to false so we can manually open it
            closeOnEscape: true,
            modal: true,
            title: 'Erro',
            close: function () {
                if (callbackClose && typeof (callbackClose) === "function") {
                    if (callbackArgument == null) {
                        callbackClose();
                    } else {
                        callbackClose(callbackArgument);
                    }
                }
            },
            buttons: {
                OK: function () {
                    $(this).dialog("close");
                }
            }
        });
    dlg.dialog("open");
};