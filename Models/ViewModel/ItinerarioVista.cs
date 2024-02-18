using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PcSoluciones.Models.ViewModel
{
    public class ItinerarioVista
    {
        public class ItinerarioModel
        {
        public int id_itinerario { get; set; }

        public DateTime fecha_asignacion { get; set; }

        public string descripcion { get; set; }

        public string num_cedulaTec { get; set; }
    }

        public class UsuarioConItinerarioModel
        {
            public ListUsuarios usuario { get; set; }
            public List<ItinerarioModel> Itinerarios { get; set; }
        }
    }
}