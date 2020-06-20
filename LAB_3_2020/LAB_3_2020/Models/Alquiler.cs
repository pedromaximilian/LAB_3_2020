using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LAB_3_2020.Models
{
    public partial class Alquiler
    {
        public Alquiler()
        {
            Pago = new List<Pago>();
        }

        public long AlquilerId { get; set; }
        public decimal Precio { get; set; }
        [DataType(DataType.Date)]
        public DateTime Inicio { get; set; }
        [DataType(DataType.Date)]
        public DateTime Fin { get; set; }
        public long InquilinoId { get; set; }
        public long InmuebleId { get; set; }


       
        public virtual Inmueble Inmueble { get; set; }
        public virtual Inquilino Inquilino { get; set; }
        public ICollection<Pago> Pago { get; set; }
    }
}
