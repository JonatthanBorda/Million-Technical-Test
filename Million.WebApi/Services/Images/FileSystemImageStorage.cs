using System.Text.RegularExpressions;

namespace Million.WebApi.Services.Images
{
    public sealed class FileSystemImageStorage : IImageStorage
    {
        private readonly IWebHostEnvironment _env;

        public FileSystemImageStorage(IWebHostEnvironment env) => _env = env;

        public async Task<string> SaveAsync(IFormFile file, CancellationToken ct)
        {
            if (file is null || file.Length == 0)
                throw new InvalidOperationException("Archivo vacío.");

            //Valida mimetype simple:
            var allowed = new[] { "image/jpeg", "image/png", "image/webp" };
            if (!allowed.Contains(file.ContentType))
                throw new InvalidOperationException("Tipo de archivo no permitido.");

            var dir = Path.Combine(_env.WebRootPath ?? "wwwroot", "images");
            Directory.CreateDirectory(dir);

            var name = $"{Guid.NewGuid():N}{Path.GetExtension(file.FileName)}";
            var path = Path.Combine(dir, name);

            using var fs = File.Create(path);
            await file.CopyToAsync(fs, ct);

            //Devuelve ruta relativa que se guarda en BD:
            return $"/images/{name}";
        }

        public Task DeleteAsync(string path, CancellationToken ct)
        {
            var phys = path;
            if (path.StartsWith('/'))
                phys = path.TrimStart('/');

            phys = Path.Combine(_env.WebRootPath ?? "wwwroot", phys.Replace('/', Path.DirectorySeparatorChar));

            if (File.Exists(phys))
                File.Delete(phys);

            return Task.CompletedTask;
        }
    }
}
