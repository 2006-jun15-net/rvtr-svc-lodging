using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RVTR.Lodging.DataContext.Repositories;
using RVTR.Lodging.ObjectModel.Models;

namespace RVTR.Lodging.WebApi.Controllers
{
  /// <summary>
  /// Lodging controller.
  /// </summary>
  [ApiController]
  [ApiVersion("0.0")]
  [EnableCors("public")]
  [Route("rest/lodging/{version:apiVersion}/[controller]")]
  public class LodgingController : ControllerBase
  {
    private readonly ILogger<LodgingController> _logger;
    private readonly UnitOfWork _unitOfWork;

    /// <summary>
    /// Lodging constructor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="unitOfWork"></param>
    public LodgingController(ILogger<LodgingController> logger, UnitOfWork unitOfWork)
    {
      _logger = logger;
      _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// deletes a lodging with a specified id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
      try
      {
        await _unitOfWork.Lodging.DeleteAsync(id);
        await _unitOfWork.CommitAsync();

        return Ok();
      }
      catch
      {
        return NotFound(id);
      }
    }

    /// <summary>
    /// gets all lodgings.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
      return Ok(await _unitOfWork.Lodging.SelectAsync());
    }

    /// <summary>
    /// gets all lodgings for a specified id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
      if(id <= 0)
      {
        
      }

      try
      {
        return Ok(await _unitOfWork.Lodging.SelectAsync(id));
      }
      catch
      {
        return NotFound(id);
      }
    }

    /// <summary>
    /// Adds a lodging 
    /// </summary>
    /// <param name="lodging"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Post(LodgingModel lodging)
    {
      await _unitOfWork.Lodging.InsertAsync(lodging);
      await _unitOfWork.CommitAsync();

      return Accepted(lodging);
    }

    /// <summary>
    /// Updates a lodging
    /// </summary>
    /// <param name="lodging"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<IActionResult> Put(LodgingModel lodging)
    {
      _unitOfWork.Lodging.Update(lodging);
      await _unitOfWork.CommitAsync();

      return Accepted(lodging);
    }
  }
}
