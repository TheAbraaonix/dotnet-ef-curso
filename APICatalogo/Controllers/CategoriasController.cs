using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

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
