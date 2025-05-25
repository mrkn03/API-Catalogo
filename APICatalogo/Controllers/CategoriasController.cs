using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<CategoriasController> _logger;

        public CategoriasController(ILogger<CategoriasController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
                var categorias = unitOfWork.CategoriaRepository.GetAll();

                return Ok(categorias);

        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            var categoria = unitOfWork.CategoriaRepository.Get(c => c.CategoriaId == id);

            if (categoria is null)
            {
                _logger.LogWarning($"Categoria com id= {id} não encontrada...");
                return NotFound($"Categoria com id= {id} não encontrada...");
            }


            return Ok(categoria);
        }

        [HttpPost]
        public ActionResult Post(Categoria categoria)
        {
            if (categoria is null)
            {
                _logger.LogWarning($"Dados inválidos...");
                return BadRequest("Dados inválidos");
            }

            var novaCategoria = unitOfWork.CategoriaRepository.Create(categoria);
            unitOfWork.Commit();


            return CreatedAtRoute("ObterCategoria", new { id = novaCategoria.CategoriaId }, novaCategoria);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Categoria categoria)
        {
            if (id != categoria.CategoriaId)
            {
                _logger.LogWarning($"Dados inválidos...");
                return BadRequest("Dados inválidos");
            }

            unitOfWork.CategoriaRepository.Update(categoria);
            unitOfWork.Commit();

            return Ok(categoria);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var categoria = unitOfWork.CategoriaRepository.Get(c => c.CategoriaId == id);

            if (categoria is null)
            {
                _logger.LogWarning($"Categoria com id= {id} não encontrada...");
                return NotFound($"Categoria com id= {id} não encontrada...");
            }

            var categoriaDeletada = unitOfWork.CategoriaRepository.Delete(categoria);
            unitOfWork.Commit();

            return Ok(categoriaDeletada);
        }
    }
}
