using System.Threading.Tasks;

using CulinaryBlogCore.Services.Models;

namespace CulinaryBlogCore.Services.Contracts
{
    public interface IImgurService
    {
        Task UploadImage(ImageViewModel imageViewModel);

        Task DeleteImage(string imageId);

        Task EditImage(ImageViewModel imageViewModel);
    }
}
