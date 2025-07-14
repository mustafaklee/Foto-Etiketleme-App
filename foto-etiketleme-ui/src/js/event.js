// events.js
export function setupNavButtons(onPrev, onNext) {
    document.getElementById("prevBtn").addEventListener("click", onPrev);
    document.getElementById("nextBtn").addEventListener("click", onNext);
}
