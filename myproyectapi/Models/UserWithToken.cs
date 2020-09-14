using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myproyectapi.Models
{
    public class UserWithToken : Usuarios
    {
        public string Token { get; set; }
        public UserWithToken(Usuarios usuario)
        {
            this.Id = usuario.Id;
            this.Nombre = usuario.Nombre;
            this.Email = usuario.Email;
            this.Pass = usuario.Pass;
        }
     }
}
