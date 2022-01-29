using Athenaeum_REST_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Athenaeum_REST_API.Services
{   
    interface IBookshelfService
    {
        //returns all bookshelfs with userID from bookshelves collection
        IList<Bookshelf> FindAll(String userID);

        //returns a single bookshelf from the bookshelves collection that matches the userID and id
        Bookshelf Find(string userId, string id);

        //creates a bookshelf in the bookshelves collection
        Bookshelf Create(Bookshelf bookshelf);

        //updates a bookshelf in the bookshelves collection
        Bookshelf Update(Bookshelf bookshelf);

        //deletes a bookshelf in the bookshelves colletion that match the requested id and userID
        void Delete(string id, string userId);

        //checks if a bookshelf exists in the bookshelves collection that matches the requested id
        Boolean Exists(string id);
    }
}
