# مستندات پیاده‌سازی ایجاد پروژه (Create Project)

این سند جزئیات فنی و منطق پیاده‌سازی شده برای قابلیت "ایجاد پروژه جدید" در سیستم TaskQuest را شرح می‌دهد.

## 🎨 طراحی رابط کاربری (UI/UX)

طراحی این صفحه بر اساس موکاپ فیگما پیاده‌سازی شده است که شامل کاور پروژه، لوگو، فیلدهای متنی و دکمه‌های عملیاتی است.

**لینک طرح در فیگما:**
[https://www.figma.com/make/p1yGBnWo5FdKiVYtRSSJym/Create-Project-Page?t=kZjTKtQJRLxHt64D-1](https://www.figma.com/make/p1yGBnWo5FdKiVYtRSSJym/Create-Project-Page?t=kZjTKtQJRLxHt64D-1)

**تصویر خروجی نهایی:**
![Create Project UI](https://files.catbox.moe/placeholder_project_creation.png)
*(توجه: تصویر دقیق رابط کاربری پیاده‌سازی شده در محیط برنامه قابل مشاهده است)*

---

## 🗄️ تغییرات دیتابیس (Database)

جدول `Projects` در دیتابیس `ToDo` برای پشتیبانی از این قابلیت به‌روزرسانی شده است.

**ستون‌های اضافه شده:**
1.  `[ProjectLogo] NVARCHAR(255)`: ذخیره مسیر فایل لوگوی پروژه.
2.  `[ProjectCover] NVARCHAR(255)`: ذخیره مسیر فایل کاور پروژه.
3.  `[Description] NVARCHAR(MAX)`: ذخیره توضیحات پروژه.

**اسکریپت به‌روزرسانی:**
```sql
ALTER TABLE [dbo].[Projects] ADD [ProjectLogo] NVARCHAR(255) NULL;
ALTER TABLE [dbo].[Projects] ADD [ProjectCover] NVARCHAR(255) NULL;
ALTER TABLE [dbo].[Projects] ADD [Description] NVARCHAR(MAX) NULL;
```

---

## 💻 منطق سمت سرور (Backend Logic)

فایل: `Projects.aspx.cs`

### 1. اعتبارسنجی (Validation)
قبل از ایجاد پروژه، بررسی‌های زیر انجام می‌شود:
*   **احراز هویت:** کاربر باید لاگین کرده باشد (`Session["User"]`).
*   **نام پروژه:** فیلد نام پروژه نباید خالی باشد.
*   **یکتایی نام:** نام پروژه باید برای **همان کاربر** یکتا باشد. اگر کاربری پروژه‌ای با نام "Test" داشته باشد، نمی‌تواند پروژه دیگری با همین نام بسازد، اما کاربران دیگر می‌توانند.

```csharp
private bool IsProjectNameTaken(int userId, string projectName)
{
    // Query: SELECT COUNT(*) FROM Projects WHERE CreatorUserId = @UserId AND ProjectName = @ProjectName
}
```

### 2. مدیریت فایل‌ها (File Handling)
*   **آپلود:** اگر کاربر لوگو یا کاور را انتخاب کند، فایل در مسیر `assets/uploads/projects/` با نام یکتا (شامل Timestamp) ذخیره می‌شود.
*   **پیش‌فرض:** اگر فایلی انتخاب نشود، تصاویر پیش‌فرض زیر استفاده می‌شوند:
    *   Logo: `assets/images/default-project-logo.png`
    *   Cover: `assets/images/default-project-cover.png`

### 3. ذخیره‌سازی (Saving)
اطلاعات نهایی شامل `CreatorUserId`, `ProjectName`, `Description`, `CreatedAt`, `ProjectLogo`, `ProjectCover` در جدول درج می‌شوند.

---

## 🖥️ منطق سمت کلاینت (Frontend Logic)

فایل: `Projects.aspx`

### 1. پاپ‌آپ (Modal)
*   استفاده از `div` با کلاس `modal-overlay` که به صورت پیش‌فرض مخفی (`display: none`) است.
*   توابع JS `openProjectModal()` و `closeProjectModal()` برای کنترل نمایش.
*   استفاده از `ClientScript.RegisterStartupScript` در سمت سرور برای باز نگه داشتن مودال در صورت بروز خطا.

### 2. پیش‌نمایش تصاویر (Image Preview)
*   کلیک روی ناحیه کاور یا لوگو، `FileUpload` مخفی مربوطه را باز می‌کند.
*   پس از انتخاب فایل، تابع `previewImage` با استفاده از `FileReader` تصویر انتخاب شده را بلافاصله نمایش می‌دهد.

### 3. استایل‌ها (CSS)
*   طراحی ریسپانسیو و مدرن با استفاده از CSS Grid و Flexbox.
*   انیمیشن `fadeIn` برای باز شدن نرم پاپ‌آپ.
*   استایل‌دهی اختصاصی برای دکمه‌های "Give up" و "Create" مطابق طرح گرافیکی.
