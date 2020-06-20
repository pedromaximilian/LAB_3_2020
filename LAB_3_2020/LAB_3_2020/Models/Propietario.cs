using System;
using System.Collections.Generic;

namespace LAB_3_2020.Models
{
    public partial class Propietario
    {
        public Propietario()
        {
            Inmueble = new List<Inmueble>();
        }

        public long PropietarioId { get; set; }
        public string Dni { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }


        public virtual ICollection<Inmueble> Inmueble { get; set; }
    }
}
