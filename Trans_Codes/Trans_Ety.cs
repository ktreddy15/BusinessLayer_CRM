using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Controller_Crm;

namespace Controller_Crm
{
    class Trans_Ety
    {
    }

    [Serializable()]
    public partial class TransUser_Ety
    {
        public Int64 DocId { get; set; }
        public Int64 AccId { get; set; }
        public DateTime FromDt { get; set; }
        public DateTime ToDt { get; set; }
        public Int64 UserId { get; set; }
        public Int16 FormatId { get; set; }
        public Int16 ParentFormatId { get; set; }
        public Int16 FinId { get; set; }
        public Int16 DeptId { get; set; }
        public Int16 CompId { get; set; }
        public string TransMode { get; set; }
        public string UserType { get; set; }
    }

    [Serializable()]
    public partial class TransProd_Ety
    {
        public Int64 ItemId { get; set; }
        public Int16 ColorId { get; set; }
        public Int64 Qty { get; set; }
        public Int16 WhId { get; set; }
        public DateTime TransDt { get; set; }
        public Decimal Rate { get; set; }
        public string Mode { get; set; }
        public string PartyWiseNlp { get; set; }
        public Int64 AccId { get; set; }
    }

    [Serializable()]
    public partial class TransProdValues_Ety
    {
        public string ChngUserId { get; set; }
        public string DocId { get; set; }
        public string DocType { get; set; }
        public string DocSeries { get; set; }
        public string DocAlpha { get; set; }
        public string DocNo { get; set; }
        public string DocDt { get; set; }
        public string AccId { get; set; }
        public string TransNo { get; set; }
        public string TransDt { get; set; }
        public string TransOthers { get; set; }
        public string DocAmount { get; set; }
        public string TypeSgn { get; set; }
        public string YearId { get; set; }
        public string Delivered { get; set; }
        public string HRemarks { get; set; }
        public string FormatId { get; set; }
        public string TotalDiscount { get; set; }
        public string TotalGrass { get; set; }
        public string DeptId { get; set; }
        public string TotalTax { get; set; }
        public string FlgAdjFin { get; set; }
        public string FlgAdjStk { get; set; }
        public string TotalExTaxDiscount { get; set; }
        public string NarrationId { get; set; }
        public string FlgRef { get; set; }
        public string StkBrId { get; set; }
        public string LdgId { get; set; }
        public string Cheaked { get; set; }
        public string Mode { get; set; }
        public string CmpId { get; set; }

        //@InDocId BIGINT,  
        //@InSlNo1 VARCHAR(500),  
        //@InFldId1 VARCHAR(500),  
        //@InFldValue1 varchar(1000),  
        //@InFldType1 varchar(1000),  
        //@InMode varchar(10), 
        //TransDtl3 fielsd

        public string InSlNo3 { get; set; }
        public string InFldId3 { get; set; }
        public string InFldValue3 { get; set; }
        public string InFldType3 { get; set; }
        public string InMode3 { get; set; }

        //------------for Dtl1-----------------//

        //InDocId BIGINT,  
        //InFormatId SMALLINT,  
        //InYearId SMALLINT,  
        //InDocDt VARCHAR(30),  
        //InSlNo1 VARCHAR(500),  
        //InItemId1 VARCHAR(500),  
        //InQty1 VARCHAR(1000),  
        //InItemStatus1 VARCHAR(500),  
        //InRate1 VARCHAR(1000),  
        //InQtyUnits1 VARCHAR(500),  
        //InRateUnits1 VARCHAR(500),  
        //InSizeId1 VARCHAR(500),  
        //InColorId1 VARCHAR(500),  
        //InWareHouseId1 VARCHAR(500),  
        //InBatchNo1 VARCHAR(500),  
        //InMfgDt1 VARCHAR(500),  
        //InExpDt1 VARCHAR(500),  
        //InChasisNo1 VARCHAR(1000),  
        //InPendingQty1 VARCHAR(500),  
        //InAmount1 VARCHAR(1000),  
        //InNetAmt1 VARCHAR(1000),  
        //InTreMarks1 VARCHAR(5000),  
        //InFlgPosted1 VARCHAR(500),  
        //InTypeSgn1 VARCHAR(500),  
        //InFlgStkAdj1 VARCHAR(500),  
        //InFromWH1 VARCHAR(500),  
        //InToWH1 VARCHAR(500),  
        //InTotTax1 VARCHAR(1000),  
        //InTotDisc1 VARCHAR(1000),  
        //InFlgFin1 VARCHAR(500),  
        //InLdgId1 VARCHAR(500),  
        //InUsrId SMALLINT,  
        //InMode VARCHAR(10),  
        //Result varchar(200)  OUTPUT

