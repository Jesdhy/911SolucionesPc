using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PcSoluciones.Models.ViewModel
{
    public class InformeFalloVista
    {

        public class InformeModel
        {
            public int id_informe { get; set; }
            public string estado { get; set; }
            public string repuesto { get; set; }

            public DateTime fecha_aprox { get; set; }

            public int idComput { get; set; }
        }

        public class ComputadoraConInformeFallo
        {
            public ListComputadora computadora { get; set; }
            public List<InformeModel> informes { get; set; }
        }

    }
}