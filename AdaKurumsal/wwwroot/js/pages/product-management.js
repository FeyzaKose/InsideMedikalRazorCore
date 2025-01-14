var layoutConnection = new signalR.HubConnectionBuilder()
    .withUrl("/productManagementHub")
    .withAutomaticReconnect()
    .build();

// Kullanıcının dil tercihi (örneğin data-language attribute'undan alınabilir)
const currentLanguage = document.documentElement.getAttribute('data-language') || 'tr';

layoutConnection.on("CategoriesUpdated", function (data) {
    console.log(`Kategoriler güncellendi - Dil: ${data.language}, Güncelleyen: ${data.updatedBy}, Zaman: ${data.updatedAt}`);

    // Eğer gelen güncelleme bizim dilimiz için ise veya tüm diller için ise sayfayı yenile
    if (data.language === 'all' || data.language === currentLanguage) {
        location.reload();
    }
});

layoutConnection.start()
    .then(function () {
        // Dil grubuna katıl
        return layoutConnection.invoke("JoinLanguageGroup", currentLanguage);
    })
    .catch(function (err) {
        console.error("SignalR Bağlantı Hatası:", err);
    });