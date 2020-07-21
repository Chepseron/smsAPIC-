using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MAAIF.Models
{
    public class User
    {

        public string longitude { get; set; }

        public string DealerID { get; set; }
        public string latitude { get; set; }
        public string bank_id { get; set; }
        public string source_id { get; set; }

        public string Code { get; set; }

        public string Parish { get; set; }
        public string Name { get; set; }
        public string Cluster { get; set; }

        public string NewPassword { get; set; }
        public string District { get; set; }
        public string Rollout { get; set; }
        public string SMSType { get; set; }
        public string Stream { get; set; }
        public string cust_title_code { get; set; }
        public string Subsidy { get; set; }
        public string FarmerPayment { get; set; }
        public string AccountNumber { get; set; }
        public string SubCounty { get; set; }
        public string FarmerGroup { get; set; }
        public string Location { get; set; }
        public string cust_cntry_code { get; set; }
        public DateTime dob { get; set; }
        public string parish { get; set; }
        public string ac_manager_code { get; set; }
        public string sol_id { get; set; }
        public string schm_code { get; set; }
        public string gl_sub_head_code { get; set; }
        public string crncy_code { get; set; }
        public string BillerID { get; set; }
        //agrodelaer 
        public string AgrodealerCo { get; set; }
        public string AgroProduct { get; set; }
        public string AgroSignatories { get; set; }
        public string OtherProducts { get; set; }
        public string SubDealerNumber { get; set; }
        public string SubDealerLocation { get; set; }
        public string PrimaryDesign { get; set; }
        public string PrimaryEmail { get; set; }
        public string SecondaryName { get; set; }
        public string SecondaryDesign { get; set; }
        public string SecondaryOfficeTelephone { get; set; }
        public string SecondaryMobile { get; set; }
        public string SecondaryEmail { get; set; }


        public string DocumentID { get; set; }
        public string IDNumber { get; set; }

        public string marital_status { get; set; }
        public string passport_num { get; set; }
        public string requestid { get; set; }
        public string acct_type { get; set; }
        public string freetext { get; set; }
        public string orderReference { get; set; }








        //payment
        public string TXReference { get; set; }
        public string TXReference1 { get; set; }
        public string TXReference2 { get; set; }
        public string EventID { get; set; }
        public string UserGroup { get; set; }
        public string Password { get; set; }
        public short ModuleID { get; set; }
        public string DetailOld { get; set; }

        public string DetailNew { get; set; }
        public string MerchantCode { get; set; }
        public string ServiceAccountID { get; set; }
        public string TrxType { get; set; }
        public string UniqueID { get; set; }
        public string ToBankAccountID { get; set; }
        public string BankAccountID { get; set; }
        public string PaymentDetails { get; set; }
        public string Amount { get; set; }
        public string MerchantID { get; set; }
        public string CustomerID { get; set; }
        public string TipAmount { get; set; }
        public string DiscountAmount { get; set; }
        public string ChargesToCustomer { get; set; }
        public string ChargesToMerchant { get; set; }
        public string Status { get; set; }
        public string ExtraDetails { get; set; }





        public string Category { get; set; }

        public string UserID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string MobileNumber { get; set; }
        public string NewMobileNumber { get; set; }
        public string EmailID { get; set; }
        public string TypeOfID { get; set; }

        public string ControlDate { get; set; }
        public string LastPasswordChangedOn { get; set; }
        public string Remarks { get; set; }
        public string LanguageID { get; set; }
        public string Ref1 { get; set; }
        public string Ref2 { get; set; }
        public string IsLoggedIn { get; set; }
        public string TimeLoggedIn { get; set; }
        public string TimeLoggedOut { get; set; }
        public string IPAddress { get; set; }
        public string FailedAttempts { get; set; }
        public string IsLoginDisabled { get; set; }
        public string IsLoginDeleted { get; set; }
        public string ChangePasswordAtNextLogon { get; set; }
        public string ValidTime { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string SupervisedBy { get; set; }
        public string SupervisedOn { get; set; }
        public string SessionID { get; set; }
        public string RegionID { get; set; }
        public string Gender { get; set; }
        public string NewData { get; set; }
        public string OldData { get; set; }
        public string RoleID { get; set; }
        public string RoleModuleID { get; set; }
        public string AllowView { get; set; }
        public string AllowAdd { get; set; }
        public string AllowEdit { get; set; }
        public string AllowDelete { get; set; }
        public string IsSupervisionRequired { get; set; }
        public string AllowSupervision { get; set; }
        public string RoleName { get; set; }
        public string GroupName { get; set; }
        public string refNo { get; set; }
        public string blockedReason { get; set; }
        public string blockedDescrition { get; set; }
    }
}