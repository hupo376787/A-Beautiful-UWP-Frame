using System.Collections.Generic;
using Windows.System.UserProfile;

namespace Sample.Helpers
{
    public class LanguageHelper
    {
        //多语言
        public static string GetCurLanguage()
        {
            var languages = GlobalizationPreferences.Languages;
            if (languages.Count > 0)
            {
                List<string> lLang = new List<string>();
                lLang.Add("zh-cn、zh、zh-Hans、zh-hans-cn、zh-sg、zh-hans-sg");
                lLang.Add("en-us、en、en-au、en-ca、en-gb、en-ie、en-in、en-nz、en-sg、en-za、en-bz、en-hk、en-id、en-jm、en-kz、en-mt、en-my、en-ph、en-pk、en-tt、en-vn、en-zw、en-053、en-021、en-029、en-011、en-018、en-014");
                for (int i = 0; i < lLang.Count; i++)
                {
                    if (lLang[i].ToLower().Contains(languages[0].ToLower()))
                    {
                        string temp = lLang[i].ToLower();
                        string[] tempArr = temp.Split('、');

                        return tempArr[0];
                    }
                    //else
                        //return "en-us";
                }
            }
            return "en-us";
        }
    }
}
