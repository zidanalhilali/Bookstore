namespace Bookstore.Models
{
    public static class DbSeeder
    {
        public static void Seed(BookstoreDbContext db)
        {
            if (db.Authors.Any()) return;   // لا نكرر البيانات

            db.Authors.AddRange(
                new Author
                {
                    FullName = "Naguib Mahfouz",
                    Books =
                    {
                        new Book { Title = "Palace Walk", Description = "First novel of the Cairo Trilogy." }
                    }
                },
                new Author
                {
                    FullName = "Agatha Christie",
                    Books =
                    {
                        new Book { Title = "Murder on the Orient Express", Description = "A Hercule Poirot mystery." }
                    }
                },
                new Author
                {
                    FullName = "Robert C. Martin",
                    Books =
                    {
                        new Book { Title = "Clean Code", Description = "A handbook of agile software craftsmanship." }
                    }
                });

            db.SaveChanges();
        }
    }
}