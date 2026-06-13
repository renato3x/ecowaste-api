using EcoWasteApi.Models;
using EcoWasteApi.Services;
using EcoWasteApi.ViewModels.Requests;
using EcoWasteApi.ViewModels.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcoWasteApi.Controllers;

[ApiController]
[Route("api/waste-records")]
[Authorize]
public class WasteRecordsController(IWasteRecordService wasteRecordService) : ControllerBase
{
  [HttpPost]
  [ProducesResponseType(typeof(WasteRecordResponse), StatusCodes.Status201Created)]
  public async Task<IActionResult> Create([FromBody] CreateWasteRecordRequest request, CancellationToken cancellationToken)
  {
    var response = await wasteRecordService.CreateAsync(request, cancellationToken);
    return StatusCode(StatusCodes.Status201Created, response);
  }

  [HttpGet]
  [ProducesResponseType(typeof(PagedResponse<WasteRecordResponse>), StatusCodes.Status200OK)]
  public async Task<ActionResult<PagedResponse<WasteRecordResponse>>> GetPaged(
    [FromQuery(Name = "page")] int page = 1,
    [FromQuery(Name = "page_size")] int pageSize = 10,
    CancellationToken cancellationToken = default)
  {
    var response = await wasteRecordService.GetPagedAsync(page, pageSize, cancellationToken);
    return Ok(response);
  }

  [HttpGet("{id:int}")]
  [ProducesResponseType(typeof(WasteRecordResponse), StatusCodes.Status200OK)]
  public async Task<ActionResult<WasteRecordResponse>> GetById(int id, CancellationToken cancellationToken)
  {
    var response = await wasteRecordService.GetByIdAsync(id, cancellationToken);
    return Ok(response);
  }

  [HttpPut("{id:int}")]
  [ProducesResponseType(typeof(WasteRecordResponse), StatusCodes.Status200OK)]
  public async Task<ActionResult<WasteRecordResponse>> Update(int id, [FromBody] UpdateWasteRecordRequest request,
    CancellationToken cancellationToken)
  {
    var response = await wasteRecordService.UpdateAsync(id, request, cancellationToken);
    return Ok(response);
  }

  [HttpDelete("{id:int}")]
  [Authorize(Roles = UserRoles.Admin)]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
  {
    await wasteRecordService.DeleteAsync(id, cancellationToken);
    return NoContent();
  }

  [HttpGet("alerts")]
  [ProducesResponseType(typeof(CollectionAlertResponse), StatusCodes.Status200OK)]
  public async Task<ActionResult<CollectionAlertResponse>> GetAlerts(CancellationToken cancellationToken)
  {
    var response = await wasteRecordService.GetCollectionAlertAsync(cancellationToken);
    return Ok(response);
  }

  [HttpGet("report")]
  [ProducesResponseType(typeof(WasteReportResponse), StatusCodes.Status200OK)]
  public async Task<ActionResult<WasteReportResponse>> GetReport(CancellationToken cancellationToken)
  {
    var response = await wasteRecordService.GetReportAsync(cancellationToken);
    return Ok(response);
  }
}

