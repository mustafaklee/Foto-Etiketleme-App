export function resetAnnotations() {
    const checkboxes = document.querySelectorAll("#findingCheckboxes input[type=checkbox]");
    checkboxes.forEach(cb => cb.checked = false);

    const biradsSelect = document.getElementById("biradsSelect");
    if (biradsSelect) biradsSelect.value = "";
}

export function getSelectedFindings() {
    const selected1 = Array.from(document.querySelectorAll("#findingCheckboxes1 input:checked")).map(el => el.value);
    const selected2 = Array.from(document.querySelectorAll("#findingCheckboxes2 input:checked")).map(el => el.value);

    return {
        sagMeme: selected1,
        solMeme: selected2
    };
}



export function getSelectedBirads() {
    const select = document.getElementById("biradsSelect");
    return select ? select.value : "";
}
