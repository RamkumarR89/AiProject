using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using AutoMapper;
using ReportService.Business.Services;
using ReportService.Domain.DTOs;
using ReportService.Data.Repositories;
using ReportService.Domain.Entities;

namespace ReportService.Tests
{
    public class ChatServiceTests
    {
        private readonly Mock<IChatRepository> _chatRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ChatService _chatService;

        public ChatServiceTests()
        {
            _chatRepositoryMock = new Mock<IChatRepository>();
            _mapperMock = new Mock<IMapper>();
            _chatService = new ChatService(_chatRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task CreateSessionAsync_ShouldReturnSessionDto()
        {
            // Arrange
            var userId = "testuser";
            var session = new ChatSession { Id = 1, UserId = userId, CreatedAt = DateTime.UtcNow, IsActive = true };
            var sessionDto = new ChatSessionDto { Id = 1, UserId = userId, CreatedAt = session.CreatedAt, IsActive = true };
            _chatRepositoryMock.Setup(r => r.CreateSessionAsync(It.IsAny<ChatSession>())).ReturnsAsync(session);
            _mapperMock.Setup(m => m.Map<ChatSessionDto>(session)).Returns(sessionDto);

            // Act
            var result = await _chatService.CreateSessionAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);
        }

        [Fact]
        public async Task GetSessionAsync_ShouldReturnSessionDto()
        {
            // Arrange
            var session = new ChatSession { Id = 1, UserId = "testuser", CreatedAt = DateTime.UtcNow, IsActive = true };
            var sessionDto = new ChatSessionDto { Id = 1, UserId = "testuser", CreatedAt = session.CreatedAt, IsActive = true };
            _chatRepositoryMock.Setup(r => r.GetSessionAsync(1)).ReturnsAsync(session);
            _mapperMock.Setup(m => m.Map<ChatSessionDto>(session)).Returns(sessionDto);

            // Act
            var result = await _chatService.GetSessionAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task ProcessMessageAsync_ShouldReturnChatResponse()
        {
            // Arrange
            var request = new ChatRequest { ChatSessionId = 1, Message = "Hello", UserId = "testuser" };
            var session = new ChatSession { Id = 1, UserId = "testuser", CreatedAt = DateTime.UtcNow, IsActive = true };
            _chatRepositoryMock.Setup(r => r.GetSessionAsync(1)).ReturnsAsync(session);
            _chatRepositoryMock.Setup(r => r.AddMessageAsync(It.IsAny<ChatMessage>())).ReturnsAsync(new ChatMessage());

            // Act
            var result = await _chatService.ProcessMessageAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.ChatSessionId);
        }

        [Fact]
        public async Task GetSessionResponseAsync_ShouldReturnChatResponse()
        {
            // Arrange
            var request = new GetSessionResponseRequest { SessionId = 1, Message = "Response" };
            var session = new ChatSession { Id = 1, UserId = "testuser", CreatedAt = DateTime.UtcNow, IsActive = true };
            _chatRepositoryMock.Setup(r => r.GetSessionAsync(1)).ReturnsAsync(session);
            _chatRepositoryMock.Setup(r => r.AddMessageAsync(It.IsAny<ChatMessage>())).ReturnsAsync(new ChatMessage());

            // Act
            var result = await _chatService.GetSessionResponseAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.ChatSessionId);
        }

        [Fact]
        public async Task GetSessionMessagesAsync_ShouldReturnMessages()
        {
            // Arrange
            var messages = new List<ChatMessage> { new ChatMessage { Id = 1, ChatSessionId = 1, Message = "Hi", Role = "user", CreatedAt = DateTime.UtcNow } };
            var messageDtos = new List<ChatMessageDto> { new ChatMessageDto { Id = 1, ChatSessionId = 1, Message = "Hi", Role = "user", CreatedAt = DateTime.UtcNow } };
            _chatRepositoryMock.Setup(r => r.GetSessionMessagesAsync(1)).ReturnsAsync(messages);
            _mapperMock.Setup(m => m.Map<IEnumerable<ChatMessageDto>>(messages)).Returns(messageDtos);

            // Act
            var result = await _chatService.GetSessionMessagesAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task UpdateChartConfigurationAsync_ShouldReturnChartConfigurationDto()
        {
            // Arrange
            var configDto = new ChartConfigurationDto { Id = 1, ChatSessionId = 1 };
            var config = new ChartConfiguration { Id = 1, ChatSessionId = 1 };
            _chatRepositoryMock.Setup(r => r.UpdateChartConfigurationAsync(configDto)).ReturnsAsync(config);
            _mapperMock.Setup(m => m.Map<ChartConfigurationDto>(config)).Returns(configDto);

            // Act
            var result = await _chatService.UpdateChartConfigurationAsync(configDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task GetUserSessionsAsync_ShouldReturnSessionDtos()
        {
            // Arrange
            var sessions = new List<ChatSession> { new ChatSession { Id = 1, UserId = "testuser", CreatedAt = DateTime.UtcNow, IsActive = true } };
            var sessionDtos = new List<ChatSessionDto> { new ChatSessionDto { Id = 1, UserId = "testuser", CreatedAt = DateTime.UtcNow, IsActive = true } };
            _chatRepositoryMock.Setup(r => r.GetUserSessionsAsync("testuser")).ReturnsAsync(sessions);
            _mapperMock.Setup(m => m.Map<IEnumerable<ChatSessionDto>>(sessions)).Returns(sessionDtos);

            // Act
            var result = await _chatService.GetUserSessionsAsync("testuser");

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }
    }
} 