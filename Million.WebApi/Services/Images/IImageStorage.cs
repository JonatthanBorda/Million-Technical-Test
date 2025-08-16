namespace Million.WebApi.Services.Images
{
    /// <summary>
    /// Abstracción de almacenamiento de imágenes.
    /// </summary>
    public interface IImageStorage
    {
        Task<string> SaveAsync(IFormFile file, CancellationToken ct);
        Task DeleteAsync(string path, CancellationToken ct);
    }
}
