using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductsCatalog.Application.Common.Models;
using ProductsCatalog.Application.Features.Customers;
using ProductsCatalog.Application.Features.Customers.Commands.CreateCustomer;
using ProductsCatalog.Application.Features.Customers.Commands.DeleteCustomer;
using ProductsCatalog.Application.Features.Customers.Commands.UpdateCustomer;
using ProductsCatalog.Application.Features.Customers.Queries.GetCustomerById;
using ProductsCatalog.Application.Features.Customers.Queries.GetCustomers;
using ProductsCatalog.Domain.Enums;

namespace ProductsCatalog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController(ISender sender) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = nameof(ERole.Administrator))]
    public async Task<ActionResult<Guid>> CreateAsync([FromBody] CreateCustomerCommand command)
    {
        var id = await sender.Send(command);
        return CreatedAtRoute("GetCustomerById", new { id }, id);
    }

    [HttpGet("{id:guid}", Name = "GetCustomerById")]
    public async Task<ActionResult<CustomerDto>> ReadAsync(Guid id)
    {
        var customer = await sender.Send(new GetCustomerByIdQuery(id));
        return customer is null ? NotFound() : Ok(customer);
    }

    public sealed record UpdateCustomerRequest(string Name, string Email);

    [HttpPut("{id:guid}")]
    [Authorize(Roles = nameof(ERole.Administrator))]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateCustomerRequest request)
    {
        await sender.Send(new UpdateCustomerCommand(id, request.Name, request.Email));
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = nameof(ERole.Administrator))]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        await sender.Send(new DeleteCustomerCommand(id));
        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<CustomerDto>>> FindAsync(
        [FromQuery] string? filter, [FromQuery] int skip = 0, [FromQuery] int take = 20)
    {
        var result = await sender.Send(new GetCustomersQuery(filter, skip, take));
        return Ok(result);
    }
}
