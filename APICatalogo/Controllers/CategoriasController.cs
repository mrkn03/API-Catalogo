using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Pagination;
using APICatalogo.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CategoriasController> _logger;

        public CategoriasController(ILogger<CategoriasController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            this._unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetAsync()
        {
            var categorias = await _unitOfWork.CategoriaRepository.GetAllAsync();

            var categoriasDTO = CategoriaDTOMappingExtensions.ToCategoriaDTOs(categorias);

            return Ok(categoriasDTO);

        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public async Task<ActionResult<CategoriaDTO>> Get(int id)
        {
            var categoria = await _unitOfWork.CategoriaRepository.GetAsync(c => c.CategoriaId == id);

            if (categoria is null)
            {
                _logger.LogWarning($"Categoria com id= {id} não encontrada...");
                return NotFound($"Categoria com id= {id} não encontrada...");
            }

            var categoriaDTO = CategoriaDTOMappingExtensions.ToCategoriaDTO(categoria);

            return Ok(categoriaDTO);
        }

        [HttpPost]
        public async Task<ActionResult<CategoriaDTO>> Post(CategoriaDTO categoriaDTO)
        {
            if (categoriaDTO is null)
            {
                _logger.LogWarning($"Dados inválidos...");
                return BadRequest("Dados inválidos");
            }

            var categoria = CategoriaDTOMappingExtensions.ToCategoria(categoriaDTO);

            var novaCategoria = _unitOfWork.CategoriaRepository.Create(categoria);
            await _unitOfWork.CommitAsync();


            return CreatedAtRoute("ObterCategoria", new { id = novaCategoria.CategoriaId }, novaCategoria);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<CategoriaDTO>> Put(int id, CategoriaDTO categoriaDTO)
        {
            if (id != categoriaDTO.CategoriaId)
            {
                _logger.LogWarning($"Dados inválidos...");
                return BadRequest("Dados inválidos");
            }

            var categoria = CategoriaDTOMappingExtensions.ToCategoria(categoriaDTO);

            _unitOfWork.CategoriaRepository.Update(categoria);
            await _unitOfWork.CommitAsync();

            var categoriaAtualizadaDTO = CategoriaDTOMappingExtensions.ToCategoriaDTO(categoria);

            return Ok(categoriaAtualizadaDTO);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CategoriaDTO>> Delete(int id)
        {
            var categoria = await _unitOfWork.CategoriaRepository.GetAsync(c => c.CategoriaId == id);

            if (categoria is null)
            {
                _logger.LogWarning($"Categoria com id= {id} não encontrada...");
                return NotFound($"Categoria com id= {id} não encontrada...");
            }

            var categoriaDeletada = _unitOfWork.CategoriaRepository.Delete(categoria);
            await _unitOfWork.CommitAsync();

            var categoriaDeletadaDTO = CategoriaDTOMappingExtensions.ToCategoriaDTO(categoriaDeletada);

            return Ok(categoriaDeletadaDTO);
        }

        [HttpGet("pagination")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasPaginadas([FromQuery] CategoriasParameters categoriasParameters)
        {
            var categorias = await _unitOfWork.CategoriaRepository.GetCategoriasAsync(categoriasParameters);

            var metadados = new
            {
                categorias.TotalCount,
                categorias.PageSize,
                categorias.CurrentPage,
                categorias.TotalPages,
                categorias.HasNext,
                categorias.HasPrevious
            };

            Response.Headers.Append("X-Pagination", System.Text.Json.JsonSerializer.Serialize(metadados));

            var categoriasDTO = categorias.ToCategoriaDTOs();

            return Ok(categoriasDTO);
        }
    }
}