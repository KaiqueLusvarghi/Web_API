using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _Context; // Injeção de depencia no contrutor

        public CategoriasController(AppDbContext context)
        {
            _Context = context;
        }

        //Obetendo as cateogrias e incluindo os produtos das categorias
        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            try
            {
                // return _Context.Categorias.Include(p=> p.Produtos).AsNoTracking().ToList();  nunca retornar todos objetos, sem aplicar um filtro
                return _Context.Categorias.Include(p => p.Produtos).Where(c => c.CategoriaId <= 10).ToList();
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitação"); 
            }
        }

        //retornando todas as cateogiras

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            try
            {
                return _Context.Categorias.AsNoTracking().ToList();
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitação");
            }
        }




        //retornando as categorias por Id
        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(int id) 
        {
            try
            {
                var categoria = _Context.Categorias.FirstOrDefault(p => p.CategoriaId == id);
                if (categoria == null)
                {
                    return NotFound($"Categoria com o id {id} não encontrada");
                }
                return Ok(categoria);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitação");

            }
        }

        //Adcionando uma nova categoria
        [HttpPost]
        public ActionResult Post(Categoria categoria)
        {
            try
            {
                if (categoria is null)
                    return BadRequest("Dados invalidos, tente novamente");

                _Context.Categorias.Add(categoria);
                _Context.SaveChanges();

                return new CreatedAtRouteResult("ObterCategoria",
                    new { id = categoria.CategoriaId }, categoria);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitação");
            }
        }
        //Atualizando uma categoria
        [HttpPut("{id:int}")]

        public ActionResult Put(int id,Categoria categoria)
        {
            try
            {
                if (id != categoria.CategoriaId)
                {
                    return BadRequest("Dados invalidos, tente novamente");
                }
                _Context.Entry(categoria).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _Context.SaveChanges();
                return Ok(categoria);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitação");

            }
        }

        //deletando uma categoria
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id) 
        {
            try
            {
                var categoria = _Context.Categorias.FirstOrDefault(p => p.CategoriaId == id);
                if (categoria == null)
                {
                    return NotFound($"Categoria com o id {id} não encontrada");
                }
                _Context.Categorias.Remove(categoria);
                _Context.SaveChanges();
                return Ok(categoria);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitação");

            }
        }
    }

}
