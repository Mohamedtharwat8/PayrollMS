using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PayrollMS.Helpers;
using PayrollMS.Models;
using PayrollMS.Services.Interface;
using PayrollMS.Services.Service;
using System.Data;
using System.Net;

namespace PayrollMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Employeer")]
    public class EmployeesController : ControllerBase
    {

        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        [HttpGet("GetAllEmployees")]
        [ProducesResponseType(typeof(ResponseResult<IEnumerable<Employeer>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {

                var reult = await _employeeService.GetAllEmployees().ConfigureAwait(false);
                return Ok(reult);

            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.ToInt(), ex.ToString());
            }
        }



        [HttpGet]
        [Route("GetEmployeeById/{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {

            try
            {

                var reult = await _employeeService.GetEmployeerById(id).ConfigureAwait(false);
                return Ok(reult);

            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.ToInt(), ex.ToString());
            }
        }




    }
}
