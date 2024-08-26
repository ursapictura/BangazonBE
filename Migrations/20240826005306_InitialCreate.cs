using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BangazonBE.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirebaseKey = table.Column<string>(type: "text", nullable: true),
                    Username = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BuyerId = table.Column<int>(type: "integer", nullable: false),
                    PaymentTypeId = table.Column<int>(type: "integer", nullable: true),
                    OrderDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Closed = table.Column<bool>(type: "boolean", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Users_BuyerId",
                        column: x => x.BuyerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SellerId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    DatePosted = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Image = table.Column<string>(type: "text", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_Users_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderProduct",
                columns: table => new
                {
                    OrdersId = table.Column<int>(type: "integer", nullable: false),
                    ProductsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderProduct", x => new { x.OrdersId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_OrderProduct_Orders_OrdersId",
                        column: x => x.OrdersId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderProduct_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Paintball" },
                    { 2, "School Supplies" },
                    { 3, "Costumes" },
                    { 4, "Black Market" }
                });

            migrationBuilder.InsertData(
                table: "PaymentTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Card" },
                    { 2, "Paypal" },
                    { 3, "ApplePay" },
                    { 4, "GooglePay" },
                    { 5, "I.O.U." }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirebaseKey", "FirstName", "LastName", "Username" },
                values: new object[,]
                {
                    { 1, "anadir@greendale.edu", null, "Abed", "Nadir", "InspectorSpaceTime" },
                    { 2, "tbarnes@greendale.edu", null, "Troy", "Barnes", "ConstableReggie" },
                    { 3, "sbennett@greendale.edu", null, "Shirley", "Bennet", "shirleySandwiches" },
                    { 4, "jwinger@greendale.edu", null, "Jeff", "Winger", "Wingman" },
                    { 5, "bperry@greendale.edu", null, "Britta", "Perry", "Britta" },
                    { 6, "aedison@greendale.edu", null, "Annie", "Edison", "Milady" },
                    { 7, "dean@greendale.edu", null, "Craig", "Pelton", "DeanLightful" },
                    { 8, "aosbourne@greendale.edu", null, "Alex", "Osbourne", "StarBurns" },
                    { 9, "kevin@greendale.edu", null, "Ben", "Chang", "Kevin" }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "Address", "BuyerId", "Closed", "OrderDate", "PaymentTypeId" },
                values: new object[,]
                {
                    { 1, "Apt 303, Greendale, Colorado", 2, true, null, 2 },
                    { 2, "Abandoned Horse Stables, Greendale Community College, Greendale, Colorado", 8, false, null, 5 },
                    { 3, "Inside the Air Vents, Greendale Community College, Greendale, Colorado", 9, true, null, 1 },
                    { 4, "Apt 303, Greendale, Colorado", 1, false, null, 3 }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "DatePosted", "Description", "Image", "Name", "Price", "Quantity", "SellerId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2013, 9, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "50 red paintballs. High quality rounds quaranteed to explode on contact.", "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fsc01.alicdn.com%2Fkf%2FHTB1t.dojr_I8KJjy1Xaq6zsxpXaz%2F230016343%2FHTB1t.dojr_I8KJjy1Xaq6zsxpXaz.jpg&f=1&nofb=1&ipt=bae9eed89237af046bf8fc55a031b15e180a44a5e85cdbded158ec1f1157e017&ipo=images", "Red Paintballs x 50", 20.00m, 20, 8 },
                    { 2, 1, new DateTime(2013, 9, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "50 silver paintballs. Rare find.", "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fwww.discountpaintball.com%2Fcdn-cgi%2Fimage%2Fwidth%253D600%252Cquality%253D100%2Fassets%2Fimages%2F900-270-03611-1.jpg&f=1&nofb=1&ipt=5236d4f3c11f23bd6bdeada50f8d01cb3e7895dce7fcdf4be2f94872590f36c1&ipo=images", "Silver Paintballs x 50", 40.00m, 10, 4 },
                    { 3, 3, new DateTime(2012, 3, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Goatees for dark timelines. Made from high quality felt.", "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fi.pinimg.com%2F736x%2F94%2Fd9%2Fa5%2F94d9a54617e69c7fc968373236573d00--timeline-tape.jpg&f=1&nofb=1&ipt=c81763d68d9d5d8fb91354adbc059652caebe9f105935212167c7af72f3ae05c&ipo=images", "Villanous Goatee", 5.00m, 7, 2 },
                    { 4, 4, new DateTime(2011, 8, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Answers to upcoming Ladders midterm.", "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fcdn-0.studybreaks.com%2Fwp-content%2Fuploads%2F2017%2F07%2FTest-Bubble-Sheet-1024x682.jpg&f=1&nofb=1&ipt=3d37374e4800ba9dc3128d3f3fdf684e9bb6d9d44fb0de5a3f0326d55d94f13e&ipo=images", "Test Answers for Ladders", 25.00m, 4, 9 },
                    { 5, 3, new DateTime(2009, 11, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sequined ballgown in shimmering magenta, size 8.", "https://external-content.duckduckgo.com/iu/?u=http%3A%2F%2Fwww.heyuguys.com%2Fimages%2F2012%2F09%2FCommunity-Fabulous-Dean.jpg&f=1&nofb=1&ipt=e7c19e8d51e20a333b685ac09292f375c392c74887ef357c7fda0fa57750983e&ipo=images", "Ballgown size 8", 50.00m, 1, 7 },
                    { 6, 2, new DateTime(2010, 10, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Restractable purple gel pen with fine point tip.", "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fwww.theknowledgetree.com%2Fprodimages%2F175684-DEFAULT-l.jpg&f=1&nofb=1&ipt=386507ad54b25be58a9b3644397e91cab2fd976025805b45625776df3f3d09d4&ipo=images", "Purple gel pen", 2.00m, 12, 6 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderProduct_ProductsId",
                table: "OrderProduct",
                column: "ProductsId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_BuyerId",
                table: "Orders",
                column: "BuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SellerId",
                table: "Products",
                column: "SellerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderProduct");

            migrationBuilder.DropTable(
                name: "PaymentTypes");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
