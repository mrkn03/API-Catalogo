using APICatalogo.Context;
using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
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
        public ActionResult<IEnumerable<CategoriaDTO>> Get()
        {
            var categorias = unitOfWork.CategoriaRepository.GetAll();

            var categoriasDTO = CategoriaDTOMappingExtensions.ToCategoriaDTOs(categorias);

            return Ok(categoriasDTO);

        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<CategoriaDTO> Get(int id)
        {
            var categoria = unitOfWork.CategoriaRepository.Get(c => c.CategoriaId == id);

            if (categoria is null)
            {
                _logger.LogWarning($"Categoria com id= {id} não encontrada...");
                return NotFound($"Categoria com id= {id} não encontrada...");
            }

            var categoriaDTO = CategoriaDTOMappingExtensions.ToCategoriaDTO(categoria);

            return Ok(categoriaDTO);
        }

        [HttpPost]
        public ActionResult<CategoriaDTO> Post(CategoriaDTO categoriaDTO)
        {
            if (categoriaDTO is null)
            {
                _logger.LogWarning($"Dados inválidos...");
                return BadRequest("Dados inválidos");
            }

           var categoria = CategoriaDTOMappingExtensions.ToCategoria(categoriaDTO);

            var novaCategoria = unitOfWork.CategoriaRepository.Create(categoria);
            unitOfWork.Commit();


            return CreatedAtRoute("ObterCategoria", new { id = novaCategoria.CategoriaId }, novaCategoria);
        }

        [HttpPut("{id:int}")]
        public ActionResult<CategoriaDTO> Put(int id, CategoriaDTO categoriaDTO)
        {
            if (id != categoriaDTO.CategoriaId)
            {
                _logger.LogWarning($"Dados inválidos...");
                return BadRequest("Dados inválidos");
            }

            var categoria = CategoriaDTOMappingExtensions.ToCategoria(categoriaDTO);

            unitOfWork.CategoriaRepository.Update(categoria);
            unitOfWork.Commit();

            var categoriaAtualizadaDTO = CategoriaDTOMappingExtensions.ToCategoriaDTO(categoria);

            return Ok(categoriaAtualizadaDTO);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<CategoriaDTO> Delete(int id)
        {
            var categoria = unitOfWork.CategoriaRepository.Get(c => c.CategoriaId == id);

            if (categoria is null)
            {
                _logger.LogWarning($"Categoria com id= {id} não encontrada...");
                return NotFound($"Categoria com id= {id} não encontrada...");
            }

            var categoriaDeletada = unitOfWork.CategoriaRepository.Delete(categoria);
            unitOfWork.Commit();

            var categoriaDeletadaDTO = CategoriaDTOMappingExtensions.ToCategoriaDTO(categoriaDeletada);

            return Ok(categoriaDeletadaDTO);
        }
    }
}
