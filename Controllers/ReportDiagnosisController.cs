using _5PJS.Context;
using _5PJS.Entities;
using _5PJS.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _5PJS.Controllers
{
    [Route("api/ReportDiagnosis")]
    [ApiController]
    public class ReportDiagnosisController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public ReportDiagnosisController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterReportDiagnosis([FromBody] ReportDiagnosisRequest request)
        {
            if (!_context.reports.Any(r => r.CrmDoctor == request.CrmDoctor && r.Id == request.IdReport))
            {
                return BadRequest("O Crm no Doutor está invalido ou esse Relatorio não é desse doutor");
            }
            //Cria o usuario para a gente
            if(!_context.diagnosiss.Any(d => d.Id == request.IdDiagnosis))
            {
                return BadRequest("O diagnostico não existe");
            }
            var reportDiagnosis = new ReportDiagnosis
            {
                IdDiagnosis = request.IdDiagnosis,
                IdReport = request.IdReport

            };

            _context.reportDiagnoses.Add(reportDiagnosis);
            await _context.SaveChangesAsync();

            return Ok("Esse Diagnostico foi cadastrado com sucesso no Relatorio");
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteReportDiagnosis([FromBody] ReportDiagnosisRequest request)
        {

            try
            {
                if (await _context.reportDiagnoses.AnyAsync(rd => rd.IdDiagnosis == request.IdDiagnosis && rd.IdReport == request.IdReport))
                {
                    ReportDiagnosis reportDiagnosis = await _context.reportDiagnoses.Where(rd => rd.IdDiagnosis == request.IdDiagnosis && rd.IdReport == request.IdReport).FirstAsync();
                    _context.reportDiagnoses.Remove(reportDiagnosis);
                    await _context.SaveChangesAsync();
                    return Ok("Diagnostico deletado com sucesso do Relatorio");


                }

            }
            catch (Exception ex)
            {
                return Ok("Diagnostico não pode ser deletado do Relatorio");
            }

            return Ok("Erro ao inserir os dados");
        }
    }
}
