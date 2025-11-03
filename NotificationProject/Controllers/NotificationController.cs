using Microsoft.AspNetCore.Mvc;
using NotificationProject.Models.DTO;
using NotificationProject.Service;
using NotificationProject.Service.IService;
using static NotificationProject.Utilities.SD;

namespace NotificationProject.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly ILogger<NotificationController> _logger;
        private readonly INotificationService _notificationService;
        public NotificationController(ILogger<NotificationController> logger,INotificationService notificationService)
        {
            _logger = logger;
            _notificationService = notificationService;
        }

        // Send Notification with default channel
        [HttpPost(Name = "SendNotification")]
        [ProducesResponseType(statusCode: StatusCodes.Status201Created)]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SendNotification([FromBody] SendNotificationDto dto)
        {
            try
            {
                _logger.LogInformation("API called to send notification for user {UserId}", dto.UserId);
                var result = await _notificationService.SendNotificationAsync(dto.UserId, dto.Title, dto.Message, null);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }
        }

        // Send Notification with selected channel
        [HttpPost("override", Name = "SendNotificationWithChannel")]
        [ProducesResponseType(statusCode: StatusCodes.Status201Created)]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SendNotificationWithChannel([FromBody] SendNotificationDto dto, [FromQuery] NotificationChannel channel)
        {
            try
            {
                var result = await _notificationService.SendNotificationAsync(dto.UserId, dto.Title, dto.Message, channel);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }
        }


        // Get notifications for one user or all users with pagination
        [HttpGet(Name = "GetNotifications")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetNotifications([FromQuery] int? userId, [FromQuery] int skip = 0, [FromQuery] int take = 50)
        {
            try
            {
                var list = await _notificationService.GetNotificationsAsync(userId, skip, take);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }
        }

        // get notification with id
        [HttpGet("{id}", Name = "GetNotificationById")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var n = await _notificationService.GetNotificationById(id);
                if (n == null) return NotFound();
                return Ok(n);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }
        }

    }
}
