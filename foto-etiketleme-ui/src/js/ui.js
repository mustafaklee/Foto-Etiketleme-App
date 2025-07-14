// ui.js
export function showPhotos(photoList) {
    const container = document.querySelector(".photos-container");
    container.innerHTML = "";

    photoList.forEach(p => {
        const wrapper = document.createElement("div");
        wrapper.className = "photo-wrapper";

        const img = document.createElement("img");
        img.src = p.path;
        img.onclick = () => toggleImageSize(img);

        const caption = document.createElement("div");
        caption.className = "photo-caption";
        caption.textContent = "📷 Pozisyon: Otomatik"; // örnek

        wrapper.appendChild(img);
        wrapper.appendChild(caption);
        container.appendChild(wrapper);
    });
}
