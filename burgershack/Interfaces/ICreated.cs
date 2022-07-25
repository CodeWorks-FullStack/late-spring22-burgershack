using burgershack.Models;

namespace burgershack.Interfaces
{
  public interface ICreated
  {
    string CreatorId { get; set; }
    Profile Creator { get; set; }
  }
}