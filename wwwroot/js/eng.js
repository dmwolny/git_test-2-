// call.js
"use strict";

const conn = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
const group = "eng" + document.getElementById("Engineering_Shift").value + document.getElementById("Engineering_Date").value;

conn.on("ReceiveCall", function (input, message) {
    document.getElementById(input).value = message;
});
conn.on("DeleteImage",  function (filename) {
    removeCard(filename);
})
conn.on("AddImage", function (id, filename) {
    console.log("add picture");
    addImage(id, filename);
})
conn.start().then(() => {
    conn.invoke("JoinCallGroup", group);
})

document.getElementById("Engineering_Safety").addEventListener("input", (event) => {
    sendCall(event);
});

document.getElementById("Engineering_Quality").addEventListener("input", (event) => {
    sendCall(event);
});

document.getElementById("Engineering_Delivery").addEventListener("input", (event) => {
    sendCall(event);
});

document.getElementById("Engineering_Cost").addEventListener("input", (event) => {
    sendCall(event);
});

document.getElementById("Engineering_Morale").addEventListener("input", (event) => {
    sendCall(event);
});

async function deleteImage(id, fileName) {

    await fetch("?handler=Delete&id=" + encodeURIComponent(id) + "&fileName=" + encodeURIComponent(fileName), {
        method: "POST",
        headers:
        {
            "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
        }
    }).then(response => response.json())
        .then(message => console.log(message))
        .then(() => conn.invoke("ToDeleteImage", parseInt(id,10), fileName, group))
        .catch(function (err) {
            return console.error(err.toString());
        });;
}
async function sendCall(event) {
    event.preventDefault();
    var input = event.target.id;
    var message = document.getElementById(input).value;
    var form = document.getElementById("form");
    // Grab the date and format it to razor standards
    var dateOnly = new Date(form.Engineering_Date.value).toISOString().slice(0, 10);
    // Grab all data from the form
    const data = {
        Id: form.Engineering_Id.value,
        Shift: form.Engineering_Shift.value,
        Date: dateOnly,
        Safety: form.Engineering_Safety.value,
        Quality: form.Engineering_Quality.value,
        Delivery: form.Engineering_Delivery.value,
        Cost: form.Engineering_Cost.value,
        Morale: form.Engineering_Morale.value
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

function removeCard(filename) {
    const card = document.querySelector('.col-lg-4[data-id="' + filename + '"]');
    console.log(group);
    if (card) {
        card.remove();
    }
}
function addImage(id, filename) {
    const container = document.querySelector('.row');

    //Create new elements
    const ColDiv = document.createElement('div');
    ColDiv.classList.add('col-lg-4');
    ColDiv.setAttribute('data-id', filename);

    const cardDiv = document.createElement('div');
    cardDiv.classList.add('card');
    cardDiv.classList.add('mb-2');

    const img = document.createElement('img');
    img.src = '/images/Shift Notes/' + filename;
    img.classList.add('card-img-top');

    const cardBodyDiv = document.createElement('div');
    cardBodyDiv.classList.add('card-body');

    const deleteBtn = document.createElement('a');
    deleteBtn.classList.add('btn', 'btn-danger');
    deleteBtn.href = '#!';
    deleteBtn.textContent = 'Delete';
    deleteBtn.setAttribute('filename', filename);
    deleteBtn.setAttribute('data-id', id)

    // Append everything together
    cardBodyDiv.appendChild(deleteBtn);
    cardDiv.appendChild(img);
    cardDiv.appendChild(cardBodyDiv);
    ColDiv.appendChild(cardDiv);
    container.appendChild(ColDiv);
}