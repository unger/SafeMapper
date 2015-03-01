namespace SafeMapper.Tests.Model.Resources
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Threading;

    public static class ResourceStringsClass
    {
        private static Dictionary<string, NameValueCollection> resourceStrings = new Dictionary<string, NameValueCollection>
                            {
                                {
                                    "sv",
                                    new NameValueCollection
                                        {
                                            { "Undefined", "Odefinerat" },
                                            { "Value1", "Värde 1" },
                                            { "Value2", "Värde 2" },
                                            { "Value3", "Värde 3" }
                                        }
                                },
                                {
                                    "en",
                                    new NameValueCollection
                                        {
                                            { "Undefined", "Odefinerat" },
                                            { "Value1", "Value 1" },
                                            { "Value2", "Value 2" },
                                            { "Value3", "Value 3" }
                                        }
                                }
                            };

        public static string Undefined
        {
            get
            {
                return GetString("Undefined");
            }
        }

        public static string Value1
        {
            get
            {
                return GetString("Value1");
            }
        }

        public static string Value2
        {
            get
            {
                return GetString("Value2");
            }
        }

        public static string Value3
        {
            get
            {
                return GetString("Value3");
            }
        }

        private static string GetString(string key)
        {
            var lang = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
            if (resourceStrings.ContainsKey(lang))
            {
                return resourceStrings[lang][key];
            }

            return resourceStrings["en"][key];
        }
    }
}