        public string InDocId1 { get; set; }
        public string InFormatId1 { get; set; }
        public string InYearId1 { get; set; }
        public string InDocDt1 { get; set; }
        public string InSlNo1 { get; set; }
        public string InItemId1 { get; set; }
        public string InQty1 { get; set; }
        public string InItemStatus1 { get; set; }
        public string InRate1 { get; set; }
        public string InQtyUnits1 { get; set; }
        public string InRateUnits1 { get; set; }
        public string InSizeId1 { get; set; }
        public string InColorId1 { get; set; }
        public string InWareHouseId1 { get; set; }
        public string InBatchNo1 { get; set; }
        public string InMfgDt1 { get; set; }
        public string InExpDt1 { get; set; }
        public string InChasisNo1 { get; set; }
        public string InPendingQty1 { get; set; }
        public string InAmount1 { get; set; }
        public string InNetAmt1 { get; set; }
        public string InTrRemarks1 { get; set; }
        public string InFlgPosted1 { get; set; }
        public string InTypeSgn1 { get; set; }
        public string InFlgStkAdj1 { get; set; }
        public string InFromWH1 { get; set; }
        public string InToWH1 { get; set; }
        public string InTotTax1 { get; set; }
        public string InTotDisc1 { get; set; }
        public string InFlgFin1 { get; set; }
        public string InLdgId1 { get; set; }
        public string InMode1 { get; set; }
        public string InDt1RefDocId1 { get; set; }
        public string InDt1RefSlNo1 { get; set; }

        //---------for TransDtl4---------------//

        //@InDocId BIGINT,
        //@InFormatId SMALLINT,
        //@InYearId SMALLINT,
        //@InSlNo1 VARCHAR(500),
        //@InItemId1 VARCHAR(500),
        //@InDocDt1 VARCHAR(500),
        //@InTaxId1 VARCHAR(500),
        //@InGRPId1 VARCHAR(500),
        //@InTaxRate1 VARCHAR(1000),
        //@InTaxPerQty1 VARCHAR(1000),
        //@InTaxAmt1 VARCHAR(1000),
        //@InTypeSgn1 VARCHAR(500),
        //@InTaxTypeId1 VARCHAR(500),
        //@InFlgFin1 VARCHAR(500),
        //@InLdgId1 VARCHAR(500),
        //@InMode1 VARCHAR(200),
        //@InUser SMALLINT,
        //@Result VARCHAR(200) OUTPUT

        //@InDocId BIGINT,  
        //@InFormatId SMALLINT,  
        //@InYearId SMALLINT,  
        //@InSlNo1 VARCHAR(500),  
        //@InItemId1 VARCHAR(500),  
        //@InDocDt1 VARCHAR(500),  
        //@InTaxId1 VARCHAR(500),  
        //@InGRPId1 VARCHAR(500),  
        //@InTaxRate1 VARCHAR(1000),  
        //@InTaxPerQty1 VARCHAR(1000),  
        //@InTaxAmt1 VARCHAR(1000),  
        //--   @InTypeSgn1 VARCHAR(500),       -- To be Removed    (For Credit='C' and Debit = 'D')  
        //@InTaxTypeId1 VARCHAR(500),  
        //@InFlgFin CHAR(1),  
        //@InTaxType1 VARCHAR(500),       -- Added by Mohan   (For Tax='T' and Discount='D')  
        //--@InLdgId1 VARCHAR(500),  
        //@InFldId1 VARCHAR(500),           -- Added by Mohan  
        //@InCmpId SMALLINT,              -- Added by Mohan  
        //@InMode1 VARCHAR(200),  
        //@InUser SMALLINT,  
        //@Result VARCHAR(200) OUTPUT
        
        public string InSlNo4 { get; set; }
        public string InItemId4 { get; set; }
        public string InDocDt4 { get; set; }
        public string InTaxId4 { get; set; }
        public string InGRPId4 { get; set; }
        public string InTaxRate4 { get; set; }
        public string InTaxPerQty4 { get; set; }
        public string InTaxAmt4 { get; set; }
        public string InTypeSgn4 { get; set; }
        public string InTaxTypeId4 { get; set; }
        public string InFlgFin4 { get; set; }
        public string InLdgId4 { get; set; }
        public string InMode4 { get; set; }
        public string InTaxType4 { get; set; }
        public string InFldId4 { get; set; }

        //------------Etrs_SchemeofferProd_Proc----------------//
        //DECLARE @INSNO varchar(1000)
        //DECLARE @INSUBSCHEMEID varchar(1000)
        //DECLARE @INACCOUNTID varchar(1000)
        //DECLARE @INPRODUCTID varchar(1000)
        //DECLARE @INQUANTITY varchar(1000)
        //DECLARE @INMRP varchar(1000)

        public string InSno_Sch { get; set; }
        public string InSubSchemeId_Sch { get; set; }
        public string InProdId_Sch { get; set; }
        public string InQty_Sch { get; set; }
        public string InNetRate_Sch { get; set; }
        public string InAccId_Sch { get; set; }

        //-------------SchemeDetails-19-------------------//

