using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SistemasTurnos.Models;

namespace SistemasTurnos.Controllers
{
    public class HomeController : Controller
    {
        private dbServicioTurnosEntities db = new dbServicioTurnosEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Logearse(string user, string pwd)
        {
            var usuario = db.Usuarios.Where(u => u.UserName == user && u.Password == pwd).SingleOrDefault();
            return Json(usuario);
        }


        [HttpPost]
        public JsonResult getServices()
        { 
            return Json(db.Servicios.ToList());
        }
        
        [HttpPost]
        public JsonResult setTurno(int id)
        {
            var today = DateTime.Now.Date;
            var fecha = today.ToString().Split(' ')[0];
            var hoy=DateTime.Now.ToShortTimeString();
            var hora = Convert.ToInt32(hoy.ToString().Split(':')[0]);
            var tanda = hoy.ToString().Split(' ')[1];
            //var tanda2 = today.
            if ((hora >= 12 && (tanda == "a.m." || tanda == "A.M.")) || (hora >= 6 && tanda == "p.m." || tanda == "P.M.")) {

                return Json("Sistema de turnos cerrado, vuelva luego");
            }
            
            var currentService = db.Turnos.Where(t => t.TipoServicio == id && t.Fecha== fecha)
                .OrderByDescending(t=>t.id)
                .Select(t => t.Turno).FirstOrDefault();
            int? order = 1;
            if (currentService != null) order = currentService + order;

            var turno = new Turnos();
            turno.TipoServicio = id;
            turno.Fecha = fecha;
            turno.Habilitado = true;
            turno.Turno = order;

            db.Turnos.Add(turno);
            db.SaveChanges();

            var codService = db.Servicios.Where(s => s.id == id).FirstOrDefault();

            //var totalturnos = db.Turnos.Where(t => t.id == id && t.Habilitado == true
            // && t.Fecha == fecha /*&& t.Despachado != true*/).ToList();

            int mins = Convert.ToInt32(codService.TiempoEstimado);
            var tiempoEstimado = mins * (order);


            return Json($"{ codService.CodigoServicio }-{ order } { tiempoEstimado }");
        }

       

        [HttpPost]
        public JsonResult getTickets()
        {
            List<fmtTurnos> lista = new List<fmtTurnos>();
            var today = DateTime.Now.Date;
            var fecha = today.ToString().Split(' ')[0];
            var tickets = db.Turnos.Where(t => t.Fecha == fecha && (t.Despachado == null || t.Despachado == false) && t.Habilitado == true && (t.atendido == null || t.atendido == false)).ToList();
            int turno = 1;
            //var tickets = from a in db.Turnos
            //              where a.Fecha == fecha && (a.Despachado == null || a.Despachado == false) && a.Habilitado == true && (a.atendido == null || a.atendido == false)
            //              select new
            //              {

            //              };

            foreach (var item in tickets)
            {
                lista.Add(new fmtTurnos
                {
                    id = item.id,
                    TipoServicio = item.TipoServicio,
                    Turno = item.Turno,
                    Fecha = item.Fecha,
                    Hora_inicio = item.Hora_inicio,
                    Hora_fin = item.Hora_fin,
                    Despachado = item.Despachado,
                    Habilitado = item.Habilitado,
                    atendido = item.atendido,
                    usuario = item.usuario,
                    no = turno
                });
                turno++;
            }

            return Json(lista);
        }
        
        [HttpPost]
        public JsonResult getTicketsFinishes()
        {
            var today = DateTime.Now.Date;
            var fecha = today.ToString().Split(' ')[0];
            var tickets = db.Turnos.Where(t => t.Fecha == fecha && (t.Despachado == null || t.Despachado == false) && t.Habilitado == true && t.atendido == true).ToList();
            return Json(tickets);
        }
        
        [HttpPost]
        public JsonResult getAllTicketsAtende()
        {
            var today = DateTime.Now.Date;
            var fecha = today.ToString().Split(' ')[0];
            var tickets = db.Turnos.Where(t => /*t.Fecha == fecha &&*/ t.Despachado==true && t.Habilitado == true && t.atendido == true).ToList();
            return Json(tickets);
        }


