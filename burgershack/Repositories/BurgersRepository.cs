using System.Collections.Generic;
using System.Data;
using System.Linq;
using burgershack.Interfaces;
using burgershack.Models;
using Dapper;

namespace burgershack.Repositories
{
  public class BurgersRepository : IRepo<Burger>
  {
    private readonly IDbConnection _db;

    public BurgersRepository(IDbConnection db)
    {
      _db = db;
    }
    public List<Burger> GetAll()
    {
      string sql = @"
      SELECT
        a.*,
        b.*
      FROM burgers b
      JOIN accounts a ON a.id = b.creatorId";
      return _db.Query<Profile, Burger, Burger>(sql, (prof, burg) =>
      {
        burg.Creator = prof;
        return burg;
      }).ToList();
    }

    public Burger GetById(int id)
    {
      string sql = @"
      SELECT
        a.*,
        b.*
      FROM burgers b
      JOIN accounts a ON a.id = b.creatorId
      WHERE b.id = @id";
      return _db.Query<Profile, Burger, Burger>(sql, (prof, burg) =>
      {
        burg.Creator = prof;
        return burg;
      }, new { id }).FirstOrDefault();
    }

    public Burger Create(Burger burgerData)
    {
      string sql = @"
      INSERT INTO burgers
      (name, description, price, creatorId)
      VALUES
      (@Name, @Description, @Price, @CreatorId);
      SELECT LAST_INSERT_ID();
      ";

      int id = _db.ExecuteScalar<int>(sql, burgerData);
      burgerData.Id = id;
      return burgerData;
    }

    public void Edit(Burger original)
    {
      string sql = @"
      UPDATE burgers
      SET
        name = @Name,
        description = @Description,
        price = @Price
      WHERE id = @Id;";
      _db.Execute(sql, original);

    }

    internal List<BurgerFavoriteViewModel> GetFavoritesByAccountId(string userId)
    {
      // THE DOUBLE JOIN!!!!
      // Join and Populate (ORDER MATTERS)
      string sql = @"
      SELECT
        a.*,
        b.*,
        f.id AS FavoriteId
      FROM favorites f
      JOIN burgers b ON b.id = f.burgerId
      JOIN account a ON a.id = b.creatorId
      WHERE f.accountId = @userId;";

      return _db.Query<Account, BurgerFavoriteViewModel, BurgerFavoriteViewModel>(sql, (prof, burger) =>
      {
        burger.Creator = prof;
        return burger;
      }, new { userId }).ToList();






    }

    public void Delete(int id)
    {
      string sql = "DELETE FROM burgers WHERE id = @id LIMIT 1";
      _db.Execute(sql, new { id });
    }

  }
}