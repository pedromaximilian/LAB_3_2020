using System;
using System.Collections.Generic;

namespace LAB_3_2020.Models
{
    public partial class Inquilino
    {
        public Inquilino()
        {
            Alquiler = new List<Alquiler>();
        }

        public long InquilinoId { get; set; }
        public string Dni { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }

        public virtual ICollection<Alquiler> Alquiler { get; set; }
    }
}
