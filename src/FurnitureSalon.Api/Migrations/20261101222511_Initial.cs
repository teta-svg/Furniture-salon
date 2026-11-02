using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FurnitureSalon.Api.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    ID_Client = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Client_Type = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Phone = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Client__B5AE4EC8035D03BC", x => x.ID_Client);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    ID_Employee = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Surname = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Patronymic = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    EmployeeRole = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Employee__D9EE4F36EAE5E4CF", x => x.ID_Employee);
                });

            migrationBuilder.CreateTable(
                name: "Fabric_Category",
                columns: table => new
                {
                    ID_Fabric_Category = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Price_Markup = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Fabric_C__F76C2D93F7DF6A7A", x => x.ID_Fabric_Category);
                });

            migrationBuilder.CreateTable(
                name: "Material",
                columns: table => new
                {
                    ID_Material = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Material_Name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Material_Type = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Unit = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Quantity_In_Stock = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Material__A7F521BB56378454", x => x.ID_Material);
                });

            migrationBuilder.CreateTable(
                name: "Model",
                columns: table => new
                {
                    ID_Model = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Model_Name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Base_Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Model__BBF1B356B0D76D63", x => x.ID_Model);
                });

            migrationBuilder.CreateTable(
                name: "Storage_Module",
                columns: table => new
                {
                    ID_Storage_Module = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Module_Name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Base_Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Storage___098ED3C95BB59D44", x => x.ID_Storage_Module);
                });

            migrationBuilder.CreateTable(
                name: "Supplier",
                columns: table => new
                {
                    ID_Supplier = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Supplier_Name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Supplier__408B7094F4DE711E", x => x.ID_Supplier);
                });

            migrationBuilder.CreateTable(
                name: "Transformation_Mechanism",
                columns: table => new
                {
                    ID_Transformation_Mechanism = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Price_Markup = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Transfor__A5AAB5AF0E68D8ED", x => x.ID_Transformation_Mechanism);
                });

            migrationBuilder.CreateTable(
                name: "Legal_Client",
                columns: table => new
                {
                    ID_Legal_Client = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Client = table.Column<int>(type: "int", nullable: false),
                    Company_Name = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: false),
                    INN = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    KPP = table.Column<string>(type: "varchar(9)", unicode: false, maxLength: 9, nullable: true),
                    Surname_Contact_Person = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Name_Contact_Person = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Patronymic_Contact_Person = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Legal_Cl__F83D2EC38DD0AE1A", x => x.ID_Legal_Client);
                    table.ForeignKey(
                        name: "FK_Client_Legal_Client",
                        column: x => x.ID_Client,
                        principalTable: "Client",
                        principalColumn: "ID_Client");
                });

            migrationBuilder.CreateTable(
                name: "Physical_Client",
                columns: table => new
                {
                    ID_Physical_Client = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Client = table.Column<int>(type: "int", nullable: false),
                    Surname = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Patronymic = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Physical__9B6CB94BAE432EC8", x => x.ID_Physical_Client);
                    table.ForeignKey(
                        name: "FK_Client_Physical_Client",
                        column: x => x.ID_Client,
                        principalTable: "Client",
                        principalColumn: "ID_Client");
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    ID_Order = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Client = table.Column<int>(type: "int", nullable: false),
                    ID_Employee = table.Column<int>(type: "int", nullable: false),
                    Creation_Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Total_Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Delivery_City = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Delivery_Street = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Delivery_Building = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    Delivery_Apartment = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Order__EC9FA9558D295011", x => x.ID_Order);
                    table.ForeignKey(
                        name: "FK_Client_Order",
                        column: x => x.ID_Client,
                        principalTable: "Client",
                        principalColumn: "ID_Client");
                    table.ForeignKey(
                        name: "FK_Employee_Order",
                        column: x => x.ID_Employee,
                        principalTable: "Employee",
                        principalColumn: "ID_Employee");
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    ID_User = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Employee = table.Column<int>(type: "int", nullable: false),
                    Login = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Role = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__User__ED4DE442235391E7", x => x.ID_User);
                    table.ForeignKey(
                        name: "FK_User_Employee",
                        column: x => x.ID_Employee,
                        principalTable: "Employee",
                        principalColumn: "ID_Employee");
                });

            migrationBuilder.CreateTable(
                name: "Material_Supplier",
                columns: table => new
                {
                    ID_Material_Supplier = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Supplier = table.Column<int>(type: "int", nullable: false),
                    ID_Material = table.Column<int>(type: "int", nullable: false),
                    Delivery_Time = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Material__02F63F71CEE15E8C", x => x.ID_Material_Supplier);
                    table.ForeignKey(
                        name: "FK_Material_Supplier",
                        column: x => x.ID_Material,
                        principalTable: "Material",
                        principalColumn: "ID_Material");
                    table.ForeignKey(
                        name: "FK_Supplier_Material",
                        column: x => x.ID_Supplier,
                        principalTable: "Supplier",
                        principalColumn: "ID_Supplier");
                });

            migrationBuilder.CreateTable(
                name: "Furniture_Configuration",
                columns: table => new
                {
                    ID_Furniture_Configuration = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Model = table.Column<int>(type: "int", nullable: false),
                    ID_Fabric_Category = table.Column<int>(type: "int", nullable: false),
                    ID_Transformation_Mechanism = table.Column<int>(type: "int", nullable: false),
                    Length = table.Column<int>(type: "int", nullable: false),
                    Width = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Furnitur__5E18CBFB4A801A2C", x => x.ID_Furniture_Configuration);
                    table.ForeignKey(
                        name: "FK_Fabric_Category_Furniture_Configuration",
                        column: x => x.ID_Fabric_Category,
                        principalTable: "Fabric_Category",
                        principalColumn: "ID_Fabric_Category");
                    table.ForeignKey(
                        name: "FK_Model_Furniture_Configuration",
                        column: x => x.ID_Model,
                        principalTable: "Model",
                        principalColumn: "ID_Model");
                    table.ForeignKey(
                        name: "FK_Transformation_Mechanism_Furniture_Configuration",
                        column: x => x.ID_Transformation_Mechanism,
                        principalTable: "Transformation_Mechanism",
                        principalColumn: "ID_Transformation_Mechanism");
                });

            migrationBuilder.CreateTable(
                name: "Delivery_Note",
                columns: table => new
                {
                    ID_Delivery_Note = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Order = table.Column<int>(type: "int", nullable: false),
                    ID_Employee = table.Column<int>(type: "int", nullable: false),
                    Creation_Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Loading_Date = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Delivery__E5534D98513B5279", x => x.ID_Delivery_Note);
                    table.ForeignKey(
                        name: "FK_Delivery_Note_Employee",
                        column: x => x.ID_Employee,
                        principalTable: "Employee",
                        principalColumn: "ID_Employee");
                    table.ForeignKey(
                        name: "FK_Delivery_Note_Order",
                        column: x => x.ID_Order,
                        principalTable: "Order",
                        principalColumn: "ID_Order");
                });

            migrationBuilder.CreateTable(
                name: "Invoice",
                columns: table => new
                {
                    ID_Invoice = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Order = table.Column<int>(type: "int", nullable: false),
                    Invoice_Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Invoice_Amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Invoice_Status = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Invoice__0540CA60CF1E800E", x => x.ID_Invoice);
                    table.ForeignKey(
                        name: "FK_Invoice_Order",
                        column: x => x.ID_Order,
                        principalTable: "Order",
                        principalColumn: "ID_Order");
                });

            migrationBuilder.CreateTable(
                name: "Production_Order",
                columns: table => new
                {
                    ID_Production_Order = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Order = table.Column<int>(type: "int", nullable: false),
                    ID_Employee = table.Column<int>(type: "int", nullable: false),
                    Creation_Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Completion_Date = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Producti__87934F0A08FAE0CC", x => x.ID_Production_Order);
                    table.ForeignKey(
                        name: "FK_Production_Order_Employee",
                        column: x => x.ID_Employee,
                        principalTable: "Employee",
                        principalColumn: "ID_Employee");
                    table.ForeignKey(
                        name: "FK_Production_Order_Order",
                        column: x => x.ID_Order,
                        principalTable: "Order",
                        principalColumn: "ID_Order");
                });

            migrationBuilder.CreateTable(
                name: "Furniture_Configuration_Material",
                columns: table => new
                {
                    ID_Furniture_Configuration_Material = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Furniture_Configuration = table.Column<int>(type: "int", nullable: false),
                    ID_Material = table.Column<int>(type: "int", nullable: false),
                    Required_Quantity = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Furnitur__72C75F37E83F22EE", x => x.ID_Furniture_Configuration_Material);
                    table.ForeignKey(
                        name: "FK_Furniture_Configuration_Material",
                        column: x => x.ID_Furniture_Configuration,
                        principalTable: "Furniture_Configuration",
                        principalColumn: "ID_Furniture_Configuration");
                    table.ForeignKey(
                        name: "FK_Material_Furniture_Configuration",
                        column: x => x.ID_Material,
                        principalTable: "Material",
                        principalColumn: "ID_Material");
                });

            migrationBuilder.CreateTable(
                name: "Furniture_Configuration_Storage_Module",
                columns: table => new
                {
                    ID_Furniture_Configuration_Storage_Module = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Furniture_Configuration = table.Column<int>(type: "int", nullable: false),
                    ID_Storage_Module = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Furnitur__C201BD2C0D64D80A", x => x.ID_Furniture_Configuration_Storage_Module);
                    table.ForeignKey(
                        name: "FK_Furniture_Configuration_Storage_Module",
                        column: x => x.ID_Furniture_Configuration,
                        principalTable: "Furniture_Configuration",
                        principalColumn: "ID_Furniture_Configuration");
                    table.ForeignKey(
                        name: "FK_Storage_Module_Furniture_Configuration",
                        column: x => x.ID_Storage_Module,
                        principalTable: "Storage_Module",
                        principalColumn: "ID_Storage_Module");
                });

            migrationBuilder.CreateTable(
                name: "Order_Item",
                columns: table => new
                {
                    ID_Order_Item = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Order = table.Column<int>(type: "int", nullable: false),
                    ID_Furniture_Configuration = table.Column<int>(type: "int", nullable: false),
                    Planned_Ready_Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Current_Status = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Order_It__BA5176FE8987E783", x => x.ID_Order_Item);
                    table.ForeignKey(
                        name: "FK_Order_Item_Furniture_Configuration",
                        column: x => x.ID_Furniture_Configuration,
                        principalTable: "Furniture_Configuration",
                        principalColumn: "ID_Furniture_Configuration");
                    table.ForeignKey(
                        name: "FK_Order_Item_Order",
                        column: x => x.ID_Order,
                        principalTable: "Order",
                        principalColumn: "ID_Order");
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    ID_Payment = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Order = table.Column<int>(type: "int", nullable: false),
                    ID_Invoice = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Payment_Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Payment_Type = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    Payment_Status = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    Payment_Method = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    Payment_Doc_Number = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Payment__C2118ADE17B2C885", x => x.ID_Payment);
                    table.ForeignKey(
                        name: "FK_Payment_Invoice",
                        column: x => x.ID_Invoice,
                        principalTable: "Invoice",
                        principalColumn: "ID_Invoice");
                    table.ForeignKey(
                        name: "FK_Payment_Order",
                        column: x => x.ID_Order,
                        principalTable: "Order",
                        principalColumn: "ID_Order");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Delivery_Note_ID_Employee",
                table: "Delivery_Note",
                column: "ID_Employee");

            migrationBuilder.CreateIndex(
                name: "UQ__Delivery__EC9FA954568F202A",
                table: "Delivery_Note",
                column: "ID_Order",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Furniture_Configuration_ID_Fabric_Category",
                table: "Furniture_Configuration",
                column: "ID_Fabric_Category");

            migrationBuilder.CreateIndex(
                name: "IX_Furniture_Configuration_ID_Model",
                table: "Furniture_Configuration",
                column: "ID_Model");

            migrationBuilder.CreateIndex(
                name: "IX_Furniture_Configuration_ID_Transformation_Mechanism",
                table: "Furniture_Configuration",
                column: "ID_Transformation_Mechanism");

            migrationBuilder.CreateIndex(
                name: "IX_Furniture_Configuration_Material_ID_Furniture_Configuration",
                table: "Furniture_Configuration_Material",
                column: "ID_Furniture_Configuration");

            migrationBuilder.CreateIndex(
                name: "IX_Furniture_Configuration_Material_ID_Material",
                table: "Furniture_Configuration_Material",
                column: "ID_Material");

            migrationBuilder.CreateIndex(
                name: "IX_Furniture_Configuration_Storage_Module_ID_Furniture_Configuration",
                table: "Furniture_Configuration_Storage_Module",
                column: "ID_Furniture_Configuration");

            migrationBuilder.CreateIndex(
                name: "IX_Furniture_Configuration_Storage_Module_ID_Storage_Module",
                table: "Furniture_Configuration_Storage_Module",
                column: "ID_Storage_Module");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_ID_Order",
                table: "Invoice",
                column: "ID_Order");

            migrationBuilder.CreateIndex(
                name: "UQ__Legal_Cl__B5AE4EC9BE96BC68",
                table: "Legal_Client",
                column: "ID_Client",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Material_Supplier_ID_Material",
                table: "Material_Supplier",
                column: "ID_Material");

            migrationBuilder.CreateIndex(
                name: "IX_Material_Supplier_ID_Supplier",
                table: "Material_Supplier",
                column: "ID_Supplier");

            migrationBuilder.CreateIndex(
                name: "IX_Order_ID_Client",
                table: "Order",
                column: "ID_Client");

            migrationBuilder.CreateIndex(
                name: "IX_Order_ID_Employee",
                table: "Order",
                column: "ID_Employee");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Item_ID_Furniture_Configuration",
                table: "Order_Item",
                column: "ID_Furniture_Configuration");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Item_ID_Order",
                table: "Order_Item",
                column: "ID_Order");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_ID_Invoice",
                table: "Payment",
                column: "ID_Invoice");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_ID_Order",
                table: "Payment",
                column: "ID_Order");

            migrationBuilder.CreateIndex(
                name: "UQ__Physical__B5AE4EC98E767E71",
                table: "Physical_Client",
                column: "ID_Client",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Production_Order_ID_Employee",
                table: "Production_Order",
                column: "ID_Employee");

            migrationBuilder.CreateIndex(
                name: "UQ__Producti__EC9FA9546295D219",
                table: "Production_Order",
                column: "ID_Order",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_ID_Employee",
                table: "User",
                column: "ID_Employee");

            migrationBuilder.Sql(@"USE Furniture_salon;
GO

INSERT INTO Employee (Surname, [Name], Patronymic, EmployeeRole)
VALUES
    ('Иванов', 'Иван', 'Иванович', 'Администратор'),
    ('Петрова', 'Анна', 'Сергеевна', 'Менеджер'),
    ('Сидоров', 'Алексей', 'Павлович', 'Технолог');
GO

INSERT INTO [User] (ID_Employee, [Login], [Password], [Role])
VALUES
    (1, 'admin', 'admin123', 'Администратор'),
    (2, 'manager', 'manager123', 'Менеджер'),
    (3, 'tech', 'tech123', 'Технолог');
GO

INSERT INTO Client (Client_Type, Phone) VALUES
('Физическое лицо', '+79991112233'),
('Физическое лицо', '+79992223344'),
('Юридическое лицо', '+74951234567'),
('Физическое лицо', '+79993334455'),
('Юридическое лицо', '+74957654321');

INSERT INTO Physical_Client (ID_Client, Surname, Name, Patronymic) VALUES
(1, 'Иванов', 'Алексей', 'Сергеевич'),
(2, 'Петрова', 'Мария', 'Андреевна'),
(4, 'Сидоров', 'Дмитрий', NULL);

INSERT INTO Legal_Client (ID_Client, Company_Name, INN, KPP, Surname_Contact_Person, Name_Contact_Person, Patronymic_Contact_Person) VALUES
(3, 'ООО «Комфорт»', '1234567890', '123456789', 'Смирнов', 'Игорь', 'Павлович'),
(5, 'ООО «Интерьер Плюс»', '9876543210', '987654321', 'Кузнецова', 'Елена', NULL);

INSERT INTO Employee (Surname, Name, Patronymic, EmployeeRole) VALUES
('Васильев', 'Антон', 'Игоревич', 'Менеджер'),
('Орлова', 'Екатерина', 'Дмитриевна', 'Менеджер'),
('Фёдоров', 'Максим', NULL, 'Менеджер');

INSERT INTO Model (Model_Name, Base_Price) VALUES
('Диван «Милан»', 45000),
('Кресло «Релакс»', 18000),
('Угловой диван «Верона»', 72000),
('Кровать «Альба»', 38000),
('Шкаф «Модерн»', 25000);

INSERT INTO Fabric_Category ([Name], Price_Markup) VALUES
('Стандарт', 1.00),
('Премиум', 1.25),
('Люкс', 1.50);

INSERT INTO Transformation_Mechanism ([Name], Price_Markup) VALUES
('Без механизма', 1.00),
('Еврокнижка', 1.15),
('Аккордеон', 1.20);

INSERT INTO Furniture_Configuration (ID_Model, ID_Fabric_Category, ID_Transformation_Mechanism, [Length], Width, Height, Color) VALUES
(1, 1, 2, 200, 90, 85, 'Серый'),
(2, 2, 1, 85, 80, 95, 'Бежевый'),
(3, 3, 2, 280, 160, 90, 'Тёмно-синий'),
(4, 2, 1, 210, 160, 100, 'Белый'),
(5, 1, 1, 180, 60, 220, 'Орех');

INSERT INTO [Order] (ID_Client, ID_Employee, Creation_Date, Total_Price, Delivery_City, Delivery_Street, Delivery_Building, Delivery_Apartment) VALUES
(1, 1, '2026-10-01', 63000, 'Москва', 'Ленина', '12', '45'),
(2, 2, '2026-10-05', 36000, 'Москва', 'Пушкина', '7', '18'),
(3, 1, '2026-10-10', 144000, 'Москва', 'Тверская', '25', NULL),
(4, 3, '2026-10-15', 38000, 'Москва', 'Садовая', '3', '12'),
(5, 2, '2026-10-20', 50000, 'Москва', 'Мира', '16', NULL);

INSERT INTO Order_Item (ID_Order, ID_Furniture_Configuration, Planned_Ready_Date, Current_Status, Quantity) VALUES
(1, 1, '2026-11-01', 'В производстве', 1),
(1, 2, '2026-11-05', 'Ожидает производства', 1),
(2, 2, '2026-11-10', 'В производстве', 2),
(3, 3, '2026-11-15', 'Ожидает производства', 2),
(4, 4, '2026-11-20', 'Готов', 1),
(5, 5, '2026-11-25', 'В производстве', 2);
GO
INSERT INTO Storage_Module (Module_Name, Base_Price)
VALUES
('Бельевой ящик', 3500),
('Боковая полка', 1800),
('Подлокотник-бар', 4200);

INSERT INTO Material (Material_Name, Material_Type, Unit, Quantity_In_Stock)
VALUES
('Ткань стандартная', 'Ткань', 'м', 250),
('Ткань премиальная', 'Ткань', 'м', 120),
('Поролон', 'Наполнитель', 'кг', 180),
('Деревянный каркас', 'Каркас', 'шт.', 35),
('Металлический механизм', 'Механизм', 'шт.', 25);

INSERT INTO Supplier (Supplier_Name, Phone)
VALUES
('ООО Текстиль', '+74951110001'),
('ООО МебельКомплект', '+74951110002');

INSERT INTO Material_Supplier (ID_Supplier, ID_Material, Delivery_Time, Price)
VALUES
(1, 1, 5, 450),
(1, 2, 7, 900),
(2, 3, 3, 180),
(2, 4, 10, 4500),
(2, 5, 6, 2200);

INSERT INTO Furniture_Configuration_Storage_Module
(ID_Furniture_Configuration, ID_Storage_Module)
VALUES
(1, 1),
(1, 2),
(2, 1),
(2, 3),
(5, 1);

INSERT INTO Furniture_Configuration_Material
(ID_Furniture_Configuration, ID_Material, Required_Quantity)
VALUES
(1, 1, 12),
(1, 3, 18),
(1, 4, 1),
(1, 5, 1),
(2, 2, 18),
(2, 3, 25),
(2, 4, 2),
(2, 5, 1),
(3, 1, 8),
(3, 3, 12),
(3, 4, 1),
(4, 2, 5),
(4, 3, 6),
(4, 4, 1),
(5, 2, 12),
(5, 3, 18),
(5, 4, 1),
(5, 5, 1);

INSERT INTO Invoice (ID_Order, Invoice_Date, Invoice_Amount, Invoice_Status)
VALUES
(1, '2026-10-20', 53500, 'Частично оплачен'),
(2, '2026-10-22', 75500, 'Выставлен'),
(3, '2026-10-25', 104000, 'Оплачен'),
(4, '2026-10-26', 36000, 'Оплачен');

INSERT INTO Payment
(ID_Order, ID_Invoice, Amount, Payment_Date, Payment_Type, Payment_Status, Payment_Method, Payment_Doc_Number)
VALUES
(1, 1, 26750, '2026-10-20', 'Предоплата', 'Проведён', 'Банковская карта', 'PAY-0001'),
(3, 3, 104000, '2026-10-25', 'Полная оплата', 'Проведён', 'Банковский перевод', 'PAY-0002'),
(4, 4, 36000, '2026-10-26', 'Полная оплата', 'Проведён', 'Наличные', 'PAY-0003');

INSERT INTO Production_Order (ID_Order, ID_Employee, Creation_Date, Completion_Date)
VALUES
(1, 2, '2026-10-21', NULL),
(2, 2, '2026-10-23', NULL),
(3, 2, '2026-10-26', '2026-10-28');

INSERT INTO Delivery_Note (ID_Order, ID_Employee, Creation_Date, Loading_Date)
VALUES
(3, 3, '2026-10-28', '2026-10-28'),
(4, 3, '2026-10-27', '2026-10-28');

GO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Delivery_Note");

            migrationBuilder.DropTable(
                name: "Furniture_Configuration_Material");

            migrationBuilder.DropTable(
                name: "Furniture_Configuration_Storage_Module");

            migrationBuilder.DropTable(
                name: "Legal_Client");

            migrationBuilder.DropTable(
                name: "Material_Supplier");

            migrationBuilder.DropTable(
                name: "Order_Item");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "Physical_Client");

            migrationBuilder.DropTable(
                name: "Production_Order");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Storage_Module");

            migrationBuilder.DropTable(
                name: "Material");

            migrationBuilder.DropTable(
                name: "Supplier");

            migrationBuilder.DropTable(
                name: "Furniture_Configuration");

            migrationBuilder.DropTable(
                name: "Invoice");

            migrationBuilder.DropTable(
                name: "Fabric_Category");

            migrationBuilder.DropTable(
                name: "Model");

            migrationBuilder.DropTable(
                name: "Transformation_Mechanism");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "Employee");
        }
    }
}
