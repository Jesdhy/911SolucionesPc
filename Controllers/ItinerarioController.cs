using PcSoluciones.Models;
using PcSoluciones.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PcSoluciones.Controllers
{
    public class ItinerarioController : Controller
    {
        // GET: Itinerario
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult List()
        {
            List<ItinerarioVista.UsuarioConItinerarioModel> lst = new List<ItinerarioVista.UsuarioConItinerarioModel>();
            try
            {
                using (stSolucionesPcEntities db = new stSolucionesPcEntities())
                {
                    lst = db.Usuario
                        .Where(u => db.Itinerario.Any(c => c.num_cedulaTec == u.num_cedula))
                        .GroupJoin(db.Itinerario, u => u.num_cedula, c => c.num_cedulaTec, (u, ItineUsuario) => new ItinerarioVista.UsuarioConItinerarioModel
                        {
                            usuario = new ListUsuarios
                            {
                                num_cedula = u.num_cedula,
                                nombre = u.nombre
                            },
                            Itinerarios = ItineUsuario.Select(Itn => new ItinerarioVista.ItinerarioModel
                            {
                                id_itinerario = Itn.id_itinerario,
                                fecha_asignacion = (DateTime)Itn.fecha_asignacion,
                                descripcion = Itn.descripcion,
                                num_cedulaTec = Itn.num_cedulaTec
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

        public ActionResult New()
        {
            using (var db = new stSolucionesPcEntities())
            {
                var listaCedulas = db.Usuario.Select(u => new SelectListItem
                {
                    Value = u.num_cedula,
                    Text = u.num_cedula
                }).ToList();

                listaCedulas.Insert(0, new SelectListItem { Value = "" });
                ViewBag.NumCedulaList = listaCedulas;
            }

            return View();
        }

        [HttpPost]
        public ActionResult Save(ItinerarioVista.ItinerarioModel nuevoItinerario)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var db = new stSolucionesPcEntities())
                    {
                        var nuevoItinerarioDB = new Itinerario
                        {
                            fecha_asignacion = nuevoItinerario.fecha_asignacion,
                            descripcion = nuevoItinerario.descripcion,
                            num_cedulaTec = nuevoItinerario.num_cedulaTec
                        };

                        db.Itinerario.Add(nuevoItinerarioDB);
                        db.SaveChanges();

                        return Content("1"); 
                    }
                }
                catch (Exception ex)
                {
                    return Content(ex.Message);
                }
            }

            return View("New", nuevoItinerario);
        }

        public ActionResult Edit(int id)
        {
            ItinerarioVista.ItinerarioModel model = new ItinerarioVista.ItinerarioModel();
            try
            {
                using (stSolucionesPcEntities db = new stSolucionesPcEntities())
                {
                    var itinerario = db.Itinerario.Find(id);
                    model.id_itinerario = itinerario.id_itinerario;
                    model.fecha_asignacion = (DateTime)itinerario.fecha_asignacion;
                    model.descripcion = itinerario.descripcion;
                    model.num_cedulaTec = itinerario.num_cedulaTec;
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Update(ItinerarioVista.ItinerarioModel model)
        {
            try
            {
                using (stSolucionesPcEntities db = new stSolucionesPcEntities())
                {
                    var itinerario = db.Itinerario.Find(model.id_itinerario);
                    itinerario.fecha_asignacion = model.fecha_asignacion;
                    itinerario.descripcion = model.descripcion;
                    itinerario.num_cedulaTec = model.num_cedulaTec;
                    db.Entry(itinerario).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                using (stSolucionesPcEntities db = new stSolucionesPcEntities())
                {
                    var itinerario = db.Itinerario.Find(id);
                    db.Itinerario.Remove(itinerario);
                    db.SaveChanges();
                }
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
    }
}