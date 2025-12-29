# سیستم انتخاب تصویر پروفایل (Choose Avatar)

## معرفی
سیستم انتخاب تصویر پروفایل یکی از ویژگیهای جدید برنامه TaskQuest است که به کاربران اجازه میدهد تصویر پروفایل خود را انتخاب کرده یا آپلود کنند. این سیستم پس از ورود یا ثبتنام موفق کاربر فعال میشود.

## فایلهای مرتبط

### صفحات ASPX
- **ChooseAvatar.aspx** - رابط کاربری اصلی
- **ChooseAvatar.aspx.cs** - منطق سرور و پردازش

### فایلهای CSS
- **assets/css/choose-avatar.css** - استایلهای اختصاصی صفحه انتخاب آواتار

### فایلهای پشتیبانی
- **assets/images/avatars/** - پوشه نگهداری تصاویر پیشفرض
- **assets/images/default-avatar.png** - تصویر پیشفرض عمومی

## تغییرات و بهبودهای جدید

### ✨ ویژگیهای جدید
- **آواتارهای کوچکتر**: کاهش اندازه آواتارها از 120px به 80px برای ظاهر تمیزتر
- **چیدمان تکخطی**: آواتارها اکنون در یک خط با قابلیت اسکرول افقی نمایش داده میشوند
- **ریسپانسیو کامل**: بهبود طراحی برای تمام سایزهای صفحه نمایش
- **فیکس انکودینگ**: حل مشکل نمایش متن فارسی با استفاده از UTF-8

### 🎯 بهبودهای طراحی
- استفاده از flexbox برای چیدمان منظم آواتارها
- حذف حالت چندخطی و پیادهسازی اسکرول افقی
- اندازههای واکنشگرا: 80px (دسکتاپ) → 70px → 60px → 55px (موبایل کوچک)
- حفظ فاصله یکنواخت بین آواتارها

### 🔧 فیکسهای فنی
- پیادهسازی سیستم انکودینگ متمرکز با استفاده از کلاس BasePage
- تبدیل متنهای استاتیک به کنترلهای Label برای کنترل سروری
- اضافه شدن متد SafeEncodePersianText() برای تبدیل بایت به رشته
- تنظیمات globalization در Web.config برای UTF-8 و فرهنگ فارسی
- بهینهسازی CSS برای عملکرد بهتر
- اضافه شدن استایل RTL به پیامهای خطا در جاواسکریپت

## ساختار صفحه ChooseAvatar.aspx

### بخشهای اصلی
1. **نوار بالایی** - عنوان صفحه و راهنما
2. **گالری تصاویر** - 6 تصویر پیشفرض برای انتخاب
3. **بخش آپلود** - امکان آپلود تصویر سفارشی
4. **دکمههای عملیات** - "انجام شد" و "فعلا از این مرحله بگذر"

### کد HTML ساختار
```html
<div class="container">
    <div class="row">
        <div class="col-md-12">
            <h2>تصویر پروفایل خود را انتخاب کنید</h2>
            <p>از بین تصاویر زیر یکی را انتخاب کنید یا تصویر خود را آپلود کنید</p>
        </div>
    </div>
    
    <!-- گالری تصاویر پیشفرض -->
    <div class="default-avatars">
        <div class="avatar-option" data-avatar="1">
            <img src="assets/images/avatars/avatar1.svg" alt="آواتار 1">
        </div>
        <div class="avatar-option" data-avatar="2">
            <img src="assets/images/avatars/avatar2.svg" alt="آواتار 2">
        </div>
        <div class="avatar-option" data-avatar="3">
            <img src="assets/images/avatars/avatar3.svg" alt="آواتار 3">
        </div>
        <div class="avatar-option" data-avatar="4">
            <img src="assets/images/avatars/avatar4.svg" alt="آواتار 4">
        </div>
        <div class="avatar-option" data-avatar="5">
            <img src="assets/images/avatars/avatar5.svg" alt="آواتار 5">
        </div>
        <div class="avatar-option" data-avatar="6">
            <img src="assets/images/avatars/avatar6.svg" alt="آواتار 6">
        </div>
    </div>
    
    <!-- بخش آپلود -->
    <div class="upload-section">
        <h3>یا تصویر خود را آپلود کنید</h3>
        <input type="file" id="avatarUpload" accept="image/*">
    </div>
    
    <!-- دکمههای عملیات -->
    <div class="action-buttons">
        <button id="skipButton" class="btn btn-secondary">فعلا از این مرحله بگذر</button>
        <button id="completeButton" class="btn btn-primary">انجام شد</button>
    </div>
</div>
```

## استایلهای CSS جدید

### چیدمان تکخطی آواتارها
```css
.default-avatars {
    display: flex;
    justify-content: center;
    gap: 15px;
    margin-bottom: 40px;
    flex-wrap: nowrap;
    overflow-x: auto;
    padding: 10px;
}

.avatar-option {
    position: relative;
    width: 80px;
    height: 80px;
    border-radius: 50%;
    cursor: pointer;
    transition: all 0.3s ease;
    border: 2px solid transparent;
    overflow: hidden;
    background: #f8f9fa;
    display: flex;
    align-items: center;
    justify-content: center;
    flex-shrink: 0;
}

.avatar-option:hover {
    border-color: #3498db;
    transform: scale(1.05);
}

.avatar-option.selected {
    border-color: #2ecc71;
    box-shadow: 0 0 10px rgba(46, 204, 113, 0.3);
}
```

### ریسپانسیو برای سایزهای مختلف
```css
/* تبلت */
@media (max-width: 768px) {
    .avatar-option {
        width: 70px;
        height: 70px;
    }
}

