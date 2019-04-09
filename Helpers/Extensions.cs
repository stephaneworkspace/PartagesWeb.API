//-----------------------------------------------------------------------
// <license>https://github.com/stephaneworkspace/PartagesWeb.API/blob/master/LICENSE.md</license>
// <author>Stéphane</author>
//-----------------------------------------------------------------------
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PartagesWeb.API.Helpers
{
    /// <summary>
    /// Extensions
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Header pour Application-Errorm utilisé dans Configure de Startup.cs
        /// </summary>
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        /// <summary>
        /// Header pour la pagination
        /// </summary>
        /// <param name="response">Reponse http</param>
        /// <param name="currentPage">Page actuel</param>
        /// <param name="itemsPerPage">Éléments par pages</param>
        /// <param name="totalItems">Nombre total d'éléements</param>
        /// <param name="totalPages">Nombre total de pages</param>
        public static void AddPagination(this HttpResponse response,int currentPage, int itemsPerPage, int totalItems, int totalPages)
        {
            var paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);

            // Ces 2 lines pour .pagination a coté de .result sinon c'est juste dans le header
            var camelCaseFormatter = new JsonSerializerSettings();
            camelCaseFormatter.ContractResolver = new CamelCasePropertyNamesContractResolver();
            // Fin comment


            response.Headers.Add("Pagination",
                JsonConvert.SerializeObject(paginationHeader, camelCaseFormatter));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}
