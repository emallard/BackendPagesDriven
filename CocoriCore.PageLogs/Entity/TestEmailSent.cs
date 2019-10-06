﻿using System;
using CocoriCore.Page;

namespace CocoriCore.PageLogs
{
    public class TestEmailSent : EntityBase<TestEmailSent>
    {
        public string TestName;
        public int IndexInTest;
        public string UserName;
        public string ScenarioNames;
        public string PageName;
        public string MessageName;
        public string EmailName;
        /*
        public TId<Test> TestId;
        public TId<TestUser> TestUserId;
        public TId<TestPage> TestPageId;
        public TId<PageType> PageTypeId;
        public TId<TestMessage> TestMessageId;
        public TId<MessageType> MessageTypeId;

        public TId<EmailType> EmailTypeId;
        public MyMailMessage MailMessage;
        */
    }
}