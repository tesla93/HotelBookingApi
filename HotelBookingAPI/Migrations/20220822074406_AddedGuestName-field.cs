using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelBookingAPI.Migrations
{
    public partial class AddedGuestNamefield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GuestName",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GuestName",
                table: "Bookings");
        }
    }
}
