using AutoMapper;
using Domain.Contracts.Repository;
using Domain.Models.Models.Domain;
using Entities.Entities;
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

        public CommanderRepository(CommanderDBContext commanderDBContext, CommandProfile mapper)
        {
            _commanderDBContext = commanderDBContext;
            _mapper = mapper;
        }

        public Command CreateCommandRepo(Command cmd)
        {
            var details = _mapper.Mapper.Map<Command, TblCommand>(cmd);
            var addDetails = _commanderDBContext.Add(details).Entity;
            _commanderDBContext.SaveChanges();

            var response = _mapper.Mapper.Map<TblCommand, Command>(addDetails);
            return response;
        }

        public IEnumerable<Command> GetAllCommandRepo()
        {
            var data = _commanderDBContext.TblCommands.ToList();

            var responseData = _mapper.Mapper.Map<IEnumerable<Command>>(data);

            return responseData;
           //return _context.tblCommands.ToList();
        }

        public Command GetCommandByIdRepo(int id)
        {
            var data = _commanderDBContext.TblCommands.FirstOrDefault(x => x.Id == id);
            var responseData = _mapper.Mapper.Map<Command>(data);
            return responseData;
        }

        public bool SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}
