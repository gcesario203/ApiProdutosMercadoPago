using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiDoCesao.Data;
using ApiDoCesao.Data.Business;
using ApiDoCesao.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MercadoPago;
using Microsoft.AspNetCore.Authorization;

namespace ApiDoCesao.Controllers
{
    [Route("usuarios")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        
        private UsuariosBusiness _usuariosCollection;

        public UsuariosController(MongoDb dbContext)
        {
            _usuariosCollection = new UsuariosBusiness(dbContext);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult CriarUsuario([FromBody] UsuarioDto pUsuarioDto)
        {
            try 
            {
                _usuariosCollection.Salvar(pUsuarioDto);

                return StatusCode(201, "Usuario criado com sucesso");
            } 
            catch(Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }
        }

        [HttpGet]
        [Authorize]
        [Route("/eu")]
        public IActionResult UsuarioPorId()
        {
            try
            {
                var Email = User.Identity.Name;
                var usuario = _usuariosCollection.UsuarioPorEmail(Email);

                return Ok(usuario);
            }
            catch(Exception err)
            {
                return StatusCode(404, $"{err.Message}");
            }
        }

        [HttpGet]
        [Authorize(Roles ="admin")]
        public IActionResult ListarUsuarios()
        {
            try
            {
                return Ok(_usuariosCollection.MostrarTodosUsuarios());
            }
            catch(Exception err)
            {
                return StatusCode(500, $"{err.Message}");
            }
        }

        [HttpDelete]
        [Authorize]
        public IActionResult DeletarUsuario()
        {
            try
            {
                var user = _usuariosCollection.UsuarioPorEmail(User.Identity.Name);
                 _usuariosCollection.DeletarUsuario(user.UsuarioId);

                return Ok("usuario deletado com sucesso");
            }
            catch (Exception err)
            {
                return StatusCode(404, $"{err.Message}");
            }
        }

        [HttpPut]
        [Authorize]
        public IActionResult AlterarUsuario(UsuarioDto pUsuarioDto)
        {
            try
            {
                var user = _usuariosCollection.UsuarioPorEmail(User.Identity.Name);
                _usuariosCollection.AlterarUsuario(user.UsuarioId, pUsuarioDto);

                return Ok("Usuario editado com sucesso");
            }
            catch (Exception err)
            {
                return StatusCode(404, $"{err.Message}");
            }
        }
    }
}
