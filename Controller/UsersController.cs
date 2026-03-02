using FirstProject.Application.Common;
using FirstProject.Application.Common.Permissions;
using FirstProject.Application.Features.Users.Commands.AsignGroup;
using FirstProject.Application.Features.Users.Commands.AsignRole;
using FirstProject.Application.Features.Users.Commands.ChangeGroup;
using FirstProject.Application.Features.Users.Commands.ChangeRole;
using FirstProject.Application.Features.Users.Commands.CreateUser;
using FirstProject.Application.Features.Users.Commands.DeleteUser;
using FirstProject.Application.Features.Users.Commands.RemoveGroup;
using FirstProject.Application.Features.Users.Commands.RemoveRole;
using FirstProject.Application.Features.Users.Commands.UpdateUser;
using FirstProject.Application.Features.Users.Queries.GetAllUsers;
using FirstProject.Application.Features.Users.Queries.GetUserById;
using FirstProject.Application.Features.Users.Queries.GetUsersByRole;
using FirstProject.Domain.Entities;
using FirstProject.Infrastructure.Auth.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [RequirePermission(PermissionCodes.Users_Create)]
    public async Task<ActionResult<Result<Guid>>> Create([FromBody] CreateUserCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [RequirePermission(PermissionCodes.Users_Delete)]
    public async Task<ActionResult<Result<Guid>>> Delete([FromRoute] Guid id)
    {
        var result = await _mediator.Send(new DeleteUserCommand { UserID = id });
        return Ok(result);
    }


    [HttpPut("{id}")]
    [RequirePermission(PermissionCodes.Users_Update)]
    public async Task<ActionResult<Result<Guid>>> Update([FromRoute] Guid id, [FromBody] UpdateUserCommand command)
    {
        var result = await _mediator.Send(command with { UserID = id });
        return Ok(result);
    }

    [HttpGet("{id}")]
    [RequirePermission(PermissionCodes.Users_View)]
    public async Task<ActionResult<Result<Guid>>> GetById([FromRoute] Guid id)
    {
        var result = await _mediator.Send(new GetUserByIdQuery { UserId = id });
        return Ok(result);
    }

    [HttpPost("asign-role")]
    [RequirePermission(PermissionCodes.Users_Update)]
    public async Task<ActionResult<Result<Guid>>> AsignRole([FromBody] AsignRoleCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("asign-group")]
    [RequirePermission(PermissionCodes.Users_Update)]
    public async Task<ActionResult<Result<Guid>>> AsignGroup([FromBody] AsignGroupCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{userId}/roles/{roleId}")]
    [RequirePermission(PermissionCodes.Users_Update)]
    public async Task<ActionResult<Result<Guid>>> RemoveRole([FromRoute] Guid userId, [FromRoute] Guid roleId)
    {
        var result = await _mediator.Send(new RemoveRoleFromUserCommand { UserId = userId, RoleId = roleId });
        return Ok(result);
    }

    [HttpDelete("{userId}/groups/{groupId}")]
    [RequirePermission(PermissionCodes.Users_Update)]
    public async Task<ActionResult<Result<Guid>>> RemoveGroup([FromRoute] Guid userId, [FromRoute] Guid groupId)
    {
        var result = await _mediator.Send(new RemoveGroupFromUserCommand { UserId = userId, GroupId = groupId });
        return Ok(result);
    }

    [HttpPut("{userId}/roles")]
    [RequirePermission(PermissionCodes.Users_Update)]
    public async Task<ActionResult<Result<Guid>>> ChangeRoles([FromRoute] Guid userId, [FromBody] ChangeRolesForUserCommand body)
    {
        var result = await _mediator.Send(body with { UserId = userId });
        return Ok(result);
    }

    [HttpPut("{userId}/groups")]
    [RequirePermission(PermissionCodes.Users_Update)]
    public async Task<ActionResult<Result<Guid>>> ChangeGroups([FromRoute] Guid userId, [FromBody] ChangeGroupsForUserCommand body)
    {
        var result = await _mediator.Send(body with { UserId = userId });
        return Ok(result);
    }

    [HttpGet]
    [RequirePermission(PermissionCodes.Users_List)]
    public async Task<ActionResult<Result<IReadOnlyList<User>>>> GetAll()
    {
        var result = await _mediator.Send(new GetAllUsersQuery());
        return Ok(result);
    }
    [HttpGet("role/{roleName}")]
    [RequirePermission(PermissionCodes.Users_List)]
    public async Task<ActionResult<Result<IReadOnlyList<User>>>> GetUsersByRole([FromRoute] string roleName)
    {
        var result = await _mediator.Send(new GetUsersByRoleQuery { RoleName = roleName });
        return Ok(result);
    }
}