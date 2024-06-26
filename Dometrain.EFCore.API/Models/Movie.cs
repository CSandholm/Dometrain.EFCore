using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Dometrain.EFCore.API.Models;

public class Movie
{

    public int Identifier { get; set; }
    public string? Title { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string? Synopsis { get; set; }
    public AgeRating AgeRating { get; set; }

    public decimal InternetRating { get; set; }

    public Genre Genre { get; set; }
    public int MainGenreId { get; set; }
    public Person Director { get; set; }
    public ICollection<Person> Actors { get; set; }
    
}

public enum AgeRating
{
    All = 0,
    ElementarySchool = 1,
    HighSchool = 2,
    Adolescent = 3,
    Adult = 4
}

public class MovieTitle
{
    public int Id { get; set; }
    public string? Title { get; set; }
}