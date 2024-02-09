using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PcSoluciones.Models.ViewModel
{
    public class ListComputadora
    {
        public int id_computadora { get; set; }

        public string modelo { get; set; }

        public string descripcion { get; set; }

        public string num_cedulaClie { get; set; }

    }
}