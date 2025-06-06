using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Warehouse.Api.Data;
using Warehouse.Shared.Models;

namespace Warehouse.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "ROLE_USER_HIP_WMS")]
    [ApiController]
    public class AppController : ControllerBase
    {
        private readonly DefaultDbContext _context;

        public AppController(DefaultDbContext context)
        {
            _context = context;
        }

        [HttpGet("returnMerchandiseAuthorizations")]
        public async Task<ActionResult<IEnumerable<ReturnMerchandiseAuthorizationDto>>> GetAllRmas()
        {
            var rmas = await _context.ReturnMerchandiseAuthorization
                .Include(r => r.Lines)
                .Include(r => r.TrackAndTraces)
                .Select(r => new ReturnMerchandiseAuthorizationDto
                {
                    Platform = r.Platform,
                    Channel = r.Channel,
                    OrderId = r.OrderId,
                    ReturnRequestId = r.ReturnRequestId,
                    DistributionCenter = r.DistributionCenter,
                    Currency = r.Currency,
                    TrackAndTrace = r.TrackAndTraces.Select(t => t.TrackAndTraceCode).ToList(),
                    Lines = r.Lines.Select(l => new ReturnMerchandiseAuthorizationLineDto
                    {
                        LineId = l.LineId,
                        ArticleCode = l.ArticleCode,
                        Quantity = l.Quantity,
                        Reason = l.Reason
                    }).ToList()
                })
                .ToListAsync();

            return Ok(rmas);
        }

        [HttpGet("returnMerchandiseAuthorization/byTrackAndTrace/{code}")]
        public async Task<ActionResult<ReturnMerchandiseAuthorizationDto>> GetRmaByTrackAndTrace(string code)
        {
            var rma = await _context.ReturnMerchandiseAuthorization
                .Include(r => r.Lines)
                .Include(r => r.TrackAndTraces)
                .Where(r => r.TrackAndTraces.Any(t => t.TrackAndTraceCode == code))
                .Select(r => new ReturnMerchandiseAuthorizationDto
                {
                    Platform = r.Platform,
                    Channel = r.Channel,
                    OrderId = r.OrderId,
                    ReturnRequestId = r.ReturnRequestId,
                    DistributionCenter = r.DistributionCenter,
                    Currency = r.Currency,
                    TrackAndTrace = r.TrackAndTraces.Select(t => t.TrackAndTraceCode).ToList(),
                    Lines = r.Lines.Select(l => new ReturnMerchandiseAuthorizationLineDto
                    {
                        LineId = l.LineId,
                        ArticleCode = l.ArticleCode,
                        Quantity = l.Quantity,
                        Reason = l.Reason
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (rma == null)
                return NotFound();

            return Ok(rma);
        }
    }
}
