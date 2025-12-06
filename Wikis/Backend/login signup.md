# مستندات ثبت نام و ورود (Login / Signup)

**نویسنده:** Mahsa Dinani
**تاریخ:** 2025-12-06
**پروژه:** TaskQuest

---

## مقدمه
این سند توضیح می‌دهد که چگونه بخش‌های ثبت‌نام و ورود کاربران در پروژه پیاده‌سازی شده‌اند. هدف اصلی، ایجاد یک سیستم ساده و امن برای مدیریت کاربران با استفاده از دیتابیس SQL Server موجود است.

## تغییرات و پیاده‌سازی‌ها

### 1. اتصال به دیتابیس (Connection String)
برای اتصال به دیتابیس `ToDo.mdf` که در مسیر `P:\Dbs` قرار دارد، تغییر زیر در فایل `Web.config` اعمال شد:

```xml
<connectionStrings>
    <add name="TodoAppDB" 
         connectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=P:\Dbs\ToDo.mdf;Integrated Security=True;Connect Timeout=30" 
         providerName="System.Data.SqlClient" />
</connectionStrings>
```
این تنظیم به برنامه اجازه می‌دهد تا با استفاده از LocalDB به فایل دیتابیس متصل شود.

### 2. صفحه ثبت‌نام (Sign Up)

#### تغییرات ظاهری (Frontend)
در فایل `SignUp.aspx`، دو فیلد جدید اضافه شد تا اطلاعات کامل‌تری از کاربر دریافت شود:
- **نام کاربری (Username):** برای شناسایی کاربر در سیستم.
- **تکرار رمز عبور (Confirm Password):** برای اطمینان از صحت رمز عبور وارد شده.

#### اصلاح جاوااسکریپت
فایل `login.js` که مسئولیت اعتبارسنجی کلاینت‌ساید را بر عهده داشت، به‌گونه‌ای اصلاح شد که پس از اعتبارسنجی موفق، اجازه ارسال فرم به سرور (PostBack) را بدهد. پیش از این، کد جاوااسکریپت جلوی ارسال فرم را می‌گرفت و صرفاً یک انیمیشن نمایش می‌داد.

#### منطق کد (Backend)
در فایل `SignUp.aspx.cs`، متد `RegisterUser` با منطق زیر پیاده‌سازی شده است:

```csharp
private void RegisterUser(string username, string email, string password)
{
    string connectionString = WebConfigurationManager.ConnectionStrings["TodoAppDB"].ConnectionString;

    using (SqlConnection conn = new SqlConnection(connectionString))
    {
        try
        {
            conn.Open();

            // 1. Check if Email exists
            string checkEmailQuery = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
            // ... (اجرای کوئری)
            if (emailCount > 0)
            {
                ShowError("email", "This Email is already registered!");
                return;
            }

            // 2. Check if Username exists
            string checkUserQuery = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
            // ... (اجرای کوئری)
            if (userCount > 0)
            {
                ShowError("username", "This Username is already taken!");
                return;
            }

            // 3. Hash password (SHA256)
            string passwordHash = HashPassword(password);

            // 4. Insert User
            string insertQuery = "INSERT INTO Users (Username, Email, PasswordHash, CreatedAt) VALUES (@Username, @Email, @PasswordHash, @CreatedAt)";
            // ... (اجرای کوئری درج)

            insertCmd.ExecuteNonQuery();
            Response.Redirect("Login.aspx");
        }
        catch (Exception ex)
        {
            // نمایش خطا به کاربر
            string msg = ex.Message.Replace("'", "\\'");
            ShowError("username", "System Error: " + msg);
        }
    }
}

// متد کمکی برای نمایش خطاهای سرور در قالب UI موجود
private void ShowError(string field, string message)
{
    string script = $@"
        document.addEventListener('DOMContentLoaded', function() {{
            const formGroup = document.getElementById('{field}').closest('.form-group');
            const errorElement = document.getElementById('{field}Error');
            if(formGroup && errorElement) {{
                formGroup.classList.add('error');
                errorElement.textContent = '{message}';
                errorElement.classList.add('show');
            }}
        }});";
    ClientScript.RegisterStartupScript(this.GetType(), "ServerError_" + field, script, true);
}
```

**نکات کلیدی:**
1.  **اعتبارسنجی مجزا:** ابتدا ایمیل و سپس نام کاربری بررسی می‌شوند تا خطای دقیق به کاربر نمایش داده شود.
2.  **نمایش خطا:** از متد `ShowError` برای تزریق کد جاوااسکریپت استفاده می‌شود تا خطاها دقیقاً در مکان مناسب (زیر فیلد مربوطه) و با استایل قالب نمایش داده شوند.
3.  **امنیت:** رمز عبور قبل از ذخیره شدن با استفاده از الگوریتم SHA256 هش می‌شود.

### 3. صفحه ورود (Login)

#### منطق کد (Backend)
در فایل `Login.aspx.cs`، متد `LoginUser` وظیفه احراز هویت را بر عهده دارد و از همان مکانیزم `ShowError` برای نمایش پیام‌های "User not found" یا "Invalid Password" استفاده می‌کند.

```csharp
private void LoginUser(string email, string password)
{
    // ... (اتصال به دیتابیس)
    // 1. Get user by email
    // ...
    if (reader.Read())
    {
        // 2. Check password
        if (HashPassword(password) == dbPasswordHash)
        {
            // Login successful
            Session["User"] = username;
            FormsAuthentication.SetAuthCookie(username, false);
            Response.Redirect("Default.aspx");
        }
        else
        {
            ShowError("password", "Invalid Password!");
        }
    }
    else
    {
        ShowError("email", "User not found!");
    }
}
```

## ساختار فرضی دیتابیس
کدها بر اساس این فرض نوشته شده‌اند که جدول `Users` در دیتابیس دارای ستون‌های زیر است:
- `Username` (از نوع متنی)
- `Email` (از نوع متنی)
- `PasswordHash` (از نوع متنی - برای ذخیره رمز عبور)
- `CreatedAt` (از نوع تاریخ)

## نحوه استفاده
برای اجرای پروژه، کافی است برنامه را اجرا کرده و وارد صفحه `SignUp.aspx` شوید. پس از ساخت حساب کاربری، به صفحه `Login.aspx` هدایت می‌شوید تا با اطلاعات خود وارد سیستم شوید.
