using System;

namespace CocoriCore.Page
{
    public class TestBrowserClaimsProvider
    {
        public Func<object, IClaims> OnResponse;

        public TestBrowserClaimsProvider(Func<object, IClaims> onResponse)
        {
            OnResponse = onResponse;
        }
    }
}
