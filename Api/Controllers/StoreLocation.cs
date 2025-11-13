using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoreLocation : ControllerBase
    {
        [HttpGet]
        public IActionResult GetStoreLocation()
        {
            var location = new
            {
                Latitude = 10.875139,
                Longitude = 106.800729,
                Address = "Phòng 308 Lầu 3 Nhà Văn hóa Sinh viên TP.HCM, Số 1 Lưu Hữu Phước, Đông Hoà, Dĩ An, Thành phố Hồ Chí Minh, Việt Nam"
            };
            return Ok(location);
        }
    }
}
