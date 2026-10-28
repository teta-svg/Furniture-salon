using FurnitureSalon.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FurnitureSalon.Api.Data;

public partial class FurnitureSalonDbContext : DbContext
{
    public FurnitureSalonDbContext(DbContextOptions<FurnitureSalonDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<DeliveryNote> DeliveryNotes { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<FabricCategory> FabricCategories { get; set; }

    public virtual DbSet<FurnitureConfiguration> FurnitureConfigurations { get; set; }

    public virtual DbSet<FurnitureConfigurationMaterial> FurnitureConfigurationMaterials { get; set; }

    public virtual DbSet<FurnitureConfigurationStorageModule> FurnitureConfigurationStorageModules { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<LegalClient> LegalClients { get; set; }

    public virtual DbSet<Material> Materials { get; set; }

    public virtual DbSet<MaterialSupplier> MaterialSuppliers { get; set; }

    public virtual DbSet<Model> Models { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PhysicalClient> PhysicalClients { get; set; }

    public virtual DbSet<ProductionOrder> ProductionOrders { get; set; }

    public virtual DbSet<StorageModule> StorageModules { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<TransformationMechanism> TransformationMechanisms { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.IdClient).HasName("PK__Client__B5AE4EC8035D03BC");

            entity.ToTable("Client");

            entity.Property(e => e.IdClient).HasColumnName("ID_Client");
            entity.Property(e => e.ClientType)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Client_Type");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<DeliveryNote>(entity =>
        {
            entity.HasKey(e => e.IdDeliveryNote).HasName("PK__Delivery__E5534D98513B5279");

            entity.ToTable("Delivery_Note");

            entity.HasIndex(e => e.IdOrder, "UQ__Delivery__EC9FA954568F202A").IsUnique();

            entity.Property(e => e.IdDeliveryNote).HasColumnName("ID_Delivery_Note");
            entity.Property(e => e.CreationDate).HasColumnName("Creation_Date");
            entity.Property(e => e.IdEmployee).HasColumnName("ID_Employee");
            entity.Property(e => e.IdOrder).HasColumnName("ID_Order");
            entity.Property(e => e.LoadingDate).HasColumnName("Loading_Date");

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.DeliveryNotes)
                .HasForeignKey(d => d.IdEmployee)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Delivery_Note_Employee");

            entity.HasOne(d => d.IdOrderNavigation).WithOne(p => p.DeliveryNote)
                .HasForeignKey<DeliveryNote>(d => d.IdOrder)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Delivery_Note_Order");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.IdEmployee).HasName("PK__Employee__D9EE4F36EAE5E4CF");

            entity.ToTable("Employee");

            entity.Property(e => e.IdEmployee).HasColumnName("ID_Employee");
            entity.Property(e => e.EmployeeRole)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Patronymic)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Surname)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<FabricCategory>(entity =>
        {
            entity.HasKey(e => e.IdFabricCategory).HasName("PK__Fabric_C__F76C2D93F7DF6A7A");

            entity.ToTable("Fabric_Category");

            entity.Property(e => e.IdFabricCategory).HasColumnName("ID_Fabric_Category");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PriceMarkup)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Price_Markup");
        });

        modelBuilder.Entity<FurnitureConfiguration>(entity =>
        {
            entity.HasKey(e => e.IdFurnitureConfiguration).HasName("PK__Furnitur__5E18CBFB4A801A2C");

            entity.ToTable("Furniture_Configuration");

            entity.Property(e => e.IdFurnitureConfiguration).HasColumnName("ID_Furniture_Configuration");
            entity.Property(e => e.Color)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IdFabricCategory).HasColumnName("ID_Fabric_Category");
            entity.Property(e => e.IdModel).HasColumnName("ID_Model");
            entity.Property(e => e.IdTransformationMechanism).HasColumnName("ID_Transformation_Mechanism");

            entity.HasOne(d => d.IdFabricCategoryNavigation).WithMany(p => p.FurnitureConfigurations)
                .HasForeignKey(d => d.IdFabricCategory)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Fabric_Category_Furniture_Configuration");

            entity.HasOne(d => d.IdModelNavigation).WithMany(p => p.FurnitureConfigurations)
                .HasForeignKey(d => d.IdModel)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Model_Furniture_Configuration");

            entity.HasOne(d => d.IdTransformationMechanismNavigation).WithMany(p => p.FurnitureConfigurations)
                .HasForeignKey(d => d.IdTransformationMechanism)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transformation_Mechanism_Furniture_Configuration");
        });

        modelBuilder.Entity<FurnitureConfigurationMaterial>(entity =>
        {
            entity.HasKey(e => e.IdFurnitureConfigurationMaterial).HasName("PK__Furnitur__72C75F37E83F22EE");

            entity.ToTable("Furniture_Configuration_Material");

            entity.Property(e => e.IdFurnitureConfigurationMaterial).HasColumnName("ID_Furniture_Configuration_Material");
            entity.Property(e => e.IdFurnitureConfiguration).HasColumnName("ID_Furniture_Configuration");
            entity.Property(e => e.IdMaterial).HasColumnName("ID_Material");
            entity.Property(e => e.RequiredQuantity)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Required_Quantity");

            entity.HasOne(d => d.IdFurnitureConfigurationNavigation).WithMany(p => p.FurnitureConfigurationMaterials)
                .HasForeignKey(d => d.IdFurnitureConfiguration)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Furniture_Configuration_Material");

            entity.HasOne(d => d.IdMaterialNavigation).WithMany(p => p.FurnitureConfigurationMaterials)
                .HasForeignKey(d => d.IdMaterial)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Material_Furniture_Configuration");
        });

        modelBuilder.Entity<FurnitureConfigurationStorageModule>(entity =>
        {
            entity.HasKey(e => e.IdFurnitureConfigurationStorageModule).HasName("PK__Furnitur__C201BD2C0D64D80A");

            entity.ToTable("Furniture_Configuration_Storage_Module");

            entity.Property(e => e.IdFurnitureConfigurationStorageModule).HasColumnName("ID_Furniture_Configuration_Storage_Module");
            entity.Property(e => e.IdFurnitureConfiguration).HasColumnName("ID_Furniture_Configuration");
            entity.Property(e => e.IdStorageModule).HasColumnName("ID_Storage_Module");

            entity.HasOne(d => d.IdFurnitureConfigurationNavigation).WithMany(p => p.FurnitureConfigurationStorageModules)
                .HasForeignKey(d => d.IdFurnitureConfiguration)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Furniture_Configuration_Storage_Module");

            entity.HasOne(d => d.IdStorageModuleNavigation).WithMany(p => p.FurnitureConfigurationStorageModules)
                .HasForeignKey(d => d.IdStorageModule)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Storage_Module_Furniture_Configuration");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.IdInvoice).HasName("PK__Invoice__0540CA60CF1E800E");

            entity.ToTable("Invoice");

            entity.Property(e => e.IdInvoice).HasColumnName("ID_Invoice");
            entity.Property(e => e.IdOrder).HasColumnName("ID_Order");
            entity.Property(e => e.InvoiceAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Invoice_Amount");
            entity.Property(e => e.InvoiceDate).HasColumnName("Invoice_Date");
            entity.Property(e => e.InvoiceStatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("Invoice_Status");

            entity.HasOne(d => d.IdOrderNavigation).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.IdOrder)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Invoice_Order");
        });

        modelBuilder.Entity<LegalClient>(entity =>
        {
            entity.HasKey(e => e.IdLegalClient).HasName("PK__Legal_Cl__F83D2EC38DD0AE1A");

            entity.ToTable("Legal_Client");

            entity.HasIndex(e => e.IdClient, "UQ__Legal_Cl__B5AE4EC9BE96BC68").IsUnique();

            entity.Property(e => e.IdLegalClient).HasColumnName("ID_Legal_Client");
            entity.Property(e => e.CompanyName)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("Company_Name");
            entity.Property(e => e.IdClient).HasColumnName("ID_Client");
            entity.Property(e => e.Inn)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("INN");
            entity.Property(e => e.Kpp)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("KPP");
            entity.Property(e => e.NameContactPerson)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Name_Contact_Person");
            entity.Property(e => e.PatronymicContactPerson)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Patronymic_Contact_Person");
            entity.Property(e => e.SurnameContactPerson)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Surname_Contact_Person");

            entity.HasOne(d => d.IdClientNavigation).WithOne(p => p.LegalClient)
                .HasForeignKey<LegalClient>(d => d.IdClient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Client_Legal_Client");
        });

        modelBuilder.Entity<Material>(entity =>
        {
            entity.HasKey(e => e.IdMaterial).HasName("PK__Material__A7F521BB56378454");

            entity.ToTable("Material");

            entity.Property(e => e.IdMaterial).HasColumnName("ID_Material");
            entity.Property(e => e.MaterialName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Material_Name");
            entity.Property(e => e.MaterialType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Material_Type");
            entity.Property(e => e.QuantityInStock)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Quantity_In_Stock");
            entity.Property(e => e.Unit)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MaterialSupplier>(entity =>
        {
            entity.HasKey(e => e.IdMaterialSupplier).HasName("PK__Material__02F63F71CEE15E8C");

            entity.ToTable("Material_Supplier");

            entity.Property(e => e.IdMaterialSupplier).HasColumnName("ID_Material_Supplier");
            entity.Property(e => e.DeliveryTime).HasColumnName("Delivery_Time");
            entity.Property(e => e.IdMaterial).HasColumnName("ID_Material");
            entity.Property(e => e.IdSupplier).HasColumnName("ID_Supplier");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdMaterialNavigation).WithMany(p => p.MaterialSuppliers)
                .HasForeignKey(d => d.IdMaterial)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Material_Supplier");

            entity.HasOne(d => d.IdSupplierNavigation).WithMany(p => p.MaterialSuppliers)
                .HasForeignKey(d => d.IdSupplier)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Supplier_Material");
        });

        modelBuilder.Entity<Model>(entity =>
        {
            entity.HasKey(e => e.IdModel).HasName("PK__Model__BBF1B356B0D76D63");

            entity.ToTable("Model");

            entity.Property(e => e.IdModel).HasColumnName("ID_Model");
            entity.Property(e => e.BasePrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Base_Price");
            entity.Property(e => e.ModelName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Model_Name");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.IdOrder).HasName("PK__Order__EC9FA9558D295011");

            entity.ToTable("Order");

            entity.Property(e => e.IdOrder).HasColumnName("ID_Order");
            entity.Property(e => e.CreationDate).HasColumnName("Creation_Date");
            entity.Property(e => e.DeliveryApartment)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("Delivery_Apartment");
            entity.Property(e => e.DeliveryBuilding)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("Delivery_Building");
            entity.Property(e => e.DeliveryCity)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Delivery_City");
            entity.Property(e => e.DeliveryStreet)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Delivery_Street");
            entity.Property(e => e.IdClient).HasColumnName("ID_Client");
            entity.Property(e => e.IdEmployee).HasColumnName("ID_Employee");
            entity.Property(e => e.TotalPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Total_Price");

            entity.HasOne(d => d.IdClientNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdClient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Client_Order");

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdEmployee)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employee_Order");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.IdOrderItem).HasName("PK__Order_It__BA5176FE8987E783");

            entity.ToTable("Order_Item");

            entity.Property(e => e.IdOrderItem).HasColumnName("ID_Order_Item");
            entity.Property(e => e.CurrentStatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("Current_Status");
            entity.Property(e => e.IdFurnitureConfiguration).HasColumnName("ID_Furniture_Configuration");
            entity.Property(e => e.IdOrder).HasColumnName("ID_Order");
            entity.Property(e => e.PlannedReadyDate).HasColumnName("Planned_Ready_Date");

            entity.HasOne(d => d.IdFurnitureConfigurationNavigation).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.IdFurnitureConfiguration)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Item_Furniture_Configuration");

            entity.HasOne(d => d.IdOrderNavigation).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.IdOrder)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Item_Order");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.IdPayment).HasName("PK__Payment__C2118ADE17B2C885");

            entity.ToTable("Payment");

            entity.Property(e => e.IdPayment).HasColumnName("ID_Payment");
            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.IdInvoice).HasColumnName("ID_Invoice");
            entity.Property(e => e.IdOrder).HasColumnName("ID_Order");
            entity.Property(e => e.PaymentDate).HasColumnName("Payment_Date");
            entity.Property(e => e.PaymentDocNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Payment_Doc_Number");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("Payment_Method");
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("Payment_Status");
            entity.Property(e => e.PaymentType)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("Payment_Type");

            entity.HasOne(d => d.IdInvoiceNavigation).WithMany(p => p.Payments)
                .HasForeignKey(d => d.IdInvoice)
                .HasConstraintName("FK_Payment_Invoice");

            entity.HasOne(d => d.IdOrderNavigation).WithMany(p => p.Payments)
                .HasForeignKey(d => d.IdOrder)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payment_Order");
        });

        modelBuilder.Entity<PhysicalClient>(entity =>
        {
            entity.HasKey(e => e.IdPhysicalClient).HasName("PK__Physical__9B6CB94BAE432EC8");

            entity.ToTable("Physical_Client");

            entity.HasIndex(e => e.IdClient, "UQ__Physical__B5AE4EC98E767E71").IsUnique();

            entity.Property(e => e.IdPhysicalClient).HasColumnName("ID_Physical_Client");
            entity.Property(e => e.IdClient).HasColumnName("ID_Client");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Patronymic)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Surname)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdClientNavigation).WithOne(p => p.PhysicalClient)
                .HasForeignKey<PhysicalClient>(d => d.IdClient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Client_Physical_Client");
        });

        modelBuilder.Entity<ProductionOrder>(entity =>
        {
            entity.HasKey(e => e.IdProductionOrder).HasName("PK__Producti__87934F0A08FAE0CC");

            entity.ToTable("Production_Order");

            entity.HasIndex(e => e.IdOrder, "UQ__Producti__EC9FA9546295D219").IsUnique();

            entity.Property(e => e.IdProductionOrder).HasColumnName("ID_Production_Order");
            entity.Property(e => e.CompletionDate).HasColumnName("Completion_Date");
            entity.Property(e => e.CreationDate).HasColumnName("Creation_Date");
            entity.Property(e => e.IdEmployee).HasColumnName("ID_Employee");
            entity.Property(e => e.IdOrder).HasColumnName("ID_Order");

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.ProductionOrders)
                .HasForeignKey(d => d.IdEmployee)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Production_Order_Employee");

            entity.HasOne(d => d.IdOrderNavigation).WithOne(p => p.ProductionOrder)
                .HasForeignKey<ProductionOrder>(d => d.IdOrder)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Production_Order_Order");
        });

        modelBuilder.Entity<StorageModule>(entity =>
        {
            entity.HasKey(e => e.IdStorageModule).HasName("PK__Storage___098ED3C95BB59D44");

            entity.ToTable("Storage_Module");

            entity.Property(e => e.IdStorageModule).HasColumnName("ID_Storage_Module");
            entity.Property(e => e.BasePrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Base_Price");
            entity.Property(e => e.ModuleName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Module_Name");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.IdSupplier).HasName("PK__Supplier__408B7094F4DE711E");

            entity.ToTable("Supplier");

            entity.Property(e => e.IdSupplier).HasColumnName("ID_Supplier");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.SupplierName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Supplier_Name");
        });

        modelBuilder.Entity<TransformationMechanism>(entity =>
        {
            entity.HasKey(e => e.IdTransformationMechanism).HasName("PK__Transfor__A5AAB5AF0E68D8ED");

            entity.ToTable("Transformation_Mechanism");

            entity.Property(e => e.IdTransformationMechanism).HasColumnName("ID_Transformation_Mechanism");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PriceMarkup)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Price_Markup");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PK__User__ED4DE442235391E7");

            entity.ToTable("User");

            entity.Property(e => e.IdUser).HasColumnName("ID_User");
            entity.Property(e => e.IdEmployee).HasColumnName("ID_Employee");
            entity.Property(e => e.Login)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Role)
                .HasMaxLength(30)
                .IsUnicode(false);

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdEmployee)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Employee");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
