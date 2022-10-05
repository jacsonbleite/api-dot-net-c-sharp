using Microsoft.AspNetCore.Mvc;
using src.Models;
using Microsoft.EntityFrameworkCore;
using src.Persistence;
using System.Net;

namespace src.Controllers;

[ApiController]
[Route("[controller]")]
public class PessoaController : ControllerBase
{
    private DatabaseContex _context { get; set; }

    public PessoaController(DatabaseContex context)
    {
        this._context = context;
    }

    [HttpGet]
    public ActionResult<List<Pessoa>> Get()
    {
        //Retornos -> Ok = 200, NotContent = 204

        //Inserindo dados
        // Pessoa pessoa = new Pessoa("Jacson", 39,"55577788800");
        // Contrato novoContrato = new Contrato("abc", 50.55);
        // pessoa.contratos.Add(novoContrato);

        var result = _context.Pessoas.Include(p => p.contratos).ToList();

        if (!result.Any()) return NoContent();

        return Ok(result);
    }

    [HttpPost]
    public ActionResult<Pessoa> Post([FromBody] Pessoa pessoa)
    {
        try
        {
            _context.Pessoas.Add(pessoa);
            _context.SaveChanges();
        }
        catch (System.Exception)
        {
            return BadRequest(new {
                msg = "Erro ao cadastrar pessoa",
                status = HttpStatusCode.BadRequest
            });            
        }

        return Created("criado",pessoa);
    }

    [HttpPut("{id}")]
    public ActionResult<Object> Update([FromRoute]int id, [FromBody]Pessoa pessoa)
    {
        var result = _context.Pessoas.SingleOrDefault(e => e.Id == id);
        
        if(result is null){
            return NotFound(new{
                msg = "Registro não encontrado!",
                status = HttpStatusCode.NotFound
            });
        }
        
        try{            
            _context.Pessoas.Update(pessoa);
            _context.SaveChanges();
        }catch (System.Exception){
            return BadRequest(new{
                msg = "Erro: Dados do id " + id + " não atualizado",
                status = HttpStatusCode.OK
            });
        }

        return Ok(new{
            msg = "Dados do id " + id + " atualizados",
            status = HttpStatusCode.OK
        });
    }

    [HttpDelete("{id}")]
    public ActionResult<Object> Delete([FromRoute] int id)
    {
        var result = _context.Pessoas.SingleOrDefault(e => e.Id == id);

        if (result is null)
        {
            return BadRequest(new
            {
                msg = "Pessoa não cadastrada no sistema!",
                status = HttpStatusCode.BadRequest
            });
        }
        _context.Pessoas.Remove(result);
        _context.SaveChanges();

        return Ok(new
        {
            msg = "Pessoa do id " + id + " excluída com sucesso!",
            status = HttpStatusCode.OK
        });
    }

}