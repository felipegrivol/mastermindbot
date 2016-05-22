/// <reference path="jquery-1.10.2.min.js" />
/// <reference path="jquery.signalR-2.2.0.min.js" />

$(function () {
    var chat = $.connection.mastermindHub;

    chat.client.postHistory = function (title, message) {
        var containerName = $('<span/>').text(title).html();
        var containerMessage = $('<div/>').text(message).html();

        $("#history").append(
            '<li><strong>' + containerName + '</strong>: '
                + containerMessage + '</li>');
    };

    $.connection.hub.start().done(function () {

        $("#start").click(function () {
            var name = $("#name").val();

            chat.server.startGame(name);
        });

        $("#send").click(function () {
            var guess = $("#message").val();

            chat.server.sendGuess(guess);

            $("#message").val('');
        });

        $("#bot").click(function () {
            chat.server.botGuess();
        });

    });

});