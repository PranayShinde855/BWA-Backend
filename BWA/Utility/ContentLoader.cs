using Newtonsoft.Json;
namespace BWA.Utility
{
    public static class ContentLoader
    {
        public static Dictionary<string, string> en_US = new Dictionary<string, string>();
        public static void LanguageLoader()
        {
            try
            {
                string languageContentUrl, languageData;
                if (en_US == null || en_US.Count <= 0)
                {
                    languageContentUrl = Path.Combine(Utils.RootPath, "Content\\en-US.json");
                    languageData = File.ReadAllText(languageContentUrl);

                    var _en_US = JsonConvert.DeserializeObject<Dictionary<string, string>>(languageData);
                    if (_en_US != null) en_US = _en_US;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public static string ReturnLanguageData(string key, string language = "en_US")
        {
            try
            {
                switch (language)
                {
                    case "en_US":
                        return en_US[key];
                    default:
                        return en_US[key];
                }
            }
            catch
            {
                return key;
            }
        }
    }
}
