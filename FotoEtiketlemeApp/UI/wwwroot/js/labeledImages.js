
    let fotograflar = [];
    let etiketler = [];
    let currentIndex = 0;
    let etiketSecimleri = [];
    let hasEtiket = [];
function updatePhoto() {
    const photo = fotograflar[currentIndex];
    if (!photo) return;

    const photoEl = document.getElementById("photo");
    const fotoIndexText = document.getElementById("fotoIndex");

    photoEl.src = photo.path;
    photoEl.dataset.id = photo.id;
    fotoIndexText.textContent = `${currentIndex + 1}. görsel`;

    // Seçili etiketi işaretle (isteğe bağlı)
    const aktifEtiketId = hasEtiket?.[currentIndex]?.id;
    document.querySelectorAll(".etiket-btn button").forEach(btn => {
        btn.classList.remove("selected"); // önce temizle
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
    // 1. Fotoğrafı göster
    if (fotograflar.length === 0) {
        document.getElementById("photo").src = "";
        document.getElementById("fotoIndex").textContent = "Görsel bulunamadı.";
        return;
    }

    // 2. Etiket butonlarını temizle + yeniden oluştur
    const etiketContainer = document.querySelector(".etiket-btn");
    etiketContainer.innerHTML = ""; // temizle

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
            console.log(etiketSecimleri);
        });
        etiketContainer.appendChild(btn);
    });
    updatePhoto();
}


document.addEventListener("DOMContentLoaded", () => {

        // FILTRELE
        document.getElementById("filterBtn").addEventListener("click", () => {
            const start = document.getElementById("startDate").value;
            const end = document.getElementById("endDate").value;
            const messageEl = document.getElementById("responseMessage");
            if (!start || !end) {
                alert("Tarihleri seçiniz.");
                return;
            }
            fetch(`https://localhost:7252/api/FotografEtiketle/GetFotoByDate?startDate=${start}&endDate=${end}`)
                .then(res => res.json())
                .then(data => {
                    if (data.success) {
                        loadPhotoUI(data.data); // API'den dönen DTO
                        if (data.success) {
                            messageEl.textContent = data.message;
                            messageEl.className = "alert alert-success text-center fw-bold";
                        } else {
                            messageEl.textContent = data.message;
                            messageEl.className = "alert alert-danger text-center fw-bold";
                        }
                    } else {
                        messageEl.textContent = data.message;
                        messageEl.style.color = "red";
                    }
                })
                .catch(() => alert("Veri alınırken hata oluştu."));
        });

    // ETIKETLEME
    document.querySelectorAll(".etiket-btn button").forEach(btn => {
        btn.addEventListener("click", () => {
            const etiketId = parseInt(btn.dataset.id);
            const fotografId = parseInt(document.getElementById("photo").dataset.id);

            etiketSecimleri = etiketSecimleri.filter(x => x.fotografId !== fotografId);
            etiketSecimleri.push({ fotografId, etiketId });
            console.log(etiketSecimleri);
        });
    });

    // NAVIGATION
    document.getElementById("btn-next").addEventListener("click", () => {
        if (currentIndex < fotograflar.length - 1) {
        currentIndex++;
    updatePhoto();  
        }
    });

    document.getElementById("btn-prev").addEventListener("click", () => {
        if (currentIndex > 0) {
        currentIndex--;
    updatePhoto();
        }
    });

    // KAYDET
    document.getElementById("saveBtn").addEventListener("click", () => {
        const msgBox = document.getElementById("responseMessage");

        if (etiketSecimleri.length === 0) {
            msgBox.textContent = "Lütfen en az bir fotoğraf için etiket seçin.";
            msgBox.className = "alert alert-danger text-center fw-bold";
            return;
        }

        fetch("https://localhost:7252/api/FotografEtiketle/PostFoto", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(etiketSecimleri)
        })
            .then(res => res.json())
            .then(json => {
                msgBox.textContent = json.message;
                msgBox.className = json.success
                    ? "alert alert-success text-center fw-bold"
                    : "alert alert-danger text-center fw-bold";
            })
            .catch(() => {
                msgBox.textContent = "Sunucu hatası oluştu.";
                msgBox.className = "alert alert-danger text-center fw-bold";
            });
    });

});
