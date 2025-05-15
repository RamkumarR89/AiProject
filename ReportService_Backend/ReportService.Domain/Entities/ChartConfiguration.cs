using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using ReportService.Domain.Enums;

namespace ReportService.Domain.Entities
{
    public partial class ChartConfiguration
    {
        public long Id { get; set; }
        public long ChatSessionId { get; set; }
        public int Type { get; set; }
        public string? XAxisField { get; set; }
        public string? YAxisField { get; set; }
        public string? SeriesField { get; set; }
        public string? SizeField { get; set; }
        public string? ColorField { get; set; }
        public string? OptionsJson { get; set; }
        public string? FiltersJson { get; set; }

        [NotMapped]
        public ChartTypeEnum ChartType
        {
            get => (ChartTypeEnum)Type;
            set => Type = (int)value;
        }

        [NotMapped]
        public Dictionary<string, object>? Options
        {
            get => string.IsNullOrEmpty(OptionsJson) ? null : JsonSerializer.Deserialize<Dictionary<string, object>>(OptionsJson);
            set => OptionsJson = value == null ? null : JsonSerializer.Serialize(value);
        }

        [NotMapped]
        public Dictionary<string, object>? Filters
        {
            get => string.IsNullOrEmpty(FiltersJson) ? null : JsonSerializer.Deserialize<Dictionary<string, object>>(FiltersJson);
            set => FiltersJson = value == null ? null : JsonSerializer.Serialize(value);
        }

        public virtual ChatSession ChatSession { get; set; } = null!;
    }
} 