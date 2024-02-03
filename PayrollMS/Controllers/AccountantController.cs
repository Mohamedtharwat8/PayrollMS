using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PayrollMS.Dtos;
using PayrollMS.Helpers;
using PayrollMS.Models;
using PayrollMS.Services.Interface;
using PayrollMS.Services.Service;
using System.Data;
using System.Linq;
using System.Net;

namespace PayrollMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Accountant")]

    public class AccountantController : ControllerBase
    {
        private readonly ISalaryService _salaryService;

        public AccountantController(ISalaryService salaryService)
        {
            _salaryService = salaryService;
        }



        [HttpGet]
        [Route("GetSalaryApprovalRequest/{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {

            try
            {

                var reult = await _salaryService.GetSalaryApprovalRequest(id).ConfigureAwait(false);
                return Ok(reult);

            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.ToInt(), ex.ToString());
            }
        }



        [HttpPost]
        [Route("AddSalary")]
        public async Task<IActionResult> AddSalary([FromBody] Salary salary)
        {

            try
            {

                var reult = await _salaryService.AddSalary(salary).ConfigureAwait(false);
                return Ok(reult);

            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.ToInt(), ex.ToString());
            }
        }



        [HttpPost]
        [Route("AddSalaryRequest")]
        public async Task<IActionResult> AddSalaryRequest([FromBody] SalaryRequest salaryRequest)
        {

            try
            {

                var reult = await _salaryService.AddSalaryApprovalRequest(salaryRequest).ConfigureAwait(false);
                return Ok(reult);

            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.ToInt(), ex.ToString());
            }
        }


        


    }
}
