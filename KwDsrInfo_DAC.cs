using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Controller_Crm;

namespace Controller_Crm
{
    public class KwDsrInfo_DAC : IDisposable
    {
        // Pointer to an external unmanaged resource.
        private IntPtr handle;
        // Track whether Dispose has been called.
        private bool disposed = false;

        //EntityConnection cn1;
        SqlConnection cn, cn1;
        string res;
        int n;
        public KwDsrInfo_DAC()
        {
            cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Crm_CommonDbStr"].ConnectionString);
            //cn = new SqlConnection("Data source=192.168.0.90;Database=db_repository;user id=sa;password=pwd;Timeout=360;Connection reset=false");
            cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["Crm_ConnStr"].ConnectionString);
        }
        public enum MsgType
        {
            AlertMessage, Information, Error
        };
        public KwDsrInfo_DAC(IntPtr handle)
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

        ~KwDsrInfo_DAC()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            Dispose(false);
        }

        public DataTable CRM_Get_Trexp_Data(TransUser_Ety tu_ety)
        {
            string strQuey = "SELECT  tr.trvexpid,empid,Convert(varchar,tourfrmdate,106)tourfrmdate, " +
                            " Convert(varchar,tourtodate,106) tourtodate,dbo.PROPERCASE(firstname+' '+ middlename+' ' + lastname ) tourauthorizedby, " +
                            " Convert(NUMERIC(18,2),Advancedrawn)Advancedrawn,substring(Convert(VARCHAR,deptime,100),12,8) deptime, " +
                            " substring(Convert(VARCHAR,arivaltime,100),12,8)arivaltime,repdatetime,Convert(NUMERIC(18,2),Amtclaimed) Amtclaimed,amtdisallowed, " +
                            " Convert(NUMERIC(18,2),amtpassed) amtpassed,approvedby " +
                            " FROM dbo.DSR_TravellingexpenseMast_tbl tr  " +
                            " INNER JOIN HRMS_Employee_Master h ON tr.tourauthorizedby =h.Employeeid " +
                            " WHERE  tr.tourfrmdate BETWEEN '" + tu_ety.FromDt + "' and '" + tu_ety.ToDt + "'  and tr.empid=" + tu_ety.UserId + "  order by trvexpid";
            using (Trans_Main_DAC dac = new Trans_Main_DAC())
            {
                return dac.Get_StrQuery_Data(strQuey);
            }

        }
        public DataTable CRM_Get_Area(string type)
        {
            string strQuey = "SELECT AreaId,dbo.PROPERCASE(AreaDescription)AreaDescription " +
                " FROM DSR_AreaMast_Tbl where AreaType='" + type + "' order by AreaDescription";

            using (Trans_Main_DAC dac = new Trans_Main_DAC())
            {
                return dac.Get_StrQuery_Data(strQuey);
            }
        }
        public DataTable CRM_Get_Depts(string type)
        {
            string strQuey = string.Empty;
            if (type == null)
                strQuey = "select Dept_id,dbo.propercase(Dept_desc) Dept_desc from dept_details Where Dflag='N' order by Dept_Desc";
            else
            {
                strQuey = "SELECT  d.Dept_id,dbo.propercase(Dept_desc) Dept_desc  FROM dept_details d "
                + " INNER JOIN (select DISTINCT departmentid from hrms_employee_master m  "
                + " INNER JOIN hrms_DsrEntryEmployees_tbl dsr on m.Employeeid =dsr.empid where  statusid<>1 ) em"
                + " ON em.departmentid=d.Dept_Id  Where d.Dflag='N'  order by Dept_Desc";
            }
            using (Trans_Main_DAC dac = new Trans_Main_DAC())
            {
                return dac.Get_StrQuery_Data(strQuey);
            }
        }

        public DataTable CRM_Get_DsrUserRights(Int64 UserId, string DsrMenuName)
        {
            string strQuey = "SELECT AddBtn,ModifyBtn,PrintBtn,EraseBtn,ViewBtn,MenuPermission,"
                        + " MenuName,updateCTC,Type FROM hrms_Security_tbl "
                        + " WHERE userid=" + UserId + " AND MenuName= '" + DsrMenuName + "'";

            using (Trans_Main_DAC dac = new Trans_Main_DAC())
            {
                return dac.Get_StrQuery_Data(strQuey);
            }

        }
        public DataTable CRM_Get_DeptWise_Employees(Int16 DeptId, string type)
        {
            string strQuey = string.Empty;
            if (type == null)
            {
                strQuey = "select Employeeid,EmployeeCode,[EmployeeName] EmpName " +
                   " from hrms_EmployeeMast_View  where deptid=" + DeptId + " and statusid<>1 order by EmployeeCode";
            }
            else
            {
                strQuey = "select Employeeid,EmployeeCode,[EmployeeName] EmpName " +
                    " from hrms_EmployeeMast_View m inner join hrms_DsrEntryEmployees_tbl h on m.employeeid=h.empid " +
                    " where deptid=" + DeptId + " and statusid<>1 order by EmployeeName";
            }
            using (Trans_Main_DAC dac = new Trans_Main_DAC())
            {
                return dac.Get_StrQuery_Data(strQuey);
            }
        }

        public DataTable CRM_Get_KW_Dsr_Main_Details(TransUser_Ety tu_ety)
        {

            string strQuey = string.Empty;
            if (tu_ety.TransMode == "Mast")
            {
                strQuey = "select DSRId,replace(Convert(varchar,DSRDate,106),' ','-')DSRDate,CASE DSRoption WHEN 1 THEN 'Direct Market' WHEN 2"
             + " THEN 'Office and Market' ELSE 'Meeting' END DSRoption,City_Town AreasVisited,"//dbo.getDsrArea(DSRId)AreasVisited,"
             + " Convert(NUMERIC(18,2),POAmount) PoAmount,Convert(NUMERIC(18,2),ChequeAmount)ChequeAmount,"
             + " Convert(NUMERIC(18,2),Cashcollected)Cashcollected,Remarks,substring(Remarks,1,20)Remarks1,"
             + " isnull(a.AccCode +':'+a.AccName,'')  Account,isnull(a.acc_id,0) acc_id From DSR_DailySalesRpt_Tbl d"
             + " LEFT JOIN GWTME_Sc_Accmast_View a ON d.AccId=a.acc_id  "
             + " where DSRDate between '" + tu_ety.FromDt + "' and '" + tu_ety.ToDt + "' and  d.empid=" + tu_ety.UserId + " order by d.DSRDate";
            }
            else if (tu_ety.TransMode == "Auth")
            {
                strQuey = "select DSRId,replace(Convert(varchar,DSRDate,106),' ','-')DSRDate,CASE DSRoption WHEN 1 THEN 'Direct Market' WHEN 2"
             + " THEN 'Office and Market' ELSE 'Meeting' END DSRoption,City_Town AreasVisited,"//dbo.getDsrArea(DSRId)AreasVisited,"
             + " Convert(NUMERIC(18,2),POAmount) PoAmount,Convert(NUMERIC(18,2),ChequeAmount)ChequeAmount,"
             + " Convert(NUMERIC(18,2),Cashcollected)Cashcollected,Remarks,substring(Remarks,1,20)Remarks1,"
             + " isnull(a.AccCode +':'+a.AccName,'')  Account,isnull(a.acc_id,0) acc_id From DSR_DailySalesRptAuth_Tbl d"
             + " LEFT JOIN GWTME_Sc_Accmast_View a ON d.AccId=a.acc_id  "
             + " where DSRDate between '" + tu_ety.FromDt + "' and '" + tu_ety.ToDt + "' and  d.empid=" + tu_ety.UserId + " order by d.DSRDate";
            }

            using (Trans_Main_DAC dac = new Trans_Main_DAC())
            {
                return dac.Get_StrQuery_Data(strQuey);
            }
        }

        public DataTable CRM_Get_KW_Dsr_Details(TransUser_Ety tu_ety)//   Int64 vDsrId, string rbtnDsrAuth)
        {
            string strQuey = string.Empty;
            if (tu_ety.TransMode == "Mast")
            {
                strQuey = "SELECT DSRId,DSRDate,d.Empid,DSRoption,City_Town AreasVisited,Convert(NUMERIC(18,2),POAmount)POAmount,"
               + " Convert(NUMERIC(18,2),ChequeAmount) ChequeAmount,Convert(NUMERIC(18,2),Cashcollected)Cashcollected, "
               + " Remarks ,isnull(a.AccCode,'') AccCode,isnull(a.AccName,'') AccName,isnull(a.acc_id,0) AccId " +
               " FROM dbo.DSR_DailySalesRpt_Tbl d  LEFT JOIN GWTME_Sc_Accmast_View a ON d.AccId=a.acc_id WHERE DSRId=" + tu_ety.DocId;
            }
            else if (tu_ety.TransMode == "Auth")
            {
                strQuey = "SELECT DSRId,DSRDate,d.Empid,DSRoption,City_Town AreasVisited,Convert(NUMERIC(18,2),POAmount)POAmount,"
                + " Convert(NUMERIC(18,2),ChequeAmount) ChequeAmount,Convert(NUMERIC(18,2),Cashcollected)Cashcollected, "
                + " Remarks ,isnull(a.AccCode,'') AccCode,isnull(a.AccName,'') AccName,isnull(a.acc_id,0) AccId " +
                " FROM dbo.DSR_DailySalesRptAuth_Tbl d  LEFT JOIN GWTME_Sc_Accmast_View a ON d.AccId=a.acc_id WHERE DSRId=" + +tu_ety.DocId;
            }
            using (Trans_Main_DAC dac = new Trans_Main_DAC())
            {
                return dac.Get_StrQuery_Data(strQuey);
            }
        }

        public DataTable CRM_Get_Employee_Details(Int64 EmployeeId)
        {
            string strQuey = "select deptid departmentid, dbo.propercase(d.Dept_Desc) Dept_Desc,employeecode,EmployeeName Name " +
            " from hrms_EmployeeMast_View e,dept_details d where employeeid = " + EmployeeId + " and d.Dept_Id=e.deptid order by employeecode";
            using (Trans_Main_DAC dac = new Trans_Main_DAC())
            {
                return dac.Get_StrQuery_Data(strQuey);
            }
        }

        public DataTable CRM_Get_Employee_Designation_Details(Int64 EmployeeId)
        {
            string strQuey = "SELECT e.Employeeid ,dbo.PROPERCASE(e.Desig_Desc) Designation FROM hrms_EmployeeMast_View e"
                + " WHERE e.Employeeid =" + EmployeeId;
            using (Trans_Main_DAC dac = new Trans_Main_DAC())
            {
                return dac.Get_StrQuery_Data(strQuey);
            }
        }

        public DataTable CRM_Get_Employee_Reporting_Details(Int64 EmployeeId)
        {
            string strQuey = "SELECT Employeeid,dbo.PROPERCASE(EmpName)EmpName FROM dbo.getallReportings(" + EmployeeId + ") WHERE employeeid NOT IN (" + EmployeeId + ")";
            using (Trans_Main_DAC dac = new Trans_Main_DAC())
            {
                return dac.Get_StrQuery_Data(strQuey);
            }
        }

        public DataTable CRM_Get_KW_TrExp_Header_Details(Int64 TravId)
        {
            string strQuey = "SELECT trvexpid,empid,dbo.PROPERCASE(Desig_Desc) Desig_Desc,Area,Replace(Convert(VARCHAR,tourfrmdate,106),' ','-')tourfrmdate,"
                + " Replace(Convert(VARCHAR,tourtodate,106),' ','-')tourtodate,tourauthorizedby,	Convert(NUMERIC(18,2),Advancedrawn)Advancedrawn, "
               + "substring(Convert(VARCHAR,deptime,100),13,8) deptime,substring(Convert(VARCHAR,arivaltime,100),13,8)arivaltime,Convert(VARCHAR,repdatetime,106) repdatetime,"
            + " Convert(NUMERIC(18,2),Amtclaimed) Amtclaimed,Convert(NUMERIC(18,2),amtdisallowed) amtdisallowed,Convert(NUMERIC(18,2),amtpassed) amtpassed,approvedby,departmentid "
            + " FROM dbo.DSR_TravellingexpenseMast_tbl Tr INNER JOIN HRms_Employee_master m  on tr.empid= m.employeeid "
            + " INNER JOIN Designations d on d.Desig_id=m.designationid WHERE trvexpid =" + TravId;
            using (Trans_Main_DAC dac = new Trans_Main_DAC())
            {
                return dac.Get_StrQuery_Data(strQuey);
            }
        }
        public DataTable CRM_Get_KW_TrExp_Details(Int64 vTrExpid, string rbtnDsrAuth)
        {
            DataTable dt = new DataTable();
            cn1.Open();
            string strProc = string.Empty;
            try
            {
                SqlParameter Flag = new SqlParameter("@rbtnDsrAuth", SqlDbType.VarChar);
                Flag.Value = rbtnDsrAuth;

                SqlParameter TrExpid = new SqlParameter("@TravId", SqlDbType.BigInt);
                TrExpid.Value = vTrExpid;

                SqlParameter[] paramFields = { Flag, TrExpid };

                strProc = "Hrms_TravelingDetailsData_PROC";
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
                if (res == null)
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

        public DataTable CRM_Get_KW_TrExp_Dtl_Details(Int64 TravId)
        {
            string strQuey = "SELECT  Replace(Convert(VARCHAR,trvdate,106),' ','-') TrvDate,Modeoftravell,Particulars TrFrom,ParticularsTo TrTo,Convert(NUMERIC(18,2),Fare)Fare,"
                + " Convert(NUMERIC(18,2),Lodging) LodgingBoard,Convert(NUMERIC(18,2),POrderValue) OrderVal,"
                + " dbo.getAreaName(particulars) particulars,dbo.getAreaName(particularsTo) particularsTo, "
                + " Convert(NUMERIC(18,2),CollectionAmt) ColAmt, Convert(NUMERIC(18,2),Miscexp) MicExpenses,Remarks, "
                + " Convert(NUMERIC(18,2),(Fare+Lodging+ Miscexp )) Total,Supportingflag,TSLNO FROM dbo.DSR_Travellingexpdetails_tbl WHERE travellid =" + TravId;

            using (Trans_Main_DAC dac = new Trans_Main_DAC())
            {
                return dac.Get_StrQuery_Data(strQuey);
            }
        }
        public DataTable CRM_Get_KW_TrExp_Pending_dtls(Int64 TravId)
        {
            string strQuey = "SELECT  Replace(Convert(VARCHAR,trvdate,106),' ','-') TrvDate,TSLNO,Modeoftravell,Particulars,Convert(NUMERIC(18,2),Fare)Fare,"
                + " Convert(NUMERIC(18,2),Lodging) LodgingBoard,Convert(NUMERIC(18,2),Locconvy) LocalConveyance,"
                + " Convert(NUMERIC(18,2),Extconvy)ExtraConveyance,Convert(NUMERIC(18,2),Luggagechrg) LuggageCharge,"
                + " Convert(NUMERIC(18,2),Miscexp) MicExpenses,Remarks, "
                + " Convert(NUMERIC(18,2),(Fare+Lodging+ Locconvy+	Extconvy+Luggagechrg+Miscexp )) Total,Supportingflag "
                + " FROM dbo.DSR_Travellingexpdetails_tbl WHERE travellid =" + TravId + " and authflag='N'";

            using (Trans_Main_DAC dac = new Trans_Main_DAC())
            {
                return dac.Get_StrQuery_Data(strQuey);
            }
        }

        public DataTable CRM_Get_KW_TrExp_Main_Details(TransUser_Ety tu_ety)
        {
            string strQuey = "SELECT tr.trvexpid,empid,a.AreaDescription,Convert(varchar,tourfrmdate,106)tourfrmdate,"
                + "Convert(varchar,tourtodate,106) tourtodate,dbo.PROPERCASE(firstname+' '+ middlename+' ' + lastname ) tourauthorizedby,"
                + " Convert(NUMERIC(18,2),Advancedrawn)Advancedrawn,substring(Convert(VARCHAR,deptime,100),12,8) deptime,"
                + " substring(Convert(VARCHAR,arivaltime,100),12,8)arivaltime,repdatetime,Convert(NUMERIC(18,2),Amtclaimed) Amtclaimed,amtdisallowed,"
                + " Convert(NUMERIC(18,2),amtpassed) amtpassed,approvedby FROM dbo.DSR_TravellingexpenseMast_tbl tr "
                + " INNER JOIN HRMS_Employee_Master h ON tr.tourauthorizedby =h.Employeeid "
                + "INNER JOIN  DSR_Travellingexpdetails_tbl d ON d.travellid =tr.trvexpid"
                + " INNER JOIN DSR_AreaMast_Tbl a ON a.AreaId =d.particulars "
                + " INNER JOIN ( SELECT  DISTINCT trvexpid FROM DSR_Travellingexpdetails_tbl td INNER JOIN "
                + " DSR_TravellingexpenseMast_tbl tr1  ON td.travellid=tr1.trvexpid  WHERE "
                + " tr1.empid=" + tu_ety.UserId + " AND tr1.tourfrmdate BETWEEN '" + tu_ety.FromDt + "' and '" + tu_ety.ToDt + "' /*AND td.authflag ='N'*/) b"
                + " ON b.trvexpid=tr.trvexpid WHERE  tr.tourfrmdate BETWEEN '" + tu_ety.FromDt + "' and '" + tu_ety.ToDt + "' "
                + " and tr.empid=" + tu_ety.UserId + " order by trvexpid";
            using (Trans_Main_DAC dac = new Trans_Main_DAC())
            {
                return dac.Get_StrQuery_Data(strQuey);
            }
        }

        public DataTable CRM_Get_KW_TADA_Summary(TransUser_Ety tu_ety)
        {
            DataTable dt = new DataTable();
            cn1.Open();
            string strProc = string.Empty;
            try
            {
                SqlParameter[] ParamDSRSummary = new SqlParameter[2];
                ParamDSRSummary[0] = new SqlParameter("@frmdate", SqlDbType.SmallDateTime);
                ParamDSRSummary[1] = new SqlParameter("@todate", SqlDbType.SmallDateTime);

                ParamDSRSummary[0].Value = tu_ety.FromDt;
                ParamDSRSummary[1].Value = tu_ety.ToDt;

                strProc = "DSR_Conveance_Proc";
                if (strProc != string.Empty)
                {
                    SqlDataReader sdr = SqlHelper.ExecuteReader(cn1, CommandType.StoredProcedure, strProc, ParamDSRSummary);
                    dt.Load(sdr);

                    if (!sdr.IsClosed)
                        sdr.Close();
                }
            }
            catch (Exception msg)
            {
                cn1.Close();
                if (res == null)
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

        public DataTable CRM_Get_KW_DSR_Summary(TransUser_Ety tu_ety)
        {

            DataTable dt = new DataTable();
            cn1.Open();
            string strProc = string.Empty;
            try
            {
                SqlParameter[] ParamDSRSummary = new SqlParameter[2];
                ParamDSRSummary[0] = new SqlParameter("@fromDate", SqlDbType.SmallDateTime);
                ParamDSRSummary[1] = new SqlParameter("@toDate", SqlDbType.SmallDateTime);

                ParamDSRSummary[0].Value = tu_ety.FromDt;
                ParamDSRSummary[1].Value = tu_ety.ToDt;

                strProc = "Dsr_DsrSummary_Proc";
                if (strProc != string.Empty)
                {
                    SqlDataReader sdr = SqlHelper.ExecuteReader(cn1, CommandType.StoredProcedure, strProc, ParamDSRSummary);
                    dt.Load(sdr);

                    if (!sdr.IsClosed)
                        sdr.Close();
                }
            }
            catch (Exception msg)
            {
                cn1.Close();
                if (res == null)
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

        public CRM_ConExecStatus_Info_Ety CRM_Save_KW_DSR_Details(Hashtable ht, string ProcType)
        {
            CRM_ConExecStatus_Info_Ety ocrm_ConExecStatus_Info_ety = new CRM_ConExecStatus_Info_Ety();
            cn1.Open();
            string strProc = string.Empty;
            try
            {

                SqlParameter[] dsrparam = new SqlParameter[14];
                dsrparam[0] = new SqlParameter("@Indsrid", SqlDbType.BigInt);
                dsrparam[1] = new SqlParameter("@Indsrdate", SqlDbType.SmallDateTime);
                dsrparam[2] = new SqlParameter("@Inempid", SqlDbType.SmallInt);
                dsrparam[3] = new SqlParameter("@Indsroption", SqlDbType.SmallInt);
                dsrparam[4] = new SqlParameter("@Inareasvisited", SqlDbType.VarChar, 4000);
                dsrparam[5] = new SqlParameter("@Inpoamount", SqlDbType.Decimal);
                dsrparam[6] = new SqlParameter("@Inchequeamount", SqlDbType.Decimal);
                dsrparam[7] = new SqlParameter("@Incashcollected", SqlDbType.Decimal);
                dsrparam[8] = new SqlParameter("@Inremarks", SqlDbType.VarChar, 4000);
                dsrparam[9] = new SqlParameter("@InAccId", SqlDbType.BigInt);
                dsrparam[10] = new SqlParameter("@Inusrid", SqlDbType.SmallInt);
                dsrparam[11] = new SqlParameter("@Inmode", SqlDbType.VarChar, 50);
                dsrparam[12] = new SqlParameter("@execstatus", SqlDbType.SmallInt);
                dsrparam[13] = new SqlParameter("@result", SqlDbType.VarChar, 500);
                dsrparam[12].Direction = ParameterDirection.Output;
                dsrparam[13].Direction = ParameterDirection.Output;

                dsrparam[0].Value = Convert.ToInt64(ht["DsrId"]);
                dsrparam[1].Value = Convert.ToDateTime(ht["DsrDare"]);
                dsrparam[2].Value = Convert.ToInt16(ht["EmpId"]);
                dsrparam[3].Value = Convert.ToInt16(ht["DsrMode"]);
                dsrparam[4].Value = ht["DsrArea"];
                dsrparam[5].Value = Convert.ToDecimal(ht["PoAmt"]);
                dsrparam[6].Value = Convert.ToDecimal(ht["ChqAmt"]);
                dsrparam[7].Value = Convert.ToDecimal(ht["CashAmt"]);
                dsrparam[8].Value = ht["Remarks"];
                dsrparam[9].Value = Convert.ToInt64(ht["AccId"]);
                dsrparam[10].Value = Convert.ToInt16(ht["UserId"]);
                dsrparam[11].Value = ht["Mode"];
                dsrparam[12].Value = 0;
                dsrparam[13].Value = "hh";

                strProc = "DSR_dailySalesRpt_proc";

                if (ProcType == "Mast")
                    strProc = "DSR_dailySalesRpt_proc";
                else if (ProcType == "MastAuth")
                    strProc = "DSR_dailySalesRptMastAuth_proc";
                else if (ProcType == "Auth")
                    strProc = "DSR_dailySalesRptAuth_proc";

                if (strProc != string.Empty)
                {
                    SqlHelper.ExecuteNonQuery(cn1, CommandType.StoredProcedure, strProc, dsrparam);
                }

                ocrm_ConExecStatus_Info_ety.ExecStatus = Convert.ToByte(dsrparam[12].Value.ToString());
                ocrm_ConExecStatus_Info_ety.Confirm = dsrparam[13].Value.ToString();

                if (cn1 != null && cn1.State == ConnectionState.Open) cn1.Close();
            }
            catch (Exception msg)
            {
                if (cn1 != null) cn1.Close();
                ocrm_ConExecStatus_Info_ety.Confirm = msg.Message.Replace("'", "");
                ocrm_ConExecStatus_Info_ety.ExecStatus = 0;
            }
            finally
            {
                if (cn1.State == ConnectionState.Open) cn1.Close();
                if (cn1 != null) cn1.Dispose();
            }

            return ocrm_ConExecStatus_Info_ety;
        }

        public CRM_ConExecStatus_Info_Ety CRM_Authorize_KW_DSR(Hashtable ht)
        {
            CRM_ConExecStatus_Info_Ety ocrm_ConExecStatus_Info_ety = new CRM_ConExecStatus_Info_Ety();
            cn1.Open();
            string strProc = string.Empty;
            try
            {
                SqlParameter[] dsrparam = new SqlParameter[7];
                dsrparam[0] = new SqlParameter("@Inempid", SqlDbType.SmallInt);
                dsrparam[1] = new SqlParameter("@Indsrid", SqlDbType.VarChar);
                dsrparam[2] = new SqlParameter("@InAuth", SqlDbType.VarChar);
                dsrparam[3] = new SqlParameter("@InDsrAuth", SqlDbType.VarChar);
                dsrparam[4] = new SqlParameter("@UsrId", SqlDbType.SmallInt);
                dsrparam[5] = new SqlParameter("@Result", SqlDbType.VarChar, 200);
                dsrparam[5].Direction = ParameterDirection.InputOutput;
                dsrparam[6] = new SqlParameter("@ExecStatus", SqlDbType.TinyInt);
                dsrparam[6].Direction = ParameterDirection.InputOutput;


                dsrparam[0].Value = Convert.ToInt32(ht["EmpId"]);
                dsrparam[1].Value = ht["DsrId"];
                dsrparam[2].Value = ht["DsrAuth"];
                dsrparam[3].Value = ht["DsrSlnoAuth"];
                dsrparam[4].Value = Convert.ToInt16(ht["UserId"]);
                dsrparam[5].Value = "";
                dsrparam[6].Value = 0;
                //int i = SqlHelper.ExecuteNonQuery(setCon(), CommandType.StoredProcedure, "dsr_multiplieAuthorization_Proc", dsrparam);

                strProc = "dsr_multiplieAuthorization_Proc";
                if (strProc != string.Empty)
                {
                    SqlHelper.ExecuteNonQuery(cn1, CommandType.StoredProcedure, strProc, dsrparam);
                }

                ocrm_ConExecStatus_Info_ety.ExecStatus = Convert.ToByte(dsrparam[6].Value.ToString());
                ocrm_ConExecStatus_Info_ety.Confirm = dsrparam[6].Value.ToString();

                if (cn1 != null && cn1.State == ConnectionState.Open) cn1.Close();
            }
            catch (Exception msg)
            {
                if (cn1 != null) cn1.Close();
                ocrm_ConExecStatus_Info_ety.Confirm = msg.Message.Replace("'", "");
                ocrm_ConExecStatus_Info_ety.ExecStatus = 0;
            }
            finally
            {
                if (cn1.State == ConnectionState.Open) cn1.Close();
                if (cn1 != null) cn1.Dispose();
            }
            return ocrm_ConExecStatus_Info_ety;
        }

        public CRM_ConExecStatus_Info_Ety CRM_Save_KW_TravelExpenses(Hashtable ht, Hashtable dtParam)
        {
            CRM_ConExecStatus_Info_Ety ocrm_ConExecStatus_Info_ety = new CRM_ConExecStatus_Info_Ety();
            //SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            cn1.Open();
            SqlTransaction st = cn1.BeginTransaction();
            string strProc = string.Empty;
            try
            {
                SqlParameter[] dsrparam = new SqlParameter[18];
                dsrparam[0] = new SqlParameter("@Intrvexpid", SqlDbType.BigInt);
                dsrparam[0].Direction = ParameterDirection.InputOutput;
                dsrparam[1] = new SqlParameter("@Inempid", SqlDbType.SmallInt);
                dsrparam[2] = new SqlParameter("@Inarea", SqlDbType.SmallInt);
                dsrparam[3] = new SqlParameter("@Intourfrmdate", SqlDbType.SmallDateTime);
                dsrparam[4] = new SqlParameter("@Intourtodate", SqlDbType.SmallDateTime);
                dsrparam[5] = new SqlParameter("@Intourauthorizedby", SqlDbType.SmallInt);
                dsrparam[6] = new SqlParameter("@Inadvancedrawn", SqlDbType.Decimal);
                dsrparam[7] = new SqlParameter("@Indeptime", SqlDbType.VarChar);
                dsrparam[8] = new SqlParameter("@Inarivaltime", SqlDbType.VarChar);
                dsrparam[9] = new SqlParameter("@Inrepdatetime", SqlDbType.SmallDateTime);
                dsrparam[10] = new SqlParameter("@Inamtclaimed", SqlDbType.Decimal);
                dsrparam[11] = new SqlParameter("@Inamtdisallowed", SqlDbType.Decimal);
                dsrparam[12] = new SqlParameter("@Inamtpassed", SqlDbType.Decimal);
                dsrparam[13] = new SqlParameter("@Inapprovedby", SqlDbType.SmallInt);
                dsrparam[14] = new SqlParameter("@Inusrid", SqlDbType.SmallInt);
                dsrparam[15] = new SqlParameter("@Inmode", SqlDbType.VarChar, 50);
                dsrparam[16] = new SqlParameter("@execstatus", SqlDbType.SmallInt);
                dsrparam[16].Direction = ParameterDirection.Output;
                dsrparam[17] = new SqlParameter("@result", SqlDbType.VarChar, 50);
                dsrparam[17].Direction = ParameterDirection.Output;

                dsrparam[0].Value = ht["trvexpid"];
                dsrparam[1].Value = Convert.ToInt16(ht["empid"]);
                dsrparam[2].Value = Convert.ToInt16(ht["area"]);
                dsrparam[3].Value = Convert.ToDateTime(ht["tourfrmdate"]);
                dsrparam[4].Value = Convert.ToDateTime(ht["tourtodate"]);
                dsrparam[5].Value = Convert.ToInt16(ht["tourauthorizedby"]);
                dsrparam[6].Value = Convert.ToDecimal(ht["advancedrawn"]);
                dsrparam[7].Value = ht["deptime"];
                dsrparam[8].Value = ht["arivaltime"];
                dsrparam[9].Value = Convert.ToDateTime(ht["repdatetime"]);
                dsrparam[10].Value = Convert.ToDecimal(ht["amtclaimed"]);
                dsrparam[11].Value = Convert.ToDecimal(ht["amtdisallowed"]);
                dsrparam[12].Value = Convert.ToDecimal(ht["amtpassed"]);
                dsrparam[13].Value = ht["approvedby"];
                dsrparam[14].Value = Convert.ToInt16(ht["UserId"]);
                dsrparam[15].Value = ht["Mode"];
                dsrparam[16].Value = 0;
                dsrparam[17].Value = "hh";

                strProc = "DSR_TravellingexpenseMast_proc";

                if (strProc != string.Empty)
                {
                    SqlHelper.ExecuteNonQuery(st, CommandType.StoredProcedure, strProc, dsrparam);
                }

                if (Convert.ToInt16(dsrparam[16].Value.ToString()) == 0)
                {
                    st.Rollback();
                    cn1.Close();
                    ocrm_ConExecStatus_Info_ety.Confirm = dsrparam[17].Value.ToString();
                    ocrm_ConExecStatus_Info_ety.ExecStatus = Convert.ToByte(dsrparam[16].Value.ToString());
                    return ocrm_ConExecStatus_Info_ety;
                }
                ocrm_ConExecStatus_Info_ety.ExecStatus = Convert.ToByte(dsrparam[16].Value.ToString());
                ocrm_ConExecStatus_Info_ety.Confirm = dsrparam[17].Value.ToString();

                if (ht["Mode"].ToString() != "DELETE")
                {
                    SqlParameter[] dsrDtparam = new SqlParameter[17];
                    dsrDtparam[0] = new SqlParameter("@Intravellid", SqlDbType.BigInt);
                    dsrDtparam[1] = new SqlParameter("@Intrvdate", SqlDbType.VarChar);
                    dsrDtparam[2] = new SqlParameter("@Inparticulars", SqlDbType.VarChar);
                    dsrDtparam[3] = new SqlParameter("@Inmodeoftravell", SqlDbType.VarChar);
                    dsrDtparam[4] = new SqlParameter("@Infare", SqlDbType.VarChar);
                    dsrDtparam[5] = new SqlParameter("@Inlodging", SqlDbType.VarChar);
                    dsrDtparam[6] = new SqlParameter("@Inlocconvy", SqlDbType.VarChar);
                    dsrDtparam[6] = new SqlParameter("@InPOrderVal", SqlDbType.VarChar);

                    dsrDtparam[7] = new SqlParameter("@InCollectAmt", SqlDbType.VarChar);
                    dsrDtparam[8] = new SqlParameter("@InparticularsTo", SqlDbType.VarChar);
                    dsrDtparam[9] = new SqlParameter("@Inmiscexp", SqlDbType.VarChar);
                    dsrDtparam[10] = new SqlParameter("@Inremarks", SqlDbType.VarChar);
                    dsrDtparam[11] = new SqlParameter("@Inusrid", SqlDbType.SmallInt);
                    dsrDtparam[12] = new SqlParameter("@Insupportingflag", SqlDbType.VarChar);
                    dsrDtparam[13] = new SqlParameter("@Inauthflag", SqlDbType.VarChar);

                    dsrDtparam[14] = new SqlParameter("@Inmode", SqlDbType.VarChar, 50);
                    dsrDtparam[15] = new SqlParameter("@execstatus", SqlDbType.SmallInt);
                    dsrDtparam[15].Direction = ParameterDirection.Output;
                    dsrDtparam[16] = new SqlParameter("@result", SqlDbType.VarChar, 50);
                    dsrDtparam[16].Direction = ParameterDirection.Output;

                    dsrDtparam[0].Value = Convert.ToInt64(dsrparam[0].Value);
                    dsrDtparam[1].Value = dtParam["Trvdate"];
                    dsrDtparam[2].Value = dtParam["particulars"];
                    dsrDtparam[3].Value = dtParam["modeoftravell"]; // Convert.ToInt16(ht["tourfrmdate"]);
                    dsrDtparam[4].Value = dtParam["fare"];
                    dsrDtparam[5].Value = dtParam["lodging"];

                    dsrDtparam[6].Value = dtParam["locconvy"];
                    dsrDtparam[7].Value = dtParam["extconvy"];
                    dsrDtparam[8].Value = dtParam["luggagechrg"];
                    dsrDtparam[6].Value = dtParam["OrderVal"];
                    dsrDtparam[7].Value = dtParam["ColAmt"];
                    dsrDtparam[8].Value = dtParam["particularsTo"];
                    dsrDtparam[9].Value = dtParam["miscexp"];
                    dsrDtparam[10].Value = dtParam["remarks"];
                    dsrDtparam[11].Value = Convert.ToInt16(ht["UserId"]);
                    dsrDtparam[12].Value = dtParam["supportingflag"];
                    dsrDtparam[13].Value = dtParam["AuthFlg"];
                    dsrDtparam[14].Value = dtParam["Mode"];
                    dsrDtparam[15].Value = 0;
                    dsrDtparam[16].Value = "hh";

                    strProc = "DSR_Travellingexpdetails_proc";

                    if (strProc != string.Empty)
                    {
                        SqlHelper.ExecuteNonQuery(st, CommandType.StoredProcedure, strProc, dsrDtparam);
                    }
                    if (Convert.ToInt16(dsrDtparam[15].Value.ToString()) == 0)
                    {
                        st.Rollback();
                        cn1.Close();
                        ocrm_ConExecStatus_Info_ety.Confirm = dsrDtparam[16].Value.ToString();
                        ocrm_ConExecStatus_Info_ety.ExecStatus = Convert.ToByte(dsrDtparam[15].Value.ToString());
                        return ocrm_ConExecStatus_Info_ety;
                    }
                    ocrm_ConExecStatus_Info_ety.ExecStatus = Convert.ToByte(dsrDtparam[15].Value.ToString());
                    ocrm_ConExecStatus_Info_ety.Confirm = dsrDtparam[16].Value.ToString();
                }
                st.Commit();
                if (cn1 != null && cn1.State == ConnectionState.Open) cn1.Close();
            }
            catch (Exception msg)
            {
                st.Rollback();
                if (cn1 != null) cn1.Close();
                if (res == null)
                    res = msg.Message;
                ocrm_ConExecStatus_Info_ety.Confirm = msg.Message;
                ocrm_ConExecStatus_Info_ety.ExecStatus = 0;
            }
            finally
            {
                if (cn1.State == ConnectionState.Open) cn1.Close();
                if (cn1 != null) cn1.Dispose();
            }

            return ocrm_ConExecStatus_Info_ety;
        }

        //public CRM_ConExecStatus_Info_Ety AuthrizeTrExpDetails(TransUser_Ety tu_ety ,string TrExpId, string TrExpAuth, Int16 Userid)
        //{
        //    SqlParameter[] TrExpAuthparam = new SqlParameter[5];
        //    TrExpAuthparam[0] = new SqlParameter("@InTrvId", SqlDbType.VarChar);
        //    TrExpAuthparam[1] = new SqlParameter("@Inauthflag", SqlDbType.VarChar);
        //    TrExpAuthparam[2] = new SqlParameter("@authby", SqlDbType.SmallInt);
        //    TrExpAuthparam[3] = new SqlParameter("@Execstatus", SqlDbType.SmallInt);
        //    TrExpAuthparam[4] = new SqlParameter("@Result", SqlDbType.VarChar, 250);
        //    TrExpAuthparam[3].Direction = ParameterDirection.Output;
        //    TrExpAuthparam[4].Direction = ParameterDirection.Output;

        //    TrExpAuthparam[0].Value = TrExpId;
        //    TrExpAuthparam[1].Value = TrExpAuth;
        //    TrExpAuthparam[2].Value = Userid;
        //    TrExpAuthparam[3].Value = 0;
        //    TrExpAuthparam[4].Value = "hh";

        //    SqlHelper.ExecuteNonQuery(setCon(), CommandType.StoredProcedure, "DSR_authorizetravellexpenses", TrExpAuthparam);

        //    return TrExpAuthparam[4].Value.ToString();
        //}
    }
}
