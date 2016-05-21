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
    public class Trans_Dtls_DAC : IDisposable
    {
        // Pointer to an external unmanaged resource.
        private IntPtr handle;
        // Track whether Dispose has been called.
        private bool disposed = false;

        //EntityConnection cn1;
        SqlConnection cn, cn1, cn2;
        string res;
        int n;
        public Trans_Dtls_DAC()
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
        public Trans_Dtls_DAC(IntPtr handle)
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

        ~Trans_Dtls_DAC()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            Dispose(false);
        }

        public DataTable Get_TransRefDocNum(TransUser_Ety tu_ety)
        {
            string strQuey = "SELECT replace(CONVERT(varchar,h.docdt,106),' ','-') AS docdt," +
                " H.docseries+''+convert(varchar,isnull(H.docalpha,''))+''+convert(varchar,H.docno)  docno " +
                " FROM CRM_TRANSHDR_MAST_TBL h WHERE DOCID IN(" + tu_ety.TransMode + ") ORDER BY docdt";

            using (Trans_Main_DAC dac = new Trans_Main_DAC())
            {
                return dac.Get_StrQuery_Data(strQuey);
            }
        }
        public DataTable Get_DefaultWareHouse(TransUser_Ety tu_ety)
        {
            //CRM_BDF_Get_TransUser_Rights_Proc (@UserId BIGINT, @FormatId SMALLINT, @FinId SMALLINT, @Flag VARCHAR (10)
            DataTable dt = new DataTable();
            cn2.Open();
            string strProc = string.Empty;
            try
            {
                SqlParameter _UserId = new SqlParameter("@UsrId", SqlDbType.BigInt);
                _UserId.Value = tu_ety.UserId;

                SqlParameter _FinId = new SqlParameter("@finId", SqlDbType.BigInt);
                _FinId.Value = tu_ety.FinId;

                SqlParameter[] paramFields = { _UserId, _FinId };

                strProc = "Etrs_getDefaultWareHouse_proc";
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

        public DataTable LoadControls(TransUser_Ety tu_ety)
        {
            DataTable dt = new DataTable();
            cn2.Open();
            string strProc = string.Empty;
            try
            {
                SqlParameter _FormatId = new SqlParameter("@FormatId", SqlDbType.BigInt);
                _FormatId.Value = tu_ety.FormatId;
                SqlParameter[] paramFields = { _FormatId };

                strProc = "Etrs_LoadControls_Proc";
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

        public object Check_ProdDiscFlag(TransUser_Ety tu_ety)
        {
            string strQuey = "SELECT Discontinued FROM Etrs_ProdMast_Tbl WHERE Item_Id =" + tu_ety.DeptId;
            using (Trans_Main_DAC dac = new Trans_Main_DAC())
            {
                return dac.Get_GWTME_StrQuery_Object(strQuey);
            }
        }

        public DataTable Get_DocWise_ProdStock(TransUser_Ety tu_ety)
        {
            string strQuey = "SELECT ITEMID,convert(INT,sum(qty))Qty,WARE_HOUSE_ID WhId,COLOR_ID ColorId ,docid "
               + " FROM CRM_TRANSDTL1_Tbl "
               + " WHERE docid=" + tu_ety.DocId + " group BY ITEMID,WARE_HOUSE_ID,COLOR_ID,Docid ";
            using (Trans_Main_DAC dac = new Trans_Main_DAC())
            {
                return dac.Get_StrQuery_Data(strQuey);
            }
        }

        public object CRM_Trans_Check_DocNo(TransUser_Ety tu_ety, string DocSeries, string DocNo, string DocAlfa)
        {
            object obj = new object();
            //CRM_Trans_Check_DocNo_Proc(@DocId BIGINT,@DocSeries VARCHAR(50),@DocNo VARCHAR(6),@DocAlfa VARCHAR(2),@AuthPA VARCHAR(2),@FinId SMALLINT)
            cn1.Open();
            string strProc = string.Empty;
            try
            {
                SqlParameter pDocId = new SqlParameter("@DocId", SqlDbType.BigInt);
                pDocId.Value = tu_ety.DocId;

                SqlParameter pFinId = new SqlParameter("@FinId", SqlDbType.SmallInt);
                pFinId.Value = tu_ety.FinId;

                SqlParameter pMode = new SqlParameter("@Mode", SqlDbType.VarChar);
                pMode.Value = tu_ety.TransMode;

                SqlParameter pDocSeries = new SqlParameter("@DocSeries", SqlDbType.VarChar);
                pDocSeries.Value = DocSeries;

                SqlParameter pDocNo = new SqlParameter("@DocNo", SqlDbType.VarChar);
                pDocNo.Value = DocNo;

                SqlParameter pDocAlfa = new SqlParameter("@DocAlfa", SqlDbType.VarChar);
                pDocAlfa.Value = DocNo;

                SqlParameter[] paramFields = { pDocId, pFinId, pMode, pDocSeries, pDocNo, pDocAlfa };
                strProc = "CRM_Trans_Check_DocNo_Proc";
                if (strProc != string.Empty)
                {
                    obj = SqlHelper.ExecuteScalar(cn1, CommandType.StoredProcedure, strProc, paramFields);
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
            return obj;
        }

        public DataTable CRM_Trans_Get_DocSeries(TransUser_Ety tu_ety)
        {
            DataTable dt = new DataTable();
            cn1.Open();
            string strProc = string.Empty;
            try
            {
                SqlParameter pDocId = new SqlParameter("@DocId", SqlDbType.BigInt);
                pDocId.Value = tu_ety.DocId;

                SqlParameter pMode = new SqlParameter("@Mode", SqlDbType.VarChar);
                pMode.Value = tu_ety.TransMode;

                SqlParameter[] paramFields = { pDocId, pMode };
                strProc = "CRM_Trans_Get_DocSeries";
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

        public object CRM_Trans_Get_MaxDocAlpha(TransUser_Ety tu_ety)
        {
            object obj = new object();
            //CRM_Trans_Check_DocNo_Proc(@DocId BIGINT,@DocSeries VARCHAR(50),@DocNo VARCHAR(6),@DocAlfa VARCHAR(2),@AuthPA VARCHAR(2),@FinId SMALLINT)
            cn1.Open();
            string strProc = string.Empty;
            try
            {
                SqlParameter pDocId = new SqlParameter("@docid", SqlDbType.BigInt);
                pDocId.Value = tu_ety.DocId;

                SqlParameter pMode = new SqlParameter("@Mode", SqlDbType.VarChar);
                pMode.Value = tu_ety.TransMode;

                SqlParameter[] paramFields = { pDocId, pMode };
                strProc = "crm_getDocalpha_proc";
                if (strProc != string.Empty)
                {
                    obj = SqlHelper.ExecuteScalar(cn1, CommandType.StoredProcedure, strProc, paramFields);
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
            return obj;
        }

        public object Get_MaxDocNo(TransUser_Ety tu_ety, string docseries)
        {
            //string strmaxno = "select dbo.GETMAXDOCNO('" + yearid + "','" + docseries + "','" + formatid + "','" + rbncheck + "')";
            //if (ParantFormat == 27)
            //    return SqlHelper.ExecuteScalar(HttpContext.Current.Session["GWTMEConStr"].ToString(), CommandType.Text, strmaxno);
            //else
            //    return SqlHelper.ExecuteScalar(setCon(), CommandType.Text, strmaxno);

            object obj = new object();
            //CRM_Trans_Check_DocNo_Proc(@DocId BIGINT,@DocSeries VARCHAR(50),@DocNo VARCHAR(6),@DocAlfa VARCHAR(2),@AuthPA VARCHAR(2),@FinId SMALLINT)

            SqlConnection cn; //= new SqlConnection();
            if (tu_ety.ParentFormatId == 27)
                cn = new SqlConnection(ConfigurationManager.ConnectionStrings["GWTME_ConnStr"].ConnectionString);
            else
                cn = new SqlConnection(ConfigurationManager.ConnectionStrings["Crm_ConnStr"].ConnectionString);
            cn.Open();
            string strProc = string.Empty;
            try
            {
                SqlParameter pFormatId = new SqlParameter("@FormatId", SqlDbType.SmallInt);
                pFormatId.Value = tu_ety.FormatId;

                SqlParameter pFinId = new SqlParameter("@FinId", SqlDbType.VarChar);
                pFinId.Value = tu_ety.FinId;

                SqlParameter pMode = new SqlParameter("@Mode", SqlDbType.VarChar);
                pMode.Value = tu_ety.TransMode;

                SqlParameter pDocSeries = new SqlParameter("@DocSeries", SqlDbType.VarChar);
                pDocSeries.Value = docseries;


                SqlParameter[] paramFields = { pFormatId, pFinId, pMode, pDocSeries };

                if (tu_ety.ParentFormatId == 27)
                    strProc = "etrs_Trans_Get_MaxDocNo_Proc";
                else
                    strProc = "CRM_Trans_Get_MaxDocNo_Proc";
                if (strProc != string.Empty)
                {
                    obj = SqlHelper.ExecuteScalar(cn, CommandType.StoredProcedure, strProc, paramFields);
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
                if (cn != null) cn1.Dispose();
            }
            return obj;
        }

        public object CRM_Get_Prod_WhWise_Color_Qty(TransUser_Ety tu_ety, TransProd_Ety tp_ety)
        {
            object obj = new object();
            //CRM_Get_Prod_WhWise_Color_Qty_Proc(@ItemId BIGINT ,@ColorId SMALLINT ,@WhId SMALLINT,@FormatId SMALLINT	,@FinId SMALLINT,@UserType VARCHAR(50),@UserId BIGINT)
            cn1.Open();
            string strProc = string.Empty;
            try
            {
                SqlParameter pItemId, pColorId, pWhId, pUserId, pFormatId, pFinId, pUserType;

                pItemId = new SqlParameter("@ItemId", SqlDbType.BigInt);
                pItemId.Value = tp_ety.ItemId;

                pColorId = new SqlParameter("@ColorId", SqlDbType.SmallInt);
                pColorId.Value = tp_ety.ColorId;

                pWhId = new SqlParameter("@WhId", SqlDbType.SmallInt);
                pWhId.Value = tp_ety.WhId;

                pUserId = new SqlParameter("@UserId", SqlDbType.BigInt);
                pUserId.Value = tu_ety.UserId;

                pFormatId = new SqlParameter("@FormatId", SqlDbType.SmallInt);
                pFormatId.Value = tu_ety.FormatId;

                pFinId = new SqlParameter("@FinId", SqlDbType.SmallInt);
                pFinId.Value = tu_ety.FinId;

                pUserType = new SqlParameter("@UserType", SqlDbType.VarChar);
                pUserType.Value = tu_ety.UserType;

                SqlParameter[] paramFields = { pItemId, pColorId, pWhId, pUserId, pFormatId, pFinId, pUserType };

                strProc = "CRM_Get_Prod_WhWise_Color_Qty_Proc";
                if (strProc != string.Empty)
                {
                    obj = SqlHelper.ExecuteScalar(cn1, CommandType.StoredProcedure, strProc, paramFields);
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
            return obj;
        }

        public string[] SaveHeader(TransProdValues_Ety psipv, string mode, string userType,
         string vLimitFlag, string vLimitMsg, Hashtable htNlpDtl, string NlpMail, TransUser_Ety tu_ety)
        {

            //string hdocid, string strslno, string hdrstatus, string dtlstatus, string flag, string finflag, string stkflag,
            //string vLimitFlag, string vLimitMsg, Hashtable htGrHdr, Hashtable htGrDtl, string NLPAuth, TransUser_Ety tu_ety
            SqlConnection cn; //= new SqlConnection();
            if (tu_ety.ParentFormatId == 27)
                cn = new SqlConnection(ConfigurationManager.ConnectionStrings["GWTME_ConnStr"].ConnectionString);
            else
                cn = new SqlConnection(ConfigurationManager.ConnectionStrings["Crm_ConnStr"].ConnectionString);

            cn.Open();
            SqlTransaction st = cn.BeginTransaction();

            string[] result = new string[2];
            string res = string.Empty;
            SqlParameter[] ProductParams = new SqlParameter[32];
            try
            {
                char FlgAdjStk = 'N';
                if (mode == "MODIFY" && tu_ety.ParentFormatId == 27)
                {
                    string strFlgAdjStk = string.Empty;
                    object objFlgAdj;
                    if (tu_ety.ParentFormatId == 27)
                    {
                        strFlgAdjStk = "SELECT FLG_ADJ_STK FROM ETRS_TRANSHDR_MAST_TBL WHERE DOCID=" + Convert.ToInt64(psipv.DocId) + "";
                        objFlgAdj = SqlHelper.ExecuteScalar(st, CommandType.Text, strFlgAdjStk);
                    }
                    else
                    {
                        strFlgAdjStk = "SELECT FLG_ADJ_STK FROM CRM_TRANSHDR_MAST_TBL WHERE DOCID=" + Convert.ToInt64(psipv.DocId) + "";
                        objFlgAdj = SqlHelper.ExecuteScalar(st, CommandType.Text, strFlgAdjStk);
                    }
                    FlgAdjStk = Convert.ToChar(objFlgAdj);
                }

                ProductParams[0] = new SqlParameter("@InDocId1", SqlDbType.BigInt);
                ProductParams[1] = new SqlParameter("@InYearId1", SqlDbType.SmallInt);
                ProductParams[2] = new SqlParameter("@InFormat_Id1", SqlDbType.SmallInt);
                ProductParams[3] = new SqlParameter("@InDocType1", SqlDbType.VarChar);
                ProductParams[4] = new SqlParameter("@InDocSeries1", SqlDbType.VarChar);
                ProductParams[5] = new SqlParameter("@InDocAlpha1", SqlDbType.Char, 1);
                ProductParams[6] = new SqlParameter("@InDocNo1", SqlDbType.BigInt);
                ProductParams[7] = new SqlParameter("@InDocDt1", SqlDbType.SmallDateTime);
                ProductParams[8] = new SqlParameter("@InAccId1", SqlDbType.BigInt);
                ProductParams[9] = new SqlParameter("@InDeptId1", SqlDbType.SmallInt);
                ProductParams[10] = new SqlParameter("@InDocAmount1", SqlDbType.Decimal);
                ProductParams[11] = new SqlParameter("@InTransNo1", SqlDbType.VarChar);
                ProductParams[12] = new SqlParameter("@InTransDt1", SqlDbType.SmallDateTime);
                ProductParams[13] = new SqlParameter("@InTransOthers1", SqlDbType.VarChar);
                ProductParams[14] = new SqlParameter("@InHremarks1", SqlDbType.VarChar);
                ProductParams[15] = new SqlParameter("@InTotalGross1", SqlDbType.Decimal);
                ProductParams[16] = new SqlParameter("@InTotalTax1", SqlDbType.Decimal);
                ProductParams[17] = new SqlParameter("@InTotalDiscount1", SqlDbType.Decimal);
                ProductParams[18] = new SqlParameter("@InTotalExTaxDiscount1", SqlDbType.Decimal);
                ProductParams[19] = new SqlParameter("@InNarration_Id1", SqlDbType.BigInt);
                ProductParams[20] = new SqlParameter("@InDelivered1", SqlDbType.Char, 1);
                ProductParams[21] = new SqlParameter("@InFlgAdjFin1", SqlDbType.Char, 1);
                ProductParams[22] = new SqlParameter("@InFlgAdjStk1", SqlDbType.Char, 1);
                ProductParams[23] = new SqlParameter("@InFlg_Ref1", SqlDbType.Char, 1);
                ProductParams[24] = new SqlParameter("@InStk_Br_Id1", SqlDbType.SmallInt);
                ProductParams[25] = new SqlParameter("@InLdgId1", SqlDbType.SmallInt);
                ProductParams[26] = new SqlParameter("@InMode1", SqlDbType.VarChar);
                ProductParams[27] = new SqlParameter("@InUsrId", SqlDbType.SmallInt);
                ProductParams[28] = new SqlParameter("@InCmpId", SqlDbType.SmallInt);
                ProductParams[29] = new SqlParameter("@Result", SqlDbType.VarChar, 200);
                ProductParams[30] = new SqlParameter("@ExecStatus", SqlDbType.Int);
                ProductParams[31] = new SqlParameter("@InChngUsrId", SqlDbType.SmallInt);//31 dec 2009

                ProductParams[0].Direction = ParameterDirection.InputOutput;
                ProductParams[29].Direction = ParameterDirection.Output;
                ProductParams[30].Direction = ParameterDirection.Output;
                ProductParams[0].Value = Convert.ToInt64(psipv.DocId);
                if (mode == "MODIFY")
                    ProductParams[0].Value = Convert.ToInt64(psipv.DocId);
                ProductParams[1].Value = tu_ety.FinId;
                ProductParams[2].Value = tu_ety.FormatId;
                ProductParams[3].Value = psipv.DocType;
                ProductParams[4].Value = psipv.DocSeries;
                ProductParams[5].Value = Convert.ToChar(psipv.DocAlpha);
                ProductParams[6].Value = Convert.ToInt64(psipv.DocNo);
                ProductParams[7].Value = Convert.ToDateTime(psipv.DocDt);
                ProductParams[8].Value = Convert.ToInt64(psipv.AccId);
                ProductParams[9].Value = Convert.ToInt16(psipv.DeptId);
                ProductParams[10].Value = Convert.ToDecimal(psipv.DocAmount);
                ProductParams[11].Value = psipv.TransNo;
                ProductParams[12].Value = Convert.ToDateTime(psipv.DocDt).Date;
                ProductParams[13].Value = psipv.TransOthers;
                ProductParams[14].Value = psipv.HRemarks;
                ProductParams[15].Value = Convert.ToDecimal(psipv.TotalGrass);
                ProductParams[16].Value = Convert.ToDecimal(psipv.TotalTax);
                ProductParams[17].Value = Convert.ToDecimal(psipv.TotalDiscount);
                ProductParams[18].Value = Convert.ToDecimal(psipv.TotalExTaxDiscount);
                ProductParams[19].Value = Convert.ToInt64(psipv.NarrationId);
                ProductParams[20].Value = Convert.ToChar(psipv.Delivered);
                ProductParams[21].Value = Convert.ToChar(psipv.FlgAdjFin);
                ProductParams[22].Value = Convert.ToChar(psipv.FlgAdjStk);
                ProductParams[23].Value = Convert.ToChar(psipv.FlgRef);
                ProductParams[24].Value = Convert.ToInt16(psipv.StkBrId);
                ProductParams[25].Value = 0;
                ProductParams[26].Value = psipv.Mode;
                ProductParams[27].Value = tu_ety.UserId;
                ProductParams[28].Value = tu_ety.CompId;
                ProductParams[29].Value = "";
                ProductParams[30].Value = 0;
                ProductParams[31].Value = tu_ety.UserId;

                if (tu_ety.ParentFormatId == 27)
                    SqlHelper.ExecuteNonQuery(st, CommandType.StoredProcedure, "etrs_TransHdrSgl_ProcM", ProductParams);
                else
                    SqlHelper.ExecuteNonQuery(st, CommandType.StoredProcedure, "Crm_TransHdrSgl_ProcM", ProductParams);

                if (Convert.ToInt16(ProductParams[30].Value) == 0)
                {
                    psipv.InResult = ProductParams[29].Value.ToString().Replace("'", " ");
                    st.Rollback();
                    cn.Close();
                    result[0] = ProductParams[29].Value.ToString().Replace("'", " ");
                    result[1] = "0";
                    return result;
                }
                result[1] = "1";
                res = ProductParams[29].Value.ToString().Replace("'", " ");

                if (psipv.InMode3 != null)
                {
                    SqlParameter[] Dtl3Params = new SqlParameter[14];
                    Dtl3Params[0] = new SqlParameter("@InDocId", SqlDbType.BigInt);
                    Dtl3Params[1] = new SqlParameter("@InSlNo1", SqlDbType.VarChar);
                    Dtl3Params[2] = new SqlParameter("@InFldId1", SqlDbType.VarChar);
                    Dtl3Params[3] = new SqlParameter("@InFldValue1", SqlDbType.VarChar);
                    Dtl3Params[4] = new SqlParameter("@InFldType1", SqlDbType.VarChar);

                    Dtl3Params[5] = new SqlParameter("@FormatId", SqlDbType.SmallInt);
                    Dtl3Params[6] = new SqlParameter("@CmpId", SqlDbType.SmallInt);
                    Dtl3Params[7] = new SqlParameter("@FinYearId", SqlDbType.SmallInt);
                    Dtl3Params[8] = new SqlParameter("@UsrId", SqlDbType.SmallInt);
                    Dtl3Params[9] = new SqlParameter("@InModeD", SqlDbType.VarChar);
                    Dtl3Params[10] = new SqlParameter("@InMode1", SqlDbType.VarChar);
                    Dtl3Params[11] = new SqlParameter("@Result", SqlDbType.VarChar, 200);
                    Dtl3Params[12] = new SqlParameter("@ExecStatus", SqlDbType.Int);

                    Dtl3Params[11].Direction = ParameterDirection.Output;
                    Dtl3Params[12].Direction = ParameterDirection.Output;
                    Dtl3Params[13] = new SqlParameter("@ChngUsrId", SqlDbType.SmallInt);

                    Dtl3Params[0].Value = Convert.ToInt64(ProductParams[0].Value);
                    if (mode == "MODIFY")
                        Dtl3Params[0].Value = Convert.ToInt64(psipv.DocId);
                    Dtl3Params[1].Value = psipv.InSlNo3;
                    Dtl3Params[2].Value = psipv.InFldId3;
                    Dtl3Params[3].Value = psipv.InFldValue3;
                    Dtl3Params[4].Value = psipv.InFldType3;
                    Dtl3Params[5].Value = tu_ety.FormatId;
                    Dtl3Params[6].Value = tu_ety.CompId;
                    Dtl3Params[7].Value = tu_ety.FinId;
                    Dtl3Params[8].Value = tu_ety.UserId;
                    Dtl3Params[9].Value = psipv.Mode;
                    Dtl3Params[10].Value = psipv.InMode3;
                    Dtl3Params[11].Value = "";
                    Dtl3Params[12].Value = 0;
                    Dtl3Params[13].Value = Convert.ToInt16(psipv.ChngUserId);

                    if (tu_ety.ParentFormatId == 27)
                        SqlHelper.ExecuteNonQuery(st, CommandType.StoredProcedure, "etrs_TransDtl3_Proc", Dtl3Params);
                    else
                        SqlHelper.ExecuteNonQuery(st, CommandType.StoredProcedure, "CRM_TransDtl3_Proc", Dtl3Params);

                    if (Convert.ToInt16(Dtl3Params[12].Value) == 0)
                    {
                        psipv.InResult = Dtl3Params[11].Value.ToString().Replace("'", " ");
                        st.Rollback();
                        cn.Close();
                        result[0] = Dtl3Params[11].Value.ToString().Replace("'", " ");
                        result[1] = "0";
                        return result;
                    }
                    result[1] = "1";
                    res += Dtl3Params[11].Value.ToString().Replace("'", " ") + ";  ";
                }

                SqlParameter[] Dtl1Params = new SqlParameter[41];
                Dtl1Params[0] = new SqlParameter("@InDocId", SqlDbType.BigInt);
                Dtl1Params[1] = new SqlParameter("@InFormatId", SqlDbType.SmallInt);
                Dtl1Params[2] = new SqlParameter("@InYearId", SqlDbType.SmallInt);
                Dtl1Params[3] = new SqlParameter("@InDocDt", SqlDbType.VarChar);
                Dtl1Params[4] = new SqlParameter("@InSlNo1", SqlDbType.VarChar);
                Dtl1Params[5] = new SqlParameter("@InItemId1", SqlDbType.VarChar);
                Dtl1Params[6] = new SqlParameter("@InQty1", SqlDbType.VarChar);
                Dtl1Params[7] = new SqlParameter("@InItemStatus1", SqlDbType.VarChar);
                Dtl1Params[8] = new SqlParameter("@InRate1", SqlDbType.VarChar);
                Dtl1Params[9] = new SqlParameter("@InQtyUnits1", SqlDbType.VarChar);
                Dtl1Params[10] = new SqlParameter("@InRateUnits1", SqlDbType.VarChar);
                Dtl1Params[11] = new SqlParameter("@InSizeId1", SqlDbType.VarChar);
                Dtl1Params[12] = new SqlParameter("@InColorId1", SqlDbType.VarChar);
                Dtl1Params[13] = new SqlParameter("@InWareHouseId1", SqlDbType.VarChar);
                Dtl1Params[14] = new SqlParameter("@InBatchNo1", SqlDbType.VarChar);
                Dtl1Params[15] = new SqlParameter("@InMfgDt1", SqlDbType.VarChar);
                Dtl1Params[16] = new SqlParameter("@InExpDt1", SqlDbType.VarChar);
                Dtl1Params[17] = new SqlParameter("@InChasisNo1", SqlDbType.VarChar);
                Dtl1Params[18] = new SqlParameter("@InPendingQty1", SqlDbType.VarChar);
                Dtl1Params[19] = new SqlParameter("@InAmount1", SqlDbType.VarChar);
                Dtl1Params[20] = new SqlParameter("@InNetAmt1", SqlDbType.VarChar);
                Dtl1Params[21] = new SqlParameter("@InTreMarks1", SqlDbType.VarChar);
                Dtl1Params[22] = new SqlParameter("@InFlgPosted1", SqlDbType.VarChar);
                Dtl1Params[23] = new SqlParameter("@InTypeSgn1", SqlDbType.VarChar);
                Dtl1Params[24] = new SqlParameter("@InFlgStkAdj1", SqlDbType.VarChar);
                Dtl1Params[25] = new SqlParameter("@InFromWH1", SqlDbType.VarChar);
                Dtl1Params[26] = new SqlParameter("@InToWH1", SqlDbType.VarChar);
                Dtl1Params[27] = new SqlParameter("@InTotTax1", SqlDbType.VarChar);
                Dtl1Params[28] = new SqlParameter("@InTotDisc1", SqlDbType.VarChar);
                Dtl1Params[29] = new SqlParameter("@InFlgFin1", SqlDbType.VarChar);
                Dtl1Params[30] = new SqlParameter("@InLdgId1", SqlDbType.VarChar);
                Dtl1Params[31] = new SqlParameter("@InRefDocId1", SqlDbType.VarChar);
                Dtl1Params[32] = new SqlParameter("@InRefSlNo1", SqlDbType.VarChar);
                Dtl1Params[33] = new SqlParameter("@InUsrId", SqlDbType.SmallInt);
                Dtl1Params[34] = new SqlParameter("@InCmpId", SqlDbType.SmallInt);
                Dtl1Params[35] = new SqlParameter("@InMode1", SqlDbType.VarChar);
                Dtl1Params[36] = new SqlParameter("@Result", SqlDbType.VarChar, 200);
                Dtl1Params[37] = new SqlParameter("@ExecStatus", SqlDbType.Int);
                Dtl1Params[38] = new SqlParameter("@InModeD", SqlDbType.VarChar);
                Dtl1Params[39] = new SqlParameter("@InHdrFlgStAdj", SqlDbType.Char);
                Dtl1Params[40] = new SqlParameter("@InChngUsrId", SqlDbType.SmallInt);
                Dtl1Params[36].Direction = ParameterDirection.Output;
                Dtl1Params[37].Direction = ParameterDirection.Output;

                Dtl1Params[0].Value = Convert.ToInt64(ProductParams[0].Value);

                if (mode == "MODIFY")
                    Dtl1Params[0].Value = Convert.ToInt64(psipv.DocId);
                Dtl1Params[1].Value = tu_ety.FormatId;
                Dtl1Params[2].Value = tu_ety.FinId;
                Dtl1Params[3].Value = psipv.InDocDt1;
                Dtl1Params[4].Value = psipv.InSlNo1;
                Dtl1Params[5].Value = psipv.InItemId1;
                Dtl1Params[6].Value = psipv.InQty1;
                Dtl1Params[7].Value = psipv.InItemStatus1;
                Dtl1Params[8].Value = psipv.InRate1;
                Dtl1Params[9].Value = psipv.InQtyUnits1;
                Dtl1Params[10].Value = psipv.InRateUnits1;
                Dtl1Params[11].Value = psipv.InSizeId1;
                Dtl1Params[12].Value = psipv.InColorId1;
                Dtl1Params[13].Value = psipv.InWareHouseId1;
                Dtl1Params[14].Value = psipv.InBatchNo1;
                Dtl1Params[15].Value = psipv.InMfgDt1;
                Dtl1Params[16].Value = psipv.InExpDt1;
                Dtl1Params[17].Value = psipv.InChasisNo1;
                Dtl1Params[18].Value = psipv.InQty1;
                Dtl1Params[19].Value = psipv.InAmount1;
                Dtl1Params[20].Value = psipv.InNetAmt1;
                Dtl1Params[21].Value = psipv.InTrRemarks1;
                Dtl1Params[22].Value = psipv.InFlgPosted1;
                Dtl1Params[23].Value = psipv.InTypeSgn1;
                Dtl1Params[24].Value = psipv.InFlgStkAdj1;
                Dtl1Params[25].Value = psipv.InFromWH1;
                Dtl1Params[26].Value = psipv.InToWH1;
                Dtl1Params[27].Value = psipv.InTotTax1;
                Dtl1Params[28].Value = psipv.InTotDisc1;
                Dtl1Params[29].Value = psipv.InFlgFin1;
                Dtl1Params[30].Value = psipv.InLdgId1;
                Dtl1Params[31].Value = psipv.InDt1RefDocId1;
                Dtl1Params[32].Value = psipv.InDt1RefSlNo1;
                Dtl1Params[33].Value = tu_ety.UserId;
                Dtl1Params[34].Value = tu_ety.CompId;
                Dtl1Params[35].Value = psipv.InMode1;
                Dtl1Params[36].Value = "";
                Dtl1Params[37].Value = 0;
                Dtl1Params[38].Value = psipv.Mode;
                Dtl1Params[39].Value = FlgAdjStk;
                Dtl1Params[40].Value = tu_ety.UserId;

                if (tu_ety.ParentFormatId == 27)
                    SqlHelper.ExecuteNonQuery(st, CommandType.StoredProcedure, "etrs_TransDtl1_Proc", Dtl1Params);
                else
                    SqlHelper.ExecuteNonQuery(st, CommandType.StoredProcedure, "CRM_TransDtl1_Proc", Dtl1Params);

                if (Convert.ToInt16(Dtl1Params[37].Value) == 0)
                {
                    psipv.InResult = Dtl1Params[36].Value.ToString().Replace("'", " ");
                    st.Rollback();
                    cn.Close();
                    result[0] = Dtl1Params[36].Value.ToString().Replace("'", " ");
                    result[1] = "0";
                    return result;
                }
                result[1] = "1";
                res += Dtl1Params[36].Value.ToString().Replace("'", " ") + ";  ";

                if (psipv.InMode4 != null)
                {
                    SqlParameter[] Dtl4Params = new SqlParameter[22];
                    Dtl4Params[0] = new SqlParameter("@InDocId", SqlDbType.BigInt);
                    Dtl4Params[1] = new SqlParameter("@InFormatId", SqlDbType.SmallInt);
                    Dtl4Params[2] = new SqlParameter("@InYearId", SqlDbType.SmallInt);
                    Dtl4Params[3] = new SqlParameter("@InSlNo1", SqlDbType.VarChar);
                    Dtl4Params[4] = new SqlParameter("@InItemId1", SqlDbType.VarChar);
                    Dtl4Params[5] = new SqlParameter("@InDocDt", SqlDbType.VarChar);
                    Dtl4Params[6] = new SqlParameter("@InTaxId1", SqlDbType.VarChar);
                    Dtl4Params[7] = new SqlParameter("@InGRPId1", SqlDbType.VarChar);
                    Dtl4Params[8] = new SqlParameter("@InTaxRate1", SqlDbType.VarChar);
                    Dtl4Params[9] = new SqlParameter("@InTaxPerQty1", SqlDbType.VarChar);
                    Dtl4Params[10] = new SqlParameter("@InTaxAmt1", SqlDbType.VarChar);
                    Dtl4Params[11] = new SqlParameter("@InTaxTypeId1", SqlDbType.VarChar);
                    Dtl4Params[12] = new SqlParameter("@InFlgFin", SqlDbType.Char, 1);
                    Dtl4Params[13] = new SqlParameter("@InTaxType1", SqlDbType.VarChar);
                    Dtl4Params[14] = new SqlParameter("@InFldId1", SqlDbType.VarChar);
                    Dtl4Params[15] = new SqlParameter("@InCmpId", SqlDbType.SmallInt);
                    Dtl4Params[16] = new SqlParameter("@InMode1", SqlDbType.VarChar);
                    Dtl4Params[17] = new SqlParameter("@InUser", SqlDbType.SmallInt);
                    Dtl4Params[18] = new SqlParameter("@Result", SqlDbType.VarChar, 200);
                    Dtl4Params[19] = new SqlParameter("@ExecStatus", SqlDbType.Int);
                    Dtl4Params[20] = new SqlParameter("@InModeD", SqlDbType.VarChar);
                    Dtl4Params[21] = new SqlParameter("@InChngUser", SqlDbType.SmallInt);

                    Dtl4Params[18].Direction = ParameterDirection.Output;
                    Dtl4Params[19].Direction = ParameterDirection.Output;

                    Dtl4Params[0].Value = Convert.ToInt64(ProductParams[0].Value);

                    if (mode == "MODIFY")
                    {
                        Dtl4Params[0].Value = Convert.ToInt64(psipv.DocId);
                    }
                    Dtl4Params[1].Value = tu_ety.FormatId;
                    Dtl4Params[2].Value = tu_ety.FinId;
                    Dtl4Params[3].Value = psipv.InSlNo4;
                    Dtl4Params[4].Value = psipv.InItemId4;
                    Dtl4Params[5].Value = psipv.InDocDt4;
                    Dtl4Params[6].Value = psipv.InTaxId4;
                    Dtl4Params[7].Value = psipv.InGRPId4;
                    Dtl4Params[8].Value = psipv.InTaxRate4;
                    Dtl4Params[9].Value = psipv.InTaxPerQty4;
                    Dtl4Params[10].Value = psipv.InTaxAmt4;
                    Dtl4Params[11].Value = psipv.InTaxTypeId4;
                    Dtl4Params[12].Value = Convert.ToChar(psipv.InFlgFin4);
                    Dtl4Params[13].Value = psipv.InTaxType4;
                    Dtl4Params[14].Value = psipv.InFldId4;
                    Dtl4Params[15].Value = tu_ety.CompId;
                    Dtl4Params[16].Value = psipv.InMode4;
                    Dtl4Params[17].Value = tu_ety.UserId;
                    Dtl4Params[18].Value = "";
                    Dtl4Params[19].Value = 0;
                    Dtl4Params[20].Value = psipv.Mode;
                    Dtl4Params[21].Value = Convert.ToInt16(psipv.ChngUserId);

                    if (tu_ety.ParentFormatId == 27)
                        SqlHelper.ExecuteNonQuery(st, CommandType.StoredProcedure, "etrs_TransDtl4_Proc", Dtl4Params);
                    else
                        SqlHelper.ExecuteNonQuery(st, CommandType.StoredProcedure, "CRM_TransDtl4_Proc", Dtl4Params);

                    if (Convert.ToInt16(Dtl4Params[19].Value) == 0)
                    {
                        psipv.InResult = Dtl4Params[18].Value.ToString().Replace("'", " ");
                        st.Rollback();
                        cn.Close();
                        result[0] = Dtl4Params[18].Value.ToString().Replace("'", " ");
                        result[1] = "0";
                        return result;
                    }
                    result[1] = "1";
                    res += Dtl4Params[18].Value.ToString().Replace("'", " ") + ";  ";
                }

                //For save Nlp TransDtls  && tu_ety.ParentFormatId == 27
                if (htNlpDtl.Count > 0)
                {
                    //SqlParameter[] nlpParams = new SqlParameter[13];
                    SqlParameter InDtlDocId = new SqlParameter("@InDtlDocId", SqlDbType.BigInt);
                    SqlParameter InDtlSlno = new SqlParameter("@InDtlSlno", SqlDbType.VarChar);
                    SqlParameter InCatSlno = new SqlParameter("@InCatSlno", SqlDbType.VarChar);
                    SqlParameter InNlpRemarks = new SqlParameter("@InNlpRemarks", SqlDbType.VarChar);
                    SqlParameter InNlpFlag = new SqlParameter("@InNlpFlag", SqlDbType.VarChar);
                    SqlParameter InNlp_Id = new SqlParameter("@InNlp_Id", SqlDbType.VarChar);
                    SqlParameter InNlpRange_Id = new SqlParameter("@InNlpRange_Id", SqlDbType.VarChar);
                    SqlParameter InProdSlno = new SqlParameter("@InProdSlno", SqlDbType.VarChar);
                    SqlParameter InAppToParty_Flg = new SqlParameter("@InAppToParty_Flg", SqlDbType.VarChar);

                    SqlParameter InNlpRate = new SqlParameter("@InNlpRate", SqlDbType.VarChar);
                    SqlParameter InRateType = new SqlParameter("@InRateType", SqlDbType.VarChar);
                    SqlParameter InDiscPcnt = new SqlParameter("@InDiscPcnt", SqlDbType.VarChar);
                    SqlParameter InItem_Id = new SqlParameter("@InItem_Id", SqlDbType.VarChar);
                    SqlParameter InMRP = new SqlParameter("@InMRP", SqlDbType.VarChar);

                    SqlParameter nlpFormatId = new SqlParameter("@FormatId", SqlDbType.SmallInt);
                    SqlParameter nlpCmpId = new SqlParameter("@CmpId", SqlDbType.SmallInt);
                    SqlParameter nlpFinYearId = new SqlParameter("@FinYearId", SqlDbType.SmallInt);
                    SqlParameter nlpUsrId = new SqlParameter("@UsrId", SqlDbType.SmallInt);
                    SqlParameter nlpInModeD = new SqlParameter("@InModeD", SqlDbType.VarChar);
                    SqlParameter nlpInNlpMode = new SqlParameter("@InNlpMode", SqlDbType.VarChar);
                    SqlParameter nlpResult = new SqlParameter("@Result", SqlDbType.VarChar, 500);
                    SqlParameter nlpExecStatus = new SqlParameter("@ExecStatus", SqlDbType.SmallInt);

                    nlpResult.Direction = ParameterDirection.Output;
                    nlpExecStatus.Direction = ParameterDirection.Output;

                    InDtlDocId.Value = Convert.ToInt64(ProductParams[0].Value);

                    if (mode == "MODIFY")
                        InDtlDocId.Value = Convert.ToInt64(htNlpDtl["DtlDocId"].ToString());
                    InDtlSlno.Value = htNlpDtl["DtlSlno"].ToString();
                    InCatSlno.Value = htNlpDtl["CatSlno"].ToString();
                    InNlpRemarks.Value = htNlpDtl["NlpRemarks"].ToString();
                    InNlpFlag.Value = htNlpDtl["NlpFlag"].ToString();
                    InNlp_Id.Value = htNlpDtl["Nlp_Id"].ToString();
                    InNlpRange_Id.Value = htNlpDtl["NlpRange_Id"].ToString();
                    InProdSlno.Value = htNlpDtl["ProdSlno"].ToString();
                    InAppToParty_Flg.Value = htNlpDtl["AppToParty_Flg"].ToString();

                    InNlpRate.Value = htNlpDtl["NlpRate"].ToString();
                    InRateType.Value = htNlpDtl["RateType"].ToString();
                    InDiscPcnt.Value = htNlpDtl["DiscPcnt"].ToString();
                    InItem_Id.Value = htNlpDtl["Item_Id"].ToString();
                    InMRP.Value = htNlpDtl["MRP"].ToString();

                    nlpFormatId.Value = tu_ety.FormatId;
                    nlpCmpId.Value = tu_ety.CompId;
                    nlpFinYearId.Value = tu_ety.FinId;
                    nlpUsrId.Value = Convert.ToInt16(psipv.ChngUserId);
                    nlpInModeD.Value = psipv.Mode;
                    nlpInNlpMode.Value = htNlpDtl["NlpMode"].ToString();
                    nlpResult.Value = "nlp";
                    nlpExecStatus.Value = 0;

                    SqlParameter[] nlpParams = {InDtlDocId,InDtlSlno,InCatSlno,InNlpRemarks,InNlpFlag,
                   InNlp_Id,InNlpRange_Id,InProdSlno,InAppToParty_Flg,InNlpRate,InRateType,InDiscPcnt,InItem_Id,InMRP,
                  nlpFormatId,nlpCmpId,nlpFinYearId,nlpUsrId,nlpInModeD,nlpInNlpMode,nlpResult,nlpExecStatus};

                    if (tu_ety.ParentFormatId == 27)
                        SqlHelper.ExecuteNonQuery(st, CommandType.StoredProcedure, "etrs_NlpTransDtl_Proc", nlpParams);
                    else
                        SqlHelper.ExecuteNonQuery(st, CommandType.StoredProcedure, "CRM_NlpTransDtl_Proc", nlpParams);

                    if (Convert.ToInt16(nlpExecStatus.Value) == 0)
                    {
                        psipv.InResult = nlpResult.Value.ToString().Replace("'", " ");
                        st.Rollback();
                        cn.Close();
                        result[0] = nlpResult.Value.ToString().Replace("'", " ");
                        result[1] = "0";
                        return result;
                    }
                    result[1] = "1";
                    res += nlpResult.Value.ToString().Replace("'", " ") + ";  ";
                }

                if (psipv.Mode == "AUTHM" || psipv.Mode == "AUTHA" || psipv.Mode == "N")
                {
                    SqlParameter[] transSLNO = new SqlParameter[4];
                    transSLNO[0] = new SqlParameter("@InDocId", SqlDbType.BigInt);
                    transSLNO[1] = new SqlParameter("@InTableType", SqlDbType.VarChar, 1);
                    transSLNO[2] = new SqlParameter("@OutErrMsg", SqlDbType.VarChar, 200);
                    transSLNO[3] = new SqlParameter("@ExecStatus", SqlDbType.TinyInt);
                    transSLNO[2].Direction = ParameterDirection.Output;
                    transSLNO[3].Direction = ParameterDirection.Output;

                    transSLNO[0].Value = Convert.ToInt64(ProductParams[0].Value);

                    if (mode == "NEW")
                    {
                        if (userType == "Y")
                            transSLNO[1].Value = "M";
                        else
                            transSLNO[1].Value = "A";
                    }

                    if (mode == "MODIFY")
                    {
                        transSLNO[0].Value = Convert.ToInt64(psipv.DocId);
                        if (psipv.Mode == "MODIFYM" || psipv.Mode == "MODIFYA")
                            transSLNO[1].Value = "A";
                        else if (psipv.Mode == "AUTHM" || psipv.Mode == "AUTHA")
                            transSLNO[1].Value = "M";
                    }

                    transSLNO[2].Value = "";
                    if (tu_ety.ParentFormatId == 27)
                        SqlHelper.ExecuteNonQuery(st, CommandType.StoredProcedure, "etrs_UpdSlNosPSI_Proc", transSLNO);
                    else
                        SqlHelper.ExecuteNonQuery(st, CommandType.StoredProcedure, "CRM_UpdSlNosPSI_Proc", transSLNO);

                    if (Convert.ToInt16(transSLNO[3].Value) == 0)
                    {
                        psipv.InResult = transSLNO[2].Value.ToString().Replace("'", " ");
                        st.Rollback();
                        cn.Close();
                        result[0] = transSLNO[2].Value.ToString().Replace("'", " ");
                        result[1] = "0";
                        return result;
                    }
                    result[1] = "1";
                    res += transSLNO[2].Value.ToString().Replace("'", " ") + ";  ";
                }
                string compid = tu_ety.CompId.ToString();// HttpContext.Current.Session["CompId"].ToString();

                // for options saving

                SqlParameter[] BdfFields = new SqlParameter[29];

                BdfFields[0] = new SqlParameter("@InDocId", SqlDbType.BigInt);
                BdfFields[1] = new SqlParameter("@InCustPONum", SqlDbType.VarChar);
                BdfFields[2] = new SqlParameter("@InCustPoDate", SqlDbType.SmallDateTime);
                BdfFields[3] = new SqlParameter("@InDelivaryDate", SqlDbType.SmallDateTime);
                BdfFields[4] = new SqlParameter("@InPackPrintDetails", SqlDbType.VarChar);
                BdfFields[5] = new SqlParameter("@InSplInst", SqlDbType.VarChar);
                BdfFields[6] = new SqlParameter("@InContPersnName", SqlDbType.VarChar);
                BdfFields[7] = new SqlParameter("@InContpersnDesig", SqlDbType.VarChar);
                BdfFields[8] = new SqlParameter("@InPayementTerms", SqlDbType.VarChar);
                BdfFields[9] = new SqlParameter("@InPrintCost", SqlDbType.Decimal);
                BdfFields[10] = new SqlParameter("@InGiftPackCost", SqlDbType.Decimal);
                BdfFields[11] = new SqlParameter("@InFreightCharge", SqlDbType.Decimal);
                BdfFields[12] = new SqlParameter("@InOtherExpenses", SqlDbType.Decimal);
                BdfFields[13] = new SqlParameter("@InOtherRemarks", SqlDbType.VarChar);
                BdfFields[14] = new SqlParameter("@InTotalGP", SqlDbType.Decimal);
                BdfFields[15] = new SqlParameter("@InTotalExpneses", SqlDbType.Decimal);
                BdfFields[16] = new SqlParameter("@InNetGP", SqlDbType.Decimal);
                BdfFields[17] = new SqlParameter("@Inother1", SqlDbType.VarChar);
                BdfFields[18] = new SqlParameter("@Inother2", SqlDbType.VarChar);
                BdfFields[19] = new SqlParameter("@Inother3", SqlDbType.VarChar);
                BdfFields[20] = new SqlParameter("@Inother4 ", SqlDbType.VarChar);
                BdfFields[21] = new SqlParameter("@InFormat_Id ", SqlDbType.SmallInt);
                BdfFields[22] = new SqlParameter("@InMode1", SqlDbType.VarChar);
                BdfFields[23] = new SqlParameter("@InYearId1", SqlDbType.SmallInt);
                BdfFields[24] = new SqlParameter("@InUsrId", SqlDbType.SmallInt);
                BdfFields[25] = new SqlParameter("@InCmpId", SqlDbType.SmallInt);
                BdfFields[26] = new SqlParameter("@Result", SqlDbType.VarChar, 250);
                BdfFields[27] = new SqlParameter("@ExecStatus", SqlDbType.Int);
                BdfFields[28] = new SqlParameter("@InChngUsrId", SqlDbType.SmallInt);
                BdfFields[26].Direction = ParameterDirection.Output;
                BdfFields[27].Direction = ParameterDirection.Output;

                BdfFields[0].Value = Convert.ToInt64(ProductParams[0].Value);
                if (mode == "MODIFY")
                    BdfFields[0].Value = Convert.ToInt64(psipv.DocId);

                BdfFields[1].Value = psipv.CustPoNum;
                BdfFields[2].Value = psipv.DocDt;
                BdfFields[3].Value = psipv.DelivDate;
                BdfFields[4].Value = psipv.PackPrintDetails;
                BdfFields[5].Value = psipv.SplInst;
                BdfFields[6].Value = psipv.CntPrsnnm;
                BdfFields[7].Value = psipv.CntDesg;
                BdfFields[8].Value = psipv.PaymentTerms;
                BdfFields[9].Value = psipv.Printing;
                BdfFields[10].Value = psipv.GiftPacking;
                BdfFields[11].Value = psipv.FreightCharges;
                BdfFields[12].Value = psipv.OtherExpenses;
                BdfFields[13].Value = psipv.Instructions;
                BdfFields[14].Value = psipv.TotalGP;
                BdfFields[15].Value = psipv.TotalExpenses;
                BdfFields[16].Value = psipv.NetGpGenerated;
                BdfFields[17].Value = psipv.other1;
                BdfFields[18].Value = psipv.other2;
                BdfFields[19].Value = psipv.other3;
                BdfFields[20].Value = psipv.other4;
                BdfFields[21].Value = psipv.FormatId;
                BdfFields[22].Value = psipv.Mode;
                BdfFields[23].Value = tu_ety.FinId;
                BdfFields[24].Value = tu_ety.UserId;
                BdfFields[25].Value = tu_ety.CompId;
                BdfFields[26].Value = "";
                BdfFields[27].Value = 0;
                BdfFields[28].Value = Convert.ToInt16(tu_ety.UserId);

                if (tu_ety.ParentFormatId == 27)
                    SqlHelper.ExecuteNonQuery(st, CommandType.StoredProcedure, "etrs_Transdtl6_Proc", BdfFields);
                else
                    SqlHelper.ExecuteNonQuery(st, CommandType.StoredProcedure, "crm_Transdtl6_Proc", BdfFields);
                if (Convert.ToInt16(BdfFields[27].Value) == 0)
                {
                    psipv.InResult = BdfFields[26].Value.ToString();
                    st.Rollback();
                    cn.Close();
                    result[0] = BdfFields[26].Value.ToString().Replace("'", " ");
                    result[1] = "0";
                    return result;
                }
                if (vLimitFlag == "Y" && (compid == "8" || compid == "7" || compid == "11"))
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

                    transSendMail[0].Value = Convert.ToInt16(psipv.AccId);
                    transSendMail[1].Value = Convert.ToInt16(psipv.DeptId);
                    transSendMail[2].Value = Convert.ToInt64(ProductParams[0].Value);
                    transSendMail[3].Value = vLimitMsg;
                    transSendMail[4].Value = "result";
                    transSendMail[5].Value = 0;

                    if (tu_ety.ParentFormatId == 27)
                        SqlHelper.ExecuteNonQuery(st, CommandType.StoredProcedure, "etrs_SendCreditLimitMail_Proc", transSendMail);
                    //else
                    //SqlHelper.ExecuteNonQuery(st, CommandType.StoredProcedure, "CRM_SendCreditLimitMail_Proc", transSendMail);

                    if (Convert.ToInt16(transSendMail[5].Value) == 0)
                    {
                        psipv.InResult = transSendMail[4].Value.ToString();
                        st.Rollback();
                        cn.Close();
                        result[0] = transSendMail[4].Value.ToString().Replace("'", " ");
                        result[1] = "0";
                        return result;
                    }
                }

                if (vLimitFlag == "N" && (compid == "8" || compid == "7" || compid == "11") && (psipv.Mode == "AUTHM" || psipv.Mode == "AUTHA" || psipv.Mode == "N"))
                {
                    SqlParameter[] transSendMastMail = new SqlParameter[6];
                    transSendMastMail[0] = new SqlParameter("@Accid", SqlDbType.SmallInt);
                    transSendMastMail[1] = new SqlParameter("@DeptId", SqlDbType.SmallInt);
                    transSendMastMail[2] = new SqlParameter("@Docid", SqlDbType.BigInt);
                    transSendMastMail[3] = new SqlParameter("@Reason", SqlDbType.VarChar, 1000);
                    transSendMastMail[4] = new SqlParameter("@OutErrMsg", SqlDbType.VarChar, 200);
                    transSendMastMail[5] = new SqlParameter("@ExecStatus", SqlDbType.TinyInt);
                    transSendMastMail[4].Direction = ParameterDirection.Output;
                    transSendMastMail[5].Direction = ParameterDirection.Output;

                    transSendMastMail[0].Value = Convert.ToInt16(psipv.AccId);
                    transSendMastMail[1].Value = Convert.ToInt16(psipv.DeptId);
                    transSendMastMail[2].Value = Convert.ToInt64(ProductParams[0].Value);
                    transSendMastMail[3].Value = vLimitMsg;
                    transSendMastMail[4].Value = "result";
                    transSendMastMail[5].Value = 0;

                    if (tu_ety.ParentFormatId == 27)
                        SqlHelper.ExecuteNonQuery(st, CommandType.StoredProcedure, "etrs_SendCreditLimitMastMail_Proc", transSendMastMail);
                    //else
                    //SqlHelper.ExecuteNonQuery(st, CommandType.StoredProcedure, "CRM_SendCreditLimitMastMail_Proc", transSendMastMail);

                    if (Convert.ToInt16(transSendMastMail[5].Value) == 0)
                    {
                        psipv.InResult = transSendMastMail[4].Value.ToString();
                        st.Rollback();
                        cn.Close();
                        result[0] = transSendMastMail[4].Value.ToString().Replace("'", " ");
                        result[1] = "0";
                        return result;
                    }
                }
                if (NlpMail == "Y" && (compid == "8" || compid == "7" || compid == "11") && tu_ety.ParentFormatId == 27 || NlpMail == "NA")
                {
                    SqlParameter pNlpEmail = new SqlParameter("@Indocid", SqlDbType.BigInt);
                    pNlpEmail.Value = Convert.ToInt64(ProductParams[0].Value);
                    SqlParameter ByndNlp = new SqlParameter("@Mode", SqlDbType.VarChar);
                    ByndNlp.Value = NlpMail;
                    SqlParameter[] EmailNlpParams = { pNlpEmail, ByndNlp };

                    if (mode == "MODIFY")
                        pNlpEmail.Value = Convert.ToInt64(psipv.DocId);

                    if (tu_ety.ParentFormatId == 27)
                        SqlHelper.ExecuteNonQuery(st, CommandType.StoredProcedure, "etrs_EmailNlpSalesOrder_Proc", EmailNlpParams);
                    //else
                    //SqlHelper.ExecuteNonQuery(st, CommandType.StoredProcedure, "CRM_EmailNlpSalesOrder_Proc", pNlpEmail);
                }
                if (NlpMail == "N" && (compid == "8" || compid == "7" || compid == "11") && tu_ety.ParentFormatId == 27)
                {
                    SqlParameter pNlpEmail = new SqlParameter("@Indocid", SqlDbType.BigInt);
                    pNlpEmail.Value = Convert.ToInt64(ProductParams[0].Value);
                    if (mode == "MODIFY")
                        pNlpEmail.Value = Convert.ToInt64(psipv.DocId);

                    int nlpm = SqlHelper.ExecuteNonQuery(st, CommandType.StoredProcedure, "Etrs_EmailNlpSalesOrderMast_Proc", pNlpEmail);
                }
                psipv.DocId = ProductParams[0].Value.ToString();

                //st.Commit();
                //return BdfFields[26].Value.ToString();
                result[1] = "1";
                result[0] = res;
                //  res += transSLNO[2].Value.ToString().Replace("'", " ") + ";  ";
                st.Commit();

                if (psipv.InRefDocId != null)
                {
                    SqlParameter[] RefParams = new SqlParameter[15];
                    RefParams[0] = new SqlParameter("@InDocId", SqlDbType.BigInt);
                    RefParams[1] = new SqlParameter("@InSlNo", SqlDbType.VarChar);
                    RefParams[2] = new SqlParameter("@InRefDocId", SqlDbType.VarChar);
                    RefParams[3] = new SqlParameter("@InRefSlNo", SqlDbType.VarChar);
                    RefParams[4] = new SqlParameter("@InItemId", SqlDbType.VarChar);
                    RefParams[5] = new SqlParameter("@InQty", SqlDbType.VarChar);
                    RefParams[6] = new SqlParameter("@InFormatId", SqlDbType.SmallInt);
                    RefParams[7] = new SqlParameter("@InCmpId", SqlDbType.SmallInt);
                    RefParams[8] = new SqlParameter("@InFinYearId", SqlDbType.SmallInt);
                    RefParams[9] = new SqlParameter("@InUsrId", SqlDbType.SmallInt);
                    RefParams[10] = new SqlParameter("@InMode1", SqlDbType.VarChar);
                    RefParams[11] = new SqlParameter("@InMode", SqlDbType.VarChar);
                    RefParams[12] = new SqlParameter("@Result", SqlDbType.VarChar, 200);
                    RefParams[13] = new SqlParameter("@ExecStatus", SqlDbType.Int);
                    RefParams[14] = new SqlParameter("@InChngUsrId", SqlDbType.SmallInt);

                    RefParams[12].Direction = ParameterDirection.Output;
                    RefParams[13].Direction = ParameterDirection.Output;
                    RefParams[0].Value = Convert.ToInt64(ProductParams[0].Value); ;
                    RefParams[1].Value = psipv.InSlNo_ref;
                    RefParams[2].Value = psipv.InRefDocId;
                    RefParams[3].Value = psipv.InRefSlNo;
                    RefParams[4].Value = psipv.InItemIdRef;
                    RefParams[5].Value = psipv.InQtyRef;
                    RefParams[6].Value = tu_ety.FormatId;
                    RefParams[7].Value = tu_ety.CompId;
                    RefParams[8].Value = tu_ety.FinId;
                    RefParams[9].Value = tu_ety.UserId;
                    RefParams[10].Value = psipv.InRefMode;
                    RefParams[11].Value = psipv.Mode;
                    RefParams[12].Value = "";
                    RefParams[13].Value = 0;
                    RefParams[14].Value = Convert.ToInt16(psipv.ChngUserId);

                    int r = SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["Crm_ConnStr"].ConnectionString, CommandType.StoredProcedure, "CRM_TransRef_Proc", RefParams);

                    if (Convert.ToInt16(RefParams[13].Value) == 0)
                    {
                        psipv.InResult = RefParams[12].Value.ToString();
                        st.Rollback();
                        cn.Close();
                        result[0] = RefParams[12].Value.ToString().Replace("'", " ");
                        result[1] = "0";
                        return result;
                    }
                    result[1] = "1";
                    res += RefParams[12].Value.ToString().Replace("'", " ") + ";  ";

                    result[1] = "1";
                    result[0] = res;
                }

                return result;
            }
            catch (Exception ex)
            {
                psipv.InResult = ex.Message.Replace("'", " ");
                st.Rollback();
                cn.Close();
                result[0] = ex.Message.Replace("'", " ");
                result[1] = "0";
                return result;
            }
            finally
            {
                cn.Close();

            }
        }

        public int Save_PendingReasons(TransUser_Ety tu_ety)
        {
            int Re = 0;
            cn2.Open();
            string strProc = string.Empty;
            try
            {
                SqlParameter pAccId, pDeptId, pReason;

                pAccId = new SqlParameter("@AccId", SqlDbType.BigInt);
                pAccId.Value = tu_ety.AccId;
                pDeptId = new SqlParameter("@DeptId", SqlDbType.SmallInt);
                pDeptId.Value = tu_ety.DeptId;
                pReason = new SqlParameter("@Reason", SqlDbType.VarChar, 8000);
                pReason.Value = tu_ety.TransMode;


                SqlParameter[] paramFields = { pAccId, pDeptId, pReason };

                strProc = "Etrs_SavePendingReasons_Proc1";
                if (strProc != string.Empty)
                {
                    Re = SqlHelper.ExecuteNonQuery(cn2, CommandType.StoredProcedure, strProc, paramFields);
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
            return Re;
        }

        public DataTable CRM_Get_FormatSeries(TransUser_Ety tu_ety)
        {
            string strQuey = "select Series_Id,Series From CRM_SeriesMast_View where Format_Id=" + tu_ety.FormatId + " And year_id=" + tu_ety.FinId + " order by Series";

            using (Trans_Main_DAC dac = new Trans_Main_DAC())
            {
                return dac.Get_StrQuery_Data(strQuey);
            }
        }
        public DataTable Get_NarrationService_TypeSign()
        {
            string strQuey = "select Nrtn_Id ,Nrtn_SaleType from ETRS_NarrationService_Tbl where DiscontinueFlag='N'"; // Nrtn_Type='N' and 
            using (Trans_Main_DAC dac = new Trans_Main_DAC())
            {
                return dac.Get_GWTME_StrQuery_Data(strQuey);
            }
        }

        public DataTable Get_Trans_AccDtls(TransUser_Ety tu_ety)
        {
            SqlParameter vAccId = new SqlParameter("@AccId", SqlDbType.BigInt);
            vAccId.Value = tu_ety.AccId;
            string strQuey = "select acc_id  hdfHAccId,ACCCODE CMBHACCCODE,Primary_Code CMBHPACCCODE,Secondary_Code CMBHSACCCODE,AccName CMBHACCNAME " +
                " from etrs_AccMast_tbl where acc_Id=" + tu_ety.AccId + "";
            using (Trans_Main_DAC dac = new Trans_Main_DAC())
            {
                return dac.Get_GWTME_StrQuery_Data(strQuey);
            }
        }
        public DataTable CRM_Get_TransHdr_PotentialDtls(TransUser_Ety tu_ety)
        {
            SqlParameter vAccId = new SqlParameter("@AccId", SqlDbType.BigInt);
            vAccId.Value = tu_ety.AccId;
            string strQuey = "SELECT PotentialId hdfHAccId,'0' CMBHACCCODE,'0' CMBHPACCCODE,'0' CMBHSACCCODE,PotentialName CMBHACCNAME "
            + " FROM CRM_PotentialMast_Tbl WHERE PotentialId=" + tu_ety.AccId + "";
            using (Trans_Main_DAC dac = new Trans_Main_DAC())
            {
                return dac.Get_StrQuery_Data(strQuey);
            }
        }
        public DataTable Get_Dept_Dtls(TransUser_Ety tu_ety)
        {
            string strQuey = "select Dept_Id hdfCMBHDEPTNAME, Dept_Desc CMBHDEPTNAME from etrs_Deptmast_tbl where Dept_id=" + tu_ety.DeptId + "";
            using (Trans_Main_DAC dac = new Trans_Main_DAC())
            {
                return dac.Get_GWTME_StrQuery_Data(strQuey);
            }
        }

        public DataTable Get_Narration_Dtls(TransUser_Ety tu_ety)
        {
            string strQuey = "select Nar_Id hdfCMBHNARATION,Narration CMBHNARATION from etrs_NarrationMast_tbl where Nar_Id=" + tu_ety.AccId + "";
            using (Trans_Main_DAC dac = new Trans_Main_DAC())
            {
                return dac.Get_GWTME_StrQuery_Data(strQuey);
            }
        }
        public DataTable CRM_Get_Trans_Dtl3_Dtls(TransUser_Ety tu_ety)
        {
            SqlParameter vDocId = new SqlParameter("@DocId", SqlDbType.BigInt);
            vDocId.Value = tu_ety.DocId;
            string strQuey = "select (select Control_Name from Crm_fdfielddetails_View where field_Id=Fldid) Control_Name ,Fldval  "
            + " from Crm_TransDtl3_tbl where slno=0 and docId=" + tu_ety.DocId + "";
            using (Trans_Main_DAC dac = new Trans_Main_DAC())
            {
                return dac.Get_StrQuery_Data(strQuey);
            }
        }

        public DataTable CRM_Get_IsExistedInMasterAuthor(TransUser_Ety tu_ety)
        {
            string strQuey = "SELECT docid,DOCSERIES,DOCNO,DOCALPHA,FORMAT_ID,DOCDT FROM CRM_TRANSHDR_AUTHOR_TBL WHERE DOCID=" + tu_ety.DocId;
            using (Trans_Main_DAC dac = new Trans_Main_DAC())
            {
                return dac.Get_StrQuery_Data(strQuey);
            }

        }
        public DataTable Get_FinYearDates(TransUser_Ety tu_ety)
        {
            string strQuey = "select a.FinYear_StrtDate , a.FinYear_EndDate,(FinYear_StrtDate)+1 FinYear_StrtDate1,(FinYear_StrtDate)+1 " +
            "FinYear_EndDate1 from Etrs_FinYearMast_Tbl a where a.FinYear_Id=" + tu_ety.FinId;

            using (Trans_Main_DAC dac = new Trans_Main_DAC())
            {
                return dac.Get_StrQuery_Data(strQuey);
            }
        }

    }
}
