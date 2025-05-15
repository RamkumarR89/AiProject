// IChatService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ReportService.Domain.DTOs;
using ReportService.Domain.Entities;

namespace ReportService.Business.Services
{
    public interface IChatService
    {
        Task<ChatSessionDto> CreateSessionAsync(string userId, string reportName);
        Task<ChatSessionDto> GetSessionAsync(long sessionId);
        Task<ChatResponse> ProcessMessageAsync(ChatRequest request);
        Task<ChatResponse> GetSessionResponseAsync(GetSessionResponseRequest request);
        Task<IEnumerable<ChatMessageDto>> GetSessionMessagesAsync(long sessionId);
        Task<bool> UpdateLatestGeneratedSqlAsync(long sessionId, string generatedSql);
        Task<ChartConfigurationDto> UpdateChartConfigurationAsync(ChartConfigurationDto config);
        Task<IEnumerable<ChatSessionDto>> GetUserSessionsAsync(string userId);
        Task<SessionWorkflowStatusDto?> GetNextStepAsync(long chatSessionId);
        Task<bool> UpdateReportNameAsync(long sessionId, string reportName);
        Task<bool> UpdateMessageAndGeneratedSqlAsync(long sessionId, string? message, string? generatedSql);
        Task<IEnumerable<ChartType>> GetChartTypesAsync();
    }
}