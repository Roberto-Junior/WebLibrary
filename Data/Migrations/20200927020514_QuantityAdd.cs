using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BiblioTechA.Data.Migrations
{
    public partial class QuantityAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReservationDateRelease",
                table: "BookReservationHistory");

            migrationBuilder.DropColumn(
                name: "ReservationDateReturn",
                table: "BookReservationHistory");

            migrationBuilder.DropColumn(
                name: "ReservationDateRelease",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "ReservationDateReturn",
                table: "Book");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReservationDateRelease",
                table: "BookReservationHistory",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ReservationDateReturn",
                table: "BookReservationHistory",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ReservationDateRelease",
                table: "Book",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ReservationDateReturn",
                table: "Book",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReservationDateRelease",
                table: "BookReservationHistory");

            migrationBuilder.DropColumn(
                name: "ReservationDateReturn",
                table: "BookReservationHistory");

            migrationBuilder.DropColumn(
                name: "ReservationDateRelease",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "ReservationDateReturn",
                table: "Book");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReservationDateGetIN",
                table: "BookReservationHistory",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ReservationDateGetOut",
                table: "BookReservationHistory",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ReservationDateGetIN",
                table: "Book",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ReservationDateGetOut",
                table: "Book",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
