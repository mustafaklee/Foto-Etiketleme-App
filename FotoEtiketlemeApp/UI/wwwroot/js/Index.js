function getCookieValue(name) {
    const cookies = document.cookie.split(';');
    for (let cookie of cookies) {
        let [key, value] = cookie.trim().split('=');
        if (key === name) return decodeURIComponent(value);
    }
    return null;
}



document.addEventListener("DOMContentLoaded", () => {
    const token = getCookieValue("JwtToken");

    fetch("https://localhost:7252/api/FotografEtiketle/GetStats", {
        method: "GET",
        headers: {
            "Authorization": `Bearer ${token}`,
            "Content-Type": "application/json"
        }
    })
        .then(res => res.json())
        .then(data => {
            if (data.success) {
                document.getElementById("email").textContent = data.data.email;
                document.getElementById("etiketlenmis").textContent = data.data.etiketlenmis;
                document.getElementById("bekleyen").textContent = data.data.bekleyen;
                document.getElementById("toplam").textContent = data.data.etiketlenmis + data.data.bekleyen;

            } else {
                console.warn("İstatistik alınamadı:", data.message);
            }
        })
        .catch(err => {
            console.error("İstek hatası:", err);
        });
});
