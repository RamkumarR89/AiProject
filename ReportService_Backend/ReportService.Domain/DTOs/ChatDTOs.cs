using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ReportService.Domain.DTOs
{
    public class ChatSessionDto
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public bool IsActive { get; set; }
        public List<ChatMessageDto> Messages { get; set; }
        public string? ReportName { get; set; }
        public List<MeasuredValueDto> MeasuredValues { get; set; }
        //public ChartConfigurationDto CurrentChart { get; set; }
    }

    public class ChatMessageDto
    {
        public long Id { get; set; }
        public long ChatSessionId { get; set; }
        public string Message { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public string GeneratedSql { get; set; }
    }

    public class ChatRequest
    {
        public long ChatSessionId { get; set; }
        
        [Required]
        [MinLength(1)]
        public string Message { get; set; }
        
        [Required]
        public string UserId { get; set; }
    }

    public class ChatResponse
    {
        public long ChatSessionId { get; set; }
        public string Message { get; set; }
        public string? GeneratedSql { get; set; }
        public ChartConfigurationDto? ChartConfigurationDetail { get; set; }
    }

    public class CreateSessionRequest
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string? ReportName { get; set; }
    }

    public class SendMessageRequest
    {
        [Required]
        public long SessionId { get; set; }

        [Required]
        [MinLength(1)]
        public string Message { get; set; }
    }

    public class GetSessionResponseRequest
    {
        [Required]
        public long SessionId { get; set; }

        [Required]
        public string Message { get; set; }

        public string GeneratedSql { get; set; }
    }

    public class UpdateGeneratedSqlRequest
    {
        public long SessionId { get; set; }
        public string? GeneratedSql { get; set; }
        public string? Message { get; set; }
    }

    public class UpdateReportNameRequest
    {
        public long SessionId { get; set; }
        public string ReportName { get; set; }
    }

    public class MeasuredValueDto
    {
        public long Id { get; set; }
        public long ChatSessionId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Query { get; set; }
    }
}