using AutoMapper;
using Domain.Contracts.Repository;
using Domain.Models.Models.Domain;
using Entities.Entities;
using Microsoft.ApplicationInsights;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace DataAccess
{
    public class CommanderRepository : ICommanderRepository
    {
        private readonly CommanderDBContext _commanderDBContext;
        private readonly CommandProfile _mapper;
        private readonly ILogger<CommanderRepository> _logger;
        private readonly TelemetryClient _telemetryClient;

        public CommanderRepository(CommanderDBContext commanderDBContext, CommandProfile mapper, ILogger<CommanderRepository> logger, TelemetryClient telemetryClient)
        {
            _commanderDBContext = commanderDBContext;
            _mapper = mapper;
            _logger = logger;
            _telemetryClient = telemetryClient;
        }

        public Command CreateCommandRepo(Command cmd)
        {
            _telemetryClient.TrackEvent("Logging - in CreateCommandRepo (Repository) | telemetry");
            _logger.LogInformation("Logging - in CreateCommandRepo (Repository) | serilog");
            var details = _mapper.Mapper.Map<Command, TblCommand>(cmd);
            var addDetails = _commanderDBContext.Add(details).Entity;

            _commanderDBContext.SaveChanges();

            var response = _mapper.Mapper.Map<TblCommand, Command>(addDetails);
            return response;
        }

        public IEnumerable<Command> GetAllCommandRepo()
        {
            _telemetryClient.TrackEvent("Logging - in GetAllCommandRepo (Repository) | telemetry");
            _logger.LogInformation("Logging - in GetAllCommandRepo (Repository) | serilog");

            var data = _commanderDBContext.TblCommands.ToList();

            var responseData = _mapper.Mapper.Map<IEnumerable<Command>>(data);

            return responseData;
           //return _context.tblCommands.ToList();
        }

        public Command GetCommandByIdRepo(int? id)
        {
            _telemetryClient.TrackEvent("Logging - in GetCommandByIdRepo (Repository) | telemetry");
            _logger.LogInformation("Logging - in GetCommandByIdRepo (Repository) | serilog");

            var data = _commanderDBContext.TblCommands.FirstOrDefault(x => x.Id == id);
            var responseData = _mapper.Mapper.Map<Command>(data);
            return responseData;
        }

        public bool SaveChanges()
        {
            return (_commanderDBContext.SaveChanges() >= 0);
        }

        public Command UpdateCommandRepo(int id, Dictionary<string, object> dataKeyValue)
        {
            _telemetryClient.TrackEvent("Logging - in UpdateCommandRepo (Repository) | telemetry");
            _logger.LogInformation("Logging - in UpdateCommandRepo (Repository) | serilog");

            var orignalDBDataByMethod = _commanderDBContext.TblCommands.AsNoTracking().SingleOrDefault(b => b.Id == id);
            var dataFormDB = JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(orignalDBDataByMethod));
            foreach (var orignaldata in dataFormDB.Keys.ToList())
            {
                foreach (var updatedata in dataKeyValue.Keys.ToList())
                {

                    if (orignaldata.ToLower() == updatedata.ToLower())
                    {
                        if (dataFormDB[orignaldata] == null)
                        {
                            dataFormDB[orignaldata] = dataKeyValue[updatedata];
                        }
                        else if (dataFormDB[orignaldata] != null)
                        {
                            if (dataKeyValue[updatedata] == null)
                            {
                                dataFormDB[orignaldata] = null;
                            }
                            else if (dataKeyValue[updatedata] != null)
                            {
                                dataFormDB[orignaldata] = dataKeyValue[updatedata];
                            }

                        }
                        break;
                    }
                }
            }

            var entityUpdate = JsonConvert.DeserializeObject<TblCommand>(JsonConvert.SerializeObject(dataFormDB));
            _commanderDBContext.Entry(entityUpdate).State = EntityState.Modified;
            _commanderDBContext.SaveChangesAsync();
            return _mapper.Mapper.Map<TblCommand, Command>(entityUpdate);


            //var data = _commanderDBContext.TblCommands.FirstOrDefault(x => x.Id == id);
            //var details = _mapper.Mapper.Map<Command, TblCommand>(cmd);
            //var addDetails = _commanderDBContext.Add(details).Entity;

            //throw new NotImplementedException();

            //var commandModelFromRepo = _repo.GetCommandById(id);
            //if (commandModelFromRepo == null)
            //{
            //    return NotFound();
            //}

            //_mapper.Map(commandUpdateDto, commandModelFromRepo);

            //_repo.UpdateCommand(commandModelFromRepo);
            //_repo.SaveChanges();

            //return NoContent();
        }
    }
}
