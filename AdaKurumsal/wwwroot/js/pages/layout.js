var layoutConnection = new signalR.HubConnectionBuilder()
    .withUrl("/layoutHub")
    .withAutomaticReconnect()
    .build();

layoutConnection.on("LayoutUpdated", function () {
    location.reload();
});

layoutConnection.start()
    .catch(function (err) {
        console.error("SignalR Bağlantı Hatası:", err);
    });