export async function getFotos(page = 1, pageSize = 20) {
    const token = localStorage.getItem('jwtToken');
    const response = await fetch(`https://localhost:7252/api/FotografEtiketle/GetFoto?page=${page}&pageSize=${pageSize}`,
        {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });
    return await response.json();
}

export async function getLabeledFolders() {
    const token = localStorage.getItem('jwtToken');
    const response = await fetch(`https://localhost:7252/api/FotografEtiketle/GetLabeledFolders`,
        {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });
    return await response.json();
}



export async function GetLabeledFotos(folderId) {
    const token = localStorage.getItem('jwtToken');
    const response = await fetch(`https://localhost:7252/api/FotografEtiketle/GetLabeledImages?folderId=${folderId}`,
        {
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

export async function getStats() {
    const token = localStorage.getItem('jwtToken');
    const response = await fetch("https://localhost:7252/api/FotografEtiketle/getStats", {
        headers: {
            'Authorization': `Bearer ${token}`
        }
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
