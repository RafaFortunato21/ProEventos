using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ProEventos.Application.Dtos;

namespace ProEventos.Application.Contratos
{
    public interface IAccountService
    {
        Task<bool> UserExists(string username);
        Task<UserUpdateDto> GetUserByUserNameAsync(string username);

        Task<SignInResult> ChekUserPasswordAsync(UserUpdateDto userUpdateDto, string password);

        Task<UserUpdateDto> CreateAccountAsync(UserUpdateDto userUpdateDto);

        Task<UserUpdateDto> UpdateAccount(UserUpdateDto userUpdateDto);

        


    }
}