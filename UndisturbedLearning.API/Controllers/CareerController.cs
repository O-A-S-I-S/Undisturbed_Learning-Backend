using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UndisturbedLearning.DataAccess;
using UndisturbedLearning.Dto.Request;
using UndisturbedLearning.Entities;

namespace UndisturbedLearning.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarreerController : ControllerBase
{
    private readonly UndisturbedLearningDbContext _context;

    public CarreerController(UndisturbedLearningDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<ICollection<Career>>> Get()
    {
        ICollection<Career> response;

        response = await _context.Careers.ToListAsync();


        return Ok(response);
    }


    [HttpGet("{id:int}")]
    public async Task<ActionResult<Career>> Get(int id)
    {
        var entity = await _context.Careers.FindAsync(id);
        if (entity == null)
        {
            return NotFound("Invalid id.");
        }

        return Ok(entity);
    }


    [HttpPost]
    public async Task<ActionResult> Post(DtoCareer request)
    {
        var entity = new Career
        {
            Name = request.Name,
            Description = request.Description
        };

        _context.Careers.Add(entity);
        await _context.SaveChangesAsync();

        HttpContext.Response.Headers.Add("location", $"/api/career/{entity.Id}");

        return Ok();
    }


    [HttpPut("{id:int}")]
    public async Task<ActionResult> Put(int id, DtoCareer request)
    {
        var entity = await _context.Careers.FindAsync(id);

        if (entity == null) return NotFound();

        entity.Description = request.Description;

        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return Ok(new
        {
            Id = id
        });
    }
}