using System;
using System.Collections.Generic;

namespace ReportService.Domain.DTOs
{
    public class QueryResultDto
    {
        public string QueryText { get; set; }
        public object Results { get; set; }
        public string Error { get; set; }
    }

    public class QueryValidationDto
    {
        public bool IsValid { get; set; }
        public string Error { get; set; }
    }

    public class QueryMetadataDto
    {
        public string[] ColumnNames { get; set; }
        public string[] TableNames { get; set; }
        public string DatabaseName { get; set; }
    }
} 