PhoneService - .NET Core 6.0 ile Telefon Servisi Randevu Uygulaması
Bu proje, farklı marka ve modellere sahip telefonlar için servis randevusu oluşturmayı sağlayan bir web tabanlı randevu sistemidir. Kullanıcılar, şehir ve bayi bilgisine göre uygun randevular alabilir. 
Admin panel üzerinden marka, model, bayi, şehir ve fiyat gibi bilgiler dinamik olarak yönetilebilir.

Kullanılan Teknolojiler

Katman	Teknolojiler
Backend	ASP.NET Core 6.0 MVC, Entity Framework Core, MSSQL
Ekstra	AutoMapper, FluentValidation, Custom MailSender (SMTP)
Mimariler	Areas, DTO + ValidationRules, Custom LanguageManager

Özellikler
Kullanıcı kayıt ve giriş işlemleri

Marka/Model/Şehir/Bayi/Fiyat yönetimi (Admin Area)

Randevu oluşturma, düzenleme, listeleme

FluentValidation ile alan doğrulama

AutoMapper ile DTO mapping

Kullanıcıya randevu sonrası mail gönderimi

Admin panel yetkilendirmesi

 Modüler yapı (Custom Folder Structure)
 ________________________________________________________________________________________________

  English Version
  PhoneService - ASP.NET Core 6.0 Phone Repair Appointment System
A modular MVC web application where users can create service appointments for mobile phones by selecting brand, model, city, and dealer. 
Admins manage brands, models, cities, prices, and appointments.

Tech Stack
ASP.NET Core 6.0 MVC

Entity Framework Core (Code-First)

FluentValidation for input validation

AutoMapper for model/DTO mapping

SMTP Mail Service

MSSQL
_______________
Features
Role-based Admin panel

Multi-step appointment booking

User registration & login

Brand/model/city-based appointment filtering

Email notification after successful appointment

FluentValidation-based rules