        //DECLARE @InDocId smallint
        //DECLARE @Slno smallint
        //DECLARE @InYearId smallint
        //DECLARE @InFormatId smallint
        //DECLARE @InSubSchemeId varchar(500)
        //DECLARE @InItemId varchar(500)
        //DECLARE @InItemQty varchar(500)
        //DECLARE @InOfferProdId varchar(500)
        //DECLARE @InOfferQty varchar(500)
        //DECLARE @InAssortedQty varchar(500)
        //DECLARE @InDiscountPercent varchar(500)
        //DECLARE @InDiscountAmt varchar(500)
        //DECLARE @InMRP varchar(500)
        //DECLARE @UsrId smallint
        //DECLARE @CmpId smallint
        //DECLARE @Mode varchar(10)
        //DECLARE @InModeD varchar(10)
        //DECLARE @Result varchar(500)

        public string InDocId_Sdtl { get; set; }
        public string InSchemeId_Sdtl { get; set; }
        public string InSlno_Sdtl { get; set; }
        public string InYearId_Sdtl { get; set; }
        public string InFormatId_Sdtl { get; set; }
        public string InSubSchemeId_Sdtl { get; set; }
        public string InItemId_Sdtl { get; set; }
        public string InColorId_Sdtl { get; set; }
        public string InItemQty_Sdtl { get; set; }
        public string InRefSlno_Sdtl { get; set; }
        public string InOfferQty_Sdtl { get; set; }
        public string InAssortedQty_Sdtl { get; set; }
        public string InDiscPcnt_Sdtl { get; set; }
        public string InDiscAmt_Sdtl { get; set; }
        public string InMRP_Sdtl { get; set; }
        public string InPoints_Sdtl { get; set; }
        public string InMode_Sdtl { get; set; }
        public string InModeD_Sdtl { get; set; }
        public string InProdType_Sdtl { get; set; }
        //ReferenceSlNo
        public string InReferenceSlNo_Sdtl { get; set; }
        public string InSchMastSlNo_Sdtl { get; set; }

        //-------------SchemeDetails-8-------------------//

        //DECLARE @Slno varchar(500)
        //DECLARE @InSubSchemeId varchar(500)
        //DECLARE @InProductId varchar(500)
        //DECLARE @InQuantity varchar(500)
        //DECLARE @InMRP varchar(500)
        //DECLARE @InMandatoryFLg varchar(500)
        //DECLARE @ExecStatus varchar(500)
        //DECLARE @Result varchar(500)

        public string InSlno_Prs { get; set; }
        public string InSubSchemeId_Prs { get; set; }
        public string InProductId_Prs { get; set; }
        public string InQuantity_Prs { get; set; }
        public string InMRP_Prs { get; set; }
        public string InMandatoryFLg_Prs { get; set; }

        //--------------TransRef_tbl-----------------//
        //DECLARE @InRefId bigint
        //DECLARE @InDocId bigint
        //DECLARE @InSlNo varchar(4000)
        //DECLARE @InRefDocId varchar(4000)
        //DECLARE @InRefSlNo varchar(4000)

        public string InPendingQtyRef { get; set; }

        public string InPendFlagRef { get; set; }

        public string InSlNo_ref { get; set; }
        public string InRefDocId { get; set; }
        public string InRefSlNo { get; set; }
        public string InItemIdRef { get; set; }
        public string InQtyRef { get; set; }
        public string InRefMode { get; set; }
        public string InResult { get; set; }

        public string SetDocType(Int16 parentformat)
        {
            switch (parentformat)
            {
                case 19:
                    return "BTR";
                case 20:
                    return "STR";
                case 21:
                    return "PO";
                case 22:
                    return "PB";
                case 23:
                    return "PI";
                case 24:
                    return "PR";
                case 25:
                    return "BTI";
                case 26:
                    return "STI";
                case 27:
                    return "SO";
                case 28:
                    return "SB";
                case 29:
                    return "SI";
                case 30:
                    return "SR";
                case 31:
                    return "PS";
                default:
                    return "PS";

            }
            // return "PSI";
        }

        //--------------------------Transaction Maim-----------------//

        public string SFormatId { get; set; }
        public string Auth { get; set; }
        public string Reason { get; set; }
        public string CustPoNum { get; set; }
        public string CustPoDate { get; set; }
        public string CntPrsnnm { get; set; }
        public string CntDesg { get; set; }
        public string PackPrintDetails { get; set; }
        public string SplInst { get; set; }
        public string other1 { get; set; }
        public string other2 { get; set; }
        public string other3 { get; set; }
        public string other4 { get; set; }
        public string PaymentTerms { get; set; }
        public Decimal Printing { get; set; }
        public Decimal GiftPacking { get; set; }
        public Decimal FreightCharges { get; set; }
        public Decimal OtherExpenses { get; set; }
        public string Instructions { get; set; }

        public Decimal TotalExpenses { get; set; }
        public Decimal NetGpGenerated { get; set; }

        public Decimal TotalGP { get; set; }
        public Nullable<DateTime> DelivDate { get; set; }
        public int securityid { get; set; }
        public int finid { get; set; }
    }


}
