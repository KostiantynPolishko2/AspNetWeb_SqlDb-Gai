using _20240723_SqlDb_Gai.Database;
using _20240723_SqlDb_Gai.Models;

namespace _20240723_SqlDb_Gai.Repository
{
    public interface IColorItemRepository
    {
        public CarContext getContext();
        public IEnumerable<ColorItem> getAllColorItems();
    }
}
