using Domain;
using Domain.Contracts.Repository;
using Domain.Models.Models.Domain;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ApiThreelayerArch.UnitTestingUsingxUnit
{
    public class SqlCommanderTest
    {
        [Fact]
        public void GetAllCommand_Checking_Count()
        {
            //Arrange
            Mock<ICommanderRepository> _commanderRepositoryMock = new Mock<ICommanderRepository>();
            List<Command> commandActual = new List<Command>
            {
                new Command{Id=1, HowTo="Unit test", Line="Hello world", Platform=".net core"},
                new Command{Id=2, HowTo="Integration testing", Line="testing", Platform=".net framework"},
            };

            _commanderRepositoryMock.Setup(x => x.GetAllCommandRepo()).Returns(commandActual);
            var sqlCommander = new SqlCommander(_commanderRepositoryMock.Object);

            //Act
            var commandExpected = sqlCommander.GetAllCommand().ToList();

            //Assert
            Assert.True(commandExpected.Count == commandActual.Count);

        }
    }
}
