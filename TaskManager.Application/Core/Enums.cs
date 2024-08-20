namespace TaskManager.Application
{
    public enum TaskStateEnum : byte
    {
        New = 1,
        Active = 2,
        Closed = 3
    }

    public enum TaskPrioriyEnum : byte
    {
        High = 1,
        Medium = 2,
        Low = 3
    }

    public enum UserProfileEnum : byte
    {
        None = 0,
        Manager = 1,
        Normal = 2
    }
}
