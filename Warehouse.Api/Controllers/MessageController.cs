using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Warehouse.Api.Data;
using Warehouse.Api.Entities;
using Warehouse.Shared.Models;

namespace Warehouse.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "ROLE_API_AMQ_MESSAGE_WMS")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly DefaultDbContext _context;

        public MessageController(DefaultDbContext context)
        {
            _context = context;
        }

        [HttpPost("crm/returnMerchandiseAuthorization")]
        public async Task<IActionResult> UpsertRma([FromBody] ReturnMerchandiseAuthorizationDto dto)
        {
            if (dto == null)
                return BadRequest();

            var existing = await FindExistingRmaAsync(dto);

            if (existing != null)
            {
                UpdateRma(existing, dto);
                await _context.SaveChangesAsync();
                return Ok("Updated");
            }
            else
            {
                var newRma = CreateRma(dto);
                _context.ReturnMerchandiseAuthorization.Add(newRma);
                await _context.SaveChangesAsync();
                return Ok("Created");
            }
        }

        private async Task<ReturnMerchandiseAuthorization?> FindExistingRmaAsync(ReturnMerchandiseAuthorizationDto dto)
        {
            return await _context.ReturnMerchandiseAuthorization
                .Include(r => r.Lines)
                .Include(r => r.TrackAndTraces)
                .FirstOrDefaultAsync(r =>
                    r.Platform == dto.Platform &&
                    r.Channel == dto.Channel &&
                    r.OrderId == dto.OrderId &&
                    r.ReturnRequestId == dto.ReturnRequestId);
        }

        private void UpdateRma(ReturnMerchandiseAuthorization existing, ReturnMerchandiseAuthorizationDto dto)
        {
            existing.DistributionCenter = dto.DistributionCenter;
            existing.Currency = dto.Currency;

            // Update TrackAndTraces
            _context.TrackAndTraces.RemoveRange(existing.TrackAndTraces);
            existing.TrackAndTraces = dto.TrackAndTrace
                .Select(code => new TrackAndTrace { TrackAndTraceCode = code })
                .ToList();

            // Update Lines
            _context.ReturnMerchandiseAuthorizationLines.RemoveRange(existing.Lines);
            existing.Lines = dto.Lines
                .Select(line => new ReturnMerchandiseAuthorizationLine
                {
                    LineId = line.LineId,
                    ArticleCode = line.ArticleCode,
                    Quantity = line.Quantity,
                    Reason = line.Reason
                })
                .ToList();
        }

        private ReturnMerchandiseAuthorization CreateRma(ReturnMerchandiseAuthorizationDto dto)
        {
            return new ReturnMerchandiseAuthorization
            {
                Platform = dto.Platform,
                Channel = dto.Channel,
                OrderId = dto.OrderId,
                ReturnRequestId = dto.ReturnRequestId,
                DistributionCenter = dto.DistributionCenter,
                Currency = dto.Currency,
                TrackAndTraces = dto.TrackAndTrace
                    .Select(code => new TrackAndTrace { TrackAndTraceCode = code })
                    .ToList(),
                Lines = dto.Lines
                    .Select(line => new ReturnMerchandiseAuthorizationLine
                    {
                        LineId = line.LineId,
                        ArticleCode = line.ArticleCode,
                        Quantity = line.Quantity,
                        Reason = line.Reason
                    })
                    .ToList()
            };
        }
    }
}
