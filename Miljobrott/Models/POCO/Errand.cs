using Miljobrott.Models.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Miljobrott.Models
{
    public class Errand
    {

        public int ErrandId { get; set; }
        public string RefNumber { get; set; }

        [Display(Name = "Vart har brottet skett någonstans?")]
        [Required(ErrorMessage = "Du måste fylla i platsen för brottet!")]
        public string Place { get; set; }

        [Display(Name = "Vilken typ av brott?")]
        [Required(ErrorMessage = "Du måste fylla i typen av brott!")]
        public string TypeOfCrime { get; set; }

        [Display(Name = "När skedde brottet?")]
        [Required(ErrorMessage = "Du måste fylla i när brottet skedde!")]
        [DataType(DataType.Date)]
        public DateTime DateOfObservation { get; set; }

        [Display(Name = "Ditt namn (för- och efternamn):")]
        [Required(ErrorMessage = "Du måste fylla i ditt för- och efternamn!")]
        public string InformerName { get; set; }


        [Display(Name = "Din telefon:")]
        [Required(ErrorMessage = "Du måste fylla i ditt telefonnummer!")]
        [RegularExpression(@"^[0]{1}[0-9]{1,3}-[0-9]{5,9}$", ErrorMessage = "Skriv in ett telefonnummer med rätt format!")]
        public string InformerPhone { get; set; }

        [Display(Name = "Beskriv din observation\n(ex. namn på misstänkt person):")]
        public string? Observation { get; set; }
        public string? InvestigatorInfo { get; set; }
        public string? InvestigatorAction { get; set; }
        public string? StatusId { get; set; }
        public string? DepartmentId { get; set; }
        public string? EmployeeId { get; set; }
        public ICollection<Sample> Samples { get; set; }
        public ICollection<Picture> Pictures { get; set; }
    }
}
