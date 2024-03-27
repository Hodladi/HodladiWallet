let currentHtml5QrCode = null;

function startQrScanner(dotNetReference, preferFrontCamera = false) {
    const config = { fps: 10, qrbox: { width: 250, height: 250 } };
    const facingMode = preferFrontCamera ? "user" : "environment";

    currentHtml5QrCode = new Html5Qrcode("reader");
    currentHtml5QrCode.start(
            { facingMode: facingMode },
            config,
            (decodedText, decodedResult) => {
                console.log(`Code matched = ${decodedText}`, decodedResult);
                dotNetReference.invokeMethodAsync('ReceiveQrCodeData', decodedText)
                    .catch(error => console.error('Error calling back into Blazor', error));
                stopQrScanner();
            })
        .catch(err => {
            console.error(`Unable to start scanning. Reason: ${err}`);
        });
}

function stopQrScanner() {
    if (currentHtml5QrCode) {
        currentHtml5QrCode.stop().then(() => {
            console.log("Scanning stopped.");
            currentHtml5QrCode = null;
        }).catch(err => {
            console.error("Failed to stop scanning", err);
        });
    }
}


                    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/notificationhub")
        .build();

    connection.on("ReceivePayment", function (payment) {
        alert(`Payment Received: ${payment.amountSat} sats`);
        window.location.href = "/";
    });

    connection.start().catch(function (err) {
        return console.error(err.toString());
    });