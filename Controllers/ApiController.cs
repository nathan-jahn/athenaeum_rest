using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Athenaeum_REST_API.Models;
using Athenaeum_REST_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Newtonsoft.Json;

//this was a simple controller that I used when learning how to implement and use user authentication

namespace rest_auth_example.Controllers
{
    [Route("api")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly BookService _bookSvc;
        public ApiController(BookService svc)
        {
            _bookSvc = svc;
        }

        // GET: api/<ApiController>
        [HttpGet("public")]
        public IActionResult Public()
        {
            return Ok(new
            {
                Message = "Hello from a public endpoint! You don't need to be authenticated to see this."
            });
        }


        // GET: api/<ApiController>
        [HttpGet("private")]
        [Authorize]
        public IActionResult Private()
        {
            String userID = getID();

            return Ok(new
            {
                Message = "Hello " + userID + " from a private endpoint! You need to be authenticated to see this."
            });
        }

        // This endpoint shows the NameIdentifier of the Authorization token
        [HttpGet("claims")]
        public IActionResult Claims()
        {
            string nameIdURI = System.Security.Claims.ClaimTypes.NameIdentifier;

            List<Claim> claims = User.Claims.ToList();

            Claim idClaim = claims.Find(c => c.Type == nameIdURI);

            return Ok("Claims:\n" + idClaim.ToString());
        }

        private string getID()
        {
           string nameID = System.Security.Claims.ClaimTypes.NameIdentifier;

            List<Claim> claims = User.Claims.ToList();
            Claim idClaim = claims.Find(c => c.Type == nameID);

            return idClaim.Value.ToString();
        }


        /*
        [HttpGet("bookshelf")]
        public IActionResult bookshelf()
        {
            IMongoCollection<Bookshelf> bookshelves;

            var client = new MongoClient(
                "mongodb+srv://jahnnathan:Nj5t8k0115@main.plmey.mongodb.net/bookshelf_db"
            );
            var database = client.GetDatabase("bookshelf_db");

            bookshelves = database.GetCollection<Bookshelf>("bookshelves");



            List<Bookshelf> res = bookshelves.Find<Bookshelf>(bs => bs.UserID == getID()).ToList();

            return Ok(res);

        }
        */
    }
}
