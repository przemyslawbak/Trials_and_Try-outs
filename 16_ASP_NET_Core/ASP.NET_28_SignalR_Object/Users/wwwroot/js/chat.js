"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (message) {
    console.log('receiving');
    var msg = message.someMessage.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = message.someName + " says " + msg + ", is? " + message.isSomething + ", number: " + message.someNumber + ", array: " + message.someArray;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    console.log('sending');
    var someName = document.getElementById("userInput").value;
    var someMessage = document.getElementById("messageInput").value;
    var isSomething = true;
    var someNumber = 69;
    var someArray = ["one", "two"];
    connection.invoke("SendMessage", { someName, someMessage, isSomething, someNumber, someArray }).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});