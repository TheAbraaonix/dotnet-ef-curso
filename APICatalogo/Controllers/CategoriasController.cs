using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace APICatalogo.Controllers
{
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
        public ActionResult<IEnumerable<CategoriaDTO>> Get()
        {
            IEnumerable<Categoria> categorias = _uof.CategoriaRepository.GetAll();

            if (categorias == null) return NotFound("Não existem categorias.");

            IEnumerable<CategoriaDTO> categoriasDto = _mapper.Map<IEnumerable<CategoriaDTO>>(categorias);
            return Ok(categoriasDto);
        }

        [HttpGet("pagination")]
        public ActionResult<IEnumerable<CategoriaDTO>> Get([FromQuery] CategoriasParameters categoriasParams)
        {
            PagedList<Categoria> categorias = _uof.CategoriaRepository.GetCategorias(categoriasParams);
            return ObterCategorias(categorias);
        }

        [HttpGet("filter/nome/pagination")]
        public ActionResult<IEnumerable<CategoriaDTO>> GetCategoriasFiltradas([FromQuery] CategoriasFiltroNome categoriasFiltro)
        {
            var categoriasFiltradas = _uof.CategoriaRepository.GetCategoriaFiltroNome(categoriasFiltro);
            return ObterCategorias(categoriasFiltradas);
        }

        private ActionResult<IEnumerable<CategoriaDTO>> ObterCategorias(PagedList<Categoria> categorias)
        {
            var metadada = new
            {
                categorias.TotalCount,
                categorias.PageSize,
                categorias.CurrentPage,
                categorias.TotalPages,
                categorias.HasNext,
                categorias.HasPrevious
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadada));

            IEnumerable<CategoriaDTO> categoriaDto = _mapper.Map<IEnumerable<CategoriaDTO>>(categorias);
            return Ok(categoriaDto);
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterCategoria")]
        public ActionResult<CategoriaDTO> GetById(int id)
        {
            Categoria? categoria = _uof.CategoriaRepository.Get(c => c.CategoriaId == id);

            if (categoria == null) return NotFound($"A categoria de id {id} não existe.");

            CategoriaDTO categoriaDto = _mapper.Map<CategoriaDTO>(categoria);
            return Ok(categoriaDto);
        }

        [HttpPost]
        public ActionResult<CategoriaDTO> Post(CategoriaDTO categoriaDto)
        {
            if (categoriaDto is null) return BadRequest();

            Categoria categoria = _mapper.Map<Categoria>(categoriaDto);

            Categoria categoriaCriada = _uof.CategoriaRepository.Create(categoria);
            _uof.Commit();

            CategoriaDTO categoriaCriadaDto = _mapper.Map<CategoriaDTO>(categoriaCriada);
            return new CreatedAtRouteResult("ObterCategoria", new { id = categoriaCriadaDto.CategoriaId }, categoriaCriadaDto);
        }

        [HttpPut("{id:int:min(1)}")]
        public ActionResult<CategoriaDTO> Put(int id, CategoriaDTO categoriaDto)
        {
            if (id != categoriaDto.CategoriaId) return BadRequest();

            if (_uof.CategoriaRepository.Get(c => c.CategoriaId == id) == null) return NotFound($"A categoria de id {id} não existe.");

            Categoria categoria = _mapper.Map<Categoria>(categoriaDto);

            Categoria categoriaAtualizada = _uof.CategoriaRepository.Update(categoria);
            _uof.Commit();

            CategoriaDTO categoriaAtualizadaDto = _mapper.Map<CategoriaDTO>(categoriaAtualizada);
            return Ok(categoriaAtualizadaDto);
        }

        [HttpDelete("{id:int:min(1)}")]
        public ActionResult<CategoriaDTO> Delete(int id)
        {
            Categoria? categoria = _uof.CategoriaRepository.Get(c => c.CategoriaId == id);

            if (categoria == null) return NotFound($"A categoria de id {id} não existe.");

            Categoria categoriaExcluida = _uof.CategoriaRepository.Delete(categoria);
            _uof.Commit();

            CategoriaDTO categoriaExcluidaDto = _mapper.Map<CategoriaDTO>(categoriaExcluida);
            return Ok(categoriaExcluidaDto);
        }

    }
}
