using _5PJS.Context;
using _5PJS.Entities;
using _5PJS.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _5PJS.Controllers
{
    [Route("api/Feedback")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public FeedbackController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("View")]
        public List<FeedBack> GetFeedBack(int id = 0)
        {
            List<FeedBack> feedbackList = new List<FeedBack>();

            if (_context.feedBacks.Count() >= id)
            {
                feedbackList = _context.feedBacks.OrderBy(f => f.Id)
                         .Skip(id)
                         .Take(id + 10)
                         .ToList();
            }

            return feedbackList;


        }
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAddress([FromBody] FeedbackRequest request)
        {
            if (!_context.doctors.Any(d => d.Crm == request.CrmDoctor))
            {
                return BadRequest("O Crm no Doutor está invalido");
            }
            if(!_context.diagnosiss.Any(d=> d.Id == request.IdDiagnosis))
            {
                return BadRequest("Esse diagnostico n existe");
            }
            //Cria o usuario para a gente
            var feedback = new FeedBack
            {
                CrmDoctor = request.CrmDoctor,
                IdDiagnosis = request.IdDiagnosis,
                Title = request.Title,
                Comment = request.Comment,
                Sort = request.Sort,
                ShippingDate = DateTime.Now,
                TestDate = request.TestDate

            };

            _context.feedBacks.Add(feedback);
            await _context.SaveChangesAsync();

            return Ok("FeedBack cadastrado com sucesso");
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateFeedback([FromBody] FeedbackUpdateRequest request)
        {
            try
            {
                if (await _context.doctors.AnyAsync(d => d.Crm == request.CrmDoctor))
                {
                    var feedback = _context.feedBacks.Where(f =>
                    f.Id == request.Id).Single();


                    if (!string.IsNullOrEmpty(request.Title))
                        feedback.Title = request.Title;
                    if (!string.IsNullOrEmpty(request.Comment))
                        feedback.Comment = request.Comment;
                    if (!string.IsNullOrEmpty(request.Sort))
                        feedback.Sort = request.Sort;
                    if (request.TestDate > 0)
                        feedback.TestDate = request.TestDate;
                    


                    await _context.SaveChangesAsync();
                    return Ok("Atualização feita com sucesso");



                }
            }
            catch (Exception ex)
            {
                return Ok("Feedback não foi encontrado");
            }

            return Ok("Erro ao inserir os dados");

        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteFeedback([FromBody] FeedbackDeleteRequest request)
        {

            try
            {
                if (await _context.doctors.AnyAsync(d => d.Crm == request.CrmDoctor))
                {
                    FeedBack feedback = await _context.feedBacks.Where(f => f.Id == request.Id).FirstAsync();
                    _context.feedBacks.Remove(feedback);
                    await _context.SaveChangesAsync();
                    return Ok("Feedback deletado com sucesso");


                }

            }
            catch (Exception ex)
            {
                return Ok("Feedback não pode ser deletado");
            }

            return Ok("Erro ao inserir os dados");
        }
    }
}
