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
    public class Trans_Main_DAC : IDisposable
    {
        // Pointer to an external unmanaged resource.
        private IntPtr handle;
        // Track whether Dispose has been called.
        private bool disposed = false;

        //EntityConnection cn1;
        SqlConnection cn, cn1, cn2;
        string res;
        int n;
        public Trans_Main_DAC()
        {
            cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Crm_CommonDbStr"].ConnectionString);
            //cn = new SqlConnection("Data source=192.168.0.90;Database=db_repository;user id=sa;password=pwd;Timeout=360;Connection reset=false");
            cn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["Crm_ConnStr"].ConnectionString);
            cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["GWTME_ConnStr"].ConnectionString);
        }
        public enum MsgType
        {
            AlertMessage, Information, Error
        };
        public Trans_Main_DAC(IntPtr handle)
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

        ~Trans_Main_DAC()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            Dispose(false);
        }

        public DataTable Get_TransUserRights(TransUser_Ety tu_ety)
        {

            //CRM_BDF_Get_TransUser_Rights_Proc (@UserId BIGINT, @FormatId SMALLINT, @FinId SMALLINT, @Flag VARCHAR (10)
            DataTable dt = new DataTable();
            cn2.Open();
            string strProc = string.Empty;
            try
            {
                SqlParameter _UserId = new SqlParameter("@UserId", SqlDbType.BigInt);
                _UserId.Value = tu_ety.UserId;

                SqlParameter _FormatId = new SqlParameter("@FormatId", SqlDbType.SmallInt);
                _FormatId.Value = tu_ety.FormatId;

                SqlParameter _FinId = new SqlParameter("@FinId", SqlDbType.BigInt);
                _FinId.Value = tu_ety.FinId;

                SqlParameter _Flag = new SqlParameter("@Flag", SqlDbType.VarChar, 50);
                _Flag.Value = tu_ety.TransMode;

                SqlParameter[] paramFields = { _UserId, _FormatId, _FinId, _Flag };

                strProc = "etrs_Get_TransUser_Rights_Proc";
                if (strProc != string.Empty)
                {
                    SqlDataReader sdr = SqlHelper.ExecuteReader(cn2, CommandType.StoredProcedure, strProc, paramFields);
                    dt.Load(sdr);

                    if (!sdr.IsClosed)
                        sdr.Close();
                }
            }
            catch (Exception msg)
            {
                cn2.Close();
                if (res == null)
                    res = msg.Message;
            }
            finally
            {
                if (cn2.State == ConnectionState.Open)
                    cn2.Close();
                if (cn2 != null) cn2.Dispose();
            }
            return dt;

        }

        public DataTable Get_TransLvlRights(TransUser_Ety tu_ety)
        {

            //CRM_BDF_Get_TransUser_Rights_Proc (@UserId BIGINT, @FormatId SMALLINT, @FinId SMALLINT, @Flag VARCHAR (10)
            DataTable dt = new DataTable();
            cn2.Open();
            string strProc = string.Empty;
            try
            {
                SqlParameter _UserId = new SqlParameter("@UserId", SqlDbType.BigInt);
                _UserId.Value = tu_ety.UserId;

                SqlParameter _FormatId = new SqlParameter("@FormatId", SqlDbType.SmallInt);
                _FormatId.Value = tu_ety.FormatId;

                SqlParameter _FinId = new SqlParameter("@FinId", SqlDbType.BigInt);
                _FinId.Value = tu_ety.FinId;

                SqlParameter[] paramFields = { _UserId, _FormatId, _FinId };

                strProc = "Etrs_Get_TransLvl_Rights_Proc";
                if (strProc != string.Empty)
                {
                    SqlDataReader sdr = SqlHelper.ExecuteReader(cn2, CommandType.StoredProcedure, strProc, paramFields);
                    dt.Load(sdr);

                    if (!sdr.IsClosed)
                        sdr.Close();
                }
            }
            catch (Exception msg)
            {
                cn2.Close();
                if (res == null)
                    res = msg.Message;
            }
            finally
            {
                if (cn2.State == ConnectionState.Open)
                    cn2.Close();
                if (cn2 != null) cn2.Dispose();
            }
            return dt;

        }
        public DataTable Get_TransFormats(TransUser_Ety tu_ety)
        {
            DataTable dt = new DataTable();
            cn1.Open();
            string strProc = string.Empty;
            try
            {
                SqlParameter usr_Id, fin_Id, comp_Id;
                usr_Id = new SqlParameter("@UserId", SqlDbType.SmallInt);
                usr_Id.Value = tu_ety.UserId;
                fin_Id = new SqlParameter("@FinId", SqlDbType.SmallInt);
                fin_Id.Value = tu_ety.FinId;
                comp_Id = new SqlParameter("@CompId", SqlDbType.SmallInt);
                comp_Id.Value = tu_ety.CompId;
                SqlParameter[] paramFields = { usr_Id, fin_Id, comp_Id };

                strProc = "CRM_Trans_Get_Formats_Proc";
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
        public DataTable Get_Transactions(TransUser_Ety tu_ety)
        {
            DataTable dt = new DataTable();

            SqlParameter _FromDt, _ToDt, _Mode, _FormatId;
            _FromDt = new SqlParameter("@FromDt", SqlDbType.Date);
            _FromDt.Value = tu_ety.FromDt;

            _ToDt = new SqlParameter("@ToDt", SqlDbType.Date);
            _ToDt.Value = tu_ety.ToDt;

            _Mode = new SqlParameter("@Mode", SqlDbType.VarChar, 50);
            _Mode.Value = tu_ety.TransMode;

            _FormatId = new SqlParameter("@FormatId", SqlDbType.SmallInt);
            _FormatId.Value = tu_ety.FormatId;

            SqlParameter[] paramFields = { _FromDt, _ToDt, _Mode, _FormatId };

            if (tu_ety.ParentFormatId == 27)
            {
                //Etrs_Get_Transactions_proc(@FromDt Date,@ToDt Date,@Mode varchar(50), @FormatId smallint)
                cn2.Open();
                string strProc = string.Empty;
                try
                {
                    strProc = "Etrs_Get_Transactions_proc";
                    if (strProc != string.Empty)
                    {
                        SqlDataReader sdr = SqlHelper.ExecuteReader(cn2, CommandType.StoredProcedure, strProc, paramFields);
                        dt.Load(sdr);

                        if (!sdr.IsClosed)
                            sdr.Close();
                    }
                }
                catch (Exception msg)
                {
                    cn2.Close();
                    if (res == null)
                        res = msg.Message;
                }
                finally
                {
                    if (cn2.State == ConnectionState.Open)
                        cn2.Close();
                    if (cn2 != null) cn2.Dispose();
                }
            }
            else
            {
                //CRM_Trans_Get_Transactions_proc(@FromDt Date,@ToDt Date,@Mode varchar(50), @FormatId smallint)
                cn1.Open();
                string strProc = string.Empty;
                try
                {
                    strProc = "CRM_Trans_Get_Transactions_proc";
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
            }
            return dt;

        }

        public DataTable Check_Trans_CreditDays(TransUser_Ety tu_ety)
        {
            DataTable dt = new DataTable();
            string strQuey = string.Empty;
            if (tu_ety.DeptId == 0)
            {
                strQuey = "SELECT th.docid FROM ETRS_TRANSHDR_MAST_TBL th ,etrs_TRANSDTL1_tbl td WHERE th.docid=td.docid AND td.pending_qty>0  and th.format_id=" + tu_ety.FormatId + " AND th.accid=" + tu_ety.AccId + "  AND   datediff(day,docdt,getdate()) >(SELECT crdt_period FROM ETRS_Accdtls_Tbl  am WHERE acc_id =" + tu_ety.AccId + ")";
            }
            else
            {
                strQuey = "SELECT th.docid FROM ETRS_TRANSHDR_MAST_TBL th ,etrs_TRANSDTL1_tbl td WHERE th.docid=td.docid AND td.pending_qty>0  and th.format_id=" + tu_ety.FormatId + " AND th.accid=" + tu_ety.AccId + "  AND th.deptid=" + tu_ety.DeptId + " and  datediff(day,docdt,getdate()) >(SELECT CreditPeriod FROM ETRS_Accdept_Tbl  am WHERE acc_id =" + tu_ety.AccId + " and dept_id=" + tu_ety.DeptId + " )";
            }

            if (strQuey != string.Empty)
                dt = Get_GWTME_StrQuery_Data(strQuey);

            return dt;
        }

        public DataTable Check_Trans_BeyondCreditLimit(TransUser_Ety tu_ety)
        {
            DataTable dt = new DataTable();
            string strQuey = string.Empty;

            strQuey = "SELECT isnull(a.Crdt_Lmt,0) Crdt_Lmt,isnull(a.Crdt_Period,0)Crdt_Period,isnull(ad.CreditLmt,0) CreditLmt,isnull(ad.CreditPeriod,0) CreditPeriod,isnull(a.Lower_CrdtLmt,0) Lower_CrdtLmt,a.allowcombi " +
            " FROM ETRS_AccDtls_Tbl a,Etrs_AccDept_tbl ad " +
            " WHERE a.Acc_Id=ad.Acc_Id and a.Acc_Id=" + tu_ety.AccId + " " +
            " AND ad.dept_Id=" + tu_ety.DeptId + "";

            if (strQuey != string.Empty)
                dt = Get_StrQuery_Data(strQuey);

            return dt;
        }

        public object Check_NlpFormat(TransUser_Ety tu_ety)
        {
            object obj = new object();
            string strQuey = string.Empty;
            strQuey = "SELECT count(f.Format_Id) FROM Etrs_Formats_Tbl f INNER JOIN Etrs_FdFormatDetails_Tbl fd "
                   + " ON f.Format_Id =fd.Format_Id WHERE f.Parent_Id  =27 AND fd.Field_Id =432 AND f.Format_Id =" + tu_ety.FormatId;
            if (strQuey != string.Empty)
                obj = Get_GWTME_StrQuery_Object(strQuey);

            return obj;
        }

        public object Check_PartyWiseNlp(TransUser_Ety tu_ety)
        {
            object obj = new object();

            string strQuey = "select 1 from Etrs_NLPSchemeAccountLink_Tbl a "
            + " inner join etrs_NlpSchemeMaster_Tbl s on s.NlpScheme_Id =a.SchemeId "
            + " where s.Discontinue='N' AND convert(date,getdate()) BETWEEN convert(date,s.SchemeStart_Date) AND convert(date,s.SchemeEnd_Date) "
            + " and  a.AccountId = " + tu_ety.AccId + " ";
            obj = Get_GWTME_StrQuery_Object(strQuey);
            return obj;
        }

        public object Check_NlpFailure(TransUser_Ety tu_ety)
        {
            object obj = new object();
            //etrs_CheckNlpFailure_Proc(@DocId BigInt,@AllowMgmtNlpRate varchar(1))
            cn2.Open();
            string strProc = string.Empty;
            try
            {
                SqlParameter pDocId = new SqlParameter("@DocId", SqlDbType.BigInt);
                pDocId.Value = tu_ety.DocId;
                SqlParameter pAllowMgmtNlpRate = new SqlParameter("@AllowMgmtNlpRate", SqlDbType.VarChar, 1);
                pAllowMgmtNlpRate.Value = tu_ety.TransMode;

                SqlParameter[] paramFields = { pDocId, pAllowMgmtNlpRate };
                strProc = "etrs_CheckNlpFailure_Proc";
                if (strProc != string.Empty)
                {
                    obj = SqlHelper.ExecuteScalar(cn2, CommandType.StoredProcedure, strProc, paramFields);
                }
            }
            catch (Exception msg)
            {
                cn2.Close();
                if (res == null)
                    res = msg.Message;
            }
            finally
            {
                if (cn2.State == ConnectionState.Open)
                    cn2.Close();
                if (cn2 != null) cn2.Dispose();
            }
            return obj;
        }

        public object Get_AccLedgerType(TransUser_Ety tu_ety)
        {
            object obj = new object();
            string strQuey = string.Empty;
            strQuey = "SELECT ACC_LEDGERTYPE FROM Etrs_AccMast_Tbl WHERE ACC_ID = " + tu_ety.AccId;
            if (strQuey != string.Empty)
                obj = Get_GWTME_StrQuery_Object(strQuey);
            return obj;
        }

        public object Get_Trans_SumOutStandDays(TransUser_Ety tu_ety, decimal lowercrdlmt)
        {
            object obj = new object();
            string strQuey = string.Empty;
            if (tu_ety.TransMode == "acc")
            {
                strQuey = "SELECT isnull(max(datediff(dd, docdt, getdate())),0) OutStandDays FROM Etrs_OutStnd_tbl " +
                "WHERE OTsign='D' and OTSTNDAMT > " + lowercrdlmt + "   and ACCID=" + tu_ety.AccId + "";
            }
            else if (tu_ety.TransMode == "cost")
            {
                strQuey = "SELECT isnull(max(datediff(dd, docdt, getdate())),0) OutStandDays FROM Etrs_OutStnd_tbl " +
                "WHERE OTsign='D' and OTSTNDAMT > " + lowercrdlmt + " and ACCID=" + tu_ety.AccId + "" +
                " and deptid=" + tu_ety.DocId + " ";
            }
            if (strQuey != string.Empty)
                obj = Get_GWTME_StrQuery_Object(strQuey);

            return obj;
        }
        public object Get_Trans_SumOutStandAmount(TransUser_Ety tu_ety)
        {
            object obj = new object();
            string strQuey = string.Empty;

            if (tu_ety.TransMode == "acc")
            {
                strQuey = "select isnull(sum( case when (OTSIGN ='C')then -1* OTSTNDAMT else OTSTNDAMT end),0) OutStandAmt FROM Etrs_OutStnd_tbl " +
                 "WHERE OTSTNDAMT > 0  and ACCID=" + tu_ety.AccId + "";
            }
            if (tu_ety.TransMode == "cost")
            {
                strQuey = "select isnull(sum( case when (OTSIGN ='C')then -1* OTSTNDAMT else OTSTNDAMT end),0) OutStandAmt FROM Etrs_OutStnd_tbl " +
                "WHERE OTSTNDAMT > 0 and ACCID=" + tu_ety.AccId + "" +
                " and deptid=" + tu_ety.DocId + " ";
            }

            if (strQuey != string.Empty)
                obj = Get_GWTME_StrQuery_Object(strQuey);

            return obj;
        }

        public object Get_Trans_DocAmount(TransUser_Ety tu_ety)
        {
            object obj = new object();
            string strQuey = string.Empty;
            strQuey = "Select isnull((SELECT isnull(docamount,0) FROM ETRS_TRANSHDR_MAST_TBL where docid =" + tu_ety.DocId + "),0)";

            if (strQuey != string.Empty)
                obj = Get_GWTME_StrQuery_Object(strQuey);

            return obj;
        }

        public DataTable Check_Trans_Stock(TransUser_Ety tu_ety)
        {
            DataTable dt = new DataTable();
            string strQuey = string.Empty;
            if (tu_ety.TransMode == "Mast")
            {
                strQuey = "SELECT sum(CASE  td.TYPESGN WHEN 'C' THEN Qty ELSE -Qty End)transstock, " +
                 " itemid,WARE_HOUSE_ID , td.color_id ,docid,p.Item_Desc ,w.Wh_Name,c.Color_Desc, " +
                 " (SELECT isnull((opng_Balqty+rec_qty-Iss_qty),0) FROM Etrs_ProdStock_Tbl " +
                 " WHERE Item_Id =td.ITEMID  AND Wh_Id =td.WARE_HOUSE_ID AND color_id=td.COLOR_ID AND " +
                 " YearId =(SELECT year_id FROM  ETRS_TRANSHDR_Mast_TBL WHERE DOCID =td.docid)) stock " +
                 " FROM  Etrs_TRANSDTL1_Tbl td  " +
                 " INNER JOIN Etrs_ProdMast_Tbl p  ON td.ITEMID=p.Item_Id " +
                 " INNER JOIN ETRS_WareHouseMast_Tbl w  ON w.Wh_Id=td.WARE_HOUSE_ID" +
                 " INNER JOIN ETRS_ColorMast_Tbl  c ON c.Color_Id =td.COLOR_ID  " +
                 " WHERE docid=" + tu_ety.DocId + "  GROUP BY itemid, WARE_HOUSE_ID,p.Item_Desc ," +
                 " w.Wh_Name,c.Color_Desc,td.color_id,docid";
            }
            else if (tu_ety.TransMode == "Auth")
            {
                strQuey = "SELECT sum(CASE  td.TYPESGN WHEN 'C' THEN Qty ELSE -Qty End)transstock, " +
                 " itemid,WARE_HOUSE_ID , td.color_id ,docid,p.Item_Desc ,w.Wh_Name,c.Color_Desc, " +
                 " (SELECT isnull((opng_Balqty+rec_qty-Iss_qty),0) FROM Etrs_ProdStock_Tbl " +
                 " WHERE Item_Id =td.ITEMID  AND Wh_Id =td.WARE_HOUSE_ID AND color_id=td.COLOR_ID AND " +
                 " YearId =(SELECT year_id FROM  ETRS_TRANSHDR_AUTHOR_TBL WHERE DOCID =td.docid)) stock " +
                 " FROM  Etrs_TRANSDTL1_author_Tbl td  " +
                 " INNER JOIN Etrs_ProdMast_Tbl p  ON td.ITEMID=p.Item_Id " +
                 " INNER JOIN ETRS_WareHouseMast_Tbl w  ON w.Wh_Id=td.WARE_HOUSE_ID" +
                 " INNER JOIN ETRS_ColorMast_Tbl  c ON c.Color_Id =td.COLOR_ID  " +
                 " WHERE docid=" + tu_ety.DocId + "  GROUP BY itemid, WARE_HOUSE_ID,p.Item_Desc ," +
                 " w.Wh_Name,c.Color_Desc,td.color_id,docid";
            }

            if (strQuey != string.Empty)
                dt = Get_GWTME_StrQuery_Data(strQuey);

            return dt;
        }

        public DataTable Get_Trans_Contacts(TransUser_Ety tu_ety)
        {
            DataTable dt = new DataTable();
            cn1.Open();
            string strProc = string.Empty;
            try
            {
                SqlParameter _docid = new SqlParameter("@docid", SqlDbType.BigInt);
                _docid.Value = tu_ety.DocId;
                SqlParameter[] paramFields = { _docid };

                strProc = "CRM_GetContactEmailbsdOnAcc_proc";
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
            //SqlParameter _docid = new SqlParameter("@docid", SqlDbType.BigInt);
            //_docid.Value = docid;
            //SqlParameter[] paras = { _docid };
            //SqlDataReader dr = SqlHelper.ExecuteReader(setCon(), CommandType.StoredProcedure, "crm_getContactEmailbsdOnAcc_proc", paras);
            //if (dr.HasRows)
            //    dt.Load(dr);
            return dt;
        }

        public DataTable Get_BdfPacking_Custwizard_Details()
        {
            DataTable dt = new DataTable();
            cn1.Open();
            string strProc = string.Empty;
            try
            {
                strProc = "CRM_GetBDFCustwizData_Proc";
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

        public object Check_Trans_DocNoExist(TransUser_Ety tu_ety)
        {
            object obj = new object();
            string strQuey = string.Empty;
            if (tu_ety.TransMode == "Mast")
            {
                strQuey = "SELECT count(docid) FROM Crm_TransHdr_Mast_Tbl WHERE DOCID =" + tu_ety.DocId;
            }
            else if (tu_ety.TransMode == "Auth")
            {
                strQuey = "SELECT count(docid) FROM Crm_TransHdr_Author_Tbl WHERE DOCID =" + tu_ety.DocId;
            }

            if (strQuey != string.Empty)
                obj = Get_StrQuery_Object(strQuey);

            return obj;
        }

        public DataTable Get_Trans_Hdr_Data(TransUser_Ety tu_ety)
        {
            DataTable dt = new DataTable();
            string strProc = string.Empty;
            SqlConnection cn;
            if (tu_ety.ParentFormatId == 27)
                cn = new SqlConnection(ConfigurationManager.ConnectionStrings["GWTME_ConnStr"].ConnectionString);
            else
                cn = new SqlConnection(ConfigurationManager.ConnectionStrings["Crm_ConnStr"].ConnectionString);
            cn.Open();
            try
            {
                SqlParameter pDocId = new SqlParameter("@DocId", SqlDbType.BigInt);
                pDocId.Value = tu_ety.DocId;

                SqlParameter _Mode = new SqlParameter("@Mode", SqlDbType.VarChar);
                _Mode.Value = tu_ety.TransMode;

                SqlParameter[] paramFields = { pDocId, _Mode };

                if (tu_ety.ParentFormatId == 27)
                    strProc = "Etrs_Trans_Get_TransHdr_Proc";
                else
                    strProc = "CRM_Trans_Get_TransHdr_Proc";

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
                if (res == null)
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

        public object Get_Trans_User_WH_Rights(TransUser_Ety tu_ety)
        {
            object obj = new object();
            string strQuey = string.Empty;
            if (tu_ety.ParentFormatId == 38)
            {
                strQuey = " SELECT count(Docid) FROM ETRS_TRANSDTL1_AUTHOR_Tbl t INNER JOIN Etrs_UsrRights_Tbl u ON " +
                " t.FROMWAREHOUSE = u.Security_Id WHERE t.FROMWAREHOUSE<>0 " +
                " AND t.DOCID =" + tu_ety.DocId + " AND u.EnableP='Y' AND u.Usr_Id = " + tu_ety.UserId + " AND u.FinYear_Id= " + tu_ety.FinId + "";
            }
            else
            {
                strQuey = " SELECT count(Docid) FROM ETRS_TRANSDTL1_AUTHOR_Tbl t INNER JOIN Etrs_UsrRights_Tbl u ON " +
                " t.WARE_HOUSE_ID = u.Security_Id WHERE t.WARE_HOUSE_ID<>0 " +
                " AND t.DOCID =" + tu_ety.DocId + " AND u.EnableP='Y' AND u.Usr_Id = " + tu_ety.UserId + " AND u.FinYear_Id= " + tu_ety.FinId + "";
            }

            if (strQuey != string.Empty)
                obj = Get_GWTME_StrQuery_Object(strQuey);

            return obj;
        }

        public DataTable Get_User_Childs(TransUser_Ety tu_ety)
        {
            DataTable dt = new DataTable();
            string strQuey = "SELECT * FROM [dbo].[GetUserChild] (" + Convert.ToInt16(tu_ety.UserId) + ")";
            if (strQuey != string.Empty)
                dt = Get_GWTME_StrQuery_Data(strQuey);
            return dt;
        }

        public string Save_Trans_Data(string hdocid, string strslno, string hdrstatus, string dtlstatus, string flag, string finflag, string stkflag,
                                      string vLimitFlag, string vLimitMsg, Hashtable htGrHdr, Hashtable htGrDtl, string NLPAuth, TransUser_Ety tu_ety)
        {
            SqlConnection cn; //= new SqlConnection();
            if (tu_ety.ParentFormatId == 27)
                cn = new SqlConnection(ConfigurationManager.ConnectionStrings["GWTME_ConnStr"].ConnectionString);
            else
                cn = new SqlConnection(ConfigurationManager.ConnectionStrings["Crm_ConnStr"].ConnectionString);


            cn.Open();
            SqlTransaction st = cn.BeginTransaction();
            string restransauth = string.Empty;
            try
            {
                SqlParameter[] transauth = new SqlParameter[11];
                transauth[0] = new SqlParameter("@DocId1", SqlDbType.VarChar, 500);
                transauth[1] = new SqlParameter("@HdrStatus1", SqlDbType.VarChar, 500);
                transauth[2] = new SqlParameter("@DtlTblFlg1 ", SqlDbType.VarChar, 500);
                transauth[3] = new SqlParameter("@Format_Id ", SqlDbType.VarChar, 50);
                transauth[4] = new SqlParameter("@CmpId", SqlDbType.SmallInt);
                transauth[5] = new SqlParameter("@Yearid", SqlDbType.SmallInt);
                transauth[6] = new SqlParameter("@SecurityId ", SqlDbType.SmallInt);
                transauth[7] = new SqlParameter("@UserId", SqlDbType.SmallInt);
                transauth[8] = new SqlParameter("@Result", SqlDbType.VarChar, 200);
                transauth[9] = new SqlParameter("@ExecStatus", SqlDbType.TinyInt);
                transauth[10] = new SqlParameter("@ChngUserId", SqlDbType.SmallInt);//31 dec 2009

                transauth[0].Value = hdocid;
                transauth[1].Value = hdrstatus;
                transauth[2].Value = flag;
                transauth[3].Value = tu_ety.FormatId;
                transauth[4].Value = tu_ety.CompId;
                transauth[5].Value = tu_ety.FinId;
                transauth[6].Value = 503;
                transauth[7].Value = tu_ety.UserId;
                transauth[8].Value = "";
                transauth[8].Direction = ParameterDirection.Output;
                transauth[9].Value = 0;
                transauth[9].Direction = ParameterDirection.Output;
                transauth[10].Value = tu_ety.UserId;
                if (tu_ety.ParentFormatId == 27)
                    SqlHelper.ExecuteNonQuery(st, CommandType.StoredProcedure, "etrsCrm_TransAuth_Proc", transauth);
                else
                    SqlHelper.ExecuteNonQuery(st, CommandType.StoredProcedure, "Crm_TransAuth_Proc", transauth);
                restransauth = transauth[8].Value.ToString() + ";  ";
                if (Convert.ToInt16(transauth[9].Value) == 0)
                {
                    st.Rollback();
                    cn.Close();
                    return restransauth;
                }

                if (vLimitFlag == "Y" && (tu_ety.CompId == 8 || tu_ety.CompId == 7 || tu_ety.CompId == 11))
                {
                    SqlParameter[] transSendMail = new SqlParameter[6];
                    transSendMail[0] = new SqlParameter("@Accid", SqlDbType.SmallInt);
                    transSendMail[1] = new SqlParameter("@DeptId", SqlDbType.SmallInt);
                    transSendMail[2] = new SqlParameter("@Docid", SqlDbType.BigInt);
                    transSendMail[3] = new SqlParameter("@Reason", SqlDbType.VarChar, 1000);
                    transSendMail[4] = new SqlParameter("@OutErrMsg", SqlDbType.VarChar, 200);
                    transSendMail[5] = new SqlParameter("@ExecStatus", SqlDbType.TinyInt);
                    transSendMail[4].Direction = ParameterDirection.Output;
                    transSendMail[5].Direction = ParameterDirection.Output;

                    transSendMail[0].Value = Convert.ToInt16(tu_ety.AccId);
                    transSendMail[1].Value = tu_ety.DeptId;
                    transSendMail[2].Value = Convert.ToInt64(hdocid.Replace("~", ""));
                    transSendMail[3].Value = vLimitMsg;
                    transSendMail[4].Value = "result";
                    transSendMail[5].Value = 0;

                    int rsno = SqlHelper.ExecuteNonQuery(st, CommandType.StoredProcedure, "Etrs_SendCreditLimitMastMail_Proc", transSendMail);

                    if (Convert.ToInt16(transSendMail[5].Value) == 0)
                    {
                        restransauth = transSendMail[4].Value.ToString();
                        st.Rollback();
                        cn.Close();
                        return restransauth;
                    }
                }
                if (tu_ety.ParentFormatId == 27)
                {
                    if ((tu_ety.CompId == 8 && tu_ety.AccId == 18046) || (tu_ety.CompId == 7 && tu_ety.AccId == 8229))
                    {
                        SqlParameter pSchChkEmail = new SqlParameter("@Indocid", SqlDbType.BigInt);
                        pSchChkEmail.Value = Convert.ToInt64(hdocid.Replace("~", ""));
                        SqlHelper.ExecuteNonQuery(st, CommandType.StoredProcedure, "Etrs_EmailScehemeCheckSalesOrderMast_Proc", pSchChkEmail);
                    }
                }
                if (NLPAuth == "Y")
                {

                    SqlParameter[] transWhSendMail = new SqlParameter[1];
                    transWhSendMail[0] = new SqlParameter("@Indocid", SqlDbType.BigInt);
                    transWhSendMail[0].Value = Convert.ToInt64(hdocid.Replace("~", ""));
                    int rsno = SqlHelper.ExecuteNonQuery(st, CommandType.StoredProcedure, "Etrs_EmailNlpSalesOrderMast_Proc", transWhSendMail);
                }
                st.Commit();
            }

            catch (Exception ex)
            {
                st.Rollback();
                return ex.Message;
            }
            if (tu_ety.ParentFormatId == 27)
            {
                try
                {                //for checking credit and debit amounts if docid.IF not matched both then inserting that docid into temp table*/
                    SqlParameter[] chkoutparam = new SqlParameter[2];
                    chkoutparam[0] = new SqlParameter("@InDocId", SqlDbType.BigInt);
                    chkoutparam[1] = new SqlParameter("@InYearId", SqlDbType.SmallInt);
                    chkoutparam[0].Value = Convert.ToInt64(hdocid.Replace("~", ""));
                    chkoutparam[1].Value = tu_ety.FinId;
                    int j = SqlHelper.ExecuteNonQuery(cn, CommandType.StoredProcedure, "Etrs_CheckOutStndAmt_Proc", chkoutparam);
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
                finally
                {
                    cn.Close();
                }
            }
            return restransauth;
        }
        public string Detlete_Trans_Data(TransUser_Ety tu_ety)
        {
            SqlConnection cn; //= new SqlConnection();
            if (tu_ety.ParentFormatId == 27)
                cn = new SqlConnection(ConfigurationManager.ConnectionStrings["GWTME_ConnStr"].ConnectionString);
            else
                cn = new SqlConnection(ConfigurationManager.ConnectionStrings["Crm_ConnStr"].ConnectionString);

            cn.Open();
            SqlTransaction st = cn.BeginTransaction();
            string restransauth = string.Empty;
            try
            {
                SqlParameter[] ProductParams = new SqlParameter[9];
                ProductParams[0] = new SqlParameter("@InDocId1", SqlDbType.BigInt);
                ProductParams[1] = new SqlParameter("@InFormat_Id1", SqlDbType.SmallInt);
                ProductParams[2] = new SqlParameter("@InYearId", SqlDbType.SmallInt);
                ProductParams[3] = new SqlParameter("@InCompId", SqlDbType.SmallInt);
                ProductParams[4] = new SqlParameter("@InUsrId", SqlDbType.SmallInt);
                ProductParams[5] = new SqlParameter("@InMode", SqlDbType.VarChar);
                ProductParams[6] = new SqlParameter("@Result", SqlDbType.VarChar, 200);
                ProductParams[7] = new SqlParameter("@ExecStatus", SqlDbType.Int);
                ProductParams[8] = new SqlParameter("@InChngUsrId", SqlDbType.SmallInt);//added to check with change over user 29 dec 2009
                ProductParams[7].Direction = ParameterDirection.Output;
                ProductParams[6].Direction = ParameterDirection.Output;

                ProductParams[0].Value = tu_ety.DocId;
                ProductParams[1].Value = tu_ety.FormatId;
                ProductParams[2].Value = tu_ety.FinId;
                ProductParams[3].Value = tu_ety.CompId;
                ProductParams[4].Value = tu_ety.UserId;
                ProductParams[5].Value = tu_ety.TransMode;
                ProductParams[6].Value = "";
                ProductParams[7].Value = 0;
                ProductParams[8].Value = tu_ety.UserId;

                if (tu_ety.ParentFormatId == 27)
                    SqlHelper.ExecuteNonQuery(st, CommandType.StoredProcedure, "Etrs_TransDelete_Proc", ProductParams);
                else
                    SqlHelper.ExecuteNonQuery(st, CommandType.StoredProcedure, "CRM_TransDelete_Proc", ProductParams);

                if (Convert.ToInt16(ProductParams[7].Value) == 0)
                {
                    st.Rollback();
                    cn.Close();
                    return ProductParams[6].Value.ToString();
                }
                restransauth = ProductParams[6].Value.ToString();
                st.Commit();
                return restransauth;
            }
            catch (Exception ex)
            {
                // objLog.writeLog(HttpContext.Current.Session["UserName"].ToString(), "PSI transaction Delete", "Detete Error", HttpContext.Current.Session.SessionID, ex.Message);
                st.Rollback();
                cn.Close();
                return ex.Message.Replace("'", " ");
            }
            finally
            {
                cn.Close();
            }
        }
        public DataTable CRM_Get_Sales_AnalsisData(TransUser_Ety tu_ety)
        {
            //@UserId SMALLINT,@UserType VARCHAR(20),@CompanyType varchar(10)
            DataTable dt = new DataTable();

            SqlParameter _FromDt, _ToDt, _UserId, _UserType, _CompanyType;
            _FromDt = new SqlParameter("@FromDt", SqlDbType.SmallDateTime);
            _FromDt.Value = tu_ety.FromDt;

            _ToDt = new SqlParameter("@ToDt", SqlDbType.SmallDateTime);
            _ToDt.Value = tu_ety.ToDt;

            _UserId = new SqlParameter("@UserId", SqlDbType.BigInt);
            _UserId.Value = tu_ety.UserId;

            _UserType = new SqlParameter("@UserType", SqlDbType.VarChar);
            _UserType.Value = tu_ety.UserType;

            _CompanyType = new SqlParameter("@CompanyType", SqlDbType.VarChar);
            _CompanyType.Value = tu_ety.TransMode;

            SqlParameter[] paramFields = { _FromDt, _ToDt, _UserId, _UserType, _CompanyType };

            //CRM_Trans_Get_Transactions_proc(@FromDt Date,@ToDt Date,@Mode varchar(50), @FormatId smallint)
            cn1.Open();
            string strProc = string.Empty;
            try
            {
                strProc = "Crm_Get_SalesAnalysisData_Proc";
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


        internal DataTable Get_StrQuery_Data(string str)
        {
            DataTable dt = new DataTable();

            cn1.Open();
            string strProc = string.Empty;
            try
            {
                //SqlParameter usr_Id, fin_Id, comp_Id;
                //usr_Id = new SqlParameter("@UserId", SqlDbType.SmallInt);
                //usr_Id.Value = tu_ety.UserId;
                //fin_Id = new SqlParameter("@FinId", SqlDbType.SmallInt);
                //fin_Id.Value = tu_ety.FinId;
                //comp_Id = new SqlParameter("@CompId", SqlDbType.SmallInt);
                //comp_Id.Value = tu_ety.CompId;
                //SqlParameter[] paramFields = { usr_Id, fin_Id, comp_Id };

                strProc = "CRM_Trans_Get_Formats_Proc";
                if (strProc != string.Empty)
                {
                    SqlDataReader sdr = SqlHelper.ExecuteReader(cn1, CommandType.Text, str);
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

        internal object Get_StrQuery_Object(string str)
        {
            object obj = new object();
            cn1.Open();
            try
            {
                obj = SqlHelper.ExecuteScalar(cn1, CommandType.Text, str);
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
            return obj;
        }

        internal DataTable Get_GWTME_StrQuery_Data(string str)
        {
            DataTable dt = new DataTable();

            cn2.Open();
            string strProc = string.Empty;
            try
            {
                //SqlParameter usr_Id, fin_Id, comp_Id;
                //usr_Id = new SqlParameter("@UserId", SqlDbType.SmallInt);
                //usr_Id.Value = tu_ety.UserId;
                //fin_Id = new SqlParameter("@FinId", SqlDbType.SmallInt);
                //fin_Id.Value = tu_ety.FinId;
                //comp_Id = new SqlParameter("@CompId", SqlDbType.SmallInt);
                //comp_Id.Value = tu_ety.CompId;
                //SqlParameter[] paramFields = { usr_Id, fin_Id, comp_Id };

                strProc = "CRM_Trans_Get_Formats_Proc";
                if (strProc != string.Empty)
                {
                    SqlDataReader sdr = SqlHelper.ExecuteReader(cn2, CommandType.Text, str);
                    dt.Load(sdr);
                    if (!sdr.IsClosed)
                        sdr.Close();
                }
            }
            catch (Exception msg)
            {
                cn2.Close();
                if (res == null)
                    res = msg.Message;
            }
            finally
            {
                if (cn2.State == ConnectionState.Open)
                    cn2.Close();
                if (cn2 != null) cn1.Dispose();
            }

            return dt;
        }

        internal object Get_GWTME_StrQuery_Object(string str)
        {
            object obj = new object();
            cn2.Open();
            try
            {
                obj = SqlHelper.ExecuteScalar(cn2, CommandType.Text, str);
            }
            catch (Exception msg)
            {
                cn2.Close();
                if (res == null)
                    res = msg.Message;
            }
            finally
            {
                if (cn2.State == ConnectionState.Open)
                    cn2.Close();
                if (cn2 != null) cn2.Dispose();
            }
            return obj;
        }
    }
}
