public class SessionWorkflowStatusDto
{
    public int Step { get; set; }
    public string Label { get; set; }
    public bool IsCompleted { get; set; }

    public SessionWorkflowStatusDto(SessionWorkflowStep step, string label, bool isCompleted)
    {
        Step = (int)step;
        Label = label;
        IsCompleted = isCompleted;
    }
}
