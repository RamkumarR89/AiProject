using System;
using System.Collections.Generic;

namespace ReportService.Domain.Entities
{
    public partial class ChatSession
    {
        public long Id { get; set; }
        public string UserId { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public bool IsActive { get; set; }
        public string? ReportName { get; set; }

        public virtual ICollection<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();
        public virtual ICollection<MeasuredValue> MeasuredValues { get; set; } = new List<MeasuredValue>();
    }
} 