using JobSchedulingApi.Models;
using JobSchedulingApi.Services.JobServices.JobManagementServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quartz;

namespace JobSchedulingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly IJobManagement _jobManagement;

        public JobsController(IJobManagement jobManagement)
        {
            _jobManagement = jobManagement;
        }

        [HttpGet("StopJob")]
        public async Task<IActionResult> StopJob()
        {
            await _jobManagement.PauseJob();

            return Ok();
        }

        [HttpGet("ResumeJob")]
        public async Task<IActionResult> ResumeJob()
        {
            await _jobManagement.ResumeJob();

            return Ok();
        }

        [HttpPost("ChangeSchedule")]
        public async Task<IActionResult> ChangeSchedule([FromBody] ConfiguredSchedule configuredSchedule)
        {
            await _jobManagement.RescheduleJob(configuredSchedule);

            return Ok();
        }

        [HttpGet("GetSchedule")]
        public IActionResult GetSchedule()
        {
            return Ok(_jobManagement.GetReschedule());
        }
    }
}
