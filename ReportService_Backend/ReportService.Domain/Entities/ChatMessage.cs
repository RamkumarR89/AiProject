using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace ReportService.Domain.Entities
{
    public partial class ChatMessage
    {
        public long Id { get; set; }
        public long ChatSessionId { get; set; }
        public string Message { get; set; } = null!;
        public string Role { get; set; } = null!;  // "user" or "assistant"
        public DateTime CreatedAt { get; set; }
        public string? GeneratedSql { get; set; }

        public virtual ChatSession ChatSession { get; set; } = null!;
    }
} 