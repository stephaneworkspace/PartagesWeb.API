//-----------------------------------------------------------------------
// <license>https://github.com/stephaneworkspace/PartagesWeb.API/blob/master/LICENSE.md</license>
// <author>Stéphane</author>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace PartagesWeb.API
{
    /// <summary>
    /// Class Program pour le démarage de l'application asp.net core
    /// </summary>
    public class Program
    {
        /// <summary>  
        /// Cette méthode est le constructeur 
        /// </summary> 
        /// <param name="args"> Arguments</param>
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        /// <summary>  
        /// Cette méthode permet de créer le WebHost
        /// </summary> 
        /// <param name="args"> Arguments</param>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
