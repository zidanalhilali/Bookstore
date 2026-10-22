namespace Bookstore.Services
{
    public interface IImageService
    {
         string Save(IFormFile file);   // يحفظ ويرجع الاسم الجديد
        void Delete(string? fileName);  // يحذف الصورة
    }
}
