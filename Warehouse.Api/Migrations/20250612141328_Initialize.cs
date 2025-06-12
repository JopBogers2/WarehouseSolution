using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Warehouse.Api.Migrations
{
    /// <inheritdoc />
    public partial class Initialize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FulfillmentPickupPointAddresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PickupPointId = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PickupPointName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Country = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    City = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ZipCode = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AddressLine1 = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AddressLine2 = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FulfillmentPickupPointAddresses", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FulfillmentShippingAddresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FirstName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Phone = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MobilePhone = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AddressLine1 = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AddressLine2 = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ZipCode = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    City = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Country = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FulfillmentShippingAddresses", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ReturnMerchandiseAuthorizations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Platform = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Channel = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OrderId = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ReturnRequestId = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DistributionCenter = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Currency = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnMerchandiseAuthorizations", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Fulfillments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Platform = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Channel = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OrderId = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Carrier = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ServiceLevel = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ShipmentId = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ShippingAddressId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PickupPointAddressId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fulfillments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fulfillments_FulfillmentPickupPointAddresses_PickupPointAddr~",
                        column: x => x.PickupPointAddressId,
                        principalTable: "FulfillmentPickupPointAddresses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Fulfillments_FulfillmentShippingAddresses_ShippingAddressId",
                        column: x => x.ShippingAddressId,
                        principalTable: "FulfillmentShippingAddresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ReturnMerchandiseAuthorizationLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    LineId = table.Column<int>(type: "int", nullable: false),
                    ArticleCode = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Resolution = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ReturnMerchandiseAuthorizationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnMerchandiseAuthorizationLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReturnMerchandiseAuthorizationLines_ReturnMerchandiseAuthori~",
                        column: x => x.ReturnMerchandiseAuthorizationId,
                        principalTable: "ReturnMerchandiseAuthorizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TrackAndTraces",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TrackAndTraceCode = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ReturnMerchandiseAuthorizationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackAndTraces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrackAndTraces_ReturnMerchandiseAuthorizations_ReturnMerchan~",
                        column: x => x.ReturnMerchandiseAuthorizationId,
                        principalTable: "ReturnMerchandiseAuthorizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FulfillmentLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    LineId = table.Column<int>(type: "int", nullable: false),
                    DistributionCenter = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ArticleCode = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    FulfillmentType = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ShippingDate = table.Column<DateOnly>(type: "date", nullable: false),
                    FulfillmentId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FulfillmentLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FulfillmentLines_Fulfillments_FulfillmentId",
                        column: x => x.FulfillmentId,
                        principalTable: "Fulfillments",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FulfillmentReturnMerchandiseAuthorization",
                columns: table => new
                {
                    FulfillmentsId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ReturnMerchandiseAuthorizationsId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FulfillmentReturnMerchandiseAuthorization", x => new { x.FulfillmentsId, x.ReturnMerchandiseAuthorizationsId });
                    table.ForeignKey(
                        name: "FK_FulfillmentReturnMerchandiseAuthorization_Fulfillments_Fulfi~",
                        column: x => x.FulfillmentsId,
                        principalTable: "Fulfillments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FulfillmentReturnMerchandiseAuthorization_ReturnMerchandiseA~",
                        column: x => x.ReturnMerchandiseAuthorizationsId,
                        principalTable: "ReturnMerchandiseAuthorizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_FulfillmentLines_FulfillmentId",
                table: "FulfillmentLines",
                column: "FulfillmentId");

            migrationBuilder.CreateIndex(
                name: "IX_FulfillmentReturnMerchandiseAuthorization_ReturnMerchandiseA~",
                table: "FulfillmentReturnMerchandiseAuthorization",
                column: "ReturnMerchandiseAuthorizationsId");

            migrationBuilder.CreateIndex(
                name: "IX_Fulfillments_PickupPointAddressId",
                table: "Fulfillments",
                column: "PickupPointAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Fulfillments_ShippingAddressId",
                table: "Fulfillments",
                column: "ShippingAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnMerchandiseAuthorizationLines_ReturnMerchandiseAuthori~",
                table: "ReturnMerchandiseAuthorizationLines",
                column: "ReturnMerchandiseAuthorizationId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackAndTraces_ReturnMerchandiseAuthorizationId",
                table: "TrackAndTraces",
                column: "ReturnMerchandiseAuthorizationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FulfillmentLines");

            migrationBuilder.DropTable(
                name: "FulfillmentReturnMerchandiseAuthorization");

            migrationBuilder.DropTable(
                name: "ReturnMerchandiseAuthorizationLines");

            migrationBuilder.DropTable(
                name: "TrackAndTraces");

            migrationBuilder.DropTable(
                name: "Fulfillments");

            migrationBuilder.DropTable(
                name: "ReturnMerchandiseAuthorizations");

            migrationBuilder.DropTable(
                name: "FulfillmentPickupPointAddresses");

            migrationBuilder.DropTable(
                name: "FulfillmentShippingAddresses");
        }
    }
}
