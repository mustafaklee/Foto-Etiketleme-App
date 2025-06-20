// api.js
export async function getFotos() {
    const response = await fetch("https://localhost:7252/api/FotografEtiketle/GetFoto");
    return await response.json();
}

export async function getEtiketler() {
    const response = await fetch("https://localhost:7252/api/FotografEtiketle/GetBreastAndFinding");
    return await response.json();
}
