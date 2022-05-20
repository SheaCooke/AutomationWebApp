using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DisplayKustoDiffToUsers
{
    public class TranslationDictionaries
    {
        public static Dictionary<string, string> USSecTranslationRules = new Dictionary<string, string>()
        {
            {"AME","RXME"},
            {"DigiCert", "USSec" },
            {"SSLAdminV2", "USSec" },
            {"ESRP", "USSec" },
            {"keyvault.kusto.windows.net", "keyvaultrx.usseceast.kusto.core.microsoft.scloud"},
            {"dsms.kusto.windows.net", "dsmsrx.usseceast.kusto.core.microsoft.scloud"},
            {"idsharedwus.westus.kusto.windows.net", "acscluster.usseceast"},
            {"keyvault", "keyvaultrx.usseceast" },
            {"genevareference.westcentralus", "datastudiostreaming.kusto.core.microsoft.scloud"}/*,
            {"","s360v2kustorxprodrw.ussecwest.kusto.core.microsoft.scloud" },
            {"","service360db" },
            {"","keyvaultrx.usseceast" },
            {"","warm-rx" },
            {"","RX" }*/
        };

        public static Dictionary<string, string> USNatTranslation = new Dictionary<string, string>()
        {
            {"AME","EXME"},
            {"DigiCert", "USNat" },
            {"SSLAdminV2", "USNat" },
            {"ESRP", "USNat" },
            {"keyvault.kusto.windows.net", "keyvaultex.usnatwest.kusto.core.eaglex.ic.gov"},
            {"dsms.kusto.windows.net", "dsmsusnatw.usnatwest.kusto.core.eaglex.ic.gov"}/*,
            {"","s360kustoexprodrw.usnatwest.kusto.core.eaglex.ic.gov" },
            {"","service360db" },
            {"","genevareference.usnatwest" },
            {"","keyvaultex.usnatwest" },
            {"","EX" }*/
        };



    }
}
