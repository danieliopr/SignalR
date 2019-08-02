"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/ChatHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " says " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});

connection.on("SendMessageClientConnectionId", function (connectionId) {
    var x = document.getElementById("myList");
    var option = document.createElement("option");
    option.text = connectionId;
    x.add(option);
});

connection.start().then(function(){
    document.getElementById("sendButton").disabled = false;
    var hub = connection ;
    var connectionUrl = hub["connection"].transport.webSocket.url ;
    
    var connectionId = connectionUrl.split("/ChatHub?id=");

    connection.invoke("SendMessageClientConnectionId", connectionId[1]).catch(function (err) {
        console.log("Connection é: " + $.connection.hub.id);
        return console.error(err.toString());
    }).catch(function (err) {
    console.log(connectionUrl);
        return console.error(err.toString());
    });
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    // var connectionId = document.getElementById('myList').value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("sendButtonIndividual").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    var connectionId = document.getElementById("myList").value;
    connection.invoke("SendMessageClient",connectionId, user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("reiniciarControlador").addEventListener("click", function (event) {
    var connectionId = document.getElementById("idConnection").value;
    connection.invoke("ReiniciarControlador", connectionId).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});
document.getElementById("iniciarJogo").addEventListener("click", function (event) {
    var connectionId = document.getElementById("idConnection").value;
    connection.invoke("IniciarJogo", connectionId, "1").catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("download").addEventListener("click", function (event) {
    var connectionId = document.getElementById("idConnection").value;
    connection.invoke("Download", connectionId).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});