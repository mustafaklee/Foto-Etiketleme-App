<template>
  <div>
    <!-- Floating Background Elements -->
    <div class="floating-element" style="top: 20%; left: 10%; font-size: 4rem; color: white;">
      <i class="fas fa-camera"></i>
    </div>
    <div class="floating-element" style="top: 60%; right: 15%; font-size: 3rem; color: white; animation-delay: -3s;">
      <i class="fas fa-images"></i>
    </div>
    <div class="floating-element" style="bottom: 30%; left: 20%; font-size: 2rem; color: white; animation-delay: -1.5s;">
      <i class="fas fa-tag"></i>
    </div>

    <nav>
      <div class="flex-nav-container">
        <router-link class="flex-nav-item" to="/home">
          <i class="fas fa-home"></i> Ana Menü
        </router-link>
        
        <router-link class="flex-nav-item" to="/photo-assignment">
          <i class="fas fa-tag"></i> Fotoğraf Atama
        </router-link>
        
        <router-link class="flex-nav-item" to="/user-create">
          <i class="fas fa-user-plus"></i> Kullanıcı Oluştur
        </router-link>
        <router-link class="flex-nav-item" to="/admin">
          <i class="fas fa-cog"></i> Kullanma Klavuzu
        </router-link>
        <button class="flex-nav-item" @click="logout">
          <i class="fas fa-sign-out-alt"></i> Çıkış Yap
        </button>
      </div>
    </nav>

    <div class="dashboard-container">
      <!-- Welcome Section -->
      <div class="welcome-section">
        <h1 class="welcome-title">Fotoğraf İstatistikleri</h1>
        <p class="welcome-text">Doktor seçerek detaylı istatistikleri görüntüleyebilirsiniz</p>
      </div>

      <!-- Loading State -->
      <div v-if="loading" class="loading-section">
        <div class="loading-spinner">
          <i class="fas fa-spinner fa-spin"></i>
          <p>Veriler yükleniyor...</p>
        </div>
      </div>

      <!-- Error State -->
      <div v-if="error" class="error-section">
        <div class="error-message">
          <i class="fas fa-exclamation-triangle"></i>
          <p>{{ error }}</p>
          <button @click="fetchDoctors" class="retry-button">
            <i class="fas fa-redo"></i> Tekrar Dene
          </button>
        </div>
      </div>

      <!-- Doctor Selection -->
      <div class="doctor-selection" v-show="showDoctorSelectionPanel && !loading && !error">
        <h2 class="selection-title">
          <i class="fas fa-user-md"></i> Doktor Seçimi
        </h2>
        <div class="selection-controls">
          <button 
            class="compare-button" 
            :disabled="selectedDoctors.length !== 2"
            @click="compareSelectedDoctors"
          >
            <i class="fas fa-exchange-alt"></i> Karşılaştır
          </button>
          <button class="clear-button" @click="clearSelectedDoctors">
            <i class="fas fa-times"></i> Temizle
          </button>
        </div>
        <div class="doctor-grid">
          <div 
            v-for="doctor in doctors" 
            :key="doctor.id"
            class="doctor-card"
            :class="{ selected: selectedDoctors.includes(doctor.id) }"
            @click="toggleDoctorSelection(doctor.id)"
          >
            <div class="doctor-icon">
              <i class="fas fa-user-md"></i>
            </div>
            <div class="doctor-name">{{ doctor.name }}</div>
            <div class="doctor-email">{{ doctor.email }}</div>
            <div class="doctor-info">
              <small>ID: {{ doctor.id }}</small>
            </div>
            <div class="selection-checkbox"><i class="fas fa-check"></i></div>
          </div>
        </div>
      </div>

      <!-- Doctor Statistics Section -->
      <div class="doctor-stats-section" :class="{ active: showStatsSection }">
        <button class="back-button" @click="showDoctorSelectionMethod">
          <i class="fas fa-arrow-left"></i> Geri Dön
        </button>
        
        <div class="stats-comparison-container">
          <div 
            v-for="(doctorId) in selectedDoctors" 
            :key="doctorId"
            class="doctor-stats"
          >
            <div class="stats-header">
              <h2 class="stats-doctor-title">{{ getDoctorById(doctorId)?.name }}</h2>
              <p class="stats-doctor-email">{{ getDoctorById(doctorId)?.email }}</p>
            </div>

            <!-- BIRADS İstatistikleri -->
            <div class="stats-section">
              <h3 class="stats-section-title">
                <i class="fas fa-chart-bar"></i> BIRADS İstatistikleri
              </h3>
              <div class="stats-grid">
                <div 
                  v-for="birad in getBiradsStatsForDoctor(doctorId)" 
                  :key="`birads-${doctorId}-${birad.etiketId}`"
                  class="stat-card"
                >
                  <div class="stat-icon">
                    <i class="fas fa-tag"></i>
                  </div>
                  <span class="stat-number">{{ birad.count }}</span>
                  <p class="stat-label">{{ getBiradsLabel(birad.etiketId) }}</p>
                  <p class="stat-description">BIRADS {{ birad.etiketId }} etiketleme sayısı</p>
                  <div class="progress-container">
                    <div class="progress-bar">
                      <div 
                        class="progress-fill birads-progress" 
                        :style="{ width: getBiradsPercentage(doctorId, birad.count) + '%' }"
                      ></div>
                    </div>
                  </div>
                </div>
              </div>
            </div>

            <!-- Findings İstatistikleri -->
            <div class="stats-section">
              <h3 class="stats-section-title">
                <i class="fas fa-search"></i> Findings İstatistikleri
              </h3>
              <div class="stats-grid">
                <div 
                  v-for="finding in getFindingsStatsForDoctor(doctorId)" 
                  :key="`finding-${doctorId}-${finding.etiketId}`"
                  class="stat-card"
                >
                  <div class="stat-icon">
                    <i class="fas fa-eye"></i>
                  </div>
                  <span class="stat-number">{{ finding.count }}</span>
                  <p class="stat-label">{{ getFindingLabel(finding.etiketId) }}</p>
                  <p class="stat-description">Finding {{ finding.etiketId }} etiketleme sayısı</p>
                  <div class="progress-container">
                    <div class="progress-bar">
                      <div 
                        class="progress-fill findings-progress" 
                        :style="{ width: getFindingPercentage(doctorId, finding.count) + '%' }"
                      ></div>
                    </div>
                  </div>
                </div>
              </div>
            </div>

            <!-- Özet İstatistikler -->
            <div class="summary-stats">
              <div class="summary-card">
                <h4>Toplam BIRADS Etiketleme</h4>
                <span class="summary-number">{{ getTotalBiradsForDoctor(doctorId) }}</span>
              </div>
              <div class="summary-card">
                <h4>Toplam Findings Etiketleme</h4>
                <span class="summary-number">{{ getTotalFindingsForDoctor(doctorId) }}</span>
              </div>
              <div class="summary-card">
                <h4>Genel Toplam</h4>
                <span class="summary-number">{{ getTotalLabelsForDoctor(doctorId) }}</span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { getStats,getDoctorList } from "../js/api";



