﻿using Microsoft.EntityFrameworkCore;
using RealSys.CoreLib.Models.DTO.Jobs;
using RealSys.CoreLib.Models.DTO.Products;
using RealSys.CoreLib.Models.DTO.Suppliers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealSys.CoreLib.Models.Erp
{

    public partial class ErpDbContext : DbContext
    {
        public ErpDbContext(DbContextOptions<ErpDbContext> options)
            : base(options)
        {
        }


        public virtual DbSet<JobMain> JobMains { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<JobType> JobTypes { get; set; }
        public virtual DbSet<Services> Services { get; set; }
        public virtual DbSet<JobServices> JobServices { get; set; }
        public virtual DbSet<JobItinerary> JobItineraries { get; set; }
        public virtual DbSet<Destination> Destinations { get; set; }
        public virtual DbSet<JobPickup> JobPickups { get; set; }
        public virtual DbSet<Branch> Branches { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<SupplierType> SupplierTypes { get; set; }
        public virtual DbSet<SupplierItem> SupplierItems { get; set; }
        public virtual DbSet<JobServicePickup> JobServicePickups { get; set; }
        public virtual DbSet<JobStatus> JobStatus { get; set; }
        public virtual DbSet<JobThru> JobThrus { get; set; }
        public virtual DbSet<Bank> Banks { get; set; }
        public virtual DbSet<JobPayment> JobPayments { get; set; }
        public virtual DbSet<CarCategory> CarCategories { get; set; }
        public virtual DbSet<CarUnit> CarUnits { get; set; }
        public virtual DbSet<CarDestination> CarDestinations { get; set; }
        public virtual DbSet<CarRate> CarRates { get; set; }
        public virtual DbSet<CarReservation> CarReservations { get; set; }
        public virtual DbSet<CarImage> CarImages { get; set; }
        public virtual DbSet<JobContact> JobContacts { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductPrice> ProductPrices { get; set; }
        public virtual DbSet<ProductImage> ProductImages1 { get; set; }
        public virtual DbSet<ProductCondition> ProductConditions { get; set; }
        public virtual DbSet<ProductCategory> ProductCategories { get; set; }
        public virtual DbSet<ProductProdCat> ProductProdCats { get; set; }
        public virtual DbSet<PreDefinedNote> PreDefinedNotes { get; set; }
        public virtual DbSet<JobNote> JobNotes { get; set; }
        public virtual DbSet<JobChecklist> JobChecklists { get; set; }
        public virtual DbSet<CustCategory> CustCategories { get; set; }
        public virtual DbSet<CustCat> CustCats { get; set; }
        public virtual DbSet<CustEntMain> CustEntMains { get; set; }
        public virtual DbSet<SalesLead> SalesLeads { get; set; }
        public virtual DbSet<SalesStatusCode> SalesStatusCodes { get; set; }
        public virtual DbSet<SalesStatus> SalesStatus { get; set; }
        public virtual DbSet<SalesActCode> SalesActCodes { get; set; }
        public virtual DbSet<SalesActivity> SalesActivities { get; set; }
        public virtual DbSet<CustEntity> CustEntities { get; set; }
        public virtual DbSet<SalesActStatus> SalesActStatus { get; set; }
        public virtual DbSet<SalesLeadCatCode> SalesLeadCatCodes { get; set; }
        public virtual DbSet<SalesLeadCategory> SalesLeadCategories { get; set; }
        public virtual DbSet<CustSalesCategory> CustSalesCategories { get; set; }
        public virtual DbSet<SrvActionCode> SrvActionCodes { get; set; }
        public virtual DbSet<SrvActionItem> SrvActionItems { get; set; }
        public virtual DbSet<JobAction> JobActions { get; set; }
        public virtual DbSet<InvItem> InvItems { get; set; }
        public virtual DbSet<InvItemCat> InvItemCats { get; set; }
        public virtual DbSet<InvItemCategory> InvItemCategories { get; set; }
        public virtual DbSet<JobServiceItem> JobServiceItems { get; set; }
        public virtual DbSet<SupplierInvItem> SupplierInvItems { get; set; }
        public virtual DbSet<JobNotificationRequest> JobNotificationRequests { get; set; }
        public virtual DbSet<CustFiles> CustFiles { get; set; }
        public virtual DbSet<SupplierPoHdr> SupplierPoHdrs { get; set; }
        public virtual DbSet<SupplierPoDtl> SupplierPoDtls { get; set; }
        public virtual DbSet<SupplierPoStatus> SupplierPoStatus { get; set; }
        public virtual DbSet<SupplierPoItem> SupplierPoItems { get; set; }
        public virtual DbSet<CustFileRef> CustFileRefs { get; set; }
        public virtual DbSet<SalesLeadLink> SalesLeadLinks { get; set; }
        public virtual DbSet<InvCarRecord> InvCarRecords { get; set; }
        public virtual DbSet<InvCarRecordType> InvCarRecordTypes { get; set; }
        public virtual DbSet<InvCarGateControl> InvCarGateControls { get; set; }
        public virtual DbSet<JobTrail> JobTrails { get; set; }
        public virtual DbSet<CarViewPage> CarViewPages { get; set; }
        public virtual DbSet<CarRatePackage> CarRatePackages { get; set; }
        public virtual DbSet<CarRateUnitPackage> CarRateUnitPackages { get; set; }
        public virtual DbSet<CarResPackage> CarResPackages { get; set; }
        public virtual DbSet<CarUnitMeta> CarUnitMetas { get; set; }
        public virtual DbSet<CoopMember> CoopMembers { get; set; }
        public virtual DbSet<CoopMemberItem> CoopMemberItems { get; set; }
        public virtual DbSet<PaypalTransaction> PaypalTransactions { get; set; }
        public virtual DbSet<PaypalAccount> PaypalAccounts { get; set; }
        public virtual DbSet<RateGroup> RateGroups { get; set; }
        public virtual DbSet<CarRateGroup> CarRateGroups { get; set; }
        public virtual DbSet<EmailBlasterTemplate> EmailBlasterTemplates { get; set; }
        public virtual DbSet<BlasterLog> BlasterLogs { get; set; }
        public virtual DbSet<EmailBlasterLogs> EmailBlasterLogs { get; set; }
        public virtual DbSet<JobEntMain> JobEntMains { get; set; }
        public virtual DbSet<CashExpense> CashExpenses { get; set; }
        public virtual DbSet<PortalCustomer> PortalCustomers { get; set; }
        public virtual DbSet<Expenses> Expenses { get; set; }
        public virtual DbSet<JobExpenses> JobExpenses { get; set; }
        public virtual DbSet<ExpensesCategory> ExpensesCategories { get; set; }
        public virtual DbSet<PkgDestination> PkgDestinations { get; set; }
        public virtual DbSet<JobPost> JobPosts { get; set; }
        public virtual DbSet<OnlineReservation> OnlineReservations { get; set; }
        public virtual DbSet<RsvPayment> RsvPayments { get; set; }
        public virtual DbSet<DriverInstructions> DriverInstructions { get; set; }
        public virtual DbSet<DriverInsJobService> DriverInsJobServices { get; set; }
        public virtual DbSet<SalesLeadCompany> SalesLeadCompanies { get; set; }
        public virtual DbSet<CustEntAddress> CustEntAddresses { get; set; }
        public virtual DbSet<CustEntCat> CustEntCats { get; set; }
        public virtual DbSet<CustEntClauses> CustEntClauses { get; set; }
        public virtual DbSet<SupplierContact> SupplierContacts { get; set; }
        public virtual DbSet<SupplierItemRate> SupplierItemRates { get; set; }
        public virtual DbSet<SupplierUnit> SupplierUnits { get; set; }
        public virtual DbSet<SalesLeadItems> SalesLeadItems { get; set; }
        public virtual DbSet<SalesLeadQuotedItem> SalesLeadQuotedItems { get; set; }
        public virtual DbSet<CustSocialAcc> CustSocialAccs { get; set; }
        public virtual DbSet<AdminEmail> AdminEmails { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<SupplierContactStatus> SupplierContactStatus { get; set; }
        public virtual DbSet<CustEntAssign> CustEntAssigns { get; set; }
        public virtual DbSet<SupplierActivity> SupplierActivities { get; set; }
        public virtual DbSet<SupDocument> SupDocuments { get; set; }
        public virtual DbSet<SupplierDocument> SupplierDocuments { get; set; }
        public virtual DbSet<CustEntActivity> CustEntActivities { get; set; }
        public virtual DbSet<CustEntDocuments> CustEntDocuments { get; set; }
        public virtual DbSet<CustNotif> CustNotifs { get; set; }
        public virtual DbSet<CustNotifActivity> CustNotifActivities { get; set; }
        public virtual DbSet<CustNotifRecipient> CustNotifRecipients { get; set; }
        public virtual DbSet<CustNotifRecipientList> CustNotifRecipientLists { get; set; }
        public virtual DbSet<CustEntActStatus> CustEntActStatus { get; set; }
        public virtual DbSet<CustEntActType> CustEntActTypes { get; set; }
        public virtual DbSet<SupplierActStatus> SupplierActStatus { get; set; }
        public virtual DbSet<CustEntActivityType> CustEntActivityTypes { get; set; }
        public virtual DbSet<SupplierActivityType> SupplierActivityTypes { get; set; }
        public virtual DbSet<JobVehicle> JobVehicles { get; set; }
        public virtual DbSet<Vehicle> Vehicles { get; set; }
        public virtual DbSet<VehicleType> VehicleTypes { get; set; }
        public virtual DbSet<VehicleModel> VehicleModels { get; set; }
        public virtual DbSet<VehicleBrand> VehicleBrands { get; set; }
        public virtual DbSet<VehicleTransmission> VehicleTransmissions { get; set; }
        public virtual DbSet<VehicleFuel> VehicleFuels { get; set; }
        public virtual DbSet<VehicleDrive> VehicleDrives { get; set; }
        public virtual DbSet<JobPostSale> JobPostSales { get; set; }
        public virtual DbSet<CustEntAccountType> CustEntAccountTypes { get; set; }
        public virtual DbSet<JobPaymentStatus> JobPaymentStatus { get; set; }
        public virtual DbSet<JobMainPaymentStatus> JobMainPaymentStatus { get; set; }
        public virtual DbSet<InvItemCommi> InvItemCommis { get; set; }
        public virtual DbSet<JobPaymentType> JobPaymentTypes { get; set; }
        public virtual DbSet<JobPostSalesStatus> JobPostSalesStatus { get; set; }
        public virtual DbSet<SvcGroup> SvcGroups { get; set; }
        public virtual DbSet<SvcDetail> SvcDetails { get; set; }
        public virtual DbSet<CustEntActPostSale> CustEntActPostSales { get; set; }
        public virtual DbSet<CustEntActPostSaleStatus> CustEntActPostSaleStatus { get; set; }
        public virtual DbSet<CustEntActActionCodes> CustEntActActionCodes { get; set; }
        public virtual DbSet<CustEntActActionStatus> CustEntActActionStatus { get; set; }
        public virtual DbSet<CarDetail> CarDetails { get; set; }
        public virtual DbSet<CarResType> CarResTypes { get; set; }
        public virtual DbSet<SalesLeadSupActivity> SalesLeadSupActivities { get; set; }
        public virtual DbSet<SalesProcStatus> SalesProcStatus { get; set; }
        public virtual DbSet<SalesProcStatusCode> SalesProcStatusCodes { get; set; }
        public virtual DbSet<SupplierActActionCode> SupplierActActionCodes { get; set; }
        public virtual DbSet<SupplierActActionStatus> SupplierActActionStatus { get; set; }
        public virtual DbSet<SalesLeadQuotedItemStatus> SalesLeadQuotedItemStatus { get; set; }
        public virtual DbSet<SalesStatusType> SalesStatusTypes { get; set; }
        public virtual DbSet<SalesStatusRestriction> SalesStatusRestrictions { get; set; }
        public virtual DbSet<SalesStatusAllowedUsers> SalesStatusAllowedUsers { get; set; }
        public virtual DbSet<SalesStatusStatus> SalesStatusStatus { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<SalesLeadFile> SalesLeadFiles { get; set; }
        public virtual DbSet<InvItemCrLogUnit> InvItemCrLogUnits { get; set; }
        public virtual DbSet<InvCarMntRcmd> InvCarMntRcmds { get; set; }
        public virtual DbSet<InvCarMntPriority> InvCarMntPriorities { get; set; }
        public virtual DbSet<InvCarRcmdStatus> InvCarRcmdStatus { get; set; }
        public virtual DbSet<InvCarRcmdRequest> InvCarRcmdRequests { get; set; }
        public virtual DbSet<CustAssocType> CustAssocTypes { get; set; }


        public virtual DbSet<cSupplierList> cSupplierLists { get; set; }
        public virtual DbSet<cProductList> cProductLists { get; set; }
        public virtual DbSet<cSupplierItem> cSupplierItems { get; set; }
        public virtual DbSet<cJobConfirmed> cJobConfirmeds { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Necessary, since our model isnt a EF model
            modelBuilder.Entity<cSupplierList>(entity =>
            {
                entity.HasNoKey();
            });


            modelBuilder.Entity<cJobConfirmed>().HasNoKey();
        }

    }
}
