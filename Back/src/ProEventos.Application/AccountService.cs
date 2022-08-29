using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Domain.Identity;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Application
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly IUserPersist _userPersist;

        public  AccountService(UserManager<User> userManager,
                              SignInManager<User> SignInManager,
                              IMapper mapper,
                              IUserPersist userPersist)
        {
            _userManager = userManager;
            _signInManager = SignInManager;
            _mapper = mapper;
            _userPersist = userPersist;
        }

        public async Task<SignInResult> ChekUserPasswordAsync(UserUpdateDto userUpdateDto, string password)
        {
            try
            {
                 var user = await _userManager.Users
                                            .SingleOrDefaultAsync(user => user.UserName == userUpdateDto.UserName.ToLower());
                
                return await _signInManager.CheckPasswordSignInAsync(user, password, false);


            }
            catch (Exception ex)
            {
                throw new Exception($" Erro ao tentar verificar o password. Error: {ex.Message}");
            }
        }

        public async Task<UserUpdateDto> CreateAccountAsync(UserUpdateDto userUpdateDto)
        {
            try
            {
                var user = _mapper.Map<User>(userUpdateDto);
                var result = await _userManager.CreateAsync(user, userUpdateDto.Password);
                
                if (result.Succeeded  )
                {
                    var userToReturn = _mapper.Map<UserUpdateDto>(user);
                    return userToReturn;
                }

                return null;

            }
            catch (Exception ex)
            {
                throw new Exception($" Erro ao tentar criar conta. Error: {ex.Message}");
            }
        }

        public async Task<UserUpdateDto> GetUserByUserNameAsync(string userName)
        {
            try
            {
                 var user = await _userPersist.GetUsersByUserNameAsync(userName);

                 if (user == null) return null;

                 var userUpdate = _mapper.Map<UserUpdateDto>(user);

                 return userUpdate;


            }
            catch (Exception ex)
            {
                throw new Exception($" Erro ao tentar localizar o usuario por Username. Error: {ex.Message}");
            }
        }

        public async Task<UserUpdateDto> UpdateAccount(UserUpdateDto userUpdateDto)
        {
            try
            {
                var user = await _userPersist.GetUsersByUserNameAsync(userUpdateDto.UserName);

                if (user == null) return null;

                userUpdateDto.Id = user.Id;

                _mapper.Map(userUpdateDto, user);


                if (userUpdateDto.Password != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    await _userManager.ResetPasswordAsync(user, token, userUpdateDto.Password);
                }


                _userPersist.Update<User>(user);

                if (await _userPersist.SaveChangesAsync())
                {
                    var userRetorno = await _userPersist.GetUsersByUserNameAsync(user.UserName);

                    return _mapper.Map<UserUpdateDto>(userRetorno);
                }

                return null;


            }
            catch (Exception ex)
            {
                throw new Exception($" Erro ao tentar atualizar o Usuário. Error: {ex.Message}");
            }
        }

        public async Task<bool> UserExists(string userName)
        {
            try
            {
                 
                return await _userManager.Users.AnyAsync(user => user.UserName == userName.ToLower());

            }
            catch (Exception ex)
            {
                throw new Exception($" Erro ao verificar se o uusário existe. Error: {ex.Message}");
            }
        }
    }
}