using FirstProject.Application.Common;
using FirstProject.Application.Common.Permissions;
using FirstProject.Application.Features.Groups.Commands.CreateGroup;
using FirstProject.Application.Features.Groups.Commands.DeleteGroup;
using FirstProject.Application.Features.Groups.Commands.UpdateGroup;
using FirstProject.Application.Features.Groups.Queries.GetGroupById;
using FirstProject.Infrastructure.Auth.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GroupsController : ControllerBase
{
    private readonly IMediator _mediator;

    public GroupsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [RequirePermission(PermissionCodes.Groups_Create)]
    public async Task<ActionResult<Result<Guid>>> Create([FromBody] CreateGroupCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [RequirePermission(PermissionCodes.Groups_Delete)]
    public async Task<ActionResult<Result<Guid>>> Delete([FromRoute] Guid id)
    {
        var result = await _mediator.Send(new DeleteGroupCommand { GroupId = id });
        return Ok(result);
    }


    [HttpPut("{id}")]
    [RequirePermission(PermissionCodes.Groups_Update)]
    public async Task<ActionResult<Result<Guid>>> Update([FromRoute] Guid id, [FromBody] UpdateGroupCommand command)
    {
        var result = await _mediator.Send(command with { GroupId = id });
        return Ok(result);
    }

    [HttpGet("{id}")]
    [RequirePermission(PermissionCodes.Groups_View)]
    public async Task<ActionResult<Result<Guid>>> GetById([FromRoute] Guid id)
    {
        var result = await _mediator.Send(new GetGroupByIdQuery { GroupId = id });
        return Ok(result);
    }
}