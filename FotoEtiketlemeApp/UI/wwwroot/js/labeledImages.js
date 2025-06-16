let fotograflar = [];
let etiketler = [];
let currentIndex = 0;
let etiketSecimleri = [];
let hasEtiket = [];

function getCookieValue(name) {
    const cookies = document.cookie.split(';');
    for (let cookie of cookies) {
        let [key, value] = cookie.trim().split('=');
        if (key === name) return decodeURIComponent(value);
    }
    return null;
}

const token = getCookieValue("JwtToken");


function updatePhoto() {
    const photo = fotograflar[currentIndex];
    if (!photo) return;

    const photoEl = document.getElementById("photo");
    const fotoIndexText = document.getElementById("fotoIndex");

    photoEl.src = photo.path;
    photoEl.dataset.id = photo.id;
    fotoIndexText.textContent = `${currentIndex + 1}. görsel`;

    const aktifEtiketId = hasEtiket?.[currentIndex]?.id;
    document.querySelectorAll(".etiket-btn button").forEach(btn => {
        btn.classList.remove("selected");
        if (parseInt(btn.dataset.id) === aktifEtiketId) {
            btn.classList.add("selected");
        }
    });
}

function loadPhotoUI(data) {
    fotograflar = data.fotograflar;
    etiketler = data.etiketler;
    currentIndex = 0;
    etiketSecimleri = [];
    hasEtiket = data.hasEtiket ?? [];

    if (fotograflar.length === 0) {
        document.getElementById("photo").src = "";
        document.getElementById("fotoIndex").textContent = "Görsel bulunamadı.";
        return;
    }

    const etiketContainer = document.querySelector(".etiket-btn");
    etiketContainer.innerHTML = "";

    etiketler.forEach(etiket => {
        const btn = document.createElement("button");
        btn.type = "button";
        btn.textContent = etiket.etiketAd;
        btn.dataset.id = etiket.id;

        btn.addEventListener("click", () => {
            const etiketId = parseInt(btn.dataset.id);
            const fotografId = parseInt(document.getElementById("photo").dataset.id);

            etiketSecimleri = etiketSecimleri.filter(x => x.fotografId !== fotografId);
            etiketSecimleri.push({ fotografId, etiketId });
            updatePhoto();
        });

        etiketContainer.appendChild(btn);
    });

    updatePhoto();
}

document.addEventListener("DOMContentLoaded", () => {
    const messageEl = document.getElementById("responseMessage");

    // 📅 Filtreleme
    document.getElementById("filterBtn").addEventListener("click", () => {
        const start = document.getElementById("startDate").value;
        const end = document.getElementById("endDate").value;

        if (!start || !end) {
            alert("Tarihleri seçiniz.");
            return;
        }

        fetch(`https://localhost:7252/api/FotografEtiketle/GetFotoByDate?startDate=${start}&endDate=${end}`, {
            method: "GET",
            headers: {
                "Authorization": `Bearer ${token}`,
                "Content-Type": "application/json"
            }
        })
            .then(res => {
                if (!res.ok) throw new Error("API erişim hatası veya yetkisiz istek.");
                return res.json();
            })
            .then(data => {
                if (data.success) {
                    loadPhotoUI(data.data);
                    messageEl.textContent = data.message;
                    messageEl.className = "alert alert-success text-center fw-bold";
                } else {
                    messageEl.textContent = data.message;
                    messageEl.className = "alert alert-danger text-center fw-bold";
                }
            })
            .catch(err => {
                console.error(err);
                messageEl.textContent = "Veri alınırken hata oluştu.";
                messageEl.className = "alert alert-danger text-center fw-bold";
            });
    });

    // 🔁 Sonraki fotoğraf
    document.getElementById("btn-next").addEventListener("click", () => {
        if (currentIndex < fotograflar.length - 1) {
            currentIndex++;
            updatePhoto();
        }
    });

    // 🔁 Önceki fotoğraf
    document.getElementById("btn-prev").addEventListener("click", () => {
        if (currentIndex > 0) {
            currentIndex--;
            updatePhoto();
        }
    });

    // 💾 Kaydet
    document.getElementById("saveBtn").addEventListener("click", () => {
        if (etiketSecimleri.length === 0) {
            messageEl.textContent = "Lütfen en az bir fotoğraf için etiket seçin.";
            messageEl.className = "alert alert-danger text-center fw-bold";
            return;
        }

        fetch("https://localhost:7252/api/FotografEtiketle/PostFoto", {
            method: "POST",
            headers: {
                "Authorization": `Bearer ${token}`,
                "Content-Type": "application/json"
            },
            body: JSON.stringify(etiketSecimleri)
        })
            .then(res => res.json())
            .then(json => {
                messageEl.textContent = json.message;
                messageEl.className = json.success
                    ? "alert alert-success text-center fw-bold"
                    : "alert alert-danger text-center fw-bold";
            })
            .catch(() => {
                messageEl.textContent = "Sunucu hatası oluştu.";
                messageEl.className = "alert alert-danger text-center fw-bold";
            });
    });
});