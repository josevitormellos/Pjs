using _5PJS.Context;
using _5PJS.Entities;
using _5PJS.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace _5PJS.Controllers
{
    [Route("api/MethodDiagnosis")]
    [ApiController]
    public class MethodDiagnosisController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public MethodDiagnosisController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet("View")]
        public MethodDiagnosis GetMethod(int id= 0)
        {
            MethodDiagnosis method = _context.methodDiagnoses.Where(m => m.Id == id).Single();

            return method;


        }
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAddress([FromBody] MethodDiagnosisRequest request)
        {
            if (!_context.doctors.Any(d => d.Crm == request.CrmDoctor))
            {
                return BadRequest("O Crm no Doutor está invalido");
            }
            //Cria o usuario para a gente
            var method = new MethodDiagnosis
            {
                CrmDoctor = request.CrmDoctor,
                Name = request.Name,
                Description = request.Description,
                Effectiveness = request.Effectiveness,
                ResponseTime = request.ResponseTime,
                Recommendations = request.Recommendations

            };

            _context.methodDiagnoses.Add(method);
            await _context.SaveChangesAsync();

            return Ok("Metodo de Diagnostico cadastrado com sucesso");
        }
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateMethod([FromBody] MethodDiagnosisUpdateRequest request)
        {
            try
            {
                if (await _context.methodDiagnoses.AnyAsync(m => m.CrmDoctor == request.CrmDoctor && m.Id == request.Id))
                {
                    var method = _context.methodDiagnoses.Where(m =>
                    m.Id == request.Id).Single();


                    if (!string.IsNullOrEmpty(request.Name))
                        method.Name = request.Name;
                    if (!string.IsNullOrEmpty(request.Description))
                        method.Description = request.Description;
                    if (request.Effectiveness != method.Effectiveness)
                        method.Effectiveness = request.Effectiveness;
                    if (request.ResponseTime > 0)
                        method.ResponseTime = request.ResponseTime;
                    if (!string.IsNullOrEmpty(request.Recommendations))
                        method.Recommendations = request.Recommendations;
                    


                    await _context.SaveChangesAsync();
                    return Ok("Atualização feita com sucesso");



                }
            }
            catch (Exception ex)
            {
                return Ok("Metodo de Diagnostico não foi encontrado");
            }

            return Ok("Erro ao inserir os dados");

        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteMethodDiagnosis([FromBody] MethodDiagnosisDeleteRequest request)
        {

            try
            {
                if (await _context.methodDiagnoses.AnyAsync(m => m.CrmDoctor == request.CrmDoctor && m.Id == request.Id))
                {
                    var method = _context.methodDiagnoses.Where(m =>
                    m.Id == request.Id).Single();
                    _context.methodDiagnoses.Remove(method);
                    await _context.SaveChangesAsync();
                    return Ok("Metodo de Diagnostico deletado com sucesso");


                }

            }
            catch (Exception ex)
            {
                return Ok("Metodo de Diagnostico não pode ser deletado");
            }

            return Ok("Erro ao inserir os dados");
        }
    }
}
