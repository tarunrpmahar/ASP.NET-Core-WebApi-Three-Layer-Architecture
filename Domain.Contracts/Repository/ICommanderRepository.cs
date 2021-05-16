using Domain.Models.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts.Repository
{
    public interface ICommanderRepository
    {
        bool SaveChanges();
        IEnumerable<Command> GetAllCommandRepo();
        Command GetCommandByIdRepo(int? id);
        Command CreateCommandRepo(Command cmd);
        Command UpdateCommandRepo(int id, Dictionary<string, object> dataKeyValue);
        //void DeleteCommand(Command cmd);
    }
}
