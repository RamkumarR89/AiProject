using System;
using System.Collections.Generic;
using ReportService.Domain.Enums;

namespace ReportService.Domain.DTOs
{
    public class ChartConfigurationDto
    {
        public long Id { get; set; }
        public long ChatSessionId { get; set; }
        public ChartTypeEnum Type { get; set; }
        public string? XaxisField { get; set; }
        public string? YaxisField { get; set; }
        public string? SeriesField { get; set; }
        public string? SizeField { get; set; }
        public string? ColorField { get; set; }
        public Dictionary<string, object>? Options { get; set; }
        public Dictionary<string, object>? Filters { get; set; }
    }
} 