export async function getFotos() {
    const token = localStorage.getItem('jwtToken');
    const response = await fetch("https://localhost:7252/api/FotografEtiketle/GetFoto", {
        headers: {
            'Authorization': `Bearer ${token}`
        }
    });
    return await response.json();
}

export async function getEtiketler() {
    const token = localStorage.getItem('jwtToken');
    const response = await fetch("https://localhost:7252/api/FotografEtiketle/GetBreastAndFinding", {
        headers: {
            'Authorization': `Bearer ${token}`
        }
    });
    return await response.json();
}

export async function postEtiketler(data) {
    const token = localStorage.getItem('jwtToken');
    console.log("Veritabanına gönderilecek etiket verisi:", data);
    const response = await fetch("https://localhost:7252/api/FotografEtiketle/PostFoto", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(data)
    });
    return await response.json();
}

export async function loginUser(credentials) {
    const response = await fetch("https://localhost:7252/api/Auth/Login", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(credentials)
    });
    if (!response.ok) {
        throw new Error("Giriş başarısız");
    }

    return await response.json();
}
