namespace TaskManager.Application
{
    public enum TaskStateEnum : byte
    {
        New = 0,
        Active = 1,
        Closed = 2
    }

    public enum TaskPrioriyEnum : byte
    {
        High = 0,
        Medium = 1,
        Low = 3
    }
}
