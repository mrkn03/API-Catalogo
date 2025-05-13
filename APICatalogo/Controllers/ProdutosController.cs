using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoRepository produtoRepository;

        public ProdutosController(IProdutoRepository produtoRepository)
        {
            this.produtoRepository = produtoRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var produtos = produtoRepository.GetProdutos();

            return Ok(produtos);
        }

        [HttpGet("{id}", Name = "ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = produtoRepository.GetProduto(id);

            return produto is null ? NotFound($"Produto com id= {id} não encontrado...") : Ok(produto);
        }

        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            produtoRepository.Add(produto);

            return CreatedAtRoute("ObterProduto", new { id = produto.ProdutoId }, produto);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            if(id != produto.ProdutoId)
            {
                return BadRequest("Dados inválidos...");
            }

            produtoRepository.Update(produto);

            return Ok(produto);

        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
           var produto = produtoRepository.GetProduto(id);

            if (produto is null)
            {
                return NotFound($"Produto com id= {id} não encontrado...");
            }
           var produtoDeletado =  produtoRepository.Delete(id);
            
            return Ok(produtoDeletado);
        }
    }
}
