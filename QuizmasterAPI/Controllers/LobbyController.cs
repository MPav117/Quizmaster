using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quizmaster.Interfaces;
using Quizmaster.Models;
using Quizmaster.Services;

namespace Quizmaster.Controllers
{
    [ApiController]
    [Route("API/Lobby")]

    public class LobbyController : Controller
    {
        private readonly ILobbyService _lobbyService;

        public LobbyController(ILobbyService lobbyService)
        {
            _lobbyService = lobbyService;
        }

        [HttpGet("GetLobby/{id}")]
        public async Task<ActionResult> GetLobby(int id)
        {
            var result = await _lobbyService.GetLobby(id);
            if (result.IsError == false)
            {
                return Ok(result.Value);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost("CreateLobby")]
        public async Task<ActionResult> CreateLobby([FromBody] Lobby newLobby)
        {
            var result = await _lobbyService.CreateLobby(newLobby);
            if (result.IsError == false)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet("GetLobbyList")]
        public async Task<ActionResult> GetLobbyList()
        {
            var result = await _lobbyService.GetLobbyList();

            if (result.IsError == false)
            {
                return Ok(result.Value);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPut("UpdateLobby")]
        public async Task<ActionResult> UpdateLobby([FromBody] Lobby updatedLobby)
        {
            var result = await _lobbyService.UpdateLobby(updatedLobby);

            if (result.IsError == false)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpDelete("DeleteLobby/{id}")]
        public async Task<ActionResult> DeleteLobby(int id)
        {
            var result = await _lobbyService.DeleteLobby(id);

            if (result.IsError == false)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet("GetLobbySessions/{id}")]
        public async Task<ActionResult> GetLobbySessions(int id)
        {
            var result = await _lobbyService.GetLobbySessions(id);

            if (result.IsError == false)
            {
                return Ok(result.Value);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}