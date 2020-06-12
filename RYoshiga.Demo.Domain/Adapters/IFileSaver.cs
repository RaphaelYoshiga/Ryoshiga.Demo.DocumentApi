using System.IO;
using System.Threading.Tasks;

namespace RYoshiga.Demo.Domain.Adapters
{
    public interface IFileSaver
    {
        Task Save(string fileName, Stream stream);
    }
}
