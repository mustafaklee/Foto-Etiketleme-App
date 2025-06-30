<template>
  <div class="main-container">
    <!-- Sol Panel -->
    <div class="left-panel">
      <TreeView :treeData="folders" @selectFolder="onSelectFolder" />
    </div>

    <!-- Orta Panel (Fotoğraflar) -->
    <div class="middle-panel">
      <div class="photos-container">
        <!-- JS ile fotoğraflar yüklenecek -->
      </div>
    </div>

    <!-- Sağ Panel (Navigasyon ve Etiketler) -->
    <div class="right-panel">
      <div class="container mt-4">
        <!-- Sekmeler -->
        <ul class="nav nav-tabs" id="myTab" role="tablist">

          <li class="nav-item" role="presentation">
            <a 
              class="nav-link" 
              :class="{ active: activeTab === 'sagMeme' }" 
              id="sag-tab" 
              data-bs-toggle="tab" 
              href="#sagMeme" 
              role="tab" 
              aria-controls="sagMeme" 
              aria-selected="activeTab === 'sagMeme'"
              @click="activeTab = 'sagMeme'"
            >
              Sağ Meme
            </a>
          </li>
          <li class="nav-item" role="presentation">
            <a 
              class="nav-link" 
              :class="{ active: activeTab === 'solMeme' }" 
              id="sol-tab" 
              data-bs-toggle="tab" 
              href="#solMeme" 
              role="tab" 
              aria-controls="solMeme" 
              aria-selected="activeTab === 'solMeme'"
              @click="activeTab = 'solMeme'"
            >
              Sol Meme
            </a>
          </li>
        </ul>

        <!-- Tab içerikleri -->
        <div class="tab-content" id="myTabContent">
          <div class="tab-pane fade" :class="{ 'show active': activeTab === 'sagMeme' }" id="sagMeme" role="tabpanel" aria-labelledby="sag-tab">
            <div class="etiket-checkbox-group">
              <label v-for="item in findings" :key="item.id">
                <input
                  type="checkbox"
                  :value="item.id"
                  v-model="selectedFindings1"
                />
                {{ item.etiketAd }}
              </label>
            </div>

            <div class="mb-1" style="margin-top: 30%;">
              <label for="biradsSelect"><strong>BI-RADS</strong></label>
              <select id="biradsSelectSag" v-model="selectedBiradsSag">
                <option value="">Seçiniz</option>
                <option v-for="item in biradsList" :key="item.id" :value="item.id">
                  {{ item.etiketAd }}
                </option>
              </select>
            </div>

          </div>
          <div class="tab-pane fade" :class="{ 'show active': activeTab === 'solMeme' }" id="solMeme" role="tabpanel" aria-labelledby="sol-tab">
            <div class="etiket-checkbox-group">
              <label v-for="item in findings" :key="item.id">
                <input
                  type="checkbox"
                  :value="item.id"
                  v-model="selectedFindings2"
                />
                {{ item.etiketAd }}
              </label>
            </div>
            <div class="mb-1" style="margin-top: 30%;">
              <label for="biradsSelect"><strong>BI-RADS</strong></label>
              <select id="biradsSelectSol" v-model="selectedBiradsSol">
                <option value="">Seçiniz</option>
                <option v-for="item in biradsList" :key="item.id" :value="item.id">
                  {{ item.etiketAd }}
                </option>
              </select>
            </div>
          </div>
        </div>
      </div>
      <button @click="saveTags">Ekle</button>
      <button @click="veritabaninaGonder">Veritabanına Gönder</button>
    </div>

  </div>
</template>

