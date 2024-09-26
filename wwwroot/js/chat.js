"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (input, message) {
    document.getElementById(input).value = message;
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("safety").addEventListener("input", function (event) {
    var input = event.target.id;
    var message = document.getElementById(input).value;
    connection.invoke("SendMessage", input, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});
document.getElementById("quality").addEventListener("input", function (event) {
    var input = event.target.id;
    var message = document.getElementById(input).value;
    connection.invoke("SendMessage", input, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});
document.getElementById("delivery").addEventListener("input", function (event) {
    var input = event.target.id;
    var message = document.getElementById(input).value;
    connection.invoke("SendMessage", input, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});
document.getElementById("cost").addEventListener("input", function (event) {
    var input = event.target.id;
    var message = document.getElementById(input).value;
    connection.invoke("SendMessage", input, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});
document.getElementById("morale").addEventListener("input", function (event) {
    var input = event.target.id;
    var message = document.getElementById(input).value;
    connection.invoke("SendMessage", input, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});
