using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cw1
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            foreach(var a in args)
            {
                Console.WriteLine(a);
            }
            //var emails = await GetEmails(args[0]);
            var emails = await GetEmails("https://www.pja.edu.pl/");
            foreach(var email in emails)
            {
                Console.WriteLine(email);
            }
        }

        static async Task<IList<string>> GetEmails(string url)
        {
            var httpClient = new HttpClient();
            var listOfEmails = new List<string>();
            var response = await httpClient.GetAsync(url);
            Regex emailRegex = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.IgnoreCase);
            MatchCollection emailMatches = emailRegex.Matches(response.Content.ReadAsStringAsync().Result);
            foreach (var emailMatche in emailMatches)
            {
                listOfEmails.Add(emailMatche.ToString());
            }
            return listOfEmails;
        } 
    }
}
