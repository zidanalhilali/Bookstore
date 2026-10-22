using Microsoft.EntityFrameworkCore;

namespace Bookstore.Models.Repositories
{
    public class AuthorDbRepository : IBookstoreRepository<Author>
    {
        private readonly BookstoreDbContext _db;
        public AuthorDbRepository(BookstoreDbContext db)
        {
            _db = db;
        }

        public IList<Author> GetAll()
        {
            return _db.Authors.ToList();
        }

        public Author? Find(int id)
        {
            return _db.Authors.Include(a => a.Books).SingleOrDefault(a => a.Id == id);
        }

        public void Add(Author entity)
        {
            _db.Authors.Add(entity);
            _db.SaveChanges();
        }

        public void Update(int id, Author newAuthor)
        {
            var author = Find(id);
            if (author is null) return;

            author.FullName = newAuthor.FullName;
            _db.SaveChanges();

        }

        public void Delete(int id)
        {
            var author = Find(id);
            if (author is null) return;


            _db.Authors.Remove(author);
            _db.SaveChanges();

        }

        public IList<Author> Search(string? term)
        {
            var query = _db.Authors.AsQueryable();

            if (!string.IsNullOrWhiteSpace(term))
            {
                var text = term.Trim();
                query = query.Where(a => a.FullName.Contains(text));
            }

            return query.OrderBy(a => a.FullName).ToList();
        }
    }
}
