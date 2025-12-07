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

### 2. پیام‌های خطای کاربرپسند فارسی
تمام پیام‌های خطا به متن‌های دوستانه فارسی تغییر کرده‌اند:

**در فرم ورود:**
- اگر ایمیل وجود نداشته باشد: "شما هنوز کاربر ما نیستی دوست عزیز :)"
- اگر رمز عبور اشتباه باشد: "رمز عبورت اشتباهه دوست عزیز!"

**در فرم ثبت‌نام:**
- اگر ایمیل تکراری باشد: "این ایمیل قبلا ثبت شده !"
- اگر نام کاربری تکراری باشد: "این نام کاربری قبلا ثبت شده"

### 3. صفحه ثبت‌نام (Sign Up)

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
                ShowError("email", "این ایمیل قبلا ثبت شده !");
                return;
            }

            // 2. Check if Username exists
            string checkUserQuery = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
            SqlCommand checkUserCmd = new SqlCommand(checkUserQuery, conn);
            checkUserCmd.Parameters.AddWithValue("@Username", username);
            int userCount = (int)checkUserCmd.ExecuteScalar();

            if (userCount > 0)
            {
                ShowError("username", "این نام کاربری قبلا ثبت شده");
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

### 4. صفحه ورود (Login)

#### منطق کد (Backend)
در فایل `Login.aspx.cs`، متد `LoginUser` وظیفه احراز هویت را بر عهده دارد و از همان مکانیزم `ShowError` برای نمایش پیام‌های کاربرپسند فارسی استفاده می‌کند.

**پیام‌های خطای به‌روزرسانی شده:**
- اگر ایمیل وجود نداشته باشد: "شما هنوز کاربر ما نیستی دوست عزیز :)"
- اگر رمز عبور اشتباه باشد: "رمز عبورت اشتباهه دوست عزیز!"

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
                    ShowError("password", "رمز عبورت اشتباهه دوست عزیز!");
                }
            }
            else
            {
                ShowError("email", "شما هنوز کاربر ما نیستی دوست عزیز :)");
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

### 5. ورود با گوگل (Google Login)

#### پیکربندی API گوگل
در فایل `Login.aspx`، اسکریپت API گوگل اضافه شد:

```html
<meta name="google-signin-client_id" content="YOUR_GOOGLE_CLIENT_ID.apps.googleusercontent.com">
<script src="https://accounts.google.com/gsi/client" async defer></script>
```

#### پیاده‌سازی کلاینت‌ساید
در فایل `login.js`، متد `handleSocialLogin` برای مدیریت ورود با گوگل پیاده‌سازی شد:

```javascript
async handleSocialLogin(provider, button) {
    console.log(`Initiating ${provider} sign-in...`);
    
    // Add Material loading state
    button.style.pointerEvents = 'none';
    button.style.opacity = '0.7';
    
    try {
        if (provider === 'Google') {
            // Use Google Identity Services API
            if (window.google && window.google.accounts) {
                // Show Google sign-in popup
                window.google.accounts.id.initialize({
                    client_id: 'YOUR_GOOGLE_CLIENT_ID.apps.googleusercontent.com',
                    callback: this.handleGoogleCallback.bind(this),
                    auto_select: false,
                    cancel_on_tap_outside: true
                });
                
                // Show the Google sign-in prompt
                window.google.accounts.id.prompt();
            } else {
                console.error('Google Identity Services not loaded');
                alert('Google login service is not available. Please try again later.');
            }
        }
    } catch (error) {
        console.error(`${provider} authentication failed: ${error.message}`);
        alert('Login with Google failed. Please try again.');
    } finally {
        button.style.pointerEvents = 'auto';
        button.style.opacity = '1';
    }
}

handleGoogleCallback(response) {
    console.log('Google login callback received');
    
    if (response.credential) {
        // Create a hidden form to submit the Google credential to the server
        const form = document.createElement('form');
        form.method = 'POST';
        form.action = 'Login.aspx';
        
        // Add credential field
        const credentialInput = document.createElement('input');
        credentialInput.type = 'hidden';
        credentialInput.name = 'credential';
        credentialInput.value = response.credential;
        form.appendChild(credentialInput);
        
        document.body.appendChild(form);
        form.submit();
    } else {
        console.error('No credential received from Google');
        alert('Google login failed. Please try again.');
    }
}
```

#### پیاده‌سازی سرور‌ساید
در فایل `Login.aspx.cs`، متد `HandleGoogleLogin` برای پردازش اطلاعات گوگل پیاده‌سازی شد:

```csharp
private void HandleGoogleLogin(string credential)
{
    try
    {
        // Extract basic info from the credential (simplified JWT parsing)
        string email = "";
        string name = "";
        string googleId = "";
        
        // Basic JWT parsing (base64 decode of payload)
        var parts = credential.Split('.');
        if (parts.Length == 3)
        {
            try
            {
                var payload = parts[1];
                payload = payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '=');
                var jsonPayload = Encoding.UTF8.GetString(Convert.FromBase64String(payload));
                var serializer = new JavaScriptSerializer();
                var payloadData = serializer.Deserialize<Dictionary<string, object>>(jsonPayload);
                
                if (payloadData.ContainsKey("email")) email = payloadData["email"].ToString();
                if (payloadData.ContainsKey("name")) name = payloadData["name"].ToString();
                if (payloadData.ContainsKey("sub")) googleId = payloadData["sub"].ToString();
            }
            catch (Exception decodeEx)
            {
                Log($"JWT decode error: {decodeEx.Message}");
            }
        }

        if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(name))
        {
            // Check if user exists in database
            string connectionString = WebConfigurationManager.ConnectionStrings["TodoAppDB"].ConnectionString;
            
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                
                // Check if user exists by email
                string checkQuery = "SELECT Username FROM Users WHERE Email = @Email";
                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@Email", email);
                
                string username = checkCmd.ExecuteScalar()?.ToString();
                
                if (string.IsNullOrEmpty(username))
                {
                    // Create new user from Google account
                    username = name.Replace(" ", "").ToLower() + (googleId.Length >= 6 ? googleId.Substring(0, 6) : googleId);
                    
                    // Ensure username is unique
                    string uniqueUsername = username;
                    int counter = 1;
                    while (true)
                    {
                        string checkUserQuery = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
                        SqlCommand checkUserCmd = new SqlCommand(checkUserQuery, conn);
                        checkUserCmd.Parameters.AddWithValue("@Username", uniqueUsername);
                        int userCount = (int)checkUserCmd.ExecuteScalar();
                        
                        if (userCount == 0) break;
                        
                        uniqueUsername = username + counter;
                        counter++;
                    }
                    
                    // Create user with Google info (no password needed)
                    string insertQuery = "INSERT INTO Users (FullName, Username, Email, PasswordHash, CreatedAt) VALUES (@FullName, @Username, @Email, @PasswordHash, @CreatedAt)";
                    SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                    insertCmd.Parameters.AddWithValue("@FullName", name);
                    insertCmd.Parameters.AddWithValue("@Username", uniqueUsername);
                    insertCmd.Parameters.AddWithValue("@Email", email);
                    insertCmd.Parameters.AddWithValue("@PasswordHash", "GOOGLE_AUTH"); // Special marker for Google auth
                    insertCmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                    
                    insertCmd.ExecuteNonQuery();
                    
                    username = uniqueUsername;
                }
                
                // Login successful - set session and cookie
                Session["User"] = username;
                FormsAuthentication.SetAuthCookie(username, false);
                
                // Redirect to main page
                Response.Redirect("Default.aspx");
            }
        }
        else
        {
            ShowError("email", "خطا در دریافت اطلاعات از گوگل");
        }
    }
    catch (Exception ex)
    {
        Log($"Error in Google login: {ex.Message}");
        ShowError("email", "خطا در ورود با گوگل. لطفا دوباره تلاش کنید.");
    }
}
```

**نکات کلیدی ورود با گوگل:**
1. **بدون نیاز به رمز عبور:** کاربران با حساب گوگل خود می‌توانند بدون وارد کردن رمز عبور وارد شوند.
2. **ایجاد کاربر جدید:** اگر کاربر با ایمیل گوگل وجود نداشته باشد، به‌طور خودکار ایجاد می‌شود.
3. **نام کاربری یکتا:** سیستم نام کاربری منحصربه‌فرد بر اساس نام گوگل و شناسه کاربر ایجاد می‌کند.
4. **شناسه گوگل:** رمز عبور در دیتابیس با مقدار "GOOGLE_AUTH" علامت‌گذاری می‌شود تا نشان دهد این کاربر با گوگل وارد شده است.
5. **امنیت:** در نسخه فعلی، JWT به‌صورت ساده پردازش می‌شود. در محیط تولید باید توکن به‌درستی تأیید شود.

## ساختار واقعی دیتابیس
کدها بر اساس ساختار واقعی جدول `Users` در دیتابیس نوشته شده‌اند که دارای ستون‌های زیر است:
- `UserID` (کلید اصلی)
- `FullName` (اجباری)
- `Username` (اجباری)
- `Email` (اجباری)
- `PasswordHash` (اجباری)
- `CreatedAt` (اجباری)

## نحوه استفاده
برای اجرای پروژه، کافی است برنامه را اجرا کرده و وارد صفحه `SignUp.aspx` شوید. پس از ساخت حساب کاربری، به صفحه `Login.aspx` هدایت می‌شوید تا با اطلاعات خود وارد سیستم شوید. همچنین می‌توانید از طریق دکمه "ورود با گوگل" بدون وارد کردن رمز عبور وارد سیستم شوید.