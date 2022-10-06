using back_end.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using back_end.Filtros;
using Microsoft.EntityFrameworkCore;
using back_end.DTOs;
using AutoMapper;
using back_end.Utilidades;

namespace back_end.Controllers
{
    [Route("api/generos")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GenerosController: ControllerBase
    {
        private readonly ILogger<GenerosController> logger;
        private readonly ApplicationDBContext context;
        private readonly IMapper mapper;

        public GenerosController(ILogger<GenerosController> logger, ApplicationDBContext context, IMapper mapper)
        {
            this.logger = logger;
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<GeneroDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.Generos.AsQueryable();
            await HttpContext.InsertarParametrosPagiinacionEnCabecera(queryable);
            var generos = await queryable.OrderBy(x => x.Nombre).Paginar(paginacionDTO).ToListAsync();
            return mapper.Map<List<GeneroDTO>>(generos);

        }

        [HttpGet("{id:int}")] // api/generos/1
        public async Task<ActionResult<GeneroDTO>> Get(int id)
        {
            var genero = await context.Generos.FirstOrDefaultAsync(x => x.Id == id);
            if (genero == null)
            {
                return NotFound();
            }
            return mapper.Map<GeneroDTO>(genero);
        }

        [HttpPost]
        public async  Task<ActionResult> Post([FromBody] GeneroCreacionDTO generoCreacionDTO)
        {
            //throw new NotImplementedException();
            var genero = mapper.Map<Genero>(generoCreacionDTO);
            context.Add(genero);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] GeneroCreacionDTO generoCreacionDTO)
        {
            var genero = await context.Generos.FirstOrDefaultAsync(x => x.Id == id);
            if (genero == null)
            {
                return NotFound();
            }
            genero = mapper.Map(generoCreacionDTO, genero);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Generos.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Genero() { Id = id });
            await context.SaveChangesAsync();
            return NoContent();

        }
    }
}
