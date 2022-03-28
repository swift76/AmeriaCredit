using System.Threading.Tasks;

namespace IntelART.Communication
{
    public interface ISmsSender
    {
        Task SendAsync(string to, string text);
    }
}
