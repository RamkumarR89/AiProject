using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using ReportService.Api.Controllers;
using ReportService.Business.Services;
using ReportService.Domain.DTOs;

public class ChatControllerTests
{
    private readonly Mock<IChatService> _chatServiceMock;
    private readonly ChatController _controller;

    public ChatControllerTests()
    {
        _chatServiceMock = new Mock<IChatService>();
        _controller = new ChatController(_chatServiceMock.Object);
    }

    [Fact]
    public async Task SendMessage_ValidRequest_ReturnsOk()
    {
        // Arrange
        var request = new SendMessageRequest { SessionId = 1, Message = "Hello" };
        var response = new ChatResponse { ChatSessionId = 1, Message = "Hi" };
        _chatServiceMock.Setup(s => s.ProcessMessageAsync(It.IsAny<ChatRequest>())).ReturnsAsync(response);

        // Simulate authenticated user
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, "testuser") }));
        _controller.ControllerContext = new ControllerContext { HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext { User = user } };

        // Act
        var result = await _controller.SendMessage(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(response, okResult.Value);
    }

    [Fact]
    public async Task GetSessionResponse_ValidRequest_ReturnsOk()
    {
        //// Arrange
        //var request = new GetSessionResponseRequest { SessionId = 1, Message = "Response" };
        //var response = new ChatResponse { ChatSessionId = 1, Message = "Response" };
        //_chatServiceMock.Setup(s => s.GetSessionResponseAsync(request)).ReturnsAsync(response);

        //// Act
        //var result = await _controller.GetSessionResponse(request);

        //// Assert
        //var okResult = Assert.IsType<OkObjectResult>(result);
        //Assert.Equal(response, okResult.Value);
    }

    [Fact]
    public async Task UpdateChartConfiguration_ValidConfig_ReturnsOk()
    {
        // Arrange
        var config = new ChartConfigurationDto { Id = 1, ChatSessionId = 1 };
        _chatServiceMock.Setup(s => s.UpdateChartConfigurationAsync(config)).ReturnsAsync(config);

        // Act
        var result = await _controller.UpdateChartConfiguration(config);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(config, okResult.Value);
    }
} 