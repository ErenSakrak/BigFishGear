function showNotification(message) {
    const notification = document.getElementById("notification");
    notification.textContent = message;
    notification.style.display = "block";
    notification.style.opacity = 1;

    setTimeout(() => {
        notification.style.opacity = 0;
        setTimeout(() => {
            notification.style.display = "none";
        }, 300);
    }, 2000); // Bildirim 2 saniye boyunca gösterilecek
}
function addToCart(productId) {
    fetch(`/Cart/Add?productId=${productId}`)
        .then(response => {
            if (response.ok) {
                showNotification("Ürün sepete başarıyla eklendi.");
            } else {
                console.error("Ürün sepete eklenemedi.");
            }
        })
        .catch(error => console.error("Hata:", error));
}

function removeFromCart(productId, rowElement) {
    fetch(`/Cart/Remove?productId=${productId}`, { method: 'POST' })
        .then(response => {
            if (response.ok) {
                showNotification("Ürün sepetten başarıyla silindi.");
                // Silinen ürünü DOM'dan kaldır
                rowElement.remove(); // Silinen ürünün satırını kaldır
            } else {
                console.error("Ürün sepetten silinemedi.");
            }
        })
        .catch(error => console.error("Hata:", error));
}