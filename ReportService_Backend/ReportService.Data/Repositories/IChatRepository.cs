// IChatRepository.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ReportService.Domain.DTOs;
using ReportService.Domain.Entities;

namespace ReportService.Data.Repositories
{
    public interface IChatRepository
    {
        Task<ChatSession> CreateSessionAsync(ChatSession session);
        Task<ChatSession> GetSessionAsync(long sessionId);
        Task<IEnumerable<ChatSession>> GetUserSessionsAsync(string userId);
        Task<ChatMessage> AddMessageAsync(ChatMessage message);
        Task<IEnumerable<ChatMessage>> GetSessionMessagesAsync(long sessionId);
        Task<bool> UpdateLatestGeneratedSqlAsync(long sessionId, string generatedSql);
        Task<ChartConfiguration> UpdateChartConfigurationAsync(ChartConfigurationDto config);
        Task<bool> SaveChangesAsync();

        Task<SessionWorkflow> CreateSessionWorkflowAsync(long sessionId,bool hasReportName);
        Task<SessionWorkflowStatusDto?> GetNextStepAsync(long chatSessionId);

        Task<bool> UpdateReportNameAsync(long sessionId, string reportName);

        Task<bool> UpdateMessageAndGeneratedSqlAsync(long sessionId, string? message, string? generatedSql);
        Task<IEnumerable<ChartType>> GetAllChartTypesAsync();
    }
}