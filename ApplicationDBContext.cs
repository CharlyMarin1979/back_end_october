using back_end.Entidades;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end
{
    public class ApplicationDBContext : IdentityDbContext
    {
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<PeliculasActores>().HasKey(x => new { x.ActorId, x.PeliculaId});
            builder.Entity<PeliculasGeneros>().HasKey(x => new { x.GeneroId, x.PeliculaId });
            builder.Entity<PeliculasCines>().HasKey(x => new { x.CineId, x.PeliculaId });

            base.OnModelCreating(builder);
        }

        public DbSet<Genero> Generos { get; set; }
        public DbSet<Actor> Actores { get; set; }
        public DbSet<Cine> Cines { get; set; }
        public DbSet<Pelicula> Peliculas { get; set; }
        public DbSet<PeliculasActores> PeliculasActores { get; set; }
        public DbSet<PeliculasGeneros> PeliculasGeneros { get; set; }
        public DbSet<PeliculasCines> PeliculasCines { get; set; }

    }
}
