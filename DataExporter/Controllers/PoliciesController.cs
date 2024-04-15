using DataExporter.Dtos;
using DataExporter.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DataExporter.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PoliciesController : ControllerBase
    {
        private PolicyService _policyService;

        public PoliciesController(PolicyService policyService) 
        { 
            _policyService = policyService;
        }

        [HttpPost]
        public async Task<IActionResult> PostPolicies([FromBody]CreatePolicyDto createPolicyDto)
        {
            var policy = await _policyService.CreatePolicyAsync(createPolicyDto);

            if (policy == null)
            {
                return StatusCode(500);
            }
            return Ok(policy);
        }


        [HttpGet]
        public async Task<IActionResult> GetPolicies()
        {
            return Ok(await _policyService.ReadPoliciesAsync());
        }

        /// <summary>
        /// 
        /// Changes for this method:
        /// 
        /// 
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns></returns>
        [HttpGet("{policyId}")]
        public async Task<IActionResult> GetPolicy(int policyId) // Parameter name needs to match the route attribute
        {
            var policy = await _policyService.ReadPolicyAsync(policyId); // Need to await async call
            if (policy == null)
            {
                return NotFound(); // Handle not found
            }

            return Ok(policy); 
        }


        [HttpPost("export")]
        public async Task<IActionResult> ExportData([FromQuery]DateTime startDate, [FromQuery] DateTime endDate)
        {
            if (startDate == default(DateTime))
            {
                return BadRequest("Please enter a valid Start Date.");
            }
            if (endDate == default(DateTime))
            {
                return BadRequest("Please enter a valid End Date.");
            }

            var exportData = await _policyService.ExportPoliciesAsync(startDate, endDate);

            return Ok(exportData);
        }
    }
}
