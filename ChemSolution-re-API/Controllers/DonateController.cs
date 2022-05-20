using ChemSolution_re_API.Data;
using ChemSolution_re_API.Entities;
using ChemSolution_re_API.Services.Pay;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MlkPwgen;

namespace ChemSolution_re_API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class DonateController : ControllerBase
  {
    private readonly IPayService _payService;
    private readonly DataContext _context;

    public DonateController(IPayService payService, DataContext context)
    {
      _payService = payService;
      _context = context;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<PayButtonModel>> GetDonate(int amount)
    {
      var orderId = PasswordGenerator.Generate(length: 10, allowed: Sets.Alphanumerics);

      var user = await _context.Users.SingleOrDefaultAsync(u => u.Id.ToString() == HttpContext.User.Identity!.Name);

      if (user == null)
      {
        return NotFound("User not found");
      }

      var payButton = _payService.GetPayButton(new PayOptions()
      {
        Amount = amount.ToString(),
        Currency = "UAH",
        Description = "Get game currency",
        Version = "3",
        OrderId = orderId
      });
      _context.Orders.Add(new Order()
      {
        OrderId = orderId,
        CoinsAmount = amount * 2,
        Data = payButton.Data,
        Signature = payButton.Signature,
        User = user
      });
      await _context.SaveChangesAsync();
      return payButton;
    }

    public class LiqPayAnswer
    {
      public string Data { set; get; } = string.Empty;
      public string Signature { set; get; } = string.Empty;
    }

    [HttpPost]
    public async Task<ActionResult> Post(LiqPayAnswer answer)
    {
      var order = await _context.Orders.Include(o => o.User).SingleOrDefaultAsync(o =>
            o.Data == answer.Data && o.Signature == answer.Signature);
      if (order != null)
      {
        // do later checking status
        order.User!.Balance += order.CoinsAmount;
        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
        return Ok();
      }
      return BadRequest();
    }

    [Authorize]
    [HttpPost("buy/element/{id}")]
    public async Task<ActionResult> BuyElement(int id)
    {
      var user = await _context.Users
          .Include(u => u.Elements)
          .SingleOrDefaultAsync(u => u.Id.ToString() == User.Identity!.Name);
      var element = await _context.Elements.FindAsync(id);
      if (user != null && element != null)
      {
        if (user.Balance >= element.Price && !user.Elements.Contains(element))
        {
          user.Balance -= element.Price;
          user.Elements.Add(element);
          await _context.SaveChangesAsync();
          return Ok();
        }
        return BadRequest("Not enough money or already bought");
      }
      return NotFound("Element and User not found");
    }
  }
}
