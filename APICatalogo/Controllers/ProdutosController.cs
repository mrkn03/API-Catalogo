using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        
        private readonly IUnitOfWork unitOfWork;

        public ProdutosController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var produtos = unitOfWork.ProdutoRepository.GetAll();

            return Ok(produtos);
        }

        [HttpGet("{id}", Name = "ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = unitOfWork.ProdutoRepository.Get(p => p.ProdutoId == id);

            return produto is null ? NotFound($"Produto com id= {id} não encontrado...") : Ok(produto);
        }

        [HttpGet("produtos/{id:int}")]
        public ActionResult<IEnumerable<Produto>> GetProdutosPorCategoria(int id)
        {
            var produtos = unitOfWork.ProdutoRepository.GetProdutosPorCategoria(id);

            if (produtos is null)
            {
                return NotFound($"Nenhum produto encontrado para a categoria com id= {id}...");
            }

            return Ok(produtos);
        }

        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            unitOfWork.ProdutoRepository.Create(produto);
            unitOfWork.Commit();

            return CreatedAtRoute("ObterProduto", new { id = produto.ProdutoId }, produto);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            if(id != produto.ProdutoId)
            {
                return BadRequest("Dados inválidos...");
            }

            unitOfWork.ProdutoRepository.Update(produto);
            unitOfWork.Commit();

            return Ok(produto);

        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
           var produto = unitOfWork.ProdutoRepository.Get(p => p.ProdutoId == id);

            if (produto is null)
            {
                return NotFound($"Produto com id= {id} não encontrado...");
            }
           var produtoDeletado = unitOfWork.ProdutoRepository.Delete(produto);
            unitOfWork.Commit();

            return Ok(produtoDeletado);
        }
    }
}
