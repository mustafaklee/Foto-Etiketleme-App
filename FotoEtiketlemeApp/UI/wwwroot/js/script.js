let currentIndex = 0;
let etiketSecimleri = []; // { fotografId, etiketId }

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
    if (fotograflar[currentIndex]) {
        const f = fotograflar[currentIndex];
        const photoEl = document.getElementById("photo");
        const fotoIndexText = document.getElementById("fotoIndex");

        photoEl.src = f.path;
        photoEl.dataset.id = f.id;
        fotoIndexText.textContent = `${currentIndex + 1}. görsel`;
    }
}

document.addEventListener("DOMContentLoaded", () => {
    updatePhoto();

    document.querySelectorAll(".etiket-btn").forEach(btn => {
        btn.addEventListener("click", () => {
            const etiketId = parseInt(btn.dataset.id);
            const fotografId = parseInt(document.getElementById("photo").dataset.id);

            etiketSecimleri = etiketSecimleri.filter(x => x.fotografId !== fotografId);
            etiketSecimleri.push({ fotografId, etiketId });
            console.log(etiketSecimleri);
        });
    });

    document.getElementById("saveBtn").addEventListener("click", () => {
        const msgBox = document.getElementById("resultMessage");
        if (etiketSecimleri.length === 0) {
            msgBox.textContent = "Lutfen en az bir fotograf icin etiket secin.";
            msgBox.classList.remove("d-none", "alert-success");
            msgBox.classList.add("alert-danger");
            return;
        }

        fetch("http://192.168.1.104:5001/api/FotografEtiketle/PostFoto", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`,
            },
            body: JSON.stringify(etiketSecimleri)
        })
            .then(res => res.json())
            .then(json => {
                msgBox.textContent = json.message;

                msgBox.classList.remove("d-none", "alert-success", "alert-danger");
                msgBox.classList.add(json.success ? "alert-success" : "alert-danger");
            })
            .catch(() => {
                const msgBox = document.getElementById("resultMessage");
                msgBox.textContent = "Sunucu hatasý oluþtu.";
                msgBox.classList.remove("d-none", "alert-success");
                msgBox.classList.add("alert-danger");
            });
    });


});
