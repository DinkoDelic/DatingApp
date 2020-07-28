using System;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DatingApp.API.Helpers
{
    public static class Extensions
    {
        //Global Exceptions that get sent up HTTP request
        public static void AddAplicationError(this HttpResponse response, string message)
        {
             response.Headers.Add("Application-Error", message);
             response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
             response.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        public static void AddPagination(this HttpResponse response, 
        int currentPage, int itemsPerPage, int totalItems, int totalPages)
        {
            var paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);
            //camelCase format
            var camelCaseFormatter = new JsonSerializerSettings();
            camelCaseFormatter.ContractResolver = new CamelCasePropertyNamesContractResolver();
            
            response.Headers.Add("Pagination", JsonConvert.SerializeObject(paginationHeader, camelCaseFormatter));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }

        public static int CalculateAge(this DateTime yearOfBirth)
        {
            int age =DateTime.Today.Year - yearOfBirth.Year;
            if(yearOfBirth.AddYears(age) > DateTime.Today)
                age--;

            return age; 
        }
    }
}