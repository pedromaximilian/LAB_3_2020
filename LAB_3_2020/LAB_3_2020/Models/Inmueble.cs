using System;
using System.Collections.Generic;

namespace LAB_3_2020.Models
{
    public partial class Inmueble
    {
        public Inmueble()
        {
            Alquiler = new List<Alquiler>();
        }

        public long InmuebleId { get; set; }
        public string Direccion { get; set; }
        public int Ambientes { get; set; }
        public string Tipo { get; set; }
        public string Uso { get; set; }
        public decimal Precio { get; set; }
        public Boolean Disponible { get; set; }
        public long PropietarioId { get; set; }

        public virtual Propietario Propietario { get; set; }
        public virtual ICollection<Alquiler> Alquiler { get; set; }
    }
}
