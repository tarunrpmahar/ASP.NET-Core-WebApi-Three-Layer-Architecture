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
            _telemetryClient.TrackEvent("TSM in GET | telemetry");
            _logger.LogInformation("TSM in GET | serilog");
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
            _telemetryClient.TrackEvent("TSM in GetById | telemetry");
            _logger.LogInformation("TSM in GetById | serilog");
            if (id == null)
            {
                return BadRequest();
            }

            try
            {
                var response = _commander.GetCommandById(id);
                if (response == null)
                {
                    _telemetryClient.TrackEvent("TSM Id not valid | telemetry");
                    _logger.LogInformation("TSM Id not valid try | serilog");
                    return NotFound();
                }
                var result = _mapper.Mapper.Map<CommandDTO>(response);
                return Ok(result);
            }
            catch (Exception)
            {
                _telemetryClient.TrackEvent("TSM GetById | telemetry");
                _logger.LogInformation("TSM Bad request | serilog");
                return BadRequest();
            }
        }

        [HttpPost]
        public ActionResult CreateCommand(CommandDTO commandDTO) //<CommandDTO>
        {
            _telemetryClient.TrackEvent("TSM in CreateCommand | telemetry");
            _logger.LogInformation("TSM in CreateCommand | serilog");
            if (ModelState.IsValid)
            {
                _telemetryClient.TrackEvent("TSM Model Valid in CreateCommand | telemetry");
                _logger.LogInformation("TSM Model Valid in CreateCommand | serilog");

                var details = _mapper.Mapper.Map<CommandDTO, Command>(commandDTO);
                var response = _commander.CreateCommand(details);

                var result = _mapper.Mapper.Map<Command, CommandDTO>(response);

                return new OkObjectResult(result);
            }
            else
            {
                _telemetryClient.TrackEvent("TSM Model Not Valid in CreateCommand | telemetry");
                _logger.LogInformation("TSM Model Not Valid in CreateCommand | serilog");
                return BadRequest();
            }          
        }

        //[HttpPost]
        /*public ActionResult CreateCommand(Command command)
        {
            if(ModelState.IsValid)
            {
                var response = _commander.CreateCommand(command);
                return new OkObjectResult(response);
            }

            return BadRequest();          
        }*/

        [HttpPut("{id}")]
        public IActionResult UpdateCommand(int id, [FromBody] JObject jObject) // 
        {
            try
            {
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
