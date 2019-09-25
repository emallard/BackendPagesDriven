using System;

namespace CocoriCore.PageLogs
{

    public class TestEmailRead : EntityBase<TestEmailRead>
    {
        public TId<Test> TestId;
        public TId<TestUser> UserId;
        public TId<EmailType> EmailTypeId;
        public object MailMessage;
    }
}