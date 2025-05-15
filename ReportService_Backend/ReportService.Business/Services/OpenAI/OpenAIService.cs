using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels;
using OpenAI.Managers;
using OpenAI.Interfaces;

namespace ReportService.Business.Services.OpenAI
{
    public class OpenAIService : ISqlGenerationService
    {
        private readonly IOpenAIService _openAI;
        private readonly string _model = Models.Gpt_4;

        public OpenAIService(IConfiguration configuration)
        {
            var apiKey = configuration["OpenAI:ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
                throw new ArgumentNullException(nameof(apiKey), "OpenAI API key is not configured");

            _openAI = new global::OpenAI.Managers.OpenAIService(new global::OpenAI.OpenAiOptions { ApiKey = apiKey });
        }

        public async Task<string> GenerateSqlQueryAsync(string userPrompt, string databaseSchema)
        {
            var messages = new List<ChatMessage>
            {
                ChatMessage.FromSystem(@"You are a SQL query generator. 
                    Generate SQL queries based on natural language prompts while following these rules:
                    1. Only use tables and columns that exist in the provided schema
                    2. Use proper SQL syntax and best practices
                    3. Include appropriate JOINs when needed
                    4. Add comments to explain complex parts
                    5. Ensure the query is optimized for performance"),
                ChatMessage.FromUser($"Database Schema:\n{databaseSchema}"),
                ChatMessage.FromUser($"Generate a SQL query for: {userPrompt}")
            };

            var chatRequest = new ChatCompletionCreateRequest
            {
                Model = _model,
                Messages = messages
            };

            var response = await _openAI.ChatCompletion.CreateCompletion(chatRequest);
            return response?.Choices[0]?.Message?.Content ?? string.Empty;
        }

        public async Task<string> RefineSqlQueryAsync(string currentQuery, string userFeedback, string databaseSchema)
        {
            var messages = new List<ChatMessage>
            {
                ChatMessage.FromSystem(@"You are a SQL query refinement assistant. 
                    Modify the provided SQL query based on user feedback while:
                    1. Maintaining the original query's core purpose
                    2. Only using existing tables and columns
                    3. Ensuring the changes are valid SQL
                    4. Explaining the changes made"),
                ChatMessage.FromUser($"Database Schema:\n{databaseSchema}"),
                ChatMessage.FromUser($"Current Query:\n{currentQuery}"),
                ChatMessage.FromUser($"Please modify the query based on this feedback: {userFeedback}")
            };

            var chatRequest = new ChatCompletionCreateRequest
            {
                Model = _model,
                Messages = messages
            };

            var response = await _openAI.ChatCompletion.CreateCompletion(chatRequest);
            return response?.Choices[0]?.Message?.Content ?? string.Empty;
        }

        public async Task<bool> ValidateSqlQueryAsync(string query, string databaseSchema)
        {
            var messages = new List<ChatMessage>
            {
                ChatMessage.FromSystem(@"You are a SQL query validator. 
                    Analyze the provided SQL query and respond with 'VALID' or 'INVALID' followed by explanation.
                    Check for:
                    1. SQL syntax correctness
                    2. Use of existing tables and columns
                    3. Proper JOIN conditions
                    4. Potential performance issues"),
                ChatMessage.FromUser($"Database Schema:\n{databaseSchema}"),
                ChatMessage.FromUser($"Validate this SQL query:\n{query}")
            };

            var chatRequest = new ChatCompletionCreateRequest
            {
                Model = _model,
                Messages = messages
            };

            var response = await _openAI.ChatCompletion.CreateCompletion(chatRequest);
            var content = response?.Choices[0]?.Message?.Content;
            return !string.IsNullOrEmpty(content) && content.StartsWith("VALID", StringComparison.OrdinalIgnoreCase);
        }

        public async Task<string> ExplainQueryAsync(string query)
        {
            var messages = new List<ChatMessage>
            {
                ChatMessage.FromSystem(@"You are a SQL query explainer. 
                    Provide a clear, concise explanation of what the SQL query does.
                    Include:
                    1. Main purpose of the query
                    2. Tables involved
                    3. Key operations (joins, aggregations, etc.)
                    4. Expected output format"),
                ChatMessage.FromUser($"Explain this SQL query in simple terms:\n{query}")
            };

            var chatRequest = new ChatCompletionCreateRequest
            {
                Model = _model,
                Messages = messages
            };

            var response = await _openAI.ChatCompletion.CreateCompletion(chatRequest);
            return response?.Choices[0]?.Message?.Content ?? string.Empty;
        }
    }
} 