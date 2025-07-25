﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PurchaseManager.Api.Migrations
{
    /// <inheritdoc />
    public partial class repository_patch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discount",
                table: "RawMaterialSupplierOrders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Discount",
                table: "RawMaterialSupplierOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
