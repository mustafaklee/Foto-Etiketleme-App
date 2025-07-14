<template>
  <div class="main-container">
    <!-- Sol Panel -->
    <div class="left-panel" @scroll="onLeftPanelScroll" ref="leftPanelRef" style="overflow-y:scroll">
      <TreeView :treeData="[...labeledFoldersRef, ...folders]" :selectedFolderId="selectedFolderId" @selectFolder="onSelectFolder" />
      <div v-if="isLoading" class="loading">Yükleniyor...</div>
      <div v-if="!hasMoreFolders" class="no-more">Tüm klasörler yüklendi.</div>
    </div>

    <!-- Orta Panel (Fotoğraflar) -->
    <div class="middle-panel">
      <div class="photos-container">
        <!-- JS ile fotoğraflar yüklenecek -->
      </div>
      <div v-if="currentPatientAge">
        <Strong>Hasta Yaşı: {{ currentPatientAge }}</Strong>
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
      <button class="btn" @click="saveTags">{{ saveButtonText }}</button>

      <button class="btn"  @click="veritabaninaGonder">Veritabanına Gönder</button>
    </div>

  </div>
</template>

<script>
import { ref, onMounted } from "vue";
import TreeView from "../components/TreeView.vue";
import { loadPhotos } from "../js/photoViewer";
import { getFotos, GetLabeledFotos, getEtiketler, postEtiketler } from "../js/api";
export default {
  components: {
    TreeView,
  },
  setup() {
    const activeTab = ref("sagMeme");
    const findings = ref([]);
    const biradsList = ref([]);
    const selectedFindings1 = ref([]);
    const selectedFindings2 = ref([]);
    const selectedBiradsSol = ref("");
    const selectedBiradsSag = ref("");
    const folders = ref([]); // etiketsiz (unlabeled) klasörler
    const labeledFoldersRef = ref([]); // etiketli (labeled) klasörler
    const etiketeHazirData = ref({});
    let currentPhotoList = [];
    const folderTagData = ref({})
    const saveButtonText = ref("Ekle");
    // Sayfalama için yeni state'ler
    const currentPage = ref(1);
    const isLoading = ref(false);
    const hasMoreFolders = ref(true);
    const leftPanelRef = ref(null);
    let currentPatientAge = ref(null);
    const selectedFolderId = ref(null);
    // Klasörleri yükle (sayfalı)
    const loadMoreFolders = async () => {
      if (isLoading.value || !hasMoreFolders.value) return;
      isLoading.value = true;
      try{
          const fotoResponse = await getFotos(currentPage.value, 40);
          const newFolders = (fotoResponse.data || []).map((folder) => ({
          id: folder.folderId,
          name: folder.folderPath,
          patient_age: folder.patientAge,
          expanded: false,
          sourceType: "unlabeled",
          children: (folder.fotograflar || []).map((photo) => ({
            id: photo.id,
            name: photo.path.split("/").pop(),
            photoUrl: photo.path,
            view_position: photo.view_Position,
            laterality_id: photo.laterality_id,
            expanded: false,
            children: [],
          })),
      }));
       if (newFolders.length === 0) {
        hasMoreFolders.value = false;
      }
      folders.value = folders.value.concat(newFolders);
      currentPage.value += 1;
      isLoading.value = false;
      // Eğer ilk yükleme ise ilk klasörün fotoğraflarını yükle
      if (folders.value.length > 0 && currentPhotoList.length === 0) {
        const firstFolder = folders.value[0];
        if (firstFolder.children && firstFolder.children.length > 0) {
          const photoItems = firstFolder.children
          .sort((a, b) => {
            // Önce laterality_id'ye göre sırala (1=Sağ, 2=Sol)
            if (a.laterality_id !== b.laterality_id) {
              return a.laterality_id - b.laterality_id;
            }
            // Aynı laterality_id'de view_position'a göre sırala (1=CC, 2=MLO)
            return a.view_position - b.view_position;
          })
          .map(child => ({
            id: child.id,
            path: child.photoUrl,
            laterality_id: child.laterality_id,
            view_position: child.view_position,
          }));
          currentPatientAge.value = firstFolder.patient_age || "Bilinmiyor";
          currentPhotoList = photoItems;
          loadPhotos(photoItems);
        }
      }
    }
      catch (error) {
        console.error("Klasörler yüklenirken hata:", error);
        isLoading.value = false;
      }
      
    };

    // Sol panel scroll event
    const onLeftPanelScroll = () => {
      const el = leftPanelRef.value;
      if (el && el.scrollTop + el.clientHeight >= el.scrollHeight - 50) {
        loadMoreFolders();
      }
    };

    // Klasörleri gruplayan yardımcı fonksiyon
    function groupPhotosByFolder(photoList, sourceType) {
      const folderMap = {};
      photoList.forEach(photo => {
        // Klasör adını path'ten çıkar
        const folderPath = photo.path ? photo.path.split('/').slice(0, -1).join('/') : null;
        if (!folderPath) return; // path yoksa bu fotoğrafı atla
        if (!folderMap[folderPath]) {
          folderMap[folderPath] = {
            id: folderPath + '_' + sourceType,
            name: folderPath.split('/').pop(), 
            expanded: false,
            sourceType,
            children: [],
            patient_age: photo.patient_age || "Bilinmiyor"
          };
        }
        folderMap[folderPath].children.push({
          id: photo.id,
          name: photo.path.split('/').pop(),
          photoUrl: photo.path,
          view_position: photo.view_Position,
          laterality_id: photo.laterality_id,
          expanded: false,
          children: [],
          tags: photo.tags || null
        });
      });
      return Object.values(folderMap);
    }

    onMounted(async () => {
      const etiketResponse = await getEtiketler();
      findings.value = etiketResponse.data?.findingCategories || [];
      biradsList.value = etiketResponse.data?.breastBirads || [];
      // Etiketli fotoğrafları çek
      const labeledResponse = await GetLabeledFotos();
      // Tüm klasörlerdeki fotoğrafları tek diziye aç
      const allLabeledPhotos = (labeledResponse.data || []).flatMap(folder => 
        (folder.fotograflar || []).map(photo => ({
          ...photo,
          path: photo.path, // path zaten var
          patient_age: folder.patientAge || "Bilinmiyor",
        }))
      );
      
      labeledFoldersRef.value = groupPhotosByFolder(allLabeledPhotos, "labeled");
      await loadMoreFolders();
    });

    // Fotoğraf verileri ile kaydetme işlemi
    const saveTags = () => {
      const photosToSave = [];

      currentPhotoList.forEach((photo) => {
        const lateralityCode = parseInt(photo.laterality_id) === 1 ? "R" : "L";

        // İlgili taraftaki BI-RADS ve finding listesi
        const biradsValue   = lateralityCode === "L"
                              ? selectedBiradsSol.value
                              : selectedBiradsSag.value;

        const findings = lateralityCode === "L"
                              ? selectedFindings2.value
                              : selectedFindings1.value;

        // Her fotoğraf için veriyi hazırlama
        const photoData = {
          image_id: photo.id,
          breast_birads: biradsValue, 
          finding_categories: findings,
        };

        photosToSave.push(photoData);
        saveButtonText.value = "Eklendi";

          // 2 saniye sonra tekrar "Ekle" yap
          setTimeout(() => {
            saveButtonText.value = "Ekle";
          }, 2000);
      });

      // Aktif klasörün id'sini bul
      let activeFolderId = null;
      if (folders.value && folders.value.length > 0 && currentPhotoList.length > 0) {
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
        // Fotoğraf etiketleme verilerini de kaydet
        etiketeHazirData.value[activeFolderId] = photosToSave;
      }
    };

    // Klasör seçildiğinde ilgili fotoğrafları yükle
    const onSelectFolder = (folderId) => {
      selectedFolderId.value = folderId;
      // Hem etiketsiz hem etiketli klasörlerde ara
      const selectedFolder = 
        folders.value.find(f => f.id === folderId) ||
        labeledFoldersRef.value.find(f => f.id === folderId);
      if (selectedFolder && selectedFolder.children && selectedFolder.children.length > 0) {
        const photoItems = selectedFolder.children
        .sort((a, b) => {
          // Önce laterality_id'ye göre sırala (1=Sağ, 2=Sol)
          if (a.laterality_id !== b.laterality_id) {
            return a.laterality_id - b.laterality_id;
          }
          // Aynı laterality_id'de view_position'a göre sırala (1=CC, 2=MLO)
          return a.view_position - b.view_position;
        })
        .map(child => ({
          id: child.id,
          path: child.photoUrl,
          laterality_id: child.laterality_id,
          view_position: child.view_position,
        }));
        currentPatientAge.value = selectedFolder.patient_age || "Bilinmiyor";
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
        
        // Eğer daha önce etiketleme yapılmamışsa, mevcut fotoğraflardan etiketleri al
        if (selectedFolder && selectedFolder.children) {
          // Tüm fotoğrafları laterality'ye göre grupla
          const allPhotos = selectedFolder.children;
          
          // Sağ meme findings ve birads - hem sağ fotoğraflardan hem de sağ etiketlerden
          const sagFindings = [];
          let sagBirads = "";
          
          // Sol meme findings ve birads - hem sol fotoğraflardan hem de sol etiketlerden
          const solFindings = [];
          let solBirads = "";
          
          allPhotos.forEach(photo => {
            if (photo.tags) {
              // Fotoğrafın laterality'si ile etiketlerin laterality'si eşleşmeli
              if (parseInt(photo.laterality_id) === 1) {
                // Sağ fotoğraf - sağ etiketleri al
                if (photo.tags.finding_categories) {
                  sagFindings.push(...photo.tags.finding_categories);
                }
                if (photo.tags.breast_birads != null && sagBirads === "") {
                  sagBirads = photo.tags.breast_birads;
                }
              } else if (parseInt(photo.laterality_id) === 2) {
                // Sol fotoğraf - sol etiketleri al
                if (photo.tags.finding_categories) {
                  solFindings.push(...photo.tags.finding_categories);
                }
                if (photo.tags.breast_birads != null && solBirads === "") {
                  solBirads = photo.tags.breast_birads;
                }
              }
            }
          });
          
          // Benzersiz findings'leri al ve değişkenlere ata
          selectedFindings1.value = [...new Set(sagFindings)];
          selectedBiradsSag.value = sagBirads;
          selectedFindings2.value = [...new Set(solFindings)];
          selectedBiradsSol.value = solBirads;
        }
      }
    };

    const veritabaninaGonder = async () => {
      try {
        // Tüm klasörlerin fotoğraf etiketleme verilerini birleştir
        let allPhotoTags = [];
        Object.values(etiketeHazirData.value).forEach(photoList => {
          allPhotoTags = allPhotoTags.concat(photoList);
        });

        const cleanList = JSON.parse(JSON.stringify(allPhotoTags));
        const result = await postEtiketler(cleanList);
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
      saveTags,
      onSelectFolder,
      folderTagData,
      veritabaninaGonder,
      loadMoreFolders,
      isLoading,
      hasMoreFolders,
      leftPanelRef,
      onLeftPanelScroll,
      labeledFoldersRef,
      saveButtonText,
      currentPatientAge,
      selectedFolderId
    };
  },
};
</script>


<style scoped>
@import "@/assets/label-images.css"; /* Scoped içinde gereksiz olabilir */
</style>
