using Domain.Models.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts.Domain
{
    public interface ICommander
    {
        bool SaveChanges();
        IEnumerable<Command> GetAllCommand();
        Command GetCommandById(int id);
        Command CreateCommand(Command cmd);
        Command UpdateCommand(int id, Dictionary<string, object> dataKeyValue);
        //void DeleteCommand(Command cmd);
    }
}
