using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Entidades
{
    public class Rating
    {
        public int Id { get; set; }
        public int Puntuacion { get; set; }
        public int PeliculaId { get; set; }

    }
}
