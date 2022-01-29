using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Athenaeum_REST_API.Services
{
    //Service class to implement authentication logic 
    public static class AuthenticationService
    {
        //returns the claimed user id sent
        public static string GetClaim(ClaimsPrincipal User)
        {
            List<Claim> claims = User.Claims.ToList();
            string nameIdentifier = System.Security.Claims.ClaimTypes.NameIdentifier;
            Claim idClaim = claims.Find(c => c.Type == nameIdentifier);
            string claimedUserId = idClaim.Value.ToString();

            return claimedUserId;
        }
    }
}
