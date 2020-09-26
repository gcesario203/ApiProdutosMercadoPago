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
        public IActionResult SalvarUsuario([FromBody] UsuarioDto pUsuarioDto)
        {
            try 
            {
                _usuariosCollection.Salvar(pUsuarioDto);

                return StatusCode(201, "Usuario criado com sucesso");
            } 
            catch(Exception ex)
            {
                return StatusCode(500, $"{ex}");
            }
        }

        [HttpGet("{pId}")]
        public IActionResult UsuarioPorId(int pId)
        {
            try
            {
                var usuario = _usuariosCollection.UsuarioPorId(pId);

                return Ok(usuario);
            }
            catch(Exception err)
            {
                return StatusCode(404, $"{err}");
            }
        }

        [HttpDelete("{pId}")]
        public IActionResult DeleteUsuario(int pId)
        {
            try
            {
                 _usuariosCollection.DeletarUsuario(pId);

                return Ok("usuario deletado com sucesso");
            }
            catch (Exception err)
            {
                return StatusCode(404, $"{err}");
            }
        }

        [HttpPut("{pId}")]
        public IActionResult UsuarioPorId(int pId, UsuarioDto pUsuarioDto)
        {
            try
            {
                _usuariosCollection.AlterarUsuario(pId, pUsuarioDto);

                return Ok("Usuario editado com sucesso");
            }
            catch (Exception err)
            {
                return StatusCode(404, $"{err}");
            }
        }
    }
}
