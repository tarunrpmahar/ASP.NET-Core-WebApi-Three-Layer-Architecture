using Xunit;
using System;
using Moq;
using Domain.Contracts.Domain;
using Utility;
using ApiThreeLayerArch.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace ApiThreelayerArch.UnitTestingUsingxUnit
{
    public class CommandsUnitTest
    {
        [Fact]
        public void Get_when_Called_return_ok_response()
        {
            Mock<ICommander> _commander = new Mock<ICommander>();
            CommandProfile _mapper = new CommandProfile();

            var controller = new CommandsController(_commander.Object, _mapper);

            var response = controller.Get();

            Assert.NotNull(response);

            Assert.IsType<OkObjectResult>(response);
        }
    }
}
