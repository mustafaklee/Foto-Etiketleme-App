<template>
  <nav>
    <div class="flex-nav-container">
            <a class="flex-nav-item" href="/Home/Index">Ana Menü</a>
            <a class="flex-nav-item" href="/Home/AdminIndex">Ana Menü</a>
            <a class="flex-nav-item" data-toggle="modal" data-target="#imageCountModal">Fotoğraf Etiketle</a>
            <a class="flex-nav-item" href="/Home/labeledImages">Etiketlenmiş Fotoğraflar</a>
            <a class="flex-nav-item" href="/Login/Register">Kullanıcı Oluştur</a>
        <a class="flex-nav-item" href="/Login/Logout">Çıkış Yap</a>
    </div>
  </nav>
  
  
  <div class="summerize">
    <h2 style="margin-bottom: 30px;">Fotoğraf İstatistikleri</h2>
    <p>Hoş geldin <span id="email"></span></p>
    <p>Toplam Atanan Fotoğraf: <span id="toplam">0</span></p>
    <p>Etiketlenmiş Fotoğraflar: <span id="etiketlenmis">0</span></p>
    <p>Bekleyen Fotoğraflar: <span id="bekleyen">0</span></p>
    <div id="istatistikMessage"></div>
</div>
</template>


<style scoped> 
@import "@/assets/index.css"; 
</style>