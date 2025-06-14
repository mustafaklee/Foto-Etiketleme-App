let currentIndex = 0;
let etiketSecimleri = []; // { fotografId, etiketId }

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

    document.getElementById("saveBtn").addEventListener("click", () => {
        const msgBox = document.getElementById("resultMessage");
        if (etiketSecimleri.length === 0) {
            msgBox.textContent = "Lutfen en az bir fotograf icin etiket secin.";
            msgBox.classList.remove("d-none", "alert-success");
            msgBox.classList.add("alert-danger");
            return;
        }

        fetch("https://localhost:7252/api/FotografEtiketle/PostFoto", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
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
