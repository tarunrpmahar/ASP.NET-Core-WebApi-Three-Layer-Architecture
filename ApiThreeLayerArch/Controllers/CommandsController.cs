using AutoMapper;
using Domain.Contracts.Domain;
using Domain.Models.Models.Domain;
using Domain.Models.Models.PresentationDTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility;

namespace ApiThreeLayerArch.Controllers
{
    [Route("api/commands")]
    [ApiController]
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
        public ActionResult<IEnumerable<Command>> Get()
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
    }
}
