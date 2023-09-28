namespace Request1;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;

public class Program1
{
    static async Task Main(string[] args)
    {
        string base_url = "https://sef.podkolzin.consulting/api/users/lastSeen";
        int offset = 0;
        int page_size = 20;

        while (true)
        {
            string api_url = $"{base_url}?offset={offset}";
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(api_url);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Failed to retrieve data. Status code: {response.StatusCode}");
                    break;
                }

                string json_data = await response.Content.ReadAsStringAsync();
                UserDataResponse userDataResponse = JsonConvert.DeserializeObject<UserDataResponse>(json_data);

                foreach (UserData user in userDataResponse.data)
                {
                    Console.WriteLine("User Information:");
                    Console.WriteLine($"Name: {user.firstName}");
                    Console.WriteLine($"Nickname: {user.nickname}");
                    string lastSeenStatus = GetLastSeenStatus(user);
                    Console.WriteLine($"Status: {lastSeenStatus}");
                    Console.WriteLine();
                }

                if (userDataResponse.data.Count < page_size)
                {
                    break;
                }
                offset += page_size;
            }
        }
    }

    public static string GetLastSeenStatus(UserData user)
    {
        DateTime current_time = DateTime.UtcNow;
        bool isOnline = user.isOnline;

        if (isOnline)
        {
            return "online";
        }

        string lastSeenStr = user.lastSeenDate;

        if (string.IsNullOrEmpty(lastSeenStr))
        {
            return "N/A";
        }


        DateTime lastSeenTime = DateTime.Parse(lastSeenStr).ToUniversalTime();
        TimeSpan timeElapsed = current_time - lastSeenTime;

        if (timeElapsed < TimeSpan.FromSeconds(30))
        {
            return "just now";
        }
        else if (timeElapsed < TimeSpan.FromMinutes(1))
        {
            return "less than a minute ago";
        }
        else if (timeElapsed < TimeSpan.FromMinutes(60))
        {
            return "a couple of minutes ago";
        }
        else if (timeElapsed < TimeSpan.FromMinutes(120))
        {
            return "an hour ago";
        }
        else if (timeElapsed < TimeSpan.FromDays(1))
        {
            return "today";
        }
        else if (timeElapsed < TimeSpan.FromDays(2))
        {
            return "yesterday";
        }
        else if (timeElapsed < TimeSpan.FromDays(7))
        {
            return "this week";
        }
        else
        {
            return "a long time ago";
        }
    }
}

public class UserDataResponse
{
    public List<UserData> data { get; set; }
}

public class UserData
{
    public string firstName { get; set; }
    public string nickname { get; set; }
    public bool isOnline { get; set; }
    public string lastSeenDate { get; set; }
}
