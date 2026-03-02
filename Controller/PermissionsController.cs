using FirstProject.Application.Common;
using FirstProject.Application.Common.Permissions;
using FirstProject.Application.Features.Permissions.Commands.CreatePermission;
using FirstProject.Application.Features.Permissions.Commands.DeletePermission;
using FirstProject.Application.Features.Permissions.Commands.UpdatePermission;
using FirstProject.Application.Features.Permissions.Queries.GetPermissions;
using FirstProject.Infrastructure.Auth.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PermissionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PermissionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [RequirePermission(PermissionCodes.Permissions_List)]
    public async Task<ActionResult<Result<IReadOnlyList<PermissionItemDto>>>> GetAll()
    {
        var result = await _mediator.Send(new GetPermissionsQuery());
        return Ok(result);
    }

    [HttpPost]
    [RequirePermission(PermissionCodes.Permissions_Create)]
    public async Task<ActionResult<Result<Guid>>> Create([FromBody] CreatePermissionCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [RequirePermission(PermissionCodes.Permissions_Delete)]
    public async Task<ActionResult<Result<Guid>>> Delete([FromRoute] Guid id)
    {
        var result = await _mediator.Send(new DeletePermissionCommand { PermissionId = id });
        return Ok(result);
    }


    [HttpPut("{id}")]
    [RequirePermission(PermissionCodes.Permissions_Update)]
    public async Task<ActionResult<Result<Guid>>> Update([FromRoute] Guid id, [FromBody] UpdatePermissionCommand command)
    {
        var result = await _mediator.Send(command with { PermissionId = id });
        return Ok(result);
    }

    // [HttpGet("{id}")]
    // public async Task<ActionResult<Result<Guid>>> GetById([FromRoute] Guid id)
    // {
    //     var result = await _mediator.Send(new GetPermissionByIdQuery { PermissionId = id });
    //     return Ok(result);
    // }                    
}