function getCookieValue(name) {
    const cookies = document.cookie.split(';');
    for (let cookie of cookies) {
        let [key, value] = cookie.trim().split('=');
        if (key === name) return decodeURIComponent(value);
    }
    return null;
}




document.addEventListener("DOMContentLoaded", function () {
        const token = getCookieValue("JwtToken");
    fetch("http://192.168.1.104:5001/api/FotografEtiketle/GetStatsForAdmin", {
            method: "GET",
            headers: {
                "Authorization": `Bearer ${token}`,
                "Content-Type": "application/json"
            }
        })
        .then(response => {
            if (!response.ok) throw new Error("İstek başarısız");
            return response.json();
        })
        .then(data => {
            const tbody = document.querySelector("#istatistikTablosu tbody");

            const istatistikler = data.data || data;

            istatistikler.forEach(item => {
                const tr = document.createElement("tr");

                tr.innerHTML = `
                            <td>${item.email}</td>
                            <td>${item.atananFotoSayisi}</td>
                            <td>${item.etiketlenenFotoSayisi}</td>
                            <td>${item.bekleyenFotoSayisi}</td>
                        `;

                tbody.appendChild(tr);
            });
        })
        .catch(error => {
            console.error("Hata oluştu:", error);
            alert("İstatistikler yüklenemedi.");
        });
});