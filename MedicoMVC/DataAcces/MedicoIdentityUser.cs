using Microsoft.AspNetCore.Identity;

namespace MedicoMVC.DataAcces
{
    public class MedicoIdentityUser: IdentityUser
    {
        public string NombreCompleto { get; set; } = default!;  
    }
}
