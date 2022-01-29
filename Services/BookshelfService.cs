using Athenaeum_REST_API.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Athenaeum_REST_API.Services
{
    public class BookshelfService: IBookshelfService
    {
        private readonly IMongoCollection<Bookshelf> _bookshelves;
        BookService bookService;
        public BookshelfService(IBookshelfDatabaseSettings settings, BookService bookService)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _bookshelves = database.GetCollection<Bookshelf>("bookshelves");
            this.bookService = bookService;
        }

        public IList<Bookshelf> FindAll(string userId)
        {
            var filter = Builders<Bookshelf>.Filter;
            var userIDFilter = filter.Eq(bookshelf => bookshelf.UserID, userId);
            //var filter = Builders<Bookshelf>.Filter.Where(b => b.UserID.Equals(userId));
            return _bookshelves.Find<Bookshelf>(userIDFilter).ToList();
        }

        public Bookshelf Find(string userId, string id)
        {
            var filter = Builders<Bookshelf>.Filter;
            var bookshelfIDFilter = filter.Eq(bookshelf => bookshelf.Id, id);
            var userIDFilter = filter.Eq(bookshelf => bookshelf.UserID, userId);
            //var filter = Builders<Bookshelf>.Filter.Where(b => b.UserID.Equals(userId) && b.Id.Equals(id));
            return _bookshelves.Find<Bookshelf>(bookshelfIDFilter & userIDFilter).FirstOrDefault();
        }

        public Bookshelf Create(Bookshelf bookshelf)
        {
            try
            {
                _bookshelves.InsertOne(bookshelf);
                return bookshelf;
            }
            catch (Exception e)
            {
                return null;
            }

            
            
        }

        public Bookshelf Update(Bookshelf bookshelf)
        {
            var filter = Builders<Bookshelf>.Filter;
            var bookshelfIDFilter = filter.Eq(bookshelf => bookshelf.Id, bookshelf.Id);
            _bookshelves.ReplaceOne(bookshelfIDFilter, bookshelf);
            return bookshelf;
        }

        public void Delete(string id, string userId)
        {
            var filter = Builders<Bookshelf>.Filter;
            var bookshelfIDFilter = filter.Eq(bookshelf => bookshelf.Id, id);
            var userIDFilter = filter.Eq(bookshelf => bookshelf.UserID, userId);

            //delete all books in bookshelf
            bookService.DeleteMany(id, userId);            

            _bookshelves.DeleteOne(bookshelfIDFilter & userIDFilter);
            //_bookshelves.DeleteOne(b => b.Id.Equals(id));
        }

        public Boolean Exists(string id)
        {
            if(id == null)
            {
                return false;
            }

            return _bookshelves.Find<Bookshelf>(b => b.Id.Equals(id)).CountDocuments() > 0;
        }
    }
}
