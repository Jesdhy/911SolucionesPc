using PcSoluciones.Models;
using PcSoluciones.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PcSoluciones.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Usuario
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult List()
        {
            List<ListUsuarios> lst = new List<ListUsuarios>();
            using (stSolucionesPcEntities db = new stSolucionesPcEntities())
            {
                lst =
                    (from d in db.Usuario 
                     orderby d.num_cedula descending
                     select new ListUsuarios
                     {
                         num_cedula = d.num_cedula,
                         nombre = d.nombre,
                         nom_usuario = d.nom_usuario,
                     }).ToList();
            }
            return View(lst);
        }

        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Save(UsuarioVista model)
        {
            try
            {
                using (stSolucionesPcEntities db = new stSolucionesPcEntities())
                {
                    var oUsuario = new Usuario();
                    oUsuario.num_cedula = model.num_cedula;
                    oUsuario.nombre = model.nombre;
                    oUsuario.nom_usuario = model.nom_usuario;
                    oUsuario.clave = model.clave;
                    db.Usuario.Add(oUsuario);
                    db.SaveChanges();
                }
                return Content("1");
            }

            catch (Exception ex)
            {
                return Content(ex.Message);
            }

        }


        public ActionResult Edit(string num_cedula)
        {
            UsuarioVista model = new UsuarioVista();
            try
            {
                using (stSolucionesPcEntities db = new stSolucionesPcEntities())
                {
                    var oEmpleado = db.Usuario.Find(num_cedula);
                    model.nombre = oEmpleado.nombre;
                    model.nom_usuario = oEmpleado.nom_usuario;
                    model.clave = oEmpleado.clave;
                    model.num_cedula = oEmpleado.num_cedula;
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
            return View(model);
        }



        public ActionResult Update(UsuarioVista model)
        {
            try
            {
                using (stSolucionesPcEntities db = new stSolucionesPcEntities())
                {
                    var oUsuario = db.Usuario.Find(model.num_cedula);
                    oUsuario.nombre = model.nombre;
                    oUsuario.nom_usuario = model.nom_usuario;
                    oUsuario.clave = model.clave;
                    db.Entry(oUsuario).State = System.Data.Entity.EntityState.Modified;
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
        public ActionResult Delete(string num_cedula)
        {
            try
            {
                using (stSolucionesPcEntities db = new stSolucionesPcEntities())
                {
                    var oUsuario = db.Usuario.Find(num_cedula);
                    db.Usuario.Remove(oUsuario);
                    db.SaveChanges();
                }
                return Content("1");
            }

            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }


        public ActionResult ListBox()
        {
            return View();
        }

    }
}