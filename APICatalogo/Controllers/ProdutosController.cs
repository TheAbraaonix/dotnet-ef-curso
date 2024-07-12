using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;

        public ProdutosController(IUnitOfWork uof, IMapper mapper)
        {
            _uof = uof;
            _mapper = mapper;
        }

        [HttpGet("produtos/{id:int:min(1)}")]
        public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosCategoria(int id)
        {
            IEnumerable<Produto> produtos = _uof.ProdutoRepository.GetProdutosPorCategoria(id);

            if (produtos == null) return NotFound($"A categoria de id {id} não possui produtos.");

            IEnumerable<ProdutoDTO> produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);
            return Ok(produtosDto);
        }
        
        [HttpGet]
        public ActionResult<IEnumerable<ProdutoDTO>> Get()
        {
            IEnumerable<Produto> produtos = _uof.ProdutoRepository.GetAll();

            if (produtos == null) return NotFound("Não existem produtos.");

            IEnumerable<ProdutoDTO> produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);
            return Ok(produtosDto);
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public ActionResult<ProdutoDTO> GetById(int id)
        {
            Produto? produto = _uof.ProdutoRepository.Get(p => p.ProdutoId == id);

            if (produto == null) return NotFound($"O produto de id {id} não existe.");

            ProdutoDTO produtoDto = _mapper.Map<ProdutoDTO>(produto);
            return Ok(produtoDto);
        }

        [HttpPost]
        public ActionResult<ProdutoDTO> Post(ProdutoDTO produtoDto)
        {
            if (produtoDto == null) return BadRequest();

            Produto produto = _mapper.Map<Produto>(produtoDto);

            Produto novoProduto = _uof.ProdutoRepository.Create(produto);
            _uof.Commit();
            
            ProdutoDTO novoProdutoDto = _mapper.Map<ProdutoDTO>(novoProduto);
            return new CreatedAtRouteResult("ObterProduto", new { id = novoProdutoDto.ProdutoId }, novoProdutoDto);
        }

        [HttpPatch("{id}/UpdatePartial")]
        public ActionResult<ProdutoDTOUpdateResponse> Patch(int id, JsonPatchDocument<ProdutoDTOUpdateRequest> patchProdutoDto)
        {
            if (patchProdutoDto == null || id <= 0) return BadRequest();

            Produto? produto = _uof.ProdutoRepository.Get(p => p.ProdutoId == id);

            if (produto == null) return NotFound($"O produto de id {id} não existe.");

            ProdutoDTOUpdateRequest produtoUpdateRequest = _mapper.Map<ProdutoDTOUpdateRequest>(produto);

            patchProdutoDto.ApplyTo(produtoUpdateRequest, ModelState);

            if (!ModelState.IsValid || TryValidateModel(produtoUpdateRequest)) return BadRequest(ModelState);

            Produto produtoAtualizado = _mapper.Map<Produto>(produto);

            _uof.ProdutoRepository.Update(produtoAtualizado);
            _uof.Commit();

            return Ok(_mapper.Map<ProdutoDTOUpdateResponse>(produtoAtualizado));
        }

        [HttpPut("{id:int:min(1)}")]
        public ActionResult<ProdutoDTO> Put(int id, ProdutoDTO produtoDto)
        {
            if (id != produtoDto.ProdutoId) return BadRequest();

            if (_uof.ProdutoRepository.Get(p => p.ProdutoId == id) == null) return NotFound($"O produto de id {id} não existe.");

            Produto produto = _mapper.Map<Produto>(produtoDto);
            
            Produto produtoAtualizado = _uof.ProdutoRepository.Update(produto);
            _uof.Commit();
            
            ProdutoDTO produtoAtualizadoDto = _mapper.Map<ProdutoDTO>(produtoAtualizado);
            return Ok(produtoAtualizadoDto);
        }

        [HttpDelete("{id:int:min(1)}")]
        public ActionResult<ProdutoDTO> Delete(int id)
        {
            Produto? produto = _uof.ProdutoRepository.Get(p => p.ProdutoId == id);

            if (produto == null) return NotFound($"O produto de id {id} não existe.");

            Produto produtoExcluido = _uof.ProdutoRepository.Delete(produto);
            _uof.Commit();

            ProdutoDTO produtoExcluidoDto = _mapper.Map<ProdutoDTO>(produtoExcluido);
            return Ok(produtoExcluidoDto);
        }
    }
}