export default {
  name: 'HomeView',
  data() {
    return {
      doctors: [],
      selectedDoctors: [],
      showDoctorSelectionPanel: true,
      showStatsSection: false,
      loading: false,
      error: null,
      apiData: null,
      
      // Etiket tanımları
      biradsLabels: {
        1: 'BIRADS 1',
        2: 'BIRADS 2', 
        3: 'BIRADS 3',
        4: 'BIRADS 4',
        5: 'BIRADS 5',
        6: 'BIRADS 6'
      },
      
      findingLabels: {
        1: 'Mass',
        2: 'Calcification',
        3: 'Architectural Distortion',
        4: 'Asymmetry',
        5: 'Skin Changes',
        6: 'Nipple Retraction',
        7: 'Lymphadenopathy'
      }
    }
  },
  
  async mounted() {
    await this.fetchDoctors()
  },
  
  methods: {
    async fetchDoctors() {
      this.loading = true
      this.error = null
      
      try {
        // Doktor listesini çek
        const response = await getDoctorList()
        this.doctors = response.data.data || response.data
        
        if (!this.doctors || this.doctors.length === 0) {
          this.error = 'Doktor verisi bulunamadı'
        }
      } catch (err) {
        this.error = 'Doktor verileri yüklenirken hata oluştu: ' + (err.response?.data?.message || err.message)
      } finally {
        this.loading = false
      }
    },
    
    async fetchComparisonData() {
      if (this.selectedDoctors.length !== 2) return
      
      this.loading = true
      this.error = null
      
      try {
        const response = await getStats({
          params: {
            doctor1Id: this.selectedDoctors[0],
            doctor2Id: this.selectedDoctors[1]
          }
        })
        
        this.apiData = response.data.data
      } catch (err) {
        this.error = 'İstatistik verileri yüklenirken hata oluştu: ' + (err.response?.data?.message || err.message)
      } finally {
        this.loading = false
      }
    },
    
    toggleDoctorSelection(doctorId) {
      const index = this.selectedDoctors.indexOf(doctorId)
      
      if (index === -1) {
        if (this.selectedDoctors.length < 2) {
          this.selectedDoctors.push(doctorId)
        }
      } else {
        this.selectedDoctors.splice(index, 1)
      }
    },
    
    async compareSelectedDoctors() {
      if (this.selectedDoctors.length !== 2) return

      this.showDoctorSelectionPanel = false
      this.showStatsSection = true
      
      await this.fetchComparisonData()
    },
    
    clearSelectedDoctors() {
      this.selectedDoctors = []
    },
    
    showDoctorSelectionMethod() {
      this.showStatsSection = false
      this.showDoctorSelectionPanel = true
      this.clearSelectedDoctors()
      this.apiData = null
    },
    
    getDoctorById(doctorId) {
      return this.doctors.find(doctor => doctor.id === doctorId)
    },
    
    getBiradsStatsForDoctor(doctorId) {
      if (!this.apiData) return []
      
      if (doctorId === this.selectedDoctors[0]) {
        return this.apiData.doctor1BiradsCounts || []
      } else if (doctorId === this.selectedDoctors[1]) {
        return this.apiData.doctor2BiradsCounts || []
      }
      
      return []
    },
    
    getFindingsStatsForDoctor(doctorId) {
      if (!this.apiData) return []
      
      if (doctorId === this.selectedDoctors[0]) {
        return this.apiData.doctor1FindingCounts || []
      } else if (doctorId === this.selectedDoctors[1]) {
        return this.apiData.doctor2FindingCounts || []
      }
      
      return []
    },
    
    getBiradsLabel(etiketId) {
      return this.biradsLabels[etiketId] || `BIRADS ${etiketId}`
    },
    
    getFindingLabel(etiketId) {
      return this.findingLabels[etiketId] || `Finding ${etiketId}`
    },
    
    getBiradsPercentage(doctorId, count) {
      const total = this.getTotalBiradsForDoctor(doctorId)
      return total > 0 ? (count / total) * 100 : 0
    },
    
    getFindingPercentage(doctorId, count) {
      const total = this.getTotalFindingsForDoctor(doctorId)
      return total > 0 ? (count / total) * 100 : 0
    },
    
    getTotalBiradsForDoctor(doctorId) {
      const biradsStats = this.getBiradsStatsForDoctor(doctorId)
      return biradsStats.reduce((total, item) => total + item.count, 0)
    },
    
    getTotalFindingsForDoctor(doctorId) {
      const findingsStats = this.getFindingsStatsForDoctor(doctorId)
      return findingsStats.reduce((total, item) => total + item.count, 0)
    },
    
    getTotalLabelsForDoctor(doctorId) {
      return this.getTotalBiradsForDoctor(doctorId) + this.getTotalFindingsForDoctor(doctorId)
    },
    
    async logout() {
      try {
        this.$router.push('/login')
      } catch (err) {
        console.error('Logout error:', err)
        // Hata durumunda da login sayfasına yönlendir
        this.$router.push('/login')
      }
    }
  }
}
</script>

<style scoped>
@import "@/assets/index.css";
</style>