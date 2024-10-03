using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace invelli.Migrations
{
    /// <inheritdoc />
    public partial class update_table_POand_POD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_SupplierID",
                table: "PurchaseOrders",
                column: "SupplierID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderDetails_ProductID",
                table: "PurchaseOrderDetails",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderDetails_PurchaseOrderID",
                table: "PurchaseOrderDetails",
                column: "PurchaseOrderID");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderDetails_Products_ProductID",
                table: "PurchaseOrderDetails",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderDetails_PurchaseOrders_PurchaseOrderID",
                table: "PurchaseOrderDetails",
                column: "PurchaseOrderID",
                principalTable: "PurchaseOrders",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_Suppliers_SupplierID",
                table: "PurchaseOrders",
                column: "SupplierID",
                principalTable: "Suppliers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderDetails_Products_ProductID",
                table: "PurchaseOrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderDetails_PurchaseOrders_PurchaseOrderID",
                table: "PurchaseOrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_Suppliers_SupplierID",
                table: "PurchaseOrders");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_SupplierID",
                table: "PurchaseOrders");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrderDetails_ProductID",
                table: "PurchaseOrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrderDetails_PurchaseOrderID",
                table: "PurchaseOrderDetails");
        }
    }
}
