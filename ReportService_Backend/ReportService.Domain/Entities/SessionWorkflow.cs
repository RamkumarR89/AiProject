using System;

namespace ReportService.Domain.Entities
{
    public class SessionWorkflow
    {
        public long Id { get; set; }
        public long ChatSessionId { get; set; }

        // Flags to track the workflow status
        public bool HasReportName { get; set; }
        public bool HasMessageQuery { get; set; } 
        public bool HasChartConfigured { get; set; }
        public bool IsPublished { get; set; }

        // Timestamps for creation and updates
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation property for the related ChatSession
        public ChatSession ChatSession { get; set; }
    }
}
