using Newtonsoft.Json.Linq;
using System.Xml.XPath;

namespace BlazorApp.Client
{
    public static class FetchConnectionStringFromSecrets
    {
        public static string json(string csprojDirectory)
        {
            var connectionString = "";
            var secrets_jsonPath = @"%USERPROFILE%\AppData\Roaming\Microsoft\UserSecrets\";
            foreach (var fileName in Directory.GetFiles(csprojDirectory))
                if (fileName.EndsWith(".csproj"))
                    secrets_jsonPath += new XPathDocument(fileName).CreateNavigator().Evaluate("string(//*/UserSecretsId/text())");
            secrets_jsonPath += @"\secrets.json";

            var json = File.ReadAllText(Environment.ExpandEnvironmentVariables(secrets_jsonPath));
            JObject connectionJson = JObject.Parse(json);
            connectionString = connectionJson["ConnectionStrings:AppConfig"].Value<string>();
            return connectionString;
        }
    }
}
