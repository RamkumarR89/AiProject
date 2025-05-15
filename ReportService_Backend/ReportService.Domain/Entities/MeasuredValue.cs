using System;

namespace ReportService.Domain.Entities
{
    public class MeasuredValue
    {
        public long Id { get; set; }
        public long ChatSessionId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Query { get; set; }
        public virtual ChatSession ChatSession { get; set; }
    }
} 