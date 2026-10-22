using Bookstore.Models;
using Bookstore.Models.Repositories;
using Bookstore.Services;
using Bookstore.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookstoreRepository<Book> _bookRepository;
        private readonly IBookstoreRepository<Author> _authorRepository;
        private readonly IImageService _imageService;

        public BookController(
            IBookstoreRepository<Book> bookRepository,
            IBookstoreRepository<Author> authorRepository,
            IImageService imageService)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _imageService = imageService;
        }

        // GET: Book
        public ActionResult Index(string? search)
        {
            ViewData["Search"] = search;
            return View(_bookRepository.Search(search));
        }

        // GET: Book/Details/5
        public ActionResult Details(int id)
        {
            var book = _bookRepository.Find(id);
            if (book is null) return NotFound();

            return View(book);
        }

        // GET: Book/Create
        public ActionResult Create()
        {
            return View(new BookFormViewModel
            {
                Authors = _authorRepository.GetAll()
            });
        }

        // POST: Book/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookFormViewModel model)
        {
            if (!ModelState.IsValid) return FormView(model);

            var authorId = model.AuthorId!.Value;
            if (_authorRepository.Find(authorId) is null)
            {
                ModelState.AddModelError(nameof(model.AuthorId), "The selected author does not exist.");
                return FormView(model);
            }

            // 1) حفظ الصورة (إن وُجدت)
            string? fileName = null;
            if (model.File is not null)
            {
                try
                {
                    fileName = _imageService.Save(model.File);
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError(nameof(model.File), ex.Message);
                    return FormView(model);
                }
            }

            // 2) حفظ الكتاب، وإن فشل نحذف الصورة التي رفعناها للتو
            try
            {
                _bookRepository.Add(new Book
                {
                    Title = model.Title,
                    Description = model.Description,
                    AuthorId = authorId,
                    ImageUrl = fileName
                });
            }
            catch
            {
                _imageService.Delete(fileName);
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Book/Edit/5
        public ActionResult Edit(int id)
        {
            var book = _bookRepository.Find(id);
            if (book is null) return NotFound();

            return View(new BookFormViewModel
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                AuthorId = book.AuthorId,
                ImageUrl = book.ImageUrl,
                Authors = _authorRepository.GetAll()
            });
        }

        // POST: Book/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BookFormViewModel viewModel)
        {
            var oldBook = _bookRepository.Find(viewModel.Id);
            if (oldBook is null) return NotFound();

            // نحفظ اسم الصورة القديمة الآن، قبل أي تعديل (انظر الشرح)
            var oldImage = oldBook.ImageUrl;
            viewModel.ImageUrl = oldImage;

            if (!ModelState.IsValid) return FormView(viewModel);

            var authorId = viewModel.AuthorId!.Value;
            if (_authorRepository.Find(authorId) is null)
            {
                ModelState.AddModelError(nameof(viewModel.AuthorId), "The selected author does not exist.");
                return FormView(viewModel);
            }

            // 1) حفظ الصورة الجديدة (إن وُجدت)
            string? newFileName = null;
            if (viewModel.File is not null)
            {
                try
                {
                    newFileName = _imageService.Save(viewModel.File);
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError(nameof(viewModel.File), ex.Message);
                    return FormView(viewModel);
                }
            }

            // 2) تحديث قاعدة البيانات، وإن فشل نحذف الصورة الجديدة ونُبقي القديمة
            try
            {
                _bookRepository.Update(viewModel.Id, new Book
                {
                    Title = viewModel.Title,
                    Description = viewModel.Description,
                    AuthorId = authorId,
                    ImageUrl = newFileName ?? oldImage
                });
            }
            catch
            {
                _imageService.Delete(newFileName);
                throw;
            }

            // 3) بعد نجاح القاعدة فقط، نحذف الصورة القديمة
            if (newFileName is not null)
                _imageService.Delete(oldImage);

            return RedirectToAction(nameof(Index));
        }

        // GET: Book/Delete/5
        public ActionResult Delete(int id)
        {
            var book = _bookRepository.Find(id);
            if (book is null) return NotFound();

            return View(book);
        }

        // POST: Book/ConfirmDelete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmDelete(int id)
        {
            var book = _bookRepository.Find(id);
            if (book is null) return NotFound();

            var imageUrl = book.ImageUrl;

            _bookRepository.Delete(id);        // الأهم أولاً
            _imageService.Delete(imageUrl);    // ثم التنظيف

            return RedirectToAction(nameof(Index));
        }

        // يعيد عرض النموذج مع قائمة المؤلفين (تتكرر في أكثر من مكان)
        private ActionResult FormView(BookFormViewModel model)
        {
            model.Authors = _authorRepository.GetAll();
            return View(model);
        }
    }
}