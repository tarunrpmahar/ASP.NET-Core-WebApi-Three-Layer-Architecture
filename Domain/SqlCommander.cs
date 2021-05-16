using AutoMapper;
using Domain.Contracts.Domain;
using Domain.Contracts.Repository;
using Domain.Models.Models.Domain;
using Domain.Models.Models.PresentationDTO;
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

        public SqlCommander(ICommanderRepository commanderRepository)
        {
            _commanderRepository = commanderRepository;
        }

        public Command CreateCommand(Command command)
        {
            return _commanderRepository.CreateCommandRepo(command);
        }

        public IEnumerable<Command> GetAllCommand()
        {
            return _commanderRepository.GetAllCommandRepo();
        }

        public Command GetCommandById(int? id)
        {
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
            return _commanderRepository.UpdateCommandRepo(id, dataKeyValue);
        }
    }
}
