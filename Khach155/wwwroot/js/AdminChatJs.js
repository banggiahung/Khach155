"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
    connection.invoke("HistoryChatAdmin").catch(function (err) {
        console.error(err.toString());
    });
}).catch(function (err) {
    return console.error(err.toString());
});

//in lịch sử
connection.on("ReceiveMessageHistoryAdmin", function (history) {
    console.log(history)
    history.forEach(item => {
        let parrentDiv = document.createElement("div");
        parrentDiv.classList.add("col-2");
        parrentDiv.style.paddingRight = 20;
        let ul = document.createElement("ul");
        item.forEach(item2 => {
            let li = document.createElement("li");
            ul.appendChild(li);
            li.style.color = 'blue';
            if (item2.fromUser == 9) {
                li.style.color = 'red';
                li.style.textAlign = 'right'
            }
            li.textContent = `${item2.fromUser} says ${item2.content}`;
        })
        parrentDiv.appendChild(ul);
        document.getElementById("container2").appendChild(parrentDiv);
    })
});
// lấy lịch sử chát với người dùng

connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    li.style.color = 'blue';
    li.textContent = `${user} says ${message}`;
});

connection.on("ReceiveMessageAdmin", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${user} says ${message}`;
});


document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessageAdmin", "doanhuyxh2@gmail.com", message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});