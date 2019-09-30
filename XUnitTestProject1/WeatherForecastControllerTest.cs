using Microsoft.Extensions.Logging;
using Moq;
using OpenTracing;
using System;
using WebApplication1.Controllers;
using Xunit;

namespace XUnitTestProject1
{
    public class WeatherForecastControllerTest
    {
        [Fact]
        public void WeatherForecastController_Get_Success()
        {
            var logger = new Mock<ILogger<WeatherForecastController>>();
            var tracer = new Mock<ITracer>();
            var spanBuilder = new Mock<ISpanBuilder>();
            var scope = new Mock<IScope>();

            spanBuilder
                .Setup(s => s.StartActive(It.IsAny<bool>()))
                .Returns(scope.Object);

            tracer
                .Setup(t => t.BuildSpan(It.IsAny<string>()))
                .Returns(spanBuilder.Object);

            var controller = new WeatherForecastController(logger.Object, tracer.Object);
            var results = controller.Get();

            spanBuilder.Verify(s => s.StartActive(true), Times.Once);

            tracer.Verify(t => t.BuildSpan("WeatherForecast"), Times.Once);

            Assert.True(results != null);
        }
    }
}
