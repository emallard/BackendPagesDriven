using CocoriCore;

namespace CocoriCore
{
    public interface IClaimsResponse
    {
        IClaims GetClaims();
        object GetResponse();
    }
}