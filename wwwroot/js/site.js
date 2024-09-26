// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function getBase64(file) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = () => resolve(reader.result);
        reader.onerror = error => reject(error);
    }
    );
}

function getBase64FromImgSrc(imgSrc) {
    return new Promise((resolve, reject) => {
        const img = new Image();
        img.crossOrigin = "anonymous";
        // For CORS-enabled images
        img.onload = () => {
            const canvas = document.createElement("canvas");
            const ctx = canvas.getContext("2d");
            canvas.width = img.width;
            canvas.height = img.height;
            ctx.drawImage(img, 0, 0);
            const dataURL = canvas.toDataURL();
            resolve(dataURL);
        }
            ;
        img.onerror = reject;
        img.src = imgSrc;
    }
    );
}

function generatePostIt() {
    const myHeaders = new Headers();
    myHeaders.append("Content-Type", "application/json");

    const raw = JSON.stringify({
        "id": 2,
        "model": "MQ",
        "vin": "500001",
        "date": "9/20/2024",
        "location": "BSR",
        "originator": "Jenny",
        "title": "unit leader",
        "shift": "A",
        "dept": 200,
        "zone": 02,
        "station": "Tailcans",
        "description": "Water leak due to missing sealer near tailcan",
        "NOK": "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"
    });

    const requestOptions = {
        method: "POST",
        headers: myHeaders,
        body: raw,
        redirect: "follow"
    };

    fetch("https://prod-237.westeurope.logic.azure.com:443/workflows/aee1ea17fc224aa1ad80f8c1bca63bc5/triggers/manual/paths/invoke?api-version=2016-06-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=v9Vv7ZgX3liE2nnTzMqFHGXUIJFErnX1AL5SMvarIzs", requestOptions).then((response) => response.text()).then((result) => {
        console.log([result]);
        let blob = decodeBase64([result]);
        download(blob);
    }
    ).then(() => {
        document.getElementById("spinner2").hidden = true;
        document.getElementById("postit").disabled = false;
}).catch((error) => console.error(error));
}

function decodeBase64(base64) {
    // Decode base64 string to ArrayBuffer   
    const byteCharacters = atob(base64);
    const byteNumbers = new Array(byteCharacters.length);
    for (let i = 0; i < byteCharacters.length; i++) {
        byteNumbers[i] = byteCharacters.charCodeAt(i);
    }
    const byteArray = new Uint8Array(byteNumbers);

    // Create a Blob from ArrayBuffer
    const blob = new Blob([byteArray], {
        type: 'application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet',
    });

    return blob;
}

function download(file) {
    // Create a temporary anchor element
    const link = document.createElement('a');
    link.href = window.URL.createObjectURL(file);
    link.download = 'downloaded_template.xlsx';
    // File name
    document.body.appendChild(link);

    // Programmatically click the link to trigger the download
    link.click();

    // Clean up
    document.body.removeChild(link);
    window.URL.revokeObjectURL(link.href);
}

function displayToast(message) {
    document.getElementById('message').innerHTML = message;
    const genericToast = document.getElementById('generic')

    const toastBootstrap = bootstrap.Toast.getOrCreateInstance(genericToast);
    toastBootstrap.show();
}