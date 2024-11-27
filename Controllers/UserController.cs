using _5PJS.Context;
using _5PJS.Entities;
using _5PJS.Request;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _5PJS.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("Login")]
        public async Task<IActionResult> Login([FromBody] Request.LoginRequest request)
        {
            var user = await _context.users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return Unauthorized("Credenciais inválidas.");
            }
            Doctor doctor = _context.doctors.Where(d => d.IdUser == user.Id).Single();
            UserRequest userRequest = new UserRequest
            {
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                DateBirth = user.DateBirth,
                Crm = doctor.Crm,
                Specialty = doctor.Specialty
            };

            return Ok(userRequest);
        }

        [HttpGet("ViewDoctor")]
        public IActionResult GetDoctor(string crm, string nome, int id = 0)
        {
            var result = (from user in _context.users
                         join doctor in _context.doctors
                         on user.Id equals doctor.IdUser
                         select new
                         {
                             UserId = user.Id,
                             UserName = user.Name,
                             UserEmail = user.Email,
                             UserPhoje = user.Phone,
                             DoctorCrm = doctor.Crm,
                             DoctorSpecialty = doctor.Specialty
                         }).ToList();
            if (!string.IsNullOrEmpty(nome))
            {
                result = result.Where(d => _context.users.Where(u => u.Name.StartsWith(nome)).Select(u=> u.Id).ToList().Contains(d.UserId)).ToList();
            }
            else if (!string.IsNullOrEmpty(crm))
            {
                result = result.Where(d => d.DoctorCrm == crm).ToList();
            }

            if (_context.doctors.Count() >= id)
            {
                result = result.OrderBy(d => d.UserId)
                         .Skip(id)
                         .Take(id + 10)
                         .ToList();
            }

            return Ok(result);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserRequest request)
        {
            //Verificação dos dados basica
            if (await _context.users.AnyAsync(u => u.Email == request.Email))
            {

                return BadRequest("Email já registrado.");
            }

            if (request.Password != request.PasswordConfirmed)
            {

                return BadRequest("O cadastro está com senha incompativeis");
            }

            //Cria o usuario para a gente
            var user = new User
            {

                Email = request.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Name = request.Name,
                Phone = request.Phone, 
                DateBirth = request.DateBirth
            };

            //Procura o maior id + 1 para encontrar o proximo id, para fazer a chave estrageira de doctor
            int idNext = (int)_context.users.Max(u => u.Id) + 1;
            _context.users.Add(user);
            _context.SaveChanges();

            //Cria o Doctor para a gente
            var doctor = new Doctor
            {
                IdUser = idNext,
                Crm = request.Crm,
                Specialty = request.Specialty
            };

            _context.doctors.Add(doctor);
            await _context.SaveChangesAsync();


            return Ok("Usuário registrado.");
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateRequest request)
        {
            try
            {
                if (await _context.users.AnyAsync(u => u.Email == request.EmailVerific))
                {
                    var user = _context.users.Where(u => u.Email == request.EmailVerific).Single();

                    if (BCrypt.Net.BCrypt.Verify(request.PasswordVerific, user.Password))
                    {

                        if (!string.IsNullOrEmpty(request.Name))
                            user.Name = request.Name;
                        if (!string.IsNullOrEmpty(request.Email) && ValidateDate.ValidateEmail(request.Email, _context))
                            user.Email = request.Email;
                        if (!string.IsNullOrEmpty(request.Password))
                            user.Password = request.Password;
                        if (!string.IsNullOrEmpty(request.Phone))
                            user.Phone = request.Phone;
                        if (request.DateBirth != user.DateBirth)
                            user.DateBirth = request.DateBirth;
                        if (request.IdSpecialty != 0)
                            user.Password = request.Password;

                        await _context.SaveChangesAsync();
                        return Ok("Atualização feita com sucesso");

                    }

                }
            }
            catch (Exception ex)
            {
                return Ok("Usuario não foi encontrado");
            }

            return Ok("Erro ao inserir os dados");

        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteUsuario([FromBody] Request.LoginRequest request)
        {

            try
            {
                if (await _context.users.AnyAsync(u => u.Email == request.Email))
                {
                    var user = _context.users.Where(u => u.Email == request.Email).Single();

                    if (BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                    {
                        _context.users.Remove(user);
                        await _context.SaveChangesAsync();
                        return Ok("Usuario deletado com sucesso");
                    }

                }
                
            }
            catch (Exception ex)
            {
                return Ok("Usuario não foi encontrado");
            }

            return Ok("Erro ao inserir os dados");
        }

    }
}
