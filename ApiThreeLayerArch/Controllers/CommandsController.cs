using AutoMapper;
using Domain.Contracts.Domain;
using Domain.Models.Models.Domain;
using Domain.Models.Models.PresentationDTO;
using Microsoft.AspNetCore.Mvc;
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

        public CommandsController(ICommander commander, CommandProfile mapper)    //IMapper mapper
        {
            _commander = commander;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult Get()  //<IEnumerable<Command>>
        {
            var response = _commander.GetAllCommand();
            var result = _mapper.Mapper.Map<IEnumerable<Command>, IEnumerable<CommandDTO>>(response);
            return new OkObjectResult(result);
        }

        [HttpGet("{id}")]
        public ActionResult<Command> GetById(int id)
        {
            var response = _commander.GetCommandById(id);
            var result = _mapper.Mapper.Map<CommandDTO>(response);
            return Ok(result);
        }

        [HttpPost]
        public ActionResult<CommandDTO> CreateCommand(CommandDTO commandDTO)
        {
            var details = _mapper.Mapper.Map<CommandDTO, Command>(commandDTO);
            var response = _commander.CreateCommand(details);
            var result = _mapper.Mapper.Map<Command, CommandDTO>(response);

            return new OkObjectResult(result);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCommand(int id, [FromBody]  JObject jObject) // 
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
