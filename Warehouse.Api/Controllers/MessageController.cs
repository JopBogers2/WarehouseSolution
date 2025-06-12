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

        [HttpPost("wms/fulfillment")]
        public async Task<IActionResult> UpsertFulfillment([FromBody] FulfillmentDto dto)
        {
            if (dto == null)
                return BadRequest();

            var existing = await FindExistingFulfillmentAsync(dto);

            var relatedRmas = await _context.ReturnMerchandiseAuthorizations
                .Where(r =>
                    r.Platform == dto.Platform &&
                    r.Channel == dto.Channel &&
                    r.OrderId == dto.OrderId)
                .ToListAsync();

            if (existing != null)
            {
                UpdateFulfillment(existing, dto);

                existing.ReturnMerchandiseAuthorizations = relatedRmas;

                foreach (var rma in relatedRmas)
                {
                    if (!rma.Fulfillments.Contains(existing))
                        rma.Fulfillments.Add(existing);
                }

                await _context.SaveChangesAsync();
                return Ok("Updated");
            }
            else
            {
                var newFulfillment = CreateFulfillment(dto);

                newFulfillment.ReturnMerchandiseAuthorizations = relatedRmas;
                foreach (var rma in relatedRmas)
                {
                    if (!rma.Fulfillments.Contains(newFulfillment))
                        rma.Fulfillments.Add(newFulfillment);
                }

                _context.Fulfillments.Add(newFulfillment);
                await _context.SaveChangesAsync();
                return Ok("Created");
            }
        }

        [HttpPost("crm/returnMerchandiseAuthorization")]
        public async Task<IActionResult> UpsertRma([FromBody] ReturnMerchandiseAuthorizationDto dto)
        {
            if (dto == null)
                return BadRequest();

            var existing = await FindExistingRmaAsync(dto);

            var relatedFulfillments = await _context.Fulfillments
            .Where(f =>
                f.Platform == dto.Platform &&
                f.Channel == dto.Channel &&
                f.OrderId == dto.OrderId)
            .ToListAsync();

            if (existing != null)
            {
                UpdateRma(existing, dto);

                existing.Fulfillments = relatedFulfillments;
                foreach (var fulfillment in relatedFulfillments)
                {
                    if (!fulfillment.ReturnMerchandiseAuthorizations.Contains(existing))
                        fulfillment.ReturnMerchandiseAuthorizations.Add(existing);
                }

                await _context.SaveChangesAsync();
                return Ok("Updated");
            }
            else
            {
                var newRma = CreateRma(dto);

                newRma.Fulfillments = relatedFulfillments;
                foreach (var fulfillment in relatedFulfillments)
                {
                    if (!fulfillment.ReturnMerchandiseAuthorizations.Contains(newRma))
                        fulfillment.ReturnMerchandiseAuthorizations.Add(newRma);
                }

                _context.ReturnMerchandiseAuthorizations.Add(newRma);
                await _context.SaveChangesAsync();
                return Ok("Created");
            }
        }

        private async Task<Fulfillment?> FindExistingFulfillmentAsync(FulfillmentDto dto)
        {
            return await _context.Fulfillments
                .Include(r => r.ShippingAddress)
                .Include(r => r.PickupPointAddress)
                .Include(r => r.Lines)
                .FirstOrDefaultAsync(r =>
                    r.Platform == dto.Platform &&
                    r.Channel == dto.Channel &&
                    r.OrderId == dto.OrderId &&
                    r.ShipmentId == dto.ShipmentId);
        }

        private void UpdateFulfillment(Fulfillment existing, FulfillmentDto dto)
        {
            existing.Email = dto.Email;
            existing.Carrier = dto.Carrier;
            existing.ServiceLevel = dto.ServiceLevel;

            // Update Shipping Address
            existing.ShippingAddress.FirstName = dto.ShippingAddress.FirstName;
            existing.ShippingAddress.LastName = dto.ShippingAddress.LastName;
            existing.ShippingAddress.CompanyName = dto.ShippingAddress.CompanyName;
            existing.ShippingAddress.Phone = dto.ShippingAddress.Phone;
            existing.ShippingAddress.MobilePhone = dto.ShippingAddress.MobilePhone;
            existing.ShippingAddress.AddressLine1 = dto.ShippingAddress.AddressLine1;
            existing.ShippingAddress.AddressLine2 = dto.ShippingAddress.AddressLine2;
            existing.ShippingAddress.ZipCode = dto.ShippingAddress.ZipCode;
            existing.ShippingAddress.City = dto.ShippingAddress.City;
            existing.ShippingAddress.Country = dto.ShippingAddress.Country;

            // Update Pickup Point Address if provided
            if (dto.PickupPointAddress != null)
            {
                if (existing.PickupPointAddress == null)
                {
                    existing.PickupPointAddress = new FulfillmentPickupPointAddress();
                }
                existing.PickupPointAddress.PickupPointId = dto.PickupPointAddress.PickupPointId;
                existing.PickupPointAddress.PickupPointName = dto.PickupPointAddress.PickupPointName;
                existing.PickupPointAddress.Country = dto.PickupPointAddress.Country;
                existing.PickupPointAddress.City = dto.PickupPointAddress.City;
                existing.PickupPointAddress.ZipCode = dto.PickupPointAddress.ZipCode;
                existing.PickupPointAddress.AddressLine1 = dto.PickupPointAddress.AddressLine1;
                existing.PickupPointAddress.AddressLine2 = dto.PickupPointAddress.AddressLine2;
            }
            else
            {
                existing.PickupPointAddress = null;
            }

            // Update Lines
            _context.FulfillmentLines.RemoveRange(existing.Lines);
            existing.Lines = dto.Lines
                .Select(line => new FulfillmentLine
                {
                    LineId = line.LineId,
                    DistributionCenter = line.DistributionCenter,
                    ArticleCode = line.ArticleCode,
                    Quantity = line.Quantity,
                    FulfillmentType = line.FulfillmentType,
                    ShippingDate = line.ShippingDate
                })
                .ToList();
        }

        private Fulfillment CreateFulfillment(FulfillmentDto dto)
        {
            return new Fulfillment
            {
                Platform = dto.Platform,
                Channel = dto.Channel,
                OrderId = dto.OrderId,
                Email = dto.Email,
                Carrier = dto.Carrier,
                ServiceLevel = dto.ServiceLevel,
                ShipmentId = dto.ShipmentId,
                ShippingAddress = new FulfillmentShippingAddress
                {
                    FirstName = dto.ShippingAddress.FirstName,
                    LastName = dto.ShippingAddress.LastName,
                    CompanyName = dto.ShippingAddress.CompanyName,
                    Phone = dto.ShippingAddress.Phone,
                    MobilePhone = dto.ShippingAddress.MobilePhone,
                    AddressLine1 = dto.ShippingAddress.AddressLine1,
                    AddressLine2 = dto.ShippingAddress.AddressLine2,
                    ZipCode = dto.ShippingAddress.ZipCode,
                    City = dto.ShippingAddress.City,
                    Country = dto.ShippingAddress.Country
                },
                PickupPointAddress = dto.PickupPointAddress != null ? new FulfillmentPickupPointAddress
                {
                    PickupPointId = dto.PickupPointAddress.PickupPointId,
                    PickupPointName = dto.PickupPointAddress.PickupPointName,
                    Country = dto.PickupPointAddress.Country,
                    City = dto.PickupPointAddress.City,
                    ZipCode = dto.PickupPointAddress.ZipCode,
                    AddressLine1 = dto.PickupPointAddress.AddressLine1,
                    AddressLine2 = dto.PickupPointAddress.AddressLine2
                } : null,
                Lines = dto.Lines
                    .Select(line => new FulfillmentLine
                    {
                        LineId = line.LineId,
                        DistributionCenter = line.DistributionCenter,
                        ArticleCode = line.ArticleCode,
                        Quantity = line.Quantity,
                        FulfillmentType = line.FulfillmentType,
                        ShippingDate = line.ShippingDate
                    })
                    .ToList()
            };

        }

        private async Task<ReturnMerchandiseAuthorization?> FindExistingRmaAsync(ReturnMerchandiseAuthorizationDto dto)
        {
            return await _context.ReturnMerchandiseAuthorizations
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
