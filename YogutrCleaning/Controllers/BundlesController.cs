﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YogurtCleaning.Business;
using YogurtCleaning.Business.Services;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.Extensions;
using YogurtCleaning.Infrastructure;
using YogurtCleaning.Models;

namespace YogurtCleaning.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class BundlesController : ControllerBase
{
    private readonly IBundlesService _bundlesService;
    private readonly IMapper _mapper;

    public BundlesController( IBundlesService bundlesService, IMapper mapper)
    {
        _bundlesService = bundlesService;
        _mapper = mapper;
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(BundleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BundleResponse>> GetBundle(int id)
    {
        var result = await _bundlesService.GetBundle(id);
        return Ok(_mapper.Map<BundleResponse>(result));
    }

    [AuthorizeRoles(Role.Admin)]
    [HttpGet]
    [ProducesResponseType(typeof(List<BundleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<List<BundleResponse>>> GetAllBundles()
    {
        var result = _mapper.Map<List<BundleResponse>>(await _bundlesService.GetAllBundles());
        return Ok(result);
    }

    [AuthorizeRoles(Role.Admin)]
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult> UpdateBundle([FromBody] BundleRequest bundle, int id)
    {
        await _bundlesService.UpdateBundle(_mapper.Map<Bundle>(bundle), id);
        return NoContent();
    }

    [AuthorizeRoles(Role.Admin)]
    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(void), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<int>> AddBundle([FromBody] BundleRequest bundle)
    {
        var result = await _bundlesService.AddBundle(_mapper.Map<Bundle>(bundle));
        return Created($"{this.GetRequestFullPath()}/{result}", result);
    }

    [AuthorizeRoles(Role.Admin)]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteBundle(int id)
    {
        await _bundlesService.DeleteBundle(id);
        return NoContent();
    }

    [AllowAnonymous]
    [HttpGet("{id}/additional-services")]
    [ProducesResponseType(typeof(List<ServiceResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ServiceResponse>>> GetAdditionalServices(int id)
    {
        var result =  _mapper.Map<List<ServiceResponse>>(await _bundlesService.GetAdditionalServices(id));
        return Ok(result);
    }
}