using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Controller_Crm;
using System.Web;
namespace Controller_Crm
{
    internal class UserInfo_DAC : IDisposable
    {
        // Pointer to an external unmanaged resource.
        private IntPtr handle;
        // Track whether Dispose has been called.
        private bool disposed = false;

        //EntityConnection cn1;
        SqlConnection cn, cn1;
        string res;

        int n;
        internal UserInfo_DAC()
        {
            cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Crm_CommonDbStr"].ConnectionString);
            //cn = new SqlConnection("Data source=192.168.0.90;Database=db_repository;user id=sa;password=pwd;Timeout=360;Connection reset=false");
            cn1 = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Crm_ConnStr"].ConnectionString);
        }
        public enum MsgType
        {
            AlertMessage, Information, Error
        };
        public UserInfo_DAC(IntPtr handle)
        {
            this.handle = handle;
        }
        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }
        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be disposed.
        // If disposing equals false, the method has been called by the
        // runtime from inside the finalizer and you should not reference
        // other objects. Only unmanaged resources can be disposed.
        private void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // Call the appropriate methods to clean up
                // unmanaged resources here.
                // If disposing is false,
                // only the following code is executed.
                CloseHandle(handle);
                handle = IntPtr.Zero;

                // Note disposing has been done.
                disposed = true;
            }
        }

        // Use interop to call the method necessary
        // to clean up the unmanaged resource.
        [System.Runtime.InteropServices.DllImport("Kernel32")]
        private extern static Boolean CloseHandle(IntPtr handle);

        // Use C# destructor syntax for finalization code.
        // This destructor will run only if the Dispose method
        // does not get called.
        // It gives your base class the opportunity to finalize.
        // Do not provide destructors in types derived from this class.

        ~UserInfo_DAC()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            Dispose(false);
        }
        internal DataTable Get_Emp_CRM_UserDetails(Int64 EmpId)
        {
            cn.Open();
            DataTable dt = new DataTable();
            string strProc = string.Empty;
            try
            {
                SqlParameter _EmpId = new SqlParameter("@EmployeeId", SqlDbType.BigInt);
                _EmpId.Value = EmpId;
                SqlParameter[] paramFields = { _EmpId };

                strProc = "CRM_Get_Employee_CRMUserDetails_Proc";
                if (strProc != string.Empty)
                {
                    SqlDataReader sdr = SqlHelper.ExecuteReader(cn, CommandType.StoredProcedure, strProc, paramFields);
                    dt.Load(sdr);

                    if (!sdr.IsClosed)
                        sdr.Close();
                }
            }
            catch (Exception msg)
            {
                cn.Close();
                //res = LogException(msg, "Hrms_GetCustWizardDetails() Procedure=" + strProc, null);
                if (res == null && res.ToString() == "")
                    res = msg.Message;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close();
                if (cn != null) cn.Dispose();
            }
            return dt;
        }
        internal DataTable Get_FinancialYearsList(Int16 CompId)
        {
            cn.Open();
            DataTable dt = new DataTable();
            string strProc = string.Empty;
            try
            {
                SqlParameter _CompId = new SqlParameter("@CompId", SqlDbType.SmallInt);
                _CompId.Value = CompId;
                SqlParameter[] paramFields = { _CompId };

                strProc = "CRM_GetFinYearList_proc";
                if (strProc != string.Empty)
                {
                    SqlDataReader sdr = SqlHelper.ExecuteReader(cn, CommandType.StoredProcedure, strProc, paramFields);
                    dt.Load(sdr);

                    if (!sdr.IsClosed)
                        sdr.Close();
                }
            }
            catch (Exception msg)
            {
                cn.Close();
                //res = LogException(msg, "Hrms_GetCustWizardDetails() Procedure=" + strProc, null);
                if (res == null && res.ToString() == "")
                    res = msg.Message;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close();
                if (cn != null) cn.Dispose();
            }
            return dt;
        }
        internal DataTable Get_Company_Rights(Int64 UserId, Int16 FinId)
        {
            cn.Open();
            DataTable dt = new DataTable();
            string strProc = string.Empty;
            try
            {
                SqlParameter _UserId = new SqlParameter("@UsrId", SqlDbType.BigInt);
                _UserId.Value = UserId;

                SqlParameter _FinId = new SqlParameter("@FinId", SqlDbType.SmallInt);
                _FinId.Value = FinId;
                SqlParameter[] paramFields = { _UserId, _FinId };

                strProc = "CRM_Get_CompanyRights_proc";
                if (strProc != string.Empty)
                {
                    SqlDataReader sdr = SqlHelper.ExecuteReader(cn, CommandType.StoredProcedure, strProc, paramFields);
                    dt.Load(sdr);

                    if (!sdr.IsClosed)
                        sdr.Close();
                }
            }
            catch (Exception msg)
            {
                cn.Close();
                //res = LogException(msg, "Hrms_GetCustWizardDetails() Procedure=" + strProc, null);
                if (res == null && res.ToString() == "")
                    res = msg.Message;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close();
                if (cn != null) cn.Dispose();
            }
            return dt;
        }
        internal DataTable GetCompaniesList(Int64 UserId, Int16 FinId)
        {
            string str = "Select distinct a.Comp_Id,a.Comp_Desc,b.CS,b.KW from CRM_CompanyMast_Tbl a, " +
                 "CRM_CompRights_Tbl b Where b.Usr_Id = " + UserId + " and a.Comp_Id = b.Comp_Id and UtDbName='S' and FinYear_Id='" + FinId + "'";
            //return DAL.ExecuteDataset(ConfigurationManager.ConnectionStrings["CommonDb"].ConnectionString, CommandType.Text, strQry).Tables[0];
            cn.Open();
            DataTable dt = new DataTable();
            string strProc = string.Empty;
            try
            {
                //SqlParameter _UserId = new SqlParameter("@UsrId", SqlDbType.BigInt);
                //_UserId.Value = UserId;

                //SqlParameter _FinId = new SqlParameter("@FinId", SqlDbType.SmallInt);
                //_FinId.Value = FinId;
                //SqlParameter[] paramFields = { _UserId, _FinId };

                //strProc = "CRM_Get_CompanyRights_proc";
                //if (strProc != string.Empty)
                //{

                SqlDataReader sdr = SqlHelper.ExecuteReader(cn, CommandType.Text, str);
                //SqlDataReader sdr = SqlHelper.ExecuteReader(cn, CommandType.StoredProcedure, strProc, paramFields);
                dt.Load(sdr);

                if (!sdr.IsClosed)
                    sdr.Close();
                //}
            }
            catch (Exception msg)
            {
                cn.Close();
                //res = LogException(msg, "Hrms_GetCustWizardDetails() Procedure=" + strProc, null);
                if (res == null && res.ToString() == "")
                    res = msg.Message;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close();
                if (cn != null) cn.Dispose();
            }
            return dt;
        }
        internal DataTable GetMenuOptions()
        {
            string str = "SELECT MenuId,MenuName Menu_Name,ParentId,Path PageLink,Menu_index,Toolbox_Image,Security_Id "
                + "FROM CRM_MenuMaster_tbl where discontinue ='N' order by parentId ";

            cn1.Open();
            DataTable dt = new DataTable();
            string strProc = string.Empty;
            try
            {
                //SqlParameter _UserId = new SqlParameter("@UsrId", SqlDbType.BigInt);
                //_UserId.Value = UserId;

                //SqlParameter _FinId = new SqlParameter("@FinId", SqlDbType.SmallInt);
                //_FinId.Value = FinId;
                //SqlParameter[] paramFields = { _UserId, _FinId };

                //strProc = "CRM_Get_CompanyRights_proc";
                //if (strProc != string.Empty)
                //{

                SqlDataReader sdr = SqlHelper.ExecuteReader(cn1, CommandType.Text, str);
                //SqlDataReader sdr = SqlHelper.ExecuteReader(cn, CommandType.StoredProcedure, strProc, paramFields);
                dt.Load(sdr);

                if (!sdr.IsClosed)
                    sdr.Close();
                //}
            }
            catch (Exception msg)
            {
                cn1.Close();
                //res = LogException(msg, "Hrms_GetCustWizardDetails() Procedure=" + strProc, null);
                if (res == null && res.ToString() == "")
                    res = msg.Message;
            }
            finally
            {
                if (cn1.State == ConnectionState.Open)
                    cn1.Close();
                if (cn1 != null) cn1.Dispose();
            }
            return dt;
        }
        internal DataTable getHrmsMenuEnableDisable(Int64 UserId, Int16 FinId)
        {
            string str = "SELECT s.Usr_Id,m.MenuId,m.MenuName Menu_Name,ParentId,Path PageLink,Menu_index,Toolbox_Image,m.Security_Id "
                + " FROM CRM_MenuMaster_tbl m inner join CRM_UsrRights_Tbl s on m.Security_Id =s.Security_Id "
                + " where s.Usr_Id=" + UserId + " AND s.FinYear_Id=" + FinId + " and s.ViewP='Y' ";

            cn1.Open();
            DataTable dt = new DataTable();
            string strProc = string.Empty;
            try
            {
                //SqlParameter _UserId = new SqlParameter("@UsrId", SqlDbType.BigInt);
                //_UserId.Value = UserId;

                //SqlParameter _FinId = new SqlParameter("@FinId", SqlDbType.SmallInt);
                //_FinId.Value = FinId;
                //SqlParameter[] paramFields = { _UserId, _FinId };

                //strProc = "CRM_Get_CompanyRights_proc";
                //if (strProc != string.Empty)
                //{

                SqlDataReader sdr = SqlHelper.ExecuteReader(cn1, CommandType.Text, str);
                //SqlDataReader sdr = SqlHelper.ExecuteReader(cn, CommandType.StoredProcedure, strProc, paramFields);
                dt.Load(sdr);

                if (!sdr.IsClosed)
                    sdr.Close();
                //}
            }
            catch (Exception msg)
            {
                cn1.Close();
                //res = LogException(msg, "Hrms_GetCustWizardDetails() Procedure=" + strProc, null);
                if (res == null && res.ToString() == "")
                    res = msg.Message;
            }
            finally
            {
                if (cn1.State == ConnectionState.Open)
                    cn1.Close();
                if (cn1 != null) cn1.Dispose();
            }
            return dt;

        }
        internal DataTable GetUserRightsForMenu(Int64 UserId, Int16 FinId)
        {
            string str = "SELECT Security_Id,Usr_Id,Comp_Id,FinYear_Id,Flag,Format_Id,PageLink, "
            + "AddP,ModifyP,EraseP,ViewP,EnableP,PrintP,AuthPA,Authpm,SelfAuth FROM dbo.CRM_UsrRights_Tbl "
            + "WHERE flag='M' AND finYear_Id=" + FinId + " and usr_id=" + UserId + " ";

            cn1.Open();
            DataTable dt = new DataTable();
            string strProc = string.Empty;
            try
            {
                //SqlParameter _UserId = new SqlParameter("@UsrId", SqlDbType.BigInt);
                //_UserId.Value = UserId;

                //SqlParameter _FinId = new SqlParameter("@FinId", SqlDbType.SmallInt);
                //_FinId.Value = FinId;
                //SqlParameter[] paramFields = { _UserId, _FinId };

                //strProc = "CRM_Get_CompanyRights_proc";
                //if (strProc != string.Empty)
                //{

                SqlDataReader sdr = SqlHelper.ExecuteReader(cn1, CommandType.Text, str);
                //SqlDataReader sdr = SqlHelper.ExecuteReader(cn, CommandType.StoredProcedure, strProc, paramFields);
                dt.Load(sdr);

                if (!sdr.IsClosed)
                    sdr.Close();
                //}
            }
            catch (Exception msg)
            {
                cn1.Close();
                //res = LogException(msg, "Hrms_GetCustWizardDetails() Procedure=" + strProc, null);
                if (res == null && res.ToString() == "")
                    res = msg.Message;
            }
            finally
            {
                if (cn1.State == ConnectionState.Open)
                    cn1.Close();
                if (cn1 != null) cn1.Dispose();
            }
            return dt;
        }
        internal DataTable GetUserTransationRights(Int64 UserId, Int16 FinId)
        {
            string str = " SELECT Security_Id,Usr_Id,Comp_Id,FinYear_Id,Flag,Format_Id,PageLink,AddP,ModifyP,EraseP,ViewP,EnableP, "
            + " PrintP,FinUpdate,StkUpdate,Vat_Need,BackDated_Rights,AuthPA,Authpm,Checked "
            + " FROM dbo.CRM_UsrRights_view v "
            + " INNER JOIN Crm_CustWizardDetails_tbl f ON v.Format_Id =f.BDformatId "
            + " WHERE flag='T' AND v.ViewP ='Y' AND finYear_Id=" + FinId + " AND usr_id=" + UserId + "";
            cn1.Open();
            DataTable dt = new DataTable();
            string strProc = string.Empty;
            try
            {
                //SqlParameter _UserId = new SqlParameter("@UsrId", SqlDbType.BigInt);
                //_UserId.Value = UserId;

                //SqlParameter _FinId = new SqlParameter("@FinId", SqlDbType.SmallInt);
                //_FinId.Value = FinId;
                //SqlParameter[] paramFields = { _UserId, _FinId };

                //strProc = "CRM_Get_CompanyRights_proc";
                //if (strProc != string.Empty)
                //{

                SqlDataReader sdr = SqlHelper.ExecuteReader(cn1, CommandType.Text, str);
                //SqlDataReader sdr = SqlHelper.ExecuteReader(cn, CommandType.StoredProcedure, strProc, paramFields);
                dt.Load(sdr);

                if (!sdr.IsClosed)
                    sdr.Close();
                //}
            }
            catch (Exception msg)
            {
                cn1.Close();
                //res = LogException(msg, "Hrms_GetCustWizardDetails() Procedure=" + strProc, null);
                if (res == null && res.ToString() == "")
                    res = msg.Message;
            }
            finally
            {
                if (cn1.State == ConnectionState.Open)
                    cn1.Close();
                if (cn1 != null) cn1.Dispose();
            }
            return dt;
        }

        internal DataTable GetUserRights(Int64 UserId, Int16 FinId, Int64 SecurityId)
        {
            string str = "SELECT Security_Id,Usr_Id,Comp_Id,FinYear_Id,Flag,Format_Id,PageLink, "
            + "AddP,ModifyP,EraseP,ViewP,EnableP,PrintP,AuthPA,Authpm,SelfAuth FROM dbo.CRM_UsrRights_Tbl "
            + "WHERE flag='M' AND finYear_Id=" + FinId + " and usr_id=" + UserId + " AND Security_Id=" + SecurityId + " ";

            cn1.Open();
            DataTable dt = new DataTable();
            string strProc = string.Empty;
            try
            {
                //SqlParameter _UserId = new SqlParameter("@UsrId", SqlDbType.BigInt);
                //_UserId.Value = UserId;

                //SqlParameter _FinId = new SqlParameter("@FinId", SqlDbType.SmallInt);
                //_FinId.Value = FinId;
                //SqlParameter[] paramFields = { _UserId, _FinId };

                //strProc = "CRM_Get_CompanyRights_proc";
                //if (strProc != string.Empty)
                //{

                SqlDataReader sdr = SqlHelper.ExecuteReader(cn1, CommandType.Text, str);
                //SqlDataReader sdr = SqlHelper.ExecuteReader(cn, CommandType.StoredProcedure, strProc, paramFields);
                dt.Load(sdr);

                if (!sdr.IsClosed)
                    sdr.Close();
                //}
            }
            catch (Exception msg)
            {
                cn1.Close();
                //res = LogException(msg, "Hrms_GetCustWizardDetails() Procedure=" + strProc, null);
                if (res == null && res.ToString() == "")
                    res = msg.Message;
            }
            finally
            {
                if (cn1.State == ConnectionState.Open)
                    cn1.Close();
                if (cn1 != null) cn1.Dispose();
            }
            return dt;
        }

        internal DataTable getpendingRecords(Int64 UserId, string UserType)
        {
            DataTable dt = new DataTable();
            cn1.Open();
            string strProc = string.Empty;
            try
            {
                SqlParameter _UserId = new SqlParameter("@UserId", SqlDbType.SmallInt);
                _UserId.Value = Convert.ToInt16(UserId);

                SqlParameter _UserType = new SqlParameter("@UserType", SqlDbType.VarChar, 20);
                _UserType.Value = UserType;

                SqlParameter[] paramFields = { _UserId, _UserType };

                strProc = "Crm_getPendingRecords_Proc";
                if (strProc != string.Empty)
                {
                    SqlDataReader sdr = SqlHelper.ExecuteReader(cn1, CommandType.StoredProcedure, strProc, paramFields);
                    dt.Load(sdr);

                    if (!sdr.IsClosed)
                        sdr.Close();
                }
            }
            catch (Exception msg)
            {
                cn1.Close();
                //res = LogException(msg, "Hrms_GetCustWizardDetails() Procedure=" + strProc, null);
                if (res == null && res.ToString() == "")
                    res = msg.Message;
            }
            finally
            {
                if (cn1.State == ConnectionState.Open)
                    cn1.Close();
                if (cn1 != null) cn1.Dispose();
            }

            return dt;
        }

        internal DataTable getSalesData(DateTime FromDate, DateTime ToDate, Int64 UserId, string UserType)
        {
            DataTable dt = new DataTable();
            cn1.Open();
            string strProc = string.Empty;
            try
            {
                SqlParameter _FromDate, _ToDate;
                _FromDate = new SqlParameter("@FromDate", SqlDbType.DateTime);
                _FromDate.Value = FromDate;

                _ToDate = new SqlParameter("@ToDate", SqlDbType.DateTime);
                _ToDate.Value = ToDate;

                SqlParameter _UserId = new SqlParameter("@UserId", SqlDbType.SmallInt);
                _UserId.Value = Convert.ToInt16(UserId);

                SqlParameter _UserType = new SqlParameter("@UserType", SqlDbType.VarChar, 20);
                _UserType.Value = UserType;
                SqlParameter[] paramFields = { _FromDate, _ToDate, _UserId, _UserType };

                strProc = "Crm_GetSalesData_Proc";
                if (strProc != string.Empty)
                {
                    SqlDataReader sdr = SqlHelper.ExecuteReader(cn1, CommandType.StoredProcedure, strProc, paramFields);
                    dt.Load(sdr);

                    if (!sdr.IsClosed)
                        sdr.Close();
                }
            }
            catch (Exception msg)
            {
                cn1.Close();
                //res = LogException(msg, "Hrms_GetCustWizardDetails() Procedure=" + strProc, null);
                if (res == null && res.ToString() == "")
                    res = msg.Message;
            }
            finally
            {
                if (cn1.State == ConnectionState.Open)
                    cn1.Close();
                if (cn1 != null) cn1.Dispose();
            }

            return dt;
        }

        internal DataTable getTaskDetails(DateTime FromDate, DateTime ToDate, Int64 UserId, string UserType)
        {
            string str = string.Empty;
            if (UserType == "SuperAdmin" || UserType == "Admin")
            {
                str = " select t.TaskId,t.TaskSubject Subject,t.TaskDate,t.DueDate,Priority,Status,t.Description,TaskForType,"
                    + " (CASE  TaskForType  WHEN 'L' THEN l.CompanyName WHEN 'A' THEN a.AccName WHEN 'P' THEN p.PotentialName ELSE '' END) RelatedTo, "
                    + " t.ContactPerson,dbo.getusername(t.CreatedBy) CreatedBy,t.CreatedDate,"
                    + "dbo.getusername(t.ModifiedBy) ModifiedBy,t.ModifiedDate "
                    + " from CRM_TaskMast_Tbl t  "
                    + " LEFT JOIN CRM_LeadMast_Tbl l ON l.LeadId=t.TaskTypeId AND  t.TaskForType='L' "
                    + " LEFT JOIN CRM_POtentialMast_Tbl p ON p.PotentialId=t.TaskTypeId AND  t.TaskForType='P' "
                    + " LEFT JOIN CRM_Accmast_View a ON a.acc_id=t.TaskTypeId AND  t.TaskForType='A' "
                    + " WHERE convert(date,t.TaskDate) between '" + FromDate + "' and '" + ToDate + "'";
            }
            else
            {
                str = " select t.TaskId,t.TaskSubject Subject,t.TaskDate,t.DueDate,Priority,Status,t.Description,TaskForType,"
                    + " (CASE  TaskForType  WHEN 'L' THEN l.CompanyName WHEN 'A' THEN a.AccName WHEN 'P' THEN p.PotentialName ELSE '' END) RelatedTo, "
                    + " t.ContactPerson,dbo.getusername(t.CreatedBy) CreatedBy,t.CreatedDate,"
                    + "dbo.getusername(t.ModifiedBy) ModifiedBy,t.ModifiedDate "
                    + " from CRM_TaskMast_Tbl t  INNER JOIN CRM_UserMast_Tbl c ON t.TaskOwnerId=c.Usr_Id "
                    + " LEFT JOIN CRM_LeadMast_Tbl l ON l.LeadId=t.TaskTypeId AND  t.TaskForType='L' "
                    + " LEFT JOIN CRM_POtentialMast_Tbl p ON p.PotentialId=t.TaskTypeId AND  t.TaskForType='P' "
                    + " LEFT JOIN CRM_Accmast_View a ON a.acc_id=t.TaskTypeId AND  t.TaskForType='A' "
                    + " WHERE Usr_ActiveFlag='N' AND usr_type='User' AND (ReportingTo =" + UserId + "  OR "
                    + " ReportingTo IN (SELECT userid FROM dbo.GetUserChild(" + UserId + ")) or "
                    + " usr_id = " + UserId + ") AND Parent_Id NOT IN(5) "
                    + " and convert(date,t.TaskDate) between '" + FromDate + "' and '" + ToDate + "'";
            }

            cn1.Open();
            DataTable dt = new DataTable();
            string strProc = string.Empty;
            try
            {
                //SqlParameter _UserId = new SqlParameter("@UsrId", SqlDbType.BigInt);
                //_UserId.Value = UserId;

                //SqlParameter _FinId = new SqlParameter("@FinId", SqlDbType.SmallInt);
                //_FinId.Value = FinId;
                //SqlParameter[] paramFields = { _UserId, _FinId };

                //strProc = "CRM_Get_CompanyRights_proc";
                //if (strProc != string.Empty)
                //{

                SqlDataReader sdr = SqlHelper.ExecuteReader(cn1, CommandType.Text, str);
                //SqlDataReader sdr = SqlHelper.ExecuteReader(cn, CommandType.StoredProcedure, strProc, paramFields);
                dt.Load(sdr);

                if (!sdr.IsClosed)
                    sdr.Close();
                //}
            }
            catch (Exception msg)
            {
                cn1.Close();
                //res = LogException(msg, "Hrms_GetCustWizardDetails() Procedure=" + strProc, null);
                if (res == null && res.ToString() == "")
                    res = msg.Message;
            }
            finally
            {
                if (cn1.State == ConnectionState.Open)
                    cn1.Close();
                if (cn1 != null) cn1.Dispose();
            }
            return dt;
        }

        internal DataTable getTaskDetails(DateTime FromDate, DateTime ToDate, Int64 UserId, string UserType, string Mode)
        {
            string str = string.Empty;
            if (UserType == "SuperAdmin" || UserType == "Admin")
            {
                if (Mode == "Mast")
                    str = " select t.TaskId,t.TaskSubject Subject,t.TaskDate,t.DueDate,Priority,Status,t.Description,TaskForType,"
                        + " (CASE  TaskForType  WHEN 'L' THEN l.CompanyName WHEN 'A' THEN a.AccName WHEN 'P' THEN p.PotentialName ELSE '' END) RelatedTo, "
                        + " t.ContactPerson,dbo.getusername(t.CreatedBy) CreatedBy,t.CreatedDate,Convert(VARCHAR,t.createdDate,106) CreatDate,"
                        + " dbo.getusername(t.ModifiedBy) ModifiedBy,t.ModifiedDate,Convert(VARCHAR,t.modifiedDate,106) ModifyDate, "
                        + " dbo.getusername(t.TaskOwnerId) TaskOwnerId"
                        + " from CRM_TaskMast_Tbl t  "
                        + " LEFT JOIN CRM_LeadMast_Tbl l ON l.LeadId=t.TaskTypeId AND  t.TaskForType='L' "
                        + " LEFT JOIN CRM_POtentialMast_Tbl p ON p.PotentialId=t.TaskTypeId AND  t.TaskForType='P' "
                        + " LEFT JOIN CRM_Accmast_View a ON a.acc_id=t.TaskTypeId AND  t.TaskForType='A'"
                        + " WHERE convert(date,t.CreatedDate) between '" + FromDate + "' and '" + ToDate + "'"
                        + " ORDER BY t.TaskOwnerId,t.TaskSubject ";

                else if (Mode == "Auth")
                    str = " select t.TaskId,t.TaskSubject Subject,t.TaskDate,t.DueDate,Priority,Status,t.Description,TaskForType,"
                        + " (CASE  TaskForType  WHEN 'L' THEN l.CompanyName WHEN 'A' THEN a.AccName WHEN 'P' THEN p.PotentialName ELSE '' END) RelatedTo, "
                        + " t.ContactPerson,dbo.getusername(t.CreatedBy) CreatedBy,t.CreatedDate,Convert(VARCHAR,t.createdDate,106) CreatDate,"
                        + "dbo.getusername(t.ModifiedBy) ModifiedBy,t.ModifiedDate,Convert(VARCHAR,t.modifiedDate,106) ModifyDate, "
                        + " dbo.getusername(t.TaskOwnerId) TaskOwnerId"
                        + " from CRM_TaskMastAuth_Tbl t  "

                        + " LEFT JOIN CRM_LeadMast_Tbl l ON l.LeadId=t.TaskTypeId AND  t.TaskForType='L' "
                        + " LEFT JOIN CRM_POtentialMast_Tbl p ON p.PotentialId=t.TaskTypeId AND  t.TaskForType='P' "
                        + " LEFT JOIN CRM_Accmast_View a ON a.acc_id=t.TaskTypeId AND  t.TaskForType='A'"
                        + " WHERE convert(date,t.CreatedDate) between '" + FromDate + "' and '" + ToDate + "'"
                        + " ORDER BY t.TaskOwnerId,t.TaskSubject ";


            }
            else
            {
                if (Mode == "Mast")
                    str = " select t.TaskId,t.TaskSubject Subject,t.TaskDate,t.DueDate,Priority,Status,t.Description,TaskForType,"
                    + " (CASE  TaskForType  WHEN 'L' THEN l.CompanyName WHEN 'A' THEN a.AccName WHEN 'P' THEN p.PotentialName ELSE '' END) RelatedTo, "
                    + " t.ContactPerson,dbo.getusername(t.CreatedBy) CreatedBy,t.CreatedDate,Convert(VARCHAR,t.createdDate,106) CreatDate,"
                    + " dbo.getusername(t.ModifiedBy) ModifiedBy,t.ModifiedDate,Convert(VARCHAR,t.modifiedDate,106) ModifyDate, "
                    + " dbo.getusername(t.TaskOwnerId) TaskOwnerId "
                    + " from CRM_TaskMast_Tbl t  INNER JOIN CRM_UserMast_Tbl c ON t.TaskOwnerId=c.Usr_Id "
                    + " LEFT JOIN CRM_LeadMast_Tbl l ON l.LeadId=t.TaskTypeId AND  t.TaskForType='L' "
                    + " LEFT JOIN CRM_POtentialMast_Tbl p ON p.PotentialId=t.TaskTypeId AND  t.TaskForType='P' "
                    + " LEFT JOIN CRM_Accmast_View a ON a.acc_id=t.TaskTypeId AND  t.TaskForType='A' "
                    + " WHERE convert(date,t.CreatedDate) between '" + FromDate + "' and '" + ToDate + "' and "
                    + " Usr_ActiveFlag='N' AND usr_type='User' AND (ReportingTo =" + UserId + "  OR "
                    + " ReportingTo IN (SELECT userid FROM dbo.GetUserChild(" + UserId + ")) or "
                    + " usr_id = " + UserId + ") AND Parent_Id NOT IN(5) ORDER BY t.TaskOwnerId,t.TaskSubject ";
                else if (Mode == "Auth")
                    str = " select t.TaskId,t.TaskSubject Subject,t.TaskDate,t.DueDate,Priority,Status,t.Description,TaskForType,"
                      + " (CASE  TaskForType  WHEN 'L' THEN l.CompanyName WHEN 'A' THEN a.AccName WHEN 'P' THEN p.PotentialName ELSE '' END) RelatedTo, "
                      + " t.ContactPerson,dbo.getusername(t.CreatedBy) CreatedBy,t.CreatedDate,Convert(VARCHAR,t.createdDate,106) CreatDate,"
                      + "dbo.getusername(t.ModifiedBy) ModifiedBy,t.ModifiedDate,Convert(VARCHAR,t.modifiedDate,106) ModifyDate, "
                      + " dbo.getusername(t.TaskOwnerId) TaskOwnerId "
                      + " from CRM_TaskMastAuth_Tbl t  INNER JOIN CRM_UserMast_Tbl c ON t.TaskOwnerId=c.Usr_Id "
                      + " LEFT JOIN CRM_LeadMast_Tbl l ON l.LeadId=t.TaskTypeId AND  t.TaskForType='L' "
                      + " LEFT JOIN CRM_POtentialMast_Tbl p ON p.PotentialId=t.TaskTypeId AND  t.TaskForType='P' "
                      + " LEFT JOIN CRM_Accmast_View a ON a.acc_id=t.TaskTypeId AND  t.TaskForType='A' "
                      + " WHERE convert(dat,t.CreatedDate) between '" + FromDate + "' and '" + ToDate + "' and "
                    + "  Usr_ActiveFlag='N' AND usr_type='User' AND (ReportingTo =" + UserId + "  OR "
                    + " ReportingTo IN (SELECT userid FROM dbo.GetUserChild(" + UserId + ")) or "
                    + " usr_id = " + UserId + ") AND Parent_Id NOT IN(5)  ORDER BY t.TaskOwnerId,t.TaskSubject ";

            }

            cn1.Open();
            DataTable dt = new DataTable();
            string strProc = string.Empty;
            try
            {
                //SqlParameter _UserId = new SqlParameter("@UsrId", SqlDbType.BigInt);
                //_UserId.Value = UserId;

                //SqlParameter _FinId = new SqlParameter("@FinId", SqlDbType.SmallInt);
                //_FinId.Value = FinId;
                //SqlParameter[] paramFields = { _UserId, _FinId };

                //strProc = "CRM_Get_CompanyRights_proc";
                //if (strProc != string.Empty)
                //{

                SqlDataReader sdr = SqlHelper.ExecuteReader(cn1, CommandType.Text, str);
                //SqlDataReader sdr = SqlHelper.ExecuteReader(cn, CommandType.StoredProcedure, strProc, paramFields);
                dt.Load(sdr);

                if (!sdr.IsClosed)
                    sdr.Close();
                //}
            }
            catch (Exception msg)
            {
                cn1.Close();
                //res = LogException(msg, "Hrms_GetCustWizardDetails() Procedure=" + strProc, null);
                if (res == null && res.ToString() == "")
                    res = msg.Message;
            }
            finally
            {
                if (cn1.State == ConnectionState.Open)
                    cn1.Close();
                if (cn1 != null) cn1.Dispose();
            }
            return dt;
        }
        internal DataTable getTaskDetailsData(string Mode, Int32 TaskOwnerId)
        {
            string str = string.Empty;
            if (Mode == "Mast")
            {
                str = " select t.TaskId,t.TaskSubject Subject,t.TaskDate,t.TaskOwnerId,t.DueDate,Priority,Status,t.Description,TaskForType,"
                        + " (CASE  TaskForType  WHEN 'L' THEN l.CompanyName WHEN 'A' THEN a.AccName WHEN 'P' THEN p.PotentialName ELSE '' END) RelatedTo, "
                        + " t.ContactPerson,dbo.getusername(t.CreatedBy) CreatedBy,t.CreatedDate,Convert(VARCHAR,t.createdDate,106) CreatDate,"
                        + "dbo.getusername(t.ModifiedBy) ModifiedBy,t.ModifiedDate,Convert(VARCHAR,t.modifiedDate,106) ModifyDate, "
                        + " dbo.getusername(t.TaskOwnerId) TaskOwnerId"
                        + " from CRM_TaskMast_Tbl t  "
                        + " LEFT JOIN CRM_LeadMast_Tbl l ON l.LeadId=t.TaskTypeId AND  t.TaskForType='L' "
                        + " LEFT JOIN CRM_POtentialMast_Tbl p ON p.PotentialId=t.TaskTypeId AND  t.TaskForType='P' "
                        + " LEFT JOIN CRM_Accmast_View a ON a.acc_id=t.TaskTypeId AND  t.TaskForType='A' where t.TaskOwnerId=" + TaskOwnerId + " ";

            }
            else if (Mode == "Auth")
            {
                str = " select t.TaskId,t.TaskSubject Subject,t.TaskOwnerId,t.TaskDate,t.DueDate,Priority,Status,t.Description,TaskForType,"
                        + " (CASE  TaskForType  WHEN 'L' THEN l.CompanyName WHEN 'A' THEN a.AccName WHEN 'P' THEN p.PotentialName ELSE '' END) RelatedTo, "
                        + " t.ContactPerson,dbo.getusername(t.CreatedBy) CreatedBy,t.CreatedDate,Convert(VARCHAR,t.createdDate,106) CreatDate,"
                        + "dbo.getusername(t.ModifiedBy) ModifiedBy,t.ModifiedDate,Convert(VARCHAR,t.modifiedDate,106) ModifyDate, "
                        + " dbo.getusername(t.TaskOwnerId) TaskOwnerId"
                        + " from CRM_TaskMastAuth_Tbl t  "
                        + " LEFT JOIN CRM_LeadMast_Tbl l ON l.LeadId=t.TaskTypeId AND  t.TaskForType='L' "
                        + " LEFT JOIN CRM_POtentialMast_Tbl p ON p.PotentialId=t.TaskTypeId AND  t.TaskForType='P' "
                        + " LEFT JOIN CRM_Accmast_View a ON a.acc_id=t.TaskTypeId AND  t.TaskForType='A' where t.TaskOwnerId=" + TaskOwnerId + " ";
            }
            cn1.Open();
            DataTable dt = new DataTable();
            string strProc = string.Empty;
            try
            {
                //SqlParameter _UserId = new SqlParameter("@UsrId", SqlDbType.BigInt);
                //_UserId.Value = UserId;

                //SqlParameter _FinId = new SqlParameter("@FinId", SqlDbType.SmallInt);
                //_FinId.Value = FinId;
                //SqlParameter[] paramFields = { _UserId, _FinId };

                //strProc = "CRM_Get_CompanyRights_proc";
                //if (strProc != string.Empty)
                //{

                SqlDataReader sdr = SqlHelper.ExecuteReader(cn1, CommandType.Text, str);
                //SqlDataReader sdr = SqlHelper.ExecuteReader(cn, CommandType.StoredProcedure, strProc, paramFields);
                dt.Load(sdr);

                if (!sdr.IsClosed)
                    sdr.Close();
                //}
            }
            catch (Exception msg)
            {
                cn1.Close();
                //res = LogException(msg, "Hrms_GetCustWizardDetails() Procedure=" + strProc, null);
                if (res == null && res.ToString() == "")
                    res = msg.Message;
            }
            finally
            {
                if (cn1.State == ConnectionState.Open)
                    cn1.Close();
                if (cn1 != null) cn1.Dispose();
            }
            return dt;
        }

        internal object Get_Employee_Id(Int64 UserId)
        {
            string str = "SELECT EmployeeId FROM RepDb_UserMaster_Tbl WHERE Usr_Id=" + UserId;

            cn.Open();
            object obj = new object();
            string strProc = string.Empty;
            try
            {
                obj = SqlHelper.ExecuteScalar(cn, CommandType.Text, str);
            }
            catch (Exception msg)
            {
                cn.Close();
                if (res == null && res.ToString() == "")
                    res = msg.Message;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close();
                if (cn != null) cn.Dispose();
            }
            return obj;

        }
    }
}
