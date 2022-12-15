using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PizzaOrder.Helpers;
using PizzaOrder.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;

namespace PizzaOrder.Context
{
    public static class Seed
    {
        private static int createdUserId = 1;
     

        public static void SeedUsers(DataContext context)
        {
            if (!context.Users.Any())
            {
                var fileData = System.IO.File.ReadAllText("Context/UserSeedData.json");
                var users = JsonConvert.DeserializeObject<List<User>>(fileData);

                for (int i = 0; i < users.Count; i++)
                {
                    User user = users[i];
                    user.UserName = user.UserName.ToLower();
                    user.Email = user.UserName.ToLower() + "@fabintel.com";
                    user.Active = true;
                    byte[] passwordhash, passwordSalt;
                    CreatePasswordHash("password", out passwordhash, out passwordSalt);
                    user.PasswordHash = passwordhash;
                    user.PasswordSalt = passwordSalt;                  
                    user.DateofBirth = DateTime.UtcNow;                  
                    user.FullName = user.UserName;
                    user.UserTypeId = (int)Enums.UserTypeId.Admin;
                    //user.CreatedDateTime = DateTime.UtcNow;
                 
                    context.Users.Add(user);
                    context.SaveChanges();
                    createdUserId = user.Id;
                }

            }
        }
    
        public static IEnumerable<(T item, int index)> ReturnIndex<T>(this IEnumerable<T> self)
        {
            return self.Select((item, index) => (item, index));
        }

        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            byte[] key = new Byte[64];
            using (HMACSHA512 hmac = new HMACSHA512(key))
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));


                // var hmac = System.Security.Cryptography.HMACSHA512()
            }

        }

        public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
                return true;
            }

        }
    }
}