/* موبایل بزرگ */
@media (max-width: 576px) {
    .avatar-option {
        width: 60px;
        height: 60px;
    }
}

/* موبایل کوچک */
@media (max-width: 480px) {
    .avatar-option {
        width: 55px;
        height: 55px;
    }
    
    .default-avatars {
        gap: 10px;
    }
}
```

## منطق سرور در ChooseAvatar.aspx.cs

### تغییرات جدید در Code-Behind

#### ارثبری از BasePage
صفحه ChooseAvatar.aspx.cs اکنون از کلاس BasePage ارثبری میکند که کنترل متمرکز انکودینگ را فراهم میکند:

```csharp
public partial class ChooseAvatar : BasePage
{
    // کلاس صفحه با کنترل متمرکز انکودینگ
}
```

#### حذف کدهای تکراری
کدهای انکودینگ تکراری حذف شدهاند زیرا توسط BasePage کنترل میشوند:
- حذف `Response.ContentEncoding = Encoding.UTF8;`
- حذف `Response.Charset = "UTF-8";`

### متدهای اصلی

#### 1. Page_Load
بررسی احراز هویت کاربر و بارگذاری اطلاعات اولیه
```csharp
protected void Page_Load(object sender, EventArgs e)
{
    if (Session["User"] == null)
    {
        Response.Redirect("Login.aspx");
    }
}
```

#### 2. SkipButton_Click
رد کردن انتخاب تصویر و هدایت به داشبورد
```csharp
protected void SkipButton_Click(object sender, EventArgs e)
{
    Response.Redirect("Dashboard.aspx");
}
```

#### 3. CompleteButton_Click
ذخیره تصویر انتخاب شده و هدایت به داشبورد
```csharp
protected void CompleteButton_Click(object sender, EventArgs e)
{
    string username = Session["User"].ToString();
    string avatarPath = selectedAvatarPath.Value;
    
    if (string.IsNullOrEmpty(avatarPath))
    {
        avatarPath = "assets/images/default-avatar.png";
    }
    
    // ذخیره در پایگاه داده
    string connectionString = WebConfigurationManager.ConnectionStrings["TodoAppDB"].ConnectionString;
    
    using (SqlConnection conn = new SqlConnection(connectionString))
    {
        conn.Open();
        
        // بررسی وجود ستون AvatarPath
        string checkColumnQuery = @"
            IF NOT EXISTS (SELECT * FROM sys.columns 
            WHERE object_id = OBJECT_ID(N'Users') AND name = 'AvatarPath')
            BEGIN
                ALTER TABLE Users ADD AvatarPath NVARCHAR(255)
            END";
        
        SqlCommand checkCmd = new SqlCommand(checkColumnQuery, conn);
        checkCmd.ExecuteNonQuery();
        
        // بهروزرسانی مسیر تصویر
        string updateQuery = "UPDATE Users SET AvatarPath = @AvatarPath WHERE Username = @Username";
        SqlCommand updateCmd = new SqlCommand(updateQuery, conn);
        updateCmd.Parameters.AddWithValue("@AvatarPath", avatarPath);
        updateCmd.Parameters.AddWithValue("@Username", username);
        
        int rowsAffected = updateCmd.ExecuteNonQuery();
        
        if (rowsAffected > 0)
        {
            Session["UserAvatar"] = avatarPath;
        }
    }
    
    Response.Redirect("Dashboard.aspx");
}
```

## تصاویر پیشفرض

### مشخصات تصاویر
- **فرمت**: SVG (Vector Graphics)
- **اندازه**: 128x128 پیکسل
- **رنگها**: آبی، سبز، قرمز، نارنجی، بنفش، صورتی
- **مسیر**: `assets/images/avatars/`

### لیست تصاویر
1. **avatar1.svg** - دایره آبی با دایره سفید داخلی
2. **avatar2.svg** - دایره سبز با مربع سفید داخلی
3. **avatar3.svg** - دایره قرمز با مثلث سفید داخلی
4. **avatar4.svg** - دایره نارنجی با ضربدر سفید
5. **avatar5.svg** - دایره بنفش با علامت منهای سفید
6. **avatar6.svg** - مستطیل صورتی با دایره و بیضی سفید

### نمونه کد SVG
```svg
<svg width="128" height="128" xmlns="http://www.w3.org/2000/svg">
  <circle cx="64" cy="64" r="60" fill="#3498db"/>
  <circle cx="64" cy="64" r="30" fill="white"/>
