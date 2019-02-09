//-----------------------------------------------------------------------
// <license>https://github.com/stephaneworkspace/PartagesWeb.API/blob/master/LICENSE.md</license>
// <author>Stéphane</author>
//-----------------------------------------------------------------------
using Newtonsoft.Json;
using PartagesWeb.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartagesWeb.API.Data
{
    /// <summary>
    /// Seed de fichier json
    /// </summary>
    public class Seed
    {
        private readonly DataContext _context;

        /// <summary>  
        /// Cette méthode permet la connexion DbContext
        /// </summary>  
        /// <param name="context"> DataContext</param>
        public Seed(DataContext context)
        {
            _context = context;
        }

        /// <summary>  
        /// Cette méthode permet d'executer la création d'utilisateur dans la base de donnée
        /// </summary> 
        public void SeedUsers()
        {
            // Don't work with IIS Express
            var userData = System.IO.File.ReadAllText("Data/Seed/UserSeedData.json");
            var users = JsonConvert.DeserializeObject<List<User>>(userData);
            foreach (var user in users)
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash("password", out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.Username = user.Username.ToLower();

                _context.Users.Add(user);
            }

            _context.SaveChanges();
        }

        /// <summary>  
        /// Cette méthode permet de créer mot de passe "Hash"
        /// </summary>  
        /// <param name="password"> Mot de passe</param>
        /// <param name="passwordHash"> (Out) Hash</param>
        /// <param name="passwordSalt"> (Out) Salt</param>
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
