using System.Net;
using AutoMapper;
using Domain.DTOs.RoleDto;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.RoleService;

public class RoleService(DataContext context, ILogger<RoleService> logger, IMapper mapper):IRoleService
{
    public async Task<Response<string>> AddRoleAsync(AddRoleDto addRoleDto)
    {
        try
        {
            logger.LogInformation("AddRole method started at {DateTime}", DateTime.Now);
            var existing = await context.Roles.AnyAsync(e => e.Name == addRoleDto.Name);
            if (existing) return new Response<string>(HttpStatusCode.BadRequest, "Role already exists!");
            var mapped = mapper.Map<Role>(addRoleDto);
            await context.Roles.AddAsync(mapped);
            await context.SaveChangesAsync();
            logger.LogInformation("AddRole method finished at {DateTime}", DateTime.Now);
            return new Response<string>("Added successfully!");
        }
        catch (Exception ex)
        {
            logger.LogError("Error in code, {DateTime}", DateTime.Now);
            return new Response<string>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<Response<bool>> DeleteRoleAsync(int id)
    {
        try
        {
            logger.LogInformation("AddRole method started at {DateTime}", DateTime.Now);
            var existing = await context.Roles.Where(e => e.Id == id).ExecuteDeleteAsync();
            if (existing == 0)
            {
                logger.LogWarning("Role not found {Id} at {DateTime}", id, DateTime.Now);
                return new Response<bool>(HttpStatusCode.BadRequest, "Role not found!");
            }
            logger.LogInformation("AddRole method finished at {DateTime}", DateTime.Now);
            return new Response<bool>(true);
        }
        catch (Exception ex)
        {
            logger.LogError("Error in code, {DateTime}", DateTime.Now);
            return new Response<bool>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<PagedResponse<List<GetRoleDto>>> GetRolesAsync(RoleFilter filter)
    {
        try
        {
            logger.LogInformation("AddRole method started at {DateTime}", DateTime.Now);
            var roles = context.Roles.AsQueryable();
            if (!string.IsNullOrEmpty(filter.Name))
                roles = roles.Where(x => x.Name.ToLower().Contains(filter.Name.ToLower()));
            var result = await roles.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();
            var total = await roles.CountAsync();

            var response = mapper.Map<List<GetRoleDto>>(result);
            logger.LogInformation("AddRole method finished at {DateTime}", DateTime.Now);
            return new PagedResponse<List<GetRoleDto>>(response, total, filter.PageNumber, filter.PageSize);
        }
        catch (Exception e)
        {
            logger.LogError("Error in code, {DateTime}", DateTime.Now);
            return new PagedResponse<List<GetRoleDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<GetRoleDto>> GetRoleByIdAsync(int id)
    {
        try
        {

            logger.LogInformation("AddRole method started at {DateTime}", DateTime.Now);
            var existing = await context.Roles.FirstOrDefaultAsync(x => x.Id == id);
            if (existing == null) { 
                logger.LogWarning("Role not found {Id} at {DateTime}", id, DateTime.Now);
                return new Response<GetRoleDto>(HttpStatusCode.BadRequest, "Role not found"); }
            var roles = mapper.Map<GetRoleDto>(existing);
            logger.LogInformation("AddRole method finished at {DateTime}", DateTime.Now);
            return new Response<GetRoleDto>(roles);
        }
        catch (Exception e)
        {
            logger.LogError("Error in code, {DateTime}", DateTime.Now);
            return new Response<GetRoleDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<string>> UpdateRoleAsync(UpdateRoleDto updateRoleDto)
    {
        try
        {
            logger.LogInformation("AddRole method started at {DateTime}", DateTime.Now);
            var existing = await context.Roles.AnyAsync(e => e.Id == updateRoleDto.Id);
            if (!existing) return new Response<string>(HttpStatusCode.BadRequest, "Role not found!");
            var mapped = mapper.Map<Role>(updateRoleDto);
            context.Roles.Update(mapped);
            await context.SaveChangesAsync();
            logger.LogInformation("AddRole method finished at {DateTime}", DateTime.Now);
            return new Response<string>("Updated successfully");
        }
        catch (Exception ex)
        {
            logger.LogError("Error in code, {DateTime}", DateTime.Now);
            return new Response<string>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }
}
