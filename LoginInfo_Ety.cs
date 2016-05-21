using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Controller_Crm
{
    public class LoginInfo_Ety
    {

    }

    [Serializable()]
    public partial class LoginInfo
    {
        public string UserType { get; set; }
        public string UserName { get; set; }
        public Int64 UserId { get; set; }
        public string IpAddress { get; set; }
        public string Sessionid { get; set; }
        public Int16 CompId { get; set; }
        public string CompName { get; set; }
        public Int16 FinId { get; set; }
        public string FinyearDesc { get; set; }
        public string FinyearEndDate { get; set; }
        public string FinyearStartDate { get; set; }
        public string ConStr { get; set; }
        public string UserParentType { get; set; }
        public DataTable UserRights { get; set; }
        public string CompType { get; set; }
    }
}
