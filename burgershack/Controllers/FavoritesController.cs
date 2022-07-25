using System;
using System.Threading.Tasks;
using burgershack.Models;
using burgershack.Services;
using CodeWorks.Auth0Provider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace burgershack.Controllers
{
  [ApiController]
  [Authorize]
  [Route("api/[controller]")]
  public class FavoritesController : ControllerBase
  {
    private readonly FavoritesService _fs;

    public FavoritesController(FavoritesService fs)
    {
      _fs = fs;
    }

    [HttpPost]
    public async Task<ActionResult<Favorite>> Create([FromBody] Favorite favoriteData)
    {
      try
      {
        Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
        favoriteData.AccountId = userInfo.Id;
        Favorite newFavorite = _fs.Create(favoriteData);
        return Ok(newFavorite);
      }
      catch (Exception error)
      {
        return BadRequest(error.Message);
      }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Favorite>> Delete(int id)
    {
      try
      {
        Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
        _fs.Delete(id, userInfo.Id);
        return Ok("Deleted");
      }
      catch (Exception error)
      {
        return BadRequest(error.Message);
      }

    }
  }
}