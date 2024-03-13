using Dometrain.EFCore.API.Data;
using Dometrain.EFCore.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dometrain.EFCore.API.Controllers;

[ApiController]
[Route("[controller]")]
public class MoviesController : Controller
{
    private readonly MoviesContext _context;

    public MoviesController(MoviesContext context)
    {
        _context = context;
    }       
        
    [HttpGet]
    [ProducesResponseType(typeof(List<Movie>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _context.Movies.ToListAsync());
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Movie), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        // Find takes an identifier, like id
        var movie = await _context.Movies.FindAsync(id);
        
        // Through exeption if there are more than one result
        //var movie = await _context.Movies.SingleOrDefaultAsync(x => x.Id == id);
        
        //Returns fist match, expecting a single result.
        //var movie = await _context.Movies.FirstOrDefaultAsync(x => x.Id == id);
        
        return movie == null ? NotFound() : Ok(movie);
    }

    [HttpGet("by-year/{year:int}")]
    [ProducesResponseType(typeof(List<Movie>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllByYear([FromRoute] int year)
    {
        //Defered execution: linq only executes when you ask for results. First/ToArray/Where does not execute immedietly,
        // we only define the queries until we hit ToListAsync, only then do the query execute. 
        //var in this case is IQueryable. The Where() is acutally a where claus in sql.
        
        //If we want to display title and year only, then the following code isn't effective since it's getting 
        // all the fields from the database when we only want to fetch title and year.
        
        /*
        var allMovies = _context.Movies;
        var filteredMovies = allMovies.Where(x => x.ReleaseDate.Year == year);
        return Ok(await filteredMovies.ToListAsync());
        */
        
        //Projection: make another movie class only having an id and a title. Don't work with the full object just because it is easy.
        var filteredTitles = await _context.Movies
            .Where(movie => movie.ReleaseDate.Year == year)
            .Select(movie => new MovieTitle { Id = movie.Identifier, Title = movie.Title })
            .ToListAsync();

        return Ok(filteredTitles);
        //Projections let's us control how much data that is being pulled from the DB.
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(Movie), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] Movie movie)
    {
        await _context.Movies.AddAsync(movie);
        
        //movie object lacks id, ef does this for us after SaveChanges()
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new {id = movie.Identifier}, movie);
    }
    
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(Movie), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] Movie movie)
    {
        var existingMovie = await _context.Movies.FindAsync(id);

        if (existingMovie is null)
        {
            return NotFound();
        }

        existingMovie.Title = movie.Title;
        existingMovie.ReleaseDate = movie.ReleaseDate;
        existingMovie.Synopsis = movie.Synopsis;

        await _context.SaveChangesAsync();

        return Ok(existingMovie);
    }
    
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remove([FromRoute] int id)
    {
        var existingMovie = await _context.Movies.FindAsync(id);

        if (existingMovie is null)
        {
            return NotFound();
        }

        _context.Movies.Remove(existingMovie);

        await _context.SaveChangesAsync();

        return Ok();
    }
}