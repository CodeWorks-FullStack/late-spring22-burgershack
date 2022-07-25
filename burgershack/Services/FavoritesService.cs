using System;
using burgershack.Models;
using burgershack.Repositories;

namespace burgershack.Services
{
  public class FavoritesService
  {
    private readonly FavoritesRepository _repo;

    public FavoritesService(FavoritesRepository repo)
    {
      _repo = repo;
    }
    internal Favorite GetById(int id)
    {
      Favorite found = _repo.Get(id);
      if (found == null)
      {
        throw new Exception("Invalid Id");
      }
      return found;
    }

    internal Favorite Create(Favorite favoriteData)
    {
      // PREVENT DUPLICATE MANY TO MANY (HINT)
      Favorite exists = _repo.FindExisting(favoriteData);
      if (exists != null)
      {
        return exists;
      }
      return _repo.Create(favoriteData);
    }

    internal void Delete(int id, string userId)
    {
      Favorite toDelete = GetById(id);
      if (toDelete.AccountId != userId)
      {
        throw new Exception("Invalid Action");
      }
      _repo.Delete(id);
    }
  }
}