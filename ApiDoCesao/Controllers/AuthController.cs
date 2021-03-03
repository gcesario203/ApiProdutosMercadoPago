﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ApiDoCesao.Data;
using ApiDoCesao.Data.Business;
using ApiDoCesao.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiDoCesao.Controllers
{
    [Route("login")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private AuthBusiness _authBusiness;

        public AuthController(MongoDb dbContext)
        {
            _authBusiness = new AuthBusiness(dbContext);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Autenticar([FromBody] AuthDto pAuth)
        {
            try
            {
                var auth = _authBusiness.AutenticarUsuario(pAuth);

                return Ok(auth);
            }
            catch(Exception err)
            {
                return StatusCode(400, err.Message);
            }
        }
    }
}
