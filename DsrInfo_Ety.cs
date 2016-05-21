using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Controller_Crm
{
    public class DsrInfo_Ety
    {
    }

    [Serializable()]
    public partial class Dsr_Ety
    {
        //@FromDt Date,@ToDt Date,@Mode varchar(50), @SoId bigint,@UserId bigint
        public string FromDt { get; set; }
        public string ToDt { get; set; }
        public Int64 UserId { get; set; }
        public Int64 SoId { get; set; }
        public string Mode { get; set; }
        public string UserType { get; set; }
    }

    [Serializable()]
    public partial class CRM_Created_user_Ety
    {
        public Int64 Createdby { get; set; }
        public DateTime CreatedDate { get; set; }
        public Int64 Modifiedby { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Int64 AuthorizedBy { get; set; }
        public DateTime AuthorizedDate { get; set; }
    }

    [Serializable()]
    public partial class CRM_ConExecStatus_Info_Ety
    {
        public string Confirm { get; set; }
        public byte ExecStatus { get; set; }
    }

    [Serializable()]
    public partial class CRM_RowMode_Ety
    {
        public string RoMode { get; set; }
    }

    [Serializable()]
    public partial class Dsr_Details_Ety
    {
        public string DsrLeadId { get; set; }
        public string TaskId { get; set; }
        public string DsrSlnoAuth { get; set; }
        public string NextVisitOpt { get; set; }
        public string DsrAuth { get; set; }
        public string Nextvisit { get; set; }
        public decimal totCollAmt { get; set; }
        public decimal totPoAmt { get; set; }
        public Int64 SecurityId { get; set; }
        public Int16 CmpId { get; set; }
        public Int16 FinYearId { get; set; }
        public Int64 UsrId { get; set; }
        public Int16 ExecStatus { get; set; }
        public string Mode { get; set; }
        public string rbtnAuth { get; set; }
        public string Remarks { get; set; }
        public string ARemarks { get; set; }

        public string CollAmt { get; set; }
        public string POAmount { get; set; }
        public string Reason { get; set; }
        public string DsrCntPersnDesig { get; set; }
        public string DsrCntName { get; set; }
        public string DsrAccFlag { get; set; }
        public string DsrAccName { get; set; }

        public string DsrAccId { get; set; }
        public string Dsrtotime { get; set; }
        public string Dsrfromtime { get; set; }
        public string DsrSlno { get; set; }
        public Int64 DsrOwner { get; set; }
        public DateTime DsrDate { get; set; }
        public Int64 DsrId { get; set; }
        public Int32 AuthOpt { get; set; }

        public string Result { get; set; }
        public Int64 ReasonId { get; set; }
        public DateTime Authdate { get; set; }
        public string Discontinue { get; set; }
        public CRM_RowMode_Ety crm_RowMode_ety { get; set; }

    }
}
