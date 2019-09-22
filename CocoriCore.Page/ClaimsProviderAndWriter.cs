
namespace CocoriCore.Page
{
    public class ClaimsProviderAndWriter : IClaimsProvider, IClaimsWriter
    {
        private IClaims claims;
        public bool HasClaims => claims != null;

        public TClaims GetClaims<TClaims>()
        {
            return (TClaims)claims;
        }

        public void SetClaims(IClaims claims)
        {
            this.claims = claims;
        }
    }
}