﻿using AutoMapper;
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

    }
}