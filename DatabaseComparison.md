# گزارش مقایسه دیتابیس‌ها (Mahsa vs Yasi)

این گزارش تفاوت‌های ساختاری (Schema) بین دو فایل بکاپ زیر را نشان می‌دهد:
- **Mahsa**: `mahsa.bak` (Restore شده با نام `DB_Mahsa`)
- **Yasi**: `yasi.bak` (Restore شده با نام `DB_Yasi`)

## خلاصه
دیتابیس `mahsa.bak` (که احتمالاً مربوط به SQL Server Reporting Services یا نسخه پیچیده‌تری است) شامل جداول و ویوهای سیستمی بسیار زیادی است که در `yasi.bak` وجود ندارند. دیتابیس `yasi.bak` ساختار ساده‌تری دارد و به نظر می‌رسد نسخه تمیزتر یا متفاوتی از برنامه باشد.

---

## 1. جداول (Tables)

تعداد زیادی جدول در `mahsa.bak` وجود دارد که در `yasi.bak` **نیستند**. لیست زیر فقط شامل جداولی است که در یکی هست و در دیگری نیست:

### موجود در Mahsa اما ناموجود در Yasi
بسیاری از این جداول مربوط به ساختار داخلی SQL Server Reporting Services یا سیستم‌های مدیریت دسترسی پیچیده هستند:
- `ActiveSubscriptions`
- `Batch`
- `CachePolicy`
- `Catalog`
- `ChunkData`
- `ChunkSegmentMapping`
- `ConfigurationInfo`
- `DataModelDataSource`
- `DataSets`
- `DataSource`
- `DBUpgradeHistory`
- `Event`
- `ExecutionLogStorage`
- `Favorites`
- `History`
- `Keys`
- `ModelDrillthrough`
- `ModelItemPolicy`
- `ModelPerspective`
- `Notifications`
- `Policies`
- `ReportSchedule`
- `Roles`
- `RunningJobs`
- `Schedule`
- `SecData`
- `Segment`
- `SegmentedChunk`
- `ServerParameters`
- `ServerUpgradeHistory`
- `SnapshotData`
- `Subscriptions`
- `SystemPolicy`
- `Tasks` (توجه: جدول Tasks ممکن است در هر دو باشد اما ساختار متفاوت داشته باشد، اما در اینجا به عنوان جدول سیستمی لیست شده است)
- `UpgradeInfo`
- `UserProfile`
- `Users` (جدول Users در هر دو وجود دارد اما ساختار آن در Mahsa بسیار پیچیده‌تر است)

### موجود در Yasi اما ناموجود در Mahsa
*هیچ جدولی یافت نشد که در Yasi باشد ولی در Mahsa نباشد.* (این نشان می‌دهد Mahsa احتمالاً تمام جداول Yasi را دارد یا Yasi زیرمجموعه‌ای از آن است).

---

## 2. ستون‌ها (Columns)

تفاوت‌های جزئی در جداول مشترک و تفاوت‌های کلی در جداول اختصاصی:

### تفاوت در جدول `Users`
جدول `Users` در هر دو دیتابیس وجود دارد، اما در `Mahsa` ستون‌های بسیار بیشتری دارد که نشان‌دهنده سیستم احراز هویت پیچیده‌تر است:

**ستون‌های موجود در Mahsa (که در Yasi نیستند):**
- `Setting`
- `Sid`
- `UserType`
- `AuthType`

**ستون‌های موجود در Yasi (که در Mahsa نیستند):**
- `PasswordHash` (در Mahsa احتمالاً مدیریت پسورد متفاوت است)
- `CreatedAt`
- `ProfileImage`

### جداول کاملاً متفاوت
همانطور که ذکر شد، جداول سیستمی زیادی در Mahsa وجود دارد که تمام ستون‌های آن‌ها در Yasi غایب هستند (مانند جداول مربوط به `Catalog`, `Subscriptions`, `History` و ...).

---

## نتیجه‌گیری
- **Mahsa.bak**: به نظر می‌رسد یک دیتابیس سیستمی (مانند ReportServer) یا یک نسخه بسیار پیشرفته با قابلیت‌های گزارش‌گیری و مدیریت کاربران سطح بالا است. حجم فایل (35MB) نیز بسیار بیشتر از Yasi است.
- **Yasi.bak**: یک دیتابیس اپلیکیشن ساده (ToDo App) با حجم کم (5MB) است که جداول اصلی برنامه را دارد.

**پیشنهاد:** اگر هدف توسعه اپلیکیشن ساده ToDo است، دیتابیس `yasi.bak` نسخه صحیح و سبک‌تر می‌باشد. `mahsa.bak` احتمالاً اشتباهاً از یک دیتابیس سیستمی بکاپ گرفته شده است.
