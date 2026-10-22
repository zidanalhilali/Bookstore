using Microsoft.EntityFrameworkCore;

namespace Bookstore.Models.Repositories
{
    public class BookDbRepository : IBookstoreRepository<Book>
    {

        private readonly BookstoreDbContext _db;
        public BookDbRepository(BookstoreDbContext db)
        {
            _db = db;
        }

        public IList<Book> GetAll()
        {
            return _db.Books.Include(b => b.Author).ToList();
        }

        public Book? Find(int id)
        {
            return _db.Books.Include(b => b.Author).SingleOrDefault(b => b.Id == id);

        }

        public void Add(Book entity)
        {

            _db.Books.Add(entity);
            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            var book = Find(id);
            if (book is null) return;

            _db.Books.Remove(book);
            _db.SaveChanges();

        }



        public void Update(int id, Book newBook)
        {
            var book = _db.Books.Find(id);
            if (book is null) return;

            book.Title = newBook.Title;
            book.Description = newBook.Description;
            book.AuthorId = newBook.AuthorId;
            book.ImageUrl = newBook.ImageUrl;
            _db.SaveChanges();
        }

        public IList<Book> Search(string? term)
        {
            var query = _db.Books.Include(b => b.Author).AsQueryable();

            if (!string.IsNullOrWhiteSpace(term))
            {
                var text = term.Trim();
                query = query.Where(b => b.Title.Contains(text)
                                      || b.Author.FullName.Contains(text)
                                      || (b.Description != null && b.Description.Contains(text)));
            }

            return query.OrderBy(b => b.Title).ToList();
        }
    }
}
