using Athenaeum_REST_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Athenaeum_REST_API.Services
{
    public interface IBookService
    {
        //returns all books with bookshelfID and userID from books collection
        IList<Book> FindAll(string bookshelfId, string userID);

        //returns a single book from the books collection that matches the userID and id
        Book Find(string id, string userID);

        //creates a book in the books collection
        Book Create(Book book);

        //updates a book in the books collection
        Book Update(Book book, string userID);

        //deletes a book in the books colletion that match the requested id and userID
        void Delete(string id, string userID);

        //deletes any book in the booka colletion that match the requested bookshelf_id
        void DeleteMany(string bookshelfID, string userID);

        //checks if a book exists in the books collection that matches the requested id
        Boolean Exists(string id);
    }
}
