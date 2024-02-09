using PcSoluciones.Models;
using PcSoluciones.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static PcSoluciones.Models.ViewModel.ComputadoraVista;

namespace PcSoluciones.Controllers
{
    public class ComputadoraController : Controller
    {
        private stSolucionesPcEntities db = new stSolucionesPcEntities();
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult List()
        {
            List<ComputadoraVista.UsuarioConComputadoraModel> lst = new List<ComputadoraVista.UsuarioConComputadoraModel>();
            try
            {
                using (stSolucionesPcEntities db = new stSolucionesPcEntities())
                {
                    lst = db.Usuario
                        .GroupJoin(db.Computadora, u => u.num_cedula, c => c.num_cedulaClie, (u, CompUsuario) => new ComputadoraVista.UsuarioConComputadoraModel
                        {
                            usuario = new ListUsuarios
                            {
                                num_cedula = u.num_cedula,
                                nombre = u.nombre
                            },
                            Computadoras = CompUsuario.Select(compt => new ComputadoraVista.ComputadoraModel
                            {
                                id_computadora = compt.id_computadora,
                                modelo = compt.modelo,
                                descripcion = compt.descripcion,
                                num_cedulaClie = compt.num_cedulaClie,
                            }).ToList()
                        })
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View(lst);
        }
    }

}