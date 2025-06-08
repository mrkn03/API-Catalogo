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
        public ActionResult<IEnumerable<Produto>> GetProdutosPorCategoria(int id)
        {
            var produtos = unitOfWork.ProdutoRepository.GetProdutosPorCategoria(id);

            if (produtos is null)
            {
                return NotFound($"Nenhum produto encontrado para a categoria com id= {id}...");
            }

            var produtosDTO = mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

            return Ok(produtosDTO);
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProdutoDTO>> Get()
        {
            var produtos = unitOfWork.ProdutoRepository.GetAll();

            var produtosDTO = mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

            return Ok(produtosDTO);
        }

        [HttpGet("{id}", Name = "ObterProduto")]
        public ActionResult<ProdutoDTO> Get(int id)
        {
            var produto = unitOfWork.ProdutoRepository.Get(p => p.ProdutoId == id);

            var produtoDTO = mapper.Map<ProdutoDTO>(produto);

            return produto is null ? NotFound($"Produto com id= {id} não encontrado...") : Ok(produtoDTO);
        }

        [HttpPost]
        public ActionResult<ProdutoDTO> Post(ProdutoDTO produtoDTO)
        {

            if (produtoDTO is null)
            {
                return BadRequest("Dados inválidos...");
            }

            var produto = mapper.Map<Produto>(produtoDTO);

            var novoProduto = unitOfWork.ProdutoRepository.Create(produto);
            unitOfWork.Commit();

            var novoProdutoDTO = mapper.Map<ProdutoDTO>(novoProduto);

            return CreatedAtRoute("ObterProduto", new { id = novoProdutoDTO.ProdutoId }, novoProdutoDTO);
        }

        [HttpPut("{id:int}")]
        public ActionResult<ProdutoDTO> Put(int id, ProdutoDTO produtoDTO)
        {
            if (id != produtoDTO.ProdutoId)
            {
                return BadRequest("Dados inválidos...");
            }

            var produto = mapper.Map<Produto>(produtoDTO);

            unitOfWork.ProdutoRepository.Update(produto);
            unitOfWork.Commit();

            var produtoAtualizadoDTO = mapper.Map<ProdutoDTO>(produto);

            return Ok(produtoAtualizadoDTO);

        }

        [HttpDelete("{id:int}")]
        public ActionResult<ProdutoDTO> Delete(int id)
        {
            var produto = unitOfWork.ProdutoRepository.Get(p => p.ProdutoId == id);

            if (produto is null)
            {
                return NotFound($"Produto com id= {id} não encontrado...");
            }


            var produtoDeletado = unitOfWork.ProdutoRepository.Delete(produto);
            unitOfWork.Commit();

            var produtoDeletadoDTO = mapper.Map<ProdutoDTO>(produtoDeletado);

            return Ok(produtoDeletadoDTO);
        }

        [HttpPatch("{id:int}")]
        public ActionResult<ProdutoDTOUpdateResponse> Patch(int id, JsonPatchDocument<ProdutoDTOUpdateRequest> patchProdutoDTO)
        {
            if (id <= 0 || patchProdutoDTO is null)
            {
                return BadRequest("Dados inválidos...");
            }

            var produto = unitOfWork.ProdutoRepository.Get(p => p.ProdutoId == id);
            
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
            unitOfWork.Commit();

            var produtoDTOUpdateResponse = mapper.Map<ProdutoDTOUpdateResponse>(produto);

            return Ok(produtoDTOUpdateResponse);
        }

        [HttpGet("pagination")]
        public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosPaginados([FromQuery] ProdutosParameters produtosParameters)
        {
            var produtos = unitOfWork.ProdutoRepository.GetProdutos(produtosParameters);
            
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
