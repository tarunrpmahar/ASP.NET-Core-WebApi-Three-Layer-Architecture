using AutoMapper;
using Domain.Contracts.Domain;
using Domain.Contracts.Repository;
using Domain.Models.Models.Domain;
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
        public IEnumerable<Command> GetAllCommand()
        {
            return _commanderRepository.GetAllCommand();
        }

        public bool SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}
