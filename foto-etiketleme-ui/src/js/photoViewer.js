//photoViewer.js

let currentPhotoList = [];
let currentIndex = 0;

export function loadPhotos(photoList) {
    currentPhotoList = photoList;
    currentIndex = 0;
    renderPhotos();
}



function renderPhotos() {
    const photoContainer = document.querySelector(".photos-container");
    if (!photoContainer) return;

    photoContainer.innerHTML = "";

    const visiblePhotos = currentPhotoList;
    const captions = [];

    visiblePhotos.forEach((photo, index) => {
        const wrapper = document.createElement("div");
        wrapper.classList.add("photo-wrapper");

        const img = document.createElement("img");
        img.src = photo.path;

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
}
