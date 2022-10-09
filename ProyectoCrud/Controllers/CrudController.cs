using ProyectoCrud.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoCrud.Controllers
{
    public class CrudController : Controller
    {
        static string cadena = "data source = DESKTOP-530G8NK\\WINCCPLUSMIG2008; Initial Catalog = DbProyectoCrud; Integrated Security = true";

        // GET: Crud
        /*public ActionResult HomeCrud(string mensaje)
        {
            ViewBag.Message = mensaje;
            return View();
        }
           */     
        [HttpPost]
        public ActionResult Guardar(ModeloContactos modeloContactos)
        {
            try
            {
                using (SqlConnection Conexion = new SqlConnection(cadena))
                {
                    SqlCommand comando = new SqlCommand("CrearContacto" , Conexion); // Create a object of SqlCommand class
                
                    comando.Parameters.AddWithValue("@Nombre", modeloContactos.Nombre);
                    comando.Parameters.AddWithValue("@Telefono", modeloContactos.Telefono);
                    comando.Parameters.AddWithValue("@Correo", modeloContactos.Correo);

                    comando.CommandType = CommandType.StoredProcedure;
                    Conexion.Open();
                    comando.ExecuteNonQuery();
                    ViewBag.Message = "Registro Exitoso";
                    Conexion.Close();
                } 
            }
            catch (Exception)
            {
                ViewBag.Message = "Registro Fallido";                
            }
            
            return RedirectToAction("HomeCrud", "Crud", ViewBag.Message);
        }

        // GET: Crud/Leer
        [HttpGet]
        public ActionResult HomeCrud()
        {
            ModeloContactos modelo = new ModeloContactos();
            var lista = new List<ModeloContactos>();

           // try
           // {
                using (SqlConnection Conexion = new SqlConnection(cadena))
                {
                    SqlCommand comando = new SqlCommand("MostrarContacto", Conexion);
                    comando.CommandType = CommandType.StoredProcedure;
                    Conexion.Open();

                    using (var Datos = comando.ExecuteReader())
                    {
                        while (Datos.Read())
                        {
                            lista.Add(new ModeloContactos() {
                                IdContacto = Convert.ToInt16(Datos["IdContactos"]) ,
                                Nombre     = Datos["Nombre"].ToString(),
                                Telefono   = Datos["Telefono"].ToString(),
                                Correo     = Datos["Correo"].ToString()
                            });
                        }
                    }   
                    ViewBag.Message = "Lectura Exitosa";
                    Conexion.Close();
                }
            // }
            // catch (Exception e)
            // {
            //     ViewBag.Message = "Lectura Fallido";               
            // }
            ViewBag.Lista = lista;
            return View(lista);            
        }        

        // GET: Crud/Edit/5
        public ActionResult Editar(int id)
        {
            ModeloContactos modeloContactos = new ModeloContactos();
            try
            {
                SqlConnection Conexion = new SqlConnection(cadena);
                SqlCommand comando = new SqlCommand("EditarContacto", Conexion);
                comando.Parameters.AddWithValue("IdContacto", id);
                comando.CommandType = CommandType.StoredProcedure;
                Conexion.Open();

                using (var Datos = comando.ExecuteReader())
                {
                    while (Datos.Read())
                    {                            
                      modeloContactos.IdContacto = Convert.ToInt32(Datos["IdContacto"]);
                      modeloContactos.Nombre = Datos["Nombre"].ToString();
                      modeloContactos.Telefono = Datos["Telefono"].ToString();
                      modeloContactos.Correo = Datos["Correo"].ToString();                                                                                  
                    }
                 }
                 ViewBag.Message = "Modificacion Exitosa";
                 Conexion.Close();                
            }
            catch (Exception)
            {
                ViewBag.Message = "Modificacion Fallido";
            }

            return View(modeloContactos);
        }        

        [HttpPost]
        public ActionResult Eliminar(int id)
        {
            try
            {
                SqlConnection Conexion = new SqlConnection(cadena);
                SqlCommand comando = new SqlCommand("EliminarContacto", Conexion);
                comando.Parameters.AddWithValue("IdContacto", id);
                comando.CommandType = CommandType.StoredProcedure;
                Conexion.Open();                
                ViewBag.Message = "Contacto Eliminado";
                Conexion.Close();
            }
            catch (Exception)
            {
                ViewBag.Message = "Contacto No Eliminado";
            }
            return RedirectToAction("HomeCrud", "Crud");
        }        
    }
}
