// ChatRepository.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ReportService.Data.Context;
using ReportService.Domain.DTOs;
using ReportService.Domain.Entities;
using ReportService.Domain.Enums;

namespace ReportService.Data.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly ReportServiceContext _context;

        public ChatRepository(ReportServiceContext context)
        {
            _context = context;
        }

        public async Task<ChatSession> CreateSessionAsync(ChatSession session)
        {
            await _context.ChatSessions.AddAsync(session);
            await _context.SaveChangesAsync();
            return session;
        }

        public async Task<SessionWorkflow> CreateSessionWorkflowAsync(long sessionId , bool hasReportName)
        {
            var workflow = new SessionWorkflow
            {
                ChatSessionId = sessionId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                HasReportName = hasReportName,
                HasMessageQuery = false,
                HasChartConfigured = false,
                IsPublished = false
            };

            await _context.SessionWorkflows.AddAsync(workflow);
            await _context.SaveChangesAsync();
            return workflow;
        }

        public async Task<ChatSession> GetSessionAsync(long sessionId)
        {
            return await _context.ChatSessions
                .Include(s => s.ChatMessages)
                .Where(s => s.ChatMessages.Any())
                .FirstOrDefaultAsync(s => s.Id == sessionId);
        }

        public async Task<IEnumerable<ChatSession>> GetUserSessionsAsync(string userId)
        {
            return await _context.ChatSessions
                .Include(s => s.ChatMessages)
                .Where(s => s.ChatMessages.Any())
                .Where(s => s.UserId == userId && s.IsActive)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }

        public async Task<ChatMessage> AddMessageAsync(ChatMessage message)
        {
            try
            {
                await _context.ChatMessages.AddAsync(message);
                await _context.SaveChangesAsync();
                return message;
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        public async Task<IEnumerable<ChatMessage>> GetSessionMessagesAsync(long sessionId)
        {
            return await _context.ChatMessages
                .Where(m => m.ChatSessionId == sessionId)
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool> UpdateLatestGeneratedSqlAsync(long sessionId, string generatedSql)
        {
            var message = await _context.ChatMessages
                .Where(m => m.ChatSessionId == sessionId)
                .OrderByDescending(m => m.CreatedAt)
                .Select(m => new { m.Id, m.GeneratedSql }) // Load only needed fields
                .FirstOrDefaultAsync();

            if (message == null || message.GeneratedSql == generatedSql)
                return false;

            var updatedMessage = new ChatMessage { Id = message.Id, GeneratedSql = generatedSql };
            _context.ChatMessages.Attach(updatedMessage);
            _context.Entry(updatedMessage).Property(m => m.GeneratedSql).IsModified = true;

            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<ChartConfiguration> UpdateChartConfigurationAsyncold(ChartConfigurationDto config)
        {
            var existingConfig = await _context.ChartConfigurations
                .FirstOrDefaultAsync(c => c.Id == config.Id);

            if (existingConfig == null)
            {
                existingConfig = new ChartConfiguration
                {
                    ChatSessionId = config.ChatSessionId
                };
                await _context.ChartConfigurations.AddAsync(existingConfig);
            }

            existingConfig.Type = (int)config.Type;
            existingConfig.XAxisField = config.XaxisField;
            existingConfig.YAxisField = config.YaxisField;
            existingConfig.SeriesField = config.SeriesField;
            existingConfig.SizeField = config.SizeField;
            existingConfig.ColorField = config.ColorField;
            if (config.Options != null)
                existingConfig.OptionsJson = JsonSerializer.Serialize(config.Options);
            if (config.Filters != null)
                existingConfig.FiltersJson = JsonSerializer.Serialize(config.Filters);

            await _context.SaveChangesAsync();
            return existingConfig;
        }

        public async Task<ChartConfiguration> UpdateChartConfigurationAsync(ChartConfigurationDto config)
        {
            var existingConfig = await _context.ChartConfigurations
                .FirstOrDefaultAsync(c => c.Id == config.Id);

            bool isNew = false;
            if (existingConfig == null)
            {
                existingConfig = new ChartConfiguration
                {
                    ChatSessionId = config.ChatSessionId
                };
                await _context.ChartConfigurations.AddAsync(existingConfig);
                isNew = true;
            }

            existingConfig.Type = (int)config.Type;
            if (!string.IsNullOrEmpty(config.XaxisField)) existingConfig.XAxisField = config.XaxisField;
            if (!string.IsNullOrEmpty(config.YaxisField)) existingConfig.YAxisField = config.YaxisField;
            if (!string.IsNullOrEmpty(config.SeriesField)) existingConfig.SeriesField = config.SeriesField;
            if (!string.IsNullOrEmpty(config.SizeField)) existingConfig.SizeField = config.SizeField;
            if (!string.IsNullOrEmpty(config.ColorField)) existingConfig.ColorField = config.ColorField;
            if (config.Options != null) existingConfig.OptionsJson = JsonSerializer.Serialize(config.Options);
            if (config.Filters != null) existingConfig.FiltersJson = JsonSerializer.Serialize(config.Filters);

            if (!isNew)
            {
                _context.Entry(existingConfig).State = EntityState.Modified;
            }
            // For new entities, EF will track as Added automatically

            await _context.SaveChangesAsync();
            return existingConfig;
        }

        //public async Task<ChartConfiguration> UpdateChartConfigurationAsync(ChartConfigurationDto config)
        //{
        //    var existingConfig = await _context.ChartConfigurations
        //        .FirstOrDefaultAsync(c => c.Id == config.Id);

        //    if (existingConfig == null)
        //    {
        //        existingConfig = new ChartConfiguration
        //        {
        //            ChatSessionId = config.ChatSessionId
        //        };
        //        await _context.ChartConfigurations.AddAsync(existingConfig);
        //    }

        //    existingConfig.Type = (int)config.Type;
        //    if (!string.IsNullOrEmpty(config.XaxisField)) existingConfig.XAxisField = config.XaxisField;
        //    if (!string.IsNullOrEmpty(config.YaxisField)) existingConfig.YAxisField = config.YaxisField;
        //    if (!string.IsNullOrEmpty(config.SeriesField)) existingConfig.SeriesField = config.SeriesField;
        //    if (!string.IsNullOrEmpty(config.SizeField)) existingConfig.SizeField = config.SizeField;
        //    if (!string.IsNullOrEmpty(config.ColorField)) existingConfig.ColorField = config.ColorField;
        //    if (config.Options != null) existingConfig.OptionsJson = JsonSerializer.Serialize(config.Options);
        //    if (config.Filters != null) existingConfig.FiltersJson = JsonSerializer.Serialize(config.Filters);

        //    _context.Entry(existingConfig).State = EntityState.Modified;
        //    await _context.SaveChangesAsync();
        //    return existingConfig;
        //}


        public async Task<bool> SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<SessionWorkflowStatusDto?> GetNextStepAsync(long chatSessionId)
        {
            var workflow = await _context.SessionWorkflows
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ChatSessionId == chatSessionId);

            if (workflow == null)
                throw new Exception("SessionWorkflow not found.");

            if (!workflow.HasReportName)
                return new SessionWorkflowStatusDto(SessionWorkflowStep.ReportName, SessionWorkflowLabels.GetLabel(SessionWorkflowStep.ReportName), false);

            if (!workflow.HasMessageQuery)
                return new SessionWorkflowStatusDto(SessionWorkflowStep.MessageAndQuery, SessionWorkflowLabels.GetLabel(SessionWorkflowStep.MessageAndQuery), false);

            if (!workflow.HasChartConfigured)
                return new SessionWorkflowStatusDto(SessionWorkflowStep.ChartConfigured, SessionWorkflowLabels.GetLabel(SessionWorkflowStep.ChartConfigured), false);

            if (!workflow.IsPublished)
                return new SessionWorkflowStatusDto(SessionWorkflowStep.Published, SessionWorkflowLabels.GetLabel(SessionWorkflowStep.Published), false);

            return null; // all done
        }

        public async Task<bool> UpdateReportNameAsync(long sessionId, string reportName)
        {
            var session = await _context.ChatSessions.FirstOrDefaultAsync(s => s.Id == sessionId);
            if (session == null) return false;
            session.ReportName = reportName;
            _context.Entry(session).State = EntityState.Modified;
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> UpdateMessageAndGeneratedSqlAsync(long sessionId, string? message, string? generatedSql)
        {
            var chatMessage = await _context.ChatMessages
                .Where(m => m.ChatSessionId == sessionId)
                .OrderByDescending(m => m.CreatedAt)
                .FirstOrDefaultAsync();

            if (chatMessage == null)
                return false;

            if (message != null)
                chatMessage.Message = message;
            if (generatedSql != null)
                chatMessage.GeneratedSql = generatedSql;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ChartType>> GetAllChartTypesAsync()
        {
            return await _context.ChartTypes
                .OrderBy(ct => ct.Name)
                .ToListAsync();
        }
    }
}