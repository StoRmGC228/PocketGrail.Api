namespace PocketGrail.Api.Controllers;

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PocketGrail.Api.Helpers;
using PocketGrail.Application.DTOs;
using PocketGrail.Application.Interfaces;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class CampaignsController : ControllerBase
{
    private readonly ICampaignService _campaignService;

    public CampaignsController(ICampaignService campaignService)
    {
        _campaignService = campaignService;
    }

    // POST /api/campaigns  — DM only, multipart/form-data
    [HttpPost]
    [Authorize(Policy = "DungeonMasterOnly")]
    public async Task<IActionResult> CreateCampaign(
        [FromForm] CreateCampaignRequest request,
        CancellationToken ct)
    {
        var dmUserId = ClaimsHelper.GetUserId(User);
        var campaign = await _campaignService.CreateCampaignAsync(request, dmUserId, ct);
        return CreatedAtAction(nameof(GetByCode), new { code = campaign.ConnectionCode }, campaign);
    }

    // POST /api/campaigns/join
    [HttpPost("join")]
    public async Task<IActionResult> JoinCampaign(
        [FromBody] JoinCampaignRequest request,
        CancellationToken ct)
    {
        var userId = ClaimsHelper.GetUserId(User);
        var campaign = await _campaignService.JoinCampaignAsync(request, userId, ct);
        return Ok(campaign);
    }

    // GET /api/campaigns  — all active campaigns
    [HttpGet]
    public async Task<IActionResult> GetActiveCampaigns(CancellationToken ct)
    {
        var campaigns = await _campaignService.GetActiveCampaignsAsync(ct);
        return Ok(campaigns);
    }

    // GET /api/campaigns/mine  — role-filtered: DM sees owned, Player sees joined
    [HttpGet("mine")]
    public async Task<IActionResult> GetMyCampaigns(CancellationToken ct)
    {
        var userId = ClaimsHelper.GetUserId(User);
        var role = User.FindFirstValue(ClaimTypes.Role) ?? string.Empty;
        var campaigns = await _campaignService.GetMyCampaignsAsync(userId, role, ct);
        return Ok(campaigns);
    }

    // GET /api/campaigns/by-id/{id}
    [HttpGet("by-id/{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var campaign = await _campaignService.GetByIdAsync(id, ct);
        return campaign is null ? NotFound() : Ok(campaign);
    }

    // GET /api/campaigns/{code}
    [HttpGet("{code}")]
    public async Task<IActionResult> GetByCode(string code, CancellationToken ct)
    {
        var campaign = await _campaignService.GetByCodeAsync(code, ct);
        return campaign is null ? NotFound() : Ok(campaign);
    }

    // DELETE /api/campaigns/{id}  — owner DM only
    [HttpDelete("{id:int}")]
    [Authorize(Policy = "DungeonMasterOnly")]
    public async Task<IActionResult> DeleteCampaign(int id, CancellationToken ct)
    {
        var dmUserId = ClaimsHelper.GetUserId(User);
        await _campaignService.DeleteCampaignAsync(id, dmUserId, ct);
        return NoContent();
    }

    // DELETE /api/campaigns/{id}/leave  — current participant leaves
    [HttpDelete("{id:int}/leave")]
    public async Task<IActionResult> LeaveCampaign(int id, CancellationToken ct)
    {
        var userId = ClaimsHelper.GetUserId(User);
        await _campaignService.LeaveCampaignAsync(id, userId, ct);
        return NoContent();
    }

}
