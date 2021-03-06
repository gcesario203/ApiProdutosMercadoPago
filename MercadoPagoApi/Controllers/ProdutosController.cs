﻿using MercadoPagoApi.Data;
using MercadoPagoApi.Data.Business;
using MercadoPagoApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace MercadoPagoApi.Controllers
{
    [Route("produtos")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private ProdutosBusiness _produtosCollection { get; set; }


        public ProdutosController(MongoDb dbContext)
        {
            _produtosCollection = new ProdutosBusiness(dbContext);
        }


        [HttpPost]
        [Authorize]
        public IActionResult SalvarProduto([FromBody] ProdutoDto pProduto)
        {
            try
            {
                _produtosCollection.Salvar(pProduto);

                return StatusCode(201, $"Produto criado com sucesso: {pProduto.Nome}");
            }
            catch (Exception err)
            {
                return StatusCode(400, $"Falha ao cadastrar produto: {err}");
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult TodosProdutos()
        {
            try
            {
                return Ok(_produtosCollection.Todos());
            }
            catch (Exception err)
            {
                return StatusCode(500, $"Falha no servidor: {err}");
            }
        }

        [HttpGet("{pId}")]
        [Authorize]
        public IActionResult ProdutoPorId(int pId)
        {
            var produtoSelecionado = _produtosCollection.ProdutoPorId(pId);

            if(produtoSelecionado != null)
            {
                return Ok(produtoSelecionado);
            }
            else
            {
                return NotFound("Produto não encontrado");
            }
        }


        [HttpPut("{pId}")]
        [Authorize]
        public IActionResult AlterarProduto(int pId, [FromBody] ProdutoDto pProduto)
        {
            var produtoAlterado = _produtosCollection.AlterarProduto(pId, pProduto);

            if(produtoAlterado != null)
            {
                return Ok("Produto alterado com sucesso");
            }
            else
            {
                return NotFound("Produto não encontrado");
            }
        }

        [HttpDelete("{pId}")]
        [Authorize]
        public IActionResult DeletarProduto(int pId)
        {
            try
            {
                _produtosCollection.DeletarProduto(pId);

                return Ok("Produto deletado com sucesso");
            }
            catch
            {
                return NotFound("Produto não encontrado");
            }
        }
    }
}
