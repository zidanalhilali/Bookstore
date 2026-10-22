using Bookstore.Models;
using Bookstore.Models.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Controllers
{
    public class AuthorController : Controller
    {
        private readonly IBookstoreRepository<Author> _authorRepository;
        public AuthorController(IBookstoreRepository<Author> authorRepository)
        {
            _authorRepository = authorRepository;
        }
        // GET: AuthorController
        public ActionResult Index(string? search)
        {
            ViewData["Search"] = search;
            return View(_authorRepository.Search(search));
        }

        // GET: AuthorController/Details/5
        public ActionResult Details(int id)
        {
            var author = _authorRepository.Find(id);
            if (author is null) return NotFound();

            return View(author);
        }

        // تعرض النموذج.
        // GET: AuthorController/Create
        public ActionResult Create()
        {
            return View();
        }

        // تستقبل البيانات التي أرسلها المستخدم.
        // POST: AuthorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("FullName")] Author author)
        {
            if (!ModelState.IsValid) return View(author);

                _authorRepository.Add(author);
                return RedirectToAction(nameof(Index));
          
        }

        // GET: AuthorController/Edit/5
        public ActionResult Edit(int id)
        {
            var author = _authorRepository.Find(id);
            if (author is null) return NotFound();

            return View(author);
        }

        // POST: AuthorController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id,[Bind("FullName")] Author author)
        {
            if (!ModelState.IsValid)
            {
                author.Id = id;
                return View(author);
            }
           
            if (_authorRepository.Find(id) is null) return NotFound();

                _authorRepository.Update(id, author);
                return RedirectToAction(nameof(Index));
 
        }

        // GET: AuthorController/Delete/5 (صفحة التأكيد)
        public ActionResult Delete(int id)
        {
            var author = _authorRepository.Find(id);
            if (author is null) return NotFound();
                
            return View(author);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var author = _authorRepository.Find(id);
            if (author is null) return NotFound();

            if (author.Books.Count > 0)
            {
                ModelState.AddModelError(string.Empty,
                    "Cannot delete an author who still has books. Delete or reassign their books first.");
                return View(author);
            }

            _authorRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
