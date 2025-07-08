let currentPhotoList = [];

export function loadPhotos(photoList) {
    currentPhotoList = photoList
    renderPhotos();
}



function renderPhotos() {
    const photoContainer = document.querySelector(".photos-container");
    if (!photoContainer) return;

    photoContainer.innerHTML = "";

    const visiblePhotos = currentPhotoList;
    const captions = [];

    visiblePhotos.forEach((photo) => {
        const wrapper = document.createElement("div");
        wrapper.classList.add("photo-wrapper");

        const img = document.createElement("img");
        img.src = photo.path;

        const caption = document.createElement("div");
        caption.classList.add("photo-caption");

        let captionText = "";

        if (photo.view_position === 1) {
            captionText = photo.laterality_id === 1 ? "R - CC" : "L - CC";
        } else if (photo.view_position === 2) {
            captionText = photo.laterality_id === 1 ? "R - MLO" : "L - MLO";
        }
        caption.textContent = captionText;
        captions.push(captionText);

        wrapper.appendChild(img);
        wrapper.appendChild(caption);
        photoContainer.appendChild(wrapper);
    });
}
