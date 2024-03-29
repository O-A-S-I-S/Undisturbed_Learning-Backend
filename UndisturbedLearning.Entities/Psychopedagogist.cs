using System.ComponentModel.DataAnnotations;

namespace UndisturbedLearning.Entities;

public class Psychopedagogist: EntityBase
{
   [StringLength(9, MinimumLength = 9)]
   [Required]
   public string Code { get; set; }
   [StringLength(25)]
   [Required]
   [DataType(DataType.Password)]
   public string Password { get; set; }
   [StringLength(8, MinimumLength = 8)]
   [Required]
   public string Dni { get; set; }
   [StringLength(30)]
   [Required]
   public string Surname { get; set; }
   [StringLength(30)]
   [Required]
   public string LastName { get; set; }
   [DataType(DataType.Date)]
   public DateTime BirthDate { get; set; }
   [DataType(DataType.EmailAddress)]
   public string Email { get; set; }
   [StringLength(9, MinimumLength = 9)]
   public string Cellphone { get; set; }
   [StringLength(7, MinimumLength = 7)]
   public string Telephone { get; set; }
   [Required]
   public bool IndividualAssistance { get; set; }

   [Required]
   public int ProfessionId { get; set; }
   public Profession Profession { get; set; }
   
   [Required]
   public int CampusId { get; set; }
   public Campus Campus { get; set; }
   
   public ICollection<Appointment>? Appointments { get; set; }
}