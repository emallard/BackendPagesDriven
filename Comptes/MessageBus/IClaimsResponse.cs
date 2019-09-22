using CocoriCore;

namespace Comptes
{
    public interface IClaimsResponse
    {
        IClaims GetClaims();
        object GetResponse();
    }
}