using Xunit;
using System;
using Moq;
using Domain.Contracts.Domain;
using Utility;
using ApiThreeLayerArch.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Domain.Models.Models.Domain;
using Domain.Models.Models.PresentationDTO;
using System.ComponentModel.DataAnnotations;

namespace ApiThreelayerArch.UnitTestingUsingxUnit
{
    public class CommandsUnitTest
    {
        private readonly Mock<ICommander> _commanderMock;
        private readonly CommandProfile _mapperMock;
        public CommandsUnitTest()
        {
            _commanderMock = new Mock<ICommander>();
            _mapperMock = new CommandProfile();
        }

        public static IList<ValidationResult> Validate(object model)
        {
            var results = new List<ValidationResult>();
            var validationContext = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, validationContext, results, true);
            if (model is IValidatableObject) (model as IValidatableObject).Validate(validationContext);
            return results;
        }

        [Fact]
        public void Get_Return_OkResult()
        {
            //Arrange
            var commandResponse = new List<Command>
            {
                new Command{Id=1, HowTo="Unit test", Line="Hello world", Platform=".net core"},
                new Command{Id=2, HowTo="Integration testing", Line="testing", Platform=".net framework"},
                new Command{Id=3, HowTo="web api", Line="create webapi", Platform="asp.net mvc"},
                new Command{Id=4, HowTo="razor page", Line="web ui", Platform="asp.net framework"}
            };
            _commanderMock.Setup(x => x.GetAllCommand()).Returns(commandResponse);

            var controller = new CommandsController(_commanderMock.Object, _mapperMock);

            //Act
            var response = controller.Get();

            //Assert
            //Assert.NotNull(response);
            Assert.IsType<OkObjectResult>(response);
        }

        [Fact]
        public void Get_Return_NotFoundResult()
        {
            //Arrange
            var commandResponse = new List<Command>
            {
                new Command{Id=1, HowTo="Unit test", Line="Hello world", Platform=".net core"},
                new Command{Id=2, HowTo="Integration testing", Line="testing", Platform=".net framework"},
                new Command{Id=3, HowTo="web api", Line="create webapi", Platform="asp.net mvc"},
                new Command{Id=4, HowTo="razor page", Line="web ui", Platform="asp.net framework"}
            };
            _commanderMock.Setup(x => x.GetAllCommand()).Returns(() => null);
            var controller = new CommandsController(_commanderMock.Object, _mapperMock);
            
            //Act
            var response = controller.Get();

            //Assert
            Assert.IsType<NotFoundResult>(response);
        }

        [Fact]
        public void GetById_Return_OkResult()
        {
            //Arrange
            var getId = 2;
            var commandResponse = new Command { Id = 2, HowTo = "Integration testing", Line = "testing", Platform = ".net framework" };
            _commanderMock.Setup(x => x.GetCommandById(getId)).Returns(commandResponse);

            var controller = new CommandsController(_commanderMock.Object, _mapperMock);

            //Act
            var response = controller.GetById(getId);

            //Assert
            Assert.IsType<OkObjectResult>(response);
        }

        [Fact]
        public void GetById_Return_NotFoundResult()
        {
            //Arrange
            var getId = 1;

            var commandResponse = new Command { Id = 2, HowTo = "Integration testing", Line = "testing", Platform = ".net framework" };
            _commanderMock.Setup(x => x.GetCommandById(getId)).Returns(() => null);

            var controller = new CommandsController(_commanderMock.Object, _mapperMock);

            //Act
            var response = controller.GetById(getId);

            //Assert
            Assert.IsType<NotFoundResult>(response);
        }

        [Fact]
        public void GetById_Return_BadRequestResult()
        {
            //Arrange
            int? getId = null;

            var commandResponse = new Command { Id = 2, HowTo = "Integration testing", Line = "testing", Platform = ".net framework" };
            _commanderMock.Setup(x => x.GetCommandById(getId)).Returns(() => null);

            var controller = new CommandsController(_commanderMock.Object, _mapperMock);

            //Act
            var response = controller.GetById(getId);

            //Assert
            Assert.IsType<BadRequestResult>(response);
        }

        //[Fact]
        /*public void GetById_matchResult()
        {
            //Arrange
            int? getId = 2;

            var commandResponse = new Command { Id = 2, HowTo = "Integration testing", Line = "testing", Platform = ".net framework" };
            _commanderMock.Setup(x => x.GetCommandById(getId)).Returns(commandResponse);

            var controller = new CommandsController(_commanderMock.Object, _mapperMock);

            //Act
            var response = controller.GetById(getId);

            //Assert
            
        }*/

        [Fact]
        public void CreateCommand_Return_OkResult()
        {
            //Arrange
            var commandResponse = new List<Command>
            {
                new Command{Id=1, HowTo="web api Unit", Line="create webapi testing", Platform="asp.net core"},
                new Command{Id=2, HowTo="Integration testing", Line="testing", Platform=".net framework"},
                new Command{Id=3, HowTo="web api", Line="create webapi", Platform="asp.net mvc"},
                new Command{Id=4, HowTo="razor page", Line="web ui", Platform="asp.net framework"}
            };

            var commandDto = new CommandDTO { Id = 5, HowTo = "Add Details", Line = "checking", Platform = "asp.net core" };
            var command = new Command { Id = 5, HowTo = "Add Details", Line = "checking", Platform = "asp.net core" };

            _commanderMock.Setup(x => x.CreateCommand(It.IsAny<Command>())).Returns(command)
                .Callback((Command cmd) =>
                {
                    cmd.Id = commandResponse.Count + 1;
                    commandResponse.Add(cmd);
                }).Verifiable();

            var controller = new CommandsController(_commanderMock.Object, _mapperMock);

            //Act
            var response = controller.CreateCommand(commandDto);

            //Assert
            Assert.IsType<OkObjectResult>(response);
        }

        [Fact]
        public void CreateCommand_InvalidData_Return_BadRequestResult()
        {
            //Arrange
            var commandDto = new CommandDTO { Id = 1, HowTo = "Add Details", Line = "checking", Platform = "asp.net core" };
            var command = new Command { Id = 1, HowTo = "Test HowTo can't be more than twenty characteres", Line = "checking", Platform = "asp.net core" };

            _commanderMock.Setup(x => x.CreateCommand(command)).Returns(command);

            var controller = new CommandsController(_commanderMock.Object, _mapperMock);

            //Act
            var response = controller.CreateCommand(commandDto);
            //var response = controller.CreateCommand(commandDto);

            //Assert
            Assert.IsType<BadRequestResult>(response);
        }

        //[Fact]

        /*public void Validating_Model()
        {
            //Arrange
            Mock<ICommander> _commanderMock = new Mock<ICommander>();
            CommandProfile _mapperMock = new CommandProfile();

            //var commandDto = new CommandDTO { Id = 1, HowTo = "Add Details", Line = "checking", Platform = "asp.net core" };
            var model = new Command { Id = 1, HowTo = "Test HowTo can't be more than twenty characteres", Line = "checking", Platform = "asp.net core" };

            //_commanderMock.Setup(x => x.CreateCommand(command)).Returns(command);

            //var controller = new CommandsController(_commanderMock.Object, _mapperMock);

            //Act
            var result = CommandsUnitTest.Validate(model);
            //var response = controller.CreateCommand(commandDto);

            //Assert
            Assert.Equal(1, result.Count);
        }*/

    }
}
