using Athenaeum_REST_API.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Athenaeum_REST_API.Services
{
    public class BookService: IBookService
    {
        private readonly IMongoCollection<Book> _books;

        public BookService(IBookshelfDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _books = database.GetCollection<Book>("books");
        }

        //Returns all books from the books collections that match the bookshelfID and userID
        public IList<Book> FindAll(string bookshelfId, string userID)
        {
            var filter = Builders<Book>.Filter;
            var bookshelfIDFilter = filter.Eq(book => book.BookshelfId, bookshelfId);
            var userIDFilter = filter.Eq(book => book.UserID, userID);

            //return _books.Find<Book>(book => book.BookshelfId.Equals(bookshelfId)).ToList();

            return _books.Find(bookshelfIDFilter & userIDFilter).ToList();

            /*
            IMongoCollection<Book> books;

            var client = new MongoClient(
                "mongodb+srv://jahnnathan:Nj5t8k0115@main.plmey.mongodb.net/bookshelf_db"
            );
            var database = client.GetDatabase("bookshelf_db");

            books = database.GetCollection<Book>("books");



            List<Book> res = books.Find<Book>(_ => true).ToList();

            return res;
            */
        }

        //return the book in the books collection that matches the id and userid
        public Book Find(string id, string userID)
        {
            var filter = Builders<Book>.Filter;
            var bookIDFilter = filter.Eq(book => book.Id, id);
            var userIDFilter = filter.Eq(book => book.UserID, userID);

            return _books.Find<Book>(bookIDFilter & userIDFilter).FirstOrDefault();

            //return _books.Find<Book>(book => book.Id.Equals(id)).FirstOrDefault();
        }

        //creates the requested book in the books collection
        public Book Create(Book book)
        {
            try{
                _books.InsertOne(book);
                return book;
            }
            catch(Exception e)
            {
                return null;
            }
            
        }

        //updates the selected book in the books collection
        public Book Update(Book book, string userID)
        {
            var filter = Builders<Book>.Filter;
            var userIDFilter = filter.Eq(book => book.UserID, userID);
            var bookIDFilter = filter.Eq(b => b.Id, book.Id);
              _books.ReplaceOne(userIDFilter & bookIDFilter, book);
            return book;
        }

        //deletes the book that matches the id and userid in the books collection
        public void Delete(string id, string userID)
        {
            var filter = Builders<Book>.Filter;
            var userIDFilter = filter.Eq(book => book.UserID, userID);
            var bookIDFilter = filter.Eq(book => book.Id, id);

            _books.DeleteOne(userIDFilter & bookIDFilter);

           // _books.DeleteOne(b => b.Id.Equals(id));
        }

        //deletes all the books in the books colletion that match the bookshelfID and userID
        public void DeleteMany(string bookshelfID, string userID)
        {
            var filter = Builders<Book>.Filter;
            var userIDFilter = filter.Eq(book => book.UserID, userID);
            var bookshelfIDFilter = filter.Eq(book => book.BookshelfId, bookshelfID);

            _books.DeleteMany(userIDFilter & bookshelfIDFilter);
        }

        //checks if a book with the requested id exists in the books collections
        public Boolean Exists(string id)
        {
            if (id == null)
            {
                return false;
            }

            return _books.Find<Book>(book => book.Id.Equals(id)).CountDocuments() > 0;
        }
    }
}
