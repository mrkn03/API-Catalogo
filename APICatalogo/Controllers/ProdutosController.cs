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
            var produtos = produtoRepository.GetAll();

            return Ok(produtos);
        }

        [HttpGet("{id}", Name = "ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = produtoRepository.Get(p => p.ProdutoId == id);

            return produto is null ? NotFound($"Produto com id= {id} não encontrado...") : Ok(produto);
        }

        [HttpGet("produtos/{id:int}")]
        public ActionResult<IEnumerable<Produto>> GetProdutosPorCategoria(int id)
        {
            var produtos = produtoRepository.GetProdutosPorCategoria(id);

            if (produtos is null)
            {
                return NotFound($"Nenhum produto encontrado para a categoria com id= {id}...");
            }

            return Ok(produtos);
        }

        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            produtoRepository.Create(produto);

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
           var produto = produtoRepository.Get(p => p.ProdutoId == id);

            if (produto is null)
            {
                return NotFound($"Produto com id= {id} não encontrado...");
            }
           var produtoDeletado = produtoRepository.Delete(produto);
            
            return Ok(produtoDeletado);
        }
    }
}
