using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RVTR.Lodging.DataContext.Repositories;
using RVTR.Lodging.ObjectModel.Models;

namespace RVTR.Lodging.WebApi.Controllers
{
  /// <summary>
  /// Rental controller.
  /// </summary>
  [ApiController]
  [ApiVersion("0.0")]
  [EnableCors("public")]
  [Route("rest/lodging/{version:apiVersion}/[controller]")]
  public class RentalController : ControllerBase
  {
    private readonly ILogger<RentalController> _logger;
    private readonly UnitOfWork _unitOfWork;

    /// <summary>
    /// Rental constructor.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="unitOfWork"></param>
    public RentalController(ILogger<RentalController> logger, UnitOfWork unitOfWork)
    {
      _logger = logger;
      _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// deletes a rental with a specified id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
      try
      {
        await _unitOfWork.Rental.DeleteAsync(id);
        await _unitOfWork.CommitAsync();

        return Ok();
      }
      catch
      {
        return NotFound(id);
      }
    }

    /// <summary>
    /// gets all rentals
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
      return Ok(await _unitOfWork.Rental.SelectAsync());
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
        return Ok(await _unitOfWork.Rental.SelectAsync(id));
      }
      catch
      {
        return NotFound(id);
      }
    }

    /// <summary>
    /// adds a rental 
    /// </summary>
    /// <param name="rental"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Post(RentalModel rental)
    {
      await _unitOfWork.Rental.InsertAsync(rental);
      await _unitOfWork.CommitAsync();

      return Accepted(rental);
    }

    /// <summary>
    /// updates a rental.
    /// </summary>
    /// <param name="rental"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<IActionResult> Put(RentalModel rental)
    {
      _unitOfWork.Rental.Update(rental);
      await _unitOfWork.CommitAsync();

      return Accepted(rental);
    }
  }
}
