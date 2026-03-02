using FirstProject.Application.Common;
using FirstProject.Application.Common.Permissions;
using FirstProject.Application.Features.Roles.Commands.AsignPermission;
using FirstProject.Application.Features.Roles.Commands.ChangePermission;
using FirstProject.Application.Features.Roles.Commands.CreateRole;
using FirstProject.Application.Features.Roles.Commands.DeleteRole;
using FirstProject.Application.Features.Roles.Commands.RemovePermission;
using FirstProject.Application.Features.Roles.Commands.UpdateRole;
using FirstProject.Application.Features.Roles.Queries.GetRoleById;
using FirstProject.Application.Features.Roles.Queries.GetRoles;
using FirstProject.Infrastructure.Auth.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RolesController : ControllerBase
{
    private readonly IMediator _mediator;

    public RolesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [RequirePermission(PermissionCodes.Roles_List)]
    public async Task<ActionResult<Result<IReadOnlyList<RoleItemDto>>>> GetAll()
    {
        var result = await _mediator.Send(new GetRolesQuery());
        return Ok(result);
    }

    [HttpPost]
    [RequirePermission(PermissionCodes.Roles_Create)]
    public async Task<ActionResult<Result<Guid>>> Create([FromBody] CreateRoleCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [RequirePermission(PermissionCodes.Roles_Delete)]
    public async Task<ActionResult<Result<Guid>>> Delete([FromRoute] Guid id)
    {
        var result = await _mediator.Send(new DeleteRoleCommand { RoleId = id });
        return Ok(result);
    }


    [HttpPut("{id}")]
    [RequirePermission(PermissionCodes.Roles_Update)]
    public async Task<ActionResult<Result<Guid>>> Update([FromRoute] Guid id, [FromBody] UpdateRoleCommand command)
    {
        var result = await _mediator.Send(command with { RoleId = id });
        return Ok(result);
    }

    [HttpGet("{id}")]
    [RequirePermission(PermissionCodes.Roles_View)]
    public async Task<ActionResult<Result<Guid>>> GetById([FromRoute] Guid id)
    {
        var result = await _mediator.Send(new GetRoleByIdQuery { RoleId = id });
        return Ok(result);
    }
    [HttpPost("asign-permission")]
    [RequirePermission(PermissionCodes.Roles_AssignPermission)]
    public async Task<ActionResult<Result<Guid>>> AsignPermission([FromBody] AsignPermissionCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{roleId}/permissions/{permissionId}")]
    [RequirePermission(PermissionCodes.Roles_AssignPermission)]
    public async Task<ActionResult<Result<Guid>>> RemovePermission([FromRoute] Guid roleId, [FromRoute] Guid permissionId)
    {
        var result = await _mediator.Send(new RemovePermissionFromRoleCommand { RoleId = roleId, PermissionId = permissionId });
        return Ok(result);
    }

    [HttpPut("{roleId}/permissions")]
    [RequirePermission(PermissionCodes.Roles_AssignPermission)]
    public async Task<ActionResult<Result<Guid>>> ChangePermissions([FromRoute] Guid roleId, [FromBody] ChangePermissionsForRoleCommand body)
    {
        var result = await _mediator.Send(body with { RoleId = roleId });
        return Ok(result);
    }
}