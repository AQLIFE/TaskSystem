using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManangerSystem.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "category_table",
                columns: table => new
                {
                    category_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    parent_category_id = table.Column<Guid>(type: "char(36)", nullable: true),
                    sort_serial = table.Column<int>(type: "int", nullable: false),
                    category_name = table.Column<string>(type: "longtext", nullable: false),
                    category_level = table.Column<int>(type: "int", nullable: false),
                    Remark = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_category_table", x => x.category_id);
                    table.ForeignKey(
                        name: "FK_category_table_category_table_parent_category_id",
                        column: x => x.parent_category_id,
                        principalTable: "category_table",
                        principalColumn: "category_id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "employee_system_account_table",
                columns: table => new
                {
                    employee_electronic_account_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    alias = table.Column<string>(type: "longtext", nullable: false),
                    employee_pwd = table.Column<string>(type: "longtext", nullable: false),
                    account_permission = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employee_system_account_table", x => x.employee_electronic_account_id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "encryption",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false),
                    encryption_id = table.Column<string>(type: "longtext", nullable: false),
                    alias = table.Column<string>(type: "longtext", nullable: false),
                    pwd = table.Column<string>(type: "longtext", nullable: false),
                    account_permission = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "inventoryInfos",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(type: "char(36)", nullable: false),
                    ProductName = table.Column<string>(type: "longtext", nullable: false),
                    ProductPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProductCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProductModel = table.Column<string>(type: "longtext", nullable: false),
                    ProductType = table.Column<Guid>(type: "char(36)", nullable: false),
                    CategoryId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventoryInfos", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_inventoryInfos_category_table_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "category_table",
                        principalColumn: "category_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "task_customer_table",
                columns: table => new
                {
                    customer_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    customer_name = table.Column<string>(type: "longtext", nullable: false),
                    customer_contact_way = table.Column<string>(type: "longtext", nullable: true),
                    customer_address = table.Column<string>(type: "longtext", nullable: true),
                    customer_type = table.Column<Guid>(type: "char(36)", nullable: false),
                    client_grade = table.Column<int>(type: "int", nullable: false),
                    add_time = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task_customer_table", x => x.customer_id);
                    table.ForeignKey(
                        name: "FK_task_customer_table_category_table_customer_type",
                        column: x => x.customer_type,
                        principalTable: "category_table",
                        principalColumn: "category_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "employee_real_info_table",
                columns: table => new
                {
                    EmployeeElectronicAccountId = table.Column<Guid>(type: "char(36)", nullable: false),
                    EmployeeName = table.Column<string>(type: "longtext", nullable: false),
                    EmployeeContactWay = table.Column<string>(type: "longtext", nullable: false),
                    EmployeeSystemAccountEmployeeId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employee_real_info_table", x => x.EmployeeElectronicAccountId);
                    table.ForeignKey(
                        name: "FK_employee_real_info_table_employee_system_account_table_Emplo~",
                        column: x => x.EmployeeSystemAccountEmployeeId,
                        principalTable: "employee_system_account_table",
                        principalColumn: "employee_electronic_account_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "task_table",
                columns: table => new
                {
                    TaskId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Time = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    customer_id = table.Column<Guid>(type: "char(36)", nullable: true),
                    Content = table.Column<string>(type: "longtext", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "char(36)", nullable: false),
                    EmployeeAccountEmployeeId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task_table", x => x.TaskId);
                    table.ForeignKey(
                        name: "FK_task_table_employee_system_account_table_EmployeeAccountEmpl~",
                        column: x => x.EmployeeAccountEmployeeId,
                        principalTable: "employee_system_account_table",
                        principalColumn: "employee_electronic_account_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_task_table_task_customer_table_customer_id",
                        column: x => x.customer_id,
                        principalTable: "task_customer_table",
                        principalColumn: "customer_id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "inOutStocks",
                columns: table => new
                {
                    ListId = table.Column<Guid>(type: "char(36)", nullable: false),
                    ChangeTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    InOutStockType = table.Column<string>(type: "longtext", nullable: false),
                    MaterialQuantity = table.Column<int>(type: "int", nullable: false),
                    Remark = table.Column<string>(type: "longtext", nullable: false),
                    ProductId = table.Column<Guid>(type: "char(36)", nullable: false),
                    TaskId = table.Column<Guid>(type: "char(36)", nullable: false),
                    CustomerId = table.Column<Guid>(type: "char(36)", nullable: true),
                    EmployeeId = table.Column<Guid>(type: "char(36)", nullable: false),
                    InventoryInfoProductId = table.Column<Guid>(type: "char(36)", nullable: false),
                    TaskCustomerCustomerId = table.Column<Guid>(type: "char(36)", nullable: false),
                    EmployeeSystemAccountEmployeeId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inOutStocks", x => x.ListId);
                    table.ForeignKey(
                        name: "FK_inOutStocks_employee_system_account_table_EmployeeSystemAcco~",
                        column: x => x.EmployeeSystemAccountEmployeeId,
                        principalTable: "employee_system_account_table",
                        principalColumn: "employee_electronic_account_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_inOutStocks_inventoryInfos_InventoryInfoProductId",
                        column: x => x.InventoryInfoProductId,
                        principalTable: "inventoryInfos",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_inOutStocks_task_customer_table_TaskCustomerCustomerId",
                        column: x => x.TaskCustomerCustomerId,
                        principalTable: "task_customer_table",
                        principalColumn: "customer_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_inOutStocks_task_table_TaskId",
                        column: x => x.TaskId,
                        principalTable: "task_table",
                        principalColumn: "TaskId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tracks",
                columns: table => new
                {
                    task_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    TaskStatus = table.Column<string>(type: "longtext", nullable: false),
                    TaskTrackTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Remark = table.Column<string>(type: "longtext", nullable: false),
                    TaskId = table.Column<Guid>(type: "char(36)", nullable: false),
                    EmployeeAccountEmployeeId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tracks", x => x.task_id);
                    table.ForeignKey(
                        name: "FK_tracks_employee_system_account_table_EmployeeAccountEmployee~",
                        column: x => x.EmployeeAccountEmployeeId,
                        principalTable: "employee_system_account_table",
                        principalColumn: "employee_electronic_account_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tracks_task_table_TaskId",
                        column: x => x.TaskId,
                        principalTable: "task_table",
                        principalColumn: "TaskId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_category_table_parent_category_id",
                table: "category_table",
                column: "parent_category_id");

            migrationBuilder.CreateIndex(
                name: "IX_employee_real_info_table_EmployeeSystemAccountEmployeeId",
                table: "employee_real_info_table",
                column: "EmployeeSystemAccountEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_inOutStocks_EmployeeSystemAccountEmployeeId",
                table: "inOutStocks",
                column: "EmployeeSystemAccountEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_inOutStocks_InventoryInfoProductId",
                table: "inOutStocks",
                column: "InventoryInfoProductId");

            migrationBuilder.CreateIndex(
                name: "IX_inOutStocks_TaskCustomerCustomerId",
                table: "inOutStocks",
                column: "TaskCustomerCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_inOutStocks_TaskId",
                table: "inOutStocks",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_inventoryInfos_CategoryId",
                table: "inventoryInfos",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_task_customer_table_customer_type",
                table: "task_customer_table",
                column: "customer_type");

            migrationBuilder.CreateIndex(
                name: "IX_task_table_EmployeeAccountEmployeeId",
                table: "task_table",
                column: "EmployeeAccountEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_task_table_customer_id",
                table: "task_table",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_tracks_EmployeeAccountEmployeeId",
                table: "tracks",
                column: "EmployeeAccountEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_tracks_TaskId",
                table: "tracks",
                column: "TaskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "employee_real_info_table");

            migrationBuilder.DropTable(
                name: "encryption");

            migrationBuilder.DropTable(
                name: "inOutStocks");

            migrationBuilder.DropTable(
                name: "tracks");

            migrationBuilder.DropTable(
                name: "inventoryInfos");

            migrationBuilder.DropTable(
                name: "task_table");

            migrationBuilder.DropTable(
                name: "employee_system_account_table");

            migrationBuilder.DropTable(
                name: "task_customer_table");

            migrationBuilder.DropTable(
                name: "category_table");
        }
    }
}
