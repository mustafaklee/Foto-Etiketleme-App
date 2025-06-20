let currentPhotoList = [];
let currentIndex = 0;

export function loadPhotos(photoList) {
    currentPhotoList = photoList;
    currentIndex = 0;
    renderPhotos();
}


export function syncMenuLabels(captions = []) {
    const label1 = document.getElementById("msLabel1");
    const label2 = document.getElementById("msLabel2");

    label1.innerHTML = `<strong>${captions[0] || "Etiket 1"}</strong>`;
    label2.innerHTML = `<strong>${captions[1] || "Etiket 2"}</strong>`;
}

function renderPhotos() {
    const photoContainer = document.querySelector(".photos-container");
    if (!photoContainer) return;

    photoContainer.innerHTML = "";

    const visiblePhotos = currentPhotoList.slice(currentIndex, currentIndex + 2);
    const captions = [];

    visiblePhotos.forEach((photo, index) => {
        const wrapper = document.createElement("div");
        wrapper.classList.add("photo-wrapper");

        const img = document.createElement("img");
        img.src = photo.path;
        img.onclick = () => toggleImageSize(img);

        const caption = document.createElement("div");
        caption.classList.add("photo-caption");

        const absoluteIndex = currentIndex + index;
        let captionText = "";

        if (absoluteIndex === 0) captionText = "R - CC";
        else if (absoluteIndex === 1) captionText = "R - MLO";
        else if (absoluteIndex === 2) captionText = "L - CC";
        else if (absoluteIndex === 3) captionText = "L - MLO";

        caption.textContent = captionText;
        captions.push(captionText);

        wrapper.appendChild(img);
        wrapper.appendChild(caption);
        photoContainer.appendChild(wrapper);
    });

    syncMenuLabels(captions); // ✅ Burada çağır
}

export function setupNavigation({ onNext }) {
    document.getElementById("nextBtn").addEventListener("click", () => {
        onNext?.();
        if (currentIndex + 2 < currentPhotoList.length) {
            currentIndex += 2;
            renderPhotos();
        }
    });

    document.getElementById("prevBtn").addEventListener("click", () => {
        if (currentIndex - 2 >= 0) {
            currentIndex -= 2;
            renderPhotos();
        }
    });
}

export function toggleImageSize(img) {
    img.classList.toggle("zoomed");
}