</svg>
```

## فرآیند کار سیستم

### 1. پس از ورود موفق
- کاربر به صفحه ChooseAvatar.aspx هدایت میشود
- اطلاعات کاربر در Session بررسی میشود
- اگر کاربر لاگین نکرده باشد، به Login.aspx برمیگردد

### 2. انتخاب تصویر
- کاربر میتواند از بین 6 تصویر پیشفرض یکی را انتخاب کند
- یا تصویر سفارشی خود را آپلود کند
- مسیر تصویر انتخاب شده در hidden field ذخیره میشود

### 3. ذخیره تصویر
- با زدن دکمه "انجام شد"، مسیر تصویر در پایگاه داده ذخیره میشود
- ستون AvatarPath در جدول Users به صورت خودکار ایجاد میشود
- مسیر تصویر در Session نیز ذخیره میشود

### 4. هدایت به داشبورد
- پس از ذخیره موفق، کاربر به Dashboard.aspx هدایت میشود
- اگر کاربر "فعلا از این مرحله بگذر" را انتخاب کند، بدون ذخیره تصویر به داشبورد میرود

## امنیت و بهبودها

### اقدامات امنیتی
1. **بررسی احراز هویت** - بررسی Session["User"] در Page_Load
2. **کوئریهای پارامتری** - استفاده از SqlParameter برای جلوگیری از SQL Injection
3. **مدیریت منابع** - استفاده از using statement برای اتصالات پایگاه داده
4. **بررسی ستون** - بررسی وجود ستون قبل از بهروزرسانی

### بهبودهای جدید
1. **چیدمان تکخطی** - آواتارها اکنون در یک خط با اسکرول افقی نمایش داده میشوند
2. **اندازه بهینه** - کاهش اندازه آواتارها برای ظاهر حرفهایتر
3. **ریسپانسیو کامل** - بهینهسازی برای تمام سایزهای صفحه نمایش
4. **انکودینگ صحیح** - پیادهسازی سیستم متمرکز با استفاده از کلاس BasePage
5. **حذف کدهای تکراری** - حذف کدهای انکودینگ تکراری از code-behind صفحات

### بهبودهای پیشنهادی
1. **اعتبارسنجی تصویر** - بررسی نوع و اندازه فایل آپلود شده
2. **فشردهسازی تصویر** - کاهش حجم تصاویر آپلود شده
3. **کش تصویر** - ذخیره تصاویر در حافظه کش برای عملکرد بهتر
4. **آپلود امن** - ذخیره تصاویر در پوشهای خارج از دسترسی مستقیم
5. **ویرایش تصویر** - اضافه کردن امکان کراپ و resize تصویر

## عیبیابی و خطاها

### خطاهای رایج
1. **کاربر لاگین نکرده** - بررسی Session["User"] در Page_Load
2. **ستون AvatarPath وجود ندارد** - بررسی و ایجاد خودکار در CompleteButton_Click
3. **اتصال پایگاه داده** - بررسی connection string در Web.config
4. **دسترسی به پوشه تصاویر** - بررسی دسترسی write به پوشه avatars

### پیامهای خطا
```csharp
// نمونه کد نمایش خطا
private void ShowError(string message)
{
    ErrorMessage.Text = message;
    ErrorMessage.Visible = true;
}
```

## تغییرات نسخه جدید

### 🆕 ویژگیهای اضافه شده
- چیدمان تکخطی آواتارها با اسکرول افقی
- کاهش اندازه آواتارها برای ظاهر مدرنتر
- بهینهسازی کامل ریسپانسیو
- پیادهسازی سیستم انکودینگ متمرکز با کلاس BasePage
- تبدیل متنهای استاتیک به کنترلهای Label برای کنترل سروری

### 🔧 پیادهسازی BasePage.cs

#### ویژگیهای کلیدی BasePage
- **کنترل متمرکز انکودینگ**: تنظیم خودکار UTF-8 برای تمام صفحات
- **متد SafeEncodePersianText()**: تبدیل بایتهای ناخوانا به متن فارسی صحیح
- **تنظیمات فرهنگ**: اعمال فرهنگ فارسی (fa-IR) به صورت خودکار
- **اعتبارسنجی پیشرندر**: بررسی نهایی انکودینگ قبل از نمایش صفحه

#### متدهای اصلی BasePage
```csharp
protected override void OnInit(EventArgs e)
{
    Response.ContentEncoding = Encoding.UTF8;
    Response.Charset = "UTF-8";
    base.OnInit(e);
}

