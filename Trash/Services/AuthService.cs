using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trash.Data.Repositories;
using Trash.Data.DataContext;
using Trash.Models.ContextModels;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;

namespace Trash.Services
{
    public class AuthService : Repository<User>
    {
        private readonly DataContext _Context;
        private IQueryable<User> TableNoTracking => _Context.Set<User>().AsNoTracking(); 
        private readonly JwtService jwtService;

        public AuthService(DataContext dbContext, JwtService jwt)
            : base(dbContext)
        {
            _Context = dbContext;
            jwtService = jwt;
        }

        public async Task AddUser(User user,string password)
        {
            byte[] PasswordSalt = new byte[1024 / 8];
            PasswordSalt = Encoding.UTF8.GetBytes("#1184$+" + user.Phone);
            byte[] PasswordHash = CreatePasswordHash(password,PasswordSalt);
            user.PasswordHash = PasswordHash;
            user.PasswordSalt = PasswordSalt;
            await Add(user);
        }

        public async Task<bool> CheckUserExist(string PhoneNumber)
        {
            var user = await TableNoTracking.AnyAsync(x => x.Phone == PhoneNumber);
            return user;
        }

        public async Task<User> GetByUserAndPass(string username,string password)
        {
            try
            {
                var UserByUsername = await _Table.Where(x => x.UserName == username).FirstAsync();
                byte[] PassCheck = CreatePasswordHash(password, UserByUsername.PasswordSalt);
                string pass1 = Encoding.Unicode.GetString(PassCheck, 0, PassCheck.Length);
                string userpass = Encoding.Unicode.GetString(UserByUsername.PasswordHash, 0, UserByUsername.PasswordHash.Length);
                if (pass1 == userpass)
                    return UserByUsername;
                return null;
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public string GenerateToken(User user)
        {
            try
            {
                return jwtService.Generate(user);
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        private byte[] CreatePasswordHash(string Password,byte[] salt)
        {
            byte[] PasswordHashed = KeyDerivation.Pbkdf2(
                password: Password,
                salt:salt,
                prf:KeyDerivationPrf.HMACSHA512,
                iterationCount:10000,
                numBytesRequested:1024 / 8
                );
            return PasswordHashed;
        }
    }
}
