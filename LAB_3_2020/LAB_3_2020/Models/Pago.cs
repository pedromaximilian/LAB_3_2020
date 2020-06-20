using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LAB_3_2020.Models
{
    public partial class Pago
    {
        public long PagoId { get; set; }
        public int NroPago { get; set; }
        public long AlquilerId { get; set; }
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }
        public decimal Importe { get; set; }

        public virtual Alquiler Alquiler { get; set; }
    }
}
