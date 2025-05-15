using System.Threading.Tasks;

namespace ReportService.Business.Services.OpenAI
{

    public interface ISqlGenerationService
    {
        Task<string> GenerateSqlQueryAsync(string userPrompt, string databaseSchema);
        Task<string> RefineSqlQueryAsync(string currentQuery, string userFeedback, string databaseSchema);
        Task<bool> ValidateSqlQueryAsync(string query, string databaseSchema);
        Task<string> ExplainQueryAsync(string query);
    }
} 