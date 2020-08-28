using System.Threading.Tasks;
using CulinaryBlogCore.Services.Utils;
using Imgur.API.Endpoints;

namespace CulinaryBlogCore.Services.Contracts
{
    public interface IImgurTokenService
    {
        Task<ImageEndpoint> GetImageEndpoint();
    }
}
