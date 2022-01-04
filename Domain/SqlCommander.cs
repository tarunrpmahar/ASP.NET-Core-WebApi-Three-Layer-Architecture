using AutoMapper;
using Domain.Contracts.Domain;
using Domain.Contracts.Repository;
using Domain.Models.Models.Domain;
using Domain.Models.Models.PresentationDTO;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class SqlCommander : ICommander
    {
        private readonly ICommanderRepository _commanderRepository;
        private readonly ILogger<SqlCommander> _logger;
        private readonly TelemetryClient _telemetryClient;
        public SqlCommander(ICommanderRepository commanderRepository, ILogger<SqlCommander> logger, TelemetryClient telemetryClient)
        {
            _commanderRepository = commanderRepository;
            _logger = logger;
            _telemetryClient = telemetryClient;
        }

        public Command CreateCommand(Command command)
        {
            _telemetryClient.TrackEvent("Logging - in CreateCommand (Business) | telemetry");
            _logger.LogInformation("Logging - in CreateCommand (Business) | serilog");
            return _commanderRepository.CreateCommandRepo(command);
        }

        public IEnumerable<Command> GetAllCommand()
        {
            _telemetryClient.TrackEvent("Logging - in GetAllCommand (Business) | telemetry");
            _logger.LogInformation("Logging - in GetAllCommand (Business) | serilog");
            return _commanderRepository.GetAllCommandRepo();
        }

        public Command GetCommandById(int? id)
        {
            _telemetryClient.TrackEvent("Logging - in GetCommandById (Business) | telemetry");
            _logger.LogInformation("Logging - in GetCommandById (Business) | serilog");
            return _commanderRepository.GetCommandByIdRepo(id);
        }

        public bool SaveChanges()
        {
            return _commanderRepository.SaveChanges(); /*(_context.SaveChanges() >= 0);*/
        }

        public Command UpdateCommand(int id, Dictionary<string, object> dataKeyValue)
        {
            if (id < 0)
            {
                throw new ArgumentNullException(nameof(id), $"{nameof(id)} is needed to perform updation");
            }
            _telemetryClient.TrackEvent("Logging - in UpdateCommand (Business) | telemetry");
            _logger.LogInformation("Logging - in UpdateCommand (Business) | serilog");
            return _commanderRepository.UpdateCommandRepo(id, dataKeyValue);
        }
    }
}
