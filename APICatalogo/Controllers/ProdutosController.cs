using APICatalogo.Context;
using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ProdutosController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        [HttpGet("produtos/{id}")]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProdutosPorCategoria(int id)
        {
            var produtos = await unitOfWork.ProdutoRepository.GetProdutosPorCategoriaAsync(id);

            if (produtos is null)
            {
                return NotFound($"Nenhum produto encontrado para a categoria com id= {id}...");
            }

            var produtosDTO = mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

            return Ok(produtosDTO);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> Get()
        {
            var produtos = await unitOfWork.ProdutoRepository.GetAllAsync();

            var produtosDTO = mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

            return Ok(produtosDTO);
        }

        [HttpGet("{id}", Name = "ObterProduto")]
        public async Task<ActionResult<ProdutoDTO>> Get(int id)
        {
            var produto = await unitOfWork.ProdutoRepository.GetAsync(p => p.ProdutoId == id);

            var produtoDTO = mapper.Map<ProdutoDTO>(produto);

            return produto is null ? NotFound($"Produto com id= {id} não encontrado...") : Ok(produtoDTO);
        }

        [HttpPost]
        public async Task<ActionResult<ProdutoDTO>> Post(ProdutoDTO produtoDTO)
        {

            if (produtoDTO is null)
            {
                return BadRequest("Dados inválidos...");
            }

            var produto = mapper.Map<Produto>(produtoDTO);

            var novoProduto = unitOfWork.ProdutoRepository.Create(produto);
            await unitOfWork.CommitAsync();

            var novoProdutoDTO = mapper.Map<ProdutoDTO>(novoProduto);

            return CreatedAtRoute("ObterProduto", new { id = novoProdutoDTO.ProdutoId }, novoProdutoDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ProdutoDTO>> Put(int id, ProdutoDTO produtoDTO)
        {
            if (id != produtoDTO.ProdutoId)
            {
                return BadRequest("Dados inválidos...");
            }

            var produto = mapper.Map<Produto>(produtoDTO);

            unitOfWork.ProdutoRepository.Update(produto);
            await unitOfWork.CommitAsync();

            var produtoAtualizadoDTO = mapper.Map<ProdutoDTO>(produto);

            return Ok(produtoAtualizadoDTO);

        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ProdutoDTO>> Delete(int id)
        {
            var produto = await unitOfWork.ProdutoRepository.GetAsync(p => p.ProdutoId == id);

            if (produto is null)
            {
                return NotFound($"Produto com id= {id} não encontrado...");
            }


            var produtoDeletado = unitOfWork.ProdutoRepository.Delete(produto);
            await unitOfWork.CommitAsync();

            var produtoDeletadoDTO = mapper.Map<ProdutoDTO>(produtoDeletado);

            return Ok(produtoDeletadoDTO);
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<ProdutoDTOUpdateResponse>> Patch(int id, JsonPatchDocument<ProdutoDTOUpdateRequest> patchProdutoDTO)
        {
            if (id <= 0 || patchProdutoDTO is null)
            {
                return BadRequest("Dados inválidos...");
            }

            var produto = await unitOfWork.ProdutoRepository.GetAsync(p => p.ProdutoId == id);

            if (produto is null)
            {
                return NotFound($"Produto com id= {id} não encontrado...");
            }

            var produtoDTOUpdateRequest = mapper.Map<ProdutoDTOUpdateRequest>(produto);

            patchProdutoDTO.ApplyTo(produtoDTOUpdateRequest, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(produtoDTOUpdateRequest, produto);
            unitOfWork.ProdutoRepository.Update(produto);
            await unitOfWork.CommitAsync();

            var produtoDTOUpdateResponse = mapper.Map<ProdutoDTOUpdateResponse>(produto);

            return Ok(produtoDTOUpdateResponse);
        }

        [HttpGet("pagination")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosPaginados([FromQuery] ProdutosParameters produtosParameters)
        {
            var produtos = await unitOfWork.ProdutoRepository.GetProdutosAsync(produtosParameters);

            var metadados = new
            {
                produtos.CurrentPage,
                produtos.PageSize,
                produtos.TotalCount,
                produtos.TotalPages,
                produtos.HasNext,
                produtos.HasPrevious
            };

            Response.Headers.Append("X-Pagination", System.Text.Json.JsonSerializer.Serialize(metadados));

            var produtosDTO = mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

            return Ok(produtosDTO);
        }
    }
}
