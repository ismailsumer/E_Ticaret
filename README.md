# 🛍️ E-Ticaret Projesi - Tam Kapsamlı E-Ticaret Çözümü

![Project Banner](https://via.placeholder.com/1200x300?text=E-Ticaret+Platformu) <!-- Proje banner görseli buraya -->

Bu proje, modern yazılım geliştirme prensipleri (Clean Architecture, SOLID) benimsenerek geliştirilmiş, ölçeklenebilir ve güvenli bir **B2C E-Ticaret Platformudur**. Kullanıcıların sorunsuz bir alışveriş deneyimi yaşamasını, yöneticilerin ise tüm süreçleri tek bir panelden yönetebilmesini hedefler.

---

## 📑 İçindekiler
- [Proje Hakkında](#-proje-hakkında)
- [Teknik Mimari](#-teknik-mimari)
- [Özellikler](#-özellikler)
- [Teknoloji Yığını](#-teknoloji-yığını)
- [Kurulum ve Çalıştırma](#-kurulum-ve-çalıştırma)
- [API Dokümantasyonu](#-api-dokümantasyonu)
- [Proje Yapısı](#-proje-yapısı)
- [Ekran Görüntüleri](#-ekran-görüntüleri)
- [Katkıda Bulunma](#-katkıda-bulunma)

---

## 🚀 Proje Hakkında

**E-Ticaret Projesi**, geleneksel monolitik yapıların modern bir uygulaması olan **N-Layer Architecture** (Katmanlı Mimari) kullanılarak modüler bir yapıda tasarlanmıştır. Bu sayede kodun okunabilirliği, test edilebilirlik ve bakım kolaylığı sağlanmıştır. 

Sistem üç ana mantıksal katmandan oluşur:
1.  **Presentation Layer (API):** Kullanıcı etkileşimini ve HTTP isteklerini yöneten RESTful servisler.
2.  **Business Layer (Services):** İş kurallarının ve mantığının işlendiği servis katmanı.
3.  **Data Access Layer (Repositories):** Veritabanı işlemlerini ve veri erişimini yöneten katman.

---

## 🏗 Teknik Mimari

Proje, **Katmanlı Mimari (N-Layer)** prensiplerine sadık kalınarak geliştirilmiştir:

- **Presentation Layer:** Controller'lar ve API uç noktaları. FluentValidation entegrasyonu burada yapılır.
- **Business Layer:** Servisler (Services), DTO dönüşümleri (AutoMapper) ve iş mantığı.
- **Data Access Layer:** Entity Framework Core, Generic Repository ve Unit of Work desenleri.
- **Cross-Cutting Concerns:** Exception Handling, Logging (Serilog), Auth (JWT).

```mermaid
graph TD
    Pres[Presentation Layer (Controllers)] --> Bus[Business Layer (Services)]
    Bus --> Data[Data Access Layer (Repositories)]
    Data --> DB[(Database)]
    
    subgraph Shared ["Shared (Cross-Cutting)"]
        Models[Entities]
        DTOs[Data Transfer Objects]
    end
    
    Pres -.-> Shared
    Bus -.-> Shared
    Data -.-> Shared
```

---

## ✨ Özellikler

### 👤 Müşteri Paneli (Frontend)
- **Kimlik Yönetimi:** Güvenli kayıt, giriş (JWT) ve profil yönetimi.
- **Ürün Kataloğu:** Kategori bazlı gezinme, dinamik arama ve ürün detayları.
- **Sepet Sistemi:** Oturum tabanlı kalıcı sepet yönetimi.
- **Sipariş Süreci:** Çok adımlı (Checkout) sipariş tamamlama ve adres seçimi.
- **Sipariş Takibi:** Geçmiş siparişlerin durumu ve detayları.
- **Responsive Tasarım:** Mobil uyumlu modern arayüz.

### 🛡️ Yönetim Paneli (Admin)
- **Dashboard:** Kritik metriklerin (Toplam satış, sipariş sayısı vb.) anlık takibi.
- **Ürün Yönetimi:** Resim yükleme destekli CRUD işlemleri.
- **Kategori & Marka:** Hiyerarşik yapı yönetimi.
- **Sipariş Yönetimi:** Sipariş durumlarını güncelleme (Hazırlanıyor, Kargoda, Teslim Edildi).
- **Kullanıcı Yönetimi:** Rol tabanlı kullanıcı yetkilendirme ve detay görüntüleme.
- **Slider & İçerik:** Ana sayfa banner yönetimi.

### ⚙️ Backend (API)
- **Güvenlik:** Role-Based Access Control (RBAC) ve JWT Token güvenliği.
- **Validasyon:** FluentValidation ile Request modellerinin doğrulanması.
- **Loglama:** Serilog ile dosya ve konsol tabanlı yapılandırılmış loglama.
- **Dokümantasyon:** Swagger/OpenAPI ile otomatik API dokümantasyonu.
- **Global Hata Yönetimi:** Merkezi exception handling middleware.

---

## 💻 Teknoloji Yığını

| Alan | Teknoloji | Versiyon | Kullanım Amacı |
|------|-----------|----------|----------------|
| **Backend** | .NET | 9.0 | Ana Framework |
| **ORM** | Entity Framework Core | 9.0 | Veritabanı Erişimi |
| **Veritabanı** | SQLite | - | Geliştirme Ortamı Veritabanı |
| **Frontend** | Angular | 19+ | SPA Framework |
| **Admin UI** | ASP.NET Core MVC | 9.0 | Server-Side Rendering UI |
| **Styling** | Bootstrap | 5.3 | Responsive Tasarım |
| **API Doc** | Swagger / OpenAPI | v1 | API Test ve Dokümantasyon |
| **Loglama** | Serilog | - | Sistem Loglama |

---

## 🛠 Kurulum ve Çalıştırma

Projeyi yerel ortamınızda çalıştırmak için aşağıdaki adımları takip edebilirsiniz.

### Ön Gereksinimler
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Node.js](https://nodejs.org/) (v18 veya üzeri önerilir)
- [Angular CLI](https://angular.io/cli) (`npm install -g @angular/cli`)
- Git

### 1. Projeyi Klonlayın
```bash
git clone https://github.com/kullaniciadi/ETicaretProjesi.git
cd ETicaretProjesi
```

### 2. Backend (API) Kurulumu
```bash
cd RestApi/ETicaretAPI
dotnet restore
# Veritabanını oluştur ve migrasyonları uygula
dotnet ef database update
# Projeyi başlat
dotnet run
```
API şu adreste çalışacaktır: `http://localhost:5292`  
Swagger Dokümantasyonu: `https://localhost:7233/swagger`

### 3. Yönetim Paneli (Admin) Kurulumu
Yeni bir terminal açın ve:
```bash
cd AdminPanel/ETicaretMVC
dotnet restore
dotnet run
```
Admin Paneli: `http://localhost:5012` veya `https://localhost:7093`

### 4. Frontend (Angular) Kurulumu
Yeni bir terminal açın ve:
```bash
cd FrontEnd
npm install
npm start
```
Uygulama: `http://localhost:4200` adresinde açılacaktır.

---

## 🔌 API Dokümantasyonu

Proje çalışırken `https://localhost:7233/swagger` adresine giderek tüm endpoint'leri test edebilirsiniz.

**Önemli Endpoint Grupları:**
- `AUTH`: Login, Register, Profile işlemleri.
- `PRODUCTS`: Ürün listeleme, detay, ekleme/güncelleme (Admin).
- `CATEGORIES`: Kategori ağacı yönetimi.
- `ORDERS`: Sipariş oluşturma ve durum takibi.
- `BASKET`: Sepet işlemleri.

---

## 📂 Proje Yapısı

```
ETicaretProjesi/
├── RestApi/
│   └── ETicaretAPI/          # Backend API Projesi
│       ├── Controllers/      # API Uç Noktaları
│       ├── Services/         # İş Mantığı Katmanı
│       ├── Repositories/     # Veri Erişim Katmanı
│       ├── Models/           # Veritabanı Entity'leri
│       └── DTOs/             # Veri Transfer Objeleri
├── FrontEnd/                 # Angular Projesi
│   └── src/app/
│       ├── components/       # Sayfa ve Bileşenler
│       ├── services/         # API İletişim Servisleri
│       ├── guards/           # Route Koruma
│       └── interceptors/     # HTTP İstek Yönetimi
└── AdminPanel/
    └── ETicaretMVC/          # MVC Admin Paneli
        ├── Controllers/      # MVC Controller'lar
        └── Views/            # Razor View Dosyaları
```

---

## 📷 Ekran Görüntüleri


# Frontend 
1. Giriş ekranı 
<img width="1814" height="952" alt="Ekran görüntüsü 2026-02-15 143621" src="https://github.com/user-attachments/assets/bdf9ef9d-f2b1-4622-ba38-bad89a993280" />

2. Kayıt olma 
<img width="1910" height="870" alt="Ekran görüntüsü 2026-02-15 143929" src="https://github.com/user-attachments/assets/868b1d30-308b-418b-80f8-39ea8e8fc34d" />

3. Ürünler 
<img width="1867" height="909" alt="Ekran görüntüsü 2026-02-15 144642" src="https://github.com/user-attachments/assets/9a26dc39-658c-49ee-a32c-c9812dbd49b2" />

4. Ürün Detayı 
<img width="1894" height="936" alt="Ekran görüntüsü 2026-02-15 144818" src="https://github.com/user-attachments/assets/e05c6304-5934-4e56-9947-bb0e49d69281" /> 

5. Sepet 
<img width="1821" height="916" alt="Ekran görüntüsü 2026-02-15 145006" src="https://github.com/user-attachments/assets/9949eed5-43a9-4eec-ac36-7286da0cdabf" />

6. Sipariş Oluşturma 
<img width="1870" height="952" alt="Ekran görüntüsü 2026-02-15 145104" src="https://github.com/user-attachments/assets/ace6148a-b4d7-4603-aa70-07f9f03fd03e" />
7. Profilim 
.<img width="1861" height="924" alt="Ekran görüntüsü 2026-02-15 145321" src="https://github.com/user-attachments/assets/5749f4cf-86eb-42e8-8c25-59b73185e9d6" />
8. Adres kayıt 
<img width="1650" height="916" alt="Ekran görüntüsü 2026-02-15 150410" src="https://github.com/user-attachments/assets/c3805044-f2a7-49b7-a589-d1fa3e2c8000" />
9. Siparişlerim 
<img width="1749" height="911" alt="Ekran görüntüsü 2026-02-15 150530" src="https://github.com/user-attachments/assets/a1466f8f-00bc-4ea3-bde3-b5d80c25cf11" />

# Admin Panel 
1. Dashboard 
<img width="1901" height="932" alt="Ekran görüntüsü 2026-02-15 151200" src="https://github.com/user-attachments/assets/c2e014aa-796f-45fc-93ea-51448a11125d" />
2. Ürün Listesi 
<img width="1902" height="970" alt="Ekran görüntüsü 2026-02-15 151330" src="https://github.com/user-attachments/assets/e02f3a48-40fa-45ca-9e1a-38bbf61eda4d" />
3. Ürün Ekleme 
<img width="1799" height="914" alt="Ekran görüntüsü 2026-02-15 151430" src="https://github.com/user-attachments/assets/5c8231c2-8e17-4f3f-a88f-cbb7d2693c43" />
4. Katagori 
<img width="1900" height="938" alt="Ekran görüntüsü 2026-02-15 151519" src="https://github.com/user-attachments/assets/8ee58c0a-786f-424d-aa92-79df5570dd32" />
5 . Markalar 
<img width="1875" height="941" alt="Ekran görüntüsü 2026-02-15 151650" src="https://github.com/user-attachments/assets/5fc25c5b-6cc6-43fd-bceb-fd9f07ea3175" />
6. Siparişler
<img width="1872" height="755" alt="Ekran görüntüsü 2026-02-15 151802" src="https://github.com/user-attachments/assets/5306e0a9-347b-42aa-8ba3-c11e12ef9e6f" />
7. Sipariş Durumları 
<img width="1909" height="970" alt="Ekran görüntüsü 2026-02-15 151920" src="https://github.com/user-attachments/assets/3c483ecc-fd62-43d8-93fe-93b9b6a757a8" />
8. Kullanıcılar
<img width="1896" height="831" alt="image" src="https://github.com/user-attachments/assets/ef7399e8-c12d-491a-86eb-c18c3e6773db" /> 
9. Yorumlar
<img width="1890" height="882" alt="Ekran görüntüsü 2026-02-15 152138" src="https://github.com/user-attachments/assets/34a34263-2892-491d-911a-afed8f4a7170" />
10. Kargo Ayarları 

<img width="1890" height="910" alt="Ekran görüntüsü 2026-02-15 152244" src="https://github.com/user-attachments/assets/ec257098-2656-49d0-88ab-6405d84fd30b" />
 11. Slider 
 <img width="1759" height="822" alt="Ekran görüntüsü 2026-02-15 152346" src="https://github.com/user-attachments/assets/22e1fdd6-72db-4406-8b05-c267499d1219" /> 












=======





## 🤝 Katkıda Bulunma

1. Bu projeyi Fork'layın.
2. Yeni özellik için branch oluşturun (`git checkout -b feature/HarikaOzellik`).
3. Değişikliklerinizi commit yapın (`git commit -m 'Harika özellik eklendi'`).
4. Branch'inizi Push yapın (`git push origin feature/HarikaOzellik`).
5. Pull Request oluşturun.

---

## 📝 Lisans

Bu proje [MIT Lisansı](LICENSE) ile lisanslanmıştır.

---
**Geliştirici:** [İsmail Sümer]  
**İletişim:** ismaillsumerr@gmail.com
