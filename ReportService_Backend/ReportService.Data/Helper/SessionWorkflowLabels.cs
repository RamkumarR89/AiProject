public static class SessionWorkflowLabels
{
    public static string GetLabel(SessionWorkflowStep step)
    {
        return step switch
        {
            SessionWorkflowStep.ReportName => "Map Report Name",
            SessionWorkflowStep.MessageAndQuery => "Map Message & Query",
            SessionWorkflowStep.ChartConfigured => "Chart Configuration",
            SessionWorkflowStep.Published => "Publish Report",
            _ => "Unknown Step"
        };
    }
}
