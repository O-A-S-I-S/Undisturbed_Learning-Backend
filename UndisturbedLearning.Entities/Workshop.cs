using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UndisturbedLearning.Entities;

public class Workshop: EntityBase
{
    [Required]
    public DateTime Start { get; set; }
    [Required]
    public DateTime End { get; set; }
    [StringLength(60)]
    [Required] 
    public string Title { get; set; }
    [StringLength(200)]
    [Required]
    public string Brief { get; set; }
    [StringLength(2000)]
    [Required]
    public string Text { get; set; }
    [StringLength(1000)]
    public string Comment { get; set; }
    [DefaultValue(false)]
    [Required]
    public bool Reminder { get; set; }
    
    [Required]
    public int PsychopedagogistId { get; set; }
    public Psychopedagogist Psychopedagogist { get; set; }

    public ICollection<Student>? Students { get; set; }
    
    [NotMapped]
    public string Date
    {
        get
        {
            return Start.ToLongDateString();
        }
    }
    [NotMapped]
    public string StartTime
    {
        get
        {
            return Start.ToShortTimeString();
        }
    }
    [NotMapped]
    public string EndTime
    {
        get
        {
            return End.ToShortTimeString();
        }
    }
}