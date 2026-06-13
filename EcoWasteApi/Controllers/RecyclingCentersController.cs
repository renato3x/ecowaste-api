using EcoWasteApi.Models;
using EcoWasteApi.Services;
using EcoWasteApi.ViewModels.Requests;
using EcoWasteApi.ViewModels.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcoWasteApi.Controllers;

[ApiController]
[Route("api/recycling-centers")]
[Authorize]
public class RecyclingCentersController(IRecyclingCenterService recyclingCenterService) : ControllerBase
{
  [HttpPost]
  [ProducesResponseType(typeof(RecyclingCenterResponse), StatusCodes.Status201Created)]
  public async Task<IActionResult> Create([FromBody] CreateRecyclingCenterRequest request, CancellationToken cancellationToken)
  {
    var response = await recyclingCenterService.CreateAsync(request, cancellationToken);
    return StatusCode(StatusCodes.Status201Created, response);
  }

  [HttpGet]
  [ProducesResponseType(typeof(PagedResponse<RecyclingCenterResponse>), StatusCodes.Status200OK)]
  public async Task<ActionResult<PagedResponse<RecyclingCenterResponse>>> GetPaged(
    [FromQuery(Name = "page")] int page = 1,
    [FromQuery(Name = "page_size")] int pageSize = 10,
    CancellationToken cancellationToken = default)
  {
    var response = await recyclingCenterService.GetPagedAsync(page, pageSize, cancellationToken);
    return Ok(response);
  }

  [HttpGet("{id:int}")]
  [ProducesResponseType(typeof(RecyclingCenterResponse), StatusCodes.Status200OK)]
  public async Task<ActionResult<RecyclingCenterResponse>> GetById(int id, CancellationToken cancellationToken)
  {
    var response = await recyclingCenterService.GetByIdAsync(id, cancellationToken);
    return Ok(response);
  }

  [HttpPut("{id:int}")]
  [ProducesResponseType(typeof(RecyclingCenterResponse), StatusCodes.Status200OK)]
  public async Task<ActionResult<RecyclingCenterResponse>> Update(int id, [FromBody] UpdateRecyclingCenterRequest request,
    CancellationToken cancellationToken)
  {
    var response = await recyclingCenterService.UpdateAsync(id, request, cancellationToken);
    return Ok(response);
  }

  [HttpDelete("{id:int}")]
  [Authorize(Roles = UserRoles.Admin)]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
  {
    await recyclingCenterService.DeleteAsync(id, cancellationToken);
    return NoContent();
  }
}

