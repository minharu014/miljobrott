using System;
using System.ComponentModel.DataAnnotations;

namespace Miljobrott.Models
{
    public class ErrandStatus
    {
        [Key]
        public string StatusId { get; set; }
        public string StatusName { get; set; }
    }
}
