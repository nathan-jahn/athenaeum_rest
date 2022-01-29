using Athenaeum_REST_API.Models;
using Athenaeum_REST_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Athenaeum_REST_API.Controllers
{
    //base route "api/book"
    [Route("api/[controller]")]
    [ApiController]

    //require authroization for all exposed endpoints
    [Authorize]

    //Controller class which exposes BookService logic to consumable REST API endpoints
    public class BookController : ControllerBase
    {

        private readonly IBookService _bookService;

        //BookService is a Dependency Injected Singleton --see Startup.cs for details
        public BookController(BookService bookService)
        {
            _bookService = bookService;
        }

        //Returns JSON for all books with a particular bookshelf queryed by the bookshelf_id
        //Request example: GET: api/book/bookshelf_id
        [HttpGet("shelf/{bookshelfID:length(24)}")]
        public IActionResult GetAll(string bookshelfID)
        {
            string userID = AuthenticationService.GetClaim(User);
            return Ok(_bookService.FindAll(bookshelfID, userID));
        }
        
        //Returns JSON for a particular book queryed by it's id
        // Request example: GET api/book/book_id
        [HttpGet("{id:length(24)}")]
        public IActionResult Get(string id)
        {
            string userID = AuthenticationService.GetClaim(User);
            return Ok(_bookService.Find(id, userID));
        }
        
        //Posts book data to books collection
        //Request example: POST api/book
        [HttpPost]
        public IActionResult Post(Book book)
        {
            var res = _bookService.Create(book);
            if(res == null)
            {
                return StatusCode(409);
            }
            else
            {
                return Ok(book);
            }
            
        }

        //Updates a book in the books collection
        //Request example: PUT api/book
        [HttpPost("update")]
        public IActionResult Update(Book book)
        {
            //verifies that the book exists in the database
            if (book.Id != null && _bookService.Exists(book.Id))
            {
                _bookService.Update(book, AuthenticationService.GetClaim(User));
                return CreatedAtRoute(new { id = book.Id.ToString() }, book);
            }
            //if the book does not exist in the database return a 204 status code
            else
            {
                return NoContent();
            }
        }

        //Deletes a particular book queryed by its id
        //Request Example: DELETE api/book/book_id
        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            //verifies that the book exists in the database
            if (_bookService.Exists(id))
            {
                _bookService.Delete(id, AuthenticationService.GetClaim(User));
                return Ok();
            }
            //if the book does not exist in the database return a 204 status code
            else
            {
                return NoContent();
            }

        }

        /* Old code which was abstracted to Authentication Service to maintain the Single Responsibility Principle
        private string UserId()
        {
            string nameID = System.Security.Claims.ClaimTypes.NameIdentifier;

            List<Claim> claims = User.Claims.ToList();
            Claim idClaim = claims.Find(c => c.Type == nameID);

            return idClaim.Value.ToString();
        }
        */
    }
}
