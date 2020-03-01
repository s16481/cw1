using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cw1
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            if(args.Length == 0)
            {
                throw new ArgumentNullException();
            }
            Regex urlRegex = new Regex(@"^http(s)?://([\w-]+.)+[\w-]+(/[\w- ./?%&=])?$");
            MatchCollection urlMatches = urlRegex.Matches(args[0]);
            if(urlMatches.Count == 0)
            {
                throw new ArgumentException();
            }
            foreach(var a in args)
            {
                Console.WriteLine(a);
            }

            try
            {
                var emails = await GetEmails(args[0]);
                if (emails.Count == 0)
                {
                    throw new NoMachEmailsException();
                }
                else
                {
                    foreach (var email in emails.Distinct())
                    {
                        Console.WriteLine(email);
                    }
                }
            }
            catch (HttpRequestException)
            {
                Console.WriteLine("Błąd w czasie pobierania strony");
            }
            catch (NoMachEmailsException)
            {
                Console.WriteLine("Nie znaleziono adresów email");
            }
        }

        static async Task<IList<string>> GetEmails(string url)
        {
            var httpClient = new HttpClient();
            var listOfEmails = new List<string>();
            try
            {
                var response = await httpClient.GetAsync(url);
                Regex emailRegex = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.IgnoreCase);
                MatchCollection emailMatches = emailRegex.Matches(response.Content.ReadAsStringAsync().Result);
                foreach (var emailMatche in emailMatches)
                {
                    listOfEmails.Add(emailMatche.ToString());
                }
            }
            finally
            {
                httpClient.Dispose();
            }

            return listOfEmails;
        } 
    }
}
