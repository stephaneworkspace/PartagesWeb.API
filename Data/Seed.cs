﻿//-----------------------------------------------------------------------
// <license>https://github.com/stephaneworkspace/PartagesWeb.API/blob/master/LICENSE.md</license>
// <author>Stéphane</author>
//-----------------------------------------------------------------------
using Newtonsoft.Json;
using PartagesWeb.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        /// <remarks>
        /// Don't work with IIS Express
        /// </remarks>
        public void SeedUsers()
        {
            var userData = System.IO.File.ReadAllText("Data/Seed/UsersSeedData.json");
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

        ///<summary>
        ///Cette méthode permet d'ajouter les sections dans la base de donnée
        ///</summary>
        /// <remarks>
        /// Don't work with IIS Express
        /// </remarks>///
        public async void SeedSections()
        {
            var sectionData = System.IO.File.ReadAllText("Data/Seed/SectionsSeedData.json", Encoding.GetEncoding("iso-8859-1"));
            var sections = JsonConvert.DeserializeObject<List<Section>>(sectionData);
            foreach (var section in sections)
            {
                if (!_context.Sections.Any(x => x.Nom.ToLower() == section.Nom.ToLower()))
                {
                    // Déterminer la dernière position en ligne ou hors ligne
                    var position = await LastPositionSection(section.SwHorsLigne);
                    // Prochaine position
                    position++;
                    section.Position = position;
                    _context.Sections.Add(section);
                    await _context.SaveChangesAsync();
                }
            }
        }

        ///<summary>
        ///Cette méthode permet d'ajouter les icones dans la base de donnée
        ///</summary>
        /// <remarks>
        /// Don't work with IIS Express
        /// </remarks>///
        public async void SeedIcones()
        {
            var data = System.IO.File.ReadAllText("Data/Seed/IconesSeedData.json", Encoding.GetEncoding("iso-8859-1"));
            var icones = JsonConvert.DeserializeObject<List<Icone>>(data);
            foreach (var icone in icones)
            {
                if (!_context.Icones.Any(x => x.NomSelectBox.ToLower() == icone.NomSelectBox.ToLower()))
                {
                    _context.Icones.Add(icone);
                    await _context.SaveChangesAsync();
                }
            }
        }

        /// <summary>  
        /// Cette méthode permet de créer mot de passe "Hash"
        /// </summary> 
        /// <remarks>
        /// Copier collé AuthRepository
        /// </remarks>
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
        /// Cette méthode permet de détermine la dernière position
        /// </summary>  
        /// <remarks>
        /// 9 février, je n'ai pas testé le OrderByDescending que j'ai rajouté
        /// Copier Coller GestionPages Repository
        /// </remarks>
        /// <param name="swHorsLigne"> Switch si on est en ligne true ou hors ligne false</param>
        private async Task<int> LastPositionSection(bool swHorsLigne)
        {
            int lastPositon = _context.Sections
                .Where(x => swHorsLigne == x.SwHorsLigne)
                .OrderByDescending(x => x.Position)
                .Select(p => p.Position)
                .DefaultIfEmpty(0)
                .Max();
            await Task.FromResult(lastPositon);
            return lastPositon;
        }
    }
}
