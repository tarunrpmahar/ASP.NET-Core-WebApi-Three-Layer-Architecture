using AutoMapper;
using Domain.Contracts.Domain;
using Domain.Models.Models.Domain;
using Domain.Models.Models.PresentationDTO;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility;
using Serilog;

namespace ApiThreeLayerArch.Controllers
{
    [ApiController]
    [Route("api/commands")]

    public class CommandsController : Controller
    {
        private readonly ICommander _commander;
        private readonly CommandProfile _mapper;
        private readonly ILogger<CommandsController> _logger;
        private readonly TelemetryClient _telemetryClient;

        public CommandsController(ICommander commander, CommandProfile mapper, ILogger<CommandsController> logger, TelemetryClient telemetryClient)    //IMapper mapper   
        {
            _commander = commander;
            _mapper = mapper;
            _logger = logger;
            _telemetryClient = telemetryClient;
        }

        [HttpGet]
        public ActionResult Get()  //<IEnumerable<Command>>
        {
            _telemetryClient.TrackEvent("Logging - in GET (controller) | telemetry");
            _logger.LogInformation("Logging - in GET (controller) | serilog");

            //throw new UnauthorizedAccessException();

            try
            {
                var response = _commander.GetAllCommand();
                if (response == null)
                {
                    return NotFound();
                }
                var result = _mapper.Mapper.Map<IEnumerable<Command>, IEnumerable<CommandDTO>>(response);
                return new OkObjectResult(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public ActionResult GetById(int? id)   //<Command>
        {
            _telemetryClient.TrackEvent("Logging - in GetById (controller) | telemetry");
            _logger.LogInformation("Logging - in GetById (controller) | serilog");
            if (id == null)
            {
                return BadRequest();
            }

            try
            {
                var response = _commander.GetCommandById(id);
                if (response == null)
                {
                    _telemetryClient.TrackEvent("Logging - Id not valid (controller) | telemetry");
                    _logger.LogInformation("Logging - Id not valid try (controller) | serilog");
                    return NotFound();
                }
                var result = _mapper.Mapper.Map<CommandDTO>(response);
                return Ok(result);
            }
            catch (Exception)
            {
                _telemetryClient.TrackEvent("Logging - GetById (controller) | telemetry");
                _logger.LogInformation("Logging - Bad request (controller) | serilog");
                return BadRequest();
            }
        }

        [HttpPost]
        public ActionResult CreateCommand(CommandDTO commandDTO) //<CommandDTO>
        {
            _telemetryClient.TrackEvent("Logging - in CreateCommand (controller) | telemetry");
            _logger.LogInformation("Logging - in CreateCommand (controller) | serilog");

            //throw new Exce

            if (ModelState.IsValid)
            {
                _telemetryClient.TrackEvent("Logging - Model Valid in CreateCommand (controller) | telemetry");
                _logger.LogInformation("Logging - Model Valid in CreateCommand (controller) | serilog");

                var details = _mapper.Mapper.Map<CommandDTO, Command>(commandDTO);
                var response = _commander.CreateCommand(details);

                var result = _mapper.Mapper.Map<Command, CommandDTO>(response);

                return new OkObjectResult(result);
            }
            else
            {
                _telemetryClient.TrackEvent("Logging - Model Not Valid in CreateCommand (controller) | telemetry");
                _logger.LogInformation("Logging - Model Not Valid in CreateCommand (controller) | serilog");
                //throw new Exception("not correct tsm");
                return BadRequest();
            }

        }

        [HttpPut("{id}")]
        public IActionResult UpdateCommand(int id, [FromBody] JObject jObject) // 
        {
            try
            {
                _telemetryClient.TrackEvent("Logging - in UpdateCommand (controller) | telemetry");
                _logger.LogInformation("Logging - in UpdateCommand (controller) | serilog");
                //var tarun = jObject.ToString();
                //DeserializeObject = json->obj
                var dataKeyValue = JsonConvert.DeserializeObject<Dictionary<string, object>>(jObject.ToString());
                var response = _commander.UpdateCommand(id, dataKeyValue);
                var result = _mapper.Mapper.Map<Command, CommandDTO>(response);
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
