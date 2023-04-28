using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealSys.CoreLib.Models.DTO.Jobs
{
    public class cJobService
    {
        [Key]
        public int Id { get; set; }
        public CoreLib.Models.Erp.JobServices Service { get; set; }
        public IQueryable<CoreLib.Models.Erp.JobAction> Actions { get; set; }
        public IQueryable<CoreLib.Models.Erp.SrvActionItem> SvcActions { get; set; }
        public IQueryable<CoreLib.Models.Erp.SrvActionItem> SvcActionsDone { get; set; }
        public IQueryable<CoreLib.Models.Erp.JobServiceItem> SvcItems { get; set; }
        public IQueryable<CoreLib.Models.Erp.SupplierPoDtl> SupplierPos { get; set; }
    }
}
