using PlatFormService.Models;

namespace PlatFormService.Data
{
    public interface IPlatformRepo
    {
        bool SaveChanges();
        IEnumerable<Platform> GetAllPlatforms();
        Platform? GetPlatFormById(int id);
        void CreatePlatform(Platform platform);
    }
}