using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Controller_Crm;

namespace Controller_Crm
{
    public class UserInfo : IDisposable
    {
        private IntPtr handle;
        private bool disposed = false;
        public UserInfo(IntPtr handle) { this.handle = handle; }
        public void Dispose() { Dispose(true); GC.SuppressFinalize(this); }
        private void Dispose(bool disposing)
        {
            if (!this.disposed) { CloseHandle(handle); handle = IntPtr.Zero; disposed = true; }
        }
        [System.Runtime.InteropServices.DllImport("Kernel32")]
        private extern static Boolean CloseHandle(IntPtr handle);
        ~UserInfo() { Dispose(false); }
        public UserInfo()
        {
        }
        public DataTable Get_Emp_CRM_UserDetails(Int64 EmpId)
        {
            using (UserInfo_DAC dac = new UserInfo_DAC())
            {
                return dac.Get_Emp_CRM_UserDetails(EmpId);
            }
        }

        public DataTable Get_FinancialYearsList(Int16 CompId)
        {
            using (UserInfo_DAC dac = new UserInfo_DAC())
            {
                return dac.Get_FinancialYearsList(CompId);
            }
        }
        public DataTable Get_Company_Rights(Int64 UserId, Int16 FinId)
        {
            using (UserInfo_DAC dac = new UserInfo_DAC())
            {
                return dac.Get_Company_Rights(UserId, FinId);
            }
        }
        public DataTable GetCompaniesList(Int64 UserId, Int16 FinId)
        {
            using (UserInfo_DAC dac = new UserInfo_DAC())
            {
                return dac.GetCompaniesList(UserId, FinId);
            }
        }
        public DataTable GetMenuOptions()
        {
            using (UserInfo_DAC dac = new UserInfo_DAC())
            {
                return dac.GetMenuOptions();
            }
        }
        public DataTable getHrmsMenuEnableDisable(Int64 UserId, Int16 FinId)
        {
            using (UserInfo_DAC dac = new UserInfo_DAC())
            {
                return dac.getHrmsMenuEnableDisable(UserId, FinId);
            }
        }
        public DataTable GetUserRightsForMenu(Int64 UserId, Int16 FinId)
        {
            using (UserInfo_DAC dac = new UserInfo_DAC())
            {
                return dac.GetUserRightsForMenu(UserId, FinId);
            }
        }
        public DataTable GetUserTransationRights(Int64 UserId, Int16 FinId)
        {
            using (UserInfo_DAC dac = new UserInfo_DAC())
            {
                return dac.GetUserTransationRights(UserId, FinId);
            }
        }
        public DataTable GetUserRights(Int64 UserId, Int16 FinId, Int64 SecurityId)
        {
            using (UserInfo_DAC dac = new UserInfo_DAC())
            {
                return dac.GetUserRights(UserId, FinId, SecurityId);
            }
        }

        public DataTable getpendingRecords(Int64 UserId, string UserType)
        {
            using (UserInfo_DAC dac = new UserInfo_DAC())
            {
                return dac.getpendingRecords(UserId, UserType);
            }
        }
        public DataTable getSalesData(DateTime FromDate, DateTime ToDate, Int64 UserId, string UserType)
        {
            using (UserInfo_DAC dac = new UserInfo_DAC())
            {
                return dac.getSalesData(FromDate, ToDate, UserId, UserType);
            }
        }
        public DataTable getTaskDetails(DateTime FromDate, DateTime ToDate, Int64 UserId, string UserType)
        {
            using (UserInfo_DAC dac = new UserInfo_DAC())
            {
                return dac.getTaskDetails(FromDate, ToDate, UserId, UserType);
            }
        }
        public DataTable getTaskDetails(DateTime FromDate, DateTime ToDate, Int64 UserId, string UserType, string Mode)
        {
            using (UserInfo_DAC dac = new UserInfo_DAC())
            {
                return dac.getTaskDetails(FromDate, ToDate, UserId, UserType, Mode);
            }
        }
        public DataTable getTaskDetailsData(string Mode, Int32 TaskOwnerId)
        {
            using (UserInfo_DAC dac = new UserInfo_DAC())
            {
                return dac.getTaskDetailsData(Mode, TaskOwnerId);
            }
        }
        public object Get_Employee_Id(Int64 UserId)
        {
            using (UserInfo_DAC dac = new UserInfo_DAC())
            {
                return dac.Get_Employee_Id(UserId);
            }
        }
    }
}
