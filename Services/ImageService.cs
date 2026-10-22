using Microsoft.AspNetCore;

namespace Bookstore.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private static readonly string[] AllowedExtensions = { ".jpeg", ".jpg", ".png", ".webp" };
        //لماذا static؟ المصفوفة نفسها لا تتغير بين الطلبات، فنُنشئها مرة واحدة ويشترك فيها الجميع بدل إنشائها مع كل طلب.
        private const long MaxSize = 5 * 1024 * 1024; // 5 ميجا 

        public ImageService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public void Delete(string? fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return;

            // Path.GetFileName يمنع محاولات الخروج من المجلد (../)
            var safeName = Path.GetFileName(fileName);
            var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", safeName);


            if (File.Exists(fullPath)) 
                File.Delete(fullPath);
        }

        public string Save(IFormFile file)
        {
            // 1) فحص الحجم  
            if (file.Length > MaxSize)
                throw new InvalidOperationException("حجم الصورة يجب ألا يزيد عن 5 ميجا");

            // 2) الفحص الامتداد
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(ext))
                throw new InvalidOperationException("نوع الملف غير مسموح");
            // نظام القائمة البيضاء (Whitelist):
            // لاحظ أننا كتبنا ما هو مسموح فقط، ولم نكتب قائمة بالممنوعات (.exe, .php...).
            // السبب أن قائمة الممنوعات لا تنتهي، وستنسى شيئاً دائماً. هذه قاعدة أمنية ذهبية: اسمح بما تعرفه، وامنع كل ما عداه.

            // 3) تجهيز المجلد
            var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            Directory.CreateDirectory(uploads);

            // 4) اسم فريد
            var fileName = $"{Guid.NewGuid()}{ext}";
            var fullPath = Path.Combine(uploads, fileName);

            // 5) الحفظ
            using var stream = new FileStream(fullPath, FileMode.Create);
            file.CopyTo(stream);

            return fileName;
        }
    }
}
