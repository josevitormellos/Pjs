using _5PJS.Context;
using _5PJS.Entities;
using _5PJS.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _5PJS.Controllers
{
    [Route("api/Patient")]
    [ApiController]
    public class PatientController : ControllerBase
    {

        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public PatientController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("View")]
        public Patient GetPatient(string Cpf)
        {
            Patient patient = _context.patients.Where(p => p.Cpf == Cpf).Single();

            return patient;


        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterPatient([FromBody] PatientRequest request)
        {


            if (_context.patients.Any(p => p.Cpf == request.Cpf) || request.Cpf == null)
            {
                return BadRequest("Cpf invalido ou em Uso");
            }
            if (!_context.doctors.Any(d => d.Crm == request.CrmMedico))
            {
                return BadRequest("Esse doutor não existe");
            }
            

            //Cria o usuario para a gente
            var patient = new Patient
            {
                CrmDoctor = request.CrmMedico,
                Cpf = request.Cpf,
                DateBirth = request.DateBirth,
                Name = request.Name,
                
                
            };

            _context.patients.Add(patient);
            await _context.SaveChangesAsync();

            //Criando o endereço    
            AddressController address = new AddressController(_context);
            AddressRequest addressRequest = new AddressRequest
            {
                PublicPlace = request.PublicPlace,
                Number = request.Number,
                Complement = request.Complement,
                Neighborhood = request.Neighborhood,
                City = request.City,
                State = request.State,
                Cep = request.Cep
            };
            address.RegisterAddress(addressRequest);
            Patient patientFinal = _context.patients.OrderByDescending(e => e.Id)
                              .First();
            patientFinal.IdAddress = patientFinal.Id;
            await _context.SaveChangesAsync();

            return Ok("Paciente cadastrado com sucesso");
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdatePatient([FromBody] PatientUpdateRequest request)
        {
            try
            {
                if (await _context.patients.AnyAsync(p => p.CrmDoctor == request.CrmDoctor && p.Cpf == request.Cpf))
                {
                    var patient = _context.patients.Where(p => p.Cpf == request.Cpf).Single();

                    if (request.DateBirth != null)
                        patient.DateBirth = request.DateBirth.Value;
                    if (!string.IsNullOrEmpty(request.Name))
                        patient.Name = request.Name;


                    await _context.SaveChangesAsync();
                    return Ok("Atualização feita com sucesso");



                }
            }
            catch (Exception ex)
            {
                return Ok("Paciente não foi encontrado");
            }

            return Ok("Erro ao inserir os dados");

        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeletePatient([FromBody] PatientDeleteRequest request)
        {

            try
            {
                if (await _context.patients.AnyAsync(p => p.CrmDoctor == request.CrmDoctor && p.Cpf == request.Cpf))
                {
                    Patient patient = await _context.patients.Where(p => p.Cpf == request.Cpf).FirstAsync();
                    _context.patients.Remove(patient);
                    await _context.SaveChangesAsync();
                    return Ok("Paciente deletado com sucesso");


                }

            }
            catch (Exception ex)
            {
                return Ok("Paciente não pode ser deletado");
            }

            return Ok("Erro ao inserir os dados");
        }
    }
}
