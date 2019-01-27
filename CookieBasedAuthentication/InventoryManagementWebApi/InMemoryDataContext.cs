using InventoryManagementWebApi.Models;
using System.Collections.Generic;

namespace InventoryManagementWebApi
{
    public class InMemoryDataContext
    {
        readonly IDictionary<int, Article> articleRepo;

        public InMemoryDataContext()
        {
            articleRepo = new Dictionary<int, Article>();
        }

        public IDictionary<int, Article> ArticleRepo => articleRepo;
    }
}
