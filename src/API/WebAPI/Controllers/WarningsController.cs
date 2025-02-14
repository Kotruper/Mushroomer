﻿using Application.Commands;
using Application.Dto;
using Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WarningsController : ControllerBase
{
    private readonly IMediator _mediator;

    public WarningsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET: api/<WarningsController>
    [HttpGet]
    public async Task<IEnumerable<WarningDto>> Get()
    {
        var request = new GetAllWarningsQuery();

        var result = await _mediator.Send(request);

        return result;
    }

    // GET api/<WarningsController>/5
    [HttpGet("{id}")]
    public async Task<WarningDto> Get(Guid id)
    {
        var request = new GetWarningQuery { Id = id };

        var result = await _mediator.Send(request);

        return result;
    }

    // POST api/<WarningsController>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] WarningDto warning)    //todo: set id to be nullable in post and not in put
    {
        var request = new AddWarningCommand { Warning = warning };

        var result = await _mediator.Send(request);

        return Ok();
    }

    // PUT api/<WarningController>/5
    //todo: [HttpPut("{id}")]
    [HttpPut]
    public async Task<IActionResult> Put([FromBody] WarningDto warning)
    {
        var request = new UpdateWarningCommand { Warning = warning };

        var result = await _mediator.Send(request);

        return Ok();
    }

    // DELETE api/<WarningController>/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var request = new DeleteWarningCommand { Id = id };

        var result = await _mediator.Send(request);

        return Ok();
    }
}
