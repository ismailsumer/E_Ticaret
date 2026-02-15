# E-Ticaret Platformu

Modern teknolojiler, temiz mimari prensipleri ve en iyi uygulama örnekleri kullanılarak geliştirilmiş kapsamlı bir E-Ticaret çözümü. Bu proje, ölçeklenebilir bir backend, dinamik bir yönetim paneli ve kullanıcı dostu bir frontend arayüzü sunar.

## 🚀 Proje Hakkında

Bu proje, gerçek dünya senaryolarını simüle eden tam kapsamlı bir e-ticaret sistemidir. Kullanıcılar ürünleri inceleyip satın alabilirken, yöneticiler gelişmiş bir panel üzerinden tüm sistemi kontrol edebilir.

### Öne Çıkan Özellikler

#### 🛒 Kullanıcı Arayüzü (Frontend)
- **Modern Tasarım:** Angular 19+ ve özel CSS ile geliştirilmiş responsive arayüz.
- **Ürün Keşfi:** Gelişmiş filtreleme, arama ve kategori bazlı listeleme.
- **Sepet & Sipariş:** Dinamik sepet yönetimi ve çok adımlı sipariş süreci.
- **Güvenlik:** JWT tabanlı kimlik doğrulama, Guard yapıları ve HttpInterceptor ile güvenli iletişim.
- **Kullanıcı Paneli:** Profil yönetimi, geçmiş siparişler ve adres defteri.

#### 🔧 Yönetim Paneli (Admin Panel)
- **Dashboard:** Özet veriler ve istatistikler.
- **Katalog Yönetimi:** Ürün, Kategori, Marka ve Slider yönetimi.
- **Sipariş Yönetimi:** Sipariş durumlarını (Hazırlanıyor, Kargoda, vb.) güncelleme ve detay görüntüleme.
- **Kullanıcı Yönetimi:** Kullanıcıları listeleme, detaylarını görme ve rol atama.
- **Ayarlar:** Site genel ayarlarını dinamik olarak yapılandırma.

#### ⚙️ Backend (API)
- **Mimari:** Clean Architecture (Onion Architecture) prensiplerine uygun katmanlı yapı.
- **Veri Erişimi:** Entity Framework Core Code-First yaklaşımı ve Repository/UnitOfWork desenleri.
- **Güvenlik:** Identity tabanlı kullanıcı yönetimi ve JWT entegrasyonu.
- **Validasyon:** FluentValidation ile kapsamlı veri doğrulama.
- **Logging:** Serilog ile yapılandırılmış loglama altyapısı.
- **Dokümantasyon:** Swagger UI ile interaktif API dokümantasyonu.

## 🛠 Kullanılan Teknolojiler

| Alan | Teknolojiler |
|------|--------------|
| **Backend** | .NET 9, ASP.NET Core Web API, Entity Framework Core, SQLite (Dev), AutoMapper, FluentValidation, Serilog |
| **Frontend** | Angular 19+, TypeScript, RxJS, HTML5, CSS3 |
| **Admin** | ASP.NET Core MVC 9, Bootstrap 5, jQuery, AJAX |
| **Araçlar** | Visual Studio 2022 / VS Code, Swagger |

## 📦 Kurulum ve Çalıştırma

Projeyi yerel ortamınızda çalıştırmak için aşağıdaki adımları izleyin.

### Gereksinimler
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Node.js](https://nodejs.org/) (LTS sürümü önerilir)
- Angular CLI (`npm install -g @angular/cli`)

### Adım 1: Backend (API) Başlatma
Veritabanı ve API sunucusunu ayağa kaldırın.

```bash
cd RestApi/ETicaretAPI
dotnet restore
dotnet run
```
API şu adreslerde çalışacaktır:
- Swagger UI: `https://localhost:7233/swagger`
- API URL: `http://localhost:5292`

> **Not:** İlk çalıştırmada veritabanı otomatik olarak oluşturulacaktır (SQLite).

### Adım 2: Yönetim Paneli (Admin) Başlatma

```bash
cd AdminPanel/ETicaretMVC
dotnet restore
dotnet run
```
Admin paneli şu adreste çalışacaktır: `http://localhost:5012` veya `https://localhost:7093`

### Adım 3: Kullanıcı Arayüzü (Frontend) Başlatma

```bash
cd FrontEnd
npm install
npm start
```
Uygulama tarayıcınızda otomatik olarak açılacaktır: `http://localhost:4200`

## 📂 Proje Yapısı

```
ETicaretProjesi/
├── RestApi/
│   └── ETicaretAPI/       # .NET 9 Web API Projesi
├── FrontEnd/              # Angular 19+ SPA Projesi
├── AdminPanel/
│   └── ETicaretMVC/       # ASP.NET MVC Admin Paneli
└── ETicaretProjesi.sln    # Solution Dosyası
```

## 🤝 Katkıda Bulunma

1. Fork yapın
2. Yeni bir feature branch oluşturun (`git checkout -b feature/YeniOzellik`)
3. Değişikliklerinizi commit edin (`git commit -m 'Yeni özellik eklendi'`)
4. Branch'inizi push edin (`git push origin feature/YeniOzellik`)
5. Pull Request oluşturun

---
*Geliştirici Notu: Bu proje eğitim ve portfolyo amaçlı geliştirilmiştir.*