        [HttpPost]
        public JsonResult atender(int ticket, int user)
        {
            var today = DateTime.Now.Date;
            var fecha = today.ToString().Split(' ')[0];
            var hora = DateTime.Now.ToString().Split(' ')[1];

            var turno = db.Turnos.Find(ticket);
            turno.usuario = user;
            turno.atendido = true;
            turno.Hora_inicio = hora;
            db.Entry(turno).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Json(null);
        }

        [HttpPost]
        public JsonResult deshabilitar(int id)
        {
            var turno = db.Turnos.Find(id);
            turno.Habilitado = false;
            db.Entry(turno).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Json(null);
        }

        [HttpPost]
        public JsonResult despachar(int id)
        {
            var today = DateTime.Now.Date;
            var fecha = today.ToString().Split(' ')[0];
            var hora = DateTime.Now.ToString().Split(' ')[1];

            var turno = db.Turnos.Find(id);
            turno.Despachado = true;
            turno.Hora_fin = hora;
            db.Entry(turno).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            
            return Json(null);
        }

        [HttpPost]
        public JsonResult getTicketsforUser()
        {
            var turnos = from a in db.Turnos.ToList()
                         join b in db.Usuarios.ToList() on a.usuario equals b.id
                         join c in db.Servicios.ToList() on a.TipoServicio equals c.id
                         where a.Despachado==true && a.Habilitado == true && a.atendido==true
                         select new
                         {
                             tipoServicio = c.CodigoServicio,
                             turno= a.Turno,
                             fecha=a.Fecha,
                             hora_inicio= a.Hora_inicio,
                             hora_fin= a.Hora_fin,
                             despachado=a.Despachado,
                             habilitado=a.Habilitado,
                             usuario=b.NombreCompleto,
                             atendido=a.atendido
                         };

            return Json(turnos.ToList());
        }



        [HttpPost]
        public JsonResult getRoles()
        {
            return Json(db.Roles.OrderByDescending(r=>r.id).ToList());
        }

        [HttpPost]
        public JsonResult createUser(Usuarios usuario)
        {
            db.Usuarios.Add(usuario);
            var res= db.SaveChanges();
            if (res==1)
            {
                return Json("USUARIO CREADO");
            }else
            {
                return Json("No se creo el usuario, intente nuevamente");
            }
            
        }

        [HttpPost]
        public JsonResult getUserCreated()
        {
            var usuarios= db.Usuarios.ToList();

            var user = from a in db.Usuarios
                       join b in db.Roles on a.Rol equals b.id
                       select new
                       {
                           nombre= a.NombreCompleto,
                           noDocumento =a.Cedula,
                           userName = a.UserName,
                           permisos=b.nombre
                       };
            return Json(user.ToList());
        }
        //getTicketsInProcess
        [HttpPost]
        public JsonResult getTicketsInProcess()
        {
            var today = DateTime.Now.Date;
            var fecha = today.ToString().Split(' ')[0];

            var tickets = from a in db.Turnos
                          join b in db.Usuarios on a.usuario equals b.id
                          where a.Fecha == fecha && (a.Despachado == null || a.Despachado == false) && a.Habilitado == true && a.atendido == true
                          select new
                          {
                              tiposervicio= a.TipoServicio+"-"+a.Turno,
                              hora_inicio= a.Hora_inicio,
                                usuario=b.NombreCompleto
                          };



            return Json(tickets);
        }

        public ViewResult panel_turnos()
        {
            return View();
        }

        [HttpPost]
        public JsonResult getTicketsWaiting()
        {
            var today = DateTime.Now.Date;
            var fecha = today.ToString().Split(' ')[0];

            //var tickets = db.Turnos.
            //Where
            //(t => t.Fecha == fecha && (t.Despachado == null || t.Despachado == false) 
            //&& t.Habilitado == true && (t.atendido == null || t.atendido == false)).ToList();
            var tickets = from a in db.Turnos
                          join b in db.Servicios on a.TipoServicio equals b.id
                          join c in db.Usuarios on a.usuario equals c.id
                          where a.Fecha == fecha && (a.Despachado == null || a.Despachado == false)
                          && a.Habilitado==true && a.atendido == true && a.Hora_fin == null && a.Hora_inicio != null
                          select new
                        {
                            ticket = b.CodigoServicio +"-"+ a.Turno,
                            usuario = c.NombreCompleto
                        };

            return Json(tickets);
        }


    }
}