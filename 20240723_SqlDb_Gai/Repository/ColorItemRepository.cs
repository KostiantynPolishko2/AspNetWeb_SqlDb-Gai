using _20240723_SqlDb_Gai.Database;
using _20240723_SqlDb_Gai.Models;

namespace _20240723_SqlDb_Gai.Repository
{
    public class ColorItemRepository : IColorItemRepository
    {
        public CarContext context { get; private set; }

        public ColorItemRepository(CarContext context) {
            this.context = context;
        }

        public CarContext getContext() => context;
        public IEnumerable<ColorItem> getAllColorItems() => context.Colors.Select(x => new ColorItem(x));
    }
}
