using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Controller_Crm;
namespace Controller_Crm
{
    internal class DsrInfo_DAC : IDisposable
    {
        // Pointer to an external unmanaged resource.
        private IntPtr handle;
        // Track whether Dispose has been called.
        private bool disposed = false;

        //EntityConnection cn1;
        SqlConnection cn, cn1;
        string res;
        int n;
        internal DsrInfo_DAC()
        {
            cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Crm_CommonDbStr"].ConnectionString);
            //cn = new SqlConnection("Data source=192.168.0.90;Database=db_repository;user id=sa;password=pwd;Timeout=360;Connection reset=false");
            cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["Crm_ConnStr"].ConnectionString);
        }
        public enum MsgType
        {
            AlertMessage, Information, Error
        };
        public DsrInfo_DAC(IntPtr handle)
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

        ~DsrInfo_DAC()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            Dispose(false);
        }

        internal DataTable Get_DsrOwner_Details(Int64 UserId, string UserType, string ReportMode)
        {
            //CRM_DSR_Owner_Proc](@UserId bigint,@UserType varchar(50), @ReportMode varchar(50)

            DataTable dt = new DataTable();
            cn1.Open();
            string strProc = string.Empty;
            try
            {
                SqlParameter _UserId = new SqlParameter("@UserId", SqlDbType.BigInt);
                _UserId.Value = UserId;

                SqlParameter _UserType = new SqlParameter("@UserType", SqlDbType.VarChar, 50);
                _UserType.Value = UserType;

                SqlParameter _ReportMode = new SqlParameter("@ReportMode", SqlDbType.VarChar, 50);
                _ReportMode.Value = ReportMode;


                SqlParameter[] paramFields = { _UserId, _UserType, _ReportMode };

                strProc = "CRM_DSR_Owner_Proc";
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
        internal DataTable Get_Dsr_Reasons()
        {
            //CRM_DSR_GetDRS_Info_Proc](@DsrId Bigint,@Mode varchar(50),@TypeFlag varchar(5))

            DataTable dt = new DataTable();
            cn1.Open();
            string strProc = string.Empty;
            try
            {
                strProc = "CRM_DSR_Reasons_Proc";
                if (strProc != string.Empty)
                {
                    SqlDataReader sdr = SqlHelper.ExecuteReader(cn1, CommandType.StoredProcedure, strProc);
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

        internal DataTable Get_Dsr_Details(DateTime FromDt, DateTime ToDt, string Mode, Int64 SoId, Int64 UserId)
        {
            //CRM_DSR_GetDSR_Details_Proc](@FromDt Date,@ToDt Date,@Mode varchar(50), @SoId bigint,@UserId bigint)

            DataTable dt = new DataTable();
            cn1.Open();
            string strProc = string.Empty;
            try
            {
                SqlParameter _FromDt, _ToDt, _Mode, _SoId, _UserId;
                _FromDt = new SqlParameter("@FromDt", SqlDbType.Date);
                _FromDt.Value = FromDt;

                _ToDt = new SqlParameter("@ToDt", SqlDbType.Date);
                _ToDt.Value = ToDt;

                _Mode = new SqlParameter("@Mode", SqlDbType.VarChar, 50);
                _Mode.Value = Mode;

                _SoId = new SqlParameter("@SoId", SqlDbType.BigInt);
                _SoId.Value = SoId;

                _UserId = new SqlParameter("@UserId", SqlDbType.BigInt);
                _UserId.Value = UserId;

                SqlParameter[] paramFields = { _FromDt, _ToDt, _Mode, _SoId, _UserId };

                strProc = "CRM_DSR_GetDSR_Details_Proc";
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

        internal DataTable Get_Dsr_Data(Int64 DsrId, string Mode, string TypeFlag)
        {
            //CRM_DSR_GetDRS_Info_Proc](@DsrId Bigint,@Mode varchar(50),@TypeFlag varchar(5))

            DataTable dt = new DataTable();
            cn1.Open();
            string strProc = string.Empty;
            try
            {
                SqlParameter _DsrId, _Mode, _TypeFlag;

                _DsrId = new SqlParameter("@DsrId", SqlDbType.BigInt);
                _DsrId.Value = DsrId;

                _Mode = new SqlParameter("@Mode", SqlDbType.VarChar, 50);
                _Mode.Value = Mode;

                _TypeFlag = new SqlParameter("@TypeFlag", SqlDbType.VarChar, 50);
                _TypeFlag.Value = TypeFlag;

                SqlParameter[] paramFields = { _DsrId, _Mode, _TypeFlag };

                strProc = "CRM_DSR_GetDRS_Info_Proc";
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

        internal DataTable Get_Dsr_PendingDetails(DateTime FromDt, DateTime ToDt, string Mode, Int64 SoId)
        {
            //CRM_DSR_GetPending_Details_Proc](@FromDt Date,@ToDt Date,@Mode varchar(50), @SoId bigint)

            DataTable dt = new DataTable();
            cn1.Open();
            string strProc = string.Empty;
            try
            {
                SqlParameter _FromDt, _ToDt, _Mode, _SoId;
                _FromDt = new SqlParameter("@FromDt", SqlDbType.Date);
                _FromDt.Value = FromDt;

                _ToDt = new SqlParameter("@ToDt", SqlDbType.Date);
                _ToDt.Value = ToDt;

                _Mode = new SqlParameter("@Mode", SqlDbType.VarChar, 50);
                _Mode.Value = Mode;

                _SoId = new SqlParameter("@SoId", SqlDbType.BigInt);
                _SoId.Value = SoId;


                SqlParameter[] paramFields = { _FromDt, _ToDt, _Mode, _SoId };

                strProc = "CRM_DSR_GetPending_Details_Proc";
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


        internal DataTable Get_Dsr_DateSO_Details(Int64 DsrId, string Mode, Int64 UserId)
        {
            //CRM_DSR_GetDRS_DateSoId__Proc](@DsrId Bigint,@Mode varchar(50),@UserId BigInt)

            DataTable dt = new DataTable();
            cn1.Open();
            string strProc = string.Empty;
            try
            {
                SqlParameter _DsrId, _Mode, _UserId;

                _DsrId = new SqlParameter("@DsrId", SqlDbType.BigInt);
                _DsrId.Value = DsrId;

                _Mode = new SqlParameter("@Mode", SqlDbType.VarChar, 50);
                _Mode.Value = Mode;

                _UserId = new SqlParameter("@UserId", SqlDbType.VarChar, 50);
                _UserId.Value = UserId;

                SqlParameter[] paramFields = { _DsrId, _Mode, _UserId };

                strProc = "CRM_DSR_GetDRS_DateSoId__Proc";
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



        internal DataTable Get_Dsr_LPA_Name_Details(string PrefixText, string LPA_Flag, Int64 SoId)
        {
            //CRM_DSR_GetLPA_Names_Proc](@PrefixText VARCHAR(500),@LPA_Flag varchar(5),@SoId bigint)

            DataTable dt = new DataTable();
            cn1.Open();
            string strProc = string.Empty;
            try
            {
                SqlParameter _PrefixText, _LPA_Flag, _SoId;

                _PrefixText = new SqlParameter("@PrefixText", SqlDbType.VarChar, 500);
                _PrefixText.Value = PrefixText;

                _LPA_Flag = new SqlParameter("@LPA_Flag", SqlDbType.VarChar, 50);
                _LPA_Flag.Value = LPA_Flag;

                _SoId = new SqlParameter("@SoId", SqlDbType.BigInt);
                _SoId.Value = SoId;

                SqlParameter[] paramFields = { _PrefixText, _LPA_Flag, _SoId };

                strProc = "CRM_DSR_GetLPA_Names_Proc";
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

        internal DataTable Get_Existing_Types(string PrefixText, string Type, Int64 SoId)
        {
            //CRM_DSR_GetLPA_Names_Proc](@PrefixText VARCHAR(500),@LPA_Flag varchar(5),@SoId bigint)

            DataTable dt = new DataTable();
            cn1.Open();
            string strProc = string.Empty;
            try
            {
                SqlParameter _PrefixText, _LPA_Flag, _SoId;

                _PrefixText = new SqlParameter("@prefixText", SqlDbType.VarChar, 500);
                _PrefixText.Value = PrefixText;

                _LPA_Flag = new SqlParameter("@Type", SqlDbType.VarChar, 50);
                _LPA_Flag.Value = Type;

                _SoId = new SqlParameter("@SoId", SqlDbType.SmallInt);
                _SoId.Value = Convert.ToInt16(SoId);

                SqlParameter[] paramFields = { _PrefixText, _LPA_Flag, _SoId };

                strProc = "crm_getExistingAccouts_Proc";
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

        internal DataTable Get_Dsr_AllContacts(string LPA_Flag, Int64 LPA_Id)
        {
            //CRM_DSR_Get_AllContacts_Proc](@LPA_Flag varchar(5),@LPA_Id bigint)

            DataTable dt = new DataTable();
            cn1.Open();
            string strProc = string.Empty;
            try
            {
                SqlParameter _LPA_Flag, _LPA_Id;

                _LPA_Flag = new SqlParameter("@LPA_Flag", SqlDbType.VarChar, 50);
                _LPA_Flag.Value = LPA_Flag;

                _LPA_Id = new SqlParameter("@LPA_Id", SqlDbType.BigInt);
                _LPA_Id.Value = LPA_Id;

                SqlParameter[] paramFields = { _LPA_Flag, _LPA_Id };

                strProc = "CRM_DSR_Get_AllContacts_Proc";
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

        //DSR CRUD Operations

        internal CRM_ConExecStatus_Info_Ety Save_Dsr_Details(Dsr_Details_Ety dsr_ety)
        {
            CRM_ConExecStatus_Info_Ety ocrm_ConExecStatus_Info_ety = new CRM_ConExecStatus_Info_Ety();
            cn1.Open();
            string strProc = string.Empty;
            SqlTransaction st = cn1.BeginTransaction();
            try
            {
                SqlParameter vInDsrId, vInSOId, vInDsrDate, vInTotalPOAmt, vInTotalCollectionAmt, vSecurityId, vCmpId, vFinYearId, vUsrId, vMode, vResult, vExecStatus;

                vInDsrId = new SqlParameter("@InDsrId", SqlDbType.BigInt);
                vInDsrId.Direction = ParameterDirection.InputOutput;
                vInDsrId.Value = dsr_ety.DsrId;

                vInSOId = new SqlParameter("@InSOId", SqlDbType.SmallInt);
                vInSOId.Value = dsr_ety.DsrOwner;

                vInDsrDate = new SqlParameter("@InDsrDate", SqlDbType.DateTime);
                vInDsrDate.Value = dsr_ety.DsrDate;

                vInTotalPOAmt = new SqlParameter("@InTotalPOAmt", SqlDbType.Decimal);
                vInTotalPOAmt.Value = dsr_ety.totPoAmt;

                vInTotalCollectionAmt = new SqlParameter("@InTotalCollectionAmt", SqlDbType.Decimal);
                vInTotalCollectionAmt.Value = dsr_ety.totCollAmt;

                vSecurityId = new SqlParameter("@SecurityId ", SqlDbType.SmallInt);
                vSecurityId.Value = dsr_ety.SecurityId;

                vCmpId = new SqlParameter("@CmpId", SqlDbType.SmallInt);
                vCmpId.Value = dsr_ety.CmpId;

                vFinYearId = new SqlParameter("@FinYearId", SqlDbType.SmallInt);
                vFinYearId.Value = dsr_ety.FinYearId;

                vUsrId = new SqlParameter("@UsrId", SqlDbType.SmallInt);
                vUsrId.Value = Convert.ToInt16(dsr_ety.UsrId);

                vMode = new SqlParameter("@Mode", SqlDbType.VarChar);
                vMode.Value = dsr_ety.Mode;

                vResult = new SqlParameter("@Result", SqlDbType.VarChar, 200);
                vResult.Direction = ParameterDirection.InputOutput;
                vResult.Value = "";

                vExecStatus = new SqlParameter("@ExecStatus", SqlDbType.TinyInt);
                vExecStatus.Direction = ParameterDirection.InputOutput;
                vExecStatus.Value = 0;

                SqlParameter[] dsrparams = { vInDsrId, vInSOId, vInDsrDate, vInTotalPOAmt, vInTotalCollectionAmt, vSecurityId, vCmpId, vFinYearId, vUsrId, vMode, vResult, vExecStatus };
                strProc = "CRM_DsrMast_proc";

                CRM_RowMode_Ety rMode_ety = dsr_ety.crm_RowMode_ety;

                if (rMode_ety.RoMode == "Mast")
                    strProc = "CRM_DsrMast_proc";
                else if (rMode_ety.RoMode == "MastAuth")
                {
                    if (dsr_ety.Mode == "MODIFYA")
                        dsr_ety.AuthOpt = 0;
                    else
                        dsr_ety.AuthOpt = 1;
                    strProc = "CRM_DsrMastAuth_Proc";
                }
                else if (rMode_ety.RoMode == "Auth")
                {
                    dsr_ety.AuthOpt = 1;
                    strProc = "CRM_DsrAuth_Proc";
                }

                if (strProc != string.Empty)
                {
                    SqlHelper.ExecuteNonQuery(st, CommandType.StoredProcedure, strProc, dsrparams);

                    if (Convert.ToInt16(vExecStatus.Value.ToString()) == 0)
                    {
                        st.Rollback();
                        cn.Close();
                        ocrm_ConExecStatus_Info_ety.Confirm = vResult.Value.ToString();
                        ocrm_ConExecStatus_Info_ety.ExecStatus = Convert.ToByte(vExecStatus.Value.ToString());
                        return ocrm_ConExecStatus_Info_ety;
                    }
                    ocrm_ConExecStatus_Info_ety.ExecStatus = Convert.ToByte(vExecStatus.Value.ToString());
                    ocrm_ConExecStatus_Info_ety.Confirm = vResult.Value.ToString();
                }

                SqlParameter pInDsrId, pInDsrSlno, pInDsrFromTime, pInDsrToTime, pInAccid, pInAccName, pInAccflag, pInContactName, pInContactDesigName, pInReason,
                    pInPOAmount, pInCollectionAmt, pInRemarks, pInNextVisitOpt, pInNextVisit, pInTaskId, pInSoId,
                    pSecurityId, pCmpId, pFinYearId, pUsrId, pMode, pResult, pExecStatus, pInDsrLeadId;

                pInDsrId = new SqlParameter("@InDsrId", SqlDbType.BigInt);
                pInDsrId.Value = Convert.ToInt64(vInDsrId.Value);

                pInDsrSlno = new SqlParameter("@InDsrSlno", SqlDbType.VarChar);
                pInDsrSlno.Value = dsr_ety.DsrSlno;

                pInDsrFromTime = new SqlParameter("@InDsrFromTime", SqlDbType.VarChar);
                pInDsrFromTime.Value = dsr_ety.Dsrfromtime;

                pInDsrToTime = new SqlParameter("@InDsrToTime", SqlDbType.VarChar);
                pInDsrToTime.Value = dsr_ety.Dsrtotime;

                pInAccid = new SqlParameter("@InAccid", SqlDbType.VarChar);
                pInAccid.Value = dsr_ety.DsrAccId;

                pInAccName = new SqlParameter("@InAccName", SqlDbType.VarChar);
                pInAccName.Value = dsr_ety.DsrAccName;

                pInAccflag = new SqlParameter("@InAccflag", SqlDbType.VarChar);
                pInAccflag.Value = dsr_ety.DsrAccFlag;

                pInContactName = new SqlParameter("@InContactName", SqlDbType.VarChar);
                pInContactName.Value = dsr_ety.DsrCntName;

                pInContactDesigName = new SqlParameter("@InContactDesigName", SqlDbType.VarChar);
                pInContactDesigName.Value = dsr_ety.DsrCntPersnDesig;

                pInReason = new SqlParameter("@InReason", SqlDbType.VarChar);
                pInReason.Value = dsr_ety.Reason;

                pInPOAmount = new SqlParameter("@InPOAmount", SqlDbType.VarChar);
                pInPOAmount.Value = dsr_ety.POAmount;

                pInCollectionAmt = new SqlParameter("@InCollectionAmt", SqlDbType.VarChar);
                pInCollectionAmt.Value = dsr_ety.CollAmt;

                pInRemarks = new SqlParameter("@InRemarks", SqlDbType.VarChar);
                pInRemarks.Value = dsr_ety.Remarks;

                pInNextVisitOpt = new SqlParameter("@InNextVisitOpt", SqlDbType.VarChar);
                pInNextVisitOpt.Value = dsr_ety.NextVisitOpt;

                pInNextVisit = new SqlParameter("@InNextVisit", SqlDbType.VarChar);
                pInNextVisit.Value = dsr_ety.Nextvisit;

                pInTaskId = new SqlParameter("@InTaskId", SqlDbType.VarChar);
                pInTaskId.Value = dsr_ety.TaskId;

                pInSoId = new SqlParameter("@InSoId", SqlDbType.SmallInt);
                pInSoId.Value = dsr_ety.DsrOwner;

                pSecurityId = new SqlParameter("@SecurityId", SqlDbType.SmallInt);
                pSecurityId.Value = dsr_ety.SecurityId;

                pCmpId = new SqlParameter("@CmpId", SqlDbType.SmallInt);
                pCmpId.Value = dsr_ety.CmpId;

                pFinYearId = new SqlParameter("@FinYearId", SqlDbType.SmallInt);
                pFinYearId.Value = dsr_ety.FinYearId;

                pUsrId = new SqlParameter("@UsrId", SqlDbType.SmallInt);
                pUsrId.Value = Convert.ToInt16(dsr_ety.UsrId);

                pMode = new SqlParameter("@Mode", SqlDbType.VarChar);
                pMode.Value = dsr_ety.Mode;

                pResult = new SqlParameter("@Result", SqlDbType.VarChar, 200);
                pResult.Direction = ParameterDirection.InputOutput;
                pResult.Value = "";

                pExecStatus = new SqlParameter("@ExecStatus", SqlDbType.TinyInt);
                pExecStatus.Direction = ParameterDirection.InputOutput;
                pExecStatus.Value = 0;

                pInDsrLeadId = new SqlParameter("@InDsrLeadId", SqlDbType.VarChar);
                pInDsrLeadId.Value = dsr_ety.DsrLeadId;

                SqlParameter[] dsrDtlparams = { pInDsrId, pInDsrSlno, pInDsrFromTime, pInDsrToTime, 
                    pInAccid, pInAccName, pInAccflag, pInContactName, pInContactDesigName, pInReason,
                    pInPOAmount, pInCollectionAmt, pInRemarks, pInNextVisitOpt, pInNextVisit, pInTaskId, pInSoId,
                    pSecurityId, pCmpId, pFinYearId, pUsrId, pMode, pResult, pExecStatus, pInDsrLeadId };

                strProc = "CRM_DsrDetailsMast_proc";

                if (rMode_ety.RoMode == "Mast")
                    strProc = "CRM_DsrDetailsMast_proc";
                else if (rMode_ety.RoMode == "MastAuth")
                    strProc = "CRM_DsrDetailsMastAuth_proc";
                else if (rMode_ety.RoMode == "Auth")
                    strProc = "CRM_DsrDetailsAuth_Proc";

                if (strProc != string.Empty)
                {
                    SqlHelper.ExecuteNonQuery(st, CommandType.StoredProcedure, strProc, dsrDtlparams);

                    if (Convert.ToInt16(pExecStatus.Value.ToString()) == 0)
                    {
                        st.Rollback();
                        cn.Close();
                        ocrm_ConExecStatus_Info_ety.Confirm = pResult.Value.ToString();
                        ocrm_ConExecStatus_Info_ety.ExecStatus = Convert.ToByte(pExecStatus.Value.ToString());
                        return ocrm_ConExecStatus_Info_ety;
                    }
                    ocrm_ConExecStatus_Info_ety.ExecStatus = Convert.ToByte(pExecStatus.Value.ToString());
                    ocrm_ConExecStatus_Info_ety.Confirm = pResult.Value.ToString();
                }

                st.Commit();

                if (dsr_ety.DsrId != 0)
                {
                    try
                    {
                        SqlParameter dsr_id = new SqlParameter("@InDsrId", SqlDbType.BigInt);
                        dsr_id.Value = dsr_ety.DsrId;
                        SqlParameter Auth_Opt = new SqlParameter("@AuthOpt", SqlDbType.SmallInt);
                        Auth_Opt.Value = dsr_ety.AuthOpt;
                        SqlParameter[] DsrFields = { dsr_id, Auth_Opt };
                        int Email = SqlHelper.ExecuteNonQuery(cn1, CommandType.StoredProcedure, "CRM_DsrDetailsEmailSending_Proc", DsrFields);
                    }
                    catch (Exception)
                    {
                        cn1.Close();
                    }
                }

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

        internal CRM_ConExecStatus_Info_Ety Delete_Dsr_Details(Dsr_Details_Ety dsr_ety)
        {
            CRM_ConExecStatus_Info_Ety ocrm_ConExecStatus_Info_ety = new CRM_ConExecStatus_Info_Ety();
            cn1.Open();
            string strProc = string.Empty;
            try
            {
                SqlParameter vInDsrId, vInSOId, vInDsrDate, vInTotalPOAmt, vInTotalCollectionAmt, vSecurityId, vCmpId, vFinYearId, vUsrId, vMode, vResult, vExecStatus;

                vInDsrId = new SqlParameter("@InDsrId", SqlDbType.BigInt);
                vInDsrId.Direction = ParameterDirection.InputOutput;
                vInDsrId.Value = dsr_ety.DsrId;

                vInSOId = new SqlParameter("@InSOId", SqlDbType.SmallInt);
                vInSOId.Value = dsr_ety.DsrOwner;

                vInDsrDate = new SqlParameter("@InDsrDate", SqlDbType.DateTime);
                vInDsrDate.Value = dsr_ety.DsrDate;

                vInTotalPOAmt = new SqlParameter("@InTotalPOAmt", SqlDbType.Decimal);
                vInTotalPOAmt.Value = dsr_ety.totPoAmt;

                vInTotalCollectionAmt = new SqlParameter("@InTotalCollectionAmt", SqlDbType.Decimal);
                vInTotalCollectionAmt.Value = dsr_ety.totCollAmt;

                vSecurityId = new SqlParameter("@SecurityId ", SqlDbType.SmallInt);
                vSecurityId.Value = dsr_ety.SecurityId;

                vCmpId = new SqlParameter("@CmpId", SqlDbType.SmallInt);
                vCmpId.Value = dsr_ety.CmpId;

                vFinYearId = new SqlParameter("@FinYearId", SqlDbType.SmallInt);
                vFinYearId.Value = dsr_ety.FinYearId;

                vUsrId = new SqlParameter("@UsrId", SqlDbType.SmallInt);
                vUsrId.Value = Convert.ToInt16(dsr_ety.UsrId);

                vMode = new SqlParameter("@Mode", SqlDbType.VarChar);
                vMode.Value = dsr_ety.Mode;

                vResult = new SqlParameter("@Result", SqlDbType.VarChar, 200);
                vResult.Direction = ParameterDirection.InputOutput;
                vResult.Value = "";

                vExecStatus = new SqlParameter("@ExecStatus", SqlDbType.TinyInt);
                vExecStatus.Direction = ParameterDirection.InputOutput;
                vExecStatus.Value = 0;

                SqlParameter[] dsrparams = { vInDsrId, vInSOId, vInDsrDate, vInTotalPOAmt, vInTotalCollectionAmt, vSecurityId, vCmpId, vFinYearId, vUsrId, vMode, vResult, vExecStatus };
                strProc = "CRM_DsrMast_proc";

                CRM_RowMode_Ety rMode_ety = dsr_ety.crm_RowMode_ety;

                if (rMode_ety.RoMode == "Mast")
                    strProc = "CRM_DsrMast_proc";
                else if (rMode_ety.RoMode == "MastAuth")
                    strProc = "CRM_DsrMastAuth_Proc";
                else if (rMode_ety.RoMode == "Auth")
                    strProc = "CRM_DsrAuth_Proc";

                if (strProc != string.Empty)
                {
                    SqlHelper.ExecuteNonQuery(cn1, CommandType.StoredProcedure, strProc, dsrparams);

                    if (Convert.ToInt16(vExecStatus.Value.ToString()) == 0)
                    {
                        cn.Close();
                        ocrm_ConExecStatus_Info_ety.Confirm = vResult.Value.ToString();
                        ocrm_ConExecStatus_Info_ety.ExecStatus = Convert.ToByte(vExecStatus.Value.ToString());
                        return ocrm_ConExecStatus_Info_ety;
                    }
                    ocrm_ConExecStatus_Info_ety.ExecStatus = Convert.ToByte(vExecStatus.Value.ToString());
                    ocrm_ConExecStatus_Info_ety.Confirm = vResult.Value.ToString();
                }

                if (cn1 != null && cn1.State == ConnectionState.Open) cn1.Close();
            }
            catch (Exception msg)
            {
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

        internal CRM_ConExecStatus_Info_Ety Authorize_Dsr_Details(Dsr_Details_Ety dsr_ety)
        {
            CRM_ConExecStatus_Info_Ety ocrm_ConExecStatus_Info_ety = new CRM_ConExecStatus_Info_Ety();
            cn1.Open();
            string strProc = string.Empty;
            SqlTransaction st = cn1.BeginTransaction();
            try
            {
                SqlParameter vInDsrId, vInDsrSlno, vInAuth, vInDsrAuth, vInRemarks, vSecurityId, vCmpId, vYearId, vUsrId, vResult, vExecStatus;

                vInDsrId = new SqlParameter("@InDsrId", SqlDbType.BigInt);
                vInDsrId.Value = dsr_ety.DsrId;

                vInDsrSlno = new SqlParameter("@InDsrSlno", SqlDbType.VarChar);
                vInDsrSlno.Value = dsr_ety.DsrSlno;

                vInAuth = new SqlParameter("@InAuth", SqlDbType.VarChar);
                vInAuth.Value = dsr_ety.DsrAuth;

                vInDsrAuth = new SqlParameter("@InDsrAuth", SqlDbType.VarChar);
                vInDsrAuth.Value = dsr_ety.DsrSlnoAuth;

                vInRemarks = new SqlParameter("@InRemarks", SqlDbType.VarChar);
                vInRemarks.Value = dsr_ety.ARemarks;

                vSecurityId = new SqlParameter("@SecurityId ", SqlDbType.SmallInt);
                vSecurityId.Value = dsr_ety.SecurityId;

                vCmpId = new SqlParameter("@CmpId", SqlDbType.SmallInt);
                vCmpId.Value = dsr_ety.CmpId;

                vYearId = new SqlParameter("@YearId", SqlDbType.SmallInt);
                vYearId.Value = dsr_ety.FinYearId;

                vUsrId = new SqlParameter("@UsrId", SqlDbType.SmallInt);
                vUsrId.Value = Convert.ToInt16(dsr_ety.UsrId);

                vResult = new SqlParameter("@Result", SqlDbType.VarChar, 200);
                vResult.Direction = ParameterDirection.InputOutput;
                vResult.Value = "";

                vExecStatus = new SqlParameter("@ExecStatus", SqlDbType.TinyInt);
                vExecStatus.Direction = ParameterDirection.InputOutput;
                vExecStatus.Value = 0;

                SqlParameter[] dsrparams = { vInDsrId, vInDsrSlno, vInAuth, vInDsrAuth, vInRemarks, vSecurityId, vCmpId, vYearId, vUsrId, vResult, vExecStatus };
                strProc = "CRM_DsrAuthorization_proc";

                if (strProc != string.Empty)
                {
                    SqlHelper.ExecuteNonQuery(st, CommandType.StoredProcedure, strProc, dsrparams);

                    if (Convert.ToInt16(vExecStatus.Value.ToString()) == 0)
                    {
                        st.Rollback();
                        cn.Close();
                        ocrm_ConExecStatus_Info_ety.Confirm = vResult.Value.ToString();
                        ocrm_ConExecStatus_Info_ety.ExecStatus = Convert.ToByte(vExecStatus.Value.ToString());
                        return ocrm_ConExecStatus_Info_ety;
                    }
                    ocrm_ConExecStatus_Info_ety.ExecStatus = Convert.ToByte(vExecStatus.Value.ToString());
                    ocrm_ConExecStatus_Info_ety.Confirm = vResult.Value.ToString();
                }

                st.Commit();

                if (dsr_ety.DsrId != 0)
                {
                    try
                    {
                        SqlParameter dsr_id = new SqlParameter("@InDsrId", SqlDbType.BigInt);
                        dsr_id.Value = dsr_ety.DsrId;

                        SqlParameter[] DsrFields = { dsr_id };
                        int Email = SqlHelper.ExecuteNonQuery(cn1, CommandType.StoredProcedure, "Crm_SendMailToSoFromDsr_proc", DsrFields);
                    }
                    catch (Exception ex)
                    {
                        cn1.Close();
                    }
                }

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
        //public string insertDSR(Dsr_Details_Ety dsr_ety, string ProcType)
        //{

        //    // string strcon = setCon();
        //    // try
        //    //  {
        //    SqlConnection con = new SqlConnection(setCon());
        //    con.Open();
        //    SqlTransaction st = con.BeginTransaction();
        //    SqlParameter[] DSRDoc = new SqlParameter[12];
        //    DSRDoc[0] = new SqlParameter("@InDsrId", SqlDbType.BigInt);
        //    DSRDoc[0].Direction = ParameterDirection.InputOutput;
        //    DSRDoc[1] = new SqlParameter("@InSOId", SqlDbType.SmallInt);
        //    DSRDoc[2] = new SqlParameter("@InDsrDate", SqlDbType.DateTime);
        //    DSRDoc[3] = new SqlParameter("@InTotalPOAmt", SqlDbType.Decimal);
        //    DSRDoc[4] = new SqlParameter("@InTotalCollectionAmt", SqlDbType.Decimal);
        //    DSRDoc[5] = new SqlParameter("@SecurityId ", SqlDbType.SmallInt);
        //    DSRDoc[6] = new SqlParameter("@CmpId", SqlDbType.SmallInt);
        //    DSRDoc[7] = new SqlParameter("@FinYearId", SqlDbType.SmallInt);
        //    DSRDoc[8] = new SqlParameter("@UsrId", SqlDbType.SmallInt);
        //    DSRDoc[9] = new SqlParameter("@Mode", SqlDbType.VarChar);
        //    DSRDoc[10] = new SqlParameter("@Result", SqlDbType.VarChar, 200);
        //    DSRDoc[10].Direction = ParameterDirection.InputOutput;
        //    DSRDoc[11] = new SqlParameter("@ExecStatus", SqlDbType.TinyInt);
        //    DSRDoc[11].Direction = ParameterDirection.InputOutput;


        //    DSRDoc[0].Value = dsr.DsrId;
        //    DSRDoc[1].Value = dsr.DsrOwner;
        //    DSRDoc[2].Value = dsr.DsrDate;
        //    DSRDoc[3].Value = dsr.totPoAmt;
        //    DSRDoc[4].Value = dsr.totCollAmt;
        //    DSRDoc[5].Value = dsr.SecurityId;
        //    DSRDoc[6].Value = dsr.CmpId;
        //    DSRDoc[7].Value = dsr.FinYearId;
        //    DSRDoc[8].Value = dsr.UsrId;
        //    DSRDoc[9].Value = dsr.Mode;
        //    DSRDoc[10].Value = "";
        //    DSRDoc[11].Value = 0;

        //    dsr.DsrId = Convert.ToInt64(DSRDoc[0].Value);
        //    int i = 0;
        //    if (ProcType == "Mast")
        //        i = DAL.ExecuteNonQuery(st, CommandType.StoredProcedure, "CRM_DsrMast_proc", DSRDoc);
        //    else if (ProcType == "MastAuth")
        //    {
        //        if (dsr.Mode == "MODIFYA")
        //            dsr.AuthOpt = 0;
        //        else
        //            dsr.AuthOpt = 1;
        //        i = DAL.ExecuteNonQuery(st, CommandType.StoredProcedure, "CRM_DsrMastAuth_Proc", DSRDoc);
        //        dsr.DsrId = Convert.ToInt64(DSRDoc[0].Value);
        //    }
        //    else if (ProcType == "Auth")
        //    {
        //        dsr.AuthOpt = 1;
        //        i = DAL.ExecuteNonQuery(st, CommandType.StoredProcedure, "CRM_DsrAuth_Proc", DSRDoc);
        //        dsr.DsrId = Convert.ToInt64(DSRDoc[0].Value);
        //    }

        //    if (DSRDoc[11].Value.ToString() != "1")
        //    {
        //        st.Rollback();
        //        con.Close();
        //        return DSRDoc[10].Value.ToString();
        //    }

        //    SqlParameter[] DSRDtDoc = new SqlParameter[25];
        //    DSRDtDoc[0] = new SqlParameter("@InDsrId", SqlDbType.BigInt);
        //    DSRDtDoc[1] = new SqlParameter("@InDsrSlno", SqlDbType.VarChar);
        //    DSRDtDoc[2] = new SqlParameter("@InDsrFromTime", SqlDbType.VarChar);
        //    DSRDtDoc[3] = new SqlParameter("@InDsrToTime", SqlDbType.VarChar);
        //    DSRDtDoc[4] = new SqlParameter("@InAccid", SqlDbType.VarChar);
        //    DSRDtDoc[5] = new SqlParameter("@InAccName", SqlDbType.VarChar);
        //    DSRDtDoc[6] = new SqlParameter("@InAccflag", SqlDbType.VarChar);
        //    DSRDtDoc[7] = new SqlParameter("@InContactName", SqlDbType.VarChar);
        //    DSRDtDoc[8] = new SqlParameter("@InContactDesigName", SqlDbType.VarChar);
        //    DSRDtDoc[9] = new SqlParameter("@InReason", SqlDbType.VarChar);
        //    DSRDtDoc[10] = new SqlParameter("@InPOAmount", SqlDbType.VarChar);
        //    DSRDtDoc[11] = new SqlParameter("@InCollectionAmt", SqlDbType.VarChar);
        //    DSRDtDoc[12] = new SqlParameter("@InRemarks", SqlDbType.VarChar);
        //    DSRDtDoc[13] = new SqlParameter("@InNextVisitOpt", SqlDbType.VarChar);
        //    DSRDtDoc[14] = new SqlParameter("@InNextVisit", SqlDbType.VarChar);
        //    DSRDtDoc[15] = new SqlParameter("@InTaskId", SqlDbType.VarChar);
        //    DSRDtDoc[16] = new SqlParameter("@InSoId", SqlDbType.SmallInt);
        //    DSRDtDoc[17] = new SqlParameter("@SecurityId", SqlDbType.SmallInt);
        //    DSRDtDoc[18] = new SqlParameter("@CmpId", SqlDbType.SmallInt);
        //    DSRDtDoc[19] = new SqlParameter("@FinYearId", SqlDbType.SmallInt);
        //    DSRDtDoc[20] = new SqlParameter("@UsrId", SqlDbType.SmallInt);
        //    DSRDtDoc[21] = new SqlParameter("@Mode", SqlDbType.VarChar);
        //    DSRDtDoc[22] = new SqlParameter("@Result", SqlDbType.VarChar, 200);
        //    DSRDtDoc[22].Direction = ParameterDirection.InputOutput;
        //    DSRDtDoc[23] = new SqlParameter("@ExecStatus", SqlDbType.TinyInt);
        //    DSRDtDoc[23].Direction = ParameterDirection.InputOutput;
        //    DSRDtDoc[24] = new SqlParameter("@InDsrLeadId", SqlDbType.VarChar);


        //    DSRDtDoc[0].Value = Convert.ToInt64(DSRDoc[0].Value);
        //    DSRDtDoc[1].Value = dsr.DsrSlno;
        //    DSRDtDoc[2].Value = dsr.Dsrfromtime;
        //    DSRDtDoc[3].Value = dsr.Dsrtotime;
        //    DSRDtDoc[4].Value = dsr.DsrAccId;
        //    DSRDtDoc[5].Value = dsr.DsrAccName;
        //    DSRDtDoc[6].Value = dsr.DsrAccFlag;
        //    DSRDtDoc[7].Value = dsr.DsrCntName;
        //    DSRDtDoc[8].Value = dsr.DsrCntPersnDesig;
        //    DSRDtDoc[9].Value = dsr.Reason;
        //    DSRDtDoc[10].Value = dsr.POAmount;
        //    DSRDtDoc[11].Value = dsr.CollAmt;
        //    DSRDtDoc[12].Value = dsr.Remarks;
        //    DSRDtDoc[13].Value = dsr.NextVisitOpt;
        //    DSRDtDoc[14].Value = dsr.Nextvisit;
        //    DSRDtDoc[15].Value = dsr.TaskId;
        //    DSRDtDoc[16].Value = dsr.DsrOwner;
        //    DSRDtDoc[17].Value = dsr.SecurityId;
        //    DSRDtDoc[18].Value = dsr.CmpId;
        //    DSRDtDoc[19].Value = dsr.FinYearId;
        //    DSRDtDoc[20].Value = dsr.UsrId;
        //    DSRDtDoc[21].Value = dsr.Mode;
        //    DSRDtDoc[22].Value = "";
        //    DSRDtDoc[23].Value = 0;
        //    DSRDtDoc[24].Value = dsr.DsrLeadId;



        //    if (ProcType == "Mast")
        //        i = DAL.ExecuteNonQuery(st, CommandType.StoredProcedure, "CRM_DsrDetailsMast_proc", DSRDtDoc);
        //    else if (ProcType == "MastAuth")
        //        i = DAL.ExecuteNonQuery(st, CommandType.StoredProcedure, "CRM_DsrDetailsMastAuth_proc", DSRDtDoc);
        //    else if (ProcType == "Auth")
        //        i = DAL.ExecuteNonQuery(st, CommandType.StoredProcedure, "CRM_DsrDetailsAuth_Proc", DSRDtDoc);

        //    if (DSRDtDoc[23].Value.ToString() != "1")
        //    {
        //        st.Rollback();
        //        con.Close();
        //        return DSRDtDoc[22].Value.ToString();
        //    }

        //    st.Commit();
        //    if (dsr.DsrId != 0)
        //    {
        //        try
        //        {
        //            SqlParameter dsr_id = new SqlParameter("@InDsrId", SqlDbType.BigInt);
        //            dsr_id.Value = dsr.DsrId;
        //            SqlParameter Auth_Opt = new SqlParameter("@AuthOpt", SqlDbType.SmallInt);
        //            Auth_Opt.Value = dsr.AuthOpt;
        //            SqlParameter[] DsrFields = { dsr_id, Auth_Opt };
        //            int Email = DAL.ExecuteNonQuery(setCon(), CommandType.StoredProcedure, "CRM_DsrDetailsEmailSending_Proc", DsrFields);

        //        }
        //        catch (Exception ex)
        //        {
        //            con.Close();
        //        }
        //        finally { }
        //    }
        //    con.Close();
        //    return DSRDtDoc[22].Value.ToString();
        //}
        //public string DeleteDSR(Dsr_Details_Ety dsr, string ProcType)
        //{

        //    // string strcon = setCon();
        //    // try
        //    //  {
        //    SqlConnection con = new SqlConnection(setCon());
        //    con.Open();
        //    SqlTransaction st = con.BeginTransaction();
        //    SqlParameter[] DSRDoc = new SqlParameter[12];
        //    DSRDoc[0] = new SqlParameter("@InDsrId", SqlDbType.BigInt);
        //    DSRDoc[0].Direction = ParameterDirection.InputOutput;
        //    DSRDoc[1] = new SqlParameter("@InSOId", SqlDbType.SmallInt);
        //    DSRDoc[2] = new SqlParameter("@InDsrDate", SqlDbType.DateTime);
        //    DSRDoc[3] = new SqlParameter("@InTotalPOAmt", SqlDbType.Decimal);
        //    DSRDoc[4] = new SqlParameter("@InTotalCollectionAmt", SqlDbType.Decimal);
        //    DSRDoc[5] = new SqlParameter("@SecurityId ", SqlDbType.SmallInt);
        //    DSRDoc[6] = new SqlParameter("@CmpId", SqlDbType.SmallInt);
        //    DSRDoc[7] = new SqlParameter("@FinYearId", SqlDbType.SmallInt);
        //    DSRDoc[8] = new SqlParameter("@UsrId", SqlDbType.SmallInt);
        //    DSRDoc[9] = new SqlParameter("@Mode", SqlDbType.VarChar);
        //    DSRDoc[10] = new SqlParameter("@Result", SqlDbType.VarChar, 200);
        //    DSRDoc[10].Direction = ParameterDirection.InputOutput;
        //    DSRDoc[11] = new SqlParameter("@ExecStatus", SqlDbType.TinyInt);
        //    DSRDoc[11].Direction = ParameterDirection.InputOutput;


        //    DSRDoc[0].Value = dsr.DsrId;
        //    DSRDoc[1].Value = dsr.DsrOwner;
        //    DSRDoc[2].Value = dsr.DsrDate;
        //    DSRDoc[3].Value = dsr.totPoAmt;
        //    DSRDoc[4].Value = dsr.totCollAmt;
        //    DSRDoc[5].Value = dsr.SecurityId;
        //    DSRDoc[6].Value = dsr.CmpId;
        //    DSRDoc[7].Value = dsr.FinYearId;
        //    DSRDoc[8].Value = dsr.UsrId;
        //    DSRDoc[9].Value = dsr.Mode;
        //    DSRDoc[10].Value = "";
        //    DSRDoc[11].Value = 0;

        //    dsr.DsrId = Convert.ToInt64(DSRDoc[0].Value);
        //    int i = 0;
        //    if (ProcType == "Mast")
        //        i = DAL.ExecuteNonQuery(st, CommandType.StoredProcedure, "CRM_DsrMast_proc", DSRDoc);
        //    else if (ProcType == "MastAuth")
        //        i = DAL.ExecuteNonQuery(st, CommandType.StoredProcedure, "CRM_DsrMastAuth_Proc", DSRDoc);
        //    else if (ProcType == "Auth")
        //        i = DAL.ExecuteNonQuery(st, CommandType.StoredProcedure, "CRM_DsrAuth_Proc", DSRDoc);

        //    if (DSRDoc[11].Value.ToString() != "1")
        //    {
        //        st.Rollback();
        //        con.Close();
        //        return DSRDoc[10].Value.ToString();
        //    }

        //    st.Commit();
        //    con.Close();
        //    return DSRDoc[10].Value.ToString();
        //}
        //public string AuthorizeDSR(Dsr_Details_Ety dsr)
        //{
        //    SqlConnection con = new SqlConnection(setCon());
        //    con.Open();
        //    SqlTransaction st = con.BeginTransaction();
        //    SqlParameter[] DSRDoc = new SqlParameter[14];
        //    DSRDoc[0] = new SqlParameter("@InDsrId", SqlDbType.BigInt);
        //    DSRDoc[1] = new SqlParameter("@InDsrSlno", SqlDbType.VarChar);
        //    DSRDoc[2] = new SqlParameter("@InAuth", SqlDbType.VarChar);
        //    DSRDoc[3] = new SqlParameter("@InDsrAuth", SqlDbType.VarChar);
        //    DSRDoc[4] = new SqlParameter("@InRemarks", SqlDbType.VarChar);
        //    DSRDoc[5] = new SqlParameter("@SecurityId ", SqlDbType.SmallInt);
        //    DSRDoc[6] = new SqlParameter("@CmpId", SqlDbType.SmallInt);
        //    DSRDoc[7] = new SqlParameter("@YearId", SqlDbType.SmallInt);
        //    DSRDoc[8] = new SqlParameter("@UsrId", SqlDbType.SmallInt);
        //    DSRDoc[9] = new SqlParameter("@Result", SqlDbType.VarChar, 200);
        //    DSRDoc[9].Direction = ParameterDirection.InputOutput;
        //    DSRDoc[10] = new SqlParameter("@ExecStatus", SqlDbType.TinyInt);
        //    DSRDoc[10].Direction = ParameterDirection.InputOutput;


        //    DSRDoc[0].Value = dsr.DsrId;
        //    DSRDoc[1].Value = dsr.DsrSlno;
        //    DSRDoc[2].Value = dsr.DsrAuth;
        //    DSRDoc[3].Value = dsr.DsrSlnoAuth;
        //    DSRDoc[4].Value = dsr.ARemarks;
        //    DSRDoc[5].Value = dsr.SecurityId;
        //    DSRDoc[6].Value = dsr.CmpId;
        //    DSRDoc[7].Value = dsr.FinYearId;
        //    DSRDoc[8].Value = dsr.UsrId;
        //    DSRDoc[9].Value = "";
        //    DSRDoc[10].Value = 0;

        //    int i = 0;
        //    i = DAL.ExecuteNonQuery(st, CommandType.StoredProcedure, "CRM_DsrAuthorization_proc", DSRDoc);

        //    if (DSRDoc[10].Value.ToString() != "1")
        //    {
        //        st.Rollback();
        //        con.Close();
        //        return DSRDoc[9].Value.ToString();
        //    }

        //    st.Commit();

        //    if (dsr.DsrId != 0)
        //    {
        //        try
        //        {
        //            SqlParameter dsr_id = new SqlParameter("@InDsrId", SqlDbType.BigInt);
        //            dsr_id.Value = dsr.DsrId;

        //            SqlParameter[] DsrFields = { dsr_id };
        //            int Email = DAL.ExecuteNonQuery(setCon(), CommandType.StoredProcedure, "Crm_SendMailToSoFromDsr_proc", DsrFields);
        //        }
        //        catch (Exception ex)
        //        {
        //            con.Close();
        //        }
        //        finally { }

        //    }
        //    con.Close();
        //    return DSRDoc[9].Value.ToString();

        //}


        /* chanal sales DSR */
        internal DataTable Get_KW_Dsr_AccInfo(string PrefixText, string ContexKey)
        {
            //CRM_Get_KW_AccInfo_Proc @PrefixText VARCHAR(500),@ContexKey VARCHAR(50)

            DataTable dt = new DataTable();
            cn1.Open();
            string strProc = string.Empty;
            try
            {
                SqlParameter _PrefixText, _ContexKey;

                _PrefixText = new SqlParameter("@PrefixText", SqlDbType.VarChar);
                _PrefixText.Value = PrefixText;

                _ContexKey = new SqlParameter("@ContexKey", SqlDbType.VarChar);
                _ContexKey.Value = ContexKey;

                SqlParameter[] paramFields = { _PrefixText, _ContexKey };

                strProc = "CRM_Get_KW_AccInfo_Proc";
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




    }
}
