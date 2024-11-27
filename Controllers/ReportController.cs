using _5PJS.Context;
using _5PJS.Entities;
using _5PJS.Request;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _5PJS.Controllers
{
    [Route("api/Report")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public ReportController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet("View")]
        public List<Report> GetReport(int id = 0)
        {
            List<Report> reportList = new List<Report>();

            if (_context.reports.Count() >= id)
            {
                reportList = _context.reports.OrderBy(f => f.Id)
                         .Skip(id)
                         .Take(id + 10)
                         .ToList();
            }

            return reportList;


        }

        [HttpGet("ViewDoctor")]
        public List<Diagnosis> GetReportDoctor(string Crm, int id)
        {
            List<Diagnosis> DiagnosisList = new List<Diagnosis>();
            if (_context.reports.Any(r => r.CrmDoctor == Crm && r.Id == id))
            {
                Report report = _context.reports.Where(r => r.Id == id).Single();
                List<int> IdDiagnosis = _context.reportDiagnoses.Where(rd => rd.IdReport == report.Id).Select(rd => rd.IdDiagnosis).ToList();
                DiagnosisList = _context.diagnosiss.Where(d => IdDiagnosis.Contains(d.Id)).ToList();
                
            }


            return DiagnosisList;

        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterReport([FromBody] ReportRequest request)
        {
            if (!_context.doctors.Any(d => d.Crm == request.CrmDoctor))
            {
                return BadRequest("O Crm no Doutor está invalido");
            }
            //Cria o usuario para a gente
            var report = new Report
            {
                CrmDoctor = request.CrmDoctor,
                Name = request.Name,
                Description = request.Description,
                Date = DateTime.Now

            };

            _context.reports.Add(report);
            await _context.SaveChangesAsync();

            return Ok("Relatorio cadastrado com sucesso");
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateReport([FromBody] ReportUpdateRequest request)
        {
            try
            {
                if (await _context.reports.AnyAsync(r => r.CrmDoctor == request.CrmDoctor && r.Id == request.Id))
                {
                    var report = _context.reports.Where(r =>
                    r.Id == request.Id).Single();


                    if (!string.IsNullOrEmpty(request.Name))
                        report.Name = request.Name;
                    if (!string.IsNullOrEmpty(request.Description))
                        report.Description = request.Description;
                    



                    await _context.SaveChangesAsync();
                    return Ok("Atualização feita com sucesso");



                }
            }
            catch (Exception ex)
            {
                return Ok("Relatorio não foi encontrado");
            }

            return Ok("Erro ao inserir os dados");

        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteReport([FromBody] ReportDeleteRequest request)
        {

            try
            {
                if (await _context.reports.AnyAsync(r => r.CrmDoctor == request.CrmDoctor && r.Id == request.Id))
                {
                    Report report = await _context.reports.Where(r => r.Id == request.Id).FirstAsync();
                    _context.reports.Remove(report);
                    await _context.SaveChangesAsync();
                    return Ok("Relatorio deletado com sucesso");


                }

            }
            catch (Exception ex)
            {
                return Ok("Relatorio não pode ser deletado");
            }

            return Ok("Erro ao inserir os dados");
        }
    }
}
