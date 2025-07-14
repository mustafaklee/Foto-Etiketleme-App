// main.js
import { initializeTree } from "./tree.js";
import { loadPhotos } from "./photoViewer.js";
import { getFotos, getEtiketler } from "./api.js";

let currentFolders = [];

document.addEventListener("DOMContentLoaded", async () => {
    try {
        // 📥 Fotoğraf ve etiket verilerini al
        const fotoResponse = await getFotos();
        const etiketResponse = await getEtiketler();
        const findingList = etiketResponse.data?.findingCategories || [];
        const biradsList = etiketResponse.data?.breastBirads || [];

        fillEtiketControls(findingList, biradsList);
        const folders = fotoResponse.data || [];
        currentFolders = folders;

        // 📂 Ağaç görünümünü başlat
        initializeTree(folders, loadPhotos);

        // 🖼️ İlk klasör otomatik yüklensin
        if (folders.length > 0 && folders[0].fotograflar.length > 0) {
            loadPhotos(folders[0].fotograflar);
        }

        console.log("✅ JS işlemleri tamamlandı.");
    } catch (error) {
        console.error("🚨 Hata oluştu:", error);
    }
});
function fillEtiketControls(findingList, biradsList) {
    const checkboxDiv1 = document.getElementById("findingCheckboxes1");
    const checkboxDiv2 = document.getElementById("findingCheckboxes2");
    const biradsSelect = document.getElementById("biradsSelect");

    if (checkboxDiv1 && checkboxDiv2) {
        checkboxDiv1.innerHTML = "";
        checkboxDiv2.innerHTML = "";

        findingList.forEach(item => {
            const label1 = document.createElement("label");
            label1.innerHTML = `<input type="checkbox" value="${item.id}" /> ${item.etiketAd}`;
            label1.style.display = "block";
            checkboxDiv1.appendChild(label1);

            const label2 = document.createElement("label");
            label2.innerHTML = `<input type="checkbox" value="${item.id}" /> ${item.etiketAd}`;
            label2.style.display = "block";
            checkboxDiv2.appendChild(label2);
        });
    }

    if (biradsSelect) {
        biradsSelect.innerHTML = '<option value="">Seçiniz</option>';
        biradsList.forEach(item => {
            const option = document.createElement("option");
            option.value = item.id;
            option.textContent = item.etiketAd;
            biradsSelect.appendChild(option);
        });
    }
}

