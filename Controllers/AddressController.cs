using _5PJS.Context;
using _5PJS.Entities;
using _5PJS.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _5PJS.Controllers
{
    [Route("api/Address")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AddressController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("View")]
        public Address GetAddress(int id)
        {
            Address address = _context.addresses.Where(a => a.Id == id).Single();

            return address;


        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAddress([FromBody] AddressRequest request)
        {

            //Cria o usuario para a gente
            var address = new Address
            {
                PublicPlace = request.PublicPlace,
                Number = request.Number,
                Complement = request.Complement,
                Neighborhood = request.Neighborhood,
                City = request.City,
                State = request.State,
                Cep = request.Cep


            };

            _context.addresses.Add(address);
            await _context.SaveChangesAsync();

            return Ok("Endereço cadastrado com sucesso");
        }
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateAddress([FromBody] AddressUpdateRequest request)
        {
            try
            {
                if (await _context.patients.AnyAsync(p => p.CrmDoctor == request.CrmDoctor && p.Cpf == request.CpfPatient))
                {
                    var address = _context.addresses.Where(a => 
                    a.Id == _context.patients.Where(p => 
                    p.Cpf == request.CpfPatient).
                    Select(p => p.IdAddress).Single()).Single();

                   
                    if (!string.IsNullOrEmpty(request.PublicPlace))
                        address.PublicPlace = request.PublicPlace;
                    if (!string.IsNullOrEmpty(request.Complement))
                        address.Complement = request.Complement;
                    if (!string.IsNullOrEmpty(request.Neighborhood))
                        address.Neighborhood = request.Neighborhood;
                    if (!string.IsNullOrEmpty(request.City))
                        address.City = request.City;
                    if (!string.IsNullOrEmpty(request.State))
                        address.State = request.State;
                    if (!string.IsNullOrEmpty(request.Cep))
                        address.Cep = request.Cep;
                    if(request.Number> 0)
                        address.Number = request.Number;


                    await _context.SaveChangesAsync();
                    return Ok("Atualização feita com sucesso");



                }
            }
            catch (Exception ex)
            {
                return Ok("Endereço não foi encontrado");
            }

            return Ok("Erro ao inserir os dados");

        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteAddress([FromBody] AddressDeleteRequest request)
        {

            try
            {
                if (await _context.patients.AnyAsync(p => p.CrmDoctor == request.CrmDoctor && p.Cpf == request.CpfPatient))
                {
                    var address = _context.addresses.Where(a =>
                    a.Id == _context.patients.Where(p =>
                    p.Cpf == request.CpfPatient).
                    Select(p => p.IdAddress).Single()).Single();
                    _context.addresses.Remove(address);
                    await _context.SaveChangesAsync();
                    return Ok("Endereço deletado com sucesso");


                }

            }
            catch (Exception ex)
            {
                return Ok("Endereço não pode ser deletado");
            }

            return Ok("Erro ao inserir os dados");
        }
    }
}
