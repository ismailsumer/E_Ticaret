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

**E-Ticaret Projesi**, geleneksel monolitik yapıların aksine, **Onion Architecture** (Soğan Mimarisi) kullanılarak modüler bir yapıda tasarlanmıştır. Bu sayede veritabanı bağımsızlığı, test edilebilirlik ve bakım kolaylığı sağlanmıştır. 

Sistem üç ana bileşenden oluşur:
1.  **Core API (Backend):** Tüm iş mantığını, veri erişimini ve güvenliği yöneten merkezi servis.
2.  **Web UI (Frontend):** Son kullanıcıların ürünleri inceleyip satın aldığı Angular tabanlı modern arayüz.
3.  **Admin Panel:** Yöneticilerin katalog, sipariş ve kullanıcı yönetimini yaptığı MVC tabanlı panel.

---

## 🏗 Teknik Mimari

Proje, **Onion Architecture** katmanlı mimarisine sadık kalınarak geliştirilmiştir:

- **Core Layer (Entity & Application):** Dış dünyadan tamamen izole, saf C# sınıfları. Entity'ler, DTO'lar, Interface'ler burada bulunur.
- **Infrastructure Layer:** Veritabanı erişimi (EF Core), dış servis entegrasyonları (Email, Storage vb.).
- **Presentation Layer (API):** Controller'lar ve API uç noktaları.

```mermaid
graph TD
    A[Presentation Layer (API)] --> B[Infrastructure Layer]
    B --> C[Core Layer (Application & Domain)]
    A --> C
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

| Ana Sayfa | Ürün Detay |
|-----------|------------|
| ![Ana Sayfa](https://via.placeholder.com/400x200?text=Ana+Sayfa) | ![Ürün Detay](https://via.placeholder.com/400x200?text=%C3%9Cr%C3%BCn+Detay) |

| Sepet | Admin Paneli |
|-------|--------------|
| ![Sepet](https://via.placeholder.com/400x200?text=Sepet) | ![Admin Panel](https://via.placeholder.com/400x200?text=Admin+Panel) |

---

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
**Geliştirici:** [İsminiz]  
**İletişim:** email@example.com
