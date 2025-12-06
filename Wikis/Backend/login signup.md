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
         connectionString="Data Source=.\MSRDINANI;Initial Catalog=ToDo;Integrated Security=True;Connect Timeout=30" 
         providerName="System.Data.SqlClient" />
</connectionStrings>
```
این تنظیم به برنامه اجازه می‌دهد تا با استفاده از نمونه SQL Server `MSRDINANI` به دیتابیس متصل شود.

### 2. صفحه ثبت‌نام (Sign Up)

#### تغییرات ظاهری (Frontend)
در فایل `SignUp.aspx`، دو فیلد جدید اضافه شد تا اطلاعات کامل‌تری از کاربر دریافت شود:
- **نام کاربری (Username):** برای شناسایی کاربر در سیستم.
- **تکرار رمز عبور (Confirm Password):** برای اطمینان از صحت رمز عبور وارد شده.

#### اصلاح جاوااسکریپت
فایل `login.js` که مسئولیت اعتبارسنجی کلاینت‌ساید را بر عهده دارد، به‌گونه‌ای اصلاح شد که پس از اعتبارسنجی موفق، اجازه ارسال فرم به سرور (PostBack) را بدهد. این کد برای هر دو فرم ورود و ثبت‌نام کار می‌کند و فیلدهای مربوطه را بررسی می‌کند.

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
            SqlCommand checkEmailCmd = new SqlCommand(checkEmailQuery, conn);
            checkEmailCmd.Parameters.AddWithValue("@Email", email);
            int emailCount = (int)checkEmailCmd.ExecuteScalar();

            if (emailCount > 0)
            {
                ShowError("email", "This Email is already registered!");
                return;
            }

            // 2. Check if Username exists
            string checkUserQuery = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
            SqlCommand checkUserCmd = new SqlCommand(checkUserQuery, conn);
            checkUserCmd.Parameters.AddWithValue("@Username", username);
            int userCount = (int)checkUserCmd.ExecuteScalar();

            if (userCount > 0)
            {
                ShowError("username", "This Username is already taken!");
                return;
            }

            // 3. Hash password (SHA256)
            string passwordHash = HashPassword(password);

            // 4. Insert User (including FullName to handle database constraint)
            string insertQuery = "INSERT INTO Users (FullName, Username, Email, PasswordHash, CreatedAt) VALUES (@FullName, @Username, @Email, @PasswordHash, @CreatedAt)";
            SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
            insertCmd.Parameters.AddWithValue("@FullName", username); // Use username as FullName for now
            insertCmd.Parameters.AddWithValue("@Username", username);
            insertCmd.Parameters.AddWithValue("@Email", email);
            insertCmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
            insertCmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);

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
                
                // Add shake animation
                const input = document.getElementById('{field}');
                if(input) {{
                    input.style.animation = 'materialShake 0.4s ease-in-out';
                    setTimeout(() => {{ input.style.animation = ''; }}, 400);
                }}
            }}
        }});";
    ClientScript.RegisterStartupScript(this.GetType(), "ServerError_" + field, script, true);
}

private string HashPassword(string password)
{
    using (SHA256 sha256 = SHA256.Create())
    {
        byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < bytes.Length; i++)
        {
            builder.Append(bytes[i].ToString("x2"));
        }
        return builder.ToString();
    }
}
```

**نکات کلیدی:**
1.  **اعتبارسنجی مجزا:** ابتدا ایمیل و سپس نام کاربری بررسی می‌شوند تا خطای دقیق به کاربر نمایش داده شود.
2.  **نمایش خطا:** از متد `ShowError` برای تزریق کد جاوااسکریپت استفاده می‌شود تا خطاها دقیقاً در مکان مناسب (زیر فیلد مربوطه) و با استایل قالب نمایش داده شوند.
3.  **امنیت:** رمز عبور قبل از ذخیره شدن با استفاده از الگوریتم SHA256 هش می‌شود.
4.  **مقابله با محدودیت دیتابیس:** چون ستون `FullName` در جدول Users اجباری است، مقدار `Username` به عنوان `FullName` نیز ذخیره می‌شود.

### 3. صفحه ورود (Login)

#### منطق کد (Backend)
در فایل `Login.aspx.cs`، متد `LoginUser` وظیفه احراز هویت را بر عهده دارد و از همان مکانیزم `ShowError` برای نمایش پیام‌های "User not found" یا "Invalid Password" استفاده می‌کند.

```csharp
private void LoginUser(string email, string password)
{
    string connectionString = WebConfigurationManager.ConnectionStrings["TodoAppDB"].ConnectionString;

    using (SqlConnection conn = new SqlConnection(connectionString))
    {
        try
        {
            conn.Open();

            // Get user by email
            string query = "SELECT Username, PasswordHash FROM Users WHERE Email = @Email";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Email", email);

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                string dbPasswordHash = reader["PasswordHash"].ToString();
                string username = reader["Username"].ToString();

                // Check password
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
        catch (Exception ex)
        {
            string msg = ex.Message.Replace("'", "\\'");
            ShowError("email", "System Error: " + msg);
        }
    }
}
```

## ساختار واقعی دیتابیس
کدها بر اساس ساختار واقعی جدول `Users` در دیتابیس نوشته شده‌اند که دارای ستون‌های زیر است:
- `UserID` (کلید اصلی)
- `FullName` (اجباری)
- `Username` (اجباری)
- `Email` (اجباری)
- `PasswordHash` (اجباری)
- `CreatedAt` (اجباری)

## نحوه استفاده
برای اجرای پروژه، کافی است برنامه را اجرا کرده و وارد صفحه `SignUp.aspx` شوید. پس از ساخت حساب کاربری، به صفحه `Login.aspx` هدایت می‌شوید تا با اطلاعات خود وارد سیستم شوید.