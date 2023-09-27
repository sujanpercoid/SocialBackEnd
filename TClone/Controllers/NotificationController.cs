using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TClone.Data;
using TClone.Services;

namespace TClone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly TcDbcontext _noti;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly INotification _notification;

        public NotificationController(TcDbcontext noti, IMapper mapper,  IConfiguration config,INotification notification)
        {
            _noti = noti;
            _mapper = mapper;
            _config = config;
            _notification = notification;
        }

        //Get Notification 
        [HttpGet("notification/{id}")]
        public async Task<IActionResult> GetNotification([FromRoute] string id)
        {
            var not = await _notification.Notification(id);
            return Ok(not);
        }
        // Delete All Notification
        [HttpDelete("allnotification/{id}")]
        public async Task<IActionResult>DeleteAllNotification([FromRoute] string id)
        {
            var not = await _notification.DeleteAllNoti(id);
            return Ok (not);
        }
        //Delete single Notification
        [HttpDelete("noti/{id}")]
        public async Task<IActionResult>DeleteNoti([FromRoute] int id)
        {
            var noti = await _notification.DeleteNoti(id);
            return Ok(noti);
        }
    }
}
