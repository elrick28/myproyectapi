using System;
using System.Collections.Generic;

namespace myproyectapi.Models
{
    public partial class Usuarios
    {
        public Usuarios()
        {
            Maquinas = new HashSet<Maquinas>();
        }

        public int Id { get; set; }
        public string Email { get; set; }
        public string Nombre { get; set; }
        public string Pass { get; set; }

        public virtual ICollection<Maquinas> Maquinas { get; set; }
    }
}
