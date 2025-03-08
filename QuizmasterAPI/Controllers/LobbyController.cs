using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quizmaster.Interfaces;

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
    }
}