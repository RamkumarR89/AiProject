// ChatService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ReportService.Business.Services;
using ReportService.Domain.DTOs;
using ReportService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using ReportService.Data.Repositories;

namespace ReportService.Business.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly IMapper _mapper;

        public ChatService(IChatRepository chatRepository, IMapper mapper)
        {
            _chatRepository = chatRepository;
            _mapper = mapper;
        }

        public async Task<ChatSessionDto> CreateSessionAsync(string userId, string reportName)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("User ID cannot be null or empty", nameof(userId));
            }

            if (string.IsNullOrWhiteSpace(reportName))
            {
                throw new ArgumentException("ReportName cannot be null or empty", nameof(reportName));
            }

            var session = new ChatSession
            {
                UserId = userId,
                ReportName = reportName,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            var createdSession = await _chatRepository.CreateSessionAsync(session);

            if (createdSession == null)
            {
                throw new InvalidOperationException("Failed to create chat session.");
            }

            await _chatRepository.CreateSessionWorkflowAsync(createdSession.Id, reportName != null);

            return _mapper.Map<ChatSessionDto>(createdSession);
        }

        public async Task<ChatSessionDto> GetSessionAsync(long sessionId)
        {
            var session = await _chatRepository.GetSessionAsync(sessionId);
            return _mapper.Map<ChatSessionDto>(session);
        }

        public async Task<ChatResponse> ProcessMessageAsync(ChatRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            // Create user message  
            var userMessage = new ChatMessage
            {
                ChatSessionId = request.ChatSessionId,
                Message = request.Message,
                Role = "user",
                GeneratedSql = null,
                CreatedAt = DateTime.UtcNow
            };
            await _chatRepository.AddMessageAsync(userMessage);

            return new ChatResponse
            {
                ChatSessionId = request.ChatSessionId,
                Message = request.Message,
                GeneratedSql = null,
                ChartConfigurationDetail = null,
            };
        }

        public async Task<ChatResponse> GetSessionResponseAsync(GetSessionResponseRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var session = await _chatRepository.GetSessionAsync(request.SessionId);
            if (session == null)
            {
                throw new ArgumentException($"Session with ID {request.SessionId} not found");
            }

            // Map the ChatMessage entity to the correct namespace/type expected by the repository
            var assistantMessage = new ChatMessage
            {
                ChatSessionId = request.SessionId,
                Message = request.Message,
                Role = "assistant",
                CreatedAt = DateTime.UtcNow,
                GeneratedSql = request.GeneratedSql,
            };
            await _chatRepository.AddMessageAsync(assistantMessage);

            return new ChatResponse
            {
                ChatSessionId = request.SessionId,
                Message = request.Message,
                GeneratedSql = request.GeneratedSql,
            };
        }

        public async Task<IEnumerable<ChatMessageDto>> GetSessionMessagesAsync(long sessionId)
        {
            var messages = await _chatRepository.GetSessionMessagesAsync(sessionId);
            return _mapper.Map<IEnumerable<ChatMessageDto>>(messages);
        }

        public async Task<ChartConfigurationDto> UpdateChartConfigurationAsync(ChartConfigurationDto config)
        {
            var updatedConfig = await _chatRepository.UpdateChartConfigurationAsync(config);
            return _mapper.Map<ChartConfigurationDto>(updatedConfig);
        }

        public async Task<IEnumerable<ChatSessionDto>> GetUserSessionsAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("User ID cannot be null or empty", nameof(userId));
            }

            var sessions = await _chatRepository.GetUserSessionsAsync(userId);
            return _mapper.Map<IEnumerable<ChatSessionDto>>(sessions);
        }

        public async Task<bool> UpdateLatestGeneratedSqlAsync(long sessionId, string generatedSql)
        {
            var updateResult = await _chatRepository.UpdateLatestGeneratedSqlAsync(sessionId, generatedSql);
            return updateResult;
        }

        public async Task<SessionWorkflowStatusDto?> GetNextStepAsync(long chatSessionId)
        {
            return await _chatRepository.GetNextStepAsync(chatSessionId);
        }

        public async Task<bool> UpdateReportNameAsync(long sessionId, string reportName)
        {
            return await _chatRepository.UpdateReportNameAsync(sessionId, reportName);
        }

        public async Task<bool> UpdateMessageAndGeneratedSqlAsync(long sessionId, string? message, string? generatedSql)
        {
            return await _chatRepository.UpdateMessageAndGeneratedSqlAsync(sessionId, message, generatedSql);
        }

        public async Task<IEnumerable<ChartType>> GetChartTypesAsync()
        {
            return await _chatRepository.GetAllChartTypesAsync();
        }
    }
}