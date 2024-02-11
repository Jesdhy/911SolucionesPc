using PcSoluciones.Models;
using PcSoluciones.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
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
                        .Where(u => db.Computadora.Any(c => c.num_cedulaClie == u.num_cedula))
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

        public ActionResult New()
        {
            using (var db = new stSolucionesPcEntities())
            {
                // Obtener la lista de números de cédula de los clientes registrados en la base de datos
                var listaCedulas = db.Usuario.Select(u => new SelectListItem
                {
                    Value = u.num_cedula,
                    Text = u.num_cedula
                }).ToList();

                // Agregar un elemento por defecto
                listaCedulas.Insert(0, new SelectListItem { Value = ""});

                // Pasar la lista a la vista
                ViewBag.NumCedulaList = listaCedulas;
            }

            return View();
        }

        [HttpPost]
        public ActionResult Save(ComputadoraVista.ComputadoraModel nuevaComputadora)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var db = new stSolucionesPcEntities())
                    {
                        // Crear una nueva instancia de la entidad Computadora
                        var nuevaComputadoraDB = new Computadora
                        {
                            modelo = nuevaComputadora.modelo,
                            descripcion = nuevaComputadora.descripcion,
                            num_cedulaClie = nuevaComputadora.num_cedulaClie
                        };

                        // Agregar la nueva computadora a la base de datos
                        db.Computadora.Add(nuevaComputadoraDB);
                        db.SaveChanges();

                        return Content("1"); // Indica éxito al cliente
                    }
                }
                catch (Exception ex)
                {
                    // Manejar errores si ocurren
                    return Content(ex.Message);
                }
            }

            // Si hay errores de validación, volver a mostrar el formulario con los errores
            return View("New", nuevaComputadora);
        }

        public ActionResult Edit(int id_computadora)
        {
            ComputadoraVista.ComputadoraModel model = new ComputadoraVista.ComputadoraModel();
            try
            {
                using (stSolucionesPcEntities db = new stSolucionesPcEntities())
                {
                    var computadora = db.Computadora.Find(id_computadora);
                    if (computadora == null)
                    {
                        return HttpNotFound();
                    }

                    model.id_computadora = computadora.id_computadora;
                    model.modelo = computadora.modelo;
                    model.descripcion = computadora.descripcion;
                    model.num_cedulaClie = computadora.num_cedulaClie;
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Update(ComputadoraVista.ComputadoraModel model)
        {
            try
            {
                using (stSolucionesPcEntities db = new stSolucionesPcEntities())
                {
                    var computadora = db.Computadora.Find(model.id_computadora);
                    if (computadora == null)
                    {
                        return HttpNotFound();
                    }

                    computadora.modelo = model.modelo;
                    computadora.descripcion = model.descripcion;
                    computadora.num_cedulaClie = model.num_cedulaClie;

                    db.Entry(computadora).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }



        public ActionResult Delete(int id_computadora)
        {
            try
            {
                using (stSolucionesPcEntities db = new stSolucionesPcEntities())
                {
                    var computadora = db.Computadora.Find(id_computadora);
                    if (computadora == null)
                    {
                        return HttpNotFound();
                    }
                    db.Computadora.Remove(computadora);
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