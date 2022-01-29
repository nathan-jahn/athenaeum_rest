using Athenaeum_REST_API.Models;
using Athenaeum_REST_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Athenaeum_REST_API.Controllers
{
    //base route api/bookshelf
    [Route("api/[controller]")]
    [ApiController]

    //require authroization for all exposed endpoints
    [Authorize]

    //Controller class which exposes BookshelfService logic to consumable REST API endpoints
    public class BookshelfController : ControllerBase
    {
        private readonly IBookshelfService _bookshelfService;

        //BookshelfService is a Dependency Injected Singleton --see Startup.cs for details
        public BookshelfController(BookshelfService bookshelfService)
        {
            _bookshelfService = bookshelfService;
        }

        //Returns JSON for all booksheleves that match a particular user_id
        //Request example: GET: api/bookshelf
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_bookshelfService.FindAll(AuthenticationService.GetClaim(User)));
        }

        //Posts bookshelf data to bookshelves collection
        //Request example: POST api/bookshelf
        [HttpPost]
        public IActionResult Post(Bookshelf bookshelf)
        {

            var res = _bookshelfService.Create(bookshelf);
            if (res == null)
            {
                return StatusCode(409);
            }
            else
            {
                return Ok(res);
            }

        }

        //Updates a bookshelf in the bookshelf collection
        //Request example: PUT api/bookshelf
        [HttpPost("update")]
        public IActionResult Update(Bookshelf bookshelf)
        {
            //verify that the bookshelf exists in the database
            if (bookshelf.Id != null && _bookshelfService.Exists(bookshelf.Id))
            {
                _bookshelfService.Update(bookshelf);
                return CreatedAtRoute(new { id = bookshelf.Id.ToString() }, bookshelf);
            }
            //if the bookshelf does not exist in the database return a 204 status code
            else
            {
                return NoContent();
            }
        }

        //Deletes a particular bookshelf queryed by its id
        //Request Example: DELETE api/bookshelf/bookshelf_id
        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            //verifies that the bookshelf exists in the database
            if (_bookshelfService.Exists(id))
            {
                _bookshelfService.Delete(id, AuthenticationService.GetClaim(User));
                return Ok();
            }
            //if the bookshelf does not exist in the database return a 204 status code
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
