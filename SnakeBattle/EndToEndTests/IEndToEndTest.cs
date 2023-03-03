using System.Threading.Tasks;

namespace EndToEndTests
{
    interface IEndToEndTest
    {
        string Category { get; }
        string Description { get; }
        Task Run();
    }
}
