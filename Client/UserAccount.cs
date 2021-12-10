// Taken from: https://code-maze.com/using-app-roles-with-azure-active-directory-and-blazor-webassembly-hosted-apps/

using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Text.Json.Serialization;

namespace ProjectBank.Client
{
    public class UserAccount : RemoteUserAccount
    {
        [JsonPropertyName("roles")]
        public string[] Roles { get; set; } = Array.Empty<string>();
    }
}
