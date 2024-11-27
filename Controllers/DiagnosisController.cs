using _5PJS.Context;
using _5PJS.Entities;
using _5PJS.Request;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace _5PJS.Controllers
{
    [Route("api/Diagnosis")]
    [ApiController]
    public class DiagnosisController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public DiagnosisController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("View")]
        public List<Diagnosis> GetDiagnosis(int id = 0)
        {
            List<Diagnosis> diagnosissList = new List<Diagnosis>();

            if(_context.diagnosiss.Count() >= id)
            {
                diagnosissList = _context.diagnosiss.OrderBy(d => d.Id)
                         .Skip(id)
                         .Take(id + 10)
                         .ToList();
            }

            return diagnosissList;

            
        }

        [HttpGet("ViewFilter")]
        public List<Diagnosis> GetFilterDiagnosis(FilterDiagnosisRequest request)
        {

            List<Diagnosis> diagnosissList = new List<Diagnosis>();
            if (request.PacientFilter)
            {
                diagnosissList = PacientFilter(diagnosissList, request);
            }
            if (request.DocterFilter)
            {
                diagnosissList= DoctorFilter(diagnosissList, request);
            }
            if (request.FeedBackFilter)
            {
                diagnosissList = FeedBackFilter(diagnosissList, request);
            }
            if (request.MethodDiagnosisFilter)
            {
                diagnosissList = MethodDiagnosisFilter(diagnosissList, request);
            }
            if (request.AddressFilter)
            {
                diagnosissList = AddressFilter(diagnosissList,request);
            }
            

            return diagnosissList;


        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterDiagnosis([FromBody] DiagnosisRequest request)
        {
           

            if(!_context.doctors.Any(d => d.Crm == request.CrmDoctor))
            {
                return BadRequest("O Crm no Doutor está invalido");
            }
            if (!_context.patients.Any(p => p.Cpf == request.CpfPatient))
            {
                return BadRequest("O paciente não está cadastrado");
            }
            if (!_context.methodDiagnoses.Any(md => md.Id == request.IdMethodDiagnosis))
            {
                return BadRequest("Não existe esse metodo de diagnostico cadastrado");
            }

            //Cria o usuario para a gente
            var diagnosis = new Diagnosis
            {

                CrmDoctor = request.CrmDoctor,
                CpfPatient = request.CpfPatient,
                Date = request.Date,
                Result = request.Result,
                IdMethodDiagnosis = request.IdMethodDiagnosis,
                Observation = request.Observation
            };

            _context.diagnosiss.Add(diagnosis);
           await _context.SaveChangesAsync();

            return Ok("Diagnostico cadastrado com sucesso");
        }
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateDiagnosis([FromBody] DiagnosisUpdateRequest request)
        {
            try
            {
                if (await _context.diagnosiss.AnyAsync(d => d.CrmDoctor == request.CrmDoctor))
                {
                    var diagnosis = _context.diagnosiss.Where(d => d.Id == request.Id).Single();

                   

                        if (_context.patients.Any(p => p.Cpf == request.CpfPatient))
                            diagnosis.CpfPatient = request.CpfPatient;
                        if (request.Result != diagnosis.Result)
                            diagnosis.Result = request.Result;
                        if (_context.methodDiagnoses.Any(md => md.Id == request.IdMethodDiagnosis))
                            diagnosis.IdMethodDiagnosis = request.IdMethodDiagnosis;
                        if (!string.IsNullOrEmpty(request.Observation))
                            diagnosis.Observation = request.Observation;
                        

                        await _context.SaveChangesAsync();
                        return Ok("Atualização feita com sucesso");

                   

                }
            }
            catch (Exception ex)
            {
                return Ok("Diagnostico não foi encontrado");
            }

            return Ok("Erro ao inserir os dados");

        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteDiagnosis([FromBody] DiagnosisDeleteRequest request)
        {

            try
            {
                if (await _context.diagnosiss.AnyAsync(d => d.CrmDoctor == request.CrmDoctor && d.Id == request.Id))
                {
                        Diagnosis diagnosis= await _context.diagnosiss.Where(d => d.Id == request.Id).FirstAsync();
                        _context.diagnosiss.Remove(diagnosis);
                        await _context.SaveChangesAsync();
                        return Ok("Diagnostico deletado com sucesso");
                    

                }

            }
            catch (Exception ex)
            {
                return Ok("Diagnostico não pode ser deletado");
            }

            return Ok("Erro ao inserir os dados");
        }

        public List<Diagnosis> PacientFilter( List<Diagnosis> diagnosesList, FilterDiagnosisRequest request)
        {
            List<Patient> patients = _context.patients.ToList();
            if (!string.IsNullOrEmpty(request.NamePatient))
            {
                patients = patients.Where(p => p.Name.StartsWith(request.NamePatient))
                         .ToList();
            }
            if (!string.IsNullOrEmpty(request.CpfPatient)){
               patients = patients.Where(p => p.Cpf == request.CpfPatient).ToList();
            }
            else if(request.DateBirthPatient != null)
            {
                patients = patients.Where(p => p.DateBirth.Year == request.DateBirthPatient.Value.Year).ToList();
            }
            List<string> patientsCpf = patients.Select( p => p.Cpf).ToList();

            return diagnosesList.Where(d => patientsCpf.Contains(d.CpfPatient)).ToList();
        }

        public List<Diagnosis> DoctorFilter(List<Diagnosis> diagnosesList, FilterDiagnosisRequest request)
        {
            List<User> users = _context.users.ToList();
            List<Doctor> doctors = _context.doctors.ToList();
          
            if (!string.IsNullOrEmpty(request.CrmDoctor)){
                doctors = doctors.Where(d => d.Crm == request.CrmDoctor).ToList();
            }
            else if (!string.IsNullOrEmpty(request.NameDoctor))
            {
                users = users.Where(u => u.Name.StartsWith(request.NamePatient))
                         .ToList();
                doctors = doctors.Where(d => users.Select(u => u.Id).Contains(d.IdUser)).ToList();
            }

            List<string> docterCrm = doctors.Select(d => d.Crm).ToList();

            return diagnosesList.Where(d => docterCrm.Contains(d.CrmDoctor)).ToList();
        }

        public List<Diagnosis> FeedBackFilter(List<Diagnosis> diagnosesList, FilterDiagnosisRequest request)
        {
            List<FeedBack> feedBack = _context.feedBacks.ToList();

            if (!request.FeedBackView)
            {
                return diagnosesList.Where(d => !d.Feedbacks.Any())
                                       .ToList();
            }
            else
            {
                if (!string.IsNullOrEmpty(request.SortFeedBack))
                {
                    feedBack = feedBack.Where(f => f.Sort.StartsWith(request.SortFeedBack)).ToList();
                }
                if(request.ShippingDateFeedBack != null)
                {
                    feedBack = feedBack.Where(f => f.ShippingDate == request.ShippingDateFeedBack.Value).ToList();
                }
                if(request.TestDateFeedBack > 0)
                {
                    feedBack = feedBack.Where(f => f.TestDate <= request.TestDateFeedBack).ToList();
                }
            }
            List<int> FeedBackIdDignosis = feedBack.Select(d => d.Id).ToList();

            return diagnosesList.Where(d => FeedBackIdDignosis.Contains(d.Id)).ToList();
        }

        public List<Diagnosis> MethodDiagnosisFilter(List<Diagnosis> diagnosesList, FilterDiagnosisRequest request)
        {
            List<MethodDiagnosis> method = _context.methodDiagnoses.ToList();

            if (!string.IsNullOrEmpty(request.NameMethodDiagnosis))
            {
                method = method.Where(m => m.Name.StartsWith(request.NameMethodDiagnosis)).ToList();
                
            }
           if(request.ResponseTime > 0)
            {
                method = method.Where(m => m.ResponseTime <= request.ResponseTime).ToList();
            }
            List<int> MethodDignosisList = method.Select(d => d.Id).ToList();

            return diagnosesList = diagnosesList.Where(d => MethodDignosisList.Contains(d.IdMethodDiagnosis)).ToList(); ;
        }

        public List<Diagnosis> AddressFilter(List<Diagnosis> diagnosesList, FilterDiagnosisRequest request)
        {
            List<Address> address = _context.addresses.ToList();

            if (!string.IsNullOrEmpty(request.PublicPlace))
            {
                address = address.Where(f => f.PublicPlace.StartsWith(request.PublicPlace)).ToList();
            }
            if (!string.IsNullOrEmpty(request.Complement))
            {
                address = address.Where(f => f.Complement.StartsWith(request.Complement)).ToList();
            }
            if (!string.IsNullOrEmpty(request.Neighborhood))
            {
                address = address.Where(f => f.Neighborhood.StartsWith(request.Neighborhood)).ToList();
            }
            if (!string.IsNullOrEmpty(request.City))
            {
                address = address.Where(f => f.City.StartsWith(request.City)).ToList();
            }
            if (!string.IsNullOrEmpty(request.State))
            {
                address = address.Where(f => f.State.StartsWith(request.State)).ToList();
            }
            if (!string.IsNullOrEmpty(request.Cep))
            {
                address = address.Where(f => f.Cep.StartsWith(request.Cep)).ToList();
            }
            if (request.ResponseTime > 0)
            {
                address = address.Where(m => m.Number <= request.Number).ToList();
            }
            List<int> resultAddress = address.Select(a => a.Id).ToList();
            List<string> PatientAddress = _context.patients.Where(p => resultAddress.Contains(p.IdAddress)).Select(p => p.Cpf).ToList();

            

            return diagnosesList.Where(d => PatientAddress.Contains(d.CpfPatient)).ToList();
        }
    }
}
