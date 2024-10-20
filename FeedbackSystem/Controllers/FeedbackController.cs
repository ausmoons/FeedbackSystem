using AutoMapper;
using FeedbackSystem.Data;
using FeedbackSystem.Models.DTOs;
using FeedbackSystem.Models.Entities;
using FeedbackSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FeedbackSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : ControllerBase
    {
        public FeedbackController(AppDbContext context,
            IFeedbackService feedbackService,
            IMapper mapper,
            ILogger<FeedbackController> logger)
        {
            _context = context;
            _feedbackService = feedbackService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a list of feedback entries with pagination.
        /// </summary>
        /// <param name="queryOptions">Options for pagination, including page number and page size.</param>
        /// <returns>A paginated list of feedback entries.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Feedback>>> GetFeedbacks([FromQuery] QueryOptions queryOptions)
        {
            var (totalRecords, feedbackList) = await _feedbackService.GetFeedbacks(queryOptions);

            return Ok(new
            {
                TotalRecords = totalRecords,
                Page = queryOptions?.Page ?? queryOptions?.Page,
                PageSize = queryOptions?.PageSize ?? queryOptions?.PageSize,
                Data = feedbackList
            });
        }

        /// <summary>
        /// Filters feedback entries based on specific criteria.
        /// </summary>
        /// <param name="filter">The filter criteria, such as Rating, ProductId, etc.</param>
        /// <returns>A filtered list of feedback entries.</returns>
        [HttpPost("filter")]
        public async Task<ActionResult<IEnumerable<Feedback>>> FilterFeedbacks([FromBody] FeedbackFilter filter)
        {
            var (totalRecords, feedbackList) = await _feedbackService.GetFilteredFeedbacks(filter);

            if (feedbackList == null || !feedbackList.Any())
            {
                return NotFound(new { Message = "No feedback entries match the specified criteria." });
            }

            return Ok(new
            {
                TotalRecords = totalRecords,
                Page = filter?.Page ?? filter?.Page,
                PageSize = filter?.PageSize ?? filter?.PageSize,
                Data = feedbackList
            });
        }

        /// <summary>
        /// Creates a new feedback entry.
        /// </summary>
        /// <param name="feedbackDTO">The feedback object to create.</param>
        /// <returns>The newly created feedback entry.</returns>
        [HttpPost]
        public async Task<ActionResult<Feedback>> PostFeedback(FeedbackDTO feedbackDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var feedback = _mapper.Map<Feedback>(feedbackDTO);

                feedback.CreatedAt = DateTime.UtcNow;
                _context.Feedbacks.Add(feedback);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetFeedbacks), new { id = feedback.Id }, feedback);
            }
            catch (Exception ex)
            {
                return HandleException(ex, $"An unexpected error occurred while creating feedback for product {feedbackDTO.ProductId} from customer with id {feedbackDTO.CustomerId}");
            }
        }

        /// <summary>
        /// Updates an existing feedback entry by ID.
        /// </summary>
        /// <param name="Id">The feedback id.</param>
        /// <param name="feedbackDTO">The updated feedback object.</param>
        /// <returns>Ok if the update is successful.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> PutFeedback(int id, FeedbackDTO feedbackDTO)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existingFeedback = await _context.Feedbacks.FindAsync(id);
                if (existingFeedback == null)
                {
                    return NotFound(new { Message = $"Feedback with ID {id} not found." });
                }

                _mapper.Map(feedbackDTO, existingFeedback);
                await _context.SaveChangesAsync();

                return Ok(existingFeedback);
            }
            catch (Exception ex)
            {
                return HandleException(ex, $"An unexpected error occurred while updating feedback with ID {id}");
            }
        }

        /// <summary>
        /// Deletes a feedback entry by ID.
        /// </summary>
        /// <param name="id">The ID of the feedback to delete.</param>
        /// <returns>Ok if the deletion is successful.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFeedback(int id)
        {
            try
            {
                var feedback = await _context.Feedbacks.FindAsync(id);
                if (feedback == null)
                {
                    return NotFound();
                }

                _context.Feedbacks.Remove(feedback);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {

                return HandleException(ex, $"An unexpected error occurred while updating feedback with ID {id}");
            }
        }

        /// <summary>
        /// Handles exceptions by logging the error and returning a 500 status code.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        private ActionResult HandleException(Exception ex, string message)
        {
            _logger.LogError(ex, message);
            return StatusCode(500, new { Message = $"{message}. {ex.Message}." });
        }

        #region Fields
        private readonly AppDbContext _context;
        private readonly IFeedbackService _feedbackService;
        private readonly ILogger<FeedbackController> _logger;
        private readonly IMapper _mapper;
        #endregion
    }
}
