using FreeCourse.Frontends.Web.Models;
using Microsoft.Extensions.Options;

namespace FreeCourse.Frontends.Web.Helpers
{
    public class PhotoHelper
    {
        private readonly ServiceApiSettings _serviceApiSettings;

        public PhotoHelper(IOptions<ServiceApiSettings> serviceApiSettings)
        {
            _serviceApiSettings = serviceApiSettings.Value;
        }

        public string GetPhotoStockURL(string photoURL)
        {
            return $"{_serviceApiSettings.PhotoStockUri}/photos/{photoURL}";
        }
    }
}
