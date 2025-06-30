// api.js
export async function getFotos() {
    const response = await fetch("https://localhost:7252/api/FotografEtiketle/GetFoto");
    return await response.json();
}

export async function getEtiketler() {
    const response = await fetch("https://localhost:7252/api/FotografEtiketle/GetBreastAndFinding");
    return await response.json();
}

// Tüm klasörlerin etiketleme verisini veritabanına gönder
export async function postEtiketler(data) {
    console.log("Veritabanına gönderilecek etiket verisi:", data);
    const response = await fetch("https://localhost:7252/api/FotografEtiketle/PostFoto", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(data)
    });
    return await response.json();
}
