using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class GameController(GameService gameService) : ControllerBase
{
    [HttpPost("create")]
    public IActionResult CreateTable([FromBody] int buyIn)
    {
        string code = gameService.CreateTable(buyIn);
        return Ok(new { Code = code });
    }
}