<script>
import { ref, onMounted } from "vue";
import TreeView from "../components/TreeView.vue";
import { loadPhotos } from "../js/photoViewer";
import { getFotos, getEtiketler, postEtiketler } from "../js/api";
import { toRaw } from "vue";
export default {
  components: {
    TreeView,
  },
  setup() {
    const activeTab = ref("solMeme");
    const findings = ref([]);
    const biradsList = ref([]);
    const selectedFindings1 = ref([]);
    const selectedFindings2 = ref([]);
    const selectedBiradsSol = ref("");
    const selectedBiradsSag = ref("");
    const folders = ref([]);
    const etiketeHazirData = ref([]);



    let currentPhotoList = [];
    // Klasör bazlı etiketleme verisi
    const folderTagData = ref({});

    onMounted(async () => {
      const fotoResponse = await getFotos();
      const etiketResponse = await getEtiketler();

      findings.value = etiketResponse.data?.findingCategories || [];
      biradsList.value = etiketResponse.data?.breastBirads || [];

      // Kullanınabilir Ağaç Verisine Dönüştürülmesi
      folders.value = (fotoResponse.data || []).map((folder) => ({
        id: folder.folderId,
        name: folder.folderPath,
        expanded: false,
        children: (folder.fotograflar || []).map((photo) => ({
          id: photo.id,
          name: photo.path.split("/").pop(),
          photoUrl: photo.path,
          expanded: false,
          children: [],
        })),
      }));

      // İlk klasörde fotoğraf varsa, onları yükle
      if (
        folders.value.length > 0 &&
        folders.value[0].children &&
        folders.value[0].children.length > 0
      ) {
        const photoItems = folders.value[0].children.map((child) => ({
          id: child.id,
          path: child.photoUrl,
        }));
        currentPhotoList = photoItems; // currentPhotoList'e atama
        loadPhotos(photoItems);
      }
    });

    // Fotoğraf verileri ile kaydetme işlemi
    const saveTags = () => {
      const photosToSave = [];
      
      // Fotoğraflar sırasıyla (R-CC, R-MLO, L-CC, L-MLO) işlem yapıyoruz
      currentPhotoList.forEach((photo, index) => {
        let laterality = "";

        // Fotoğraf sırasına göre laterality (Sol veya Sağ meme) ve view_position (CC ya da MLO) ayarlıyoruz
        if (index === 0) {
          laterality = "R";  // Sağ meme
        } else if (index === 1) {
          laterality = "R";  // Sağ meme
        } else if (index === 2) {
          laterality = "L";  // Sol meme
        } else if (index === 3) {
          laterality = "L";  // Sol meme
        }

        // Sol ve sağ meme için doğru BI-RADS ve finding categories değerini seçiyoruz
        const biradsValue = laterality === "L" ? selectedBiradsSol.value : selectedBiradsSag.value;
        const findings = laterality === "L" ? selectedFindings2.value : selectedFindings1.value;

        // Her fotoğraf için veriyi hazırlama
        const photoData = {
          image_id: photo.id, // Fotoğrafın benzersiz ID'si
          breast_birads: biradsValue, // BI-RADS değeri
          finding_categories: findings, // Seçilen finding categories
        };
        etiketeHazirData.value = photosToSave;
        photosToSave.push(photoData);
      });

      // Kaydedilecek fotoğraf verilerini konsola yazdırıyoruz
      console.log("Kaydedilecek fotoğraf verileri:", photosToSave);

      // Tüm klasörlerin etiketleme verilerini logla
      console.log("Tüm klasörlerin etiketleme verileri:", JSON.parse(JSON.stringify(folderTagData.value)));

      // Aktif klasörün id'sini bul
      let activeFolderId = null;
      if (folders.value && folders.value.length > 0 && currentPhotoList.length > 0) {
        // Fotoğraflardan birinin parent klasörünü bul
        const photoId = currentPhotoList[0].id;
        const folder = folders.value.find(f => f.children.some(c => c.id === photoId));
        if (folder) activeFolderId = folder.id;
      }
      // Etiketleme bilgisini kaydet
      if (activeFolderId) {
        folderTagData.value[activeFolderId] = {
          selectedFindings1: [...selectedFindings1.value],
          selectedFindings2: [...selectedFindings2.value],
          selectedBiradsSol: selectedBiradsSol.value,
          selectedBiradsSag: selectedBiradsSag.value,
        };
      }
      // Bu verileri veritabanına kaydetme işlemi burada yapılabilir
    };

    // Klasör seçildiğinde ilgili fotoğrafları yükle
    const onSelectFolder = (folderId) => {
      const selectedFolder = folders.value.find(f => f.id === folderId);
      if (selectedFolder && selectedFolder.children && selectedFolder.children.length > 0) {
        const photoItems = selectedFolder.children.map(child => ({
          id: child.id,
          path: child.photoUrl,
        }));
        currentPhotoList = photoItems;
        loadPhotos(photoItems);
      } else {
        // Fotoğraf yoksa boş göster
        currentPhotoList = [];
        loadPhotos([]);
      }
      // Etiketleme alanını güncelle
      if (folderTagData.value[folderId]) {
        // Daha önce etiketleme yapılmışsa, alanları doldur
        selectedFindings1.value = [...folderTagData.value[folderId].selectedFindings1];
        selectedFindings2.value = [...folderTagData.value[folderId].selectedFindings2];
        selectedBiradsSol.value = folderTagData.value[folderId].selectedBiradsSol;
        selectedBiradsSag.value = folderTagData.value[folderId].selectedBiradsSag;
      } else {
        // Hiç etiketleme yapılmamışsa, alanları sıfırla
        selectedFindings1.value = [];
        selectedFindings2.value = [];
        selectedBiradsSol.value = "";
        selectedBiradsSag.value = "";
      }
    };

    const veritabaninaGonder = async () => {
      try {
        // Proxy içermemesi için JSON'dan geçir
        const rawList = toRaw(etiketeHazirData.value);
        const cleanList = JSON.parse(JSON.stringify(rawList)); // tüm Proxy'leri siler

        const result = await postEtiketler(cleanList);
        console.log("Veritabanına gönderme sonucu:", result);
        if (result && result.message) {
          alert("Başarılı: " + result.message);
        } else {
          alert("Veritabanına gönderme başarılı!");
        }
      } catch (err) {
        console.error("Veritabanına gönderme hatası:", err);
        alert("Hata: " + (err?.message || err));
      }
    };

    return {
      activeTab,
      findings,
      biradsList,
      selectedFindings1,
      selectedFindings2,
      selectedBiradsSol,
      selectedBiradsSag,
      folders,
      saveTags, // saveTags fonksiyonunu burada döndürüyoruz
      onSelectFolder, // yeni fonksiyonu ekle
      folderTagData,
      veritabaninaGonder,
    };
  },
};
</script>


<style scoped>
@import "@/assets/label-images.css"; /* Scoped içinde gereksiz olabilir */
</style>
