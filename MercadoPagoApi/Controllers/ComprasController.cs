using System;
using ApiDoCesao.Data;
using ApiDoCesao.Data.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiDoCesao.Controllers
{
    [Route("compras")]
    [ApiController]
    public class ComprasController : ControllerBase
    {
        private ComprasBusiness _comprasBusiness;

        public ComprasController(MongoDb dbContext)
        {
            _comprasBusiness = new ComprasBusiness(dbContext);
        }

        [HttpGet("{pId}")]
        [Authorize]
        public IActionResult AdicionarProdutoAoCarrinho(int pId,[FromQuery (Name = "quantidade")] int pQuantidade)
        {
            try
            {
                var EmailAutenticado = User.Identity.Name;
                _comprasBusiness.AdicionarProdutoAoCarrinho(pId, pQuantidade, EmailAutenticado);

                return Ok($"Produto adicionado com sucesso");
            }
            catch(Exception err)
            {
                return StatusCode(404,$"{err.Message}");
            }
        }

        [HttpDelete("{pId}")]
        [Authorize]
        public IActionResult RemoverProdutoDoCarrinho(int pId, [FromQuery(Name = "quantidade")] int pQuantidade)
        {
            try
            {
                var EmailAutenticado = User.Identity.Name;
                _comprasBusiness.RemoverProdutoDoCarrinho(pId, pQuantidade, EmailAutenticado);

                return Ok("Produto removido com sucesso");
            }
            catch (Exception err)
            {
                return StatusCode(404, $"{err}");
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult ValidarCompra()
        {
            try
            {
                var EmailAutenticado = User.Identity.Name;
                var item = _comprasBusiness.ValidarCompra(EmailAutenticado);

                return Ok(item);
            }
            catch(Exception err)
            {
                return StatusCode(500, $"{err}");
            }
        }
    }

    
}
