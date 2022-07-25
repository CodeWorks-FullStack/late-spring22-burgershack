using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using burgershack.Models;
using burgershack.Services;
using CodeWorks.Auth0Provider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace burgershack.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class BurgersController : ControllerBase
  {
    private readonly BurgersService _bs;

    public BurgersController(BurgersService bs)
    {
      _bs = bs;
    }

    [HttpGet]
    public ActionResult<List<Burger>> Get()
    {
      try
      {
        List<Burger> burgers = _bs.Get();
        return Ok(burgers);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    [HttpGet("{id}")]
    public ActionResult<Burger> Get(int id)
    {
      try
      {
        Burger burger = _bs.Get(id);
        return Ok(burger);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }


    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Burger>> Create([FromBody] Burger burgerData)
    {
      try
      {
        Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
        burgerData.CreatorId = userInfo.Id;
        Burger newBurger = _bs.Create(burgerData);
        // Manually handle the Populate (prevents creator: null)
        newBurger.Creator = userInfo;
        newBurger.CreatedAt = new DateTime();
        newBurger.UpdatedAt = new DateTime();
        return Ok(newBurger);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult<Burger>> Edit(int id, [FromBody] Burger burgerData)
    {
      try
      {
        Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
        burgerData.CreatorId = userInfo.Id;
        burgerData.Id = id;
        Burger update = _bs.Edit(burgerData);
        // Manually handle the Populate (prevents creator: null)
        update.UpdatedAt = new DateTime();
        return Ok(update);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult<Burger>> Delete(int id)
    {
      try
      {
        Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
        Burger deleted = _bs.Delete(id, userInfo.Id);
        return Ok(deleted);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }


  }
}