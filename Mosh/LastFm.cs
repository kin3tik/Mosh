using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Mosh.Models.LastFm;
using RestSharp;

namespace Mosh
{
    public class LastFm
    {
        private const string Uri = "http://ws.audioscrobbler.com/2.0/";
        private const string ApiKey = "ba692a84eaea1f1bf5a7d599aec16a0f";

        public static async Task<List<Artist>> GetArtists(string user)
        {
            var result = await ServiceHelper.GetAsync<TopArtistsRoot>(new WebServiceParameters<object>
            {
                Uri = Uri,
                HttpMethod = Method.GET,
                EndpointPath = "?method={method}&user={user}&api_key={api}&limit={limit}&format=json",
                RootElement = "topartists",
                UrlSegments = new Dictionary<string, string>
                {
                    {"method", "user.gettopartists" },
                    { "user", user },
                    { "api", ApiKey },
                    { "limit", "100" }
                }
            });

            if (result.StatusCode == HttpStatusCode.OK)
                result.IsSuccessful = true;

            return result.ResultObject.TopArtists.Artist;
        }
    }
}
