using System;
using System.Collections.Generic;

namespace myproyectapi.Models
{
    public partial class Maquinas
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string So { get; set; }
        public string Ram { get; set; }
        public string Vram { get; set; }
        public string Hdd { get; set; }
        public string Url { get; set; }
        public string UsuarioRdp { get; set; }
        public string PassRdp { get; set; }
        public int UsuarioId { get; set; }
        public sbyte State { get; set; }
        public int Port { get; set; }

        public virtual Usuarios Usuario { get; set; }
    }
}
