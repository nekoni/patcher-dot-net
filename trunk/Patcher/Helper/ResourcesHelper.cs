using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace Patcher.Helper
{
    public class ResourcesHelper
    {
        string[] strMsg = new string[] { "err_no_item_found",
                                        "err_item_already_exist"};

        const int ERR_NO_ITEM_FOUND = 0;
        const int ERR_ITEM_ALREADY_EXIST = 1;

        Dictionary<CultureInfo, Dictionary<string, string>> DictionaryCulture = new Dictionary<CultureInfo, Dictionary<string, string>>();

        public ResourcesHelper()
        {
            CultureInfo cE = new CultureInfo("EN-us");
            DictionaryCulture.Add(cE, CreateEnglishDictionary());

            CultureInfo cI = new CultureInfo("IT-it");
            DictionaryCulture.Add(cI, CreateItalianDictionary());
        }

        public Dictionary<string, string> CreateEnglishDictionary()
        {
            Dictionary<string, string> DictMsg = new Dictionary<string, string>();
            DictMsg.Add(strMsg[ERR_NO_ITEM_FOUND], "Error item not found");
            DictMsg.Add(strMsg[ERR_ITEM_ALREADY_EXIST], "Error item already exist");

            return DictMsg;
        }

        public Dictionary<string, string> CreateItalianDictionary()
        {
            Dictionary<string, string> DictMsg = new Dictionary<string, string>();
            DictMsg.Add(strMsg[ERR_NO_ITEM_FOUND], "Elemento non trovato");
            DictMsg.Add(strMsg[ERR_ITEM_ALREADY_EXIST], "Elemento gia' esistente");

            return DictMsg;
        }



        public string GetResourceString(string sCode)
        {
            if(!DictionaryCulture.ContainsKey(CultureInfo.CurrentCulture))
                return sCode;

            if(!DictionaryCulture[CultureInfo.CurrentCulture].ContainsKey(sCode))
                return sCode;

            return DictionaryCulture[CultureInfo.CurrentCulture][sCode];
        }

    }
}
