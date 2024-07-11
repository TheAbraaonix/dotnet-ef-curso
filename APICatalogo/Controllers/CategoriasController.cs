using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaRepository _repository;

        public CategoriasController(ICategoriaRepository repository)
        {
            _repository = repository;
        }

        //[HttpGet("produtos")]
        //[ServiceFilter(typeof(ApiLoggingFilter))]
        //public async Task<ActionResult<IEnumerable<Categoria>>> GetCategoriasProdutosAsync()
        //{
        //    return Ok(await _context.Categorias.Include(p => p.Produtos).Where(c => c.CategoriaId <= 5).AsNoTracking().ToListAsync());
        //}
        
        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            IEnumerable<Categoria> categorias = _repository.GetCategorias();
            
            return Ok(categorias);
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterCategoria")]
        public ActionResult<Categoria> GetById(int id)
        {
            Categoria categoria = _repository.GetCategoria(id);

            if (categoria == null) return NotFound($"A categoria de id {id} não existe.");
  
            return Ok(categoria);
        }

        [HttpPost]
        public ActionResult Post(Categoria categoria)
        {
            if (categoria is null) return BadRequest();

            Categoria categoriaCriada = _repository.Create(categoria);

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoriaCriada.CategoriaId }, categoriaCriada);
        }

        [HttpPut("{id:int:min(1)}")]
        public ActionResult Put(int id, Categoria categoria)
        {
            if (id != categoria.CategoriaId) return BadRequest();

            Categoria categoriaAtualizada = _repository.Update(id, categoria);
            
            return Ok(categoriaAtualizada);
        }

        [HttpDelete("{id:int:min(1)}")]
        public ActionResult Delete(int id)
        {
            Categoria categoria = _repository.GetCategoria(id);

            if (categoria == null) return NotFound($"A categoria de id {id} não existe.");

            Categoria categoriaExcluida = _repository.Delete(id);

            return Ok(categoriaExcluida);
        }

    }
}
