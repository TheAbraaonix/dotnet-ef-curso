using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using X.PagedList;

namespace APICatalogo.Repositories
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IPagedList<Categoria>> GetCategoriaFiltroNomeAsync(CategoriasFiltroNome categoriasParams)
        {
            var categorias = await GetAllAsync();

            if (!string.IsNullOrEmpty(categoriasParams.Nome))
            {
                categorias = categorias.Where(c => c.Nome.Contains(categoriasParams.Nome));
            }

            //var categoriasFiltradas = PagedList<Categoria>.ToPagedList(categorias.AsQueryable(), categoriasFiltroNome.PageNumber, categoriasFiltroNome.PageSize);
            var categoriasFiltradas = await categorias.ToPagedListAsync(categoriasParams.PageNumber, categoriasParams.PageSize);
            return categoriasFiltradas;
        }

        public async Task<IPagedList<Categoria>> GetCategoriasAsync(CategoriasParameters categoriaParams)
        {
            var categorias = await GetAllAsync();
            var categoriasOrdenadas = categorias.OrderBy(c => c.CategoriaId).AsQueryable();
            //var resultado = PagedList<Categoria>.ToPagedList(categoriasOrdenadas, categoriaParams.PageNumber, categoriaParams.PageSize);
            var resultado = await categoriasOrdenadas.ToPagedListAsync(categoriaParams.PageNumber, categoriaParams.PageSize);
            return resultado;
        }
    }
}
