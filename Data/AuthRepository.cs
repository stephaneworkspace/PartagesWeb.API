//-----------------------------------------------------------------------
// <license>https://github.com/stephaneworkspace/PartagesWeb.API/blob/master/LICENSE.md</license>
// <author>Stéphane</author>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PartagesWeb.API.Models;

namespace PartagesWeb.API.Data
{
    /// <summary>
    /// Repository pour l'authentification
    /// </summary>
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;


        /// <summary>  
        /// Cette méthode est le constructeur 
        /// </summary>  
        /// <param name="context"> DataContext</param>
        public AuthRepository(DataContext context)
        {
            _context = context;
        }

        /// <summary>  
        /// Cette méthode permet le login
        /// </summary>  
        /// <param name="nom"> Nom d'utilisateur</param>
        /// <param name="motDePasse"> Mot de passe</param>
        public async Task<User> Login(string nom, string motDePasse)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == nom);
            if (user == null)
            {
                return null;
            }
            if (!VerifyPasswordHash(motDePasse, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }
            return user;
        }

        /// <summary>  
        /// Cette méthode permet de vérifier le mot de passe
        /// </summary>  
        /// <param name="password"> Mot de passe</param>
        /// <param name="passwordHash"> Hash</param>
        /// <param name="passwordSalt"> Salt</param>
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
            }
            return true;
        }

        /// <summary>  
        /// Cette méthode permet l'inscription
        /// </summary>  
        /// <remarks>
        /// 8 Février : 
        /// Pour le moment [Authorize] est un utilisateur administrateur
        /// </remarks>
        /// <param name="user"> Model user</param>
        /// <param name="password"> Mot de passe</param>
        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
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

        /// <summary>  
        /// Cette méthode permet de savoir si l'utilisateur existe
        /// </summary>  
        /// <param name="nom"> Mot de passe</param>
        public async Task<bool> UserExists(string nom)
        {
            if (await _context.Users.AnyAsync(x => x.Username == nom))
                return true;

            return false;
        }
    }
}
