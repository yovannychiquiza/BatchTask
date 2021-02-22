using System;
using System.Collections.Generic;

namespace BatchTask.Models
{
    public partial class Log
    {
        public int Id { get; set; }
        public string PalletNo { get; set; }
        public string Sno { get; set; }
        public DateTime DateModified { get; set; }
        public string UserName { get; set; }
        public bool UplodedToCloud { get; set; }
        public string S3url { get; set; }
        public DateTime? DateUpload { get; set; }
    }
}
