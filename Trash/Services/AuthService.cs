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
using Trash.Models.TransferModels;

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

        public async Task<User> AddUser(User user,string password)
        {
            byte[] PasswordSalt = new byte[1024 / 8];
            PasswordSalt = Encoding.UTF8.GetBytes("#1184$+" + user.Phone);
            byte[] PasswordHash = CreatePasswordHash(password,PasswordSalt);
            user.PasswordHash = PasswordHash;
            user.PasswordSalt = PasswordSalt;
            return await Add(user);
        }

        public async Task<NewUserRequest> UpdateUser(long id, NewUserRequest updateUser)
        {
            var User = await _Context.Set<User>().FindAsync(id);
            if (updateUser.Password == null || updateUser.Password == "")
            {
                if (updateUser.Age >= 0)
                    User.Age = updateUser.Age;
                if (updateUser.Email != null)
                    User.Email = updateUser.Email;
                User.Gender = updateUser.Gender;
                if (updateUser.Name != null)
                    User.Name = updateUser.Name;
                if (updateUser.Phone != null)
                    User.Phone = updateUser.Phone;
                if (updateUser.UserName != null)
                    User.UserName = updateUser.UserName;
            }
            else
            {
                byte[] PasswordSalt = new byte[1024 / 8];
                if (updateUser.Phone != null)
                    PasswordSalt = Encoding.UTF8.GetBytes("#1184$+" + User.Phone);
                else
                    PasswordSalt = Encoding.UTF8.GetBytes("#1184$+" + updateUser.Phone);
                byte[] PasswordHash = CreatePasswordHash(updateUser.Password, PasswordSalt);
                User.PasswordHash = PasswordHash;
                User.PasswordSalt = PasswordSalt;
                if (updateUser.Age >= 0)
                    User.Age = updateUser.Age;
                if (updateUser.Email != null)
                    User.Email = updateUser.Email;
                User.Gender = updateUser.Gender;
                if (updateUser.Name != null)
                    User.Name = updateUser.Name;
                if (updateUser.Phone != null)
                    User.Phone = updateUser.Phone;
                if (updateUser.UserName != null)
                    User.UserName = updateUser.UserName;
            }
            _Context.Set<User>().Update(User);
            return updateUser;
        }

        public async Task<bool> CheckUserExist(string PhoneNumber,string UserName)
        {
            var phone = await TableNoTracking.AnyAsync(x => x.Phone == PhoneNumber);
            if (phone)
                return phone;
            var username = await TableNoTracking.AnyAsync(x => x.UserName == UserName);
            return username;
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
