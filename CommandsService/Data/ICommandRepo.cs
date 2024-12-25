using CommandsService.Models;

namespace CommandsService.Data{
    public interface ICommandRepo{

        bool SaveChanges();
        IEnumerable<Platform> GetAllPlatforms();
        void CreatePlatform(Platform plat);
        bool PlatformExist(int platformId);
        bool ExternalPlatformExists(int ExternalPlatformId);

        IEnumerable<Command> GetCommandsForPlatform(int platformId);
        Command? GetCommand(int platformId, int commandId);
        void CreateCommand(int platformId, Command command);
    }
}