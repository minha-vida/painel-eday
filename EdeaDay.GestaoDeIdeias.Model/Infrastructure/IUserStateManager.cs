using System.Web;

namespace EdeaDay.GestaoDeIdeias.Model.Infrastructure
{
    public interface IUserStateManager
    {
        void Login(string userId, HttpContextBase context);

        void LogoffCurrentUser();

        string GetCurrentUserId();
    }
}
