using System.Threading.Tasks;

namespace Persistence.FileConfigurations.TemplateEngine
{
    public interface IRazorEngine
    {
        Task<string> ParseAsync<TModel>(string viewName, TModel model);
    }
}
