using F1Pitwall.Models;
using System.Threading.Tasks;

namespace F1Pitwall.Processor
{
    public interface IFrameHandler
    {
        Task HandleAsync(Frame frame, SessionContext context);
    }
}
