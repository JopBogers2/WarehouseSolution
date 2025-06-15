using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Warehouse.Api.Data;
using Warehouse.Api.Entities;
using Warehouse.Shared.Models;

namespace Warehouse.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "ROLE_USER_HIP_WMS")]
    [ApiController]
    public class AppController : ControllerBase
    {
        private readonly DefaultDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public AppController(DefaultDbContext context, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("returnMerchandiseAuthorizations")]
        public async Task<ActionResult<IEnumerable<ReturnMerchandiseAuthorizationDto>>> GetAllRmas()
        {
            var rmas = await _context.ReturnMerchandiseAuthorizations
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
                        Reason = l.Reason,
                        Resolution = l.Resolution
                    }).ToList()
                })
                .ToListAsync();

            return Ok(rmas);
        }

        [HttpGet("returnMerchandiseAuthorizations/search")]
        public async Task<ActionResult<PagedResult<ReturnMerchandiseAuthorizationDto>>> SearchRmas(
            [FromQuery] string? orderId,
            [FromQuery] string? distributionCenter,
            [FromQuery] string? platform,
            [FromQuery] string? channel,
            [FromQuery] string? trackAndTrace,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20)
        {
            var query = _context.ReturnMerchandiseAuthorizations
                .Include(r => r.Lines)
                .Include(r => r.TrackAndTraces)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(orderId))
                query = query.Where(r => r.OrderId.Contains(orderId));
            if (!string.IsNullOrWhiteSpace(distributionCenter))
                query = query.Where(r => r.DistributionCenter.Contains(distributionCenter));
            if (!string.IsNullOrWhiteSpace(platform))
                query = query.Where(r => r.Platform.Contains(platform));
            if (!string.IsNullOrWhiteSpace(channel))
                query = query.Where(r => r.Channel.Contains(channel));
            if (!string.IsNullOrWhiteSpace(trackAndTrace))
                query = query.Where(r => r.TrackAndTraces.Any(t => t.TrackAndTraceCode == trackAndTrace));

            var totalCount = await query.CountAsync();

            var rmas = await query
                .OrderByDescending(r => r.ReturnRequestId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
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
                        Reason = l.Reason,
                        Resolution = l.Resolution
                    }).ToList()
                })
                .ToListAsync();

            return Ok(new PagedResult<ReturnMerchandiseAuthorizationDto>
            {
                Items = rmas,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            });
        }

        [HttpGet("returnMerchandiseAuthorization/byTrackAndTrace/{code}")]
        public async Task<ActionResult<ReturnMerchandiseAuthorizationDto>> GetRmaByTrackAndTrace(string code)
        {
            var rma = await _context.ReturnMerchandiseAuthorizations
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
                        Reason = l.Reason,
                        Resolution = l.Resolution
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (rma == null)
                return NotFound();

            return Ok(rma);
        }

        [HttpPost("book/return")]
        public async Task<ActionResult<ReturnMerchandiseAuthorizationDto>> PostReturn(ReturnDto returnDto)
        {
            if (returnDto == null)
                return BadRequest("Return data cannot be null.");

            var existingReturn = await FindExistingReturnAsync(returnDto);

            if (existingReturn != null)
            {
                return BadRequest("A return with the same details already exists.");
            }

            var newReturn = CreateReturn(returnDto);

            var relatedFulfillments = await _context.Fulfillments
                .Where(f =>
                    f.Platform == returnDto.Platform &&
                    f.Channel == returnDto.Channel &&
                    f.OrderId == returnDto.OrderId)
                .ToListAsync();

            newReturn.Fulfillments = relatedFulfillments;
            foreach (var fulfillment in relatedFulfillments)
            {
                fulfillment.Returns.Add(newReturn);
            }

            var relatedRmas = await _context.ReturnMerchandiseAuthorizations
                .Where(r =>
                    r.Platform == returnDto.Platform &&
                    r.Channel == returnDto.Channel &&
                    r.OrderId == returnDto.OrderId &&
                    r.ReturnRequestId == returnDto.ReturnRequestId)
                .ToListAsync();

            newReturn.ReturnMerchandiseAuthorizations = relatedRmas;
            foreach (var rma in relatedRmas)
            {
                rma.Returns.Add(newReturn);
            }

            _context.Returns.Add(newReturn);
            await _context.SaveChangesAsync();

            // External message sending
            var jwtToken = await GetJwtTokenAsync();
            if (string.IsNullOrEmpty(jwtToken))
            {
                return StatusCode(500, "Failed to authenticate with external service.");
            }

            var messageSent = await SendReturnMessageAsync(returnDto, jwtToken);
            if (!messageSent)
            {
                return StatusCode(500, "Failed to send message to external service.");
            }

            return Ok(returnDto);
        }

        private async Task<Return?> FindExistingReturnAsync(ReturnDto dto)
        {
            return await _context.Returns
                .FirstOrDefaultAsync(r =>
                    r.Platform == dto.Platform &&
                    r.Channel == dto.Channel &&
                    r.OrderId == dto.OrderId &&
                    r.ReturnRequestId == dto.ReturnRequestId);
        }

        private Return CreateReturn(ReturnDto returnDto)
        {
            return new Return
            {
                Platform = returnDto.Platform,
                Channel = returnDto.Channel,
                OrderId = returnDto.OrderId,
                ReturnRequestId = returnDto.ReturnRequestId,
                Lines = returnDto.Lines.Select(line => new ReturnLine
                {
                    LineId = line.LineId,
                    ArticleCode = line.ArticleCode,
                    Quantity = line.Quantity,
                    DistributionCenter = line.DistributionCenter,
                    Reason = line.Reason,
                    Condition = line.Condition
                }).ToList()
            };
        }

        private async Task<string?> GetJwtTokenAsync()
        {
            var username = _configuration["Auth:Username"];
            var password = _configuration["Auth:Password"];
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return null;

            var client = _httpClientFactory.CreateClient();
            var byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            var authUrl = "http://dev.auth.hip.fittinq.com/api/v1/authenticate";
            var response = await client.GetAsync(authUrl);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadAsStringAsync();
        }

        private async Task<bool> SendReturnMessageAsync(ReturnDto returnDto, string jwtToken)
        {
            returnDto.EventType = "upsert";

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            var messageUrl = "http://localhost:8084/api/v1/message/wms/return";
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var json = JsonSerializer.Serialize(returnDto, options);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(messageUrl, content);
            return response.IsSuccessStatusCode;
        }

    }
}
