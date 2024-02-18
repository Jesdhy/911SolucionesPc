using PcSoluciones.Models.ViewModel;
using PcSoluciones.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace PcSoluciones.Controllers
{
    public class InformeFalloController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult List()
        {
            List<InformeFalloVista.ComputadoraConInformeFallo> lst = new List<InformeFalloVista.ComputadoraConInformeFallo>();
            try
            {
                using (stSolucionesPcEntities db = new stSolucionesPcEntities())
                {
                    lst = db.InformeFallo
                        .Where(infr => infr.Computadora != null)
                        .Select(infr => new InformeFalloVista.ComputadoraConInformeFallo
                        {
                            computadora = new ListComputadora
                            {
                                id_computadora = infr.Computadora.id_computadora,
                                modelo = infr.Computadora.modelo
                            },
                            informes = new List<InformeFalloVista.InformeModel>
                            {
                        new InformeFalloVista.InformeModel
                        {
                            id_informe = infr.id_informe,
                            estado = infr.estado,
                            repuesto = infr.repuesto,
                            fecha_aprox = (DateTime)infr.fecha_aprox
                        }
                            }
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
                var listaIdsComputadora = db.Computadora.Select(c => new SelectListItem
                {
                    Value = c.id_computadora.ToString(),
                    Text = c.id_computadora.ToString() 
                }).ToList();

                listaIdsComputadora.Insert(0, new SelectListItem { Value = ""});

                ViewBag.IdComputadoraList = listaIdsComputadora;
            }

            return View();
        }

        [HttpPost]
        public ActionResult Save(InformeFalloVista.InformeModel nuevoInforme)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var db = new stSolucionesPcEntities())
                    {
                        var nuevoInformeDB = new InformeFallo
                        {
                            idComput = nuevoInforme.idComput, 
                            estado = nuevoInforme.estado,
                            repuesto = nuevoInforme.repuesto,
                            fecha_aprox = nuevoInforme.fecha_aprox
                        };

                        db.InformeFallo.Add(nuevoInformeDB);
                        db.SaveChanges();

                        return Content("1"); 
                    }
                }
                catch (Exception ex)
                {
                    return Content(ex.Message);
                }
            }
            return View("New", nuevoInforme);
        }



        public ActionResult Edit(int id)
        {
            InformeFalloVista.InformeModel model = new InformeFalloVista.InformeModel();
            try
            {
                using (stSolucionesPcEntities db = new stSolucionesPcEntities())
                {
                    var informe = db.InformeFallo.Include("Computadora").FirstOrDefault(i => i.id_informe == id);

                    if (informe != null)
                    {
                        model.id_informe = informe.id_informe;
                        model.estado = informe.estado;
                        model.repuesto = informe.repuesto;
                        model.fecha_aprox = (DateTime)informe.fecha_aprox;
                    }
                    else
                    {
                        return Content("Informe de fallo no encontrado");
                    }
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
            return View(model);
        }


        [HttpPost]
        public ActionResult Update(InformeFalloVista.InformeModel model)
        {
            try
            {
                using (stSolucionesPcEntities db = new stSolucionesPcEntities())
                {
                    var informe = db.InformeFallo.Find(model.id_informe);
                    informe.idComput = model.idComput;
                    informe.estado = model.estado;
                    informe.repuesto = model.repuesto;
                    informe.fecha_aprox = model.fecha_aprox;
                    db.Entry(informe).State = System.Data.Entity.EntityState.Modified;
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
                    var informe = db.InformeFallo.Find(id);
                    db.InformeFallo.Remove(informe);
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