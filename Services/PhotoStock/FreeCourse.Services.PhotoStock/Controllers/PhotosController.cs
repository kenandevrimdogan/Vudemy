using FreeCourse.Services.PhotoStock.Dtos.photos;
using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.Dtos.NoContent;
using FreeCourse.Shared.Dtos.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace FreeCourse.Services.PhotoStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : CustomBaseController
    {
        public PhotosController()
        {

        }

        [HttpPost]
        public async Task<IActionResult> PhotoSave(IFormFile photo, CancellationToken cancellationToken)
        {
            if (photo != null && photo.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photo.FileName);

                using var stream = new FileStream(path, FileMode.Create);
                await photo.CopyToAsync(stream, cancellationToken);

                var returnPath = $"photos/{photo.FileName}";

                PhotoDTO photoDTO = new() { URL = returnPath };

                return CreateActionResultInstance(ResponseDTO<PhotoDTO>.Success(photoDTO, HttpStatusCode.OK));
            }

            return CreateActionResultInstance(ResponseDTO<PhotoDTO>.Fail("photo is empty", HttpStatusCode.BadRequest));
        }

        [HttpDelete("{photoURL}")]
        public IActionResult PhotoDelete(string photoURL)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photoURL);

            if (!System.IO.File.Exists(path))
            {
                return CreateActionResultInstance(ResponseDTO<PhotoDTO>.Fail("photo not found", HttpStatusCode.BadRequest));
            }

            System.IO.File.Delete(path);

            return CreateActionResultInstance(ResponseDTO<NoContent>.Success(HttpStatusCode.NoContent));
        }
    }
}
