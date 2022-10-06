using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Utilidades
{
    public class ArchivosLocales : IAlmacenadorArchivos
    {
        private readonly IWebHostEnvironment env;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ArchivosLocales(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            this.env = env;
            this.httpContextAccessor = httpContextAccessor;
        }
        public Task BorrarArchivo(string ruta, string contenedor)
        {

            if (string.IsNullOrEmpty(ruta)){
                return Task.CompletedTask;
            }
            var nombreArchivo = Path.GetFileName(ruta);
            var directorioArchivo = Path.Combine(env.WebRootPath, contenedor, nombreArchivo);

            if (File.Exists(directorioArchivo))
            {
                File.Delete(directorioArchivo);
            }

            return Task.CompletedTask;
        }

        public async Task<string> EditarArchivo(string contenedor, IFormFile archivo, string ruta)
        {
            await BorrarArchivo(ruta, contenedor);
            return await GuardarArchivo(contenedor, archivo);



        }

        public async Task<string> GuardarArchivo(string contenedor, IFormFile archivo)
        {
            var extension = Path.GetExtension(archivo.FileName);
            var archivoNombre = $"{Guid.NewGuid()}{extension}";
            string folder = Path.Combine(env.WebRootPath, contenedor);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string ruta = Path.Combine(folder, archivoNombre);
            using (var memoryStream = new MemoryStream())
            {
                await archivo.CopyToAsync(memoryStream);
                var contenido = memoryStream.ToArray();
                await File.WriteAllBytesAsync(ruta, contenido);
            }

            var urlActual = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}";
            var rutaParaDB = Path.Combine(urlActual, contenedor, archivoNombre).Replace("\\", "/");
            return rutaParaDB;

        }
    }
}
