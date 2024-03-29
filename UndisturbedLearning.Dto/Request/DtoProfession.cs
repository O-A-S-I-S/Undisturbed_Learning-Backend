using System.ComponentModel.DataAnnotations;

namespace UndisturbedLearning.Dto.Request;

public class DtoProfession
{
    [StringLength(40)]
    public string Name { get; set; }
    [StringLength(200)]
    public string Description { get; set; }
}