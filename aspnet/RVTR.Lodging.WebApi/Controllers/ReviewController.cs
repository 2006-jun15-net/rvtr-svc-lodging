using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RVTR.Lodging.DataContext.Repositories;
using RVTR.Lodging.ObjectModel.Models;

namespace RVTR.Lodging.WebApi.Controllers
{
  /// <summary>
  /// Review controller
  /// </summary>
  [ApiController]
  [ApiVersion("0.0")]
  [EnableCors("public")]
  [Route("rest/lodging/{version:apiVersion}/[controller]")]
  public class ReviewController : ControllerBase
  {
    private readonly ILogger<ReviewController> _logger;
    private readonly UnitOfWork _unitOfWork;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="unitOfWork"></param>
    public ReviewController(ILogger<ReviewController> logger, UnitOfWork unitOfWork)
    {
      _logger = logger;
      _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// deletes a review with a specified id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
      try
      {
        await _unitOfWork.Review.DeleteAsync(id);
        await _unitOfWork.CommitAsync();

        return Ok();
      }
      catch
      {
        return NotFound(id);
      }
    }

    /// <summary>
    /// gets all reviews.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
      return Ok(await _unitOfWork.Review.SelectAsync());
    }

    /// <summary>
    /// gets rentals with a specified id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
      try
      {
        return Ok(await _unitOfWork.Review.SelectAsync(id));
      }
      catch
      {
        return NotFound(id);
      }
    }

    /// <summary>
    /// adds a rental 
    /// </summary>
    /// <param name="review"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Post(ReviewModel review)
    {
      await _unitOfWork.Review.InsertAsync(review);
      await _unitOfWork.CommitAsync();

      return Accepted(review);
    }

    /// <summary>
    /// modifies the rental.
    /// </summary>
    /// <param name="review"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<IActionResult> Put(ReviewModel review)
    {
      _unitOfWork.Review.Update(review);
      await _unitOfWork.CommitAsync();

      return Accepted(review);
    }
  }
}
