using System.Threading.Tasks;

namespace Peernet.Browser.Application.Extensions
{
    public static class TaskExtensions
    {
        public static T GetResultBlockingWithoutContextSynchronization<T>(this Task<T> task)
        {
            return task.ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}