protected override void OnLoad(EventArgs e)
{
    Thread.CurrentThread.CurrentCulture = new CultureInfo("fa-IR");
    Thread.CurrentThread.CurrentUICulture = new CultureInfo("fa-IR");
    base.OnLoad(e);
}

public string SafeEncodePersianText(string text)
{
    if (string.IsNullOrEmpty(text))
        return text;
    
    byte[] bytes = Encoding.Default.GetBytes(text);
    return Encoding.UTF8.GetString(bytes);
}
```

#### تنظیمات Web.config
```xml
<globalization fileEncoding="utf-8" 
               requestEncoding="utf-8" 
               responseEncoding="utf-8" 
               culture="fa-IR" 
               uiCulture="fa-IR" />
```

### 🔧 فیکسهای اعمال شده
- **مشکل**: آواتارها در چند خط نمایش داده میشدند
  **راهحل**: پیادهسازی چیدمان تکخطی با flexbox و اسکرول افقی
  
- **مشکل**: آواتارها بیش از حد بزرگ بودند
  **راهحل**: کاهش اندازه از 120px به 80px با سطوح ریسپانسیو
  
- **مشکل**: متون فارسی به درستی نمایش داده نمیشدند
  **راهحل**: اضافه کردن متاتگ UTF-8 و استفاده از متن فارسی تمیز
  
- **مشکل**: طراحی در موبایل بهینه نبود
  **راهحل**: بهینهسازی اندازهها برای سایزهای مختلف صفحه

### 📊 آمار بهبودها
- **کاهش اندازه آواتارها**: ~33% (از 120px به 80px)
- **بهبود چیدمان**: از چندخطی به تکخطی با اسکرول
- **بهینهسازی ریسپانسیو**: پشتیبانی از 4 سطح اندازه مختلف
- **پیادهسازی BasePage**: سیستم متمرکز برای کنترل انکودینگ
- **حذف کدهای تکراری**: حذف انکودینگ تکراری از code-behind صفحات
- **بهبود امنیت**: استفاده از ارثبری برای کنترل متمرکز

## جمعبندی
سیستم انتخاب تصویر پروفایل یک ویژگی کاربرپسند است که تجربه کاربری برنامه را بهبود میدهد. با استفاده از تصاویر پیشفرض و امکان آپلود سفارشی، کاربران میتوانند پروفایل خود را شخصیسازی کنند. سیستم به گونهای طراحی شده که استفاده از آن اختیاری است و کاربران میتوانند این مرحله را رد کنند.

تغییرات جدید شامل بهینهسازی طراحی، چیدمان تکخطی، و حل مشکلات انکودینگ است که تجربه کاربری را به طور قابل توجهی بهبود میدهد.