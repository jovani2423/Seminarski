using Microsoft.AspNetCore.Identity;

namespace RentACar.DAL.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public DateTime RegistrationDate { get; set; }
            = DateTime.Now;
    }
}
