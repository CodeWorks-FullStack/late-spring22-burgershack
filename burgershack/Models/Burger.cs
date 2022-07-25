using burgershack.Interfaces;

namespace burgershack.Models
{
  public class Burger : RepoItem<int>, ICreated
  {
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public string CreatorId { get; set; }
    public Profile Creator { get; set; }
  }

  public class BurgerFavoriteViewModel : Burger
  {
    public int FavoriteId { get; set; }
  }
}