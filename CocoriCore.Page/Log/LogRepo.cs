using System;

namespace CocoriCore.Page
{
    public class LogRepo : UserLog
    {
        public LogRepoOperation Operation;
        public Type EntityType;

        public LogRepo(LogRepoOperation operation, Type entityType)
        {
            Operation = operation;
            EntityType = entityType;
        }
    }

    public enum LogRepoOperation
    {
        Delete,
        Exists,
        Insert,
        Load,
        Query,
        Update
    }
}
