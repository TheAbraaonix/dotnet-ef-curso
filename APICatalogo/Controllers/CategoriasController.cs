using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Newtonsoft.Json;
using X.PagedList;
using Microsoft.AspNetCore.Http;

namespace APICatalogo.Controllers
{
    [EnableCors("OrigensComAcessoPermitido")]
    [EnableRateLimiting("fixedwindow")]
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;

        public CategoriasController(IUnitOfWork uof, IMapper mapper)
        {
            _uof = uof;
            _mapper = mapper;
        }

        [HttpGet]
        //[Authorize]
        [DisableRateLimiting]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get()
        {
            IEnumerable<Categoria> categorias = await _uof.CategoriaRepository.GetAllAsync();

            if (categorias == null) return NotFound("Não existem categorias.");

            IEnumerable<CategoriaDTO> categoriasDto = _mapper.Map<IEnumerable<CategoriaDTO>>(categorias);
            return Ok(categoriasDto);
        }

        [HttpGet("pagination")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get([FromQuery] CategoriasParameters categoriasParams)
        {
            IPagedList<Categoria> categorias = await _uof.CategoriaRepository.GetCategoriasAsync(categoriasParams);
            return ObterCategorias(categorias);
        }

        [HttpGet("filter/nome/pagination")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasFiltradas([FromQuery] CategoriasFiltroNome categoriasFiltro)
        {
            var categoriasFiltradas = await _uof.CategoriaRepository.GetCategoriaFiltroNomeAsync(categoriasFiltro);
            return ObterCategorias(categoriasFiltradas);
        }

        private ActionResult<IEnumerable<CategoriaDTO>> ObterCategorias(IPagedList<Categoria> categorias)
        {
            var metadada = new
            {
                categorias.Count,
                categorias.PageSize,
                categorias.PageCount,
                categorias.TotalItemCount,
                categorias.HasNextPage,
                categorias.HasPreviousPage
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadada));

            IEnumerable<CategoriaDTO> categoriaDto = _mapper.Map<IEnumerable<CategoriaDTO>>(categorias);
            return Ok(categoriaDto);
        }

        /// <summary>
        /// Obtem uma Categoria pelo seu Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Objetos Categoria</returns>
        [DisableCors]
        [HttpGet("{id:int:min(1)}", Name = "ObterCategoria")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoriaDTO>> GetById(int id)
        {
            Categoria? categoria = await _uof.CategoriaRepository.GetAsync(c => c.CategoriaId == id);

            if (categoria == null) return NotFound($"A categoria de id {id} não existe.");

            CategoriaDTO categoriaDto = _mapper.Map<CategoriaDTO>(categoria);
            return Ok(categoriaDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<CategoriaDTO>> Post(CategoriaDTO categoriaDto)
        {
            if (categoriaDto is null) return BadRequest();

            Categoria categoria = _mapper.Map<Categoria>(categoriaDto);

            Categoria categoriaCriada = _uof.CategoriaRepository.Create(categoria);
            await _uof.CommitAsync();

            CategoriaDTO categoriaCriadaDto = _mapper.Map<CategoriaDTO>(categoriaCriada);
            return new CreatedAtRouteResult("ObterCategoria", new { id = categoriaCriadaDto.CategoriaId }, categoriaCriadaDto);
        }

        [HttpPut("{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<CategoriaDTO>> Put(int id, CategoriaDTO categoriaDto)
        {
            if (id != categoriaDto.CategoriaId) return BadRequest();

            if (await _uof.CategoriaRepository.GetAsync(c => c.CategoriaId == id) == null) return NotFound($"A categoria de id {id} não existe.");

            Categoria categoria = _mapper.Map<Categoria>(categoriaDto);

            Categoria categoriaAtualizada = _uof.CategoriaRepository.Update(categoria);
            await _uof.CommitAsync();

            CategoriaDTO categoriaAtualizadaDto = _mapper.Map<CategoriaDTO>(categoriaAtualizada);
            return Ok(categoriaAtualizadaDto);
        }

        [HttpDelete("{id:int:min(1)}")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<CategoriaDTO>> Delete(int id)
        {
            Categoria? categoria = await _uof.CategoriaRepository.GetAsync(c => c.CategoriaId == id);

            if (categoria == null) return NotFound($"A categoria de id {id} não existe.");

            Categoria categoriaExcluida = _uof.CategoriaRepository.Delete(categoria);
            await _uof.CommitAsync();

            CategoriaDTO categoriaExcluidaDto = _mapper.Map<CategoriaDTO>(categoriaExcluida);
            return Ok(categoriaExcluidaDto);
        }

    }
}
