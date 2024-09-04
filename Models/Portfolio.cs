using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    [Table("Portfolios")]
    public class Portfolio
    {
        public string AppUserId { get; set; } = string.Empty;
        public int  StockId { get; set; }
        public AppUser appUser { get; set; }
        public Stock stock { get; set; }
    }
}