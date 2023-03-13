using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }
        //Exibindo os produtos
        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            try
            {
                var produtos = _context.Produtos.ToList();
                if (produtos is null)
                {
                    return BadRequest("Dados inválidos, Tente novamente  ");
                }
                return produtos;
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitação");

            }
        }

        //exibindo os produtos pelo Id que for fornecido 
        [HttpGet("{id:int}", Name = "ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            try
            {
                var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
                if (produto is null)
                {
                    return NotFound($"Produto com id {id} não encontrado");
                }
                return produto;
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitação");

            }
        }
        //Adcionando um novo produto
        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            try
            {
                if (produto is null) return BadRequest("Dados inválidos");

                _context.Produtos.Add(produto);//Trabalhando na memoria
                _context.SaveChanges(); // Persistindo os dados na tabela 

                return new CreatedAtRouteResult("ObterProduto",
                    new { id = produto.ProdutoId }, produto);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitação");

            }
        }

        //alterando um Produto
        [HttpPut("{id:int}")]
        public ActionResult Put(int id,Produto produto)
        {
            try
            {
                if (id != produto.ProdutoId)
                {
                    return NotFound($"Produto com id {id} não encontrado");
                }

                _context.Entry(produto).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();

                return Ok(produto);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitação");

            }
        }
        [HttpDelete ("{id:int}")]
        public ActionResult Delete(int id)
        {
            try
            {
                //FirstOrDefault localiza primeiro a ocorrencia, se não achar retornal um default
                //var produto = _context.Produtos.FirstOrDefault(p=> p.ProdutoId == id);

                // Find localiza primeiro o produto na memoria, mas o Id tem que ser chave Primaria
                var produto = _context.Produtos.Find(id);
                if (produto is null)
                {
                    return NotFound($"Produto com id {id} não encontrado");
                }
                _context.Produtos.Remove(produto);
                _context.SaveChanges();

                return Ok(produto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitação");


            }


        }
    }
}