using System;
using System.Collections.Generic;
using burgershack.Models;
using burgershack.Repositories;

namespace burgershack.Services
{
  public class BurgersService
  {
    private readonly BurgersRepository _repo;

    public BurgersService(BurgersRepository repo)
    {
      _repo = repo;
    }

    internal List<Burger> Get()
    {
      return _repo.GetAll();
    }

    internal Burger Get(int id)
    {
      Burger found = _repo.GetById(id);
      if (found == null)
      {
        throw new Exception("Invalid Id");
      }
      return found;
    }

    internal Burger Create(Burger burgerData)
    {
      return _repo.Create(burgerData);
    }

    internal Burger Edit(Burger burgerData)
    {
      Burger original = Get(burgerData.Id);
      if (original.CreatorId != burgerData.CreatorId)
      {
        throw new Exception("Invalid Access");
      }
      original.Name = burgerData.Name ?? original.Name;
      original.Description = burgerData.Description ?? original.Description;
      original.Price = burgerData.Price > 0 ? burgerData.Price : original.Price;

      _repo.Edit(original);
      return original;
    }

    internal Burger Delete(int id, string userId)
    {
      Burger original = Get(id);
      if (original.CreatorId != userId)
      {
        throw new Exception("Invalid Access");
      }
      _repo.Delete(id);
      return original;
    }

    internal List<BurgerFavoriteViewModel> GetFavoritesByAccountId(string userId)
    {
      return _repo.GetFavoritesByAccountId(userId);
    }
  }
}