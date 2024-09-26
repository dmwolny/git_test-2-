// call.js
"use strict";

const conn = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
const group = "maint" + document.getElementById("Maintenance_Shift").value + document.getElementById("Maintenance_Date");

conn.on("ReceiveCall", function (input, message) {
    document.getElementById(input).value = message;
});
conn.start().then(() => {
    conn.invoke("JoinCallGroup", group);
})

document.getElementById("Maintenance_Safety").addEventListener("input", (event) => {
    sendCall(event);
});

document.getElementById("Maintenance_Quality").addEventListener("input", (event) => {
    sendCall(event);
});

document.getElementById("Maintenance_Delivery").addEventListener("input", (event) => {
    sendCall(event);
});

document.getElementById("Maintenance_Cost").addEventListener("input", (event) => {
    sendCall(event);
});

document.getElementById("Maintenance_Morale").addEventListener("input", (event) => {
    sendCall(event);
});

async function sendCall(event) {
    event.preventDefault();
    var input = event.target.id;
    var message = document.getElementById(input).value;
    var form = document.getElementById("form");
    // Grab the date and format it to razor standards
    var dateOnly = new Date(form.Maintenance_Date.value).toISOString().slice(0, 10);
    // Grab all data from the form
    const data = {
        Id: form.Maintenance_Id.value,
        Shift: form.Maintenance_Shift.value,
        Date: dateOnly,
        Safety: form.Maintenance_Safety.value,
        Quality: form.Maintenance_Quality.value,
        Delivery: form.Maintenance_Delivery.value,
        Cost: form.Maintenance_Cost.value,
        Morale: form.Maintenance_Morale.value
    };
    // Send http request to submit notes to SendCall method.
    await fetch(`?handler=SendCall`, {
        method: "POST",
        body: JSON.stringify(data),
        headers:
        {
            RequestVerificationToken: document.getElementsByName("__RequestVerificationToken")[0].value,
            'Content-Type': 'application/json',
            Accept: 'application/json'
        }
    }).then(response => response.json())
        .then(data => console.log(data))
        .then(() => conn.invoke("NewCallReceived", input, message, group))
        .catch(function (err) {
            return console.error(err.toString());
        });

}