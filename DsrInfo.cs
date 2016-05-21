using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Controller_Crm;
namespace Controller_Crm
{
    public class DsrInfo : IDisposable
    {
        private IntPtr handle;
        private bool disposed = false;
        public DsrInfo(IntPtr handle) { this.handle = handle; }
        public void Dispose() { Dispose(true); GC.SuppressFinalize(this); }
        private void Dispose(bool disposing)
        {
            if (!this.disposed) { CloseHandle(handle); handle = IntPtr.Zero; disposed = true; }
        }
        [System.Runtime.InteropServices.DllImport("Kernel32")]
        private extern static Boolean CloseHandle(IntPtr handle);
        ~DsrInfo() { Dispose(false); }
        public DsrInfo()
        {
        }

        public DataTable Get_DsrOwner_Details(Int64 UserId, string UserType, string ReportMode)
        {
            using (DsrInfo_DAC dac = new DsrInfo_DAC())
            {
                return dac.Get_DsrOwner_Details(UserId, UserType, ReportMode);
            }
        }
        public DataTable Get_Dsr_Reasons()
        {
            using (DsrInfo_DAC dac = new DsrInfo_DAC())
            {
                return dac.Get_Dsr_Reasons();
            }
        }
        public DataTable Get_Dsr_Details(DateTime FromDt, DateTime ToDt, string Mode, Int64 SoId, Int64 UserId)
        {
            using (DsrInfo_DAC dac = new DsrInfo_DAC())
            {
                return dac.Get_Dsr_Details(FromDt, ToDt, Mode, SoId, UserId);
            }
        }

        public DataTable Get_Dsr_Data(Int64 DsrId, string Mode, string TypeFlag)
        {
            using (DsrInfo_DAC dac = new DsrInfo_DAC())
            {
                return dac.Get_Dsr_Data(DsrId, Mode, TypeFlag);
            }
        }
        public DataTable Get_Dsr_PendingDetails(DateTime FromDt, DateTime ToDt, string Mode, Int64 SoId)
        {
            using (DsrInfo_DAC dac = new DsrInfo_DAC())
            {
                return dac.Get_Dsr_PendingDetails(FromDt, ToDt, Mode, SoId);
            }
        }
        public DataTable Get_Dsr_DateSO_Details(Int64 DsrId, string Mode, Int64 UserId)
        {
            using (DsrInfo_DAC dac = new DsrInfo_DAC())
            {
                return dac.Get_Dsr_DateSO_Details(DsrId, Mode, UserId);
            }
        }
        public DataTable Get_Dsr_LPA_Name_Details(string PrefixText, string LPA_Flag, Int64 SoId)
        {
            using (DsrInfo_DAC dac = new DsrInfo_DAC())
            {
                return dac.Get_Dsr_LPA_Name_Details(PrefixText, LPA_Flag, SoId);
            }
        }
        public DataTable Get_Existing_Types(string PrefixText, string Type, Int64 SoId)
        {
            using (DsrInfo_DAC dac = new DsrInfo_DAC())
            {
                return dac.Get_Existing_Types(PrefixText, Type, SoId);
            }
        }

        public DataTable Get_Dsr_AllContacts(string LPA_Flag, Int64 LPA_Id)
        {
            using (DsrInfo_DAC dac = new DsrInfo_DAC())
            {
                return dac.Get_Dsr_AllContacts(LPA_Flag, LPA_Id);
            }
        }

        public CRM_ConExecStatus_Info_Ety Save_Dsr_Details(Dsr_Details_Ety dsr_ety)
        {
            using (DsrInfo_DAC dac = new DsrInfo_DAC())
            {
                return dac.Save_Dsr_Details(dsr_ety);
            }
        }

        public CRM_ConExecStatus_Info_Ety Delete_Dsr_Details(Dsr_Details_Ety dsr_ety)
        {
            using (DsrInfo_DAC dac = new DsrInfo_DAC())
            {
                return dac.Delete_Dsr_Details(dsr_ety);
            }
        }

        public CRM_ConExecStatus_Info_Ety Authorize_Dsr_Details(Dsr_Details_Ety dsr_ety)
        {
            using (DsrInfo_DAC dac = new DsrInfo_DAC())
            {
                return dac.Authorize_Dsr_Details(dsr_ety);
            }
        }
        public DataTable Get_KW_Dsr_AccInfo(string PrefixText, string ContexKey)
        {
            using (DsrInfo_DAC dac = new DsrInfo_DAC())
            {
                return dac.Get_KW_Dsr_AccInfo(PrefixText, ContexKey);
            }
        }

    }
}
