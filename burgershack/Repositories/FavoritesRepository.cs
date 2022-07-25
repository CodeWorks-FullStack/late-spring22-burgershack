using System.Data;
using burgershack.Models;
using Dapper;

namespace burgershack.Repositories
{
  public class FavoritesRepository
  {
    private readonly IDbConnection _db;

    public FavoritesRepository(IDbConnection db)
    {
      _db = db;
    }

    internal Favorite FindExisting(Favorite favoriteData)
    {
      string sql = "SELECT * FROM favorites WHERE burgerId = @BurgerId AND accountId = @AccountId";
      return _db.QueryFirstOrDefault<Favorite>(sql, favoriteData);
    }

    internal Favorite Create(Favorite favoriteData)
    {
      string sql = @"
      INSERT INTO favorites
      (accountId, burgerId)
      VALUES
      (@AccountId, @BurgerId);
      SELECT LAST_INSERT_ID();
      ";
      int id = _db.ExecuteScalar<int>(sql, favoriteData);
      favoriteData.Id = id;
      return favoriteData;
    }

    internal void Delete(int id)
    {
      string sql = "DELETE FROM favorites WHERE id = @id LIMIT 1";
      _db.Execute(sql, new { id });
    }
  }
}