﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlazingChat.Server.Controllers
{
    [Authorize]
    public abstract class BaseController : ControllerBase
    {
        private int _userId { get; set; }

        public int UserId
        {
            get
            {
                if (_userId == 0)
                {
                    _userId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
                }
                return _userId;
            }
        }
    }
}
