//-----------------------------------------------------------------------
// <license>https://github.com/stephaneworkspace/PartagesWeb.API/blob/master/LICENSE.md</license>
// <author>Stéphane</author>
//-----------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PartagesWeb.API.Dtos.GestionPages;
using PartagesWeb.API.Models;
using PartagesWeb.API.Models.Forum;
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
                CreatePasswordHash("password", out byte[] passwordHash, out byte[] passwordSalt);

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
            // Section
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
                }
                await _context.SaveChangesAsync();
            }
            // TitreMenu
            var titreMenuData = System.IO.File.ReadAllText("Data/Seed/TitreMenuSeedData.json", Encoding.GetEncoding("iso-8859-1"));
            var titreMenusDto = JsonConvert.DeserializeObject<List<TitreMenuForSeedDto>>(titreMenuData);
            foreach (var titreMenuDto in titreMenusDto)
            {
                if (!_context.TitreMenus.Any(x => x.Nom.ToLower() == titreMenuDto.Nom.ToLower()))
                {
                    Section section = _context.Sections.Where(x => x.Nom == titreMenuDto.NomSection).First();
                    // Prochaine position
                    var position = await LastPositionTitreMenu(section.Id);
                    // Prochaine position
                    position++;
                    TitreMenu titreMenu = new TitreMenu
                    {
                        SectionId = section.Id,
                        Nom = titreMenuDto.Nom,
                        Position = position
                    };
                    _context.TitreMenus.Add(titreMenu);
                    await _context.SaveChangesAsync();
                } 
            }
            // SousTitreMenu
            var sousTitreMenuData = System.IO.File.ReadAllText("Data/Seed/SousTitreMenuSeedData.json", Encoding.GetEncoding("iso-8859-1"));
            var sousTitreMenusDto = JsonConvert.DeserializeObject<List<SousTitreMenuForSeedDto>>(sousTitreMenuData);
            foreach (var sousTitreMenuDto in sousTitreMenusDto)
            {
                if (!_context.SousTitreMenus.Any(x => x.Nom.ToLower() == sousTitreMenuDto.Nom.ToLower()))
                {
                    TitreMenu titreMenu = _context.TitreMenus.Where(x => x.Nom == sousTitreMenuDto.NomTitreMenu).First();
                    // Prochaine position
                    var position = await LastPositionSousTitreMenu(titreMenu.Id);
                    // Prochaine position
                    position++;
                    SousTitreMenu sousTitreMenu = new SousTitreMenu
                    {
                        TitreMenuId = titreMenu.Id,
                        Nom = sousTitreMenuDto.Nom,
                        Position = position
                    };
                    _context.SousTitreMenus.Add(sousTitreMenu);
                    await _context.SaveChangesAsync();
                }
            }
            // Article
            var articleData = System.IO.File.ReadAllText("Data/Seed/ArticleSeedData.json", Encoding.GetEncoding("iso-8859-1"));
            var articlesDto = JsonConvert.DeserializeObject<List<ArticleForSeedDto>>(articleData);
            foreach (var articleDto in articlesDto)
            {
                if (!_context.Articles.Any(x => x.Nom.ToLower() == articleDto.Nom.ToLower()))
                {
                    SousTitreMenu sousTitreMenu = _context.SousTitreMenus.Where(x => x.Nom == articleDto.NomSousTitreMenu).First();
                    // Prochaine position
                    var position = await LastPositionArticle(sousTitreMenu.Id);
                    // Prochaine position
                    position++;
                    Article article = new Article
                    {
                        SousTitreMenuId = sousTitreMenu.Id,
                        Nom = articleDto.Nom,
                        Contenu = articleDto.Contenu,
                        Position = position
                    };
                    _context.Articles.Add(article);
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
                }
                await _context.SaveChangesAsync();
            }
        }

        ///<summary>
        ///Cette méthode permet d'ajouter les données seed du forum
        ///</summary>
        /// <remarks>
        /// Don't work with IIS Express
        /// </remarks>///
        public async void SeedForum()
        {
            // Section
            var itemData = System.IO.File.ReadAllText("Data/Seed/Forum/ForumCategorieData.json", Encoding.GetEncoding("iso-8859-1"));
            var items = JsonConvert.DeserializeObject<List<ForumCategorie>>(itemData);
            foreach (var item in items)
            {
                if (!_context.ForumCategories.Any(x => x.Nom.ToLower() == item.Nom.ToLower()))
                {
                    _context.ForumCategories.Add(item);
                }
                await _context.SaveChangesAsync();
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

        /// <summary>  
        /// Cette méthode permet de détermine la dernière position
        /// </summary>  
        /// <param name="sectionId">SectionId facultatif int?</param>
        public async Task<int> LastPositionTitreMenu(int? sectionId)
        {
            int lastPositon;
            lastPositon = _context.TitreMenus
                .Where(x => sectionId == x.SectionId)
                .OrderByDescending(x => x.Position)
                .Select(p => p.Position)
                .DefaultIfEmpty(0)
                .Max();
            await Task.FromResult(lastPositon);
            return lastPositon;
        }

        /// <summary>  
        /// Cette méthode permet de détermine la dernière position
        /// </summary>  
        /// <param name="titreMenuId">TitreMenuId facultatif int?</param>
        public async Task<int> LastPositionSousTitreMenu(int? titreMenuId)
        {
            int lastPositon;
            lastPositon = _context.SousTitreMenus
                .Where(x => titreMenuId == x.TitreMenuId)
                .OrderByDescending(x => x.Position)
                .Select(p => p.Position)
                .DefaultIfEmpty(0)
                .Max();
            await Task.FromResult(lastPositon);
            return lastPositon;
        }

        /// <summary>  
        /// Cette méthode permet de détermine la dernière position
        /// </summary>  
        /// <param name="sousTitreMenuId">SousTitreMenuId facultatif int?</param>
        public async Task<int> LastPositionArticle(int? sousTitreMenuId)
        {
            int lastPositon;
            lastPositon = _context.Articles
                .Where(x => sousTitreMenuId == x.SousTitreMenuId)
                .OrderByDescending(x => x.Position)
                .Select(p => p.Position)
                .DefaultIfEmpty(0)
                .Max();
            await Task.FromResult(lastPositon);
            return lastPositon;
        }
    }
}
