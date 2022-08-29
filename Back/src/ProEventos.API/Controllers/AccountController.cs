using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Extensions;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;

namespace ProEventos.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ITokenService _tokenService;

        public AccountController(IAccountService accountService,
                                    ITokenService tokenService)
        {
            _accountService = accountService;
            _tokenService = tokenService;
        }

        [HttpGet("GetUser")]

        public async Task<IActionResult> GetUsers(){
            try
            {
                
                var userName = User.GetUserName();
                var user = await _accountService.GetUserByUserNameAsync(userName);

                 return Ok(user);
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar usuário. Erro: {ex.Message} ");
            }

        }


        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserUpdateDto userDto){
            try
            {
                if (await _accountService.UserExists(userDto.UserName) ) return BadRequest("Usuário já existe");
                var user = await _accountService.CreateAccountAsync(userDto) ;
                
                if (user != null )
                {
                   return Ok(
                    new {
                        userName = user.UserName,
                        primeiroNome = user.PrimeiroNome,
                        token = _tokenService.CreateToken(user).Result

                    }
                 );
                }
                    

                return BadRequest("Usuário não criado, tente novamente mais tarde.");

            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar usuário. Erro: {ex.Message} ");
            }

        }


        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto userLogin){
            try
            {
                var user = await _accountService.GetUserByUserNameAsync(userLogin.Username);

                if (user == null) return Unauthorized("Usuário não identificado.");

                var result =  await _accountService.ChekUserPasswordAsync(user, userLogin.Password );

                if (!result.Succeeded) return Unauthorized("Senha inválida.");

                 return Ok(
                    new {
                        userName = user.UserName,
                        primeiroNome = user.PrimeiroNome,
                        token = _tokenService.CreateToken(user).Result

                    }
                 );


            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar usuário. Erro: {ex.Message} ");
            }

        }


        [HttpPut("UpdateUser")]
        
        public async Task<IActionResult> UpdateUser(UserUpdateDto userUpdateDto){
            try
            {
                if (userUpdateDto.UserName != User.GetUserName())
                    return Unauthorized("usuario inválido");
                    
                

                var user = await _accountService.GetUserByUserNameAsync(User.GetUserName());
                if (user == null) return Unauthorized("Usuário não identificado.");
                
                var userReturn = await _accountService.UpdateAccount(userUpdateDto);
                if (userReturn == null ) return NoContent();
                    
                return Ok(  new 
                {
                    userName = userReturn.UserName,
                    primeiroNome = userReturn.PrimeiroNome,
                    token = _tokenService.CreateToken(userReturn).Result
                });

            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar atualizar usuário. Erro: {ex.Message} ");
            }

        }


    }
}