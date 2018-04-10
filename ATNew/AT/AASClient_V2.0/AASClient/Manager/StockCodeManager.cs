using System;
using System.Collections.Generic;
using System.Linq;
using AASTrader.Model;
using AASTrader.Model.DataModel;
//using Microsoft.Practices.Unity;

namespace AASClient.Manager
{
    public class StockCodeManager : IManager
    {
        private static object sync = new object();
        private static StockCodeManager _instance;

        public static StockCodeManager GetInstance
        {
            get
            {
                if (_instance == null)
                {
                    lock (sync)
                    {
                        if (_instance == null)
                        {
                            _instance = new StockCodeManager();
                        }
                    }
                }
                return _instance;
            }
        }

        private Dictionary<string, StockCode> _codes;



        public Dictionary<string, StockCode> Codes
        {
            get { return _codes; }
        }

        public List<StockCode> CodeList
        {
            get { return _codes.Values.ToList<StockCode>(); }
        }

        public StockCodeManager()
        {
            _codes = new Dictionary<string, StockCode>();
        }

        public StockCode GetStockCode(string code)
        {
            if (_codes.ContainsKey(code))
            {
                return _codes[code];
            }

            return null;
        }

        public string GetStockName(string code)
        {
            if (_codes.ContainsKey(code))
            {
                return _codes[code].Name;
            }

            return "";
        }

        public string GetStockTitle(string code)
        {
            if (_codes.ContainsKey(code))
            {
                return string.Format("{0} [{1}]", _codes[code].Name, code);
            }

            return "";
        }

        public string FindStockCode(string filter)
        {
            List<StockCode> sg = new List<StockCode>();
            string f = filter.ToLower().Trim();
            foreach (StockCode code in CodeList)
            {
                if (code.SearchText.Contains(filter))
                {
                    return code.Code;
                }
            }

            return "";
        }

        public void UpdateCodes(List<StockCode> codes)
        {
            try
            {
                _codes.Clear();
                Dictionary<string, string> pinyin = GetPinyinDict();
                foreach (StockCode code in codes)
                {
                    if (_codes.ContainsKey(code.Code) == false)
                    {
                        //更新拼音
                        if (pinyin.ContainsKey(code.Code))
                        {
                            code.Pinyin = pinyin[code.Code];
                        }
                        _codes.Add(code.Code, code);
                    }
                }
            }
            catch (Exception ex)
            {
                AASClient.Program.logger.LogRunning("StockManager:股票代码更新失败\n{0}", ex.Message);
            }
        }

        #region 拼音字典
        public Dictionary<string, string> GetPinyinDict()
        {
            Dictionary<string, string> codes = new Dictionary<string, string>();
            codes.Add("000001", "PAYH");
            codes.Add("000612", "JZWF");
            codes.Add("000903", "YNDL");
            codes.Add("000024", "ZSDC");
            codes.Add("000637", "MHSH");
            codes.Add("000927", "YQXL");
            codes.Add("000046", "FHKG");
            codes.Add("000672", "SFSN");
            codes.Add("000958", "DFRD");
            codes.Add("000090", "TJJT");
            codes.Add("000698", "SYHG");
            codes.Add("000980", "JMGF");
            codes.Add("000404", "HYYS");
            codes.Add("000722", "HNFZ");
            codes.Add("002004", "HBYT");
            codes.Add("000430", "ZJJ");
            codes.Add("000755", "STS");
            codes.Add("002024", "SNYS");
            codes.Add("000520", "STF");
            codes.Add("002044", "JSSY");
            codes.Add("000787", "STC");
            codes.Add("000541", "FSZM");
            codes.Add("002064", "HFAL");
            codes.Add("000810", "HRJH");
            codes.Add("000564", "XAMS");
            codes.Add("002084", "HOWY");
            codes.Add("000836", "XMKJ");
            codes.Add("000591", "TJG");
            codes.Add("002104", "HBGF");
            codes.Add("000878", "YNTY");
            codes.Add("002124", "TBGF");
            codes.Add("002325", "HTGF");
            codes.Add("002144", "HDGK");
            codes.Add("002345", "CHJ");
            codes.Add("002164", "STD");
            codes.Add("002365", "YAYY");
            codes.Add("002184", "HDKZ");
            codes.Add("002385", "DBN");
            codes.Add("002204", "DLZG");
            codes.Add("002405", "SWTX");
            codes.Add("002224", "SLS");
            codes.Add("002425", "KSGF");
            codes.Add("002244", "BJJT");
            codes.Add("002445", "ZNZG");
            codes.Add("002265", "XYGF");
            codes.Add("002465", "HGTX");
            codes.Add("002285", "SLX");
            codes.Add("002485", "XNE");
            codes.Add("002305", "NGZY");
            codes.Add("002505", "DKMY");
            codes.Add("002526", "SDKJ");
            codes.Add("002729", "HLL");
            codes.Add("300131", "YTZK");
            codes.Add("002546", "XLDZ");
            codes.Add("200016", "SKJB");
            codes.Add("300151", "CHKJ");
            codes.Add("002566", "YSYY");
            codes.Add("300171", "DFL");
            codes.Add("200152", "SHB");
            codes.Add("002586", "WHGF");
            codes.Add("300191", "QNHX");
            codes.Add("200613", "DDHB");
            codes.Add("002606", "DLDC");
            codes.Add("300211", "YTKJ");
            codes.Add("300010", "LSC");
            codes.Add("002626", "JDW");
            codes.Add("300231", "YXKJ");
            codes.Add("300030", "YPYL");
            codes.Add("002646", "QQKJ");
            codes.Add("300251", "GXCM");
            codes.Add("300050", "SJDL");
            codes.Add("002666", "DLJT");
            codes.Add("300271", "HYRJ");
            codes.Add("300071", "HYJX");
            codes.Add("002686", "YLD");
            codes.Add("300291", "HLBN");
            codes.Add("300091", "JTL");
            codes.Add("002707", "ZXLY");
            codes.Add("300311", "RZX");
            codes.Add("300111", "XRK");
            codes.Add("300331", "SDWG");
            codes.Add("300351", "YGDQ");
            codes.Add("600130", "BDGF");
            codes.Add("300372", "XTDQ");
            codes.Add("600156", "HSGF");
            codes.Add("300393", "ZLGF");
            codes.Add("600178", "STD");
            codes.Add("300419", "HFKJ");
            codes.Add("600200", "JSWZ");
            codes.Add("600015", "HXYH");
            codes.Add("600223", "LSZY");
            codes.Add("600039", "SCLQ");
            codes.Add("600246", "WTDC");
            codes.Add("600068", "GZB");
            codes.Add("600268", "GDNZ");
            codes.Add("600088", "ZSCM");
            codes.Add("600290", "HYDQ");
            codes.Add("600109", "GJZQ");
            codes.Add("600313", "NFZY");
            codes.Add("600336", "AKM");
            codes.Add("600361", "HLZC");
            codes.Add("600608", "SHKJ");
            codes.Add("600823", "SMGF");
            codes.Add("600383", "JDJT");
            codes.Add("600843", "SGSB");
            codes.Add("600629", "LGSY");
            codes.Add("600408", "ATJT");
            codes.Add("600864", "HTGF");
            codes.Add("600651", "FLYH");
            codes.Add("600439", "RBK");
            codes.Add("600885", "HFGF");
            codes.Add("600675", "ZHQ");
            codes.Add("600475", "HGGF");
            codes.Add("600965", "FCWF");
            codes.Add("600695", "DJGF");
            codes.Add("600498", "FHTX");
            codes.Add("600990", "SCDZ");
            codes.Add("600717", "TJG");
            codes.Add("600520", "ZFKJ");
            codes.Add("601015", "SXHM");
            codes.Add("600737", "ZLTH");
            codes.Add("600545", "XJCJ");
            codes.Add("601126", "SFGF");
            codes.Add("600758", "HYN");
            codes.Add("600567", "SYZY");
            codes.Add("601233", "TKGF");
            codes.Add("600780", "TBNY");
            codes.Add("600588", "YYRJ");
            codes.Add("601519", "DZH");
            codes.Add("600802", "FJSN");
            codes.Add("601677", "MTLY");
            codes.Add("000004", "GNKJ");
            codes.Add("601872", "ZSLC");
            codes.Add("000613", "DDHA");
            codes.Add("601989", "ZGZG");
            codes.Add("000615", "HBJH");
            codes.Add("603077", "HBGF");
            codes.Add("000616", "YCTZ");
            codes.Add("603333", "MXDL");
            codes.Add("000617", "SYJC");
            codes.Add("603698", "HTGC");
            codes.Add("000619", "HLXC");
            codes.Add("900905", "LFXB");
            codes.Add("000620", "XHL");
            codes.Add("900925", "JDBG");
            codes.Add("000622", "HLSY");
            codes.Add("900947", "ZHBG");
            codes.Add("000623", "JLAD");
            codes.Add("000002", "WKA");
            codes.Add("000625", "CAQC");
            codes.Add("000626", "RYJT");
            codes.Add("000005", "SJXY");
            codes.Add("000008", "BLL");
            codes.Add("000627", "TMJT");
            codes.Add("000009", "ZGBA");
            codes.Add("000006", "SZYA");
            codes.Add("000628", "GXFZ");
            codes.Add("000010", "SHX");
            codes.Add("000905", "XMGW");
            codes.Add("000629", "PGFT");
            codes.Add("000011", "SWYA");
            codes.Add("000906", "WCZT");
            codes.Add("000630", "TLYS");
            codes.Add("000012", "NBA");
            codes.Add("000908", "TYKJ");
            codes.Add("000631", "SFHY");
            codes.Add("000014", "SHGF");
            codes.Add("000909", "SYKJ");
            codes.Add("000632", "SMJT");
            codes.Add("000913", "QJMT");
            codes.Add("000910", "DYKJ");
            codes.Add("000633", "HJTZ");
            codes.Add("000016", "SKJA");
            codes.Add("000911", "NNTY");
            codes.Add("000635", "YLT");
            codes.Add("000915", "SDHT");
            codes.Add("000912", "LTH");
            codes.Add("000636", "FHGK");
            codes.Add("000916", "HBGS");
            codes.Add("000007", "LQGF");
            codes.Add("000017", "SZHA");
            codes.Add("000020", "SHFA");
            codes.Add("000638", "WFFZ");
            codes.Add("000021", "CCKF");
            codes.Add("000639", "XWSP");
            codes.Add("000022", "SCWA");
            codes.Add("000650", "RHYY");
            codes.Add("000023", "STDA");
            codes.Add("000651", "GLDQ");
            codes.Add("000657", "ZWGX");
            codes.Add("000652", "TDGF");
            codes.Add("000659", "STZ");
            codes.Add("000655", "JLKY");
            codes.Add("000661", "CCGX");
            codes.Add("000656", "JKGF");
            codes.Add("000662", "SFT");
            codes.Add("000018", "ZGA");
            codes.Add("000663", "YALY");
            codes.Add("000019", "SSBA");
            codes.Add("000917", "DGCM");
            codes.Add("000918", "JKC");
            codes.Add("000667", "MHJT");
            codes.Add("000030", "FAGF");
            codes.Add("000919", "JLYY");
            codes.Add("000031", "ZLDC");
            codes.Add("000668", "RFKG");
            codes.Add("000920", "NFHT");
            codes.Add("000032", "SSDA");
            codes.Add("000669", "JHNY");
            codes.Add("000921", "HXKL");
            codes.Add("000033", "STX");
            codes.Add("000670", "SSY");
            codes.Add("000922", "JDGF");
            codes.Add("000034", "SXTF");
            codes.Add("000671", "YGC");
            codes.Add("000665", "HBGD");
            codes.Add("000035", "ZKJ");
            codes.Add("000025", "TLA");
            codes.Add("000923", "HBXG");
            codes.Add("000036", "HLKG");
            codes.Add("000026", "FYDA");
            codes.Add("000925", "ZHJD");
            codes.Add("000037", "SNDA");
            codes.Add("000027", "SZNY");
            codes.Add("000926", "FXGF");
            codes.Add("000038", "SDT");
            codes.Add("000028", "GYYZ");
            codes.Add("000666", "JWFJ");
            codes.Add("000039", "ZJJT");
            codes.Add("000029", "SSFA");
            codes.Add("000040", "BADC");
            codes.Add("000677", "HTHL");
            codes.Add("000042", "ZZKG");
            codes.Add("000678", "XYZC");
            codes.Add("000043", "ZHDC");
            codes.Add("000679", "DLYY");
            codes.Add("000673", "DDDF");
            codes.Add("000680", "STGF");
            codes.Add("000045", "SFZA");
            codes.Add("000681", "YDGF");
            codes.Add("000928", "STJ");
            codes.Add("000682", "DFDZ");
            codes.Add("000929", "LZHH");
            codes.Add("000683", "YXNY");
            codes.Add("000930", "ZLSH");
            codes.Add("000685", "ZSGY");
            codes.Add("000931", "ZGC");
            codes.Add("000686", "DBZQ");
            codes.Add("000676", "SDGK");
            codes.Add("000687", "HTTE");
            codes.Add("000688", "JXKY");
            codes.Add("000936", "HXGF");
            codes.Add("000955", "XLKG");
            codes.Add("000957", "ZTKC");
            codes.Add("000690", "BXNY");
            codes.Add("000937", "JZNY");
            codes.Add("000048", "KDE");
            codes.Add("000691", "YTSY");
            codes.Add("000938", "ZGGF");
            codes.Add("000049", "DSDC");
            codes.Add("000692", "HTRD");
            codes.Add("000939", "KDDL");
            codes.Add("000050", "STMA");
            codes.Add("000693", "HZGN");
            codes.Add("000948", "NTXX");
            codes.Add("000055", "FDJT");
            codes.Add("000695", "BHNY");
            codes.Add("000949", "XXHX");
            codes.Add("000056", "SGS");
            codes.Add("000697", "LSYS");
            codes.Add("000950", "JFHG");
            codes.Add("000058", "SSG");
            codes.Add("000932", "HLGT");
            codes.Add("000951", "ZGZQ");
            codes.Add("000059", "HJGF");
            codes.Add("000933", "SHGF");
            codes.Add("000952", "GJYY");
            codes.Add("000060", "ZJLN");
            codes.Add("000935", "SCSM");
            codes.Add("000953", "HCHG");
            codes.Add("000061", "NCP");
            codes.Add("000701", "XMXD");
            codes.Add("000062", "SZHQ");
            codes.Add("000702", "ZHKJ");
            codes.Add("000063", "ZXTX");
            codes.Add("000703", "HYSH");
            codes.Add("000065", "BFGJ");
            codes.Add("000705", "ZJZY");
            codes.Add("000066", "CCDN");
            codes.Add("000707", "SHKJ");
            codes.Add("000068", "HKSG");
            codes.Add("000708", "DYTG");
            codes.Add("000069", "HQCA");
            codes.Add("000709", "HBGT");
            codes.Add("000070", "TFXX");
            codes.Add("000710", "TXYB");
            codes.Add("000078", "HWSW");
            codes.Add("000711", "TLZY");
            codes.Add("000700", "MSKJ");
            codes.Add("000712", "JLGF");
            codes.Add("000713", "FLZY");
            codes.Add("000965", "TBJJ");
            codes.Add("000150", "YHDC");
            codes.Add("000966", "CYDL");
            codes.Add("000715", "ZXSY");
            codes.Add("000151", "ZCGF");
            codes.Add("000717", "SGSS");
            codes.Add("000716", "NFSP");
            codes.Add("000153", "FYYY");
            codes.Add("000718", "SNHQ");
            codes.Add("000088", "YTG");
            codes.Add("000155", "CHGF");
            codes.Add("000719", "DDCM");
            codes.Add("000089", "SZJC");
            codes.Add("000156", "HSCM");
            codes.Add("000720", "XNTS");
            codes.Add("000959", "SGGF");
            codes.Add("000157", "ZLZK");
            codes.Add("000721", "XAYS");
            codes.Add("000960", "XYGF");
            codes.Add("000158", "CSGF");
            codes.Add("000096", "GJNY");
            codes.Add("000961", "ZNJS");
            codes.Add("000159", "GJSY");
            codes.Add("000099", "ZXHZ");
            codes.Add("000962", "DFTY");
            codes.Add("000967", "SFGK");
            codes.Add("000100", "TCLJ");
            codes.Add("000963", "HDYY");
            codes.Add("000968", "MQH");
            codes.Add("000969", "ATKJ");
            codes.Add("000970", "ZKSH");
            codes.Add("000166", "SWHY");
            codes.Add("000971", "LDKG");
            codes.Add("000301", "DFSC");
            codes.Add("000972", "XZJ");
            codes.Add("000333", "MDJT");
            codes.Add("000973", "FSKJ");
            codes.Add("000338", "WCDL");
            codes.Add("000975", "YTZY");
            codes.Add("000400", "XJDQ");
            codes.Add("000976", "CHGF");
            codes.Add("000401", "JDSN");
            codes.Add("000977", "LCXX");
            codes.Add("000402", "JRJ");
            codes.Add("000978", "GLLY");
            codes.Add("000403", "STS");
            codes.Add("000979", "ZHGF");
            codes.Add("000723", "MJNY");
            codes.Add("000725", "JDFA");
            codes.Add("000726", "LTA");
            codes.Add("000738", "ZHDK");
            codes.Add("000985", "DQHK");
            codes.Add("000727", "HDKJ");
            codes.Add("000987", "GZYY");
            codes.Add("000739", "PLYY");
            codes.Add("000728", "GYZQ");
            codes.Add("000988", "HGKJ");
            codes.Add("000748", "CCXX");
            codes.Add("000729", "YJPJ");
            codes.Add("000989", "JZT");
            codes.Add("000750", "GHZQ");
            codes.Add("000731", "SCMF");
            codes.Add("000990", "CZGF");
            codes.Add("000751", "STX");
            codes.Add("000732", "THJT");
            codes.Add("000993", "MDDL");
            codes.Add("000752", "XCFZ");
            codes.Add("000733", "ZHKJ");
            codes.Add("000995", "HTJY");
            codes.Add("000753", "ZZFZ");
            codes.Add("000735", "LNS");
            codes.Add("000996", "ZGZQ");
            codes.Add("000981", "YYGF");
            codes.Add("000736", "ZFDC");
            codes.Add("000997", "XDL");
            codes.Add("000982", "ZYRY");
            codes.Add("000737", "NFHG");
            codes.Add("000998", "LPGK");
            codes.Add("000983", "XSMD");
            codes.Add("000999", "HRSJ");
            codes.Add("000760", "STE");
            codes.Add("001696", "ZSDL");
            codes.Add("000761", "BGBC");
            codes.Add("001896", "YNKG");
            codes.Add("000762", "XCKY");
            codes.Add("002001", "XHC");
            codes.Add("000407", "SLGF");
            codes.Add("002002", "HDXY");
            codes.Add("000408", "JGY");
            codes.Add("002003", "WXGF");
            codes.Add("000409", "SDDK");
            codes.Add("000756", "XHZY");
            codes.Add("000410", "SYJC");
            codes.Add("000757", "HWGF");
            codes.Add("000411", "YTJT");
            codes.Add("000758", "ZSGF");
            codes.Add("000413", "DXGD");
            codes.Add("000759", "ZBJT");
            codes.Add("000415", "BHZL");
            codes.Add("000416", "MSKG");
            codes.Add("002009", "TQGF");
            codes.Add("002017", "DXHP");
            codes.Add("000417", "HFBH");
            codes.Add("002010", "CHGF");
            codes.Add("002018", "HXHG");
            codes.Add("000418", "XTEA");
            codes.Add("002011", "DAHJ");
            codes.Add("002019", "XFYY");
            codes.Add("000419", "TCKG");
            codes.Add("002020", "JXYY");
            codes.Add("002012", "KEGF");
            codes.Add("000420", "JLHX");
            codes.Add("002021", "ZJGF");
            codes.Add("002013", "ZHJD");
            codes.Add("002005", "DHRD");
            codes.Add("002022", "KHSW");
            codes.Add("002014", "YXGF");
            codes.Add("002006", "STJ");
            codes.Add("002023", "HTGX");
            codes.Add("002015", "STX");
            codes.Add("000421", "NJZB");
            codes.Add("000422", "HBYH");
            codes.Add("000766", "THJM");
            codes.Add("002007", "HLSW");
            codes.Add("000423", "DAAJ");
            codes.Add("000767", "ZZDL");
            codes.Add("002008", "DZJG");
            codes.Add("000425", "XGJX");
            codes.Add("002016", "SRZY");
            codes.Add("000426", "XYKY");
            codes.Add("000783", "CJZQ");
            codes.Add("000428", "HTJD");
            codes.Add("000785", "WHZS");
            codes.Add("000429", "YGSA");
            codes.Add("000786", "BXJC");
            codes.Add("000768", "ZHFJ");
            codes.Add("002025", "HTDQ");
            codes.Add("000776", "GFZQ");
            codes.Add("002026", "SDWD");
            codes.Add("000777", "ZHKJ");
            codes.Add("002027", "QXKG");
            codes.Add("000778", "XXZG");
            codes.Add("002028", "SYDQ");
            codes.Add("000779", "STP");
            codes.Add("002029", "QPL");
            codes.Add("000780", "PZNY");
            codes.Add("002030", "DAJY");
            codes.Add("000782", "MDGF");
            codes.Add("002031", "JLGF");
            codes.Add("002032", "SBE");
            codes.Add("002038", "SLYY");
            codes.Add("000788", "BDYY");
            codes.Add("002033", "LJLY");
            codes.Add("002039", "QYDL");
            codes.Add("000509", "HSKG");
            codes.Add("002034", "MXD");
            codes.Add("002040", "NJG");
            codes.Add("000510", "JLJT");
            codes.Add("000488", "CMZY");
            codes.Add("002041", "DHZY");
            codes.Add("000511", "XTXC");
            codes.Add("000498", "SDLQ");
            codes.Add("002042", "HFSF");
            codes.Add("000513", "LZJT");
            codes.Add("000501", "EWSA");
            codes.Add("000503", "HHKG");
            codes.Add("000514", "YKF");
            codes.Add("002035", "HDGF");
            codes.Add("000504", "STC");
            codes.Add("000515", "PYTY");
            codes.Add("002036", "YKKJ");
            codes.Add("000505", "ZJKG");
            codes.Add("000789", "JXSN");
            codes.Add("002037", "JLFZ");
            codes.Add("000506", "ZRZY");
            codes.Add("000790", "HSJT");
            codes.Add("000502", "LJKG");
            codes.Add("000507", "ZHG");
            codes.Add("000791", "GSDT");
            codes.Add("002043", "TBB");
            codes.Add("000798", "ZSYY");
            codes.Add("000516", "KYTZ");
            codes.Add("000799", "JGJ");
            codes.Add("000517", "RADC");
            codes.Add("000800", "YQJC");
            codes.Add("000518", "SHSW");
            codes.Add("002045", "GGDQ");
            codes.Add("000519", "JNHJ");
            codes.Add("002046", "ZYKJ");
            codes.Add("000792", "YHGF");
            codes.Add("002047", "BYGF");
            codes.Add("000793", "HWCM");
            codes.Add("002048", "NBHX");
            codes.Add("000795", "TYGY");
            codes.Add("002049", "TFGX");
            codes.Add("000796", "YSGF");
            codes.Add("002050", "SHGF");
            codes.Add("000797", "ZGWY");
            codes.Add("002051", "ZGGJ");
            codes.Add("000801", "SCJZ");
            codes.Add("000524", "DFBG");
            codes.Add("000532", "LHGF");
            codes.Add("000802", "BJLY");
            codes.Add("000533", "WJL");
            codes.Add("000525", "HTY");
            codes.Add("000803", "JYCC");
            codes.Add("000534", "WZGF");
            codes.Add("000526", "YRTZ");
            codes.Add("000805", "STY");
            codes.Add("000536", "HYKJ");
            codes.Add("000527", "MDDQ");
            codes.Add("000806", "YHTZ");
            codes.Add("000537", "GYFZ");
            codes.Add("000528", "LG");
            codes.Add("000807", "YLGF");
            codes.Add("002054", "DMHG");
            codes.Add("000529", "GHKG");
            codes.Add("000809", "TLXC");
            codes.Add("002055", "DRDZ");
            codes.Add("002052", "TZDZ");
            codes.Add("000521", "MLDQ");
            codes.Add("002056", "HDDC");
            codes.Add("002053", "YNYH");
            codes.Add("000522", "BYSA");
            codes.Add("002057", "ZGTY");
            codes.Add("000530", "DLGF");
            codes.Add("000523", "GZLQ");
            codes.Add("002058", "WET");
            codes.Add("000531", "SHYA");
            codes.Add("002059", "YNLY");
            codes.Add("000813", "TSFZ");
            codes.Add("002060", "YSD");
            codes.Add("000815", "MLZY");
            codes.Add("000538", "YNBY");
            codes.Add("000816", "JHDL");
            codes.Add("000539", "YDLA");
            codes.Add("000818", "FDHG");
            codes.Add("000540", "ZTCT");
            codes.Add("000819", "YYXC");
            codes.Add("002061", "JSHG");
            codes.Add("000820", "JCGF");
            codes.Add("002062", "HRJS");
            codes.Add("000821", "JSQJ");
            codes.Add("002063", "YGRJ");
            codes.Add("000822", "STH");
            codes.Add("000811", "YTBL");
            codes.Add("000823", "CSDZ");
            codes.Add("000812", "SXJY");
            codes.Add("000825", "TGBX");
            codes.Add("000826", "SDHJ");
            codes.Add("000543", "WNDL");
            codes.Add("002074", "DYDQ");
            codes.Add("000828", "DGKG");
            codes.Add("000544", "ZYHB");
            codes.Add("002075", "SGGF");
            codes.Add("000829", "TYKG");
            codes.Add("000545", "JPTY");
            codes.Add("002076", "XLT");
            codes.Add("000830", "LXHG");
            codes.Add("000546", "GHKG");
            codes.Add("000547", "MFFA");
            codes.Add("002065", "DHRJ");
            codes.Add("002068", "HMGF");
            codes.Add("000548", "HNTZ");
            codes.Add("002066", "RTKJ");
            codes.Add("002069", "ZZD");
            codes.Add("000550", "JLQC");
            codes.Add("000831", "WKXT");
            codes.Add("002070", "ZHGF");
            codes.Add("000551", "CYKJ");
            codes.Add("000833", "GTGF");
            codes.Add("002071", "CCYS");
            codes.Add("000552", "JYMD");
            codes.Add("000835", "SCSD");
            codes.Add("002072", "DMGF");
            codes.Add("000553", "SLDA");
            codes.Add("002067", "JXZY");
            codes.Add("002073", "RKGF");
            codes.Add("000554", "TSSY");
            codes.Add("000555", "SZXX");
            codes.Add("002079", "SZGD");
            codes.Add("000557", "STG");
            codes.Add("002080", "ZCKJ");
            codes.Add("000558", "LYZY");
            codes.Add("002081", "JTL");
            codes.Add("000559", "WXQC");
            codes.Add("002082", "DLXC");
            codes.Add("000560", "KBDA");
            codes.Add("002083", "FRGF");
            codes.Add("000561", "FHDZ");
            codes.Add("000837", "QCFZ");
            codes.Add("000562", "HYZQ");
            codes.Add("000565", "YSXA");
            codes.Add("000563", "SGTA");
            codes.Add("000566", "HNHY");
            codes.Add("002077", "DGGF");
            codes.Add("000567", "HDGF");
            codes.Add("002078", "TYZY");
            codes.Add("000568", "LZLJ");
            codes.Add("000569", "CCGF");
            codes.Add("000851", "GHGF");
            codes.Add("000869", "ZYA");
            codes.Add("000570", "SCCA");
            codes.Add("000875", "JDGF");
            codes.Add("000852", "JZGF");
            codes.Add("000571", "XDZA");
            codes.Add("000876", "XXW");
            codes.Add("000856", "JDZB");
            codes.Add("000572", "HMQC");
            codes.Add("000877", "TSGF");
            codes.Add("000858", "WLY");
            codes.Add("000573", "YHYA");
            codes.Add("000578", "YHJT");
            codes.Add("000859", "GFSY");
            codes.Add("000576", "GDGH");
            codes.Add("000581", "WFGK");
            codes.Add("000860", "SXNY");
            codes.Add("000838", "GXDC");
            codes.Add("000582", "BBWG");
            codes.Add("000861", "HYGF");
            codes.Add("000584", "YLKG");
            codes.Add("000839", "ZXGA");
            codes.Add("000862", "YXNY");
            codes.Add("000585", "DBDQ");
            codes.Add("000848", "CDLL");
            codes.Add("000863", "SXGF");
            codes.Add("000586", "HYTX");
            codes.Add("000850", "HMGF");
            codes.Add("000868", "AKKC");
            codes.Add("000587", "JYZB");
            codes.Add("002092", "ZTHX");
            codes.Add("000589", "QLTA");
            codes.Add("002093", "GMKJ");
            codes.Add("000590", "ZGGH");
            codes.Add("002094", "QDJW");
            codes.Add("002085", "WFAW");
            codes.Add("002095", "SYB");
            codes.Add("002086", "DFHY");
            codes.Add("002096", "NLMB");
            codes.Add("002087", "XYFZ");
            codes.Add("002097", "SHZN");
            codes.Add("002088", "LYGF");
            codes.Add("002098", "XXGF");
            codes.Add("002089", "XHY");
            codes.Add("002099", "HXYY");
            codes.Add("002090", "JZKJ");
            codes.Add("002100", "TKSW");
            codes.Add("002091", "JSGT");
            codes.Add("002101", "GDHT");
            codes.Add("002102", "GFJY");
            codes.Add("000882", "HLGF");
            codes.Add("000889", "MYWL");
            codes.Add("002103", "GBGF");
            codes.Add("000890", "FES");
            codes.Add("000883", "HBNY");
            codes.Add("000592", "PTFZ");
            codes.Add("000892", "XMLH");
            codes.Add("000885", "TLSN");
            codes.Add("000593", "DTRQ");
            codes.Add("000893", "DLLY");
            codes.Add("000886", "HNGS");
            codes.Add("000594", "STG");
            codes.Add("000895", "SHFZ");
            codes.Add("000887", "ZDGF");
            codes.Add("000595", "XBZC");
            codes.Add("000897", "JBFZ");
            codes.Add("000598", "XRTZ");
            codes.Add("000596", "GJGJ");
            codes.Add("000898", "AGGF");
            codes.Add("000599", "QDSX");
            codes.Add("000597", "DBZY");
            codes.Add("000899", "GNGF");
            codes.Add("002105", "XLSY");
            codes.Add("000880", "WCZJ");
            codes.Add("000900", "XDTZ");
            codes.Add("000881", "DLGJ");
            codes.Add("002106", "LBGK");
            codes.Add("002107", "WHYY");
            codes.Add("000888", "EMSA");
            codes.Add("000600", "JTNY");
            codes.Add("000611", "SHGF");
            codes.Add("000601", "SNGF");
            codes.Add("002108", "CZMZ");
            codes.Add("000602", "JMTS");
            codes.Add("002109", "XHGF");
            codes.Add("000603", "SDKY");
            codes.Add("002110", "SGMG");
            codes.Add("000605", "BHGF");
            codes.Add("002111", "WHGT");
            codes.Add("000606", "QHMJ");
            codes.Add("002112", "SBKJ");
            codes.Add("000607", "HZKG");
            codes.Add("002113", "TRKG");
            codes.Add("000608", "YGGF");
            codes.Add("002114", "LPXD");
            codes.Add("000609", "MSGF");
            codes.Add("002115", "SWTX");
            codes.Add("000610", "XALY");
            codes.Add("000901", "HTKJ");
            codes.Add("000902", "XYF");
            codes.Add("002122", "TMGF");
            codes.Add("002136", "AND");
            codes.Add("002116", "ZGHC");
            codes.Add("002137", "SYD");
            codes.Add("002123", "RXGF");
            codes.Add("002117", "DGGF");
            codes.Add("002138", "SLDZ");
            codes.Add("002128", "LTMY");
            codes.Add("002118", "ZXYY");
            codes.Add("002139", "TBGF");
            codes.Add("002129", "ZHGF");
            codes.Add("002119", "KQDZ");
            codes.Add("002140", "DHKJ");
            codes.Add("002130", "WEHC");
            codes.Add("002125", "XTDH");
            codes.Add("002141", "RSCW");
            codes.Add("002126", "YLGF");
            codes.Add("002131", "LOGF");
            codes.Add("002142", "NBYH");
            codes.Add("002127", "STX");
            codes.Add("002132", "HXKJ");
            codes.Add("002143", "GJSP");
            codes.Add("002120", "XHGF");
            codes.Add("002133", "GYJT");
            codes.Add("002326", "YTKJ");
            codes.Add("002121", "KLDZ");
            codes.Add("002134", "STP");
            codes.Add("002327", "FAN");
            codes.Add("002135", "DNWJ");
            codes.Add("002328", "XPGF");
            codes.Add("002338", "APGD");
            codes.Add("002329", "HSRY");
            codes.Add("002339", "JCDZ");
            codes.Add("002330", "DLS");
            codes.Add("002340", "GLM");
            codes.Add("002331", "WTKJ");
            codes.Add("002145", "ZHTB");
            codes.Add("002332", "XJZY");
            codes.Add("002146", "RSFZ");
            codes.Add("002333", "LPSJ");
            codes.Add("002147", "FYZC");
            codes.Add("002334", "YWT");
            codes.Add("002148", "BWTX");
            codes.Add("002335", "KHHS");
            codes.Add("002149", "XBCL");
            codes.Add("002336", "RRL");
            codes.Add("002341", "XLKJ");
            codes.Add("002337", "SXKJ");
            codes.Add("002342", "JLSJ");
            codes.Add("002343", "HXGF");
            codes.Add("002159", "STSD");
            codes.Add("002350", "BJKR");
            codes.Add("002150", "TRZB");
            codes.Add("002160", "STC");
            codes.Add("002351", "MBZ");
            codes.Add("002151", "BDXT");
            codes.Add("002161", "YWG");
            codes.Add("002352", "DTXC");
            codes.Add("002152", "GDYT");
            codes.Add("002353", "JRGF");
            codes.Add("002162", "SMK");
            codes.Add("002153", "SJXX");
            codes.Add("002354", "KMMY");
            codes.Add("002163", "STS");
            codes.Add("002355", "XMGQ");
            codes.Add("002154", "BXN");
            codes.Add("002344", "HNPC");
            codes.Add("002356", "HND");
            codes.Add("002155", "CZKY");
            codes.Add("002346", "ZZJS");
            codes.Add("002357", "FLYY");
            codes.Add("002156", "TFWD");
            codes.Add("002358", "SYDQ");
            codes.Add("002347", "TEZG");
            codes.Add("002157", "ZBKJ");
            codes.Add("002359", "QXTT");
            codes.Add("002348", "GLGF");
            codes.Add("002158", "HZJJ");
            codes.Add("002349", "JHZY");
            codes.Add("002360", "TDHG");
            codes.Add("002173", "QZZZ");
            codes.Add("002361", "SJGF");
            codes.Add("002174", "YZWL");
            codes.Add("002165", "HBL");
            codes.Add("002175", "GLSC");
            codes.Add("002166", "LYSW");
            codes.Add("002176", "JTDJ");
            codes.Add("002167", "DFGY");
            codes.Add("002177", "YYGF");
            codes.Add("002168", "SZHC");
            codes.Add("002178", "YHZN");
            codes.Add("002169", "ZGDQ");
            codes.Add("002179", "ZHGD");
            codes.Add("002170", "BTGF");
            codes.Add("002180", "WLD");
            codes.Add("002171", "JCTY");
            codes.Add("002181", "YCM");
            codes.Add("002172", "AYKJ");
            codes.Add("002182", "YHJS");
            codes.Add("002183", "YYT");
            codes.Add("002191", "JJGF");
            codes.Add("002372", "WXXC");
            codes.Add("002362", "HWKJ");
            codes.Add("002192", "LXGF");
            codes.Add("002373", "LXYY");
            codes.Add("002363", "LJJX");
            codes.Add("002193", "SDRY");
            codes.Add("002374", "LPGF");
            codes.Add("002364", "ZHDQ");
            codes.Add("002194", "WHFG");
            codes.Add("002375", "YXGF");
            codes.Add("002185", "HTKJ");
            codes.Add("002366", "DFGF");
            codes.Add("002376", "XBY");
            codes.Add("002186", "QJD");
            codes.Add("002367", "KLDT");
            codes.Add("002187", "GBGF");
            codes.Add("002377", "GCGX");
            codes.Add("002368", "TJGF");
            codes.Add("002188", "XJL");
            codes.Add("002378", "ZYWY");
            codes.Add("002369", "ZYKJ");
            codes.Add("002189", "LDGD");
            codes.Add("002379", "LFHB");
            codes.Add("002370", "YTYY");
            codes.Add("002190", "CFJC");
            codes.Add("002380", "KYGF");
            codes.Add("002371", "QXDZ");
            codes.Add("002381", "SJGF");
            codes.Add("002382", "LFGF");
            codes.Add("002202", "JFKJ");
            codes.Add("002383", "HZSZ");
            codes.Add("002203", "HLGF");
            codes.Add("002384", "DSJM");
            codes.Add("002205", "GTGF");
            codes.Add("002195", "HLRJ");
            codes.Add("002206", "HLD");
            codes.Add("002196", "FZDJ");
            codes.Add("002207", "ZYGF");
            codes.Add("002197", "ZTDZ");
            codes.Add("002208", "HFCJ");
            codes.Add("002198", "JYZY");
            codes.Add("002209", "DYL");
            codes.Add("002199", "DJDZ");
            codes.Add("002210", "FMGJ");
            codes.Add("002200", "LDD");
            codes.Add("002211", "HDXC");
            codes.Add("002201", "JDXC");
            codes.Add("002212", "NYGF");
            codes.Add("002213", "TEJ");
            codes.Add("002223", "YYYL");
            codes.Add("002395", "SXGF");
            codes.Add("002214", "DLKJ");
            codes.Add("002396", "XWRJ");
            codes.Add("002386", "TYJT");
            codes.Add("002215", "NPX");
            codes.Add("002397", "MJJF");
            codes.Add("002387", "HNSP");
            codes.Add("002216", "SQSP");
            codes.Add("002398", "JYJT");
            codes.Add("002388", "XYZC");
            codes.Add("002217", "STH");
            codes.Add("002399", "HPR");
            codes.Add("002389", "NYKJ");
            codes.Add("002218", "TRXN");
            codes.Add("002400", "SGGF");
            codes.Add("002390", "XBZY");
            codes.Add("002219", "HKYL");
            codes.Add("002225", "PNGF");
            codes.Add("002391", "CQGF");
            codes.Add("002220", "TBGF");
            codes.Add("002226", "JNHG");
            codes.Add("002392", "BJLE");
            codes.Add("002221", "DHNY");
            codes.Add("002227", "ATX");
            codes.Add("002393", "LSZY");
            codes.Add("002222", "FJKJ");
            codes.Add("002228", "HXBZ");
            codes.Add("002394", "LFGF");
            codes.Add("002229", "HBGF");
            codes.Add("002235", "ANGF");
            codes.Add("002230", "KDXF");
            codes.Add("002236", "DHGF");
            codes.Add("002401", "ZHKJ");
            codes.Add("002237", "HBGF");
            codes.Add("002402", "HET");
            codes.Add("002238", "TWSX");
            codes.Add("002403", "ASD");
            codes.Add("002406", "YDCD");
            codes.Add("002404", "JXSC");
            codes.Add("002407", "DFD");
            codes.Add("002231", "AWTX");
            codes.Add("002408", "QXTD");
            codes.Add("002232", "QMXX");
            codes.Add("002409", "YKKJ");
            codes.Add("002233", "TPJT");
            codes.Add("002410", "GLD");
            codes.Add("002234", "STM");
            codes.Add("002239", "JFD");
            codes.Add("002240", "WHGF");
            codes.Add("002420", "YCGF");
            codes.Add("002247", "DLXC");
            codes.Add("002411", "JJJ");
            codes.Add("002421", "DSZN");
            codes.Add("002248", "STD");
            codes.Add("002412", "HSZY");
            codes.Add("002422", "KLYY");
            codes.Add("002249", "DYDJ");
            codes.Add("002413", "CFGF");
            codes.Add("002241", "GESX");
            codes.Add("002250", "LHKJ");
            codes.Add("002414", "GDHW");
            codes.Add("002242", "JYGF");
            codes.Add("002251", "BBG");
            codes.Add("002415", "HKWS");
            codes.Add("002243", "TCLX");
            codes.Add("002426", "SLJM");
            codes.Add("002416", "ASD");
            codes.Add("002423", "ZYTG");
            codes.Add("002427", "YFGF");
            codes.Add("002417", "SYD");
            codes.Add("002424", "GZBL");
            codes.Add("002428", "YNZY");
            codes.Add("002418", "KSGF");
            codes.Add("002245", "AYSC");
            codes.Add("002429", "ZCGF");
            codes.Add("002419", "THSC");
            codes.Add("002246", "BHGF");
            codes.Add("002430", "HYGF");
            codes.Add("002431", "ZLYL");
            codes.Add("002432", "JAYL");
            codes.Add("002437", "YHYY");
            codes.Add("002433", "TAT");
            codes.Add("002438", "JSST");
            codes.Add("002252", "SHLS");
            codes.Add("002439", "QMXC");
            codes.Add("002253", "CDZS");
            codes.Add("002440", "RTGF");
            codes.Add("002254", "THXC");
            codes.Add("002441", "ZYD");
            codes.Add("002255", "HLZG");
            codes.Add("002442", "LXHG");
            codes.Add("002434", "WLY");
            codes.Add("002443", "JZGD");
            codes.Add("002435", "CJRF");
            codes.Add("002444", "JXKJ");
            codes.Add("002436", "XSKJ");
            codes.Add("002256", "CHJH");
            codes.Add("002258", "LEHX");
            codes.Add("002259", "SDLY");
            codes.Add("002450", "KDX");
            codes.Add("002460", "GFLY");
            codes.Add("002260", "YLP");
            codes.Add("002461", "ZJPJ");
            codes.Add("002451", "MEDQ");
            codes.Add("002261", "TWXX");
            codes.Add("002452", "CGJT");
            codes.Add("002462", "JST");
            codes.Add("002262", "EHYY");
            codes.Add("002453", "TMJH");
            codes.Add("002266", "ZFKG");
            codes.Add("002263", "DDN");
            codes.Add("002454", "SZGF");
            codes.Add("002267", "STRQ");
            codes.Add("002264", "XHD");
            codes.Add("002455", "BCGF");
            codes.Add("002268", "WST");
            codes.Add("002446", "SLTX");
            codes.Add("002456", "OFG");
            codes.Add("002269", "MBFS");
            codes.Add("002447", "YQMY");
            codes.Add("002457", "QLGY");
            codes.Add("002270", "FYSK");
            codes.Add("002448", "ZYNP");
            codes.Add("002458", "YSGF");
            codes.Add("002271", "DFYH");
            codes.Add("002449", "GXGD");
            codes.Add("002459", "STT");
            codes.Add("002272", "CRGF");
            codes.Add("002273", "SJGD");
            codes.Add("002274", "HCHG");
            codes.Add("002281", "GXKJ");
            codes.Add("002275", "GLSJ");
            codes.Add("002282", "BSGJ");
            codes.Add("002276", "WMGF");
            codes.Add("002283", "TRQZ");
            codes.Add("002277", "YAGF");
            codes.Add("002284", "YTGF");
            codes.Add("002278", "SKGF");
            codes.Add("002466", "TQLY");
            codes.Add("002463", "HDGF");
            codes.Add("002467", "ELS");
            codes.Add("002464", "JLKJ");
            codes.Add("002468", "ADX");
            codes.Add("002279", "JQRJ");
            codes.Add("002469", "SWGC");
            codes.Add("002280", "XSJ");
            codes.Add("002470", "JZD");
            codes.Add("002471", "ZCDL");
            codes.Add("002286", "BLB");
            codes.Add("002291", "XQL");
            codes.Add("002296", "HHKJ");
            codes.Add("002287", "QZCY");
            codes.Add("002477", "CYNM");
            codes.Add("002297", "BYXC");
            codes.Add("002288", "CHKJ");
            codes.Add("002478", "CBGF");
            codes.Add("002298", "XLDQ");
            codes.Add("002289", "YSDZ");
            codes.Add("002479", "FCHB");
            codes.Add("002299", "SNFZ");
            codes.Add("002290", "HSXC");
            codes.Add("002480", "XZGF");
            codes.Add("002300", "TYDL");
            codes.Add("002472", "SHCD");
            codes.Add("002301", "QXWJ");
            codes.Add("002481", "STSP");
            codes.Add("002473", "SLD");
            codes.Add("002302", "XBJS");
            codes.Add("002292", "AFDM");
            codes.Add("002474", "RJRJ");
            codes.Add("002303", "MYS");
            codes.Add("002293", "LLJF");
            codes.Add("002475", "LXJM");
            codes.Add("002482", "GTGF");
            codes.Add("002294", "XLT");
            codes.Add("002476", "BMGF");
            codes.Add("002483", "RBGF");
            codes.Add("002295", "JYGF");
            codes.Add("002484", "JHGF");
            codes.Add("002308", "WCGF");
            codes.Add("002304", "YHGF");
            codes.Add("002309", "ZLKJ");
            codes.Add("002486", "JLJ");
            codes.Add("002310", "DFYL");
            codes.Add("002487", "DJZG");
            codes.Add("002311", "HDJT");
            codes.Add("002488", "JGGF");
            codes.Add("002492", "HJDX");
            codes.Add("002489", "ZJYQ");
            codes.Add("002493", "RSSH");
            codes.Add("002490", "SDML");
            codes.Add("002494", "HSGF");
            codes.Add("002491", "TDGD");
            codes.Add("002495", "JLGF");
            codes.Add("002306", "XEQ");
            codes.Add("002496", "HFGF");
            codes.Add("002307", "BXLQ");
            codes.Add("002497", "YHJT");
            codes.Add("002498", "HLGF");
            codes.Add("002315", "JDKJ");
            codes.Add("002507", "FLZC");
            codes.Add("002499", "KLHB");
            codes.Add("002508", "LBDQ");
            codes.Add("002316", "JQTX");
            codes.Add("002500", "SXZQ");
            codes.Add("002509", "TGXF");
            codes.Add("002317", "ZSYY");
            codes.Add("002501", "LYJZ");
            codes.Add("002510", "TQM");
            codes.Add("002318", "JLTC");
            codes.Add("002502", "HWGF");
            codes.Add("002511", "ZSJR");
            codes.Add("002319", "LTGF");
            codes.Add("002503", "SYT");
            codes.Add("002512", "DHZN");
            codes.Add("002320", "HXGF");
            codes.Add("002504", "DGWD");
            codes.Add("002513", "LFSH");
            codes.Add("002321", "HYNY");
            codes.Add("002312", "STDZ");
            codes.Add("002514", "BXKJ");
            codes.Add("002322", "LGJC");
            codes.Add("002313", "RHTX");
            codes.Add("002515", "JZHT");
            codes.Add("002323", "ZLDQ");
            codes.Add("002314", "YZGF");
            codes.Add("002516", "JSKD");
            codes.Add("002506", "STC");
            codes.Add("002517", "TYGF");
            codes.Add("002527", "XSD");
            codes.Add("002518", "KSD");
            codes.Add("300132", "QSGF");
            codes.Add("002519", "YHDZ");
            codes.Add("300133", "HCYS");
            codes.Add("002520", "RFJJ");
            codes.Add("300134", "DFKJ");
            codes.Add("002521", "QFXC");
            codes.Add("300135", "BLLQ");
            codes.Add("002522", "ZJZC");
            codes.Add("300136", "XWTX");
            codes.Add("002523", "TQQZ");
            codes.Add("300137", "XHHB");
            codes.Add("002524", "GZJT");
            codes.Add("300138", "CGSW");
            codes.Add("002324", "PLT");
            codes.Add("002731", "CHZB");
            codes.Add("002730", "DGKJ");
            codes.Add("002528", "YFT");
            codes.Add("002529", "HYJX");
            codes.Add("002539", "XDHG");
            codes.Add("002735", "WZXC");
            codes.Add("002530", "FDGF");
            codes.Add("002540", "YTKJ");
            codes.Add("002736", "GXZQ");
            codes.Add("002531", "TSFN");
            codes.Add("002732", "YTRY");
            codes.Add("002737", "KHYY");
            codes.Add("002532", "XJBY");
            codes.Add("002541", "HLGG");
            codes.Add("002738", "ZKZY");
            codes.Add("002533", "JBDG");
            codes.Add("002542", "ZHYT");
            codes.Add("002739", "WDYH");
            codes.Add("002534", "HGGF");
            codes.Add("002543", "WHDQ");
            codes.Add("002740", "ADE");
            codes.Add("002535", "LZZJ");
            codes.Add("002544", "JSKJ");
            codes.Add("002741", "GHKJ");
            codes.Add("002536", "XBGF");
            codes.Add("002545", "DFTT");
            codes.Add("002742", "SSTC");
            codes.Add("002537", "HLMD");
            codes.Add("002733", "XTGF");
            codes.Add("002743", "FHGG");
            codes.Add("002538", "SET");
            codes.Add("002734", "LMGF");
            codes.Add("002745", "MLS");
            codes.Add("002746", "XTGF");
            codes.Add("002553", "NFZC");
            codes.Add("200002", "WKB");
            codes.Add("002554", "HBP");
            codes.Add("200011", "SWYB");
            codes.Add("002555", "SRGF");
            codes.Add("200012", "NBB");
            codes.Add("002556", "HLGF");
            codes.Add("002547", "CXJG");
            codes.Add("002557", "QQSP");
            codes.Add("002548", "JXN");
            codes.Add("002558", "SJYL");
            codes.Add("002549", "KMTQ");
            codes.Add("002559", "YWGF");
            codes.Add("002550", "QHZY");
            codes.Add("002560", "TDGF");
            codes.Add("002551", "SRYL");
            codes.Add("300139", "FXXC");
            codes.Add("002552", "BDZG");
            codes.Add("002561", "XJH");
            codes.Add("002562", "XDKJ");
            codes.Add("200024", "ZSJB");
            codes.Add("300145", "NFBY");
            codes.Add("002563", "SMFS");
            codes.Add("200025", "TLB");
            codes.Add("200037", "SNDB");
            codes.Add("002564", "ZHJ");
            codes.Add("200026", "FYDB");
            codes.Add("002567", "TRS");
            codes.Add("002565", "SHLX");
            codes.Add("200028", "YZB");
            codes.Add("002568", "BRGF");
            codes.Add("200017", "SZHB");
            codes.Add("200029", "SSFB");
            codes.Add("002569", "BSGF");
            codes.Add("200018", "ZGB");
            codes.Add("200030", "FAB");
            codes.Add("300146", "TCBJ");
            codes.Add("200019", "SSBB");
            codes.Add("300141", "HSDQ");
            codes.Add("300147", "XXZY");
            codes.Add("200020", "SHFB");
            codes.Add("300142", "WSSW");
            codes.Add("200039", "ZJB");
            codes.Add("300140", "QYZB");
            codes.Add("300143", "XHSW");
            codes.Add("200045", "SFZB");
            codes.Add("200022", "SCWB");
            codes.Add("300144", "SCYY");
            codes.Add("002570", "BYM");
            codes.Add("002571", "DLGF");
            codes.Add("300149", "LZGK");
            codes.Add("002572", "SFY");
            codes.Add("300150", "SJRE");
            codes.Add("002573", "GDQX");
            codes.Add("002575", "QXWJ");
            codes.Add("002574", "MPZB");
            codes.Add("002576", "TDDL");
            codes.Add("200053", "SJDB");
            codes.Add("002577", "LBKJ");
            codes.Add("200054", "JMB");
            codes.Add("002578", "MFLY");
            codes.Add("200055", "FDB");
            codes.Add("002579", "ZJDZ");
            codes.Add("200056", "SGSB");
            codes.Add("002580", "SYGF");
            codes.Add("200058", "SSGB");
            codes.Add("002581", "WCKJ");
            codes.Add("300148", "TZWH");
            codes.Add("002582", "HXN");
            codes.Add("002583", "HND");
            codes.Add("300153", "KTDY");
            codes.Add("300159", "XYGF");
            codes.Add("002584", "XLHG");
            codes.Add("300154", "RLGF");
            codes.Add("300160", "XQGF");
            codes.Add("002585", "SXXC");
            codes.Add("300155", "AJB");
            codes.Add("300161", "HZSK");
            codes.Add("200160", "NJB");
            codes.Add("300156", "TLHB");
            codes.Add("300162", "LMGD");
            codes.Add("200168", "LYB");
            codes.Add("300157", "HTAP");
            codes.Add("300163", "XFXC");
            codes.Add("200413", "DXB");
            codes.Add("200488", "CMB");
            codes.Add("300164", "TYSY");
            codes.Add("200418", "XTEB");
            codes.Add("200505", "ZJB");
            codes.Add("300165", "TRYQ");
            codes.Add("200429", "YGSB");
            codes.Add("200512", "MCKB");
            codes.Add("300166", "DFGX");
            codes.Add("200468", "NTXB");
            codes.Add("200513", "LZB");
            codes.Add("300167", "DWSX");
            codes.Add("300152", "RKKJ");
            codes.Add("300158", "ZDZY");
            codes.Add("300168", "WDXX");
            codes.Add("300169", "TSXC");
            codes.Add("200596", "GJGB");
            codes.Add("300170", "HDXX");
            codes.Add("002587", "ATDZ");
            codes.Add("200521", "WMLB");
            codes.Add("002588", "SDL");
            codes.Add("200530", "DLB");
            codes.Add("002589", "RKYY");
            codes.Add("200539", "YDLB");
            codes.Add("002590", "WAKJ");
            codes.Add("200541", "YZMB");
            codes.Add("002591", "HDGX");
            codes.Add("200550", "JLB");
            codes.Add("002592", "BLKJ");
            codes.Add("200553", "SLDB");
            codes.Add("002593", "RSJT");
            codes.Add("200570", "SCCB");
            codes.Add("002594", "BYD");
            codes.Add("200581", "SWFB");
            codes.Add("300172", "ZDHB");
            codes.Add("300173", "SDGF");
            codes.Add("200725", "JDFB");
            codes.Add("300002", "SZTY");
            codes.Add("300174", "YLGF");
            codes.Add("002595", "HMKJ");
            codes.Add("300003", "LPYL");
            codes.Add("300175", "LYGF");
            codes.Add("200726", "LTB");
            codes.Add("300004", "NFGF");
            codes.Add("300176", "HTJM");
            codes.Add("200761", "BGBB");
            codes.Add("300181", "ZLYY");
            codes.Add("300177", "ZHD");
            codes.Add("200770", "STW");
            codes.Add("300182", "JCGF");
            codes.Add("300178", "TBGJ");
            codes.Add("200771", "HQLB");
            codes.Add("300183", "DRZB");
            codes.Add("300179", "SFD");
            codes.Add("200869", "ZYB");
            codes.Add("300184", "LYXX");
            codes.Add("300180", "HFCX");
            codes.Add("200986", "YHBB");
            codes.Add("300185", "TYZG");
            codes.Add("200625", "CAB");
            codes.Add("200992", "ZLB");
            codes.Add("300186", "DHN");
            codes.Add("200706", "WZB");
            codes.Add("300001", "TRD");
            codes.Add("300187", "YQHB");
            codes.Add("300188", "MYBK");
            codes.Add("002598", "SDZG");
            codes.Add("300189", "SNDF");
            codes.Add("002599", "STGF");
            codes.Add("300190", "WEL");
            codes.Add("002600", "JFCC");
            codes.Add("300005", "TLZ");
            codes.Add("002601", "BLL");
            codes.Add("300006", "LMYY");
            codes.Add("002602", "SJHT");
            codes.Add("300007", "HWDZ");
            codes.Add("002603", "YLYY");
            codes.Add("300008", "SHJH");
            codes.Add("002604", "LLSW");
            codes.Add("300009", "AKSW");
            codes.Add("002605", "YJPK");
            codes.Add("002596", "HNRZ");
            codes.Add("300011", "DHJS");
            codes.Add("002597", "JHSY");
            codes.Add("300012", "HCJC");
            codes.Add("300013", "XNWL");
            codes.Add("300023", "BDGF");
            codes.Add("300195", "CRGF");
            codes.Add("300014", "YWLN");
            codes.Add("300024", "JQR");
            codes.Add("300196", "CHGF");
            codes.Add("300015", "AEYK");
            codes.Add("300025", "HXCY");
            codes.Add("002607", "YXQC");
            codes.Add("300016", "BLYY");
            codes.Add("300026", "HRYY");
            codes.Add("002608", "STCB");
            codes.Add("300017", "WSKJ");
            codes.Add("300027", "HYXD");
            codes.Add("002609", "JSKJ");
            codes.Add("300018", "ZYHD");
            codes.Add("300028", "JYKJ");
            codes.Add("002610", "AKKJ");
            codes.Add("300019", "GBKJ");
            codes.Add("300029", "TLGD");
            codes.Add("002611", "DFJG");
            codes.Add("300020", "YJGF");
            codes.Add("300192", "KSWD");
            codes.Add("002612", "LZGF");
            codes.Add("300021", "DYJS");
            codes.Add("300193", "JSKJ");
            codes.Add("002613", "BBGF");
            codes.Add("300022", "JFNJ");
            codes.Add("300194", "FAYY");
            codes.Add("002614", "MFL");
            codes.Add("002615", "HES");
            codes.Add("300197", "THST");
            codes.Add("002619", "JLGY");
            codes.Add("300198", "NCGF");
            codes.Add("002620", "RHGF");
            codes.Add("300199", "HYYY");
            codes.Add("002621", "DLSL");
            codes.Add("300200", "GMXC");
            codes.Add("300203", "JGKJ");
            codes.Add("300201", "HLZ");
            codes.Add("300204", "STS");
            codes.Add("300202", "JLGF");
            codes.Add("300205", "TYXX");
            codes.Add("002616", "CQJT");
            codes.Add("300206", "LBYQ");
            codes.Add("002617", "LXKJ");
            codes.Add("300207", "XWD");
            codes.Add("002618", "DBKJ");
            codes.Add("300208", "HSDQ");
            codes.Add("300209", "TZXX");
            codes.Add("300210", "SYGF");
            codes.Add("300036", "CTRJ");
            codes.Add("300046", "TJGF");
            codes.Add("300031", "BTDY");
            codes.Add("300037", "XZB");
            codes.Add("300047", "TYDK");
            codes.Add("300032", "JLJD");
            codes.Add("300038", "MTN");
            codes.Add("300048", "HKBP");
            codes.Add("002622", "YDJT");
            codes.Add("300039", "SHKB");
            codes.Add("300212", "YHL");
            codes.Add("002623", "YMD");
            codes.Add("300040", "JZDQ");
            codes.Add("300213", "JXFH");
            codes.Add("002624", "JLGF");
            codes.Add("300041", "HTXC");
            codes.Add("300214", "RKHX");
            codes.Add("002625", "LSGF");
            codes.Add("300042", "LKKJ");
            codes.Add("300033", "THS");
            codes.Add("300215", "DKY");
            codes.Add("300043", "HDYL");
            codes.Add("300216", "QSYJ");
            codes.Add("300034", "GYGN");
            codes.Add("300044", "SWZN");
            codes.Add("300217", "DFDR");
            codes.Add("300035", "ZKDQ");
            codes.Add("300045", "HLCT");
            codes.Add("300049", "FRGF");
            codes.Add("300218", "ALGF");
            codes.Add("002627", "YCJY");
            codes.Add("300219", "HLGD");
            codes.Add("002628", "CDLQ");
            codes.Add("300220", "JYJG");
            codes.Add("002629", "RZYF");
            codes.Add("300221", "YXKJ");
            codes.Add("300051", "SWHL");
            codes.Add("300222", "KDZN");
            codes.Add("300052", "ZQB");
            codes.Add("300223", "BJJZ");
            codes.Add("300053", "OBT");
            codes.Add("300224", "ZHCC");
            codes.Add("300054", "DLGF");
            codes.Add("300225", "JLT");
            codes.Add("300055", "WBD");
            codes.Add("300226", "SHGL");
            codes.Add("300056", "SWS");
            codes.Add("300227", "GYD");
            codes.Add("300057", "WSGF");
            codes.Add("300228", "FRTZ");
            codes.Add("300059", "DFCF");
            codes.Add("300070", "BSY");
            codes.Add("300229", "TES");
            codes.Add("002636", "JAGJ");
            codes.Add("300061", "KNT");
            codes.Add("300058", "LSGB");
            codes.Add("002637", "ZYKJ");
            codes.Add("300062", "ZNDQ");
            codes.Add("002630", "HXNY");
            codes.Add("300063", "TLJT");
            codes.Add("300232", "ZMKJ");
            codes.Add("002631", "DEJJ");
            codes.Add("300064", "YJGS");
            codes.Add("300233", "JCYY");
            codes.Add("002632", "DMGX");
            codes.Add("300065", "HLX");
            codes.Add("002638", "QSGD");
            codes.Add("002633", "SKGF");
            codes.Add("300066", "SCGF");
            codes.Add("002639", "XRGF");
            codes.Add("002634", "BJGF");
            codes.Add("300067", "ANQ");
            codes.Add("002640", "BYKY");
            codes.Add("002635", "AJKJ");
            codes.Add("300068", "NDDY");
            codes.Add("002641", "YGGF");
            codes.Add("300230", "YLDY");
            codes.Add("300069", "JLHD");
            codes.Add("002642", "RZL");
            codes.Add("002643", "YTWR");
            codes.Add("300241", "RFGD");
            codes.Add("002644", "FCZY");
            codes.Add("300242", "MJKJ");
            codes.Add("002645", "HHKJ");
            codes.Add("300243", "RFGC");
            codes.Add("300234", "KEXC");
            codes.Add("300244", "DAZD");
            codes.Add("300235", "FZKJ");
            codes.Add("300245", "TJKJ");
            codes.Add("300236", "SHXY");
            codes.Add("300072", "SJHB");
            codes.Add("300237", "MCKJ");
            codes.Add("300073", "DSKJ");
            codes.Add("300238", "GHSW");
            codes.Add("300074", "HPGF");
            codes.Add("300239", "DBSW");
            codes.Add("300075", "SZZT");
            codes.Add("300240", "FLD");
            codes.Add("300076", "GQYS");
            codes.Add("300077", "GMJS");
            codes.Add("002652", "YZXC");
            codes.Add("002659", "ZTQL");
            codes.Add("002647", "HLGF");
            codes.Add("300249", "YMK");
            codes.Add("002660", "MSDY");
            codes.Add("002648", "WXSH");
            codes.Add("300079", "SMSX");
            codes.Add("002661", "KMMY");
            codes.Add("002649", "BYKJ");
            codes.Add("300250", "CLXX");
            codes.Add("002662", "JWGF");
            codes.Add("002650", "JJSP");
            codes.Add("002653", "HSK");
            codes.Add("002663", "PBYL");
            codes.Add("300246", "BLT");
            codes.Add("002654", "WRKJ");
            codes.Add("300080", "XDXC");
            codes.Add("300247", "SLJ");
            codes.Add("002655", "GDDS");
            codes.Add("300081", "HXYD");
            codes.Add("300078", "ZRSC");
            codes.Add("002656", "KNDL");
            codes.Add("300082", "AKGF");
            codes.Add("300248", "XKP");
            codes.Add("002657", "ZKJC");
            codes.Add("300083", "JSJM");
            codes.Add("002651", "LJGF");
            codes.Add("002658", "XDL");
            codes.Add("300084", "HMKJ");
            codes.Add("300085", "YZJ");
            codes.Add("300255", "CSYY");
            codes.Add("300086", "KZYY");
            codes.Add("300256", "XXKJ");
            codes.Add("300087", "QYGK");
            codes.Add("300257", "KSGF");
            codes.Add("300088", "CXKJ");
            codes.Add("300258", "JDKJ");
            codes.Add("300089", "CCJT");
            codes.Add("002665", "SHJN");
            codes.Add("300090", "SYGF");
            codes.Add("300259", "XTKJ");
            codes.Add("300252", "JXN");
            codes.Add("300260", "XLYC");
            codes.Add("002664", "XZDJ");
            codes.Add("300261", "YBHX");
            codes.Add("300253", "WNRJ");
            codes.Add("300262", "BASW");
            codes.Add("300254", "QYZY");
            codes.Add("300263", "LHJN");
            codes.Add("300264", "JCSX");
            codes.Add("300268", "WFSK");
            codes.Add("002670", "HSGF");
            codes.Add("300265", "TGXL");
            codes.Add("300269", "LJGD");
            codes.Add("002671", "LQGF");
            codes.Add("300266", "XYGL");
            codes.Add("300270", "ZWDZ");
            codes.Add("002672", "DJHB");
            codes.Add("300267", "EKZY");
            codes.Add("300097", "ZYGF");
            codes.Add("002673", "XBZQ");
            codes.Add("300098", "GXX");
            codes.Add("002674", "XYKJ");
            codes.Add("300092", "KXJD");
            codes.Add("300099", "YLK");
            codes.Add("002675", "DCYY");
            codes.Add("300093", "JGBL");
            codes.Add("300100", "SLGF");
            codes.Add("002676", "SWGF");
            codes.Add("300094", "GLSC");
            codes.Add("300101", "ZXKJ");
            codes.Add("002677", "ZJMD");
            codes.Add("300095", "HWGF");
            codes.Add("002668", "AMDQ");
            codes.Add("300102", "QZGD");
            codes.Add("300096", "YLZ");
            codes.Add("002669", "KDXC");
            codes.Add("300103", "DGLJ");
            codes.Add("002667", "AZGF");
            codes.Add("300104", "LSW");
            codes.Add("002683", "HDBP");
            codes.Add("300105", "LYJS");
            codes.Add("002684", "MSKJ");
            codes.Add("300106", "XBMY");
            codes.Add("002685", "HDZJ");
            codes.Add("300107", "JXGF");
            codes.Add("300272", "KNHB");
            codes.Add("300108", "SLGF");
            codes.Add("300273", "HJGF");
            codes.Add("002678", "ZJGQ");
            codes.Add("300274", "YGDY");
            codes.Add("002679", "FJJS");
            codes.Add("300275", "MAS");
            codes.Add("002680", "HHJX");
            codes.Add("300276", "SFZN");
            codes.Add("002681", "FDKJ");
            codes.Add("300277", "HLX");
            codes.Add("002682", "LZGF");
            codes.Add("300278", "HCD");
            codes.Add("300109", "XKY");
            codes.Add("002687", "QZB");
            codes.Add("002693", "SCYY");
            codes.Add("300110", "HRYY");
            codes.Add("002688", "JHSW");
            codes.Add("002694", "GDKJ");
            codes.Add("300279", "HJKJ");
            codes.Add("002689", "BLT");
            codes.Add("002695", "HSH");
            codes.Add("300280", "NTDY");
            codes.Add("300287", "FLX");
            codes.Add("002696", "BYGF");
            codes.Add("300281", "JMJJ");
            codes.Add("300288", "LMXX");
            codes.Add("002697", "HQLS");
            codes.Add("300282", "HGGF");
            codes.Add("300289", "LDM");
            codes.Add("002698", "BSGF");
            codes.Add("300283", "WZHF");
            codes.Add("300290", "RKKJ");
            codes.Add("002699", "MSWH");
            codes.Add("300284", "SJK");
            codes.Add("002690", "MYGD");
            codes.Add("002700", "XJHY");
            codes.Add("300285", "GCCL");
            codes.Add("002691", "SZZB");
            codes.Add("002701", "ARJ");
            codes.Add("300286", "AKR");
            codes.Add("002692", "YCDL");
            codes.Add("300292", "WTTX");
            codes.Add("300112", "WXZK");
            codes.Add("300119", "RPSW");
            codes.Add("002702", "HXSP");
            codes.Add("002706", "LXDQ");
            codes.Add("002703", "ZJSB");
            codes.Add("300293", "LYZB");
            codes.Add("002705", "XBGF");
            codes.Add("300294", "BYSW");
            codes.Add("300113", "SWKJ");
            codes.Add("300295", "SLWW");
            codes.Add("300114", "ZHDC");
            codes.Add("300296", "LYD");
            codes.Add("300115", "CYJM");
            codes.Add("300297", "LDGF");
            codes.Add("300116", "JRXF");
            codes.Add("300298", "SNSW");
            codes.Add("300117", "JYGF");
            codes.Add("300120", "JWDC");
            codes.Add("300118", "DFRS");
            codes.Add("300121", "YGHT");
            codes.Add("300122", "ZFSW");
            codes.Add("300302", "TYKJ");
            codes.Add("300310", "YTSJ");
            codes.Add("300123", "TYN");
            codes.Add("002708", "GYGF");
            codes.Add("300303", "JFGD");
            codes.Add("300124", "HCJS");
            codes.Add("002709", "TCCL");
            codes.Add("300304", "YYDQ");
            codes.Add("300125", "YSD");
            codes.Add("002711", "OPGW");
            codes.Add("300305", "YXGF");
            codes.Add("300126", "RQGF");
            codes.Add("002712", "SMCM");
            codes.Add("300129", "TSFN");
            codes.Add("300127", "YHCT");
            codes.Add("002713", "DYRS");
            codes.Add("300130", "XGD");
            codes.Add("300128", "JFXC");
            codes.Add("002714", "MYGF");
            codes.Add("300306", "YFGD");
            codes.Add("300299", "FCTX");
            codes.Add("002715", "DYGF");
            codes.Add("300307", "CXGF");
            codes.Add("300300", "HDGF");
            codes.Add("002716", "JGYY");
            codes.Add("300308", "ZJZB");
            codes.Add("300301", "CFZM");
            codes.Add("002717", "LNYL");
            codes.Add("300309", "JAKJ");
            codes.Add("002718", "YBDD");
            codes.Add("300332", "THJN");
            codes.Add("002719", "MQE");
            codes.Add("300333", "ZRKJ");
            codes.Add("002721", "JYWH");
            codes.Add("300334", "JMKJ");
            codes.Add("002722", "JLGF");
            codes.Add("300335", "DSGF");
            codes.Add("002723", "JLT");
            codes.Add("300336", "XWH");
            codes.Add("002724", "HYW");
            codes.Add("300337", "YBGF");
            codes.Add("002725", "YLGF");
            codes.Add("300338", "KYYQ");
            codes.Add("002726", "LDRS");
            codes.Add("300339", "RHRJ");
            codes.Add("002727", "YXT");
            codes.Add("300312", "BXJS");
            codes.Add("002728", "TCZY");
            codes.Add("300313", "TSSW");
            codes.Add("300314", "DWYL");
            codes.Add("300349", "JKGF");
            codes.Add("300323", "HCGD");
            codes.Add("300340", "KHGF");
            codes.Add("300350", "HPF");
            codes.Add("300324", "XJXX");
            codes.Add("300341", "MDDQ");
            codes.Add("300315", "ZQKJ");
            codes.Add("600131", "MJSD");
            codes.Add("300342", "TYJD");
            codes.Add("300316", "JSJD");
            codes.Add("600132", "ZQPJ");
            codes.Add("300343", "LCJN");
            codes.Add("300317", "JWGF");
            codes.Add("600133", "DHGX");
            codes.Add("300344", "TKBY");
            codes.Add("300318", "BHCX");
            codes.Add("600135", "LKJP");
            codes.Add("300345", "HYXC");
            codes.Add("300319", "MJKJ");
            codes.Add("600136", "DBGF");
            codes.Add("300346", "NDGD");
            codes.Add("300320", "HDGF");
            codes.Add("600137", "LSGF");
            codes.Add("300347", "TGYY");
            codes.Add("300321", "TDGF");
            codes.Add("600138", "ZQL");
            codes.Add("300348", "CLKJ");
            codes.Add("300322", "SBD");
            codes.Add("600139", "XBZY");
            codes.Add("600141", "XFJT");
            codes.Add("600143", "JFKJ");
            codes.Add("600148", "CCYD");
            codes.Add("600145", "STG");
            codes.Add("600149", "LFFZ");
            codes.Add("600146", "DYGF");
            codes.Add("600150", "ZGCB");
            codes.Add("300325", "DWXC");
            codes.Add("600151", "HTJD");
            codes.Add("300326", "KLT");
            codes.Add("600152", "WKJH");
            codes.Add("300327", "ZYDZ");
            codes.Add("600153", "JFGF");
            codes.Add("300328", "YAKJ");
            codes.Add("600155", "BSGF");
            codes.Add("300329", "HLGQ");
            codes.Add("300352", "BXY");
            codes.Add("300330", "HHJT");
            codes.Add("300353", "DTKJ");
            codes.Add("300354", "DHCS");
            codes.Add("300355", "MCKH");
            codes.Add("300366", "CYXX");
            codes.Add("600161", "TTSW");
            codes.Add("300356", "GYKJ");
            codes.Add("300367", "DFWL");
            codes.Add("600162", "XJKG");
            codes.Add("300357", "WWSW");
            codes.Add("300368", "HJGF");
            codes.Add("600163", "FJNZ");
            codes.Add("300358", "CTKJ");
            codes.Add("300369", "LMKJ");
            codes.Add("600165", "XRHL");
            codes.Add("300359", "QTJY");
            codes.Add("300370", "AKKJ");
            codes.Add("600166", "FTQC");
            codes.Add("300360", "JHKJ");
            codes.Add("300371", "HZGF");
            codes.Add("600167", "LMKG");
            codes.Add("300362", "TBZZ");
            codes.Add("600157", "YTNY");
            codes.Add("600168", "WHKG");
            codes.Add("300363", "BTGF");
            codes.Add("600158", "ZTCY");
            codes.Add("600169", "TYZG");
            codes.Add("300364", "ZWZX");
            codes.Add("600159", "DLDC");
            codes.Add("600170", "SHJ");
            codes.Add("300365", "HHKJ");
            codes.Add("600160", "JHGF");
            codes.Add("600171", "SHBL");
            codes.Add("600172", "HHXF");
            codes.Add("600173", "WLDC");
            codes.Add("300379", "DFT");
            codes.Add("600175", "MDKG");
            codes.Add("300380", "ASXX");
            codes.Add("600176", "ZGBX");
            codes.Add("300381", "YDL");
            codes.Add("600177", "YGE");
            codes.Add("300382", "SLK");
            codes.Add("300373", "YJKJ");
            codes.Add("300383", "GHXW");
            codes.Add("300375", "PLGF");
            codes.Add("300384", "SLHP");
            codes.Add("300376", "YST");
            codes.Add("300385", "XLHJ");
            codes.Add("300377", "YSS");
            codes.Add("300386", "FTCX");
            codes.Add("300378", "DJRJ");
            codes.Add("300387", "FBGF");
            codes.Add("300388", "GZHB");
            codes.Add("300389", "ABS");
            codes.Add("600186", "LHWJ");
            codes.Add("600197", "YLT");
            codes.Add("300390", "THCJ");
            codes.Add("600187", "GZSW");
            codes.Add("600198", "DTDX");
            codes.Add("300391", "KYKJ");
            codes.Add("600188", "YZMY");
            codes.Add("600199", "JZZJ");
            codes.Add("300392", "TXGF");
            codes.Add("600189", "JLSG");
            codes.Add("300394", "TFTX");
            codes.Add("600179", "HHGF");
            codes.Add("600190", "JZG");
            codes.Add("300395", "FLH");
            codes.Add("600180", "RMT");
            codes.Add("600191", "HZSY");
            codes.Add("300396", "DRYL");
            codes.Add("600182", "SJT");
            codes.Add("600192", "CCDG");
            codes.Add("300397", "THFW");
            codes.Add("600193", "CXZY");
            codes.Add("600183", "SYKJ");
            codes.Add("300398", "FKCL");
            codes.Add("600195", "ZMGF");
            codes.Add("600184", "GDGF");
            codes.Add("300399", "JTL");
            codes.Add("600196", "FXYY");
            codes.Add("600185", "GLDC");
            codes.Add("300400", "JTGF");
            codes.Add("300401", "HYSW");
            codes.Add("300402", "BSGF");
            codes.Add("600201", "JYJT");
            codes.Add("300403", "DEHY");
            codes.Add("600202", "HKD");
            codes.Add("300405", "KLJH");
            codes.Add("600203", "FRDZ");
            codes.Add("300406", "JQSW");
            codes.Add("600206", "YYXC");
            codes.Add("300407", "KFDQ");
            codes.Add("600207", "ACGK");
            codes.Add("300410", "ZYKJ");
            codes.Add("600208", "XHZB");
            codes.Add("300411", "JDGF");
            codes.Add("300416", "SSSY");
            codes.Add("300412", "JNKJ");
            codes.Add("300418", "KLWW");
            codes.Add("300413", "KLG");
            codes.Add("600209", "LDFZ");
            codes.Add("600210", "ZJQY");
            codes.Add("600211", "XCYY");
            codes.Add("600222", "TLYY");
            codes.Add("600003", "STDB");
            codes.Add("600212", "JQSY");
            codes.Add("300420", "WYKJ");
            codes.Add("600004", "BYJC");
            codes.Add("600213", "YXKC");
            codes.Add("300421", "LXGF");
            codes.Add("600005", "WGGF");
            codes.Add("600215", "CCJK");
            codes.Add("300422", "BSK");
            codes.Add("600006", "DFQC");
            codes.Add("600216", "ZJYY");
            codes.Add("300423", "LYT");
            codes.Add("600225", "TJSJ");
            codes.Add("600217", "QLSN");
            codes.Add("300425", "HNKJ");
            codes.Add("600226", "SHBK");
            codes.Add("600218", "QCDL");
            codes.Add("300426", "TDYS");
            codes.Add("600227", "CTH");
            codes.Add("600219", "NSLY");
            codes.Add("300427", "HXDL");
            codes.Add("600007", "ZGGM");
            codes.Add("600220", "JSYG");
            codes.Add("600000", "PFYH");
            codes.Add("600008", "SCGF");
            codes.Add("600221", "HNHK");
            codes.Add("600001", "HDGT");
            codes.Add("600009", "SHJC");
            codes.Add("600010", "BGGF");
            codes.Add("600235", "MFTZ");
            codes.Add("600011", "HNGJ");
            codes.Add("600236", "GGDL");
            codes.Add("600012", "WTGS");
            codes.Add("600237", "TFDZ");
            codes.Add("600228", "STC");
            codes.Add("600238", "HNYD");
            codes.Add("600229", "QDJY");
            codes.Add("600239", "YNCT");
            codes.Add("600230", "CZDH");
            codes.Add("600240", "HYDC");
            codes.Add("600231", "LGGF");
            codes.Add("600241", "SDWH");
            codes.Add("600232", "JYGF");
            codes.Add("600242", "ZCHY");
            codes.Add("600233", "DYCS");
            codes.Add("600243", "QHHD");
            codes.Add("600234", "SSWH");
            codes.Add("600016", "MSYH");
            codes.Add("600017", "RZG");
            codes.Add("600029", "NFHK");
            codes.Add("600249", "LMZ");
            codes.Add("600018", "SGJT");
            codes.Add("600030", "ZXZQ");
            codes.Add("600250", "NFGF");
            codes.Add("600019", "BGGF");
            codes.Add("600031", "SYZG");
            codes.Add("600251", "GNGF");
            codes.Add("600020", "ZYGS");
            codes.Add("600033", "FJGS");
            codes.Add("600252", "ZHJT");
            codes.Add("600021", "SHDL");
            codes.Add("600035", "CTGS");
            codes.Add("600253", "TFYY");
            codes.Add("600022", "SDGT");
            codes.Add("600036", "ZSYH");
            codes.Add("600255", "XKCL");
            codes.Add("600023", "ZNDL");
            codes.Add("600037", "GHYH");
            codes.Add("600256", "GHNY");
            codes.Add("600026", "ZHFZ");
            codes.Add("600038", "HFGF");
            codes.Add("600257", "DHGF");
            codes.Add("600027", "HDG");
            codes.Add("600247", "STCC");
            codes.Add("600258", "SLJD");
            codes.Add("600028", "ZGSH");
            codes.Add("600248", "YCHJ");
            codes.Add("600259", "GSYS");
            codes.Add("600260", "KLKJ");
            codes.Add("600052", "ZJGX");
            codes.Add("600261", "YGZM");
            codes.Add("600053", "ZJDC");
            codes.Add("600262", "BFGF");
            codes.Add("600054", "HSLY");
            codes.Add("600263", "LQJS");
            codes.Add("600055", "HRWD");
            codes.Add("600265", "STJG");
            codes.Add("600056", "ZGYY");
            codes.Add("600266", "BJCJ");
            codes.Add("600057", "XYGF");
            codes.Add("600048", "BLDC");
            codes.Add("600058", "WKFZ");
            codes.Add("600050", "ZGLT");
            codes.Add("600059", "GYLS");
            codes.Add("600051", "NBLH");
            codes.Add("600060", "HXDQ");
            codes.Add("600267", "HZYY");
            codes.Add("600061", "ZFTZ");
            codes.Add("600062", "HRSH");
            codes.Add("600273", "HFFZ");
            codes.Add("600283", "QJSL");
            codes.Add("600063", "WWG");
            codes.Add("600275", "WCY");
            codes.Add("600284", "PDJS");
            codes.Add("600064", "NJGK");
            codes.Add("600276", "HRYY");
            codes.Add("600285", "LRZY");
            codes.Add("600065", "STLY");
            codes.Add("600277", "YLNY");
            codes.Add("600287", "JSST");
            codes.Add("600066", "YTKC");
            codes.Add("600278", "DFCY");
            codes.Add("600288", "DHKJ");
            codes.Add("600067", "GCDT");
            codes.Add("600289", "YYXT");
            codes.Add("600279", "ZQGJ");
            codes.Add("600269", "GYGS");
            codes.Add("600070", "ZJFR");
            codes.Add("600069", "YGTZ");
            codes.Add("600270", "WYFZ");
            codes.Add("600071", "FHGX");
            codes.Add("600280", "ZYSC");
            codes.Add("600271", "HTXX");
            codes.Add("600072", "STG");
            codes.Add("600281", "THGF");
            codes.Add("600272", "KKSY");
            codes.Add("600073", "SHML");
            codes.Add("600282", "STN");
            codes.Add("600074", "ZDGF");
            codes.Add("600084", "ZPGF");
            codes.Add("600075", "STX");
            codes.Add("600085", "TRT");
            codes.Add("600076", "QNHG");
            codes.Add("600086", "DFJY");
            codes.Add("600077", "SDGF");
            codes.Add("600087", "TSCY");
            codes.Add("600078", "CXGF");
            codes.Add("600089", "TBDG");
            codes.Add("600079", "RFYY");
            codes.Add("600090", "PJH");
            codes.Add("600080", "JHGF");
            codes.Add("600091", "STMK");
            codes.Add("600081", "DFKJ");
            codes.Add("600093", "HJGF");
            codes.Add("600082", "HTFZ");
            codes.Add("600094", "DMC");
            codes.Add("600083", "BXGF");
            codes.Add("600095", "HGK");
            codes.Add("600096", "YTH");
            codes.Add("600103", "QSZY");
            codes.Add("600300", "WWGF");
            codes.Add("600097", "KCGJ");
            codes.Add("600104", "SQJT");
            codes.Add("600301", "STNH");
            codes.Add("600098", "GZFZ");
            codes.Add("600105", "YDGF");
            codes.Add("600302", "BZGF");
            codes.Add("600291", "XSGF");
            codes.Add("600106", "ZQLQ");
            codes.Add("600303", "SGGF");
            codes.Add("600292", "ZDYD");
            codes.Add("600107", "MEY");
            codes.Add("600305", "HSCY");
            codes.Add("600293", "SXXC");
            codes.Add("600295", "EEDS");
            codes.Add("600306", "STS");
            codes.Add("600099", "LHGF");
            codes.Add("600108", "YSJT");
            codes.Add("600307", "JGHX");
            codes.Add("600100", "TFGF");
            codes.Add("600297", "MLYY");
            codes.Add("600308", "HTGF");
            codes.Add("600101", "MXDL");
            codes.Add("600298", "AQJM");
            codes.Add("600309", "WHHX");
            codes.Add("600102", "LGGF");
            codes.Add("600299", "STX");
            codes.Add("600310", "GDDL");
            codes.Add("600311", "RHSY");
            codes.Add("600323", "HLHJ");
            codes.Add("600312", "PGDQ");
            codes.Add("600325", "HFGF");
            codes.Add("600315", "SHJH");
            codes.Add("600326", "XCTL");
            codes.Add("600316", "HDHK");
            codes.Add("600327", "DDF");
            codes.Add("600317", "YKG");
            codes.Add("600328", "LTSY");
            codes.Add("600318", "CDGF");
            codes.Add("600329", "ZXYY");
            codes.Add("600319", "YXHX");
            codes.Add("600330", "TTGF");
            codes.Add("600320", "ZHZG");
            codes.Add("600331", "HDGF");
            codes.Add("600321", "GDJS");
            codes.Add("600332", "BYS");
            codes.Add("600322", "TFFZ");
            codes.Add("600333", "CCRQ");
            codes.Add("600335", "GJQC");
            codes.Add("600119", "CJTZ");
            codes.Add("600128", "HYGF");
            codes.Add("600110", "ZKYH");
            codes.Add("600129", "TJJT");
            codes.Add("600120", "ZJDF");
            codes.Add("600111", "BGXT");
            codes.Add("600339", "TLGX");
            codes.Add("600121", "ZZMD");
            codes.Add("600112", "TCKG");
            codes.Add("600340", "HXXF");
            codes.Add("600122", "HTGK");
            codes.Add("600113", "ZJDR");
            codes.Add("600343", "HTDL");
            codes.Add("600123", "LHKC");
            codes.Add("600114", "DMGF");
            codes.Add("600345", "CJTX");
            codes.Add("600337", "MKGF");
            codes.Add("600115", "DFHK");
            codes.Add("600346", "DXS");
            codes.Add("600338", "XCZF");
            codes.Add("600116", "SXSL");
            codes.Add("600348", "YQMY");
            codes.Add("600125", "TLWL");
            codes.Add("600117", "XNTG");
            codes.Add("600350", "SDGS");
            codes.Add("600126", "HGGF");
            codes.Add("600118", "ZGWX");
            codes.Add("600351", "YBYY");
            codes.Add("600127", "JJMY");
            codes.Add("600352", "ZJLS");
            codes.Add("600363", "LCGD");
            codes.Add("600353", "XGGF");
            codes.Add("600365", "TPGF");
            codes.Add("600354", "DHZY");
            codes.Add("600366", "NBY");
            codes.Add("600355", "JLDZ");
            codes.Add("600367", "HXFZ");
            codes.Add("600356", "HFZY");
            codes.Add("600368", "WZJT");
            codes.Add("600357", "CDFT");
            codes.Add("600369", "XNZQ");
            codes.Add("600358", "GLLH");
            codes.Add("600370", "SFX");
            codes.Add("600359", "XNKF");
            codes.Add("600371", "WXDN");
            codes.Add("600360", "HWDZ");
            codes.Add("600372", "ZHDZ");
            codes.Add("600362", "JXTY");
            codes.Add("600609", "JBQC");
            codes.Add("600610", "SST");
            codes.Add("600617", "LHHX");
            codes.Add("600382", "GDMZ");
            codes.Add("600611", "DZJT");
            codes.Add("600824", "YMJT");
            codes.Add("600618", "LJHG");
            codes.Add("600612", "LFX");
            codes.Add("600619", "HLGF");
            codes.Add("600825", "XHCM");
            codes.Add("600613", "SQZY");
            codes.Add("600620", "TCGF");
            codes.Add("600826", "LSGF");
            codes.Add("600614", "DLGF");
            codes.Add("600621", "HXGF");
            codes.Add("600827", "YYGF");
            codes.Add("600373", "ZWCM");
            codes.Add("600377", "NHGS");
            codes.Add("600828", "CSJT");
            codes.Add("600375", "HLXM");
            codes.Add("600378", "TKGF");
            codes.Add("600829", "SJZY");
            codes.Add("600376", "SKGF");
            codes.Add("600379", "BGGF");
            codes.Add("600830", "XYRT");
            codes.Add("600615", "FHGF");
            codes.Add("600380", "JKY");
            codes.Add("600831", "GDWL");
            codes.Add("600616", "JFJY");
            codes.Add("600381", "STX");
            codes.Add("600832", "DFMZ");
            codes.Add("600833", "DYYY");
            codes.Add("600834", "STDT");
            codes.Add("600624", "FDFH");
            codes.Add("600835", "SHJD");
            codes.Add("600626", "SDGF");
            codes.Add("600836", "JLSY");
            codes.Add("600627", "SDGF");
            codes.Add("600837", "HTZ");
            codes.Add("600385", "STJ");
            codes.Add("600838", "SHJB");
            codes.Add("600386", "BBCM");
            codes.Add("600839", "SCCH");
            codes.Add("600387", "HYGF");
            codes.Add("600840", "XHCY");
            codes.Add("600388", "LJHB");
            codes.Add("600622", "JBJT");
            codes.Add("600389", "JSGF");
            codes.Add("600623", "SQGF");
            codes.Add("600390", "JRKJ");
            codes.Add("600391", "CFKJ");
            codes.Add("600841", "SCGF");
            codes.Add("600400", "HDGF");
            codes.Add("600844", "DHKJ");
            codes.Add("600842", "ZXYY");
            codes.Add("600401", "HRGF");
            codes.Add("600636", "SAF");
            codes.Add("600628", "XSJ");
            codes.Add("600403", "DYNY");
            codes.Add("600637", "BST");
            codes.Add("600392", "SHZY");
            codes.Add("600638", "XHP");
            codes.Add("600405", "DLY");
            codes.Add("600393", "DHSY");
            codes.Add("600639", "PDJQ");
            codes.Add("600406", "GDNR");
            codes.Add("600395", "PJGF");
            codes.Add("600630", "LTGF");
            codes.Add("600640", "HBKG");
            codes.Add("600396", "JSGF");
            codes.Add("600631", "BLGF");
            codes.Add("600641", "WYQY");
            codes.Add("600397", "AYMY");
            codes.Add("600633", "ZBCM");
            codes.Add("600642", "SNGF");
            codes.Add("600398", "HLZJ");
            codes.Add("600634", "ZJKG");
            codes.Add("600845", "BXRJ");
            codes.Add("600399", "FSTG");
            codes.Add("600635", "DZG");
            codes.Add("600846", "TJKJ");
            codes.Add("600847", "WLGF");
            codes.Add("600643", "AJGF");
            codes.Add("600848", "ZYGF");
            codes.Add("600644", "LSDL");
            codes.Add("600849", "SHYY");
            codes.Add("600645", "ZYXH");
            codes.Add("600850", "HDDN");
            codes.Add("600647", "TDCY");
            codes.Add("600851", "HXGF");
            codes.Add("600648", "WGQ");
            codes.Add("600853", "LJGF");
            codes.Add("600649", "CTKG");
            codes.Add("600854", "CLGF");
            codes.Add("600650", "JJTZ");
            codes.Add("600855", "HTCF");
            codes.Add("600858", "YZGF");
            codes.Add("600856", "CBJT");
            codes.Add("600859", "WFJ");
            codes.Add("600857", "GDSC");
            codes.Add("600409", "SYHG");
            codes.Add("600410", "HSTC");
            codes.Add("600653", "SHKG");
            codes.Add("600861", "BJCX");
            codes.Add("600415", "XSPC");
            codes.Add("600862", "NTKJ");
            codes.Add("600654", "FLGF");
            codes.Add("600416", "XDGF");
            codes.Add("600863", "NMHD");
            codes.Add("600655", "YYSC");
            codes.Add("600418", "JHQC");
            codes.Add("600425", "QSJH");
            codes.Add("600656", "BYTZ");
            codes.Add("600419", "TRRY");
            codes.Add("600426", "HLHS");
            codes.Add("600657", "XDDC");
            codes.Add("600420", "XDZY");
            codes.Add("600428", "ZYHY");
            codes.Add("600421", "YFKG");
            codes.Add("600429", "SYGF");
            codes.Add("600658", "DZC");
            codes.Add("600422", "KMZY");
            codes.Add("600432", "JENY");
            codes.Add("600660", "FYBL");
            codes.Add("600423", "LHGF");
            codes.Add("600663", "LJZ");
            codes.Add("600661", "XNY");
            codes.Add("600652", "ASGF");
            codes.Add("600664", "HYGF");
            codes.Add("600662", "QSKG");
            codes.Add("600860", "STJ");
            codes.Add("600665", "TDY");
            codes.Add("600871", "STY");
            codes.Add("600865", "BDJT");
            codes.Add("600872", "ZJGX");
            codes.Add("600866", "XHKJ");
            codes.Add("600873", "MHSW");
            codes.Add("600867", "THDB");
            codes.Add("600433", "GHGX");
            codes.Add("600868", "MYJX");
            codes.Add("600435", "BFDH");
            codes.Add("600869", "YDD");
            codes.Add("600436", "PZH");
            codes.Add("600666", "XNYY");
            codes.Add("600438", "TWG");
            codes.Add("600667", "TJSY");
            codes.Add("600671", "TMYY");
            codes.Add("600668", "JFJT");
            codes.Add("600673", "DYGK");
            codes.Add("600870", "XHDZ");
            codes.Add("600674", "CTNY");
            codes.Add("600874", "CYHB");
            codes.Add("600884", "SSGF");
            codes.Add("600461", "HCSY");
            codes.Add("600875", "DFDQ");
            codes.Add("600462", "SXZY");
            codes.Add("600446", "JZGF");
            codes.Add("600876", "LYBL");
            codes.Add("600463", "KGGF");
            codes.Add("600448", "HFGF");
            codes.Add("600877", "ZGJL");
            codes.Add("600466", "DKYY");
            codes.Add("600449", "NXJC");
            codes.Add("600879", "HTDZ");
            codes.Add("600467", "HDJ");
            codes.Add("600452", "FLDL");
            codes.Add("600880", "BRCB");
            codes.Add("600468", "BLDQ");
            codes.Add("600455", "BTGF");
            codes.Add("600881", "YTJT");
            codes.Add("600469", "FSGF");
            codes.Add("600456", "BTGF");
            codes.Add("600882", "HLKY");
            codes.Add("600470", "LGHG");
            codes.Add("600458", "SDXC");
            codes.Add("600444", "GTGY");
            codes.Add("600676", "JYG");
            codes.Add("600459", "GYBY");
            codes.Add("600883", "BWKJ");
            codes.Add("600677", "HTTX");
            codes.Add("600460", "SLW");
            codes.Add("600678", "SCJD");
            codes.Add("600891", "QLJT");
            codes.Add("600679", "JSKF");
            codes.Add("600472", "BTLY");
            codes.Add("600680", "SHPT");
            codes.Add("600892", "BCGF");
            codes.Add("600681", "WHJT");
            codes.Add("600893", "HKDL");
            codes.Add("600682", "NJXB");
            codes.Add("600894", "GRGF");
            codes.Add("600886", "GTDL");
            codes.Add("600895", "ZJGK");
            codes.Add("600887", "YLGF");
            codes.Add("600896", "ZHHS");
            codes.Add("600888", "XJZH");
            codes.Add("600897", "XMKG");
            codes.Add("600889", "NJHX");
            codes.Add("600683", "JTYT");
            codes.Add("600890", "ZFGF");
            codes.Add("600684", "ZJSY");
            codes.Add("600685", "GCGJ");
            codes.Add("600960", "BHHS");
            codes.Add("600479", "QJYY");
            codes.Add("600686", "JLQC");
            codes.Add("600961", "ZYJT");
            codes.Add("600480", "LYGF");
            codes.Add("600687", "GTKG");
            codes.Add("600692", "YTGF");
            codes.Add("600481", "SLJN");
            codes.Add("600688", "SHSH");
            codes.Add("600476", "XYKJ");
            codes.Add("600482", "FFGF");
            codes.Add("600689", "STS");
            codes.Add("600693", "DBJT");
            codes.Add("600483", "FJNF");
            codes.Add("600690", "QDHE");
            codes.Add("600694", "DSG");
            codes.Add("600485", "ZCXC");
            codes.Add("600691", "YMHG");
            codes.Add("600962", "GTZL");
            codes.Add("600486", "YNHG");
            codes.Add("600898", "SLSS");
            codes.Add("600963", "YYLZ");
            codes.Add("600487", "HTGD");
            codes.Add("600900", "CJDL");
            codes.Add("600477", "HXG");
            codes.Add("600488", "TYGF");
            codes.Add("600917", "ZQRQ");
            codes.Add("600478", "KLY");
            codes.Add("600489", "ZJHJ");
            codes.Add("600490", "PXZY");
            codes.Add("600969", "CDGJ");
            codes.Add("600491", "LYJS");
            codes.Add("600970", "ZCGJ");
            codes.Add("600493", "FZFZ");
            codes.Add("600971", "HYMD");
            codes.Add("600495", "JXCZ");
            codes.Add("600973", "BSGF");
            codes.Add("600496", "JGGG");
            codes.Add("600698", "HNTY");
            codes.Add("600497", "CHXZ");
            codes.Add("600699", "JSDZ");
            codes.Add("600696", "DLGF");
            codes.Add("600701", "GDGX");
            codes.Add("600697", "OYJT");
            codes.Add("600975", "XWF");
            codes.Add("600966", "BHZY");
            codes.Add("600976", "WHJM");
            codes.Add("600967", "BFCY");
            codes.Add("600978", "YHMY");
            codes.Add("600499", "KDJN");
            codes.Add("600987", "HMGF");
            codes.Add("600509", "TFRD");
            codes.Add("600500", "ZHGJ");
            codes.Add("600510", "HMD");
            codes.Add("600988", "CFHJ");
            codes.Add("600979", "GAAZ");
            codes.Add("600511", "GYGF");
            codes.Add("600501", "HTCG");
            codes.Add("600980", "BKCC");
            codes.Add("600512", "TDJS");
            codes.Add("600502", "AHS");
            codes.Add("600981", "HHGF");
            codes.Add("600513", "LHYY");
            codes.Add("600503", "HLJZ");
            codes.Add("600982", "NBRD");
            codes.Add("600515", "HDJS");
            codes.Add("600505", "XCDL");
            codes.Add("600983", "HFSY");
            codes.Add("600516", "FDTS");
            codes.Add("600506", "XLGF");
            codes.Add("600984", "JSJX");
            codes.Add("600703", "SAGD");
            codes.Add("600507", "FDTG");
            codes.Add("600985", "LMKH");
            codes.Add("600704", "WCZD");
            codes.Add("600702", "TPSD");
            codes.Add("600986", "KDGF");
            codes.Add("600705", "ZHTZ");
            codes.Add("600508", "SHNY");
            codes.Add("600706", "QJWL");
            codes.Add("600993", "MYL");
            codes.Add("600707", "CHGF");
            codes.Add("600995", "WSDL");
            codes.Add("600708", "HBGF");
            codes.Add("600997", "KLGF");
            codes.Add("600710", "CLGF");
            codes.Add("600715", "SLQC");
            codes.Add("600711", "STKY");
            codes.Add("600716", "FHGF");
            codes.Add("600712", "NNBH");
            codes.Add("600998", "JZT");
            codes.Add("600713", "NJYY");
            codes.Add("600999", "ZSZQ");
            codes.Add("600714", "JRKY");
            codes.Add("601000", "TSG");
            codes.Add("600991", "GQCF");
            codes.Add("601001", "DTMY");
            codes.Add("600992", "GSGF");
            codes.Add("600517", "ZXDQ");
            codes.Add("600518", "KMYY");
            codes.Add("601011", "BTL");
            codes.Add("600726", "HDNY");
            codes.Add("601002", "JYSY");
            codes.Add("601012", "LJGF");
            codes.Add("600727", "LBHG");
            codes.Add("601003", "LGGF");
            codes.Add("600521", "HHYY");
            codes.Add("600718", "DRJT");
            codes.Add("601005", "ZQGT");
            codes.Add("600522", "ZTKJ");
            codes.Add("600719", "DLRD");
            codes.Add("601006", "DQTL");
            codes.Add("600523", "GHGF");
            codes.Add("600720", "QLS");
            codes.Add("601007", "JLFD");
            codes.Add("600525", "CYJT");
            codes.Add("600721", "BHC");
            codes.Add("601008", "LYG");
            codes.Add("600526", "FDHB");
            codes.Add("600722", "JNHG");
            codes.Add("600519", "GZMT");
            codes.Add("600527", "JNGX");
            codes.Add("600723", "SSGF");
            codes.Add("601009", "NJYH");
            codes.Add("600528", "ZTEJ");
            codes.Add("600724", "NBFD");
            codes.Add("601010", "WFGF");
            codes.Add("601016", "JNFD");
            codes.Add("600725", "YWGF");
            codes.Add("601018", "NBG");
            codes.Add("601038", "YTGF");
            codes.Add("601021", "CQHK");
            codes.Add("601058", "SLGF");
            codes.Add("600728", "JDKJ");
            codes.Add("601069", "XBHJ");
            codes.Add("600729", "ZQB");
            codes.Add("601088", "ZGSH");
            codes.Add("600730", "ZGGK");
            codes.Add("601098", "ZNCM");
            codes.Add("600731", "HNHL");
            codes.Add("600529", "SDYB");
            codes.Add("600732", "SHXM");
            codes.Add("600530", "JDAL");
            codes.Add("600733", "SQF");
            codes.Add("600531", "YGJQ");
            codes.Add("600734", "SDJT");
            codes.Add("600532", "HDKY");
            codes.Add("601028", "YLGF");
            codes.Add("600533", "QXJS");
            codes.Add("600535", "TSL");
            codes.Add("601100", "HLYG");
            codes.Add("600547", "SDHJ");
            codes.Add("600536", "ZGRJ");
            codes.Add("600548", "SGS");
            codes.Add("601101", "HHNY");
            codes.Add("600537", "YJGD");
            codes.Add("600549", "XMWY");
            codes.Add("601106", "ZGYZ");
            codes.Add("600538", "GFGF");
            codes.Add("600550", "STT");
            codes.Add("601107", "SCCY");
            codes.Add("600539", "STST");
            codes.Add("600551", "SDCB");
            codes.Add("601111", "ZGGH");
            codes.Add("600540", "XSGF");
            codes.Add("600552", "FXKJ");
            codes.Add("601113", "HDGF");
            codes.Add("600543", "MGGF");
            codes.Add("600553", "TXSN");
            codes.Add("601116", "SJGW");
            codes.Add("600735", "XHJ");
            codes.Add("600555", "JLS");
            codes.Add("601117", "ZGHX");
            codes.Add("600736", "SZG");
            codes.Add("600556", "BSYY");
            codes.Add("601118", "HNXJ");
            codes.Add("601099", "TPY");
            codes.Add("600557", "KYYY");
            codes.Add("600546", "SMGJ");
            codes.Add("600558", "DXY");
            codes.Add("600559", "LBGJ");
            codes.Add("601158", "ZQSW");
            codes.Add("600560", "JZTZ");
            codes.Add("601166", "XYYH");
            codes.Add("600561", "JXCY");
            codes.Add("601168", "XBKY");
            codes.Add("600562", "GRKJ");
            codes.Add("601169", "BJYH");
            codes.Add("600563", "FLDZ");
            codes.Add("601177", "HCQJ");
            codes.Add("600565", "DMGF");
            codes.Add("601179", "ZGXD");
            codes.Add("601137", "BWH");
            codes.Add("601186", "ZGTJ");
            codes.Add("601139", "SZRQ");
            codes.Add("601188", "LJJT");
            codes.Add("600566", "HCGF");
            codes.Add("601198", "DXZQ");
            codes.Add("601199", "JNS");
            codes.Add("600738", "LZMB");
            codes.Add("600744", "HYDL");
            codes.Add("600751", "TJHY");
            codes.Add("600745", "ZYGF");
            codes.Add("600739", "LNCD");
            codes.Add("600753", "DFYH");
            codes.Add("600746", "JSSP");
            codes.Add("600740", "SXJH");
            codes.Add("600754", "JJGF");
            codes.Add("600747", "DLKG");
            codes.Add("600741", "HYQC");
            codes.Add("600755", "XMGM");
            codes.Add("600748", "SSFZ");
            codes.Add("600742", "YQF");
            codes.Add("600756", "LCRJ");
            codes.Add("600749", "XCLY");
            codes.Add("600743", "HYDC");
            codes.Add("600757", "CJCM");
            codes.Add("601225", "SXMY");
            codes.Add("601208", "DCKJ");
            codes.Add("600568", "ZZKG");
            codes.Add("601226", "HDZG");
            codes.Add("601216", "NMJZ");
            codes.Add("600569", "AYGT");
            codes.Add("601231", "HXDZ");
            codes.Add("600570", "HSDZ");
            codes.Add("601218", "JXKJ");
            codes.Add("600750", "JZYY");
            codes.Add("600571", "XYD");
            codes.Add("601222", "LYDZ");
            codes.Add("600572", "KEB");
            codes.Add("600583", "HYG");
            codes.Add("600573", "HQPJ");
            codes.Add("600584", "CDKJ");
            codes.Add("600575", "WHG");
            codes.Add("601238", "GQJT");
            codes.Add("600576", "WHWJ");
            codes.Add("601258", "PDJT");
            codes.Add("600577", "JDGF");
            codes.Add("601268", "STE");
            codes.Add("600578", "JNDL");
            codes.Add("601288", "NYYH");
            codes.Add("600579", "THY");
            codes.Add("601299", "ZGBC");
            codes.Add("600580", "WLDQ");
            codes.Add("601311", "LTGF");
            codes.Add("600581", "BYGT");
            codes.Add("600585", "HLSN");
            codes.Add("600582", "TDKJ");
            codes.Add("600586", "JJKJ");
            codes.Add("600587", "XHYL");
            codes.Add("601390", "ZGZT");
            codes.Add("600767", "YSSY");
            codes.Add("601313", "JNJJ");
            codes.Add("600768", "NBFB");
            codes.Add("601398", "GSYH");
            codes.Add("601318", "ZGPA");
            codes.Add("600769", "XLDY");
            codes.Add("601515", "DFGF");
            codes.Add("601328", "JTYH");
            codes.Add("600770", "ZYGF");
            codes.Add("600759", "ZHGF");
            codes.Add("601333", "GSTL");
            codes.Add("600771", "GYY");
            codes.Add("600760", "ZHHB");
            codes.Add("601336", "XHBX");
            codes.Add("600589", "GDRT");
            codes.Add("600761", "AHHL");
            codes.Add("601339", "BLDF");
            codes.Add("601518", "JLGS");
            codes.Add("600763", "TCYL");
            codes.Add("601369", "SGDL");
            codes.Add("600590", "THKJ");
            codes.Add("600764", "ZDGT");
            codes.Add("601377", "XYZ");
            codes.Add("600591", "STS");
            codes.Add("600765", "ZHZJ");
            codes.Add("601388", "YQZY");
            codes.Add("600592", "LXGF");
            codes.Add("600766", "YCHJ");
            codes.Add("600593", "DLSY");
            codes.Add("600773", "XCCT");
            codes.Add("600594", "YBZY");
            codes.Add("600774", "HSJT");
            codes.Add("600595", "ZFSY");
            codes.Add("600775", "NJXM");
            codes.Add("600596", "XAGF");
            codes.Add("600776", "DFTX");
            codes.Add("600597", "GMRY");
            codes.Add("600777", "XCSY");
            codes.Add("600598", "STD");
            codes.Add("600778", "YHJT");
            codes.Add("600599", "XMYH");
            codes.Add("601555", "DWZQ");
            codes.Add("600600", "QDPJ");
            codes.Add("600603", "DZXY");
            codes.Add("600601", "FZKJ");
            codes.Add("600604", "SBGX");
            codes.Add("600602", "YDDZ");
            codes.Add("601558", "STR");
            codes.Add("600779", "SJF");
            codes.Add("601601", "ZGTB");
            codes.Add("600785", "XHBH");
            codes.Add("600605", "HTNY");
            codes.Add("601628", "ZGRS");
            codes.Add("601607", "SHYY");
            codes.Add("600606", "JFTZ");
            codes.Add("601608", "ZXZG");
            codes.Add("601633", "CCQC");
            codes.Add("600607", "SSYY");
            codes.Add("601636", "QBJT");
            codes.Add("600781", "FRYY");
            codes.Add("601566", "JMW");
            codes.Add("601666", "PMGF");
            codes.Add("600782", "XGGF");
            codes.Add("601567", "SXDQ");
            codes.Add("601678", "BHGF");
            codes.Add("601668", "ZGJ");
            codes.Add("601579", "HJS");
            codes.Add("601616", "GDDQ");
            codes.Add("601669", "ZGDJ");
            codes.Add("601588", "BCS");
            codes.Add("601618", "ZGZY");
            codes.Add("600786", "DGTS");
            codes.Add("601599", "LGKJ");
            codes.Add("600783", "LXCT");
            codes.Add("600787", "ZCGF");
            codes.Add("601600", "ZGLY");
            codes.Add("600784", "LYTZ");
            codes.Add("600789", "LKYY");
            codes.Add("600790", "QFC");
            codes.Add("600791", "JNZY");
            codes.Add("601700", "FFGF");
            codes.Add("600792", "YMNY");
            codes.Add("601717", "ZMJ");
            codes.Add("600793", "STYZ");
            codes.Add("601718", "JHJT");
            codes.Add("600794", "BSKJ");
            codes.Add("601727", "SHDQ");
            codes.Add("600795", "GDDL");
            codes.Add("601766", "ZGNC");
            codes.Add("600796", "QJSH");
            codes.Add("601777", "LFGF");
            codes.Add("600797", "ZDWX");
            codes.Add("601788", "GDZ");
            codes.Add("601688", "HTZQ");
            codes.Add("601789", "NBJG");
            codes.Add("601699", "LAHN");
            codes.Add("600798", "NBHY");
            codes.Add("600800", "TJCK");
            codes.Add("600801", "HXS");
            codes.Add("600804", "PBS");
            codes.Add("600815", "XGGF");
            codes.Add("601798", "LKGX");
            codes.Add("601877", "ZTDQ");
            codes.Add("600805", "YDTZ");
            codes.Add("601799", "XYGF");
            codes.Add("601880", "DLG");
            codes.Add("600806", "KMJC");
            codes.Add("601800", "ZGJJ");
            codes.Add("601886", "JHCJ");
            codes.Add("600807", "TYGF");
            codes.Add("601801", "WXCM");
            codes.Add("601888", "ZGGL");
            codes.Add("600808", "MGGF");
            codes.Add("601808", "ZHYF");
            codes.Add("601890", "YXML");
            codes.Add("600809", "SXFJ");
            codes.Add("601818", "GDYH");
            codes.Add("601898", "ZMNY");
            codes.Add("600810", "SMGF");
            codes.Add("601857", "ZGSY");
            codes.Add("601899", "ZJKY");
            codes.Add("600811", "DFJT");
            codes.Add("601866", "ZHJY");
            codes.Add("601901", "FZZQ");
            codes.Add("600812", "HBZY");
            codes.Add("600803", "WYSH");
            codes.Add("601908", "JYT");
            codes.Add("600814", "HZJB");
            codes.Add("601918", "GTXJ");
            codes.Add("600816", "AXXT");
            codes.Add("601919", "ZGYY");
            codes.Add("600817", "STHS");
            codes.Add("601928", "FHCM");
            codes.Add("600818", "ZLGF");
            codes.Add("601929", "JSCM");
            codes.Add("600819", "YPBL");
            codes.Add("601933", "YHCS");
            codes.Add("600820", "SDGF");
            codes.Add("601939", "JSYH");
            codes.Add("600821", "JQY");
            codes.Add("601958", "JMGF");
            codes.Add("600822", "SHWM");
            codes.Add("601965", "ZGQY");
            codes.Add("601991", "DTFD");
            codes.Add("601969", "HNKY");
            codes.Add("601992", "JYGF");
            codes.Add("601988", "ZGYH");
            codes.Add("601996", "FLJT");
            codes.Add("601998", "ZXYH");
            codes.Add("603010", "WSGF");
            codes.Add("603118", "GJGF");
            codes.Add("601999", "CBCM");
            codes.Add("603011", "HDGF");
            codes.Add("603123", "CWGF");
            codes.Add("603000", "RMW");
            codes.Add("603015", "HXKJ");
            codes.Add("603126", "ZCJN");
            codes.Add("603001", "AKGJ");
            codes.Add("603017", "YQSJ");
            codes.Add("603128", "HMWL");
            codes.Add("603002", "HCDZ");
            codes.Add("603018", "SJGF");
            codes.Add("603166", "FDGF");
            codes.Add("603003", "LYRY");
            codes.Add("603019", "ZKSG");
            codes.Add("603167", "BHLD");
            codes.Add("603005", "JFKJ");
            codes.Add("603088", "NBJD");
            codes.Add("603168", "SPAS");
            codes.Add("603006", "LMGF");
            codes.Add("603099", "ZBS");
            codes.Add("603169", "LSZZ");
            codes.Add("603008", "XLM");
            codes.Add("603100", "CYGF");
            codes.Add("603188", "YBGF");
            codes.Add("603009", "BTKJ");
            codes.Add("603111", "KNJD");
            codes.Add("603222", "JMZY");
            codes.Add("603288", "HTWY");
            codes.Add("603518", "WGNS");
            codes.Add("603306", "HMKJ");
            codes.Add("603555", "GRN");
            codes.Add("603308", "YLGF");
            codes.Add("603558", "JSJT");
            codes.Add("603309", "WLYL");
            codes.Add("603588", "GNHJ");
            codes.Add("603328", "YDDZ");
            codes.Add("603600", "YYGF");
            codes.Add("603366", "RCDF");
            codes.Add("603601", "ZSKJ");
            codes.Add("603368", "LZYY");
            codes.Add("603606", "DFDL");
            codes.Add("603369", "JSY");
            codes.Add("603609", "HFMY");
            codes.Add("603399", "XHL");
            codes.Add("603611", "NLGF");
            codes.Add("603456", "JZYY");
            codes.Add("603618", "HDGF");
            codes.Add("603636", "NWRJ");
            codes.Add("603889", "XAGF");
            codes.Add("900902", "SBBG");
            codes.Add("603678", "HJDZ");
            codes.Add("603898", "HLK");
            codes.Add("900903", "DZBG");
            codes.Add("603686", "LMHW");
            codes.Add("603899", "CGWJ");
            codes.Add("900904", "SQBG");
            codes.Add("603688", "SYGF");
            codes.Add("603939", "YFYF");
            codes.Add("900906", "STZ");
            codes.Add("603699", "NWGF");
            codes.Add("603969", "YLGF");
            codes.Add("900907", "DLBG");
            codes.Add("603766", "LXTY");
            codes.Add("603988", "ZDDJ");
            codes.Add("900908", "LJBG");
            codes.Add("603788", "NBGF");
            codes.Add("603993", "LYMY");
            codes.Add("900909", "SQBG");
            codes.Add("603799", "HYGY");
            codes.Add("603997", "JFGF");
            codes.Add("900910", "HLBG");
            codes.Add("603806", "FST");
            codes.Add("603998", "FSZY");
            codes.Add("900911", "JQBG");
            codes.Add("603828", "KLD");
            codes.Add("900901", "YDBG");
            codes.Add("900912", "WGBG");
            codes.Add("900913", "LHBG");
            codes.Add("900923", "YYBG");
            codes.Add("900914", "JTBG");
            codes.Add("900924", "SGBG");
            codes.Add("900915", "ZLBG");
            codes.Add("900926", "BXB");
            codes.Add("900916", "JSBG");
            codes.Add("900927", "WMBG");
            codes.Add("900917", "HXBG");
            codes.Add("900928", "ZYBG");
            codes.Add("900918", "YPBG");
            codes.Add("900929", "JLBG");
            codes.Add("900919", "DJBG");
            codes.Add("900930", "HPTB");
            codes.Add("900920", "SCBG");
            codes.Add("900932", "LJBG");
            codes.Add("900921", "DKBG");
            codes.Add("900933", "HXB");
            codes.Add("900922", "STS");
            codes.Add("900934", "JJBG");
            codes.Add("900935", "YCBG");
            codes.Add("900946", "TYBG");
            codes.Add("900936", "EZBG");
            codes.Add("900948", "YTBG");
            codes.Add("900937", "HDBG");
            codes.Add("900949", "DDBG");
            codes.Add("900938", "THB");
            codes.Add("900950", "XCBG");
            codes.Add("900939", "HLB");
            codes.Add("900951", "STD");
            codes.Add("900940", "DMCB");
            codes.Add("900952", "JGBG");
            codes.Add("900941", "DXBG");
            codes.Add("900953", "KMB");
            codes.Add("900942", "HSBG");
            codes.Add("900955", "JLSB");
            codes.Add("900943", "KKBG");
            codes.Add("900956", "DBBG");
            codes.Add("900945", "HHBG");
            codes.Add("900957", "LYBG");
            return codes;
        }
        #endregion

        private static Dictionary<string, string> dict = new Dictionary<string, string>();
        public static string GetNameByCode(string code)
        {
            if (dict.Count < 1)
            {
                var lst = codeNames.Split('\n');
                foreach (var item in lst)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        var nv = item.Split(':');
                        if (nv.Length == 2)
                        {
                            dict.Add(nv[0], nv[1]);
                        }
                    }
                }
            }
            if (dict.ContainsKey(code))
            {
                return dict[code];
            }
            else return string.Empty;
        }

        #region Codes
        private static string codeNames = @"000300:沪深300
010107:21国债⑺
010213:02国债⒀
010303:03国债⑶
010504:05国债⑷
010512:05国债⑿
010609:06国债⑼
010616:06国债⒃
010619:06国债⒆
010703:07国债03
010706:07国债06
010710:07国债10
010713:07国债13
018002:国开1302
018003:国开1401
019002:10国债02
019003:10国债03
019005:10国债05
019007:10国债07
019009:10国债09
019010:10国债10
019012:10国债12
019014:10国债14
019015:10国债15
019018:10国债18
019019:10国债19
019022:10国债22
019023:10国债23
019024:10国债24
019026:10国债26
019027:10国债27
019029:10国债29
019031:10国债31
019032:10国债32
019034:10国债34
019037:10国债37
019038:10国债38
019040:10国债40
019041:10国债41
019102:11国债02
019103:11国债03
019105:11国债05
019106:11国债06
019108:11国债08
019110:11国债10
019112:11国债12
019115:11国债15
019116:11国债16
019117:11国债17
019119:11国债19
019121:11国债21
019122:11国债22
019123:11国债23
019124:11国债24
019203:12国债03
019204:12国债04
019205:12国债05
019206:12国债06
019208:12国债08
019209:12国债09
019210:12国债10
019212:12国债12
019213:12国债13
019214:12国债14
019215:12国债15
019216:12国债16
019218:12国债18
019220:12国债20
019221:12国债21
019301:13国债01
019303:13国债03
019305:13国债05
019308:13国债08
019309:13国债09
019310:13国债10
019311:13国债11
019313:13国债13
019315:13国债15
019316:13国债16
019317:13国债17
019318:13国债18
019319:13国债19
019320:13国债20
019323:13国债23
019324:13国债24
019325:13国债25
019401:14国债01
019403:14国债03
019404:14国债04
019405:14国债05
019406:14国债06
019408:14国债08
019409:14国债09
019410:14国债10
019412:14国债12
019413:14国债13
019416:14国债16
019417:14国债17
019419:14国债19
019420:14国债20
019421:14国债21
019424:14国债24
019425:14国债25
019426:14国债26
019427:14国债27
019429:14国债29
019430:14国债30
019502:15国债02
019503:15国债03
019504:15国债04
019505:15国债05
019507:15国债07
019508:15国债08
019510:15国债10
019511:15国债11
019512:15国债12
019513:15国债13
019514:15国债14
019516:15国债16
019517:15国债17
019518:15国债18
019519:15国债19
019520:15国债20
019521:15国债21
019522:15国债22
019523:15国债23
019524:15国债24
019525:15国债25
019526:15国债26
019527:15国债27
019528:15国债28
019529:16国债01
019530:16国债02
019531:16国债03
019532:16国债04
019533:16国债05
019534:16国债06
019535:16国债07
019536:16国债08
019537:16国债09
019538:16国债10
019539:16国债11
019540:16国债12
019541:16国债13
019542:16国债14
019543:16国债15
019802:08国债02
019803:08国债03
019806:08国债06
019810:08国债10
019813:08国债13
019818:08国债18
019820:08国债20
019823:08国债23
019825:08国债25
019902:09国债02
019903:09国债03
019905:09国债05
019907:09国债07
019911:09国债11
019912:09国债12
019916:09国债16
019917:09国债17
019919:09国债19
019920:09国债20
019923:09国债23
019925:09国债25
019926:09国债26
019927:09国债27
019930:09国债30
019932:09国债32
020105:16贴债07
020109:16贴债11
020115:16贴债17
020116:16贴债18
020117:16贴债19
020118:16贴债20
020119:16贴债21
020120:16贴债22
020121:16贴债23
020122:16贴债24
020123:16贴债25
020124:16贴债26
020125:16贴债27
020126:16贴债28
020127:16贴债29
020128:16贴债30
020129:16贴债31
020130:16贴债32
090002:0002质押
090003:0003质押
090005:0005质押
090007:0007质押
090009:0009质押
090012:0012质押
090014:0014质押
090015:0015质押
090018:0018质押
090019:0019质押
090022:0022质押
090023:0023质押
090024:0024质押
090026:0026质押
090027:0027质押
090029:0029质押
090031:0031质押
090032:0032质押
090034:0034质押
090037:0037质押
090038:0038质押
090040:0040质押
090041:0041质押
090099:0010质押
090107:0107质押
090213:0213质押
090303:0303质押
090504:0504质押
090512:0512质押
090609:0609质押
090616:0616质押
090619:0619质押
090703:0703质押
090706:0706质押
090710:0710质押
090713:0713质押
090802:0802质押
090803:0803质押
090806:0806质押
090810:0810质押
090813:0813质押
090818:0818质押
090820:0820质押
090823:0823质押
090825:0825质押
090902:0902质押
090903:0903质押
090905:0905质押
090907:0907质押
090911:0911质押
090912:0912质押
090916:0916质押
090917:0917质押
090919:0919质押
090920:0920质押
090923:0923质押
090925:0925质押
090926:0926质押
090927:0927质押
090930:0930质押
090932:0932质押
091002:11国质02
091003:11国质03
091005:11国质05
091006:11国质06
091008:11国质08
091010:11国质10
091012:11国质12
091015:11国质15
091016:11国质16
091017:11国质17
091019:11国质19
091021:11国质21
091022:11国质22
091023:11国质23
091024:11国质24
091028:12国质03
091029:12国质04
091030:12国质05
091031:12国质06
091033:12国质08
091034:12国质09
091035:12国质10
091037:12国质12
091038:12国质13
091039:12国质14
091040:12国质15
091041:12国质16
091043:12国质18
091045:12国质20
091046:12国质21
091047:13国质01
091049:13国质03
091051:13国质05
091054:13国质08
091055:13国质09
091056:13国质10
091057:13国质11
091059:13国质13
091061:13国质15
091062:13国质16
091063:13国质17
091064:13国质18
091065:13国质19
091066:13国质20
091069:13国质23
091070:13国质24
091071:13国质25
091072:14国质01
091074:14国质03
091075:14国质04
091076:14国质05
091077:14国质06
091079:14国质08
091080:14国质09
091081:14国质10
091083:14国质12
091084:14国质13
091087:14国质16
091088:14国质17
091090:14国质19
091091:14国质20
091092:14国质21
091095:14国质24
091096:14国质25
091097:14国质26
091098:14国质27
091100:14国质29
091101:14国质30
091103:15国质02
091104:15国质03
091105:15国质04
091106:15国质05
091108:15国质07
091109:15国质08
091111:15国质10
091112:15国质11
091113:15国质12
091114:15国质13
091115:15国质14
091117:15国质16
091118:15国质17
091119:15国质18
091120:15国质19
091121:15国质20
091122:15国质21
091123:15国质22
091124:15国质23
091125:15国质24
091126:15国质25
091127:15国质26
091128:15国质27
091129:15国质28
091130:16国质01
091131:16国质02
091132:16国质03
091133:16国质04
091134:16国质05
091135:16国质06
091136:16国质07
091137:16国质08
091138:16国质09
091139:16国质10
091140:16国质11
091141:16国质12
091142:16国质13
091143:16国质14
091144:16国质15
10000555:50ETF购9月1850
10000556:50ETF购9月1900
10000557:50ETF购9月1950
10000558:50ETF购9月2000
10000559:50ETF购9月2050
10000560:50ETF沽9月1850
10000561:50ETF沽9月1900
10000562:50ETF沽9月1950
10000563:50ETF沽9月2000
10000564:50ETF沽9月2050
10000569:50ETF购9月1800
10000570:50ETF沽9月1800
10000571:50ETF购9月2100
10000572:50ETF沽9月2100
10000573:50ETF购9月2150
10000574:50ETF沽9月2150
10000591:50ETF购9月2200
10000592:50ETF沽9月2200
10000595:50ETF购9月2250
10000596:50ETF沽9月2250
10000599:50ETF购9月2300
10000600:50ETF沽9月2300
10000615:50ETF购12月2050
10000616:50ETF购12月2100
10000617:50ETF购12月2150
10000618:50ETF购12月2200
10000619:50ETF购12月2250
10000620:50ETF沽12月2050
10000621:50ETF沽12月2100
10000622:50ETF沽12月2150
10000623:50ETF沽12月2200
10000624:50ETF沽12月2250
10000625:50ETF购12月2000
10000626:50ETF沽12月2000
10000629:50ETF购12月1950
10000630:50ETF沽12月1950
10000631:50ETF购7月1950
10000632:50ETF购7月2000
10000633:50ETF购7月2050
10000634:50ETF购7月2100
10000635:50ETF购7月2150
10000636:50ETF沽7月1950
10000637:50ETF沽7月2000
10000638:50ETF沽7月2050
10000639:50ETF沽7月2100
10000640:50ETF沽7月2150
10000641:50ETF购7月2200
10000642:50ETF沽7月2200
10000643:50ETF购7月2250
10000644:50ETF沽7月2250
10000645:50ETF购8月2000
10000646:50ETF购8月2050
10000647:50ETF购8月2100
10000648:50ETF购8月2150
10000649:50ETF购8月2200
10000650:50ETF沽8月2000
10000651:50ETF沽8月2050
10000652:50ETF沽8月2100
10000653:50ETF沽8月2150
10000654:50ETF沽8月2200
10000655:50ETF购8月2250
10000656:50ETF沽8月2250
10000657:50ETF购7月2300
10000658:50ETF沽7月2300
10000659:50ETF购8月2300
10000660:50ETF沽8月2300
10000661:50ETF购12月2300
10000662:50ETF沽12月2300
10000663:50ETF购7月2350
10000664:50ETF沽7月2350
10000665:50ETF购8月2350
10000666:50ETF沽8月2350
10000667:50ETF购9月2350
10000668:50ETF沽9月2350
10000669:50ETF购12月2350
10000670:50ETF沽12月2350
100980:泉州回售
102000:14繁昌质
102001:14电质02
102002:14天质02
102004:14溧昆质
102005:14乐清质
102016:14忠旺质
102017:14粤高质
102020:14玉质01
102021:14京天质
102022:10句容质
102026:14沪建质
102027:14晋城质
102029:14岳阳质
102030:14鹿城质
102038:14吴中质
102052:14甘质02
102056:16朝国质
102069:14准国质
102086:10乌城质
102096:15东方质
102103:10顺质02
102107:14紫质02
102121:15苏国质
102122:10湘高质
102124:14色质03
102140:15文小质
102149:15粤路质
102158:15东营质
102161:15石国质
102173:15津铁质
102175:15武质01
102176:15武质02
102195:16闽投Z2
102208:15网质01
102209:15网质02
102214:15建发质
102218:10装质02
102242:15当涂质
102244:15中关质
102248:15京科质
102253:15粤质01
102255:15平湖质
102269:15武夷质
102278:15津地质
102279:15浏新质
102282:15贵路质
102283:15大同质
102292:15网质03
102293:15网质04
102294:15天心Z1
102296:15云能Z
102301:15武清质
102313:15东丽质
102318:15闽投质
102319:15日照质
102326:15网质05
102327:15网质06
102345:15盐高质
102352:16恒投Z1
102356:16常城质
102357:16永经质
102359:16穗金质
102362:16闽投Z1
102365:16渝两质
102367:15沪质建
102371:16普兰质
102374:16六盘质
102375:16五家质
102383:16宁经质
102386:16丹投质
102387:16雨城质
102390:16芙蓉质
102391:16瓦沿质
102392:16平交质
102395:16吉城质
102398:16威海质
102399:16鲁信质
102400:16广晟Z1
102405:16盐都质
102409:16渝地质
102413:16皋投质
102415:16三明质
102416:16贾汪质
102418:16启交质
102420:16渝开质
102421:16青小质
102424:16惠开质
102425:16穗港Z1
102426:16渝江Z1
102427:16渤海Z1
102428:16渤海Z2
102429:G16京质1
102430:16淮城质
102431:16洛新质
102434:16晋煤Z1
102436:16惠棚质
102479:16瀚瑞Z1
103000:12奉投质
103001:12漯城质
103002:12蒙高质
103003:12珠水质
103004:12盐城质
103005:12昆创质
103006:12绍城质
103007:12西电质
103008:12国奥质
103010:12鸡国质
103011:12锡科质
103014:12筑住质
103015:12筑工质
103016:12常德质
103017:12伊国质
103018:12昌经质
103019:12湘昭质
103020:12辽城质
103021:12潍东质
103022:12韶金质
103023:12滁城质
103024:12青投质
103025:12池城质
103026:12川广质
103027:12瑞国质
103028:12诸城质
103029:12玉城质
103030:12宁城质
103031:12豫铁质
103032:12宜建质
103033:12苏城质
103034:12郑城质
103035:12沭金质
103036:12张经质
103037:12渝江质
103038:12远洲质
103039:12渝江质
103040:12绍迪质
103041:12宿水质
103042:12鄂旅质
103043:12深立质
103044:12联想质
103045:12嘉经质
103046:12濮建质
103047:12黔宏质
103048:12平国质
103049:12营沿质
103050:12榆城质
103051:12庆高质
103052:12昆产质
103053:12营口质
103054:12株云质
103055:12蓉高质
103056:12启国质
103057:12汕城质
103059:12临城质
103060:12驻投质
103061:12沛国质
103062:12冀顺质
103063:12国03质
103064:12国04质
103065:12津01质
103066:12津02质
103070:12新城质
103071:12鹰投质
103072:12曲公质
103074:12张公质
103075:12淮建质
103076:12金湖质
103077:12榕建质
103078:12云城质
103079:12保国质
103080:12苏海质
103081:12长先质
103082:12青国质
103083:12黄城质
103084:12沪临质
103085:12沪金质
103086:12诸建质
103088:12东台质
103089:12赣开质
103090:12遵国质
103091:12渝兴质
103092:12鄂华质
103093:12喀建质
103094:12甬交质
103095:12六开质
103096:12淮城质
103097:12宝投质
103098:12达投质
103099:12德建质
103100:12石国质
103101:12赣高质
103102:12滇祥质
103103:13同创质
103104:12巢城质
103106:12邯郸质
103107:12洛城质
103108:12吴经质
103110:13宁新质
103111:12长城质
103112:13豫盛质
103113:12渝出质
103114:12大丰质
103116:12渝北质
103117:12宜城质
103118:12香兴质
103119:12环太质
103120:12泉台质
103121:12盘江质
103122:13抚城质
103123:13南城质
103125:13温经质
103126:12柳城质
103127:12黄国质
103128:12萧经质
103130:13陕东质
103131:13安国质
103132:13巩义质
103133:12宁宝质
103134:13泉城质
103135:13滇公质
103136:13太城质
103137:13赣发质
103138:12长城质
103140:13沧建质
103141:13吉利质
103142:13渝三质
103143:13泰投质
103144:13蓉城质
103145:13蓉兴质
103146:13海发质
103147:13甬东质
103148:13金灌质
103149:13镇水质
103150:13南发质
103151:13长兴质
103152:13宁禄质
103153:13网质01
103154:13网质02
103155:13渭城质
103156:13涪国质
103158:13锡东质
103159:13绍城质
103161:13瑞水质
103162:13济高质
103163:13蓉文质
103164:13建城质
103165:13洪市质
103166:13江滨质
103167:13滇投质
103168:13绍中质
103169:13华峰质
103170:13厦杏质
103171:13长投质
103172:13常城质
103173:13陕有质
103174:13吉城质
103175:13湘高质
103176:13武地质
103177:13乌高质
103178:13集城质
103179:13广越质
103180:13綦东质
103182:13精控质
103183:13津广质
103184:13京投质
103185:13海宁质
103187:13泰矿质
103188:13邹城质
103189:13大旅质
103190:13奉南质
103191:13杭高质
103192:13邗城质
103193:13文城质
103194:13滨01质
103195:13滨02质
103196:13烟城质
103198:10朝02质
103199:13泰交质
103200:13自高质
103201:13南高质
103202:13平潭质
103203:13浔富质
103204:13津城质
103205:13余创质
103206:13祥源质
103207:13巴城质
103208:13西投质
103209:13三门质
103210:13皋投质
103211:13甘投质
103212:13益高质
103213:13德清质
103215:13九国质
103216:12新查质
103217:13西高质
103218:13三福质
103219:13荣经质
103220:13晋能质
103221:13武地质
103222:13京粮质
103223:13微山质
103224:13朝国质
103226:13闽兴质
103227:13宁质01
103228:13宁质02
103229:13晋公质
103230:12蓉兴质
103231:13临海质
103232:13苏海质
103234:13鹏质01
103235:13清河质
103236:12马经质
103238:13通辽质
103239:13鄞城质
103240:13合工质
103241:13烟开质
103242:13营经质
103243:13常高质
103244:13番交质
103245:13杭运质
103246:13溧城质
103247:13绍交质
103248:13京歌质
103249:13大城质
103250:13宿建质
103251:13鲁信质
103252:13邯交质
103253:13新乡质
103254:13常熟质
103256:13苏泊质
103257:13海浆质
103258:13潞质01
103259:13潞质02
103260:13遂宁质
103261:13鞍城质
103262:13楚雄质
103263:13临国质
103264:13晋城质
103265:13红河质
103266:13哈水质
103267:13金坛质
103268:13渝南质
103269:13渝大质
103270:13渝万质
103271:13金外质
103272:13绥芬质
103275:13龙岗质
103276:13津滨质
103277:13大丰质
103278:13渝双质
103279:13海拉质
103280:13通经质
103281:13石地质
103283:13武新质
103284:13琼洋质
103285:13同煤质
103286:13海航质
103287:13金特质
103289:13丽城质
103290:13长轨质
103291:13兖城质
103292:13溧城质
103293:13农六质
103294:13苏华质
103295:13宁铁质
103296:13盛江质
103297:13桐乡质
103298:13临汾质
103299:13西经质
103300:13云投质
103301:13日照质
103302:12桂交质
103303:13咸荣质
103304:13合川质
103305:13瓦国质
103306:13鄂三质
103308:13眉宏质
103309:13弘燃质
103310:13洪水质
103311:13弘湘质
103312:13景德质
103313:13苏家质
103314:13筑铁质
103315:13瓯交质
103316:13新郑质
103317:13华发质
103318:13滨城质
103319:13昌国质
103321:13岳城质
103322:13南城质
103323:13天治质
103324:13白银质
103325:13京生质
103326:13郑建质
103327:13中电质
103328:13渝鸿质
103329:12惠国质
103330:13龙工质
103332:13湘振质
103333:13铜城质
103334:13博国质
103335:13海国质
103336:13渝地质
103337:13铜建质
103338:13闽经质
103339:13渝城质
103340:13张保质
103341:13吐番质
103342:13黔南质
103343:13阳江质
103345:13京煤质
103348:13京谷质
103349:13福东质
103350:13晋煤质
103351:13克州质
103352:13平凉质
103354:13商质02
103355:13洼城质
103357:13津房质
103358:13蚌城质
103359:13三明质
103360:13成阿质
103361:13京科质
103362:13钦滨质
103363:13郑投质
103364:13临尧质
103365:13昌润质
103366:13汇丰质
103367:13锡城质
103368:13郑交质
103370:13虞新质
103371:14北辰质
103373:13平天质
103374:13塔国质
103375:13鄂供质
103376:13渝物质
103377:13渝碚质
103378:13湘九质
103379:13许投质
103380:13曹妃质
103381:09衡城质
103382:10云建质
103384:13雅发质
103385:13龙岩质
103386:13新沂质
103387:13湛基质
103388:13任城质
103389:13资水质
103390:13葫质01
103391:13葫质02
103392:13荆门质
103393:13连顺质
103394:13永城质
103395:13堰城质
103396:13姜发质
103397:13郫国质
103398:13株城质
103399:13郴高质
103400:13渝双质
103401:13冀广质
103402:13丹质01
103403:13丹质02
103404:13怀化质
103405:13宝工质
103407:13泰州质
103408:13宛城质
103409:13宿城质
103410:13网质03
103411:13网质04
103412:13金利质
103413:13寿城质
103415:13鄂质01
103416:13鄂质02
103417:13江高质
103418:13永利质
103420:13盐国质
103421:13海新质
103422:13崇明质
103423:13宜环质
103424:13柳东质
103425:13平国质
103426:13澄港质
103427:13临河质
103428:13粤垦质
103429:13亭公质
103430:14城阳质
103431:13普质01
103432:13襄建质
103433:12沪闵质
103434:13渝豪质
103435:13邯城质
103436:13海旅质
103437:13津静质
103438:13冶城质
103439:13六质01
103440:13宁德质
103441:13库质01
103442:13武质01
103443:13黔质01
103444:13六质02
103445:13泰成质
103446:13即墨质
103448:13大质01
103449:13常滨质
103450:13周质01
103451:13濮建质
103452:13府谷质
103454:13清质01
103455:13越都质
103456:13闽投质
103457:13河池质
103458:13镇质01
103459:13随质01
103460:13忻质01
103461:13清远质
103462:13财质01
103463:13津住质
103464:13易质01
103465:13黄质01
103466:13邕城质
103467:13锦质01
103468:13丰质01
103469:13格尔质
103470:13开质01
103471:13缑质01
103472:13海西质
103474:14怀质01
103475:13沈质01
103478:14仪征质
103479:14丰质01
103480:14东质01
103481:13镇质02
103482:14京华质
103483:09渝地质
103485:14苏沿质
103487:14邵城质
103488:14吴兴质
103489:14融质01
103490:14首质01
103491:14皋开质
103492:14江夏质
103493:14伊宁质
103494:14迁质01
103495:14晟晏质
103496:13丰质02
103497:14扬化质
103498:14金质01
103499:13沈质02
103500:13鹏质02
103501:14皋沿质
103502:14宏质01
103504:11鹰投质
103505:14嘉市质
103508:14滕质02
103509:14湘潭质
103510:13开质02
103511:14赣开质
103512:11门投质
103513:14铜旅质
103514:14六开质
103515:14云路质
103516:14泉高质
103517:14怀质02
103518:13忻质02
103519:14淮质01
103520:14太资质
103521:14泉港质
103522:14连普质
103523:14开城质
103524:13黔质02
103525:14毕开质
103526:14邹平质
103527:14榆神质
103529:14泉台质
103530:14海质01
103531:14海质02
103532:14甘质01
103533:14酒经质
103534:14渝中质
103535:14眉山质
103536:14莱开质
103537:14伊财质
103538:14麓城质
103539:14富质01
103540:14汉车质
103541:14文城质
103542:14陆质01
103543:14临港质
103545:14双质01
103546:14丰质02
103547:14海安质
103548:14裕峰质
103549:14新滨质
103551:14长兴质
103552:14如金质
103553:14佳城质
103555:14余城质
103556:14余经质
103557:13易质02
103558:14桥质01
103559:14冶城质
103560:13大质02
103561:14汾湖质
103562:14富蕴质
103563:14吉铁质
103564:14兴安质
103565:14庆城质
103566:14潭两质
103567:14扬开质
103568:14株国质
103569:14嘉经质
103570:14首质02
103571:14高新质
103572:14遂川质
103573:14龙岩质
103574:14攀国质
103575:14汕投质
103576:11滇铁质
103577:14甬广质
103578:14青莱质
103580:14淮开质
103581:13黄质02
103582:14廊经质
103584:14天质01
103585:14南网质
103586:14陆质02
103587:14阜质01
103588:14济宁质
103589:14海晋质
103590:13清质02
103591:14长土质
103592:14并国质
103593:14相城质
103594:14潍东质
103595:14涪陵质
103596:14长影质
103597:14海质01
103598:14济城质
103600:14贵质01
103601:14唐城质
103602:14网质01
103603:14网质02
103604:14富质02
103605:14温质01
103606:14菏泽质
103607:14津环质
103608:14句容质
103609:14常德质
103610:14云铁质
103611:14阜质02
103612:14永城质
103613:14长建质
103614:14桂农质
103615:14昆高质
103616:14鄂质01
103617:14鄂质02
103618:14粤科质
103620:14渝黔质
103621:14宣国质
103622:14钦临质
103623:14穗质01
103624:14盛质01
103625:11宁宝质
103626:13渝质02
103627:09青州质
103628:14启质01
103629:14苏金质
103630:14库城质
103631:14漕开质
103632:14合桃质
103633:14淄高质
103634:14牡国质
103635:14滕质01
103636:14防城质
103637:14融质02
103638:14信阳质
103639:14沭金质
103641:14江宁质
103643:14冀高质
103644:14沈国质
103645:14临经质
103646:14宁经质
103647:14娄底质
103648:14郴州质
103649:14邳润质
103650:13财质02
103651:14杨农质
103652:14益交质
103653:14遂河质
103654:14庆经质
103655:13缑质02
103656:14永国质
103657:14张经质
103658:14桂城质
103659:14平经质
103661:14赣四质
103662:14京投质
103663:14威经质
103665:14余交质
103666:14莱山质
103667:14苏元质
103668:14滇公质
103671:14湛新质
103672:14徐开质
103673:14火炬质
103674:14泰中质
103675:14崇川质
103676:14衢国质
103677:14乌城质
103678:14宁开质
103679:14宜经质
103680:13普质02
103681:14徐高质
103683:13周质02
103684:14新城质
103685:14临淄质
103686:14昌平质
103687:14南化质
103688:14潜城质
103689:14雨城质
103690:14中电质
103691:14宏质02
103692:14嘉公质
103693:14新凯质
103694:14克投质
103695:14广元质
103696:14东质02
103697:14马城质
103698:14奉化质
103699:14汇通质
103700:14内江质
103701:14临开质
103702:14衡水质
103703:14蓉隆质
103704:13武质02
103705:13库质02
103706:14巴国质
103707:14渝质01
103708:14兖质01
103709:14安吉质
103710:14兴国质
103711:14象山质
103712:14并经质
103713:14黔铁质
103715:14四平质
103716:14宁国质
103718:14乌房质
103720:14电质01
103721:14青海质
103722:14包滨质
103723:14启质02
103724:14富山质
103725:14曲靖质
103726:14德高质
103727:14渝保质
103729:14兰新质
103730:14长质01
103731:14朝建质
103732:14渝高质
103733:14开发质
103734:13随质02
103735:14合建质
103736:14柳龙质
103737:14虞交质
103738:14安发质
103739:14西塞质
103740:14青经质
103741:14辽鑫质
103742:14贵质02
103743:14银城质
103744:14萧经质
103745:14武安质
103746:14贺城质
103747:14太仓质
103748:14铜示质
103749:14仁城质
103750:14宜春质
103751:14徐高质
103752:14文金质
103753:14控质01
103754:14荥城质
103755:14合工质
103756:14紫质01
103757:14城质01
103758:14吉安质
103759:14威新质
103760:14余城质
103761:14深业质
103763:14昆交质
103764:14蔡家质
103765:14醴质01
103766:14景洪质
103767:14郑二质
103768:14云城质
103769:14力质01
103770:14力质02
103771:14亳建质
103773:14温质02
103774:14通辽质
103775:14新余质
103776:14绿地质
103777:14茂交质
103778:14蔡甸质
103779:14银开质
103781:14渝质02
103782:14遵国质
103783:14绍袍质
103785:14青州质
103786:14苏海质
103787:14荣经质
103790:14陶都质
103791:14孝城质
103792:14桓台质
103793:14合质01
103794:14合质02
103795:14渝惠质
103796:14襄高质
103797:14十二质
103799:14京鑫质
103800:14金城质
103801:14恩城质
103802:14保山质
103803:14津宁质
103804:14津南质
103805:14穗质02
103807:14金国质
103808:14唐丰质
103809:14龙国质
103810:14一师质
103811:14滇投质
103812:14长质02
103814:14郑投质
103815:14瑞质02
103816:14顺德质
103817:14北国质
103818:14常德质
103819:14渝旅质
103820:14济高质
103821:14百色质
103822:14合滨质
103824:14金桥质
103827:14普国质
103828:14日经质
103829:14孝质01
103830:14桂铁质
103831:14崇建质
103832:14睢宁质
103833:14如东质
103834:14德城质
103835:14渝南质
103836:14大石质
103837:14赤城质
103839:14滨新质
103840:14漳九质
103841:14清质01
103842:14神木质
103845:14晋开质
103846:14瘦西质
103847:14济源质
103848:14元国质
103850:14合川质
103851:14梧东质
103852:14冀渤质
103853:14淄城质
103854:14喀什质
103855:14淮城质
103856:14常房质
103857:14临桂质
103858:14黄海质
103859:14柳产质
103860:14汤建质
103862:14台基质
103863:14沂科质
103864:14兴城质
103865:14奎屯质
103866:14南二质
103867:09绍交质
103868:14冀融质
103869:14渝长质
103871:14绿国质
103872:14杭拱质
103873:14盛质02
103874:14哈质01
103875:14哈质02
103876:14郑高质
103877:14莱国质
103878:14苏高质
103879:14淮质02
103880:14曲经质
103882:14江北质
103883:14西永质
103884:14双质02
103885:14临城质
103886:14长农质
103887:14盐城质
103888:14邹城质
103889:14定国质
103890:14甘电质
103891:14株质01
103893:14文登质
103894:14资质02
103895:14筑经质
103896:14北港质
103897:14津广质
103898:14津水质
103899:14穗质03
103900:14滨开质
103901:14虞城质
103903:14鹤投质
103904:14连旅质
103906:14迁质02
103907:14芜宜质
103908:14靖江质
103909:14超威质
103910:14石景质
103911:14北科质
103912:13锦质02
103913:14绍交质
103914:14慈建质
103915:14桥质02
103916:14新开质
103917:14沣西质
103918:14沪南质
103921:14浏阳质
103924:14白沙质
103926:14阜宁质
103927:14玉溪质
103928:14九龙质
103929:14巴质01
103930:14堰城质
103931:09晋交质
103932:14阿克质
103933:14揭城质
103934:14渝港质
103935:14冀建质
103937:14湖中质
103938:14郴百质
103940:14滁城质
103941:14钦滨质
103942:14南绿质
103943:14兰国质
103946:14保利质
103948:14金质02
103949:14随建质
103951:14威中质
103952:14马高质
103953:09宁城质
103956:14锑都质
103957:14滨投质
103958:14浔富质
103960:14胶发质
103961:14苏望质
103962:14武经质
103963:14广安质
103964:14自高质
103965:14济西质
103966:14京国质
103968:14盐东质
103969:14醴质02
103970:14杭地质
103974:14泸纳质
103975:14溧经质
103976:14张掖质
103977:14瑞质03
103978:14新密质
103979:14陂城质
103980:14抚质01
103983:14孝质02
103984:14城质02
103985:14兖质02
103986:14建开质
103987:14昆经质
103988:14闽投质
103999:13武续质
104000:07长电质
104001:07海工质
104004:07华质3
104007:08莱钢质
104008:08华能质
104015:09长电质
104016:09中材质
104017:09大唐质
104019:09中质G2
104021:09广汇质
104028:09华发质
104032:09隧道质
104037:09三友质
104041:09招金质
104043:09紫江质
104046:10中质G2
104048:10首质02
104049:10营口质
104050:10杉杉质
104052:石化质02
104054:10中铁Z3
104055:10中铁Z4
104057:10龙质02
104059:10重钢质
104062:11西02质
104064:11龙02质
104066:11大01质
104067:11南钢质
104069:11海02质
104071:11海02质
104072:11大连质
104073:11云维质
104075:11柳钢质
104077:11西钢质
104080:11康美质
104081:11星湖质
104083:11天威质
104085:11深高质
104087:11凌钢质
104090:11马02质
104091:11重机质
104093:11中孚质
104094:11海正质
104096:11健康质
104097:11浦路质
104099:11连质02
104100:11华仪质
104102:11广01质
104103:11航质01
104105:11安质02
104106:11唐01质
104107:11安质01
104108:11新质01
104109:11新质02
104110:11众和质
104111:11永泰质
104112:11沪大质
104113:11新钢质
104114:11一重质
104115:11华质01
104116:11华质02
104118:12兴质01
104119:12兴质02
104121:11日照质
104124:11化质02
104125:11美兰质
104126:11庞质02
104127:11欧亚质
104131:11片仔质
104132:12鹏博质
104133:11柳化质
104135:12宝泰质
104136:11复星质
104138:11桂质01
104139:11洪水质
104141:12天质01
104143:12亿质01
104144:12鲁信质
104145:11桂质02
104146:12华质01
104147:12华质02
104148:11吉高质
104149:12石质01
104150:12石质02
104151:12电质01
104152:12电质02
104154:12京质02
104155:12天富质
104156:12厦工质
104157:12广质01
104158:12西钢质
104159:12亿质02
104162:12中孚质
104163:12鄂资质
104164:12通威质
104166:12电质04
104167:12兖01质
104168:12兖02质
104172:12海质02
104173:12交质01
104174:12交质02
104175:12交质03
104176:12中储质
104178:12科质02
104179:12科质03
104180:12旋风质
104181:12山鹰质
104182:12九州质
104183:12集01质
104184:12重01质
104186:12力质02
104187:12玻纤质
104188:12华质03
104189:12王01质
104190:12王02质
104191:12桂01质
104192:12桂02质
104193:12水01质
104194:12水02质
104195:12海质03
104196:12海质04
104197:12华天质
104199:12能质02
104200:12晋兰质
104201:12滦质01
104202:12螺01质
104203:12螺02质
104204:12双良质
104205:12沪交质
104207:12骆驼质
104208:12招金质
104209:12油01质
104210:12油02质
104211:12油03质
104212:12京江质
104213:12松建质
104215:12永01质
104216:12桐昆质
104217:12渝水质
104218:12航质01
104219:12榕泰质
104221:12重质02
104222:12永质02
104224:12气02质
104225:12拖01质
104226:12宝科质
104227:13尖质01
104228:13天质01
104229:12控01质
104230:12沪海质
104231:12上电质
104232:12招质01
104233:12招质02
104234:12招质03
104235:12芜湖质
104236:12哈质01
104237:12西资质
104239:13中01质
104240:13中02质
104241:12东01质
104242:12广质01
104243:12广质02
104244:12大质01
104245:13甬热质
104247:13福质01
104248:13福质02
104249:13平煤质
104250:13和质01
104251:13车01质
104252:13车02质
104253:12拖02质
104254:12拜质01
104255:13赣01质
104257:12岳质01
104258:13云煤质
104259:13信质01
104260:13信质02
104261:13华质01
104262:13华质02
104263:12豫质01
104264:13京客质
104265:13路桥质
104266:13信质03
104267:13永泰质
104268:12航质02
104269:12航质03
104270:13安信质
104271:12兖03质
104272:12兖04质
104273:13鲁质01
104274:11航质02
104276:13魏质01
104278:13华质02
104279:13外质01
104280:13通质01
104281:13通质02
104282:13通质03
104283:13盛屯质
104284:13鲁质02
104285:13杉杉质
104286:13中信质
104287:13投质01
104288:13东吴质
104289:13照港质
104292:13兴质01
104293:13兴质02
104294:12鲁创质
104295:13川质01
104298:13亚盛质
104299:13中原质
104301:13楚质01
104304:13兴质03
104305:14鲁高质
104306:13太质01
104308:13杭齿质
104310:13苏新质
104311:13通质04
104312:13通质05
104313:13通质06
104315:14东海质
104316:14赣质01
104317:14赣质02
104318:14炬质01
104319:13京质02
104320:14贸质01
104321:14银质G1
104322:14银质G2
104323:14凤凰质
104324:14国电Z1
104328:12滦质02
104329:14伊质01
104330:13中企质
104331:14营口质
104332:14亿质01
104333:14嘉宝质
104334:12大质02
104335:14爱质01
104336:13牡质01
104337:13魏质02
104338:13金桥质
104339:13香江质
104340:14武质01
104341:14连云质
104342:13包质03
104343:13和质02
104344:13尖质02
104345:12重质03
104346:14贵人质
104347:13太质02
104348:14辰质01
104349:14炬质02
104351:14辰质02
104352:12广质03
104353:14东兴质
104354:15康美质
104355:14齐鲁质
104356:14富贵质
104357:14浙证质
104358:15际质03
104360:14华质G1
104361:14福田质
104362:14实质01
104363:14太证质
104364:14路质01
104365:14昊质01
104366:14武钢质
104367:14财富质
104368:14路质02
104369:13包质04
104370:14华远质
104371:14亨质01
104372:14财通质
104373:15舟港质
104374:14招商质
104375:14苏新质
104376:15首质01
104377:14首开质
104378:13楚质02
104379:14西质01
104380:14瀚质01
104381:14安源质
104382:14京银质
104383:15恒质01
104384:15中质01
104385:15中质02
104386:15迪马质
104387:15城质01
104388:15华质G1
104390:15龙质01
104391:15云能质
104392:15恒质02
104393:15恒质03
104394:15中银质
104395:15富力质
104396:15时代质
104397:15宜质01
104398:15北巴质
104399:15投质G1
104401:15远质03
104402:15建质01
104403:15天恒质
104404:14西质02
104405:15宜质02
104406:15新湖质
104407:15广证质
104408:15美都质
104409:15龙质02
104410:15龙质03
104411:14招金质
104412:15昆药质
104413:15精工质
104414:15物质01
104415:15增质01
104416:15好民质
104417:15东旭质
104418:15盛和质
104419:15天风质
104420:15奥园质
104422:15梅质01
104423:15五洋质
104424:15华业质
104425:15际质01
104426:15际质02
104427:15正质01
104428:15信质01
104429:15海质01
104430:15增质02
104431:15闽高质
104432:15融质01
104433:15融质02
104434:15清能质
104435:15兴发质
104436:15远质02
104437:15远质01
104438:15祥源质
104439:15红豆质
104440:15光质01
104441:15赣长质
104442:15焦质01
104443:15桂金质
104444:15冠城质
104445:15融质03
104446:15万质01
104447:15物质02
104448:15光质02
104449:15绿质01
104450:15齐鲁质
104451:15九鼎质
104452:15杭质01
104453:15联发Z1
104454:15洋质02
104455:15绿质02
104456:15绿质03
104457:15新金质
104458:15泛质02
104459:15平高质
104460:15路建质
104461:15杭质02
104462:15正奇质
104463:15花样质
104464:15世质01
104465:15越质01
104466:15越质02
104467:15万质02
104468:15五质01
104469:15五质02
104470:15泛质01
104473:15联发Z2
104474:15格房质
104475:15亿质01
104476:15天瑞质
104477:15月质01
104478:14粤运Z1
104479:15南质01
104480:15南质02
104481:15铁质01
104482:15金茂质
104483:15光Z01
104484:15龙质01
104485:15厦住质
104486:15旭质01
104487:15盈德质
104488:15金地Z1
104489:15西建质
104490:15三质01
104491:15藏城质
104492:15光Z02
104493:14国电Z3
104494:15华夏Z5
104495:14亨质02
104496:15世质02
104497:15远质04
104498:15远质05
104499:15哈投Z1
104500:12郴城质
104501:12寿财质
104502:12哈合质
104503:12并龙质
104504:12通天质
104505:12绍袍质
104506:12吴交质
104507:12玉交质
104508:12兴林质
104509:12白中质
104510:12靖新质
104513:12伟星质
104514:12金融质
104515:12庆城质
104516:12青质01
104517:12青质02
104518:12保利质
104519:12锡经质
104520:12唐城质
104521:12筑金质
104522:12兴城质
104523:12亮01质
104524:12亮02质
104525:12沪嘉质
104526:12永川质
104527:12温国质
104530:12七城质
104531:12太科质
104532:12宜财质
104533:12平城质
104535:12苏飞质
104536:12慈国质
104537:12克城质
104538:12榕城质
104539:12阜城质
104540:12浦口质
104541:12宁上质
104542:12阿信质
104543:12钦开质
104544:12渝长质
104545:12蒙高质
104546:12宁高质
104547:12曲靖质
104549:12邳润质
104550:12苏信质
104551:12东泰质
104552:12新业质
104553:12虞交质
104554:12定海质
104555:12常经质
104556:12咸宁质
104557:12株高质
104558:12昆质01
104559:12昆质02
104560:12淄城质
104561:12饶城质
104562:12伊春质
104563:12毫州质
104564:12椒江质
104565:12邵城质
104566:12库城质
104567:12清河质
104568:12随州质
104569:12津生质
104570:12滇水质
104571:12兴国质
104572:12蓉投质
104573:12牡国质
104574:12淮开质
104575:12肥城质
104576:12内江质
104577:12苏相质
104578:12长宁质
104580:12临安质
104581:12津南质
104582:12湘九质
104583:12遵投质
104584:12松城质
104585:12新海质
104586:12中交质
104587:12遵桥质
104588:12益城质
104589:12毕信质
104590:12鹤城质
104591:12常交质
104592:12乌国质
104593:12衡城质
104594:12泉质01
104595:12泉质02
104596:12沪城质
104597:12宝钛质
104598:12荆门质
104599:12梵投质
104600:12鑫泰质
104601:12白山质
104602:12松城质
104603:12穗经质
104604:12吉铁质
104605:11宁海质
104606:12顺鑫质
104607:12渝地质
104608:12西永质
104609:12扬城质
104610:12乐清质
104611:12蓉质01
104612:12蓉质02
104613:12乌海质
104614:12渝缙质
104615:12百色质
104616:12黔铁质
104617:12襄投质
104618:12统众质
104619:12迁安质
104620:12乌城质
104621:12赣城质
104622:12锦城质
104623:12旅建质
104624:12滨开质
104625:12升华质
104626:12海恒质
104627:12京建质
104628:12东投质
104629:12平发质
104630:12惠投质
104631:12晋国质
104632:12江阴质
104633:12嘉经质
104634:12芜质01
104635:12芜质02
104636:12连发质
104637:12鑫城质
104638:12华信质
104639:12绍新质
104641:12武进质
104642:12朝阳质
104643:12海资质
104644:12铁岭质
104645:12苏园质
104648:12宣国质
104649:12长建质
104650:12泰能质
104652:12杨农质
104654:12昆钢质
104655:12铜建质
104658:12盘锦质
104659:12石质06
104660:12石质07
104661:12怀化质
104662:12合桃质
104663:12科发质
104664:12葫芦质
104665:12镇交质
104666:12国01质
104667:12国02质
104668:12凉国质
104669:12桂林质
104670:12新盛质
104671:12扬子质
104672:12西城质
104675:12杭城质
104676:12滨江质
104677:12江宁质
104678:12扬化质
104679:12河套质
104680:12昆建质
104681:12合农质
104682:12营口质
104683:12春和质
104684:12合高质
104685:12吉城质
104686:12白药质
104687:12金坛质
104688:12华通质
104689:12宿开质
104690:12三胞质
104691:12武清质
104692:12漳路质
104693:12佳城质
104694:12兴荣质
104695:12五国质
104697:11太资质
104701:12余城质
104702:12海安质
104703:12鞍城质
104704:12江都质
104705:12苏交质
104706:12海门质
104707:12泰兴质
104708:12伊旗质
104709:12绵阳质
104710:12济城质
104711:12郑新质
104712:12中航质
104713:12冀交质
104714:12海陵质
104715:12蓉新质
104716:12莆田质
104717:12泉矿质
104718:12渝南质
104719:12龙交质
104720:12甬城质
104721:12辽国质
104722:12淮水质
104723:12石质05
104724:12攀国质
104725:12宿产质
104726:12柳东质
104727:12东胜质
104728:12徐经质
104729:12江泉质
104730:12晋江质
104731:12镇经质
104732:12九江质
104733:11京质01
104734:11京质02
104735:11六安质
104736:12石质03
104737:12石质04
104740:12延城质
104742:12鲁高质
104743:12华发质
104744:11本溪质
104745:12方01质
104746:12方02质
104747:12晋煤质
104748:12石质01
104749:12石质02
104750:12常经质
104751:11冀新质
104752:11大丰质
104753:12姜国质
104754:11通化质
104755:12潭城质
104756:12甘农质
104757:11丹东质
104758:11张保质
104759:11泰豪质
104760:12渝富质
104761:11萧国质
104762:11吴江质
104763:11淮产质
104764:11泛01质
104765:11泛02质
104766:11宜建质
104767:11盐城质
104768:11兰城质
104769:11龙海质
104770:11国01质
104771:11国02质
104772:11山煤质
104773:11滨质01
104774:11滨质02
104775:11咸城质
104776:11新光质
104777:11吴中质
104778:11建发质
104779:11株城质
104780:11长高质
104781:11永州质
104782:11宁农质
104783:11苏中质
104784:11中兴质
104786:11联想质
104787:11赣铁质
104788:11三明质
104789:11象屿质
104790:11诸暨质
104792:11邯郸质
104794:11海城质
104795:11诸城质
104796:11冀01质
104797:11冀02质
104798:11泰矿质
104799:11武国质
104801:11焦作质
104802:11辽阳质
104803:11滁州质
104804:11渭质01
104805:11大同质
104806:11渭质02
104807:11东岭质
104808:11滕州质
104809:11准国质
104811:11蒙伦质
104812:11淮北质
104813:11宁交质
104814:11东营质
104815:11广汇质
104817:11三门质
104818:11牟平质
104819:11常城质
104820:11潍东质
104821:11吉城质
104823:11舟山质
104824:11中煤质
104825:11景德质
104826:11北港质
104827:11新奥质
104828:11抚州质
104829:11万基质
104830:11沈国质
104831:11惠通质
104832:11泰山质
104833:11赣城质
104834:11牡国质
104835:11兴泸质
104836:11盘锦质
104837:11武经质
104838:11吉利质
104839:11鑫泰质
104840:11临汾质
104841:11渝津质
104842:11合城质
104843:11绥化质
104844:11筑城质
104845:11横店质
104846:11渝富质
104847:11甬交质
104848:10桂林质
104849:11新余质
104850:11华泰质
104851:10玉01质
104852:10玉02质
104854:11中汇质
104855:11渝轻质
104856:11株高质
104857:10九华质
104858:10盐质01
104859:10盐质02
104860:10龙源质
104861:09陕煤质
104862:10闽能质
104863:10榆城质
104864:11外滩质
104865:10苏海质
104866:10杭交质
104867:11石城质
104868:10沈煤质
104869:10沪化质
104871:10镇交质
104872:10复星质
104873:10通经质
104874:10红质01
104875:10红质02
104876:11海控质
104877:10渝南质
104880:10天业质
104881:10吴江质
104882:10宁高质
104883:10楚雄质
104884:10西子质
104885:10冀交质
104886:10云投质
104887:10渝交质
104888:10华靖质
104889:10冶色质
104890:10凯迪质
104891:10通辽质
104892:10寿光质
104893:10丹东质
104894:10洪市质
104895:10德州质
104896:10芜开质
104897:10襄投质
104898:10攀国质
104899:10浦质01
104900:10浦质02
104901:10营债质
104902:10赤峰质
104903:10盐东质
104904:10长投质
104905:10南昌质
104906:10芜质01
104907:10芜质02
104911:10鞍城质
104912:10鄂国质
104914:09榕建质
104915:10镇水质
104916:10红谷质
104919:10鲁商质
104920:10黄山质
104921:10郴州质
104922:10长高质
104923:10北汽质
104924:10巢湖质
104925:09沈国质
104927:09海航质
104928:09铁岭质
104930:09盘锦质
104931:09临海质
104932:09宜城质
104934:09南质2
104935:09南通质
104936:09鹤城质
104937:10辽源质
104939:09吉安质
104940:09咸城质
104941:10镇城质
104942:09江阴质
104944:09株城质
104945:09虞水质
104946:09扬城质
104956:09常高质
104961:09武城质
104965:09潍投质
104969:09豫投质
104975:09济城质
104995:08合建质
104999:08广纸质
105018:08江铜质
105710:1010基质
105720:1020基质
105823:格力转质
105824:电气转质
105825:航信转质
105826:三一转质
105827:国贸转质
105828:九州转质
105829:广汽转质
105830:白云转质
105831:江南转质
105900:0102企质
105901:0201企质
105902:0204企质
105906:0301企质
105908:0303企质
105911:0306企质
105919:0486企质
105922:0490企质
105927:0506企质
105928:0508企质
105932:0512企质
105942:0527企质
105944:0529企质
105945:0601企质
105946:0602企质
105947:0603企质
105949:0605企质
105951:0607企质
105952:0608企质
105953:0609企质
105954:0610企质
105955:0701企质
105956:0702企质
105960:0203企质
106063:11地质04
106065:11地质06
106067:11地质08
106069:11沪质02
106071:11粤质02
106073:11浙质02
106075:11深质02
106077:12地质02
106079:12地质04
106081:12地质06
106083:12地质08
106084:12沪质01
106085:12沪质02
106086:12粤质01
106087:12粤质02
106089:12地质10
106090:12浙质01
106091:12浙质02
106092:12深质01
106093:12深质02
106095:13地质02
106097:13地质04
106098:13地质05
106099:13地质06
106100:13地质07
106101:13地质08
106102:13山质01
106103:13山质02
106104:13地质09
106105:13沪质01
106106:13沪质02
106107:13地质10
106108:13粤质01
106109:13粤质02
106110:13江质01
106111:13江质02
106112:13地质11
106113:13地质12
106114:13浙质01
106115:13浙质02
106116:13深质01
106117:13深质02
106118:14地质01
106119:14地质02
106120:14粤质01
106121:14粤质02
106122:14粤质03
106123:14地质03
106124:14地质04
106125:14地质05
106126:14山质01
106127:14山质02
106128:14山质03
106129:14地质06
106130:14地质07
106131:14地质08
106132:14江质01
106133:14江质02
106134:14江质03
106135:14江质01
106136:14江质02
106137:14江质03
106138:14宁质01
106139:14宁质02
106140:14宁质03
106141:14地质09
106142:14地质10
106143:14青质01
106144:14青质02
106145:14青质03
106146:14浙质01
106147:14浙质02
106148:14浙质03
106149:14北质01
106150:14北质02
106151:14北质03
106152:14上质01
106153:14上质02
106154:14上质03
106155:14地质11
106156:14地质12
106157:14地质13
106158:14深质01
106159:14深质02
106160:14深质03
106161:15江质01
106162:15江质02
106163:15江质03
106164:15江质04
106165:15新质01
106166:15新质02
106167:15新质03
106168:15新质04
106169:15湖质01
106170:15湖质02
106171:15湖质03
106172:15湖质04
106173:15桂质01
106174:15桂质02
106175:15桂质03
106176:15桂质04
106177:15鲁质01
106178:15鲁质02
106179:15鲁质03
106180:15鲁质04
106181:15渝质01
106182:15渝质02
106183:15渝质03
106184:15渝质04
106185:15贵质01
106186:15贵质02
106187:15贵质03
106188:15贵质04
106189:15皖质01
106190:15皖质02
106191:15皖质03
106192:15皖质04
106193:15津质01
106194:15津质02
106195:15津质03
106196:15津质04
106197:15鄂质05
106198:15鄂质06
106199:15鄂质07
106200:15鄂质08
106201:15浙质01
106202:15浙质02
106203:15浙质03
106204:15浙质04
106205:15冀质01
106206:15冀质02
106207:15冀质03
106208:15冀质04
106209:15吉质01
106210:15吉质02
106211:15吉质03
106212:15吉质04
106213:15晋质01
106214:15晋质02
106215:15晋质03
106216:15晋质04
106217:15河质Z1
106218:15河质Z2
106219:15河质Z3
106220:15广质01
106221:15广质02
106222:15广质03
106223:15广质04
106224:15江质01
106225:15江质02
106226:15江质03
106227:15江质04
106228:15宁质01
106229:15宁质02
106230:15宁质03
106231:15宁质04
106232:15新质05
106233:15新质06
106234:15新质07
106235:15新质08
106236:15川质01
106237:15川质02
106238:15川质03
106239:15川质04
106240:15豫质01
106241:15豫质02
106242:15豫质03
106243:15豫质04
106244:15辽质01
106245:15辽质02
106246:15辽质03
106247:15辽质04
106248:15云质01
106249:15云质02
106250:15云质03
106251:15云质04
106252:15青质01
106253:15青质02
106254:15青质03
106255:15青质04
106256:15琼质01
106257:15琼质02
106258:15琼质03
106259:15琼质04
106260:15江质Z1
106261:15江质Z2
106262:15江质Z3
106263:15陕质01
106264:15陕质02
106265:15陕质03
106266:15陕质04
106267:15鲁质05
106268:15鲁质06
106269:15鲁质07
106270:15鲁质08
106271:15大质01
106272:15大质02
106273:15大质03
106274:15大质04
106275:15大质Z1
106276:15大质Z2
106277:15大质Z3
106278:15大质Z4
106279:15黔质05
106280:15黔质06
106281:15黔质07
106282:15黔质08
106283:15内质01
106284:15内质02
106285:15内质03
106286:15内质04
106287:15新质Z1
106288:15新质Z2
106289:15新质Z3
106290:15新质Z4
106291:15京质01
106292:15京质02
106293:15京质03
106294:15京质04
106295:15川质05
106296:15川质06
106297:15川质07
106298:15川质08
106299:15甘质01
106300:15甘质02
106301:15甘质03
106302:15甘质04
106303:15青质01
106304:15青质02
106305:15青质03
106306:15青质04
106307:15宁质01
106308:15宁质02
106309:15宁质03
106310:15宁质04
106311:15宁质Z1
106312:15宁质Z2
106313:15宁质Z3
106314:15宁质Z4
106315:15粤质Z1
106316:15粤质Z2
106317:15粤质Z3
106318:15闽质01
106319:15闽质02
106320:15闽质03
106321:15闽质04
106322:15湘质01
106323:15湘质02
106324:15湘质03
106325:15湘质04
106326:15鄂质09
106327:15鄂质10
106328:15鄂质11
106329:15鄂质12
106330:15鄂质Z1
106331:15鄂质Z2
106332:15鄂质Z3
106333:15鄂质Z4
106334:15桂质05
106335:15桂质06
106336:15桂质07
106337:15桂质08
106338:15桂质Z1
106339:15桂质Z2
106340:15粤质05
106341:15粤质06
106342:15粤质07
106343:15粤质08
106344:15鲁质Z1
106345:15鲁质Z2
106346:15鲁质Z3
106347:15闽质Z1
106348:15闽质Z2
106349:15闽质05
106350:15闽质06
106351:15闽质07
106352:15闽质08
106353:15黑质01
106354:15黑质02
106355:15黑质03
106356:15黑质04
106357:15黑质Z1
106358:15黑质Z2
106359:15黑质Z3
106360:15云质Z1
106361:15云质Z2
106362:15云质Z3
106363:15云质Z4
106364:15渝质05
106365:15渝质06
106366:15渝质07
106367:15渝质08
106368:15渝质Z1
106369:15渝质Z2
106370:15新质09
106371:15新质10
106372:15新质11
106373:15新质12
106374:15新质Z5
106375:15新质Z6
106376:15新质Z7
106377:15新质Z8
106378:15沪质01
106379:15沪质02
106380:15沪质03
106381:15沪质04
106382:15沪质Z1
106383:15沪质Z2
106384:15辽质05
106385:15辽质06
106386:15辽质07
106387:15辽质08
106388:15辽质Z1
106389:15辽质Z2
106390:15青质05
106391:15青质06
106392:15青质07
106393:15青质08
106394:15青质Z1
106395:15青质Z2
106396:15青质Z3
106397:15津质05
106398:15津质06
106399:15津质07
106400:15津质08
106401:15津质Z1
106402:15津质Z2
106403:15津质Z3
106404:15甘质05
106405:15甘质06
106406:15甘质07
106407:15甘质08
106408:15甘质Z1
106409:15甘质Z2
106410:15皖质05
106411:15皖质06
106412:15皖质07
106413:15皖质08
106414:15皖质09
106415:15皖质Z1
106416:15皖质Z2
106417:15厦质01
106418:15厦质02
106419:15厦质03
106420:15厦质04
106421:15厦质Z1
106422:15厦质Z2
106423:15海质05
106424:15海质06
106425:15海质07
106426:15海质08
106427:15海质Z1
106428:15海质Z2
106429:15海质Z3
106430:15海质Z4
106431:15京质Z1
106432:15京质Z2
106433:15京质Z3
106434:15京质Z4
106435:15陕质05
106436:15陕质06
106437:15陕质07
106438:15陕质08
106439:15陕质Z1
106440:15陕质Z2
106441:15陕质Z3
106442:15陕质Z4
106443:15陕质Z5
106444:15陕质Z6
106445:15陕质Z7
106446:15陕质Z8
106447:15豫质05
106448:15豫质06
106449:15豫质07
106450:15豫质08
106451:15豫质Z1
106452:15豫质Z2
106453:15豫质Z3
106454:15豫质Z4
106455:15内质05
106456:15内质06
106457:15内质07
106458:15内质08
106459:15内质Z1
106460:15内质Z2
106461:15内质Z3
106462:15内质Z4
106463:15宁质05
106464:15宁质06
106465:15宁质07
106466:15宁质08
106467:15苏质05
106468:15苏质06
106469:15苏质07
106470:15苏质08
106471:15苏质Z4
106472:15苏质Z5
106473:15苏质Z6
106474:15苏质Z7
106475:15鲁质09
106476:15鲁质10
106477:15鲁质11
106478:15鲁质12
106479:15鲁质Z4
106480:15鲁质Z5
106481:15鲁质Z6
106482:15鲁质Z7
106483:15新质13
106484:15新质14
106485:15新质15
106486:15新质16
106487:15新质Z9
106488:15新质17
106489:15新质18
106490:15新质19
106491:15桂质09
106492:15桂质10
106493:15桂质11
106494:15桂质12
106495:15桂质Z3
106496:15桂质Z4
106497:15浙质05
106498:15浙质06
106499:15浙质07
106500:15浙质08
106501:15浙质Z1
106502:15浙质Z2
106503:15浙质Z3
106504:15浙质Z4
106505:15冀质05
106506:15冀质06
106507:15冀质07
106508:15冀质08
106509:15冀质Z4
106510:15冀质Z5
106511:15黔质09
106512:15黔质10
106513:15黔质11
106514:15黔质12
106515:15云质05
106516:15云质06
106517:15云质07
106518:15云质08
106519:15云质Z5
106520:15云质Z6
106521:15云质Z7
106522:15云质Z8
106523:15闽质09
106524:15闽质10
106525:15闽质11
106526:15闽质12
106527:15闽质Z3
106528:15闽质Z4
106529:15海质09
106530:15海质10
106531:15海质11
106532:15海质12
106533:15鄂质13
106534:15鄂质14
106535:15鄂质15
106536:15鄂质16
106537:15鄂质Z5
106538:15鄂质Z6
106539:15鄂质Z7
106540:15鄂质Z8
106541:15川质09
106542:15川质10
106543:15川质11
106544:15川质12
106545:15粤质09
106546:15粤质10
106547:15粤质11
106548:15粤质12
106549:15粤质Z4
106550:15粤质Z5
106551:15粤质Z6
106552:15琼质05
106553:15琼质06
106554:15琼质07
106555:15琼质08
106556:15琼质Z1
106557:15琼质Z2
106558:15琼质Z3
106559:15浙质09
106560:15浙质10
106561:15浙质11
106562:15浙质12
106563:15浙质Z5
106564:15浙质Z6
106565:15浙质Z7
106566:15浙质Z8
106567:15甘质09
106568:15甘质10
106569:15甘质11
106570:15甘质12
106571:15甘质Z3
106572:15甘质Z4
106573:15赣质05
106574:15赣质06
106575:15赣质07
106576:15赣质08
106577:15赣质Z1
106578:15赣质Z2
106579:15赣质Z3
106580:15赣质Z4
106581:15赣质Z5
106582:15赣质Z6
106583:15赣质Z7
106584:15赣质Z8
106585:15沪质05
106586:15沪质06
106587:15沪质07
106588:15沪质Z3
106589:15沪质Z4
106590:15沪质Z5
106591:15沪质Z6
106592:15川质Z1
106593:15川质Z2
106594:15川质Z3
106595:15川质Z4
106596:15闽质13
106597:15闽质14
106598:15闽质15
106599:15闽质16
106600:15闽质Z5
106601:15闽质Z6
106602:15闽质Z7
106603:15闽质Z8
106604:15皖质10
106605:15皖质11
106606:15皖质12
106607:15皖质13
106608:15皖质Z3
106609:15皖质Z4
106610:15宁质09
106611:15宁质10
106612:15宁质11
106613:15宁质12
106614:15夏质Z1
106615:15夏质Z2
106616:15夏质Z3
106617:15夏质Z4
106618:15夏质Z5
106619:15夏质Z6
106620:15津质09
106621:15津质10
106622:15津质11
106623:15津质12
106624:15津质Z4
106625:15津质Z5
106626:15津质Z6
106627:15粤质13
106628:15粤质14
106629:15粤质15
106630:15粤质16
106631:15晋质05
106632:15晋质06
106633:15晋质07
106634:15晋质08
106635:15晋质Z1
106636:15晋质Z2
106637:15豫质09
106638:15豫质10
106639:15豫质11
106640:15豫质12
106641:15豫质Z5
106642:15豫质Z6
106643:15豫质Z7
106644:15豫质Z8
106645:15黔质Z1
106646:15黔质Z2
106647:15黔质Z3
106648:15黔质Z4
106649:15苏质09
106650:15苏质10
106651:15苏质11
106652:15苏质12
106653:15苏质Z8
106654:15苏质Z9
106655:15苏质13
106656:15苏质14
106657:15云质09
106658:15云质10
106659:15云质11
106660:15云质12
106661:15云质Z9
106662:15云质13
106663:15云质14
106664:15云质15
106665:15内质09
106666:15内质10
106667:15内质11
106668:15内质12
106669:15内质Z5
106670:15内质Z6
106671:15内质Z7
106672:15内质Z8
106673:15甬质05
106674:15甬质06
106675:15甬质07
106676:15甬质08
106677:15甬质Z5
106678:15甬质Z6
106679:15甬质Z7
106680:15甬质Z8
106681:15厦质05
106682:15厦质06
106683:15厦质07
106684:15厦质08
106685:15厦质Z3
106686:15厦质Z4
106687:15陕质09
106688:15陕质10
106689:15陕质11
106690:15陕质12
106691:15陕质Z9
106692:15陕质13
106693:15陕质14
106694:15陕质15
106695:15黑质05
106696:15黑质06
106697:15黑质07
106698:15黑质08
106699:15黑质Z4
106700:15黑质Z5
106701:15大质05
106702:15大质06
106703:15大质07
106704:15大质08
106705:15大质Z5
106706:15大质Z6
106707:15大质Z7
106708:15大质Z8
106709:15吉质05
106710:15吉质06
106711:15吉质07
106712:15吉质08
106713:15吉质Z1
106714:15吉质Z2
106715:15吉质Z3
106716:15吉质Z4
106717:15京质05
106718:15京质06
106719:15京质07
106720:15京质08
106721:15京质Z5
106722:15京质Z6
106723:15京质Z7
106724:15京质Z8
106725:15京质Z9
106726:15湘质05
106727:15湘质06
106728:15湘质07
106729:15湘质08
106730:15沪质08
106731:15沪质09
106732:15鲁质13
106733:15鲁质14
106734:15鲁质15
106735:15鲁质16
106736:15黔质13
106737:15黔质14
106738:15黔质15
106739:15黔质16
106740:15黔质Z5
106741:15黔质Z6
106742:15黔质Z7
106743:15黔质Z8
106744:15浙质13
106745:15浙质14
106746:15浙质15
106747:15浙质16
106748:15青质09
106749:15青质10
106750:15青质11
106751:15青质12
106752:15闽质17
106753:15闽质18
106754:15闽质19
106755:15闽质20
106756:15闽质Z9
106757:15闽质21
106758:15内质13
106759:15内质14
106760:15内质15
106761:15内质16
106762:15内质Z9
106763:15内质17
106764:15辽质09
106765:15辽质10
106766:15辽质11
106767:15辽质12
106768:15甘质13
106769:15甘质14
106770:15甘质15
106771:15甘质16
106772:15晋质09
106773:15晋质10
106774:15晋质11
106775:15晋质12
106776:15黔质17
106777:15黔质18
106778:15黔质19
106779:15黔质20
106780:16鄂质01
106781:16鄂质02
106782:16鄂质03
106783:16鄂质04
106784:16粤质01
106785:16粤质02
106786:16粤质03
106787:16粤质04
106788:16粤质05
106789:16粤质06
106790:16粤质07
106791:16浙质01
106792:16浙质02
106793:16浙质03
106794:16浙质04
106795:16鲁质01
106796:16鲁质02
106797:16鲁质03
106798:16鲁质04
106799:16鲁质05
106800:16鲁质06
106801:16鲁质07
106802:16鲁质08
106803:16内质01
106804:16内质02
106805:16内质03
106806:16内质04
106807:16苏质01
106808:16苏质02
106809:16苏质03
106810:16苏质04
106811:16苏质05
106812:16苏质06
106813:16苏质07
106814:16苏质08
106815:16渝质01
106816:16渝质02
106817:16渝质03
106818:16渝质04
106819:16渝质05
106820:16渝质06
106821:16津质01
106822:16津质02
106823:16津质03
106824:16津质04
106825:16津质05
106826:16云质01
106827:16云质02
106828:16云质03
106829:16云质04
106830:16新质01
106831:16新质02
106832:16新质03
106833:16新质04
106834:16赣质01
106835:16赣质02
106836:16赣质03
106837:16赣质04
106838:16赣质05
106839:16赣质06
106840:16赣质07
106841:16赣质08
106842:16宁质01
106843:16宁质02
106844:16宁质03
106845:16宁质04
106846:16桂质01
106847:16桂质02
106848:16桂质03
106849:16桂质04
106850:16桂质05
106851:16桂质06
106852:16川质01
106853:16川质02
106854:16川质03
106855:16川质04
106856:16辽质01
106857:16辽质02
106858:16辽质03
106859:16辽质04
106860:16皖质01
106861:16皖质02
106862:16皖质03
106863:16皖质04
106864:16青质01
106865:16青质02
106866:16青质03
106867:16青质04
106868:16粤质08
106869:16粤质09
106870:16粤质10
106871:16粤质11
106872:16粤质12
106873:16粤质13
106874:16粤质14
106875:16桂质07
106876:16桂质08
106877:16桂质09
106878:16桂质10
106879:16新质05
106880:16新质06
106881:16新质07
106882:16新质08
106883:16新质09
106884:16新质10
106885:16新质11
106886:16新质12
106887:16黔质01
106888:16黔质02
106889:16黔质03
106890:16黔质04
106891:16黔质05
106892:16黔质06
106893:16黔质07
106894:16黔质08
106895:16黑质01
106896:16黑质02
106897:16黑质03
106898:16黑质04
106899:16黑质05
106900:16黑质06
106901:16黑质07
106902:16黑质08
106903:16湘质01
106904:16湘质02
106905:16豫质01
106906:16豫质02
106907:16豫质03
106908:16豫质04
106909:16冀质01
106910:16冀质02
106911:16冀质03
106912:16冀质04
106913:16冀质05
106914:16冀质06
106915:16冀质07
106916:16冀质08
106917:16鄂质05
106918:16鄂质06
106919:16鄂质07
106920:16鄂质08
106921:16鄂质09
106922:16鄂质10
106923:16甘质01
106924:16甘质02
106925:16甘质03
106926:16甘质04
106927:16甘质05
106928:16晋质01
106929:16晋质02
106930:16晋质03
106931:16晋质04
106932:16鲁质09
106933:16鲁质10
106934:16鲁质11
106935:16鲁质12
106936:16陕质01
106937:16陕质02
106938:16陕质03
106939:16陕质04
106940:16陕质05
106941:16陕质06
106942:16陕质07
106943:16陕质08
106944:16湘质03
106945:16湘质04
106946:16琼质01
106947:16琼质02
106948:16琼质03
106949:16甬质01
106950:16甬质02
106951:16甬质03
106952:16甬质04
106953:16甬质05
106954:16甬质06
106955:16甬质07
106956:16甬质08
106957:16岛质01
106958:16岛质02
106959:16岛质03
106960:16岛质04
106961:16岛质05
106962:16岛质06
106963:16岛质07
106964:16川质05
106965:16川质06
106966:16川质07
106967:16川质08
106968:16川质09
106969:16川质10
106970:16川质11
106971:16川质12
106972:16宁质05
106973:16宁质06
106974:16宁质07
106975:16宁质08
106976:16辽质05
106977:16辽质06
106978:16辽质07
106979:16辽质08
106980:16云质05
106981:16云质06
106982:16云质07
106983:16云质08
106984:16陕质09
106985:16陕质10
106986:16陕质11
106987:16陕质12
106988:16陕质13
106989:16陕质14
106990:16陕质15
106991:16陕质16
106992:16陕质17
106993:16陕质18
106994:16陕质19
106995:16陕质20
106996:16青质05
106997:16青质06
106998:16青质07
106999:16青质08
107105:16贴质07
107109:16贴质11
107115:16贴质17
107116:16贴质18
107117:16贴质19
107118:16贴质20
107119:16贴质21
107120:16贴质22
107121:16贴质23
107122:16贴质24
107123:16贴质25
107124:16贴质26
107125:16贴质27
107126:16贴质28
107127:16贴质29
107128:16贴质30
107129:16贴质31
107130:16贴质32
108002:国质1302
108003:国质1401
110030:格力转债
110031:航信转债
110032:三一转债
110033:国贸转债
110034:九州转债
110035:白云转债
113008:电气转债
113009:广汽转债
113010:江南转债
120102:01三峡债
120201:02三峡债
120203:02中移⒂
120204:02苏交通
120301:03沪轨道
120303:03三峡债
120306:03中电投
120486:04国电⑵
120490:04南网⑵
120506:05大唐债
120508:05铁道债
120512:05沪建⑵
120527:05武城投
120529:05宁煤债
120601:06大唐债
120602:06冀建投
120603:06航天债
120605:06三峡债
120607:水务暂停
120608:06鲁高速
120609:06赣投债
120610:06合城投
120701:07世博⑴
120702:07世博⑵
122000:07长电债
122001:07海工债
122004:07华能G3
122007:08莱钢债
122008:08华能G1
122015:09长电债
122016:09中材债
122017:09大唐债
122019:09中交G2
122021:09广汇债
122027:09京城建
122028:09华发债
122032:09隧道债
122037:09三友债
122041:09招金债
122043:09紫江债
122046:10中铁G2
122048:10首机02
122049:10营口港
122050:10杉杉债
122052:10石化02
122054:10中铁G3
122055:10中铁G4
122057:10龙源02
122059:10重钢债
122060:10银鸽债
122062:11西矿02
122064:11龙源02
122066:11大唐01
122067:11南钢债
122069:11海螺02
122071:11海航02
122072:11大连港
122073:云债暂停
122075:11柳钢债
122077:11西钢债
122080:11康美债
122081:PR星债停
122083:11天威债
122085:11深高速
122087:11凌钢债
122088:11综艺债
122090:11马钢02
122091:11重机债
122093:11中孚债
122094:11海正债
122096:11健康元
122097:11浦路桥
122099:11连港02
122100:11华仪债
122102:11广汇01
122103:11航机01
122105:11安钢02
122106:11唐新01
122107:11安钢01
122108:11新天01
122109:11新天02
122110:11众和债
122111:11永泰债
122112:11沪大众
122113:11新钢债
122114:11一重债
122115:11华锐01
122116:11华锐02
122118:12兴发01
122119:12兴发02
122121:11日照港
122124:11中化02
122125:11美兰债
122126:11庞大02
122127:11欧亚债
122131:11片仔癀
122132:12鹏博债
122133:11柳化债
122134:11华微债
122135:12宝泰隆
122136:11复星债
122138:11桂东01
122139:11洪水业
122141:12天士01
122142:11鹿港债
122143:12亿利01
122144:12鲁信债
122145:11桂东02
122146:12华新01
122147:12华新02
122148:11吉高速
122149:12石化01
122150:12石化02
122151:12国电01
122152:12国电02
122154:12京能02
122155:12天富债
122156:12厦工债
122157:12广控01
122158:12西钢债
122159:12亿利02
122162:12中孚债
122163:12鄂资债
122164:12通威发
122166:12国电04
122167:12兖煤01
122168:12兖煤02
122169:12金瑞债
122172:12中海02
122173:12中交01
122174:12中交02
122175:12中交03
122176:12中储债
122178:科环02停
122179:科环03停
122180:12旋风债
122181:12山鹰债
122182:12九州通
122183:12集优01
122184:12一重01
122186:12力帆02
122187:12玻纤债
122188:12华新03
122189:12王府01
122190:12王府02
122191:12桂冠01
122192:12桂冠02
122193:12中水01
122194:12中水02
122195:12中海03
122196:12中海04
122197:12华天成
122199:12能新02
122200:12晋兰花
122201:12开滦01
122202:12海螺01
122203:12海螺02
122204:12双良节
122205:12沪交运
122207:12骆驼集
122208:12招金券
122209:12中油01
122210:12中油02
122211:12中油03
122212:12京江河
122213:12松建化
122215:12永泰01
122216:12桐昆债
122217:12渝水务
122218:12国航01
122219:12榕泰债
122221:12重工02
122222:12永泰02
122224:12电气02
122225:12一拖01
122226:12宝科创
122227:13尖峰01
122228:13天士01
122229:12国控01
122230:12沪海立
122231:12上电债
122232:12招商01
122233:12招商02
122234:12招商03
122235:12芜湖港
122236:12哈电01
122237:12西资源
122239:13中油01
122240:13中油02
122241:12东航01
122242:12广汽01
122243:12广汽02
122244:12大唐01
122245:13甬热电
122247:13福新01
122248:13福新02
122249:13平煤债
122250:13和邦01
122251:13南车01
122252:13南车02
122253:12一拖02
122254:12拜克01
122255:13赣粤01
122256:13保税债
122257:12岳纸01
122258:13云煤业
122259:13中信01
122260:13中信02
122261:13华泰01
122262:13华泰02
122263:12豫园01
122264:13京客隆
122265:13川路桥
122266:13中信03
122267:13永泰债
122268:12国航02
122269:12国航03
122270:13安信债
122271:12兖煤03
122272:12兖煤04
122273:13鲁金01
122274:11航民02
122276:13魏桥01
122278:13华域02
122279:13外运债
122280:13海通01
122281:13海通02
122282:13海通03
122283:13盛屯债
122284:13鲁金02
122285:13杉杉债
122286:13中信建
122287:13国投01
122288:13东吴债
122289:13日照港
122292:13兴业01
122293:13兴业02
122294:12鲁创投
122295:13川投01
122298:13亚盛债
122299:13中原债
122301:13楚天01
122302:13天房债
122304:13兴业03
122305:14鲁高速
122306:13太极01
122308:13杭齿债
122310:13苏新城
122311:13海通04
122312:13海通05
122313:13海通06
122315:14东海债
122316:14赣粤01
122317:14赣粤02
122318:14中炬01
122319:13京能02
122320:14国贸01
122321:14银河G1
122322:14银河G2
122323:14凤凰债
122324:14国电01
122326:14广晟债
122327:13卧龙债
122328:12开滦02
122329:14伊泰01
122330:中债暂停
122331:14营口港
122332:14亿利01
122333:14嘉宝债
122334:12大唐02
122335:14爱众01
122336:13牡丹01
122337:13魏桥02
122338:13金桥债
122339:13香江债
122340:14武控01
122341:14连云港
122342:13包钢03
122343:13和邦02
122344:13尖峰02
122345:12重工03
122346:14贵人鸟
122347:13太极02
122348:14北辰01
122349:14中炬02
122350:14盛屯债
122351:14北辰02
122352:12广汽03
122353:14东兴债
122354:15康美债
122355:14齐鲁债
122356:14富贵鸟
122357:14浙证债
122358:15际华03
122360:14华融G1
122361:14福田债
122362:14上实01
122363:14太证债
122364:14渝路01
122365:14昊华01
122366:14武钢债
122367:14财富债
122368:14渝路02
122369:13包钢04
122370:14华远债
122371:14亨通01
122372:14财通债
122373:15舟港债
122374:14招商债
122375:14苏新债
122376:15首置01
122377:14首开债
122378:13楚天02
122379:14西南01
122380:14瀚华01
122381:14安源债
122382:14京银债
122383:15恒大01
122384:15中信01
122385:15中信02
122386:15迪马债
122387:15城乡01
122388:15华泰G1
122390:15龙湖01
122391:15云能投
122392:15恒大02
122393:15恒大03
122394:15中银债
122395:15富力债
122396:15时代债
122397:15宜华01
122398:15北巴债
122399:15中投G1
122401:15远洋03
122402:15城建01
122403:15天恒债
122404:14西南02
122405:15宜华02
122406:15新湖债
122407:15广证债
122408:15美都债
122409:15龙湖02
122410:15龙湖03
122411:14招金债
122412:15昆药债
122413:15精工债
122414:15物美01
122415:15增碧01
122416:15好民居
122417:15东旭集
122418:15盛和债
122419:15天风债
122420:15奥园债
122421:15天房债
122422:15梅花01
122423:15五洋债
122424:15华业债
122425:15际华01
122426:15际华02
122427:15海正01
122428:15信投01
122429:15海亮01
122430:15增碧02
122431:15闽高速
122432:15融创01
122433:15融创02
122434:15清能债
122435:15兴发债
122436:15远洋02
122437:15远洋01
122438:15祥源债
122439:15红豆债
122440:15龙光01
122441:15赣长运
122442:15鲁焦01
122443:15桂金债
122444:15冠城债
122445:15融创03
122446:15万达01
122447:15物美02
122448:15龙光02
122449:15绿城01
122450:15齐鲁债
122451:15九鼎债
122452:15杭实01
122453:15联发01
122454:15五洋02
122455:15绿城02
122456:15绿城03
122457:15新金债
122458:15泛海02
122459:15平高债
122460:15粤路建
122461:15杭实02
122462:15正奇债
122463:15花样年
122464:15世茂01
122465:15广越01
122466:15广越02
122467:15万达02
122468:15五矿01
122469:15五矿02
122470:15泛海03
122472:15盛屯债
122473:15联发02
122474:15格房产
122475:15亿达01
122476:15天瑞债
122477:15月星01
122478:14粤运01
122479:15南铝01
122480:15南铝02
122481:15铁建01
122482:15金茂债
122483:15新光01
122484:15龙源01
122485:15厦住宅
122486:15旭辉01
122487:15盈德债
122488:15金地01
122489:15西建工
122490:15三福01
122491:15藏城投
122492:15新光02
122493:14国电03
122494:15华夏05
122495:14亨通02
122496:15世茂02
122497:15远洋04
122498:15远洋05
122499:15哈投01
122500:PR郴城投
122501:PR寿财资
122502:12哈合力
122503:PR并龙城
122504:PR通天诚
122505:PR绍袍江
122506:PR吴交投
122507:PR玉交投
122508:PR兴林业
122509:PR白中兴
122510:PR靖新城
122513:12伟星集
122514:12金融街
122515:PR庆城投
122516:PR青州01
122517:PR青州02
122518:12保利集
122519:PR锡经开
122520:PR唐城投
122521:PR筑金阳
122522:PR兴城建
122523:12海亮01
122524:12海亮02
122525:PR沪嘉开
122526:PR永川惠
122527:PR温国投
122528:12琼港航
122530:PR七城投
122531:PR太科园
122532:PR宜财投
122533:PR平城投
122534:PR秦开发
122535:苏飞暂停
122536:PR慈国控
122537:PR克城投
122538:PR榕城乡
122539:PR阜城投
122540:PR宁浦口
122541:12宁上陵
122542:PR阿信诚
122543:12钦开投
122544:PR渝长开
122545:PR蒙高新
122546:PR宁高新
122547:PR曲靖投
122549:PR邳润城
122550:12苏国信
122551:PR如东投
122552:12新新业
122553:PR虞交通
122554:PR定海债
122555:PR常经投
122556:PR咸宁投
122557:PR株高科
122558:12昆交01
122559:12昆交02
122560:PR淄城运
122561:PR饶城投
122562:PR伊春债
122563:PR亳州债
122564:PR椒江债
122565:PR邵城投
122566:PR库城建
122567:PR小清河
122568:PR随州债
122569:PR津生态
122570:12滇水投
122571:PR兴国资
122572:12蓉投控
122573:PR牡国投
122574:PR淮开控
122575:PR肥城债
122576:PR内江债
122577:PR苏相城
122578:12长宁债
122580:PR临安债
122581:PR津南城
122582:PR湘九华
122583:PR遵投债
122584:PR松城开
122585:PR新海连
122586:PR中交通
122587:PR遵义债
122588:PR益城投
122589:PR毕信泰
122590:PR鹤城债
122591:12常交债
122592:PR乌国资
122593:PR衡城投
122594:12泉州01
122595:12泉州02
122596:12沪城开
122597:12宝钛债
122598:PR荆门债
122599:PR梵投债
122600:PR鑫泰债
122601:PR白山债
122602:PR松城投
122603:PR穗经开
122604:12吉铁投
122605:PR宁海债
122606:12顺鑫债
122607:PR渝地产
122608:PR西永债
122609:PR扬城控
122610:PR乐清债
122611:PR蓉经01
122612:PR蓉经02
122613:PR乌海债
122614:PR渝缙债
122615:PR百色债
122616:12黔铁债
122617:PR襄投债
122618:统众暂停
122619:PR迁安债
122620:PR乌城投
122621:PR赣城债
122622:PR锦城债
122623:PR旅建债
122624:PR滨开债
122625:12升华债
122626:PR海恒债
122627:PR京建工
122628:PR东投债
122629:PR平发债
122630:PR惠投债
122631:12晋国电
122632:PR江阴债
122633:PR嘉经债
122634:PR芜开01
122635:PR芜开02
122636:PR连发债
122637:PR鑫城债
122638:PR申华信
122639:PR绍新城
122640:PR仪征债
122641:PR武城投
122642:PR朝阳债
122643:12海资债
122644:PR铁岭债
122645:PR苏园建
122648:PR宣国投
122649:PR长建投
122650:泰能暂停
122651:PR广安投
122652:12杨农债
122654:昆钢暂停
122655:PR铜建投
122658:PR盘锦债
122659:12石油06
122660:12石油07
122661:PR怀化债
122662:PR合桃花
122663:PR科发债
122664:12葫芦岛
122665:PR镇交投
122666:12国网01
122667:12国网02
122668:12凉国投
122669:PR桂林债
122670:PR新盛债
122671:12扬子江
122672:PR西城投
122673:12渝李渡
122674:12渝黔江
122675:PR杭城投
122676:PR滨江债
122677:PR江宁债
122678:12扬化工
122679:PR河套债
122680:PR昆建债
122681:PR合农投
122682:PR营口债
122683:12春和债
122684:12合高新
122685:PR吉城投
122686:12白药债
122687:PR金坛债
122688:PR华通债
122689:PR宿开发
122690:12三胞债
122691:PR武清债
122692:12漳路桥
122693:PR佳城投
122694:PR兴荣债
122695:PR五国投
122696:PR丹投债
122697:11太资债
122698:12双流01
122699:12双流02
122700:12来宾债
122701:PR余城建
122702:PR海安债
122703:PR鞍城投
122704:PR江都债
122705:12苏交通
122706:PR海门债
122707:PR泰兴债
122708:PR伊旗债
122709:12绵阳债
122710:PR济城建
122711:12郑新债
122712:12中航债
122713:12冀交通
122714:PR海陵债
122715:PR蓉新城
122716:PR莆田债
122717:12泉矿债
122718:12渝南债
122719:12龙交投
122720:PR甬城投
122721:PR辽国资
122722:PR淮水利
122723:12石油05
122724:12攀国投
122725:12宿产发
122726:PR柳东债
122727:PR东胜债
122728:PR徐经开
122729:12江泉债
122730:12晋江债
122731:PR镇经开
122732:PR九江债
122733:11京资01
122734:11京资02
122735:11六安债
122736:12石油03
122737:12石油04
122740:12延城投
122741:11双鸭山
122742:12鲁高速
122743:PR华发集
122744:11本溪债
122745:12方大01
122746:12方大02
122747:晋煤暂停
122748:12石油01
122749:12石油02
122750:PR常经营
122751:11冀新债
122752:11大丰港
122753:PR姜国资
122754:11通化债
122755:PR潭城建
122756:12甘农垦
122757:11丹东债
122758:11张保债
122759:11泰豪债
122760:PR渝富债
122761:PR萧国资
122762:11吴江债
122763:11淮产投
122764:11泛海01
122765:11泛海02
122766:11宜建投
122767:11盐城南
122768:PR兰城投
122769:PR龙海债
122770:11国网01
122771:11国网02
122772:山煤暂停
122773:11滨海01
122774:11滨海02
122775:PR咸城投
122776:11新光债
122777:11吴中债
122778:11建发债
122779:11株城发
122780:PR长高新
122781:11永州债
122782:11宁农债
122783:11苏中能
122784:11中兴新
122786:11联想债
122787:11赣铁债
122788:PR三明债
122789:11象屿债
122790:PR诸暨债
122792:11邯郸债
122794:11海城债
122795:PR诸城债
122796:11冀投01
122797:11冀投02
122798:11泰矿债
122799:11武国资
122800:龙煤暂停
122801:11焦作债
122802:PR辽阳债
122803:11滁州债
122804:11渭南01
122805:PR大同债
122806:11渭南02
122807:11东岭债
122808:PR滕州债
122809:PR准国资
122810:PR邹平债
122811:11蒙奈伦
122812:11淮北债
122813:11宁交通
122814:PR东营债
122815:11广汇债
122816:11高密债
122817:11三门峡
122818:11牟平债
122819:11常城建
122820:11潍东方
122821:11吉城建
122822:PR汉中债
122823:11舟山债
122824:11中煤建
122825:PR景德镇
122826:11北港债
122827:11新奥债
122828:11抚州债
122829:万基暂停
122830:11沈国资
122831:PR惠通债
122832:PR泰山债
122833:11赣城债
122834:11牡国投
122835:11兴泸债
122836:PR盘锦投
122837:11武经发
122838:11吉利债
122839:11鑫泰债
122840:11临汾债
122841:PR渝津债
122842:PR合城债
122843:11绥化债
122844:11筑城投
122845:11横店债
122846:11渝富债
122847:甬交暂停
122848:10桂林债
122849:11新余债
122850:11华泰债
122851:10玉溪01
122852:10玉溪02
122854:11中汇债
122855:11渝轻纺
122856:11株高科
122857:PR九华债
122858:10盐城01
122859:10盐城02
122860:10龙源债
122861:陕煤暂停
122862:10闽能源
122863:10榆城投
122864:11外滩债
122865:10苏海发
122866:10杭交投
122867:11石城投
122868:沈煤暂停
122869:10沪化工
122870:PR渝大晟
122871:10镇交投
122872:10复星债
122873:10通经开
122874:10红投01
122875:10红投02
122876:11海控债
122877:PR渝南岸
122879:10天脊债
122880:天业暂停
122881:10吴江债
122882:10宁高新
122883:PR楚雄债
122884:10西子债
122885:10冀交通
122886:PR云投债
122887:10渝交通
122888:PR华靖债
122889:10冶色债
122890:10凯迪债
122891:PR通辽债
122892:10寿光债
122893:PR丹东债
122894:10洪市政
122895:10德州债
122896:10芜开债
122897:10襄投债
122898:10攀国投
122899:10杨浦01
122900:10杨浦02
122901:PR营城投
122902:PR赤峰债
122903:PR盐东方
122904:10长城投
122905:10南昌债
122906:10芜投01
122907:10芜投02
122910:PR漯河债
122911:10鞍城投
122912:10鄂国资
122914:09榕建债
122915:PR镇水投
122916:10红谷滩
122917:10太仓港
122919:10鲁商债
122920:10黄山债
122921:10郴州债
122922:10长高新
122923:10北汽投
122924:10巢湖债
122925:09沈国资
122927:09海航债
122928:09铁岭债
122929:09九江债
122930:PR盘锦债
122931:PR临海债
122932:09宜城债
122934:09南山2
122935:PR南通债
122936:PR鹤城投
122937:PR辽源债
122938:09汾湖债
122939:09吉安债
122940:09咸城投
122941:10镇城投
122942:09江阴债
122944:09株城投
122945:09虞水债
122946:PR扬城建
122956:09常高新
122961:09武城投
122965:09潍投债
122969:09豫投债
122975:09济城建
122995:08合建投
122999:08广纸债
124000:PR奉投资
124001:PR漯城投
124002:蒙高暂停
124003:12珠水务
124004:PR盐城南
124005:PR昆创债
124006:PR绍城投
124007:12西电梯
124008:国奥暂停
124009:PR渝惠农
124010:PR鸡国资
124011:12锡科技
124012:PR高密01
124013:PR高密02
124014:PR筑住投
124015:12筑工投
124016:PR常德源
124017:PR伊国资
124018:PR昌经投
124019:PR湘昭投
124020:PR辽城经
124021:PR潍东兴
124022:PR韶金叶
124023:PR滁城投
124024:12青投资
124025:PR池州债
124026:PR川广元
124027:PR瑞国投
124028:PR诸城投
124029:PR玉城投
124030:PR宁城投
124031:12豫铁投
124032:PR宜建投
124033:PR苏城投
124034:PR郑城投
124035:PR沭金源
124036:PR张经开
124037:PR渝江北
124038:12远洲控
124039:PR渝江津
124040:PR绍迪荡
124041:PR宿水务
124042:12鄂旅投
124043:12深立业
124044:12联想债
124045:PR嘉经开
124046:PR濮建投
124047:PR黔宏升
124048:PR平国资
124049:PR营沿海
124050:PR榆城投
124051:PR庆高新
124052:PR昆产投
124053:12营口港
124054:PR株云龙
124055:PR蓉高投
124056:PR启国投
124057:PR汕城开
124058:PR萍乡债
124059:PR临城发
124060:PR驻投资
124061:PR沛国资
124062:PR冀顺德
124063:12国网03
124064:12国网04
124065:PR津开01
124066:PR津开02
124070:PR新城投
124071:PR鹰投融
124072:12曲公路
124073:PR吉华债
124074:PR张公经
124075:PR淮建投
124076:12金湖债
124077:12榕建工
124078:12云城建
124079:PR保国资
124080:PR苏海投
124081:PR长先导
124082:PR青国信
124083:PR黄城投
124084:12沪临港
124085:PR沪金投
124086:PR诸建投
124087:PR芜新马
124088:PR东台债
124089:PR赣开债
124090:PR遵国投
124091:PR渝兴债
124092:12鄂华研
124093:PR喀城投
124094:甬交暂停
124095:PR六开投
124096:PR淮城资
124097:PR宝投资
124098:PR达投资
124099:PR德建投
124100:PR石国投
124101:12赣高速
124102:12滇祥航
124103:PR同创债
124104:PR巢城投
124105:12愉悦债
124106:PR邯郸债
124107:PR洛城投
124108:PR吴经开
124110:PR宁新开
124111:PR长城建
124112:13豫盛润
124113:12渝出版
124114:PR大丰债
124116:PR渝北飞
124117:PR宜城投
124118:12香兴中
124119:PR环太湖
124120:PR泉台商
124121:12盘江债
124122:PR抚城投
124123:PR南城投
124124:PR双鸭山
124125:PR温经开
124126:PR柳城投
124127:PR黄国资
124128:PR萧经开
124129:PR泉石建
124130:13陕东岭
124131:PR安国资
124132:PR巩义债
124133:12宁宝源
124134:PR泉城投
124135:13滇公投
124136:PR太城投
124137:13赣发投
124138:PR长城投
124139:PR通港闸
124140:PR沧建投
124141:13浙吉利
124142:PR渝三峡
124143:PR泰投资
124144:PR蓉城投
124145:PR蓉兴城
124146:13海发控
124147:PR甬东投
124148:PR金灌债
124149:PR镇水利
124150:PR南发展
124151:PR长兴岛
124152:13宁禄口
124153:13国网01
124154:13国网02
124155:PR渭城投
124156:PR涪国资
124158:PR锡东城
124159:PR绍城改
124160:PR蓬莱阁
124161:13瑞水泥
124162:PR济高新
124163:13蓉文旅
124164:PR建城投
124165:PR洪市政
124166:PR江滨投
124167:PR滇投债
124168:PR绍中城
124169:13华峰债
124170:PR厦杏林
124171:PR长投建
124172:PR常城投
124173:陕色暂停
124174:PR吉城债
124175:PR湘高新
124176:PR武地铁
124177:PR乌高新
124178:PR集城投
124179:13广越秀
124180:PR綦东开
124181:PR余开投
124182:13精控债
124183:PR津广成
124184:13京投债
124185:PR海宁债
124187:PR泰矿债
124188:PR邹城资
124189:13大旅游
124190:PR奉南城
124191:PR杭高新
124192:PR邗城建
124193:PR文城资
124194:PR滨海01
124195:PR滨海02
124196:PR烟城建
124198:10朝资02
124199:PR泰交债
124200:PR自高新
124201:PR南高速
124202:PR平潭债
124203:PR浔富和
124204:PR津城投
124205:PR余创债
124206:13祥源债
124207:PR巴城投
124208:13西投债
124209:PR三门峡
124210:PR皋投债
124211:13甘投债
124212:PR益高新
124213:PR德清债
124214:PR河城投
124215:PR九国资
124216:新查暂停
124217:PR西高新
124218:13三福船
124219:PR荣经开
124220:13晋能交
124221:PR武地产
124222:13京粮食
124223:13微山矿
124224:PR朝国资
124225:PR阿城投
124226:PR闽兴杭
124227:PR宁国01
124228:PR宁国02
124229:PR晋公投
124230:PR蓉兴锦
124231:PR临海投
124232:PR苏海发
124234:PR鹏铁01
124235:PR清河投
124236:PR马经开
124238:PR通辽投
124239:PR鄞城投
124240:PR合工投
124241:PR烟开发
124242:PR营经开
124243:PR常高新
124244:PR番交投
124245:PR杭运河
124246:PR溧城发
124247:13绍交投
124248:13京歌华
124249:PR大城投
124250:PR宿建投
124251:13鲁信投
124252:13邯交通
124253:PR新乡投
124254:PR常熟发
124255:PR浙新昌
124256:13苏泊尔
124257:13海浆纸
124258:潞01暂停
124259:潞02暂停
124260:PR遂发展
124261:PR鞍山投
124262:PR楚雄投
124263:PR临国资
124264:PR晋城投
124265:PR红河路
124266:PR哈水投
124267:PR金坛投
124268:PR渝南发
124269:PR渝大足
124270:PR渝万盛
124271:PR金外滩
124272:PR绥芬河
124273:13翔宇债
124275:PR龙岗投
124276:13津滨投
124277:13大丰港
124278:PR渝双桥
124279:PR海拉尔
124280:PR通经开
124281:PR石地产
124283:13武新港
124284:13琼洋浦
124285:同煤暂停
124286:13海航债
124287:金特暂停
124288:13光谷联
124289:PR丽城投
124290:PR长轨交
124291:PR兖城投
124292:PR溧城建
124293:PR农六师
124294:PR苏华靖
124295:13宁铁路
124296:13盛江泉
124297:PR桐乡投
124298:PR临汾投
124299:PR西经开
124300:13云投控
124301:PR日照债
124302:12桂交投
124303:PR咸荣盛
124304:PR合川投
124305:PR瓦国资
124306:13鄂三宁
124307:PR安经开
124308:PR眉宏大
124309:13弘燃气
124310:PR洪水利
124311:PR弘湘资
124312:PR景国资
124313:PR苏家屯
124314:13筑铁路
124315:13瓯交投
124316:PR新郑投
124317:PR华发债
124318:PR滨城投
124319:PR昌国资
124321:PR岳城投
124322:PR南城发
124323:PR新天治
124324:PR白银城
124325:PR京生物
124326:PR郑建投
124327:13中电投
124328:PR渝鸿业
124329:PR惠国投
124330:13龙工贸
124332:13湘振湘
124333:13铜城建
124334:13博国资
124335:13海国资
124336:13渝地产
124337:13铜建投
124338:13闽经开
124339:PR渝城投
124340:13张保债
124341:13吐番资
124342:13黔南资
124343:13阳江债
124344:13沪南房
124345:13京煤债
124346:13乳国资
124347:13石建投
124348:13京谷财
124349:13福东海
124350:晋煤暂停
124351:13克州债
124352:13平凉债
124353:13商洛01
124354:13商洛02
124355:13洼城投
124356:13珠汇华
124357:津房暂停
124358:13蚌城投
124359:PR三明投
124360:13成阿债
124361:13京科城
124362:13钦滨海
124363:13郑投资
124364:13临尧都
124365:13昌润债
124366:13汇丰投
124367:13锡城发
124368:13郑交投
124369:13吴城投
124370:13虞新区
124371:14北辰发
124373:13平天湖
124374:13塔国资
124375:13鄂供销
124376:13渝物流
124377:13渝碚城
124378:13湘九华
124379:13许投资
124380:13曹妃甸
124381:09衡城投
124382:10云建工
124383:10开元旅
124384:13雅发投
124385:13龙岩汇
124386:13新沂债
124387:13湛基投
124388:13任城债
124389:13资水务
124390:13葫岛01
124391:13葫岛02
124392:13荆门投
124393:13连顺兴
124394:13永城投
124395:13堰城投
124396:13姜发展
124397:13郫国投
124398:13株城发
124399:13郴高科
124400:13渝双福
124401:13冀广网
124402:13丹投01
124403:13丹投02
124404:13怀化工
124405:13宝工债
124406:13荆经开
124407:13泰州债
124408:13宛城投
124409:13宿城投
124410:13国网03
124411:13国网04
124412:13金利源
124413:13寿城投
124415:13鄂投01
124416:13鄂投02
124417:13江高新
124418:13永利债
124419:13乌兰察
124420:13盐国资
124421:13海新区
124422:13崇明债
124423:13宜环科
124424:13柳东城
124425:13平国资
124426:13澄港城
124427:13临河债
124428:13粤垦债
124429:13亭公投
124430:14城阳债
124431:13普兰01
124432:13襄建投
124433:PR沪闵行
124434:13渝豪江
124435:13邯城投
124436:13海旅业
124437:13津静海
124438:13冶城投
124439:13六安01
124440:13宁德投
124441:13库车01
124442:13武威01
124443:13黔投01
124444:13六安02
124445:13泰成兴
124446:13即墨债
124448:13大理01
124449:13常滨湖
124450:13周口01
124451:13濮建投
124452:13府谷债
124453:13秦开01
124454:13武清01
124455:13越都债
124456:13闽投债
124457:13河池投
124458:13镇投01
124459:13随州01
124460:13忻州01
124461:13清远债
124462:13海财01
124463:13津住宅
124464:13天易01
124465:13黄冈01
124466:13邕城投
124467:13锦州01
124468:13丰城01
124469:13格尔木
124470:13赣开01
124471:13宁海01
124472:13海西州
124474:14怀化01
124475:13沈湖01
124477:14滨高新
124478:14仪征债
124479:14丰城01
124480:14东台01
124481:13镇投02
124482:14京华远
124483:09渝地产
124485:14苏沿海
124486:14锦州01
124487:14邵城投
124488:14吴兴南
124489:14融强01
124490:14首开01
124491:14皋开债
124492:14江夏投
124493:14伊宁债
124494:14迁安01
124495:14晟晏债
124496:13丰城02
124497:14扬化工
124498:14金资01
124499:13沈湖02
124500:13鹏铁02
124501:14皋沿江
124502:14宏财01
124504:PR鹰投债
124505:14嘉市镇
124507:14潍滨城
124508:14滕州02
124509:14湘潭新
124510:13赣开02
124511:14赣开投
124512:11三门投
124513:14铜旅游
124514:14六开投
124515:14云路桥
124516:14泉高新
124517:14怀化02
124518:13忻州02
124519:14淮新01
124520:14太资债
124521:14泉港债
124522:14连普湾
124523:14开城投
124524:13黔投02
124525:14毕开源
124526:14邹平债
124527:14榆神债
124528:14粤云浮
124529:14泉台商
124530:14海开01
124531:14海开02
124532:14甘公01
124533:14酒经投
124534:14渝中债
124535:14眉山资
124536:14莱开投
124537:14伊财通
124538:14麓城投
124539:14富阳01
124540:14汉车都
124541:14文城投
124542:14陆嘴01
124543:14临港控
124544:14锦州02
124545:14双水01
124546:14丰城02
124547:14海安债
124548:14裕峰债
124549:14新滨江
124550:14桃城投
124551:14长兴经
124552:14如金鑫
124553:14佳城投
124554:14威楠科
124555:14余城建
124556:14余经开
124557:13天易02
124558:14宏桥01
124559:14冶城投
124560:13大理02
124561:14汾湖债
124562:14富蕴资
124563:14吉铁投
124564:14兴安盟
124565:14庆城投
124566:14潭两型
124567:14扬开发
124568:14株国投
124569:14嘉经投
124570:14首开02
124571:14高新投
124572:14遂川中
124573:14龙岩城
124574:14攀国投
124575:14汕投资
124576:11滇铁投
124577:14甬广聚
124578:14青莱西
124580:14淮开控
124581:13黄冈02
124582:14廊经开
124583:14津房信
124584:14天能01
124585:14南网债
124586:14陆嘴02
124587:14阜阳01
124588:14济宁债
124589:14海晋交
124590:13武清02
124591:14长土开
124592:14并国投
124593:14相城投
124594:14潍东方
124595:14涪陵债
124596:14长影债
124597:14海资01
124598:14济城投
124599:14西保01
124600:14贵水01
124601:14唐城债
124602:14国网01
124603:14国网02
124604:14富阳02
124605:14温高01
124606:14菏泽债
124607:14津环城
124608:14句容福
124609:14常德投
124610:14云铁投
124611:14阜阳02
124612:14永城建
124613:14长建投
124614:14桂农垦
124615:14昆高新
124616:14鄂交01
124617:14鄂交02
124618:14粤科债
124619:14中卫建
124620:14渝黔江
124621:14宣国资
124622:14钦临海
124623:14穗铁01
124624:14盛经01
124625:11宁宝源
124626:13渝豪02
124627:PR青州债
124628:14启东01
124629:14苏金灌
124630:14库城建
124631:14漕开发
124632:14合桃花
124633:14淄高新
124634:14牡国投
124635:14滕州01
124636:14防城港
124637:14融强02
124638:14信阳债
124639:14沭金源
124641:14江宁开
124642:14鸠建投
124643:14冀高开
124644:14沈国资
124645:14临经开
124646:14宁经开
124647:14娄底债
124648:14郴州债
124649:14邳润城
124650:13海财02
124651:14杨农发
124652:14益交投
124653:14遂河投
124654:14庆经投
124655:13宁海02
124656:14永国投
124657:14张经投
124658:14桂城建
124659:14平经开
124660:14桐庐投
124661:14赣四通
124662:14京投债
124663:14威经开
124664:13秦开02
124665:14余交通
124666:14莱山债
124667:14苏元禾
124668:14滇公路
124669:14西保02
124670:14蚌高新
124671:14湛新域
124672:14徐开发
124673:14火炬债
124674:14泰中兴
124675:14崇川债
124676:14衢国资
124677:14乌城投
124678:14宁开控
124679:14宜经开
124680:13普兰02
124681:14徐高新
124682:14宝高新
124683:13周口02
124684:14新城基
124685:14临淄债
124686:14昌平债
124687:14南化债
124688:14潜城投
124689:14雨城投
124690:14中电建
124691:14宏财02
124692:14嘉公路
124693:14新凯迪
124694:14克投债
124695:14广元控
124696:14东台02
124697:14马城投
124698:14奉化债
124699:14汇通债
124700:14内江投
124701:14临开债
124702:14衡水投
124703:14蓉隆博
124704:13武威02
124705:13库车02
124706:14巴国资
124707:14渝江01
124708:14兖微01
124709:14安吉债
124710:14兴国资
124711:14象山债
124712:14并经开
124713:14黔铁投
124714:14鲁国集
124715:14四平债
124716:14宁国债
124717:14姜鑫源
124718:14乌房债
124719:13鞍新02
124720:14电投01
124721:14青海创
124722:14包滨河
124723:14启东02
124724:14富山居
124725:14曲靖投
124726:14德高新
124727:14渝保税
124728:14左旗债
124729:14兰新控
124730:14长交01
124731:14朝建投
124732:14渝高开
124733:14开发投
124734:13随州02
124735:14合建投
124736:14柳龙投
124737:14虞交通
124738:14安发投
124739:14西塞山
124740:14青经开
124741:14辽鑫诚
124742:14贵水02
124743:14银城投
124744:14萧经开
124745:14武安债
124746:14贺城投
124747:14太仓港
124748:14铜示范
124749:14仁城投
124750:14宜春投
124751:14徐高铁
124752:14文金滩
124753:14海控01
124754:14荥城投
124755:14合工微
124756:14紫微01
124757:14鄂城01
124758:14吉安债
124759:14威新区
124760:14余城投
124761:14深业团
124762:14萍昌盛
124763:14昆交发
124764:14蔡家湖
124765:14醴陵01
124766:14景洪投
124767:14郑二七
124768:14云城投
124769:14合力01
124770:14合力02
124771:14亳建投
124772:14当阳债
124773:14温高02
124774:14通辽债
124775:14新余东
124776:14绿地债
124777:14茂交投
124778:14蔡甸投
124779:14银开发
124781:14渝江02
124782:14遵国投
124783:14绍袍江
124785:14青州债
124786:14苏海投
124787:14荣经开
124788:14宣北山
124789:14海东投
124790:14陶都债
124791:14孝城投
124792:14桓台债
124793:14合新01
124794:14合新02
124795:14渝惠通
124796:14襄高投
124797:14十二师
124799:14京鑫融
124800:14金城债
124801:14恩城投
124802:14保山债
124803:14津宁投
124804:14津南债
124805:14穗铁02
124806:14渝园业
124807:14金国发
124808:14唐丰南
124809:14龙国投
124810:14一师鑫
124811:14滇投债
124812:14长交02
124813:14井开债
124814:14郑投控
124815:14天瑞02
124816:14顺德投
124817:14北国资
124818:14常德源
124819:14渝旅开
124820:14济高债
124821:14百色投
124822:14合滨投
124823:14池金桥
124824:14金桥棚
124827:14普国资
124828:14日经开
124829:14孝高01
124830:14桂铁投
124831:14崇建设
124832:14睢宁润
124833:14如东泰
124834:14德城投
124835:14渝南债
124836:14大石桥
124837:14赤城投
124839:14滨新塘
124840:14漳九龙
124841:14清微01
124842:14神木债
124843:14宏河债
124844:14遵汇投
124845:14晋开发
124846:14瘦西湖
124847:14济源建
124848:14元国资
124849:14辽沿海
124850:14合川投
124851:14梧东泰
124852:14冀渤海
124853:14淄城运
124854:14喀什深
124855:14淮城投
124856:14常房债
124857:14临桂新
124858:14黄海港
124859:14柳产投
124860:14汤建投
124861:11冀渤海
124862:14台基投
124863:14沂科技
124864:14兴城建
124865:14奎屯润
124866:14南二建
124867:09绍交投
124868:14冀融投
124869:14渝长寿
124870:14嵊投控
124871:14绿国资
124872:14杭拱墅
124873:14盛经02
124874:14哈密01
124875:14哈密02
124876:14郑高新
124877:14莱国资
124878:14苏高新
124879:14淮新02
124880:14曲经开
124881:14安经开
124882:14江北嘴
124883:14西永债
124884:14双水02
124885:14临城建
124886:14长农建
124887:14盐城南
124888:14邹城债
124889:14定国资
124890:14甘电投
124891:14株高01
124892:14株高02
124893:14文登债
124894:14海资02
124895:14筑经开
124896:14北港债
124897:14津广成
124898:14津水务
124899:14穗铁03
124900:14滨开债
124901:14虞城建
124902:14陕交建
124903:14鹤投资
124904:14连旅泰
124905:14登封债
124906:14迁安02
124907:14芜宜居
124908:14靖江港
124909:14超威债
124910:14石景山
124911:14北辰债
124912:13锦州02
124913:14绍交投
124914:14慈建投
124915:14宏桥02
124916:14新开元
124917:14沣西债
124918:14沪南汇
124919:14安城投
124920:14龙海债
124921:14浏阳债
124923:14胶城投
124924:14白沙投
124925:14金湖资
124926:14阜宁债
124927:14玉溪投
124928:14九龙债
124929:14巴南01
124930:14堰城投
124931:09晋交投
124932:14阿克苏
124933:14揭城投
124934:14渝港投
124935:14冀建投
124936:14天门债
124937:14湖中兴
124938:14郴百福
124939:14蒙盛祥
124940:14滁城投
124941:14钦滨海
124942:14南绿港
124943:14兰国投
124944:14广建设
124945:14石国投
124946:14保利集
124947:14西港债
124948:14金资02
124949:14随建投
124950:14登电债
124951:14威中城
124952:14马高新
124953:09宁城建
124956:14锑都债
124957:14滨投债
124958:14浔富和
124959:14仁寿债
124960:14胶发展
124961:14苏望涛
124962:14武经开
124963:14广安经
124964:14自高投
124965:14济西投
124966:14京国资
124967:14昌经投
124968:14盐东方
124969:14醴陵02
124970:14杭地铁
124971:14宜国投
124972:14安高债
124973:14宣建债
124974:14泸纳债
124975:14溧经开
124976:14张掖债
124977:14天瑞03
124978:14新密债
124979:14陂城投
124980:14抚微01
124981:14嘉峪关
124982:14高安01
124983:14孝高02
124984:14鄂城02
124985:14兖微02
124986:14建开债
124987:14昆经开
124988:14闽投债
124989:14三星01
124999:13武续债
126018:08江铜债
127000:14繁昌投
127001:14电投02
127002:14天能02
127003:14德兴债
127004:14溧昆仑
127005:14乐清债
127006:14蓬莱债
127007:15潭万楼
127008:14龙岩01
127009:14龙岩02
127010:14海城投
127011:14鹰投债
127012:14柳微债
127013:14乐山债
127014:14三门01
127015:14世园债
127016:14忠旺债
127017:14粤高债
127018:14新昌01
127019:14丹徒投
127020:14玉交01
127021:14京天恒
127022:10句容福
127023:14集城投
127024:14攀小微
127025:14惠城投
127026:14沪建债
127027:14晋城债
127028:14新供销
127029:14岳阳债
127030:14鹿城债
127031:14鹤城投
127032:14连交通
127033:14即旅投
127034:14永嘉债
127035:14春辉01
127037:14渝双桥
127038:14吴中债
127039:14西电债
127040:14春辉02
127041:14中山交
127042:14来工投
127043:14黑重建
127044:14河润业
127045:14长兴债
127046:14海控02
127047:15江油债
127048:14浏经开
127049:14绍柯开
127050:14松原债
127051:15滕建债
127052:14甘公02
127053:15天瑞01
127054:15黔物资
127055:14邳恒润
127056:16朝国资
127057:14牟中债
127058:14连融达
127059:14芜建债
127060:14遵义投
127061:14黔西南
127062:14博兴债
127065:14阜新01
127066:14高安02
127067:16遵经债
127068:14新昌02
127069:14准国资
127070:14钦开投
127071:14清微02
127072:14泾河债
127073:15铁西债
127074:15铜大江
127075:15鸡西资
127076:15宁城投
127077:15本溪债
127078:15盘山债
127079:15郴高科
127080:15达州01
127081:15牟国资
127082:15望经开
127083:15宜创债
127084:15中区债
127085:14抚微02
127086:10乌城投
127087:15榕城01
127088:15汇丰投
127089:15新郑01
127090:15新郑02
127091:15梵投债
127092:15淀山湖
127093:15铜发债
127094:15盘经开
127095:15湘九华
127096:15东方财
127097:15毕建投
127098:15营沿海
127099:15天瑞02
127100:15新交投
127101:15吉华债
127102:15襄矿债
127103:10顺义02
127104:15涪交旅
127105:15咸荣盛
127106:14黑债01
127107:14紫微02
127108:15常天宁
127109:15天盈债
127110:15兴城建
127111:15黔南投
127112:15天诚01
127113:15乌国投
127114:15乳国资
127115:15马经开
127116:15淳新开
127117:15娄开债
127118:15丰城投
127119:15兴堰债
127120:15巴南债
127121:15苏国信
127122:10湘高速
127123:14阜新02
127124:中色暂停
127125:14三门02
127126:15遂富源
127127:15渭城投
127128:15沈大东
127129:15苏通债
127130:15临尧都
127131:14玉交02
127132:15梅山债
127133:15泗洪债
127134:15广安债
127135:15益高新
127136:15西经微
127137:15株今添
127138:15柯岩债
127139:15邛崃债
127140:15文小微
127141:15东南债
127142:15怀经开
127143:15新泰债
127144:15黄河债
127145:15长轨01
127146:15汴新债
127147:15郫国投
127148:15九江置
127149:15粤路桥
127150:15包科教
127151:15白工投
127152:15渝铜梁
127153:15吐国投
127154:15乌小微
127155:15宜兴债
127156:15联峰债
127157:15石城投
127158:15东营债
127159:15越都债
127160:15耒城投
127161:15石国控
127162:15湘铁投
127163:15海城投
127164:15庐江债
127165:15高国资
127166:15洋口港
127167:15阳江债
127168:15绍城投
127169:15鄂长江
127170:15渝城投
127171:15乌经开
127172:15滨中海
127173:15津铁投
127174:15迁安债
127175:15武铁01
127176:15武铁02
127177:15梅金叶
127178:15华南城
127180:15郴新债
127181:15桂林债
127182:15阿信诚
127183:15淮城资
127184:15漳经发
127185:15绍城改
127186:15遵道桥
127187:15宜城债
127188:15江高新
127189:15渝悦投
127190:15大足债
127191:15济高新
127192:15沪闵行
127193:15马花山
127194:15天诚02
127195:16闽投02
127196:15海海业
127197:15瓯海债
127198:15绍新城
127199:15龙口债
127200:15津环城
127201:15丹开债
127202:15呼伦债
127203:15兴泰债
127204:15七师微
127205:15巢城投
127206:15沈经区
127207:15呼小微
127208:15国网01
127209:15国网02
127210:15双鸭微
127211:15黄山债
127212:15黄城投
127213:16枝江02
127214:15建发债
127215:16兴荣控
127216:15蜀城投
127218:10装备02
127219:15九城投
127220:15邯建投
127221:15赣城债
127222:15建湖债
127223:15大洼债
127224:15西微01
127225:15鹰高新
127226:15海基债
127227:15锡山债
127228:15邗城建
127229:15潍高新
127230:15牡新区
127231:15冀广01
127232:15长轨02
127233:16余金控
127234:15十师债
127235:15椒江债
127236:15吴江债
127237:15喀城投
127238:15陕东岭
127239:15东港债
127240:15洪轨02
127241:15郑经开
127242:15当涂债
127243:15潍渤海
127244:15中关村
127245:15荆高新
127246:15徐新盛
127247:15任城债
127248:15京科城
127249:15丽水债
127250:15闽漳龙
127251:15丰县债
127252:15通途债
127253:15粤电01
127255:15平湖债
127256:15温铁01
127257:15博投债
127258:15温铁02
127259:15太科债
127260:15开小微
127261:15般阳债
127262:15连江债
127263:15沭金源
127264:15彬煤债
127265:15涪小微
127266:15桓台债
127267:15邵武债
127268:15汝州债
127269:15武夷债
127270:15港小微
127271:15乌高微
127272:15高邮债
127273:15黑山债
127274:15铜城投
127275:15一师债
127276:15昌小微
127277:15内小微
127278:15津地铁
127279:15浏新城
127280:15魏桥债
127281:15邳经发
127282:15贵路桥
127283:15大同建
127285:15茂名港
127286:15沛城投
127287:15芜新马
127288:15通高新
127289:15河池债
127290:15伊国资
127291:15苍南债
127292:15国网03
127293:15国网04
127294:15天心01
127295:15泰虹桥
127296:15云能源
127297:15兴小微
127298:15任丘债
127299:15蓬莱债
127300:15国泰债
127301:15武清债
127302:15桂城投
127303:15秦汉债
127304:15蒙金隆
127306:15伊小微
127307:16神木债
127308:15巴国资
127309:15赣陶债
127310:15海城改
127311:15麒麟债
127312:15海航债
127313:15东丽投
127314:15睢润企
127315:15机场债
127316:15洛城投
127317:15平崆旅
127318:15闽投专
127319:15日照债
127320:15萍小微
127321:15湘产债
127322:15义城投
127323:15海陵债
127324:15达州02
127326:15国网05
127327:15国网06
127328:15长轨03
127330:15赣和济
127331:15威海投
127333:15榕城02
127334:15锡创投
127335:15昌乐债
127336:15寿小微
127337:15潜城投
127338:15宜高投
127339:15金昌债
127340:15冀广02
127341:15正棚改
127342:15内双创
127344:15仁寿债
127345:15盐高新
127346:15七小微
127347:15昆水务
127348:15响水债
127349:16邵东债
127350:15浙滨债
127351:15黔畅达
127352:16恒投01
127353:15渝缙云
127354:15梅建投
127355:15黔投01
127356:16常城投
127357:16永经投
127358:16平阳债
127359:16穗金控
127360:15兴义债
127361:15老边01
127362:16闽投01
127363:16新沂债
127364:15潼南债
127365:16渝两江
127366:16红小微
127367:15沪城建
127368:16衡阳债
127369:16来宾债
127370:16奥德01
127371:16普兰店
127372:16大理债
127373:16枝江01
127374:16六盘水
127375:16五家渠
127376:15西微02
127377:16黄冈债
127378:16禹州债
127379:16泗阳债
127380:16阿勒泰
127381:16仪征债
127382:16赣投债
127383:16宁经开
127384:16开福01
127385:16兴资债
127386:16丹投债
127387:16雨城投
127388:16瓯海债
127390:16芙蓉债
127391:16瓦沿海
127392:16平交投
127393:16合川债
127394:16诸经债
127395:16吉城建
127396:16陕旅债
127397:15耒阳债
127398:16威海债
127399:16鲁信债
127400:16广晟01
127401:16铜建专
127402:16下城债
127403:15老边02
127404:16唐金债
127405:16盐都债
127406:16宏小微
127407:16汇盛债
127408:16张经开
127409:16渝地产
127410:16德兴债
127411:16滁小微
127412:16鸠江债
127413:16皋投债
127414:16邕高01
127415:16三明交
127416:16贾汪债
127417:16榕高新
127418:16启交通
127419:16启国投
127420:16渝开债
127421:16青小微
127422:16牡小微
127424:16惠开债
127425:16穗港01
127426:16渝江01
127427:16渤海01
127428:16渤海02
127429:G16京汽1
127430:16淮城资
127431:16洛新债
127434:16晋煤01
127436:16惠棚改
127479:16瀚瑞01
130063:11地债04
130065:11地债06
130067:11地债08
130069:11上海02
130071:11广东02
130073:11浙江02
130075:11深圳02
130077:12地债02
130079:12地债04
130081:12地债06
130083:12地债08
130084:12上海01
130085:12上海02
130086:12广东01
130087:12广东02
130089:12地债10
130090:12浙江01
130091:12浙江02
130092:12深圳01
130093:12深圳02
130095:13地债02
130097:13地债04
130098:13地债05
130099:13地债06
130100:13地债07
130101:13地债08
130102:13山东01
130103:13山东02
130104:13地债09
130105:13上海01
130106:13上海02
130107:13地债10
130108:13广东01
130109:13广东02
130110:13江苏01
130111:13江苏02
130112:13地债11
130113:13地债12
130114:13浙江01
130115:13浙江02
130116:13深圳01
130117:13深圳02
130118:14地债01
130119:14地债02
130120:14广东01
130121:14广东02
130122:14广东03
130123:14地债03
130124:14地债04
130125:14地债05
130126:14山东01
130127:14山东02
130128:14山东03
130129:14地债06
130130:14地债07
130131:14地债08
130132:14江苏01
130133:14江苏02
130134:14江苏03
130135:14江西01
130136:14江西02
130137:14江西03
130138:14宁夏01
130139:14宁夏02
130140:14宁夏03
130141:14地债09
130142:14地债10
130143:14青岛01
130144:14青岛02
130145:14青岛03
130146:14浙江01
130147:14浙江02
130148:14浙江03
130149:14北京01
130150:14北京02
130151:14北京03
130152:14上海01
130153:14上海02
130154:14上海03
130155:14地债11
130156:14地债12
130157:14地债13
130158:14深圳01
130159:14深圳02
130160:14深圳03
130161:15江苏01
130162:15江苏02
130163:15江苏03
130164:15江苏04
130165:15新疆01
130166:15新疆02
130167:15新疆03
130168:15新疆04
130169:15湖北01
130170:15湖北02
130171:15湖北03
130172:15湖北04
130173:15广西01
130174:15广西02
130175:15广西03
130176:15广西04
130177:15山东01
130178:15山东02
130179:15山东03
130180:15山东04
130181:15重庆01
130182:15重庆02
130183:15重庆03
130184:15重庆04
130185:15贵州01
130186:15贵州02
130187:15贵州03
130188:15贵州04
130189:15安徽01
130190:15安徽02
130191:15安徽03
130192:15安徽04
130193:15天津01
130194:15天津02
130195:15天津03
130196:15天津04
130197:15湖北05
130198:15湖北06
130199:15湖北07
130200:15湖北08
130201:15浙江01
130202:15浙江02
130203:15浙江03
130204:15浙江04
130205:15河北01
130206:15河北02
130207:15河北03
130208:15河北04
130209:15吉林01
130210:15吉林02
130211:15吉林03
130212:15吉林04
130213:15山西01
130214:15山西02
130215:15山西03
130216:15山西04
130217:15河北Z1
130218:15河北Z2
130219:15河北Z3
130220:15广东01
130221:15广东02
130222:15广东03
130223:15广东04
130224:15江西01
130225:15江西02
130226:15江西03
130227:15江西04
130228:15宁夏01
130229:15宁夏02
130230:15宁夏03
130231:15宁夏04
130232:15新疆05
130233:15新疆06
130234:15新疆07
130235:15新疆08
130236:15四川01
130237:15四川02
130238:15四川03
130239:15四川04
130240:15河南01
130241:15河南02
130242:15河南03
130243:15河南04
130244:15辽宁01
130245:15辽宁02
130246:15辽宁03
130247:15辽宁04
130248:15云南01
130249:15云南02
130250:15云南03
130251:15云南04
130252:15青岛01
130253:15青岛02
130254:15青岛03
130255:15青岛04
130256:15海南01
130257:15海南02
130258:15海南03
130259:15海南04
130260:15江苏Z1
130261:15江苏Z2
130262:15江苏Z3
130263:15陕西01
130264:15陕西02
130265:15陕西03
130266:15陕西04
130267:15山东05
130268:15山东06
130269:15山东07
130270:15山东08
130271:15大连01
130272:15大连02
130273:15大连03
130274:15大连04
130275:15大连Z1
130276:15大连Z2
130277:15大连Z3
130278:15大连Z4
130279:15贵州05
130280:15贵州06
130281:15贵州07
130282:15贵州08
130283:15内蒙01
130284:15内蒙02
130285:15内蒙03
130286:15内蒙04
130287:15新疆Z1
130288:15新疆Z2
130289:15新疆Z3
130290:15新疆Z4
130291:15北京01
130292:15北京02
130293:15北京03
130294:15北京04
130295:15四川05
130296:15四川06
130297:15四川07
130298:15四川08
130299:15甘肃01
130300:15甘肃02
130301:15甘肃03
130302:15甘肃04
130303:15青海01
130304:15青海02
130305:15青海03
130306:15青海04
130307:15宁波01
130308:15宁波02
130309:15宁波03
130310:15宁波04
130311:15宁波Z1
130312:15宁波Z2
130313:15宁波Z3
130314:15宁波Z4
130315:15广东Z1
130316:15广东Z2
130317:15广东Z3
130318:15福建01
130319:15福建02
130320:15福建03
130321:15福建04
130322:15湖南01
130323:15湖南02
130324:15湖南03
130325:15湖南04
130326:15湖北09
130327:15湖北10
130328:15湖北11
130329:15湖北12
130330:15湖北Z1
130331:15湖北Z2
130332:15湖北Z3
130333:15湖北Z4
130334:15广西05
130335:15广西06
130336:15广西07
130337:15广西08
130338:15广西Z1
130339:15广西Z2
130340:15广东05
130341:15广东06
130342:15广东07
130343:15广东08
130344:15山东Z1
130345:15山东Z2
130346:15山东Z3
130347:15福建Z1
130348:15福建Z2
130349:15福建05
130350:15福建06
130351:15福建07
130352:15福建08
130353:15黑龙01
130354:15黑龙02
130355:15黑龙03
130356:15黑龙04
130357:15黑龙Z1
130358:15黑龙Z2
130359:15黑龙Z3
130360:15云南Z1
130361:15云南Z2
130362:15云南Z3
130363:15云南Z4
130364:15重庆05
130365:15重庆06
130366:15重庆07
130367:15重庆08
130368:15重庆Z1
130369:15重庆Z2
130370:15新疆09
130371:15新疆10
130372:15新疆11
130373:15新疆12
130374:15新疆Z5
130375:15新疆Z6
130376:15新疆Z7
130377:15新疆Z8
130378:15上海01
130379:15上海02
130380:15上海03
130381:15上海04
130382:15上海Z1
130383:15上海Z2
130384:15辽宁05
130385:15辽宁06
130386:15辽宁07
130387:15辽宁08
130388:15辽宁Z1
130389:15辽宁Z2
130390:15青岛05
130391:15青岛06
130392:15青岛07
130393:15青岛08
130394:15青岛Z1
130395:15青岛Z2
130396:15青岛Z3
130397:15天津05
130398:15天津06
130399:15天津07
130400:15天津08
130401:15天津Z1
130402:15天津Z2
130403:15天津Z3
130404:15甘肃05
130405:15甘肃06
130406:15甘肃07
130407:15甘肃08
130408:15甘肃Z1
130409:15甘肃Z2
130410:15安徽05
130411:15安徽06
130412:15安徽07
130413:15安徽08
130414:15安徽09
130415:15安徽Z1
130416:15安徽Z2
130417:15厦门01
130418:15厦门02
130419:15厦门03
130420:15厦门04
130421:15厦门Z1
130422:15厦门Z2
130423:15青海05
130424:15青海06
130425:15青海07
130426:15青海08
130427:15青海Z1
130428:15青海Z2
130429:15青海Z3
130430:15青海Z4
130431:15北京Z1
130432:15北京Z2
130433:15北京Z3
130434:15北京Z4
130435:15陕西05
130436:15陕西06
130437:15陕西07
130438:15陕西08
130439:15陕西Z1
130440:15陕西Z2
130441:15陕西Z3
130442:15陕西Z4
130443:15陕西Z5
130444:15陕西Z6
130445:15陕西Z7
130446:15陕西Z8
130447:15河南05
130448:15河南06
130449:15河南07
130450:15河南08
130451:15河南Z1
130452:15河南Z2
130453:15河南Z3
130454:15河南Z4
130455:15内蒙05
130456:15内蒙06
130457:15内蒙07
130458:15内蒙08
130459:15内蒙Z1
130460:15内蒙Z2
130461:15内蒙Z3
130462:15内蒙Z4
130463:15宁夏05
130464:15宁夏06
130465:15宁夏07
130466:15宁夏08
130467:15江苏05
130468:15江苏06
130469:15江苏07
130470:15江苏08
130471:15江苏Z4
130472:15江苏Z5
130473:15江苏Z6
130474:15江苏Z7
130475:15山东09
130476:15山东10
130477:15山东11
130478:15山东12
130479:15山东Z4
130480:15山东Z5
130481:15山东Z6
130482:15山东Z7
130483:15新疆13
130484:15新疆14
130485:15新疆15
130486:15新疆16
130487:15新疆Z9
130488:15新疆17
130489:15新疆18
130490:15新疆19
130491:15广西09
130492:15广西10
130493:15广西11
130494:15广西12
130495:15广西Z3
130496:15广西Z4
130497:15浙江05
130498:15浙江06
130499:15浙江07
130500:15浙江08
130501:15浙江Z1
130502:15浙江Z2
130503:15浙江Z3
130504:15浙江Z4
130505:15河北05
130506:15河北06
130507:15河北07
130508:15河北08
130509:15河北Z4
130510:15河北Z5
130511:15贵州09
130512:15贵州10
130513:15贵州11
130514:15贵州12
130515:15云南05
130516:15云南06
130517:15云南07
130518:15云南08
130519:15云南Z5
130520:15云南Z6
130521:15云南Z7
130522:15云南Z8
130523:15福建09
130524:15福建10
130525:15福建11
130526:15福建12
130527:15福建Z3
130528:15福建Z4
130529:15青海09
130530:15青海10
130531:15青海11
130532:15青海12
130533:15湖北13
130534:15湖北14
130535:15湖北15
130536:15湖北16
130537:15湖北Z5
130538:15湖北Z6
130539:15湖北Z7
130540:15湖北Z8
130541:15四川09
130542:15四川10
130543:15四川11
130544:15四川12
130545:15广东09
130546:15广东10
130547:15广东11
130548:15广东12
130549:15广东Z4
130550:15广东Z5
130551:15广东Z6
130552:15海南05
130553:15海南06
130554:15海南07
130555:15海南08
130556:15海南Z1
130557:15海南Z2
130558:15海南Z3
130559:15浙江09
130560:15浙江10
130561:15浙江11
130562:15浙江12
130563:15浙江Z5
130564:15浙江Z6
130565:15浙江Z7
130566:15浙江Z8
130567:15甘肃09
130568:15甘肃10
130569:15甘肃11
130570:15甘肃12
130571:15甘肃Z3
130572:15甘肃Z4
130573:15江西05
130574:15江西06
130575:15江西07
130576:15江西08
130577:15江西Z1
130578:15江西Z2
130579:15江西Z3
130580:15江西Z4
130581:15江西Z5
130582:15江西Z6
130583:15江西Z7
130584:15江西Z8
130585:15上海05
130586:15上海06
130587:15上海07
130588:15上海Z3
130589:15上海Z4
130590:15上海Z5
130591:15上海Z6
130592:15四川Z1
130593:15四川Z2
130594:15四川Z3
130595:15四川Z4
130596:15福建13
130597:15福建14
130598:15福建15
130599:15福建16
130600:15福建Z5
130601:15福建Z6
130602:15福建Z7
130603:15福建Z8
130604:15安徽10
130605:15安徽11
130606:15安徽12
130607:15安徽13
130608:15安徽Z3
130609:15安徽Z4
130610:15宁夏09
130611:15宁夏10
130612:15宁夏11
130613:15宁夏12
130614:15宁夏Z1
130615:15宁夏Z2
130616:15宁夏Z3
130617:15宁夏Z4
130618:15宁夏Z5
130619:15宁夏Z6
130620:15天津09
130621:15天津10
130622:15天津11
130623:15天津12
130624:15天津Z4
130625:15天津Z5
130626:15天津Z6
130627:15广东13
130628:15广东14
130629:15广东15
130630:15广东16
130631:15山西05
130632:15山西06
130633:15山西07
130634:15山西08
130635:15山西Z1
130636:15山西Z2
130637:15河南09
130638:15河南10
130639:15河南11
130640:15河南12
130641:15河南Z5
130642:15河南Z6
130643:15河南Z7
130644:15河南Z8
130645:15贵州Z1
130646:15贵州Z2
130647:15贵州Z3
130648:15贵州Z4
130649:15江苏09
130650:15江苏10
130651:15江苏11
130652:15江苏12
130653:15江苏Z8
130654:15江苏Z9
130655:15江苏13
130656:15江苏14
130657:15云南09
130658:15云南10
130659:15云南11
130660:15云南12
130661:15云南Z9
130662:15云南13
130663:15云南14
130664:15云南15
130665:15内蒙09
130666:15内蒙10
130667:15内蒙11
130668:15内蒙12
130669:15内蒙Z5
130670:15内蒙Z6
130671:15内蒙Z7
130672:15内蒙Z8
130673:15宁波05
130674:15宁波06
130675:15宁波07
130676:15宁波08
130677:15宁波Z5
130678:15宁波Z6
130679:15宁波Z7
130680:15宁波Z8
130681:15厦门05
130682:15厦门06
130683:15厦门07
130684:15厦门08
130685:15厦门Z3
130686:15厦门Z4
130687:15陕西09
130688:15陕西10
130689:15陕西11
130690:15陕西12
130691:15陕西Z9
130692:15陕西13
130693:15陕西14
130694:15陕西15
130695:15黑龙05
130696:15黑龙06
130697:15黑龙07
130698:15黑龙08
130699:15黑龙Z4
130700:15黑龙Z5
130701:15大连05
130702:15大连06
130703:15大连07
130704:15大连08
130705:15大连Z5
130706:15大连Z6
130707:15大连Z7
130708:15大连Z8
130709:15吉林05
130710:15吉林06
130711:15吉林07
130712:15吉林08
130713:15吉林Z1
130714:15吉林Z2
130715:15吉林Z3
130716:15吉林Z4
130717:15北京05
130718:15北京06
130719:15北京07
130720:15北京08
130721:15北京Z5
130722:15北京Z6
130723:15北京Z7
130724:15北京Z8
130725:15北京Z9
130726:15湖南05
130727:15湖南06
130728:15湖南07
130729:15湖南08
130730:15上海08
130731:15上海09
130732:15山东13
130733:15山东14
130734:15山东15
130735:15山东16
130736:15贵州13
130737:15贵州14
130738:15贵州15
130739:15贵州16
130740:15贵州Z5
130741:15贵州Z6
130742:15贵州Z7
130743:15贵州Z8
130744:15浙江13
130745:15浙江14
130746:15浙江15
130747:15浙江16
130748:15青岛09
130749:15青岛10
130750:15青岛11
130751:15青岛12
130752:15福建17
130753:15福建18
130754:15福建19
130755:15福建20
130756:15福建Z9
130757:15福建21
130758:15内蒙13
130759:15内蒙14
130760:15内蒙15
130761:15内蒙16
130762:15内蒙Z9
130763:15内蒙17
130764:15辽宁09
130765:15辽宁10
130766:15辽宁11
130767:15辽宁12
130768:15甘肃13
130769:15甘肃14
130770:15甘肃15
130771:15甘肃16
130772:15山西09
130773:15山西10
130774:15山西11
130775:15山西12
130776:15贵州17
130777:15贵州18
130778:15贵州19
130779:15贵州20
130780:16湖北01
130781:16湖北02
130782:16湖北03
130783:16湖北04
130784:16广东01
130785:16广东02
130786:16广东03
130787:16广东04
130788:16广东05
130789:16广东06
130790:16广东07
130791:16浙江01
130792:16浙江02
130793:16浙江03
130794:16浙江04
130795:16山东01
130796:16山东02
130797:16山东03
130798:16山东04
130799:16山东05
130800:16山东06
130801:16山东07
130802:16山东08
130803:16内蒙01
130804:16内蒙02
130805:16内蒙03
130806:16内蒙04
130807:16江苏01
130808:16江苏02
130809:16江苏03
130810:16江苏04
130811:16江苏05
130812:16江苏06
130813:16江苏07
130814:16江苏08
130815:16重庆01
130816:16重庆02
130817:16重庆03
130818:16重庆04
130819:16重庆05
130820:16重庆06
130821:16天津01
130822:16天津02
130823:16天津03
130824:16天津04
130825:16天津05
130826:16云南01
130827:16云南02
130828:16云南03
130829:16云南04
130830:16新疆01
130831:16新疆02
130832:16新疆03
130833:16新疆04
130834:16江西01
130835:16江西02
130836:16江西03
130837:16江西04
130838:16江西05
130839:16江西06
130840:16江西07
130841:16江西08
130842:16宁夏01
130843:16宁夏02
130844:16宁夏03
130845:16宁夏04
130846:16广西01
130847:16广西02
130848:16广西03
130849:16广西04
130850:16广西05
130851:16广西06
130852:16四川01
130853:16四川02
130854:16四川03
130855:16四川04
130856:16辽宁01
130857:16辽宁02
130858:16辽宁03
130859:16辽宁04
130860:16安徽01
130861:16安徽02
130862:16安徽03
130863:16安徽04
130864:16青海01
130865:16青海02
130866:16青海03
130867:16青海04
130868:16广东08
130869:16广东09
130870:16广东10
130871:16广东11
130872:16广东12
130873:16广东13
130874:16广东14
130875:16广西07
130876:16广西08
130877:16广西09
130878:16广西10
130879:16新疆05
130880:16新疆06
130881:16新疆07
130882:16新疆08
130883:16新疆09
130884:16新疆10
130885:16新疆11
130886:16新疆12
130887:16贵州01
130888:16贵州02
130889:16贵州03
130890:16贵州04
130891:16贵州05
130892:16贵州06
130893:16贵州07
130894:16贵州08
130895:16黑龙01
130896:16黑龙02
130897:16黑龙03
130898:16黑龙04
130899:16黑龙05
130900:16黑龙06
130901:16黑龙07
130902:16黑龙08
130903:16湖南01
130904:16湖南02
130905:16河南01
130906:16河南02
130907:16河南03
130908:16河南04
130909:16河北01
130910:16河北02
130911:16河北03
130912:16河北04
130913:16河北05
130914:16河北06
130915:16河北07
130916:16河北08
130917:16湖北05
130918:16湖北06
130919:16湖北07
130920:16湖北08
130921:16湖北09
130922:16湖北10
130923:16甘肃01
130924:16甘肃02
130925:16甘肃03
130926:16甘肃04
130927:16甘肃05
130928:16山西01
130929:16山西02
130930:16山西03
130931:16山西04
130932:16山东09
130933:16山东10
130934:16山东11
130935:16山东12
130936:16陕西01
130937:16陕西02
130938:16陕西03
130939:16陕西04
130940:16陕西05
130941:16陕西06
130942:16陕西07
130943:16陕西08
130944:16湖南03
130945:16湖南04
130946:16海南01
130947:16海南02
130948:16海南03
130949:16宁波01
130950:16宁波02
130951:16宁波03
130952:16宁波04
130953:16宁波05
130954:16宁波06
130955:16宁波07
130956:16宁波08
130957:16青岛01
130958:16青岛02
130959:16青岛03
130960:16青岛04
130961:16青岛05
130962:16青岛06
130963:16青岛07
130964:16四川05
130965:16四川06
130966:16四川07
130967:16四川08
130968:16四川09
130969:16四川10
130970:16四川11
130971:16四川12
130972:16宁夏05
130973:16宁夏06
130974:16宁夏07
130975:16宁夏08
130976:16辽宁05
130977:16辽宁06
130978:16辽宁07
130979:16辽宁08
130980:16云南05
130981:16云南06
130982:16云南07
130983:16云南08
130984:16陕西09
130985:16陕西10
130986:16陕西11
130987:16陕西12
130988:16陕西13
130989:16陕西14
130990:16陕西15
130991:16陕西16
130992:16陕西17
130993:16陕西18
130994:16陕西19
130995:16陕西20
130996:16青海05
130997:16青海06
130998:16青海07
130999:16青海08
132001:14宝钢EB
132002:15天集EB
132003:15清控EB
132004:15国盛EB
132005:15国资EB
133001:14宝钢EZ
133002:15天集EZ
133003:15清质EB
133004:15国盛ZB
133005:15国资ZB
134000:15浙国质
134001:15福能质
134002:15赣质02
134003:15如意质
134005:15海投Z1
134006:15鲁星Z1
134007:15焦质02
134008:15协鑫质
134009:15红星Z1
134010:15中骏Z1
134011:14瀚华Z2
134012:15梅质02
134013:15财达质
134014:15福投质
134015:15华安Z1
134016:15赛轮质
134017:15名城Z1
134019:15龙质04
134020:15华安Z2
134021:15新城Z1
134022:15东吴质
134023:15当代质
134024:15沪城质
134025:15黔路Z1
134026:15蒙阜质
134027:15三福Z2
134028:15花园Z1
134029:15华宝质
134030:15吉利Z1
134031:15常发质
134032:15红美Z1
134033:15东旭Z2
134034:15沪国质
134035:15远东质
134036:15苏元质
134037:15旭质02
134038:15兴杭Z1
134039:15石化Z1
134040:15石化Z2
134041:15渝信Z1
134042:15渝信Z2
134043:15华凌Z1
134044:15通运Z1
134045:15复地Z1
134046:15中海Z1
134047:15国君Z1
134048:15国君Z2
134049:15中海Z2
134050:15景德Z1
134051:15五质03
134052:15五质04
134053:15南航Z1
134055:14贸质02
134056:15玉皇Z1
134057:15华发Z1
134058:15宜集质
134059:15纳通Z1
134060:15纳通Z2
134061:15东证质
134062:15大连质
134063:15中骏Z2
134064:13铁质02
134065:15晋电Z1
134066:15西王Z1
134067:15洪市质
134068:15哈投Z2
134069:15双欣质
134070:15必康质
134071:15开元Z1
134072:15开元Z2
134073:15云能Z2
134074:15合作质
134075:15桂铁质
134076:15瑞贝质
134077:15中天Z1
134078:15禹洲Z1
134079:15中航质
134080:15北汽Z1
134081:15广汇Z1
134082:15浙交Z1
134083:15浙交Z2
134084:15金源Z1
134085:15茂投质
134086:15金源Z2
134087:15保利Z1
134088:15保利Z2
134089:15绿地Z1
134090:15绿地Z2
134091:15华集Z1
134092:15连云质
134093:15华信质
134094:15晋电Z2
134095:15锡交Z1
134096:16复星Z1
134097:15鲁高Z1
134098:15义市Z1
134099:15绍交Z1
134100:16凯乐质
134101:15合景Z1
134102:15合景Z2
134103:15滇路Z1
134104:15市北质
134105:15三友Z1
134106:15三友Z2
134107:15穗工质
134108:14粤运Z2
134109:15康达质
134110:14昊华Z2
134111:15中环Z1
134112:15华集Z2
134113:15新燃Z1
134114:15花园Z2
134115:15广证Z2
134116:15天富质
134117:15苏伟质
134118:15融信Z1
134119:15国创Z1
134120:15鲁能质
134121:15南山Z2
134123:15中合Z1
134124:16新奥质
134125:15洛娃Z1
134126:15鑫苑Z1
134127:15中江Z1
134128:15宇通Z1
134129:15圣牧Z1
134130:16葛洲Z1
134131:15陕投质
134132:15邢钢质
134133:16国电Z1
134134:16番雅质
134135:16联泰Z1
134136:16茂业Z1
134137:16茂业Z2
134138:16常高质
134139:16国美Z1
134140:16富力Z1
134142:16中铁Z1
134143:16万达Z1
134144:16远东质
134145:16金辉Z1
134146:16东兴质
134147:16中粮Z1
134148:16宏桥Z1
134149:16宏桥Z2
134150:16桐昆Z1
134151:16保利Z1
134152:16保利Z2
134153:16珠投Z1
134154:16西王Z1
134155:16电建Z1
134156:16同益质
134157:16重水Z1
134158:16融信Z1
134159:16沪国质
134160:16东旭Z1
134161:16渝交质
134162:16中静Z1
134163:16青国质
134164:16中油Z1
134165:16中油Z2
134166:16广新Z1
134167:16华夏质
134168:16建发Z1
134169:16狮桥质
134170:16景瑞Z1
134171:16华证Z1
134172:16亿阳Z1
134173:16龙源Z1
134174:16工艺Z1
134175:16搜候质
134176:16绿地Z1
134177:16电气质
134178:16兆泰Z1
134179:16绿地Z2
134180:16国汽Z1
134181:16万通Z1
134182:16玉皇Z1
134183:16新华质
134184:16上港Z1
134185:16国发Z1
134186:16苏新质
134187:16景德Z1
134188:16富力Z3
134189:16新业Z1
134190:16正才Z2
134191:16靖江质
134192:16信威Z1
134193:16广越Z1
134194:16广越Z2
134195:16龙湖Z1
134196:16龙湖Z2
134197:16鑫苑Z1
134198:16上药Z1
134199:16铁工Z1
134200:16铁工Z2
134201:16香江Z1
134202:16宏桥Z3
134203:16国创Z1
134204:16丹港Z1
134205:16龙盛Z1
134206:16龙盛Z2
134207:16武金Z1
134208:16广新Z2
134209:16国美Z2
134210:16力帆质
134212:16中交质
134213:16晋建质
134214:14上实Z2
134215:14恒泰Z5
134217:16新有质
134218:16华凌Z1
134219:16中大质
134220:16新投Z1
134221:16天铝Z1
134222:16疏浚Z1
134223:16卓越Z1
134224:16新业Z2
134225:16月星Z1
134226:16锡公Z1
134227:16住总Z1
134228:16国电Z2
134229:16珠投Z3
134230:16宏桥Z5
134231:16金茂Z1
134232:16漳九质
134233:16保利Z3
134234:16保利Z4
134235:16晋然Z1
134236:16复药Z1
134237:16纳通Z1
134238:16兴发Z1
134239:16国联Z1
134240:16北部质
134241:16中牧Z1
134242:16中车Z1
134243:16中车Z2
134244:16华夏Z2
134245:16海投Z1
134246:16津投Z1
134247:16华综Z1
134248:16外运Z1
134249:16海怡Z1
134250:16瑞茂Z1
134251:16信地Z1
134252:16亿阳Z3
134253:16中油Z3
134254:16中油Z4
134256:16南航Z1
134257:16新投Z2
134258:16财通质
134259:16龙湖Z3
134260:16龙湖Z4
134261:16长园Z1
134262:16建元Z1
134263:16建元Z2
134264:16隆基Z1
134265:16正奇Z1
134266:16鑫苑Z2
134267:16广越Z3
134268:16广越Z4
134270:16南网Z1
134271:16天富Z1
134272:16国控Z1
134273:16亿达Z1
134274:16海亮Z1
134275:16海正质
134276:16南山Z1
134277:16华地Z1
134278:16紫江Z1
134279:16渤水质
134280:16北汽Z1
134281:16华综Z2
134282:16华峰Z1
134283:16交质01
134284:16交质Z2
134285:16金隅Z1
134286:16金隅Z2
134287:16首开Z1
134288:16建发Z2
134289:16珠江Z1
134290:16航民Z1
134291:16力帆Z2
134292:16中星Z1
134293:16兆泰Z2
134294:16信地Z2
134295:16川电Z1
134296:16珠投Z4
134297:16两江Z1
134298:16青港Z1
134299:16翠微Z1
134300:16联泰Z2
134301:16龙盛Z3
134302:16龙盛Z4
134303:16世茂Z1
134304:16紫金Z1
134305:16紫金Z2
134306:16复地Z1
134307:16协信Z3
134308:16皖经Z1
134309:16云投Z1
134310:16当代Z1
134311:16中化Z1
134312:16皖投Z1
134313:16西高质
134314:16汇丰Z1
134315:16远质三
134316:16福能质
134317:15智慧Z1
134318:16中油Z5
134319:16中油Z6
134320:16宇通Z1
134321:16金泰质
134322:16宇通Z2
134323:16越交Z1
134324:16越交Z2
134325:16金地Z1
134326:16金地Z2
134327:16特房Z1
134328:16忠旺Z1
134329:16国美Z3
134330:16扬城质
134331:16金辉Z2
134332:16泰豪Z1
134334:16银宝Z1
134335:16汽集Z1
134336:16宏泰质
134337:16乌房Z1
134338:16漳诏Z1
134339:16滇路Z1
134340:16鲁星Z1
134341:16洋河Z1
134342:16浦集Z1
134343:16泸工质
134344:16广电Z1
134345:16天建Z1
134346:16天建Z2
134347:16永利质
134348:16国机质
134349:16华虹Z1
134350:16海怡Z2
134351:16永泰Z1
134352:16中天Z1
134353:16象屿质
134354:16鲁商Z1
134355:16大华Z1
134358:16川电Z2
134360:16富力Z4
134361:16富力Z5
134362:16珠管Z1
134363:16复星Z2
134364:16十二质
134365:16桂铁质
134366:16当代Z2
134367:16国君Z1
134368:16国君Z2
134369:16山鹰质
134370:16宁开质
134371:16众品Z1
134372:16光大Z1
134374:16建业Z1
134375:16恒健Z1
134376:16中希Z1
134378:16华泰Z1
134379:16精控Z1
134380:16新湖Z1
134382:16津投Z2
134383:16南港Z1
134385:16九华质
134386:16财信质
134387:16福投Z1
134389:16鲁商Z2
134390:16人福质
134391:16圆融Z1
134393:16武金Z2
134394:16武商质
134396:16粤港Z1
134397:16北水Z1
134398:16华融质
134399:16桂农Z1
134400:16金辉Z3
134401:16华润Z1
134402:16红星Z1
134403:16红星Z2
134404:16外高Z1
134405:14亿利Z2
134406:16正才Z3
134407:16正才Z4
134408:16路桥Z1
134409:16融科Z1
134410:16融科Z2
134411:16小商Z1
134414:16绵投质
134415:16华建Z1
134416:16南山Z3
134417:16万达Z2
134418:16信威Z2
134420:16中电Z1
134421:16春秋Z1
134425:16苏农Z1
134426:16电投Z1
134427:16葛洲Z2
134429:16福华Z2
134430:16浙五质
134431:16广安Z1
134432:16协信Z5
134433:16晟晏质
134434:16葛洲Z3
134435:16广汇Z1
134436:16远洋Z1
134438:16信投Z1
134439:16永泰Z2
134441:15智慧Z2
134442:16国盛Z1
134443:16蓉金Z1
134445:G16嘉质1
134446:16电投Z2
134447:16复星Z3
134448:16万达Z3
134449:16油服Z1
134450:16油服Z2
134452:16南航Z2
134453:16中工Z1
134454:16吴交Z1
134455:16银河Z1
134456:16银河Z2
134457:16希望Z1
134458:16圣牧Z1
134459:16上港Z2
134460:16市政Z1
134461:16东辰Z1
134463:16香城质
134464:16路桥Z2
134465:16国投Z1
134466:16长园Z2
134467:16东南Z1
134468:16瑞茂Z2
134469:16联通Z1
134470:16联通Z2
134471:16杨农质
134472:16青港Z2
134473:16中化质
134474:16万达Z4
134475:16华宇Z1
134477:16北控Z1
134478:16北控Z2
134479:16华能Z1
134480:16华能Z2
134482:16华福Z1
134483:16光大Z2
134484:16香江Z2
134485:16协鑫Z1
134486:16长城Z1
134487:16月星Z2
134488:16南港Z2
134489:16正集Z1
134493:16成渝Z1
134494:16滇博Z1
134496:16苏华质
134497:16西王Z2
134498:16河西Z1
134499:16洪市质
134500:16兴泰质
134501:16天风Z1
134503:16兴杭质
134504:16中关Z1
134505:16广汇Z2
134510:16华电Z1
134511:16云金Z1
134512:16广安Z2
134517:16云投Z2
134524:16联想Z1
134525:16联想Z2
134997:16铁质Y1
134999:16浙交Z1
136000:15浙国资
136001:15福能债
136002:15赣粤02
136003:15如意债
136005:15海投01
136006:15鲁星01
136007:15鲁焦02
136008:15协鑫债
136009:15红星01
136010:15中骏01
136011:14瀚华02
136012:15梅花02
136013:15财达债
136014:15福投债
136015:15华安01
136016:15赛轮债
136017:15名城01
136019:15龙湖04
136020:15华安02
136021:15新城01
136022:15东吴债
136023:15当代债
136024:15沪城开
136025:15黔路01
136026:15蒙阜丰
136027:15三福02
136028:15花园01
136029:15华宝债
136030:15吉利01
136031:15常发投
136032:15红美01
136033:15东旭02
136034:15沪国资
136035:15远东一
136036:15苏元禾
136037:15旭辉02
136038:15兴杭01
136039:15石化01
136040:15石化02
136041:15渝信01
136042:15渝信02
136043:15华凌01
136044:15通运01
136045:15复地01
136046:15中海01
136047:15国君G1
136048:15国君G2
136049:15中海02
136050:15景德01
136051:15五矿03
136052:15五矿04
136053:15南航01
136055:14国贸02
136056:15玉皇01
136057:15华发01
136058:15宜集债
136059:15纳通01
136060:15纳通02
136061:15东证债
136062:15大连港
136063:15中骏02
136064:13铁龙02
136065:15晋电01
136066:15西王01
136067:15洪市政
136068:15哈投02
136069:15双欣债
136070:15必康债
136071:15开元01
136072:15开元02
136073:15云能02
136074:15合作债
136075:15桂铁投
136076:15瑞贝卡
136077:15中天01
136078:15禹洲01
136079:15中航债
136080:15北汽01
136081:15广汇01
136082:15浙交01
136083:15浙交02
136084:15金源01
136085:15金茂投
136086:15金源02
136087:15保利01
136088:15保利02
136089:15绿地01
136090:15绿地02
136091:15华集01
136092:15连云港
136093:15华信债
136094:15晋电02
136095:15锡交01
136096:16复星01
136097:15鲁高01
136098:15义市01
136099:15绍交01
136100:16凯乐债
136101:15合景01
136102:15合景02
136103:15滇路01
136104:15市北债
136105:15三友01
136106:15三友02
136107:15穗工债
136108:14粤运02
136109:15康达债
136110:14昊华02
136111:15中环01
136112:15华集02
136113:15新燃01
136114:15花园02
136115:15广证G2
136116:15天富债
136117:15苏伟驰
136118:15融信01
136119:15国创01
136120:15鲁能债
136121:15南山02
136122:15天域债
136123:15中合01
136124:16新奥债
136125:15洛娃01
136126:15鑫苑01
136127:15中江01
136128:15宇通01
136129:15圣牧01
136130:16葛洲01
136131:15陕投债
136132:15邢钢债
136133:16国电01
136134:16番雅债
136135:16联泰01
136136:16茂业01
136137:16茂业02
136138:16常高新
136139:16国美01
136140:16富力01
136142:16中铁01
136143:16万达01
136144:16远东一
136145:16金辉01
136146:16东兴债
136147:16中粮01
136148:16宏桥01
136149:16宏桥02
136150:16桐昆01
136151:16保利01
136152:16保利02
136153:16珠投01
136154:16西王01
136155:16电建01
136156:16同益债
136157:16重水01
136158:16融信01
136159:16沪国资
136160:16东旭01
136161:16渝交投
136162:16中静01
136163:16青国信
136164:16中油01
136165:16中油02
136166:16广新01
136167:16华夏债
136168:16建发01
136169:16狮桥债
136170:16景瑞01
136171:16华证01
136172:16亿阳01
136173:16龙源01
136174:16工艺01
136175:16搜候债
136176:16绿地01
136177:16电气债
136178:16兆泰01
136179:16绿地02
136180:16国汽01
136181:16万通01
136182:16玉皇01
136183:16新华债
136184:16上港01
136185:16国发01
136186:16苏新债
136187:16景德01
136188:16富力03
136189:16新业01
136190:16正才02
136191:16靖江港
136192:16信威01
136193:16广越01
136194:16广越02
136195:16龙湖01
136196:16龙湖02
136197:16鑫苑01
136198:16上药01
136199:16铁工01
136200:16铁工02
136201:16香江01
136202:16宏桥03
136203:16国创01
136204:16丹港01
136205:16龙盛01
136206:16龙盛02
136207:16武金01
136208:16广新02
136209:16国美02
136210:16力帆债
136212:16中交债
136213:16晋建发
136214:14上实02
136215:14恒泰05
136217:16新有色
136218:16华凌01
136219:16中大债
136220:16新投01
136221:16天铝01
136222:16疏浚01
136223:16卓越01
136224:16新业02
136225:16月星01
136226:16锡公01
136227:16住总01
136228:16国电02
136229:16珠投03
136230:16宏桥05
136231:16金茂01
136232:16漳九龙
136233:16保利03
136234:16保利04
136235:16晋然01
136236:16复药01
136237:16纳通01
136238:16兴发01
136239:16国联01
136240:16北部湾
136241:16中牧01
136242:16中车G1
136243:16中车G2
136244:16华夏02
136245:16海投01
136246:16津投01
136247:16华综01
136248:16外运01
136249:16海怡01
136250:16瑞茂01
136251:16信地01
136252:16亿阳03
136253:16中油03
136254:16中油04
136255:16泰阳债
136256:16南航01
136257:16新投02
136258:16财通债
136259:16龙湖03
136260:16龙湖04
136261:16长园01
136262:16建元01
136263:16建元02
136264:16隆基01
136265:16正奇01
136266:16鑫苑02
136267:16广越03
136268:16广越04
136269:16伊品债
136270:16南网01
136271:16天富01
136272:16国控01
136273:16亿达01
136274:16海亮01
136275:16海正债
136276:16南山01
136277:16华地01
136278:16紫江01
136279:16渤水产
136280:16北汽01
136281:16华综02
136282:16华峰01
136283:16浙交01
136284:16浙交02
136285:16金隅01
136286:16金隅02
136287:16首开01
136288:16建发02
136289:16珠江01
136290:16航民01
136291:16力帆02
136292:16中星01
136293:16兆泰02
136294:16信地02
136295:16川电01
136296:16珠投04
136297:16两江01
136298:16青港01
136299:16翠微01
136300:16联泰02
136301:16龙盛03
136302:16龙盛04
136303:16世茂G1
136304:16紫金01
136305:16紫金02
136306:16复地01
136307:16协信03
136308:16皖经01
136309:16云投01
136310:16当代01
136311:16中化01
136312:16皖投01
136313:16西高科
136314:16汇丰01
136315:16远东三
136316:16福能债
136317:15智慧01
136318:16中油05
136319:16中油06
136320:16宇通01
136321:16金泰债
136322:16宇通02
136323:16越交01
136324:16越交02
136325:16金地01
136326:16金地02
136327:16特房01
136328:16忠旺01
136329:16国美03
136330:16扬城控
136331:16金辉02
136332:16泰豪01
136334:16银宝01
136335:16北汽集
136336:16宏泰债
136337:16乌房01
136338:16漳诏01
136339:16滇路01
136340:16鲁星01
136341:16洋河01
136342:16浦集01
136343:16泸工债
136344:16广电01
136345:16天建01
136346:16天建02
136347:16永利债
136348:16国机债
136349:16华虹01
136350:16海怡02
136351:16永泰01
136352:16中天01
136353:16象屿债
136354:16鲁商01
136355:16大华01
136356:16宁远高
136358:16川电02
136360:16富力04
136361:16富力05
136362:16珠管01
136363:16复星02
136364:16十二师
136365:16桂铁债
136366:16当代02
136367:16国君G1
136368:16国君G2
136369:16山鹰债
136370:16宁开控
136371:16众品01
136372:16光大01
136374:16建业01
136375:16恒健01
136376:16中希01
136378:16华泰01
136379:16精控01
136380:16新湖01
136382:16津投02
136383:16南港01
136385:16九华债
136386:16财信债
136387:16福投01
136389:16鲁商02
136390:16人福债
136391:16圆融01
136393:16武金02
136394:16武商贸
136396:16粤港01
136397:16北水01
136398:16华融德
136399:16桂农01
136400:16金辉03
136401:16华润01
136402:16红星01
136403:16红星02
136404:16外高01
136405:14亿利02
136406:16正才03
136407:16正才04
136408:16路桥01
136409:16融科01
136410:16融科02
136411:16小商01
136414:16绵投债
136415:16华建01
136416:16南山03
136417:16万达02
136418:16信威02
136420:16中电01
136421:16春秋01
136425:16苏农01
136426:16电投01
136427:16葛洲02
136429:16福华02
136430:16浙五金
136431:16广安01
136432:16协信05
136433:16晟晏债
136434:16葛洲03
136435:16广汇G1
136436:16远洋01
136438:16信投G1
136439:16永泰02
136441:15智慧02
136442:16国盛01
136443:16蓉金01
136445:G16嘉化1
136446:16电投02
136447:16复星03
136448:16万达03
136449:16油服01
136450:16油服02
136451:16远洲01
136452:16南航02
136453:16中工01
136454:16吴交01
136455:16银河G1
136456:16银河G2
136457:16希望01
136458:16圣牧01
136459:16上港02
136460:16市政01
136461:16东辰01
136463:16香城建
136464:16路桥02
136465:16国投01
136466:16长园02
136467:16东南01
136468:16瑞茂02
136469:16联通01
136470:16联通02
136471:16杨农债
136472:16青港02
136473:16中化债
136474:16万达04
136475:16华宇01
136477:16北控01
136478:16北控02
136479:16华能01
136480:16华能02
136482:16华福G1
136483:16光大02
136484:16香江02
136485:16协鑫01
136486:16长城01
136487:16月星02
136488:16南港02
136489:16正集01
136493:16成渝01
136494:16滇博01
136496:16苏华成
136497:16西王02
136498:16河西01
136499:16洪市政
136500:16兴泰债
136501:16天风01
136503:16兴杭债
136504:16中关01
136505:16广汇G2
136510:16华电01
136511:16云金01
136512:16广安02
136517:16云投02
136524:16联想01
136525:16联想02
136997:16铁建Y1
136999:16浙交Y1
140000:16青海09
140001:16青海10
140002:16青海11
140003:16青海12
140004:16内蒙05
140005:16内蒙06
140006:16内蒙07
140007:16内蒙08
140008:16河南05
140009:16河南06
140010:16河南07
140011:16河南08
140012:16河南09
140013:16河南10
140014:16河南11
140015:16河南12
140016:16天津06
140017:16天津07
140018:16天津08
140019:16天津09
140020:16天津10
140021:16天津11
140022:16天津12
140023:16河北09
140024:16河北10
140025:16河北11
140026:16河北12
140027:16河北13
140028:16河北14
140029:16河北15
140030:16贵州09
140031:16贵州10
140032:16贵州11
140033:16贵州12
140034:16湖北11
140035:16湖北12
140036:16湖北13
140037:16湖北14
140038:16湖北15
140039:16湖北16
140040:16山东13
140041:16山东14
140042:16山东15
140043:16山东16
140044:16山东17
140045:16山东18
140046:16山东19
140047:16山东20
140048:16甘肃06
140049:16重庆07
140050:16重庆08
140051:16重庆09
140052:16重庆10
140053:16重庆11
140054:16重庆12
140055:16重庆13
140056:16重庆14
140057:16广西11
140058:16广西12
140059:16广西13
140060:16广西14
140061:16广西15
140062:16广西16
140063:16广西17
140064:16江苏09
140065:16江苏10
140066:16江苏11
140067:16江苏12
140068:16江苏13
140069:16江苏14
140070:16江苏15
140071:16江苏16
140072:16浙江05
140073:16浙江06
140074:16浙江07
140075:16浙江08
140076:16浙江09
140077:16浙江10
140078:16新疆13
140079:16新疆14
140080:16新疆15
140081:16新疆16
140082:16宁夏09
140083:16宁夏10
140084:16宁夏11
140085:16宁夏12
140086:16宁夏13
140087:16宁夏14
140088:16宁夏15
140089:16广东15
140090:16广东16
140091:16广东17
140092:16广东18
140093:16广东19
140094:16广东20
140095:16广东21
140096:16福建01
140097:16福建02
140098:16福建03
140099:16福建04
140100:16福建05
140101:16福建06
140102:16四川13
140103:16四川14
140104:16四川15
140105:16四川16
140106:16四川17
140107:16四川18
140108:16四川19
140109:16四川20
140110:16吉林01
140111:16吉林02
140112:16吉林03
140113:16吉林04
140114:16吉林05
140115:16吉林06
140116:16吉林07
140117:16江西09
140118:16江西10
140119:16江西11
140120:16江西12
140121:16江西13
140122:16江西14
140123:16江西15
140124:16江西16
140125:16湖南05
140126:16湖南06
140127:16内蒙09
140128:16内蒙10
140129:16内蒙11
140130:16内蒙12
140131:16内蒙13
140132:16内蒙14
140133:16内蒙15
140134:16内蒙16
140135:16山西05
140136:16山西06
140137:16山西07
140138:16山西08
140139:16山西09
140140:16山西10
140141:16河南13
140142:16河南14
140143:16河南15
140144:16河南16
140145:16河南17
140146:16河南18
140147:16河南19
140148:16河南20
140149:16安徽05
140150:16安徽06
140151:16安徽07
140152:16安徽08
140153:16安徽09
140154:16安徽10
140155:16北京01
140156:16北京02
140157:16青海13
140158:16青海14
140159:16青海15
140160:16青海16
140161:16辽宁09
140162:16辽宁10
140163:16辽宁11
140164:16辽宁12
140165:16新疆17
140166:16新疆18
140167:16新疆19
140168:16新疆20
140169:16新疆21
140170:16新疆22
140171:16新疆23
140172:16新疆24
140173:16广东22
140174:16广东23
140175:16广东24
140176:16广东25
140177:16广东26
140178:16广东27
140179:16广东28
140180:16贵州13
140181:16贵州14
140182:16贵州15
140183:16贵州16
140184:16贵州17
140185:16贵州18
141000:16青质09
141001:16青质10
141002:16青质11
141003:16青质12
141004:16内质05
141005:16内质06
141006:16内质07
141007:16内质08
141008:16豫质05
141009:16豫质06
141010:16豫质07
141011:16豫质08
141012:16豫质09
141013:16豫质10
141014:16豫质11
141015:16豫质12
141016:16津质06
141017:16津质07
141018:16津质08
141019:16津质09
141020:16津质10
141021:16津质11
141022:16津质12
141023:16冀质09
141024:16冀质10
141025:16冀质11
141026:16冀质12
141027:16冀质13
141028:16冀质14
141029:16冀质15
141030:16黔质09
141031:16黔质10
141032:16黔质11
141033:16黔质12
141034:16鄂质11
141035:16鄂质12
141036:16鄂质13
141037:16鄂质14
141038:16鄂质15
141039:16鄂质16
141040:16鲁质13
141041:16鲁质14
141042:16鲁质15
141043:16鲁质16
141044:16鲁质17
141045:16鲁质18
141046:16鲁质19
141047:16鲁质20
141048:16甘质06
141049:16渝质07
141050:16渝质08
141051:16渝质09
141052:16渝质10
141053:16渝质11
141054:16渝质12
141055:16渝质13
141056:16渝质14
141057:16桂质11
141058:16桂质12
141059:16桂质13
141060:16桂质14
141061:16桂质15
141062:16桂质16
141063:16桂质17
141064:16苏质09
141065:16苏质10
141066:16苏质11
141067:16苏质12
141068:16苏质13
141069:16苏质14
141070:16苏质15
141071:16苏质16
141072:16浙质05
141073:16浙质06
141074:16浙质07
141075:16浙质08
141076:16浙质09
141077:16浙质10
141078:16新质13
141079:16新质14
141080:16新质15
141081:16新质16
141082:16宁质09
141083:16宁质10
141084:16宁质11
141085:16宁质12
141086:16宁质13
141087:16宁质14
141088:16宁质15
141089:16粤质15
141090:16粤质16
141091:16粤质17
141092:16粤质18
141093:16粤质19
141094:16粤质20
141095:16粤质21
141096:16闽质01
141097:16闽质02
141098:16闽质03
141099:16闽质04
141100:16闽质05
141101:16闽质06
141102:16川质13
141103:16川质14
141104:16川质15
141105:16川质16
141106:16川质17
141107:16川质18
141108:16川质19
141109:16川质20
141110:16吉质01
141111:16吉质02
141112:16吉质03
141113:16吉质04
141114:16吉质05
141115:16吉质06
141116:16吉质07
141117:16赣质09
141118:16赣质10
141119:16赣质11
141120:16赣质12
141121:16赣质13
141122:16赣质14
141123:16赣质15
141124:16赣质16
141125:16湘质05
141126:16湘质06
141127:16内质09
141128:16内质10
141129:16内质11
141130:16内质12
141131:16内质13
141132:16内质14
141133:16内质15
141134:16内质16
141135:16晋质05
141136:16晋质06
141137:16晋质07
141138:16晋质08
141139:16晋质09
141140:16晋质10
141141:16豫质13
141142:16豫质14
141143:16豫质15
141144:16豫质16
141145:16豫质17
141146:16豫质18
141147:16豫质19
141148:16豫质20
141149:16皖质05
141150:16皖质06
141151:16皖质07
141152:16皖质08
141153:16皖质09
141154:16皖质10
141155:16京质01
141156:16京质02
141157:16青质13
141158:16青质14
141159:16青质15
141160:16青质16
141161:16辽质09
141162:16辽质10
141163:16辽质11
141164:16辽质12
141165:16新质17
141166:16新质18
141167:16新质19
141168:16新质20
141169:16新质21
141170:16新质22
141171:16新质23
141172:16新质24
141173:16粤质22
141174:16粤质23
141175:16粤质24
141176:16粤质25
141177:16粤质26
141178:16粤质27
141179:16粤质28
141180:16黔质13
141181:16黔质14
141182:16黔质15
141183:16黔质16
141184:16黔质17
141185:16黔质18
190030:格力转股
190031:航信转股
190032:三一转股
190033:国贸转股
190034:九州转股
191008:电气转股
191009:广汽转股
192001:宝钢换股
192002:天集换股
201000:R003
201001:R007
201002:R014
201003:R028
201004:R091
201005:R182
201008:R001
201009:R002
201010:R004
202001:RC001
202003:RC003
202007:RC007
203016:0504R007
203017:0504R028
203018:0504R091
203040:0512R007
203041:0512R028
203042:0512R091
204001:GC001
204002:GC002
204003:GC003
204004:GC004
204007:GC007
204014:GC014
204028:GC028
204091:GC091
204182:GC182
205001:提前一天
205003:提前0003
205007:提前七天
205008:提前八天
205010:提前十天
205030:提前三十
205042:提前0042
205063:提前0063
205119:提前0119
205154:提前0154
205182:提前0182
205273:提前0273
205357:提前0357
360001:农行优1
360002:中行优1
360003:浦发优1
360005:兴业优1
360006:康美优1
360007:中建优1
360008:浦发优2
360009:农行优2
360010:中行优2
360011:工行优1
360012:兴业优2
360013:光大优1
360014:中原优1
360015:中交优1
360016:电建优1
360017:中交优2
360018:北银优1
360019:南银优1
360020:华夏优1
500038:基金通乾
500056:基金科瑞
500058:基金银丰
501000:国金鑫新
501001:财通精选
501002:能源互联
501005:精准医疗
501006:精准医C
501015:财通升级
501018:南方原油
501021:香港中小
502000:500等权
502001:500等权A
502002:500等权B
502003:军工分级
502004:军工A
502005:军工B
502006:国企改革
502007:国企改A
502008:国企改B
502010:证券分级
502011:证券A
502012:证券B
502013:一带一路
502014:一带一A
502015:一带一B
502016:带路分级
502017:带路A
502018:带路B
502020:国金50
502021:国金50A
502022:国金50B
502023:钢铁分级
502024:钢铁A
502025:钢铁B
502026:新丝路
502027:新丝路A
502028:新丝路B
502030:高铁分级
502031:高铁A
502032:高铁B
502036:互联金融
502037:网金A
502038:网金B
502040:上50分级
502041:上50A
502042:上50B
502048:50分级
502049:上证50A
502050:上证50B
502053:券商分级
502054:券商A
502055:券商B
502056:医疗分级
502057:医疗A
502058:医疗B
505888:嘉实元和
510010:治理ETF
510011:治理申赎
510012:申赎资金
510020:超大ETF
510021:超大申赎
510022:申赎资金
510030:价值ETF
510031:价值申赎
510032:申赎资金
510050:50ETF
510051:50申  赎
510052:申赎资金
510060:央企ETF
510061:央企申赎
510062:申赎资金
510070:民企ETF
510071:民企申赎
510072:申赎资金
510090:责任ETF
510091:责任申赎
510092:申赎资金
510110:周期ETF
510111:周期申赎
510112:申赎资金
510120:非周ETF
510121:非周申赎
510122:申赎资金
510130:中盘ETF
510131:中盘申赎
510132:申赎资金
510150:消费ETF
510151:消费申赎
510152:申赎资金
510160:小康ETF
510161:小康申赎
510162:申赎资金
510170:商品ETF
510171:商品申赎
510172:申赎资金
510180:180ETF
510181:180申 赎
510182:申赎资金
510190:龙头ETF
510191:龙头申赎
510192:申赎资金
510210:综指ETF
510211:综指申赎
510212:申赎资金
510220:中小ETF
510221:中小申赎
510222:申赎资金
510230:金融ETF
510231:金融申赎
510232:申赎资金
510260:新兴ETF
510261:新兴申赎
510262:申赎资金
510270:国企ETF
510271:国企申赎
510272:申赎资金
510280:成长ETF
510281:成长申赎
510282:申赎资金
510290:380ETF
510291:380申赎
510292:申赎资金
510300:300ETF
510301:300申赎
510302:申赎资金
510305:跨市资金
510310:HS300ETF
510311:沪深申赎
510312:申赎资金
510315:跨市资金
510330:华夏300
510331:华夏300
510332:申赎资金
510335:跨市资金
510360:广发300
510361:广发300
510362:申赎资金
510365:跨市资金
510410:资源ETF
510411:资源申赎
510412:申赎资金
510420:180EWETF
510421:180E申赎
510422:申赎资金
510430:50等权
510431:50E申赎
510432:申赎资金
510440:500沪市
510441:500H申赎
510442:申赎资金
510450:180高ETF
510451:180高ETF
510452:申赎资金
510500:500ETF
510501:500申赎
510502:申赎资金
510505:跨市资金
510510:广发500
510511:广发500
510512:申赎资金
510515:跨市资金
510520:诺安500
510521:诺安申赎
510522:申赎资金
510525:跨市资金
510560:国寿500
510561:国寿500
510562:申赎资金
510565:跨市资金
510580:ZZ500ETF
510581:ZZ500ETF
510582:申赎资金
510585:跨市资金
510620:材料行业
510621:材料申赎
510622:申赎资金
510630:消费行业
510631:消费申赎
510632:申赎资金
510650:金融行业
510651:金融申赎
510652:申赎资金
510660:医药行业
510661:医药申赎
510662:申赎资金
510680:万家50
510681:万家申赎
510682:申赎资金
510710:上50ETF
510711:上50申赎
510712:申赎资金
510813:上海国企
510814:认购款
510880:红利ETF
510881:红利申赎
510882:申赎资金
510900:H股ETF
510901:H股申赎
510902:申赎资金
510905:跨境资金
511010:国债ETF
511011:国债申赎
511012:申赎资金
511210:企债ETF
511211:企债申赎
511212:申赎资金
511220:城投ETF
511221:城投申赎
511222:申赎资金
511800:易货币
511801:货币申赎
511802:申赎资金
511805:货币申赎
511810:理财金H
511811:理财申赎
511812:申赎资金
511815:非沪资金
511820:鹏华添利
511821:鹏华添利
511822:申赎资金
511825:跨市资金
511830:华泰货币
511831:华泰货币
511832:申赎资金
511835:非沪资金
511850:财富宝E
511851:财富宝E
511852:申赎资金
511855:跨市资金
511860:博时货币
511861:博时申赎
511862:申赎资金
511865:博时申赎
511880:银华日利
511881:日利申赎
511882:申赎资金
511885:日利申赎
511890:景顺货币
511891:景顺货币
511892:申赎资金
511895:跨市资金
511900:富国货币
511901:富国货币
511902:申赎资金
511905:跨市资金
511910:融通货币
511911:融通货币
511912:申赎资金
511915:跨市资金
511920:广发货币
511921:广发货币
511922:申赎资金
511925:跨市资金
511930:中融日盈
511931:中融日盈
511932:申赎资金
511935:跨市资金
511960:嘉实快线
511961:嘉实快线
511962:申赎资金
511965:跨市资金
511970:国寿货币
511971:国寿货币
511972:申赎资金
511975:跨市资金
511980:现金添富
511981:现金添富
511982:申赎资金
511985:跨市资金
511990:华宝添益
511991:添益申赎
511992:申赎资金
511995:货币申赎
512010:医药ETF
512011:医药申赎
512012:申赎资金
512015:跨市资金
512070:非银ETF
512071:非银申赎
512072:申赎资金
512075:跨市资金
512110:中证地产
512111:地产申赎
512112:申赎资金
512115:跨市资金
512120:中证医药
512121:医药申赎
512122:申赎资金
512125:跨市资金
512210:景顺食品
512211:食品申赎
512212:申赎资金
512215:跨市资金
512220:景顺TMT
512221:TMT申赎
512222:申赎资金
512225:跨市资金
512230:景顺医药
512231:医药申赎
512232:申赎资金
512235:跨市资金
512300:500医药
512301:500医药
512302:申赎资金
512305:跨市资金
512310:500工业
512311:500工业
512312:申赎资金
512315:跨市资金
512330:500信息
512331:500信息
512332:申赎资金
512335:跨市资金
512340:500原料
512341:500原料
512342:申赎资金
512345:跨市资金
512500:中证500
512501:500申赎
512502:申赎资金
512505:跨市资金
512510:ETF500
512511:ETF申赎
512512:申赎资金
512515:跨市资金
512600:主要消费
512601:消费申赎
512602:申赎资金
512605:跨市资金
512610:医药卫生
512611:医药申赎
512612:申赎资金
512615:跨市资金
512640:金融地产
512641:金融申赎
512642:申赎资金
512645:跨市资金
512663:军工ETF
512664:认购款
512883:证券ETF
512884:认购款
512990:MSCIA股
512991:MSCI申赎
512992:申赎资金
512995:跨市资金
513030:德国30
513031:德国申赎
513032:申赎资金
513035:非沪资金
513100:纳指ETF
513101:纳指申赎
513102:申赎资金
513105:跨境资金
513500:标普500
513501:标普申赎
513502:申赎资金
513505:非沪资金
513600:恒指ETF
513601:恒指ETF
513602:申赎资金
513605:跨市资金
513660:恒生通
513661:恒生通
513662:申赎资金
513665:跨市资金
518800:黄金基金
518801:国泰申赎
518802:申赎资金
518805:跨市资金
518880:黄金ETF
518881:黄金申赎
518882:申赎资金
518885:现金申赎
519001:银华优选
519002:安信消费
519003:海富收益
519005:海富股票
519007:海富回报
519008:添富优势
519011:海富精选
519013:海富优势
519015:海富贰号
519017:大成成长
519018:添富均衡
519019:大成景阳
519020:国泰金泰
519021:金鼎价值
519023:海富债券
519025:海富领先
519026:海富小盘
519027:海富周期
519028:华夏稳增
519029:华夏稳增
519030:海富稳固
519032:海富非周
519033:海富国策
519034:海富低碳
519035:富国天博
519039:长盛同德
519050:海富养老
519056:海富内需
519060:海富纯C
519061:海富纯A
519062:海富对冲
519066:添富蓝筹
519068:添富焦点
519069:添富价值
519078:添富增收
519087:新华分红
519089:新华成长
519093:新华钻石
519095:新华行业
519097:新华市值
519099:新华主题
519100:长盛100
519110:价值A
519111:浦银收益
519112:收益债C
519113:浦银生活
519115:浦银红利
519116:浦银300
519117:浦银400
519118:幸福债A
519119:幸福债B
519120:新兴产业
519121:6月债A
519122:6月债C
519123:浦银添A
519124:浦银添C
519125:消费A
519126:新经济
519127:盛世A
519128:月月盈A
519129:月月盈C
519130:海富新内
519132:海富数据
519133:海富改革
519134:海富富祥
519135:海富瑞益
519150:新华消费
519152:新华纯A
519153:新华纯C
519156:新华配置
519158:新华趋势
519160:新华惠A
519161:新华惠C
519162:新华增A
519163:新华增C
519165:新华鑫利
519167:新华鑫安
519170:浦银增长
519171:浦银医疗
519172:睿智A
519173:睿智C
519180:万家 180
519181:万家和谐
519183:万家引擎
519185:万家精选
519186:万家稳增
519188:万家恒A
519189:万家恒C
519190:万家双利
519191:万家新利
519192:万家市政
519195:万家品质
519196:万家蓝筹
519197:万家颐达
519198:万家颐和
519300:大成300A
519320:聚利A
519321:聚利C
519505:海富货A
519506:海富货B
519507:万家货B
519508:万家货A
519509:浦银货A
519510:浦银货B
519511:万家薪A
519512:万家薪B
519518:添富货币
519519:友邦增利
519566:日日盈A
519567:日日盈B
519598:利息B
519599:利息A
519606:国泰金鑫
519610:银河旺A
519611:银河旺C
519640:银河鸿A
519641:银河鸿C
519642:银河智造
519644:银河智联
519651:银河转型
519652:银河鑫A
519653:银河鑫C
519654:银河泽利
519655:银河服务
519656:银河灵A
519657:银河灵C
519660:银河增A
519661:银河增C
519662:银河回A
519663:银河回C
519664:美丽A
519665:美丽C
519666:银河银信
519668:银河成长
519669:银河领先
519670:银河行业
519671:300价值
519672:银河蓝筹
519673:银河康乐
519674:银河创新
519675:银河润A
519676:银河保本
519677:定投宝
519678:银河消费
519679:银河主题
519680:交银增利
519683:交银双利
519688:交银精选
519690:交银稳健
519692:交银成长
519698:交银先锋
519700:交银主题
519702:交银趋势
519704:交银制造
519706:交银价值
519712:交银核心
519714:交银消费
519718:交银纯债
519723:交银双轮
519727:交银30
519733:交银强债
519734:交银强后
519735:交银强销
519800:保证金A
519801:保证金B
519808:嘉实宝A
519809:嘉实宝B
519858:广发宝A
519859:广发宝B
519878:国保A
519879:国保B
519888:添富快线
519889:添富快B
519898:现金宝A
519899:现金宝B
519908:基金兴华
519909:安顺配置
519915:富国消费
519918:基金兴和
519929:信息量化
519933:长信利发
519937:长信先锐
519940:富全C
519941:富全A
519942:富泰C
519943:富泰A
519944:富安C
519945:富安A
519947:长信利保
519951:长信利泰
519956:睿进C
519957:睿进A
519959:长信多利
519961:利广A
519963:利盈A
519965:量化策略
519967:长信利富
519969:长信新利
519971:长信GGHL
519972:CX纯债C
519973:CX纯债A
519975:CXLH中小
519976:CX转债C
519977:CX转债A
519979:长信内需
519983:长信量化
519985:长信CZYH
519987:长信恒利
519989:长信利丰
519991:长信双利
519993:长信增利
519995:长信金利
519997:长信银利
521134:海富富祥
521135:海富瑞益
521929:信息认购
522001:银华YXZT
522002:安信转托
522003:海富SYZT
522005:海富GPZT
522007:海富QHZT
522008:添富YSZT
522011:海富JXZT
522013:海富YSZT
522015:海富EHZT
522017:大成CZZT
522018:添富JHZT
522019:大成JYZT
522020:国泰JTZT
522021:金鼎JZZT
522023:海富ZQZT
522025:海富LXZT
522026:海富XPZT
522027:海富SZZT
522028:华夏WZZT
522029:华夏WZZT
522030:海富WGZT
522032:海富FZZT
522033:海富GCZT
522034:海富DTZT
522035:富国TBZT
522039:长盛TDZT
522050:海富YLZT
522056:海富NXZT
522060:海富CCZT
522061:海富CAZT
522062:海富DCZT
522066:添富LCZT
522068:添富JDZT
522069:添富JZZT
522078:添富ZSZT
522087:新华FHZT
522089:新华CZZT
522093:新华ZSZT
522095:新华HYZT
522097:新华SZZT
522099:新华ZTZT
522100:长盛YBZT
522110:价值AZT
522111:浦银SYZT
522112:收益ZCZT
522113:浦银SHZT
522115:浦银HLZT
522116:浦银30ZT
522117:浦银40ZT
522118:幸福ZAZT
522119:幸福ZBZT
522120:新兴CYZT
522121:6月债ZT
522122:6月ZCZT
522123:浦银添ZT
522124:浦银TCZT
522125:消费AZT
522126:新经济ZT
522127:盛世AZT
522128:YYYZTA
522129:YYYZTC
522130:海富XNZT
522132:海富SJZT
522133:海富GGZT
522150:新华XFZT
522152:新华AZT
522153:新华CZT
522156:新华PZZT
522158:新华QSZT
522160:新华HAZT
522161:新华HCZT
522162:新华ZAZT
522163:新华ZCZT
522167:新华XAZT
522170:浦银ZZZT
522171:浦银YLZT
522172:睿智ZTA
522173:睿智ZTC
522180:万家18ZT
522181:万家HXZT
522183:万家YQZT
522185:万家JXZT
522186:万家WZZT
522188:万家HAZT
522189:万家HCZT
522190:万家SLZT
522191:万家XLZT
522195:万家PZZT
522196:万家LCZT
522197:万家YDZT
522300:大成ZSZT
522505:海富HAZT
522506:海富HBZT
522507:万家HBZT
522508:万家HAZT
522509:浦银HBZT
522510:浦银HBZT
522511:万家XAZT
522512:万家XBZT
522518:添富HBZT
522519:友邦ZLZT
522566:RRYZTA
522567:RRYZTB
522598:利息BZT
522599:利息AZT
522606:国泰JXZT
522610:银河WAZT
522611:银河WCZT
522640:银河HAZT
522641:银河HCZT
522642:银河ZZZT
522644:银河ZLZT
522651:银河ZXZT
522652:银河XAZT
522653:银河XCZT
522654:银河ZLZT
522655:银河FWZT
522656:银河LAZT
522657:银河LCZT
522660:银河ZLAT
522661:银河ZLCT
522662:银河HAZT
522663:银河HCZT
522664:YHMLAZT
522665:YHMLCZT
522666:银河YXZT
522668:银河CZZT
522669:银河LXZT
522670:银河HYZT
522671:300JZZT
522672:银河LCZT
522673:银河KLZT
522674:银河CXZT
522675:银河RAZT
522676:银河BBZT
522677:DTBZT
522678:银河XFZT
522679:银河ZTZT
522680:交银ZLZT
522683:交银SLZT
522688:交银JXZT
522690:交银WJZT
522692:交银CZZT
522698:交银XFZT
522700:交银ZTZT
522702:交银QSZT
522704:交银ZZZT
522706:交银JZZT
522712:交银HXZT
522714:交银XFZT
522718:交银FCZT
522723:交银SLZT
522727:交银30ZT
522733:交银QZZT
522908:兴华转托
522909:安顺PZZT
522915:富国XFZT
522918:兴和HHZT
522933:利发转托
522937:先锐转托
522947:利保转托
522951:利泰转托
522956:睿进CZT
522957:睿进AZT
522959:多利转托
522961:利广AZT
522963:利盈AZT
522965:多策略转
522967:长信LFZT
522969:长信XLZT
522971:CXGGHLZT
522972:CXCZCZT
522973:CXCZAZT
522975:CXLHZXZT
522976:CXZZCZT
522977:CXZZAZT
522979:长信NXZT
522983:长信LHZT
522985:CXCZYHZT
522987:长信HLZT
522989:长信LFZT
522991:长信SLZT
522993:长信ZLZT
522995:长信JLZT
522997:长信YLZT
523001:银华YXFH
523002:安信分红
523003:海富SYFH
523005:海富GPFH
523007:海富QHFH
523008:添富YSFH
523011:海富JXFH
523013:海富YSFH
523015:海富EHFH
523017:大成CZFH
523018:添富JHFH
523019:大成JYFH
523020:国泰JTFH
523021:金鼎JZFH
523023:海富ZQFH
523025:海富LXFH
523026:海富XPFH
523027:海富SZFH
523028:华夏WZFH
523029:华夏WZFH
523030:海富WGFH
523032:海富FZFH
523033:海富GCFH
523034:海富DTFH
523035:富国TBFH
523039:长盛TDFH
523050:海富YLFH
523056:海富NXFH
523060:海富CCFH
523061:海富CAFH
523062:海富DCFH
523066:添富LCFH
523068:添富JDFH
523069:添富JZFH
523078:添富ZSFH
523087:新华FHFH
523089:新华CZFH
523093:新华ZSFH
523095:新华HYFH
523097:新华SZFH
523099:新华ZTFH
523100:长盛YBFH
523110:价值AFH
523111:浦银SYFH
523112:收益ZCFH
523113:浦银SHFH
523115:浦银HLFH
523116:浦银30FH
523117:浦银40FH
523118:幸福ZAFH
523120:新兴CYFH
523121:6月债FH
523123:浦银添FH
523124:浦银TCFH
523125:消费AFH
523126:新经济FH
523127:盛世AFH
523128:YYYFHA
523129:YYYFHC
523130:海富XNFH
523132:海富SJFH
523133:海富GGFH
523150:新华XFFH
523152:新华AFH
523153:新华CFH
523156:新华PZFH
523158:新华QSFH
523160:新华HAFH
523161:新华HCFH
523162:新华ZAFH
523163:新华ZCFH
523167:新华XAFH
523170:浦银ZZFH
523171:浦银YLFH
523172:睿智FHA
523173:睿智FHC
523180:万家18FH
523181:万家HXFH
523183:万家YQFH
523185:万家JXFH
523186:万家WZFH
523188:万家HAFH
523189:万家HCFH
523190:万家SLFH
523191:万家XLFH
523195:万家PZFH
523196:万家LCFH
523197:万家YDFH
523300:大成ZSFH
523519:友邦ZLFH
523606:国泰JXFH
523610:银河WAFH
523611:银河WCFH
523640:银河HAFH
523641:银河HCFH
523642:银河ZZFH
523644:银河ZLFH
523651:银河ZXFH
523652:银河XAFH
523653:银河XCFH
523654:银河ZLFH
523655:银河FWFH
523656:银河LAFH
523657:银河LCFH
523660:银河ZLAF
523661:银河ZLCF
523662:银河HAFH
523663:银河HCFH
523664:YHMLAFH
523665:YHMLCFH
523666:银河YXFH
523668:银河CZFH
523669:银河LXFH
523670:银河HYFH
523671:300JZFH
523672:银河LCFH
523673:银河KLFH
523674:银河CXFH
523675:银河RAFH
523676:银河BBFH
523677:DTBFH
523678:银河XFFH
523679:银河ZTFH
523680:交银ZLFH
523683:交银SLFH
523688:交银JXFH
523690:交银WJFH
523692:交银CZFH
523698:交银XFFH
523700:交银ZTFH
523702:交银QSFH
523704:交银ZZFH
523706:交银JZFH
523712:交银HXFH
523714:交银XFFH
523718:交银FCFH
523723:交银SLFH
523727:交银30FH
523733:交银QZFH
523908:兴华分红
523909:安顺PZFH
523915:富国XFFH
523918:兴和HHFH
523933:利发分红
523937:先锐分红
523947:利保分红
523951:利泰分红
523956:睿进CFH
523957:睿进AFH
523959:多利分红
523961:利广AFH
523963:利盈AFH
523965:多策略分
523967:长信LFFH
523969:长信XLFH
523971:CXGGHLFH
523972:CXCZCFH
523973:CXCZAFH
523975:CXLHZXFH
523976:CXZZCFH
523977:CXZZAFH
523979:长信NXFH
523983:长信LHFH
523985:CXCZYHFH
523987:长信HLFH
523989:长信LFFH
523991:长信SLFH
523993:长信ZLFH
523995:长信JLFH
523997:长信YLFH
600000:浦发银行
600004:白云机场
600005:武钢股份
600006:东风汽车
600007:中国国贸
600008:首创股份
600009:上海机场
600010:包钢股份
600011:华能国际
600012:皖通高速
600015:华夏银行
600016:民生银行
600017:日照港
600018:上港集团
600019:宝钢股份
600020:中原高速
600021:上海电力
600022:山东钢铁
600023:浙能电力
600026:中海发展
600027:华电国际
600028:中国石化
600029:南方航空
600030:中信证券
600031:三一重工
600033:福建高速
600035:楚天高速
600036:招商银行
600037:歌华有线
600038:中直股份
600039:四川路桥
600048:保利地产
600050:中国联通
600051:宁波联合
600052:浙江广厦
600053:九鼎投资
600054:黄山旅游
600055:华润万东
600056:中国医药
600057:象屿股份
600058:五矿发展
600059:古越龙山
600060:海信电器
600061:国投安信
600062:华润双鹤
600063:皖维高新
600064:南京高科
600066:宇通客车
600067:冠城大通
600068:葛洲坝
600069:银鸽投资
600070:浙江富润
600071:凤凰光学
600072:钢构工程
600073:上海梅林
600074:保千里
600075:新疆天业
600076:康欣新材
600077:宋都股份
600078:澄星股份
600079:人福医药
600080:金花股份
600081:东风科技
600082:海泰发展
600083:博信股份
600084:中葡股份
600085:同仁堂
600086:东方金钰
600088:中视传媒
600089:特变电工
600090:啤酒花
600091:ST明科
600093:禾嘉股份
600094:大名城
600095:哈高科
600096:云天化
600097:开创国际
600098:广州发展
600099:林海股份
600100:同方股份
600101:明星电力
600103:青山纸业
600104:上汽集团
600105:永鼎股份
600106:重庆路桥
600107:美尔雅
600108:亚盛集团
600109:国金证券
600110:诺德股份
600111:北方稀土
600112:天成控股
600113:浙江东日
600114:东睦股份
600115:东方航空
600116:三峡水利
600117:西宁特钢
600118:中国卫星
600119:长江投资
600120:浙江东方
600121:郑州煤电
600122:宏图高科
600123:兰花科创
600125:铁龙物流
600126:杭钢股份
600127:金健米业
600128:弘业股份
600129:太极集团
600130:波导股份
600131:岷江水电
600132:重庆啤酒
600133:东湖高新
600135:乐凯胶片
600136:当代明诚
600137:浪莎股份
600138:中青旅
600139:西部资源
600141:兴发集团
600143:金发科技
600145:*ST新亿
600146:商赢环球
600148:长春一东
600149:廊坊发展
600150:中国船舶
600151:航天机电
600152:维科精华
600153:建发股份
600155:宝硕股份
600156:华升股份
600157:永泰能源
600158:中体产业
600159:大龙地产
600160:巨化股份
600161:天坛生物
600162:香江控股
600163:中闽能源
600165:新日恒力
600166:福田汽车
600167:联美控股
600168:武汉控股
600169:太原重工
600170:上海建工
600171:上海贝岭
600172:黄河旋风
600173:卧龙地产
600175:美都能源
600176:中国巨石
600177:雅戈尔
600178:东安动力
600179:*ST黑化
600180:瑞茂通
600182:S佳通
600183:生益科技
600184:光电股份
600185:格力地产
600186:莲花健康
600187:国中水务
600188:兖州煤业
600189:吉林森工
600190:锦州港
600191:华资实业
600192:长城电工
600193:创兴资源
600195:中牧股份
600196:复星医药
600197:伊力特
600198:大唐电信
600199:金种子酒
600200:江苏吴中
600201:生物股份
600202:哈空调
600203:福日电子
600206:有研新材
600207:安彩高科
600208:新湖中宝
600209:罗顿发展
600210:紫江企业
600211:西藏药业
600212:*ST江泉
600213:亚星客车
600215:长春经开
600216:浙江医药
600217:秦岭水泥
600218:全柴动力
600219:南山铝业
600220:江苏阳光
600221:海南航空
600222:太龙药业
600223:鲁商置业
600225:天津松江
600226:升华拜克
600227:赤天化
600228:昌九生化
600229:城市传媒
600230:*ST沧大
600231:凌钢股份
600232:金鹰股份
600233:大杨创世
600234:*ST山水
600235:民丰特纸
600236:桂冠电力
600237:铜峰电子
600238:海南椰岛
600239:云南城投
600240:华业资本
600241:时代万恒
600242:中昌海运
600243:青海华鼎
600246:万通地产
600247:ST成城
600248:延长化建
600249:两面针
600250:南纺股份
600251:冠农股份
600252:中恒集团
600255:鑫科材料
600256:广汇能源
600257:大湖股份
600258:首旅酒店
600259:广晟有色
600260:凯乐科技
600261:阳光照明
600262:北方股份
600265:*ST景谷
600266:北京城建
600267:海正药业
600268:国电南自
600269:赣粤高速
600270:外运发展
600271:航天信息
600272:开开实业
600273:嘉化能源
600275:武昌鱼
600276:恒瑞医药
600277:亿利洁能
600278:东方创业
600279:重庆港九
600280:中央商场
600281:太化股份
600282:南钢股份
600283:钱江水利
600284:浦东建设
600285:羚锐制药
600287:江苏舜天
600288:大恒科技
600289:亿阳信通
600290:华仪电气
600291:西水股份
600292:远达环保
600293:三峡新材
600295:鄂尔多斯
600297:广汇汽车
600298:安琪酵母
600299:安迪苏
600300:维维股份
600301:*ST南化
600302:标准股份
600303:曙光股份
600305:恒顺醋业
600306:*ST商城
600307:酒钢宏兴
600308:华泰股份
600309:万华化学
600310:桂东电力
600311:荣华实业
600312:平高电气
600313:农发种业
600315:上海家化
600316:洪都航空
600317:营口港
600318:新力金融
600319:*ST亚星
600320:振华重工
600321:国栋建设
600322:天房发展
600323:瀚蓝环境
600325:华发股份
600326:西藏天路
600327:大东方
600328:兰太实业
600329:中新药业
600330:天通股份
600331:宏达股份
600332:白云山
600333:长春燃气
600335:国机汽车
600336:澳柯玛
600337:美克家居
600338:西藏珠峰
600339:*ST天利
600340:华夏幸福
600343:航天动力
600345:长江通信
600346:*ST橡塑
600348:阳泉煤业
600350:山东高速
600351:亚宝药业
600352:浙江龙盛
600353:旭光股份
600354:敦煌种业
600355:精伦电子
600356:恒丰纸业
600358:国旅联合
600359:新农开发
600360:华微电子
600361:华联综超
600362:江西铜业
600363:联创光电
600365:通葡股份
600366:宁波韵升
600367:红星发展
600368:五洲交通
600369:西南证券
600370:三房巷
600371:万向德农
600372:中航电子
600373:中文传媒
600375:*ST星马
600376:首开股份
600377:宁沪高速
600378:天科股份
600379:宝光股份
600380:健康元
600381:ST春天
600382:广东明珠
600383:金地集团
600385:山东金泰
600386:北巴传媒
600387:海越股份
600388:龙净环保
600389:江山股份
600390:*ST金瑞
600391:成发科技
600392:盛和资源
600393:粤泰股份
600395:盘江股份
600396:金山股份
600397:安源煤业
600398:海澜之家
600399:抚顺特钢
600400:红豆股份
600401:海润光伏
600403:大有能源
600405:动力源
600406:国电南瑞
600408:安泰集团
600409:三友化工
600410:华胜天成
600415:小商品城
600416:湘电股份
600418:江淮汽车
600419:天润乳业
600420:现代制药
600421:仰帆控股
600422:昆药集团
600423:柳化股份
600425:青松建化
600426:华鲁恒升
600428:中远航运
600429:三元股份
600432:*ST吉恩
600433:冠豪高新
600435:北方导航
600436:片仔癀
600438:通威股份
600439:瑞贝卡
600444:国机通用
600446:金证股份
600448:华纺股份
600449:宁夏建材
600452:涪陵电力
600455:博通股份
600456:宝钛股份
600458:时代新材
600459:贵研铂业
600460:士兰微
600461:洪城水业
600462:九有股份
600463:空港股份
600466:蓝光发展
600467:好当家
600468:百利电气
600469:风神股份
600470:六国化工
600475:华光股份
600476:湘邮科技
600477:杭萧钢构
600478:科力远
600479:千金药业
600480:凌云股份
600481:双良节能
600482:中国动力
600483:福能股份
600485:信威集团
600486:扬农化工
600487:亨通光电
600488:天药股份
600489:中金黄金
600490:鹏欣资源
600491:龙元建设
600493:凤竹纺织
600495:晋西车轴
600496:精工钢构
600497:驰宏锌锗
600498:烽火通信
600499:DR科达洁
600500:中化国际
600501:航天晨光
600502:安徽水利
600503:华丽家族
600505:西昌电力
600506:香梨股份
600507:方大特钢
600508:上海能源
600509:天富能源
600510:黑牡丹
600511:国药股份
600512:腾达建设
600513:联环药业
600515:海航基础
600516:方大炭素
600517:置信电气
600518:康美药业
600519:贵州茅台
600520:*ST中发
600521:华海药业
600522:中天科技
600523:贵航股份
600525:长园集团
600526:菲达环保
600527:江南高纤
600528:中铁二局
600529:山东药玻
600530:交大昂立
600531:豫光金铅
600532:宏达矿业
600533:栖霞建设
600535:天士力
600536:中国软件
600537:亿晶光电
600538:国发股份
600539:ST狮头
600540:新赛股份
600543:莫高股份
600545:新疆城建
600546:*ST山煤
600547:山东黄金
600548:深高速
600549:厦门钨业
600550:保变电气
600551:时代出版
600552:凯盛科技
600555:海航创新
600556:慧球科技
600557:康缘药业
600558:大西洋
600559:老白干酒
600560:金自天正
600561:江西长运
600562:国睿科技
600563:法拉电子
600565:迪马股份
600566:济川药业
600567:山鹰纸业
600568:中珠医疗
600569:安阳钢铁
600570:恒生电子
600571:信雅达
600572:康恩贝
600573:惠泉啤酒
600575:皖江物流
600576:万家文化
600577:精达股份
600578:京能电力
600579:天华院
600580:卧龙电气
600581:*ST八钢
600582:天地科技
600583:海油工程
600584:长电科技
600585:海螺水泥
600586:金晶科技
600587:新华医疗
600588:用友网络
600589:广东榕泰
600590:泰豪科技
600592:龙溪股份
600593:大连圣亚
600594:益佰制药
600595:中孚实业
600596:新安股份
600597:光明乳业
600598:北大荒
600599:熊猫金控
600600:青岛啤酒
600601:方正科技
600602:云赛智联
600603:*ST兴业
600604:市北高新
600605:汇通能源
600606:绿地控股
600608:ST沪科
600609:金杯汽车
600610:中毅达
600611:大众交通
600612:老凤祥
600613:神奇制药
600614:鼎立股份
600615:丰华股份
600616:金枫酒业
600617:国新能源
600618:氯碱化工
600619:海立股份
600620:天宸股份
600621:华鑫股份
600622:嘉宝集团
600623:华谊集团
600624:复旦复华
600626:申达股份
600628:新世界
600629:华建集团
600630:龙头股份
600633:浙报传媒
600634:中技控股
600635:大众公用
600636:三爱富
600637:东方明珠
600638:新黄浦
600639:浦东金桥
600640:号百控股
600641:万业企业
600642:申能股份
600643:爱建集团
600644:乐山电力
600645:中源协和
600647:同达创业
600648:外高桥
600649:城投控股
600650:锦江投资
600651:XD飞乐音
600652:游久游戏
600653:申华控股
600654:中安消
600655:豫园商城
600657:信达地产
600658:电子城
600660:福耀玻璃
600661:新南洋
600662:强生控股
600663:陆家嘴
600664:哈药股份
600665:天地源
600666:奥瑞德
600667:太极实业
600668:尖峰集团
600671:天目药业
600673:东阳光科
600674:川投能源
600675:*ST中企
600676:交运股份
600677:航天通信
600678:四川金顶
600679:上海凤凰
600680:上海普天
600681:百川能源
600682:南京新百
600683:京投发展
600684:珠江实业
600685:中船防务
600686:金龙汽车
600687:刚泰控股
600688:上海石化
600689:上海三毛
600690:青岛海尔
600691:阳煤化工
600692:亚通股份
600693:东百集团
600694:大商股份
600695:绿庭投资
600696:匹凸匹
600697:欧亚集团
600698:湖南天雁
600699:均胜电子
600701:*ST工新
600702:沱牌舍得
600703:三安光电
600704:物产中大
600705:中航资本
600706:曲江文旅
600707:彩虹股份
600708:光明地产
600710:*ST常林
600711:盛屯矿业
600712:南宁百货
600713:南京医药
600714:金瑞矿业
600715:文投控股
600716:凤凰股份
600717:天津港
600718:东软集团
600719:大连热电
600720:祁连山
600721:*ST百花
600722:金牛化工
600723:首商股份
600724:宁波富达
600725:*ST云维
600726:华电能源
600727:鲁北化工
600728:佳都科技
600729:重庆百货
600730:中国高科
600731:湖南海利
600732:*ST新梅
600733:S前锋
600734:实达集团
600735:新华锦
600736:苏州高新
600737:中粮屯河
600738:兰州民百
600739:辽宁成大
600740:山西焦化
600741:华域汽车
600742:一汽富维
600743:华远地产
600744:华银电力
600745:中茵股份
600746:江苏索普
600747:大连控股
600748:上实发展
600749:西藏旅游
600750:江中药业
600751:天海投资
600753:东方银星
600754:锦江股份
600755:厦门国贸
600756:浪潮软件
600757:长江传媒
600758:红阳能源
600759:洲际油气
600760:*ST黑豹
600761:安徽合力
600763:通策医疗
600764:中电广通
600765:中航重机
600766:园城黄金
600767:运盛医疗
600768:宁波富邦
600769:祥龙电业
600770:综艺股份
600771:广誉远
600773:西藏城投
600774:汉商集团
600775:南京熊猫
600776:东方通信
600777:新潮能源
600778:友好集团
600779:水井坊
600780:通宝能源
600781:辅仁药业
600782:新钢股份
600783:鲁信创投
600784:鲁银投资
600785:新华百货
600787:中储股份
600789:鲁抗医药
600790:轻纺城
600791:京能置业
600792:云煤能源
600793:ST宜纸
600794:保税科技
600795:国电电力
600796:钱江生化
600797:浙大网新
600798:宁波海运
600800:天津磁卡
600801:华新水泥
600802:福建水泥
600803:新奥股份
600804:鹏博士
600805:悦达投资
600806:*ST昆机
600807:天业股份
600808:马钢股份
600809:山西汾酒
600810:神马股份
600811:东方集团
600812:华北制药
600814:杭州解百
600815:厦工股份
600816:安信信托
600817:*ST宏盛
600818:中路股份
600819:耀皮玻璃
600820:隧道股份
600821:津劝业
600822:上海物贸
600823:世茂股份
600824:益民集团
600825:新华传媒
600826:兰生股份
600827:百联股份
600828:茂业商业
600829:人民同泰
600830:香溢融通
600831:广电网络
600833:第一医药
600834:申通地铁
600835:上海机电
600836:界龙实业
600837:海通证券
600838:上海九百
600839:四川长虹
600841:上柴股份
600843:上工申贝
600844:丹化科技
600845:宝信软件
600846:同济科技
600847:万里股份
600848:上海临港
600850:华东电脑
600851:海欣股份
600853:龙建股份
600854:春兰股份
600855:航天长峰
600856:中天能源
600857:宁波中百
600858:银座股份
600859:王府井
600860:京城股份
600861:北京城乡
600862:中航高科
600863:内蒙华电
600864:哈投股份
600865:百大集团
600866:*ST星湖
600867:通化东宝
600868:梅雁吉祥
600869:智慧能源
600870:厦华电子
600871:石化油服
600872:中炬高新
600873:梅花生物
600874:创业环保
600875:东方电气
600876:洛阳玻璃
600877:中国嘉陵
600879:航天电子
600880:博瑞传播
600881:亚泰集团
600882:华联矿业
600883:博闻科技
600884:杉杉股份
600885:宏发股份
600886:国投电力
600887:伊利股份
600888:新疆众和
600889:南京化纤
600890:中房股份
600891:秋林集团
600892:大晟文化
600893:中航动力
600894:广日股份
600895:张江高科
600896:中海海盛
600897:厦门空港
600898:三联商社
600900:长江电力
600917:重庆燃气
600958:东方证券
600959:江苏有线
600960:渤海活塞
600961:株冶集团
600962:国投中鲁
600963:岳阳林纸
600965:福成股份
600966:博汇纸业
600967:北方创业
600969:郴电国际
600970:中材国际
600971:恒源煤电
600973:宝胜股份
600975:新五丰
600976:健民集团
600978:宜华生活
600979:广安爱众
600980:北矿科技
600981:汇鸿集团
600982:宁波热电
600983:惠而浦
600984:建设机械
600985:雷鸣科化
600986:科达股份
600987:航民股份
600988:赤峰黄金
600990:四创电子
600992:贵绳股份
600993:马应龙
600995:文山电力
600997:开滦股份
600998:九州通
600999:招商证券
601000:唐山港
601001:大同煤业
601002:晋亿实业
601003:柳钢股份
601005:重庆钢铁
601006:大秦铁路
601007:金陵饭店
601008:连云港
601009:南京银行
601010:文峰股份
601011:宝泰隆
601012:隆基股份
601015:陕西黑猫
601016:节能风电
601018:宁波港
601020:华钰矿业
601021:春秋航空
601028:玉龙股份
601038:一拖股份
601058:赛轮金宇
601069:西部黄金
601088:中国神华
601098:中南传媒
601099:太平洋
601100:恒立液压
601101:昊华能源
601106:中国一重
601107:四川成渝
601111:中国国航
601113:华鼎股份
601116:三江购物
601117:中国化学
601118:海南橡胶
601126:四方股份
601127:小康股份
601137:博威合金
601139:深圳燃气
601155:新城控股
601158:重庆水务
601166:兴业银行
601168:西部矿业
601169:北京银行
601177:杭齿前进
601179:中国西电
601186:中国铁建
601188:龙江交通
601198:东兴证券
601199:江南水务
601208:东材科技
601211:国泰君安
601216:君正集团
601218:吉鑫科技
601222:林洋能源
601225:陕西煤业
601226:华电重工
601231:环旭电子
601233:桐昆股份
601238:广汽集团
601258:庞大集团
601288:农业银行
601311:骆驼股份
601313:江南嘉捷
601318:中国平安
601328:交通银行
601333:XD广深铁
601336:新华保险
601339:百隆东方
601368:绿城水务
601369:陕鼓动力
601377:兴业证券
601388:怡球资源
601390:中国中铁
601398:工商银行
601515:东风股份
601518:吉林高速
601519:大智慧
601555:东吴证券
601558:华锐风电
601566:九牧王
601567:三星医疗
601579:会稽山
601588:北辰实业
601599:鹿港文化
601600:中国铝业
601601:中国太保
601607:上海医药
601608:中信重工
601611:中国核建
601616:广电电气
601618:中国中冶
601628:中国人寿
601633:长城汽车
601636:旗滨集团
601666:平煤股份
601668:中国建筑
601669:中国电建
601677:明泰铝业
601678:滨化股份
601688:华泰证券
601689:拓普集团
601699:潞安环能
601700:风范股份
601717:郑煤机
601718:际华集团
601727:上海电气
601766:中国中车
601777:力帆股份
601788:光大证券
601789:宁波建工
601798:蓝科高新
601799:星宇股份
601800:中国交建
601801:皖新传媒
601808:中海油服
601818:光大银行
601857:中国石油
601866:中海集运
601872:招商轮船
601877:正泰电器
601880:大连港
601886:江河集团
601888:中国国旅
601890:亚星锚链
601898:中煤能源
601899:紫金矿业
601900:南方传媒
601901:方正证券
601908:京运通
601918:*ST新集
601919:中国远洋
601928:凤凰传媒
601929:吉视传媒
601933:永辉超市
601939:建设银行
601958:金钼股份
601965:中国汽研
601966:玲珑轮胎
601968:宝钢包装
601969:海南矿业
601985:中国核电
601988:中国银行
601989:中国重工
601991:大唐发电
601992:金隅股份
601996:丰林集团
601998:XD中信银
601999:出版传媒
603000:人民网
603001:奥康国际
603002:宏昌电子
603003:龙宇燃油
603005:晶方科技
603006:联明股份
603008:喜临门
603009:北特科技
603010:万盛股份
603011:合锻智能
603012:创力集团
603015:弘讯科技
603016:新宏泰
603017:中衡设计
603018:设计股份
603019:中科曙光
603020:爱普股份
603021:山东华鹏
603022:新通联
603023:威帝股份
603025:大豪科技
603026:石大胜华
603027:千禾味业
603028:赛福天
603029:天鹅股份
603030:全筑股份
603066:音飞储存
603069:海汽集团
603077:和邦生物
603085:天成自控
603088:宁波精达
603099:长白山
603100:川仪股份
603101:汇嘉时代
603108:润达医疗
603111:康尼机电
603116:红蜻蜓
603117:万林股份
603118:共进股份
603123:翠微股份
603126:中材节能
603128:华贸物流
603131:上海沪工
603158:腾龙股份
603166:福达股份
603167:渤海轮渡
603168:莎普爱思
603169:兰石重装
603188:亚邦股份
603198:迎驾贡酒
603199:九华旅游
603222:济民制药
603223:恒通股份
603227:雪峰科技
603268:松发股份
603288:海天味业
603299:井神股份
603300:华铁科技
603306:华懋科技
603308:应流股份
603309:维力医疗
603311:金海环境
603315:福鞍股份
603318:派思股份
603328:依顿电子
603333:明星电缆
603338:浙江鼎力
603339:四方冷链
603355:莱克电气
603366:日出东方
603368:柳州医药
603369:今世缘
603377:东方时尚
603398:邦宝益智
603399:新华龙
603456:九洲药业
603508:思维列控
603518:维格娜丝
603519:立霸股份
603520:司太立
603528:多伦科技
603555:贵人鸟
603558:健盛集团
603566:普莱柯
603567:珍宝岛
603568:伟明环保
603588:高能环境
603589:口子窖
603598:引力传媒
603599:广信股份
603600:永艺股份
603601:再升科技
603606:东方电缆
603608:天创时尚
603609:禾丰牧业
603611:诺力股份
603616:韩建河山
603618:杭电股份
603636:南威软件
603669:灵康药业
603678:火炬电子
603686:龙马环卫
603688:石英股份
603696:安记食品
603698:航天工程
603699:纽威股份
603701:德宏股份
603703:盛洋科技
603718:海利生物
603726:朗迪集团
603729:龙韵股份
603737:三棵树
603766:隆鑫通用
603778:乾景园林
603779:威龙股份
603788:宁波高发
603789:星光农机
603798:康普顿
603799:华友钴业
603800:道森股份
603806:福斯特
603808:歌力思
603818:曲美家居
603822:嘉澳环保
603828:DR柯利达
603838:四通股份
603861:白云电器
603866:桃李面包
603868:飞科电器
603869:北部湾旅
603883:老百姓
603885:吉祥航空
603889:新澳股份
603898:好莱客
603899:晨光文具
603901:永创智能
603909:合诚股份
603918:金桥信息
603919:金徽酒
603936:博敏电子
603939:益丰药房
603958:哈森股份
603959:百利科技
603968:醋化股份
603969:银龙股份
603979:金诚信
603988:中电电机
603989:艾华集团
603993:洛阳钼业
603996:中新科技
603997:继峰股份
603998:方盛制药
603999:读者传媒
706035:东贝收购
730919:苏银申购
732663:三祥申购
736663:三祥配号
741919:苏银配号
751001:广发证券
751002:广发证券
751003:国泰君安
751004:国泰君安
751005:海通证券
751006:上海证券
751007:华泰证券
751008:华泰证券
751009:申万宏源
751010:渤海证券
751011:长江证券
751012:申万宏源
751014:长江证券
751015:平安证券
751016:中信证券
751017:平安证券
751018:光大证券
751019:中信证券
751020:光大证券
751021:国信证券
751022:招商证券
751023:国信证券
751024:招商证券
751025:中金公司
751026:华安证券
751028:南京银行
751029:中金公司
751030:北京银行
751031:东方证券
751032:兴业银行
751034:国海证券
751035:东吴证券
751036:中信建投
751037:安信证券
751038:第一创业
751039:第一创业
751040:第一创业
751041:第一创业
751043:国元证券
751044:兴业证券
751046:中国人寿
751047:南京银行
751048:北京银行
751052:东方证券
751053:西部证券
751054:兴业银行
751057:中科证券
751058:汉唐证券
751059:金通证券
751060:华泰财险
751061:社保基金
751062:国海证券
751063:世纪证券
751065:邮储银行
751066:中信建投
751067:安信证券
751068:广发证券
751069:国泰君安
751071:华泰证券
751072:申万宏源
751074:长江证券
751075:平安证券
751076:中信证券
751077:光大证券
751078:国信证券
751079:招商证券
751080:中金公司
751081:兴业证券
751082:中国人寿
751083:南京银行
751084:北京银行
751085:东方证券
751086:兴业银行
751087:交通银行
751088:国海证券
751090:中信建投
751091:安信证券
751092:广发证券
751093:国泰君安
751095:华泰证券
751096:申万宏源
751097:渤海证券
751098:长江证券
751099:平安证券
751100:中信证券
751101:光大证券
751102:国信证券
751103:招商证券
751104:中金公司
751106:中国人寿
751107:南京银行
751108:北京银行
751109:东方证券
751110:兴业银行
751111:交通银行
751112:国海证券
751113:浦发银行
751114:中信建投
751115:安信证券
751121:宁波银行
751122:宁波银行
751123:宁波银行
751124:宁波银行
751200:工商银行
751201:工商银行
751204:农业银行
751205:农业银行
751208:建设银行
751209:建设银行
751212:交通银行
751213:交通银行
751216:中信银行
751217:中信银行
751220:光大银行
751221:光大银行
751224:华夏银行
751225:华夏银行
751228:浦发银行
751229:浦发银行
751232:兴业银行
751233:兴业银行
751236:招商银行
751237:招商银行
751240:平安银行
751241:平安银行
751244:民生银行
751245:民生银行
751248:北京银行
751249:北京银行
751252:南京银行
751253:南京银行
751256:宁波银行
751257:宁波银行
751260:中国人寿
751261:中国人寿
751264:中国平安
751265:中国平安
751268:泰康资产
751269:泰康资产
751272:国泰君安
751273:国泰君安
751276:申银万国
751277:申银万国
751280:中信证券
751281:中信证券
751284:中银国际
751285:中银国际
751288:招商证券
751289:招商证券
751292:第一创业
751293:第一创业
751296:广发证券
751297:广发证券
751300:东方证券
751301:东方证券
751304:长江证券
751305:长江证券
751308:中金公司
751309:中金公司
751312:中信建投
751313:中信建投
751316:国海证券
751317:国海证券
751320:中投证券
751321:中投证券
751324:安信证券
751325:安信证券
751328:平安证券
751329:平安证券
751332:宏源证券
751333:宏源证券
751336:东海证券
751337:东海证券
751340:信达证券
751341:信达证券
751344:长城证券
751345:长城证券
751348:华泰证券
751349:华泰证券
751352:中德证券
751353:中德证券
751356:中山证券
751357:中山证券
751360:摩根华鑫
751361:摩根华鑫
751364:华融证券
751365:华融证券
751368:光大证券
751369:光大证券
751372:银河证券
751373:银河证券
751376:国开证券
751377:国开证券
751380:民生证券
751381:民生证券
751384:国信证券
751385:国信证券
751800:WI161010
751801:WI161101
751803:WI161603
751805:WI161505
751807:WI161407
751810:WI161010
751811:WI161101
751812:WI140307
751813:WI160903
751815:WI160705
751817:WI161407
751850:14武控02
751851:13牡丹02
751900:中原证券
751901:中原证券
751902:中原证券
751903:中原证券
751904:中原证券
751905:中原证券
751906:中原证券
751907:中原证券
751909:中信证券
751910:中信证券
751911:中信证券
751920:南京银行
751924:光大证券
751925:光大证券
751926:光大证券
751930:交通银行
751936:中国人保
751940:国海证券
751941:国海证券
751948:北京银行
751949:北京银行
751956:交通银行
751968:交通银行
751970:16电投02
751971:16电投03
751972:16深燃01
751973:14国电03
751974:14亨通02
751975:15赣粤02
751976:14渝路01
751977:14昊华01
751978:16成渝01
751979:14瀚华02
751980:15闽高速
751981:15中投G1
751982:15中投G2
751983:15国盛EB
751984:15沪国资
751985:14国贸02
751986:14西南02
751987:15昆药债
751988:15五矿03
751989:14首开债
751990:15东证债
751991:13铁龙02
751992:16沪国资
751993:14招金债
751994:16沪国资
751995:16国电01
751996:14恒泰05
751997:16国电02
751998:14富贵鸟
751999:16电投01
799981:余券划转
799982:还券划转
799983:担保划转
799984:券源划转
799988:密码服务
799998:撤销指定
799999:登记指定
888880:新标准券
900901:云赛Ｂ股
900902:市北B股
900903:大众Ｂ股
900904:神奇B股
900905:老凤祥Ｂ
900906:中毅达B
900907:鼎立Ｂ股
900908:氯碱Ｂ股
900909:华谊B股
900910:海立Ｂ股
900911:金桥Ｂ股
900912:外高Ｂ股
900913:国新B股
900914:锦投Ｂ股
900915:中路Ｂ股
900916:凤凰B股
900917:海欣Ｂ股
900918:耀皮Ｂ股
900919:绿庭B股
900920:上柴Ｂ股
900921:丹科Ｂ股
900922:三毛B股
900923:百联Ｂ股
900924:上工Ｂ股
900925:机电Ｂ股
900926:宝信Ｂ
900927:物贸Ｂ股
900928:临港B股
900929:锦旅Ｂ股
900930:沪普天Ｂ
900932:陆家Ｂ股
900933:华新Ｂ股
900934:锦江Ｂ股
900935:阳晨Ｂ股
900936:鄂资Ｂ股
900937:华电Ｂ股
900938:天海B
900939:汇丽B
900940:大名城B
900941:东信Ｂ股
900942:黄山Ｂ股
900943:开开Ｂ股
900945:海航Ｂ股
900946:天雁B股
900947:振华Ｂ股
900948:伊泰Ｂ股
900951:大化B股
900952:锦港Ｂ股
900953:凯马Ｂ
900955:海创B股
900956:东贝Ｂ股
900957:凌云Ｂ股
939988:密码服务
990009:上证380
990017:新综指
990018:180金融
990019:治理指数
990020:中型综指
990021:180治理
990022:沪公司债
990025:180基建
990026:180资源
990027:180运输
990028:180成长
990029:180价值
990030:180R成长
990031:180R价值
990032:上证能源
990033:上证材料
990034:上证工业
990035:上证可选
990036:上证消费
990037:上证医药
990038:上证金融
990039:上证信息
990040:上证电信
990041:上证公用
990042:上证央企
990043:超大盘
990044:上证中盘
990045:上证小盘
990046:上证中小
990047:上证全指
990048:责任指数
990049:上证民企
990050:50等权
990051:180等权
990052:50基本
990053:180基本
990054:上证海外
990055:上证地企
990056:上证国企
990057:全指成长
990058:全指价值
990059:全R成长
990060:全R价值
990061:沪企债30
990062:上证沪企
990063:上证周期
990064:非周期
990065:上证龙头
990066:上证商品
990067:上证新兴
990068:上证资源
990069:消费80
990070:能源等权
990071:材料等权
990072:工业等权
990073:可选等权
990074:消费等权
990075:医药等权
990076:金融等权
990077:信息等权
990078:电信等权
990079:公用等权
990090:上证流通
990091:沪财中小
990092:资源50
990093:180分层
990094:上证上游
990095:上证中游
990096:上证下游
990097:高端装备
990098:上证F200
990099:上证F300
990100:上证F500
990101:5年信用
990102:沪投资品
990103:沪消费品
990104:380能源
990105:380材料
990106:380工业
990107:380可选
990108:380消费
990109:380医药
990110:380金融
990111:380信息
990112:380电信
990113:380公用
990114:持续产业
990115:380等权
990116:信用100
990117:380成长
990118:380价值
990119:380R成长
990120:380R价值
990121:医药主题
990122:农业主题
990123:180动态
990125:180稳定
990126:消费50
990128:380基本
990129:180波动
990130:380波动
990131:上证高新
990132:上证100
990133:上证150
990134:上证银行
990135:180高贝
990136:180低贝
990137:380高贝
990138:380低贝
990139:上证转债
990141:380动态
990142:380稳定
990145:优势资源
990146:优势制造
990147:优势消费
990148:消费领先
990149:180红利
990150:380红利
990151:上国红利
990152:上央红利
990153:上民红利
990155:市值百强
990158:上证环保
990159:沪股通
990160:沪新丝路
990161:沪中国造
990162:沪互联+
990802:500沪市
990805:A股资源
990806:消费服务
990807:食品饮料
990808:医药生物
990814:细分医药
990816:细分地产
990817:兴证海峡
990819:有色金属
990821:300红利
990823:800有色
990827:中证环保
990828:300高贝
990832:中证转债
990833:中高企债
990841:800医药
990842:800等权
990846:ESG 100
990847:腾讯济安
990849:300非银
990850:300有色
990851:百发100
990852:中证1000
990853:CSSW丝路
990854:500原料
990855:央视500
990856:500工业
990857:500医药
990858:500信息
990863:CS精准医
990901:小康指数
990903:中证100
990904:中证200
990905:中证500
990906:中证800
990908:300能源
990909:300材料
990910:300工业
990911:300可选
990912:300消费
990913:300医药
990914:300金融
990917:300公用
990918:300成长
990919:300价值
990922:中证红利
990925:基本面50
990928:中证能源
990931:中证可选
990932:中证消费
990933:中证医药
990934:中证金融
990935:中证信息
990939:民企200
990940:财富大盘
990944:内地资源
990950:300基建
990951:300银行
990952:300地产
990957:300运输
990958:创业成长
990959:银河99
990961:中证上游
990963:中证下游
990964:中证新兴
990966:基本400
990969:300非周
990971:等权90
990973:技术领先
990974:800金融
990975:钱江30
990976:新华金牛
990977:内地低碳
990978:医药100
990979:大宗商品
990982:500等权
990983:智能资产
990984:300等权
990986:全指能源
990987:全指材料
990989:全指可选
990990:全指消费
990991:全指医药
990992:全指金融
990993:全指信息
990996:领先行业
990997:大消费
990998:中证TMT
990999:两岸三地
999986:红利指数
999987:上证50
999988:企债指数
999989:国债指数
999990:基金指数
999991:上证180
999992:综合指数
999993:公用指数
999994:地产指数
999995:商业指数
999996:工业指数
999997:Ｂ股指数
999998:Ａ股指数
999999:上证指数
H00038:一拖股份
H00042:东北电器
H00107:四川成渝高速公路
H00168:青岛啤酒股份
H00177:江苏宁沪高速公路
H00187:北人印刷机械股份
H00300:昆明机床
H00317:广州广船国际股份
H00323:马鞍山钢铁股份
H00338:上海石油化工股份
H00347:鞍钢股份
H00350:经纬纺织机械股份
H00358:江西铜业股份
H00386:中国石油化工股份
H00390:中国中铁
H00525:广深铁路股份
H00548:深圳高速公路股份
H00553:南京熊猫电子股份
H00564:郑煤机
H00568:山东魔龙
H00588:北京北辰实业股份
H00670:中国东方航空股份
H00719:山东新华制药股份
H00753:中国国航
H00763:中兴通讯
H00857:中国石油股份
H00874:广州药业股份
H00895:东江环保
H00902:华能国际电力股份
H00914:安徽海螺水泥股份
H00921:海信科龙
H00939:建设银行
H00991:大唐发电
H00995:安徽皖通高速公路
H00998:中信银行
H01033:仪征化纤股份
H01053:重庆钢铁股份
H01055:中国南方航空股份
H01057:浙江世宝
H01065:天津创业环保股份
H01071:华电国际电力股份
H01072:东方电气
H01088:中国神华
H01108:洛阳玻璃股份
H01138:中海发展股份
H01157:中联重科
H01171:兖州煤业股份
H01186:中国铁建
H01211:比亚迪
H01288:农业银行
H01336:新华保险
H01398:工商银行
H01513:丽珠医药
H01618:中国中冶
H01766:中国南车
H01800:中国交建
H01812:晨鸣纸业
H01898:中煤能源
H01919:中国远洋
H01988:民生银行
H02009:金隅股份
H02039:中集集团
H02196:复星医药
H02202:万科企业
H02208:金风科技
H02238:广汽集团
H02318:中国平安
H02333:长城汽车
H02338:潍柴动力
H02600:中国铝业
H02601:中国太保
H02607:上海医药
H02628:中国人寿
H02727:上海电气
H02866:中海集运
H02880:大连港
H02883:中海油田服务
H02899:紫金矿业
H03328:交通银行
H03968:招商银行
H03988:中国银行
H03993:洛阳钼业
H06030:中信证券
H06199:中国北车
H06818:中国光大银行
H06837:海通证券
000001:平安银行
000002:万 科Ａ
000004:国农科技
000005:世纪星源
000006:深振业Ａ
000007:全新好
000008:神州高铁
000009:中国宝安
000010:美丽生态
000011:深物业A
000012:南 玻Ａ
000014:沙河股份
000016:深康佳Ａ
000017:深中华A
000018:神州长城
000019:深深宝Ａ
000020:深华发Ａ
000021:深科技
000022:深赤湾Ａ
000023:深天地Ａ
000025:特 力Ａ
000026:飞亚达Ａ
000027:深圳能源
000028:国药一致
000029:深深房Ａ
000030:富奥股份
000031:中粮地产
000032:深桑达Ａ
000034:神州数码
000035:中国天楹
000036:华联控股
000037:*ST南电A
000038:深大通
000039:中集集团
000040:东旭蓝天
000042:中洲控股
000043:中航地产
000045:深纺织Ａ
000046:泛海控股
000048:康达尔
000049:德赛电池
000050:深天马Ａ
000055:方大集团
000056:皇庭国际
000058:深 赛 格
000059:华锦股份
000060:中金岭南
000061:农 产 品
000062:深圳华强
000063:中兴通讯
000065:北方国际
000066:长城电脑
000068:华控赛格
000069:华侨城Ａ
000070:特发信息
000078:海王生物
000088:盐 田 港
000089:深圳机场
000090:天健集团
000096:广聚能源
000099:中信海直
000100:TCL 集团
000150:宜华健康
000151:中成股份
000153:丰原药业
000156:华数传媒
000157:中联重科
000158:常山股份
000159:国际实业
000166:申万宏源
000301:东方市场
000333:美的集团
000338:潍柴动力
000400:许继电气
000401:冀东水泥
000402:金 融 街
000403:ST生化
000404:华意压缩
000407:胜利股份
000408:*ST金源
000409:山东地矿
000410:沈阳机床
000411:英特集团
000413:东旭光电
000415:渤海金控
000416:民生控股
000417:合肥百货
000418:小天鹅Ａ
000419:通程控股
000420:吉林化纤
000421:南京公用
000422:湖北宜化
000423:东阿阿胶
000425:徐工机械
000426:兴业矿业
000428:华天酒店
000429:粤高速Ａ
000430:张家界
000488:晨鸣纸业
000498:山东路桥
000501:鄂武商Ａ
000502:绿景控股
000503:海虹控股
000504:*ST生物
000505:*ST珠江
000506:中润资源
000507:珠海港
000509:华塑控股
000510:金路集团
000511:*ST烯碳
000513:丽珠集团
000514:渝 开 发
000516:国际医学
000517:荣安地产
000518:四环生物
000519:江南红箭
000520:长航凤凰
000521:美菱电器
000523:广州浪奇
000524:岭南控股
000525:红 太 阳
000526:紫光学大
000528:柳 工
000529:广弘控股
000530:大冷股份
000531:穗恒运Ａ
000532:力合股份
000533:万 家 乐
000534:万泽股份
000536:华映科技
000537:广宇发展
000538:云南白药
000539:粤电力Ａ
000540:中天城投
000541:佛山照明
000543:皖能电力
000544:中原环保
000545:金浦钛业
000546:金圆股份
000547:航天发展
000548:湖南投资
000550:江铃汽车
000551:创元科技
000552:靖远煤电
000553:沙隆达Ａ
000554:泰山石油
000555:神州信息
000557:西部创业
000558:莱茵体育
000559:万向钱潮
000560:昆百大Ａ
000561:烽火电子
000563:陕国投Ａ
000564:西安民生
000565:渝三峡Ａ
000566:海南海药
000567:海德股份
000568:泸州老窖
000570:苏常柴Ａ
000571:新大洲Ａ
000572:海马汽车
000573:粤宏远Ａ
000576:广东甘化
000581:威孚高科
000582:北部湾港
000584:友利控股
000585:东北电气
000586:汇源通信
000587:金洲慈航
000589:黔轮胎Ａ
000590:启迪古汉
000591:太阳能
000592:平潭发展
000593:大通燃气
000595:宝塔实业
000596:古井贡酒
000597:东北制药
000598:兴蓉环境
000599:青岛双星
000600:建投能源
000601:韶能股份
000603:盛达矿业
000605:渤海股份
000606:*ST易桥
000607:华媒控股
000608:阳光股份
000609:绵石投资
000610:西安旅游
000611:*ST天首
000612:焦作万方
000613:大东海A
000615:京汉股份
000616:海航投资
000617:*ST济柴
000619:海螺型材
000620:新华联
000622:*ST恒立
000623:吉林敖东
000625:长安汽车
000626:如意集团
000627:天茂集团
000628:高新发展
000629:*ST钒钛
000630:铜陵有色
000631:顺发恒业
000632:三木集团
000633:*ST合金
000635:英 力 特
000636:风华高科
000637:茂化实华
000638:万方发展
000639:西王食品
000650:仁和药业
000651:格力电器
000652:泰达股份
000655:金岭矿业
000656:金科股份
000657:中钨高新
000659:珠海中富
000661:长春高新
000662:天夏智慧
000663:永安林业
000665:湖北广电
000666:经纬纺机
000667:美好集团
000668:荣丰控股
000669:金鸿能源
000670:*ST盈方
000671:阳 光 城
000672:上峰水泥
000673:当代东方
000676:智度投资
000677:恒天海龙
000678:襄阳轴承
000679:大连友谊
000680:山推股份
000681:视觉中国
000682:东方电子
000683:远兴能源
000685:中山公用
000686:东北证券
000687:华讯方舟
000688:建新矿业
000690:宝新能源
000691:ST亚太
000692:惠天热电
000693:ST华泽
000695:滨海能源
000697:炼石有色
000698:沈阳化工
000700:模塑科技
000701:厦门信达
000702:正虹科技
000703:恒逸石化
000705:浙江震元
000707:双环科技
000708:大冶特钢
000709:河钢股份
000710:天兴仪表
000711:京蓝科技
000712:锦龙股份
000713:丰乐种业
000715:中兴商业
000716:黑芝麻
000717:*ST韶钢
000718:苏宁环球
000719:大地传媒
000720:新能泰山
000721:西安饮食
000722:湖南发展
000723:美锦能源
000725:京东方Ａ
000726:鲁 泰Ａ
000727:华东科技
000728:国元证券
000729:燕京啤酒
000731:四川美丰
000732:泰禾集团
000733:振华科技
000735:罗 牛 山
000736:中房地产
000737:南风化工
000738:中航动控
000739:普洛药业
000748:长城信息
000750:国海证券
000751:锌业股份
000752:西藏发展
000753:漳州发展
000755:山西三维
000756:新华制药
000757:浩物股份
000758:中色股份
000759:中百集团
000760:斯太尔
000761:本钢板材
000762:西藏矿业
000766:通化金马
000767:漳泽电力
000768:中航飞机
000776:广发证券
000777:中核科技
000778:新兴铸管
000779:三毛派神
000780:平庄能源
000782:美达股份
000783:长江证券
000785:武汉中商
000786:北新建材
000788:北大医药
000789:万年青
000790:华神集团
000791:甘肃电投
000792:盐湖股份
000793:华闻传媒
000795:英洛华
000796:凯撒旅游
000797:中国武夷
000798:中水渔业
000799:酒鬼酒
000800:一汽轿车
000801:四川九洲
000802:北京文化
000803:金宇车城
000806:银河生物
000807:云铝股份
000809:铁岭新城
000810:创维数字
000811:烟台冰轮
000812:陕西金叶
000813:天山纺织
000815:美利纸业
000816:智慧农业
000818:方大化工
000819:岳阳兴长
000820:金城股份
000821:京山轻机
000822:山东海化
000823:超声电子
000825:太钢不锈
000826:启迪桑德
000828:东莞控股
000829:天音控股
000830:鲁西化工
000831:*ST五稀
000833:贵糖股份
000835:长城动漫
000836:鑫茂科技
000837:秦川机床
000838:财信发展
000839:中信国安
000848:承德露露
000850:华茂股份
000851:高鸿股份
000852:石化机械
000856:*ST冀装
000858:五 粮 液
000859:国风塑业
000860:顺鑫农业
000861:海印股份
000862:银星能源
000863:三湘股份
000868:安凯客车
000869:张 裕Ａ
000875:吉电股份
000876:新 希 望
000877:天山股份
000878:云南铜业
000880:潍柴重机
000881:大连国际
000882:华联股份
000883:湖北能源
000885:同力水泥
000886:海南高速
000887:中鼎股份
000888:峨眉山Ａ
000889:茂业通信
000890:法 尔 胜
000892:星美联合
000893:东凌国际
000895:双汇发展
000897:津滨发展
000898:鞍钢股份
000899:赣能股份
000900:现代投资
000901:航天科技
000902:新洋丰
000903:云内动力
000905:厦门港务
000906:物产中拓
000908:景峰医药
000909:数源科技
000910:大亚科技
000911:南宁糖业
000912:泸天化
000913:*ST钱江
000915:山大华特
000916:华北高速
000917:电广传媒
000918:嘉凯城
000919:金陵药业
000920:南方汇通
000921:海信科龙
000922:佳电股份
000923:河北宣工
000925:众合科技
000926:福星股份
000927:一汽夏利
000928:中钢国际
000929:兰州黄河
000930:中粮生化
000931:中 关 村
000932:华菱钢铁
000933:*ST神火
000935:四川双马
000936:华西股份
000937:冀中能源
000938:紫光股份
000939:凯迪生态
000948:南天信息
000949:新乡化纤
000950:*ST建峰
000951:中国重汽
000952:广济药业
000953:河池化工
000955:欣龙控股
000957:中通客车
000958:东方能源
000959:首钢股份
000960:锡业股份
000961:中南建设
000962:*ST东钽
000963:华东医药
000965:天保基建
000966:长源电力
000967:盈峰环境
000968:*ST煤气
000969:安泰科技
000970:中科三环
000971:高升控股
000972:中基健康
000973:佛塑科技
000975:银泰资源
000976:春晖股份
000977:浪潮信息
000978:桂林旅游
000979:中弘股份
000980:金马股份
000981:银亿股份
000982:中银绒业
000983:西山煤电
000985:大庆华科
000987:广州友谊
000988:华工科技
000989:九 芝 堂
000990:诚志股份
000993:闽东电力
000995:*ST皇台
000996:中国中期
000997:新 大 陆
000998:隆平高科
000999:华润三九
001696:宗申动力
001896:豫能控股
001979:招商蛇口
002001:新 和 成
002002:鸿达兴业
002003:伟星股份
002004:华邦健康
002005:德豪润达
002006:精功科技
002007:华兰生物
002008:大族激光
002009:天奇股份
002010:传化股份
002011:盾安环境
002012:凯恩股份
002013:中航机电
002014:永新股份
002015:霞客环保
002016:世荣兆业
002017:东信和平
002018:华信国际
002019:亿帆鑫富
002020:京新药业
002021:中捷资源
002022:科华生物
002023:海特高新
002024:苏宁云商
002025:航天电器
002026:山东威达
002027:分众传媒
002028:思源电气
002029:七 匹 狼
002030:达安基因
002031:巨轮智能
002032:苏 泊 尔
002033:丽江旅游
002034:美 欣 达
002035:华帝股份
002036:联创电子
002037:久联发展
002038:双鹭药业
002039:黔源电力
002040:南 京 港
002041:登海种业
002042:华孚色纺
002043:兔 宝 宝
002044:美年健康
002045:国光电器
002046:轴研科技
002047:宝鹰股份
002048:宁波华翔
002049:紫光国芯
002050:三花股份
002051:中工国际
002052:同洲电子
002053:云南盐化
002054:德美化工
002055:得润电子
002056:横店东磁
002057:中钢天源
002058:威 尔 泰
002059:云南旅游
002060:粤 水 电
002061:*ST江化
002062:宏润建设
002063:远光软件
002064:华峰氨纶
002065:东华软件
002066:瑞泰科技
002067:景兴纸业
002068:黑猫股份
002069:*ST獐岛
002070:众和股份
002071:长城影视
002072:凯瑞德
002073:软控股份
002074:国轩高科
002075:沙钢股份
002076:雪 莱 特
002077:大港股份
002078:太阳纸业
002079:苏州固锝
002080:中材科技
002081:金 螳 螂
002082:栋梁新材
002083:孚日股份
002084:海鸥卫浴
002085:万丰奥威
002086:东方海洋
002087:新野纺织
002088:鲁阳节能
002089:新 海 宜
002090:金智科技
002091:江苏国泰
002092:中泰化学
002093:国脉科技
002094:青岛金王
002095:生 意 宝
002096:南岭民爆
002097:山河智能
002098:浔兴股份
002099:海翔药业
002100:天康生物
002101:广东鸿图
002102:冠福股份
002103:广博股份
002104:恒宝股份
002105:信隆实业
002106:莱宝高科
002107:沃华医药
002108:沧州明珠
002109:*ST兴化
002110:三钢闽光
002111:威海广泰
002112:三变科技
002113:天润数娱
002114:罗平锌电
002115:三维通信
002116:中国海诚
002117:东港股份
002118:紫鑫药业
002119:康强电子
002120:新海股份
002121:科陆电子
002122:天马股份
002123:荣信股份
002124:天邦股份
002125:湘潭电化
002126:银轮股份
002127:南极电商
002128:露天煤业
002129:中环股份
002130:沃尔核材
002131:利欧股份
002132:恒星科技
002133:广宇集团
002134:天津普林
002135:东南网架
002136:安 纳 达
002137:麦达数字
002138:顺络电子
002139:拓邦股份
002140:东华科技
002141:蓉胜超微
002142:宁波银行
002143:印纪传媒
002144:宏达高科
002145:中核钛白
002146:荣盛发展
002147:新光圆成
002148:北纬通信
002149:西部材料
002150:通润装备
002151:北斗星通
002152:广电运通
002153:石基信息
002154:报 喜 鸟
002155:湖南黄金
002156:通富微电
002157:正邦科技
002158:汉钟精机
002159:三特索道
002160:常铝股份
002161:远 望 谷
002162:悦心健康
002163:中航三鑫
002164:宁波东力
002165:红 宝 丽
002166:莱茵生物
002167:东方锆业
002168:深圳惠程
002169:智光电气
002170:芭田股份
002171:楚江新材
002172:澳洋科技
002173:*ST创疗
002174:游族网络
002175:东方网络
002176:江特电机
002177:御银股份
002178:延华智能
002179:中航光电
002180:艾派克
002181:粤 传 媒
002182:云海金属
002183:怡 亚 通
002184:海得控制
002185:华天科技
002186:全 聚 德
002187:广百股份
002188:巴士在线
002189:利达光电
002190:成飞集成
002191:劲嘉股份
002192:融捷股份
002193:山东如意
002194:武汉凡谷
002195:二三四五
002196:方正电机
002197:证通电子
002198:嘉应制药
002199:*ST东晶
002200:云投生态
002201:九鼎新材
002202:金风科技
002203:海亮股份
002204:大连重工
002205:国统股份
002206:海 利 得
002207:准油股份
002208:合肥城建
002209:达 意 隆
002210:飞马国际
002211:宏达新材
002212:南洋股份
002213:特 尔 佳
002214:大立科技
002215:诺 普 信
002216:三全食品
002217:合力泰
002218:拓日新能
002219:恒康医疗
002220:天宝股份
002221:东华能源
002222:福晶科技
002223:鱼跃医疗
002224:三 力 士
002225:濮耐股份
002226:江南化工
002227:奥 特 迅
002228:合兴包装
002229:鸿博股份
002230:科大讯飞
002231:奥维通信
002232:启明信息
002233:塔牌集团
002234:民和股份
002235:安妮股份
002236:大华股份
002237:恒邦股份
002238:天威视讯
002239:奥特佳
002240:威华股份
002241:歌尔股份
002242:九阳股份
002243:通产丽星
002244:滨江集团
002245:澳洋顺昌
002246:北化股份
002247:帝龙新材
002248:华东数控
002249:大洋电机
002250:联化科技
002251:步 步 高
002252:上海莱士
002253:川大智胜
002254:泰和新材
002255:海陆重工
002256:彩虹精化
002258:利尔化学
002259:升达林业
002260:德奥通航
002261:拓维信息
002262:恩华药业
002263:大 东 南
002264:新 华 都
002265:西仪股份
002266:浙富控股
002267:陕天然气
002268:卫 士 通
002269:美邦服饰
002270:华明装备
002271:东方雨虹
002272:川润股份
002273:水晶光电
002274:华昌化工
002275:桂林三金
002276:万马股份
002277:友阿股份
002278:神开股份
002279:久其软件
002280:联络互动
002281:光迅科技
002282:博深工具
002283:天润曲轴
002284:亚太股份
002285:世联行
002286:保龄宝
002287:奇正藏药
002288:超华科技
002289:*ST宇顺
002290:禾盛新材
002291:星期六
002292:奥飞娱乐
002293:罗莱生活
002294:信立泰
002295:精艺股份
002296:辉煌科技
002297:博云新材
002298:中电鑫龙
002299:圣农发展
002300:太阳电缆
002301:齐心集团
002302:西部建设
002303:美盈森
002304:洋河股份
002305:南国置业
002306:中科云网
002307:北新路桥
002308:威创股份
002309:中利科技
002310:东方园林
002311:海大集团
002312:三泰控股
002313:日海通讯
002314:南山控股
002315:焦点科技
002316:键桥通讯
002317:众生药业
002318:久立特材
002319:乐通股份
002320:海峡股份
002321:华英农业
002322:理工环科
002323:雅百特
002324:普利特
002325:洪涛股份
002326:永太科技
002327:富安娜
002328:新朋股份
002329:皇氏集团
002330:得利斯
002331:皖通科技
002332:仙琚制药
002333:罗普斯金
002334:英威腾
002335:科华恒盛
002336:*ST人乐
002337:赛象科技
002338:奥普光电
002339:积成电子
002340:格林美
002341:新纶科技
002342:巨力索具
002343:慈文传媒
002344:海宁皮城
002345:潮宏基
002346:柘中股份
002347:泰尔重工
002348:高乐股份
002349:精华制药
002350:北京科锐
002351:漫步者
002352:鼎泰新材
002353:杰瑞股份
002354:天神娱乐
002355:兴民钢圈
002356:赫美集团
002357:富临运业
002358:森源电气
002359:齐星铁塔
002360:同德化工
002361:神剑股份
002362:汉王科技
002363:隆基机械
002364:中恒电气
002365:永安药业
002366:台海核电
002367:康力电梯
002368:太极股份
002369:卓翼科技
002370:亚太药业
002371:七星电子
002372:伟星新材
002373:千方科技
002374:丽鹏股份
002375:亚厦股份
002376:新北洋
002377:国创高新
002378:章源钨业
002379:*ST鲁丰
002380:科远股份
002381:双箭股份
002382:蓝帆医疗
002383:合众思壮
002384:东山精密
002385:大北农
002386:天原集团
002387:黑牛食品
002388:新亚制程
002389:南洋科技
002390:信邦制药
002391:长青股份
002392:北京利尔
002393:力生制药
002394:联发股份
002395:双象股份
002396:星网锐捷
002397:梦洁股份
002398:建研集团
002399:海普瑞
002400:省广股份
002401:中海科技
002402:和而泰
002403:爱仕达
002404:嘉欣丝绸
002405:四维图新
002406:远东传动
002407:多氟多
002408:齐翔腾达
002409:雅克科技
002410:广联达
002411:必康股份
002412:汉森制药
002413:雷科防务
002414:高德红外
002415:海康威视
002416:爱施德
002417:三元达
002418:康盛股份
002419:天虹商场
002420:毅昌股份
002421:达实智能
002422:科伦药业
002423:*ST中特
002424:贵州百灵
002425:凯撒股份
002426:胜利精密
002427:尤夫股份
002428:云南锗业
002429:兆驰股份
002430:杭氧股份
002431:棕榈股份
002432:九安医疗
002433:太安堂
002434:万里扬
002435:长江润发
002436:兴森科技
002437:誉衡药业
002438:江苏神通
002439:启明星辰
002440:闰土股份
002441:众业达
002442:龙星化工
002443:金洲管道
002444:巨星科技
002445:中南文化
002446:盛路通信
002447:壹桥海参
002448:中原内配
002449:国星光电
002450:康得新
002451:摩恩电气
002452:长高集团
002453:天马精化
002454:松芝股份
002455:百川股份
002456:欧菲光
002457:青龙管业
002458:益生股份
002459:天业通联
002460:赣锋锂业
002461:珠江啤酒
002462:嘉事堂
002463:沪电股份
002464:金利科技
002465:海格通信
002466:天齐锂业
002467:二六三
002468:艾迪西
002469:三维工程
002470:金正大
002471:中超控股
002472:双环传动
002473:圣莱达
002474:榕基软件
002475:立讯精密
002476:宝莫股份
002477:雏鹰农牧
002478:常宝股份
002479:富春环保
002480:新筑股份
002481:双塔食品
002482:广田集团
002483:润邦股份
002484:江海股份
002485:希努尔
002486:嘉麟杰
002487:大金重工
002488:金固股份
002489:浙江永强
002490:山东墨龙
002491:通鼎互联
002492:恒基达鑫
002493:荣盛石化
002494:华斯股份
002495:佳隆股份
002496:辉丰股份
002497:雅化集团
002498:汉缆股份
002499:科林环保
002500:山西证券
002501:利源精制
002502:骅威文化
002503:搜于特
002504:弘高创意
002505:大康农业
002506:协鑫集成
002507:涪陵榨菜
002508:老板电器
002509:天广消防
002510:天汽模
002511:中顺洁柔
002512:达华智能
002513:*ST蓝丰
002514:宝馨科技
002515:金字火腿
002516:旷达科技
002517:恺英网络
002518:科士达
002519:银河电子
002520:日发精机
002521:齐峰新材
002522:浙江众成
002523:天桥起重
002524:光正集团
002526:山东矿机
002527:新时达
002528:英飞拓
002529:海源机械
002530:丰东股份
002531:天顺风能
002532:新界泵业
002533:金杯电工
002534:杭锅股份
002535:林州重机
002536:西泵股份
002537:海立美达
002538:司尔特
002539:新都化工
002540:亚太科技
002541:鸿路钢构
002542:中化岩土
002543:万和电气
002544:杰赛科技
002545:东方铁塔
002546:新联电子
002547:春兴精工
002548:金新农
002549:凯美特气
002550:千红制药
002551:尚荣医疗
002552:宝鼎科技
002553:南方轴承
002554:惠博普
002555:三七互娱
002556:辉隆股份
002557:洽洽食品
002558:世纪游轮
002559:亚威股份
002560:通达股份
002561:徐家汇
002562:兄弟科技
002563:森马服饰
002564:天沃科技
002565:上海绿新
002566:益盛药业
002567:唐人神
002568:百润股份
002569:步森股份
002570:贝因美
002571:德力股份
002572:索菲亚
002573:清新环境
002574:明牌珠宝
002575:群兴玩具
002576:通达动力
002577:雷柏科技
002578:闽发铝业
002579:中京电子
002580:圣阳股份
002581:未名医药
002582:好想你
002583:海能达
002584:西陇科学
002585:双星新材
002586:围海股份
002587:奥拓电子
002588:史丹利
002589:瑞康医药
002590:万安科技
002591:恒大高新
002592:八菱科技
002593:日上集团
002594:比亚迪
002595:豪迈科技
002596:海南瑞泽
002597:金禾实业
002598:山东章鼓
002599:盛通股份
002600:江粉磁材
002601:佰利联
002602:世纪华通
002603:以岭药业
002604:龙力生物
002605:姚记扑克
002606:大连电瓷
002607:亚夏汽车
002608:*ST舜船
002609:捷顺科技
002610:爱康科技
002611:东方精工
002612:朗姿股份
002613:北玻股份
002614:蒙发利
002615:哈尔斯
002616:长青集团
002617:露笑科技
002618:丹邦科技
002619:巨龙管业
002620:瑞和股份
002621:三垒股份
002622:永大集团
002623:亚玛顿
002624:完美环球
002625:龙生股份
002626:金达威
002627:宜昌交运
002628:成都路桥
002629:仁智油服
002630:华西能源
002631:德尔未来
002632:道明光学
002633:申科股份
002634:棒杰股份
002635:安洁科技
002636:金安国纪
002637:赞宇科技
002638:勤上光电
002639:雪人股份
002640:跨境通
002641:永高股份
002642:荣之联
002643:万润股份
002644:佛慈制药
002645:华宏科技
002646:青青稞酒
002647:宏磊股份
002648:卫星石化
002649:博彦科技
002650:加加食品
002651:利君股份
002652:扬子新材
002653:海思科
002654:万润科技
002655:共达电声
002656:摩登大道
002657:中科金财
002658:雪迪龙
002659:中泰桥梁
002660:茂硕电源
002661:克明面业
002662:京威股份
002663:普邦园林
002664:信质电机
002665:首航节能
002666:德联集团
002667:鞍重股份
002668:奥马电器
002669:康达新材
002670:华声股份
002671:龙泉股份
002672:东江环保
002673:西部证券
002674:兴业科技
002675:东诚药业
002676:顺威股份
002677:浙江美大
002678:珠江钢琴
002679:福建金森
002680:长生生物
002681:奋达科技
002682:龙洲股份
002683:宏大爆破
002684:猛狮科技
002685:华东重机
002686:亿利达
002687:乔治白
002688:金河生物
002689:远大智能
002690:美亚光电
002691:冀凯股份
002692:远程电缆
002693:双成药业
002694:顾地科技
002695:煌上煌
002696:百洋股份
002697:红旗连锁
002698:博实股份
002699:美盛文化
002700:新疆浩源
002701:奥瑞金
002702:海欣食品
002703:浙江世宝
002705:新宝股份
002706:良信电器
002707:众信旅游
002708:光洋股份
002709:天赐材料
002711:欧浦智网
002712:思美传媒
002713:东易日盛
002714:牧原股份
002715:登云股份
002716:金贵银业
002717:岭南园林
002718:友邦吊顶
002719:麦趣尔
002721:金一文化
002722:金轮股份
002723:金莱特
002724:海洋王
002725:跃岭股份
002726:龙大肉食
002727:一心堂
002728:台城制药
002729:好利来
002730:电光科技
002731:萃华珠宝
002732:燕塘乳业
002733:雄韬股份
002734:利民股份
002735:王子新材
002736:国信证券
002737:葵花药业
002738:中矿资源
002739:万达院线
002740:爱迪尔
002741:光华科技
002742:三圣特材
002743:富煌钢构
002745:木林森
002746:仙坛股份
002747:埃斯顿
002748:世龙实业
002749:国光股份
002750:龙津药业
002751:易尚展示
002752:昇兴股份
002753:永东股份
002755:东方新星
002756:永兴特钢
002757:南兴装备
002758:华通医药
002759:天际股份
002760:凤形股份
002761:多喜爱
002762:金发拉比
002763:汇洁股份
002765:蓝黛传动
002766:索菱股份
002767:先锋电子
002768:国恩股份
002769:普路通
002770:科迪乳业
002771:真视通
002772:众兴菌业
002773:康弘药业
002775:文科园林
002776:柏堡龙
002777:久远银海
002778:高科石化
002779:中坚科技
002780:三夫户外
002781:奇信股份
002782:可立克
002783:凯龙股份
002785:万里石
002786:银宝山新
002787:华源包装
002788:鹭燕医药
002789:建艺集团
002790:瑞尔特
002791:坚朗五金
002792:通宇通讯
002793:东音股份
002795:永和智控
002796:世嘉科技
002797:第一创业
002798:帝王洁具
002799:环球印务
002800:天顺股份
002801:微光股份
002802:洪汇新材
002803:吉宏股份
002805:丰元股份
100213:国债0213
100303:国债0303
100504:国债0504
100512:国债0512
100609:国债0609
100616:国债0616
100619:国债0619
100703:国债0703
100706:国债0706
100710:国债0710
100713:国债0713
100802:国债0802
100803:国债0803
100806:国债0806
100810:国债0810
100813:国债0813
100818:国债0818
100820:国债0820
100823:国债0823
100825:国债0825
100902:国债0902
100903:国债0903
100905:国债0905
100907:国债0907
100911:国债0911
100912:国债0912
100916:国债0916
100917:国债0917
100919:国债0919
100920:国债0920
100923:国债0923
100925:国债0925
100926:国债0926
100927:国债0927
100930:国债0930
100932:国债0932
101002:国债1002
101003:国债1003
101005:国债1005
101007:国债1007
101009:国债1009
101010:国债1010
101012:国债1012
101014:国债1014
101015:国债1015
101018:国债1018
101019:国债1019
101022:国债1022
101023:国债1023
101024:国债1024
101026:国债1026
101027:国债1027
101029:国债1029
101031:国债1031
101032:国债1032
101034:国债1034
101037:国债1037
101038:国债1038
101040:国债1040
101041:国债1041
101102:国债1102
101103:国债1103
101105:国债1105
101106:国债1106
101108:国债1108
101110:国债1110
101112:国债1112
101115:国债1115
101116:国债1116
101117:国债1117
101119:国债1119
101121:国债1121
101122:国债1122
101123:国债1123
101124:国债1124
101203:国债1203
101204:国债1204
101205:国债1205
101206:国债1206
101208:国债1208
101209:国债1209
101210:国债1210
101212:国债1212
101213:国债1213
101214:国债1214
101215:国债1215
101216:国债1216
101218:国债1218
101220:国债1220
101221:国债1221
101301:国债1301
101303:国债1303
101305:国债1305
101308:国债1308
101309:国债1309
101310:国债1310
101311:国债1311
101313:国债1313
101315:国债1315
101316:国债1316
101317:国债1317
101318:国债1318
101319:国债1319
101320:国债1320
101323:国债1323
101324:国债1324
101325:国债1325
101401:国债1401
101403:国债1403
101404:国债1404
101405:国债1405
101406:国债1406
101408:国债1408
101409:国债1409
101410:国债1410
101412:国债1412
101413:国债1413
101416:国债1416
101417:国债1417
101419:国债1419
101420:国债1420
101421:国债1421
101424:国债1424
101425:国债1425
101426:国债1426
101427:国债1427
101429:国债1429
101430:国债1430
101502:国债1502
101503:国债1503
101504:国债1504
101505:国债1505
101507:国债1507
101508:国债1508
101510:国债1510
101511:国债1511
101512:国债1512
101513:国债1513
101514:国债1514
101516:国债1516
101517:国债1517
101518:国债1518
101519:国债1519
101520:国债1520
101521:国债1521
101522:国债1522
101523:国债1523
101524:国债1524
101525:国债1525
101526:国债1526
101527:国债1527
101528:国债1528
101601:国债1601
101602:国债1602
101603:国债1603
101604:国债1604
101605:国债1605
101606:国债1606
101607:国债1607
101608:国债1608
101609:国债1609
101610:国债1610
101611:国债1611
101612:国债1612
101613:国债1613
101614:国债1614
101615:国债1615
101917:国债917
107001:青海1608
107002:青海16Z1
107003:青海16Z2
107004:青海16Z3
107005:青海16Z4
107006:内蒙1605
107007:内蒙1606
107008:内蒙1607
107009:内蒙1608
107010:河南1605
107011:河南1606
107012:河南1607
107013:河南1608
107014:河南16Z1
107015:河南16Z2
107016:河南16Z3
107017:河南16Z4
107018:天津1603
107019:天津1604
107020:天津1605
107021:天津1606
107022:天津16Z4
107023:天津16Z5
107024:天津16Z6
107025:河北1605
107026:河北1606
107027:河北1607
107028:河北1608
107029:河北16Z5
107030:河北16Z6
107031:河北16Z7
107032:贵州1605
107033:贵州1606
107034:贵州1607
107035:贵州1608
107036:湖北16Z3
107037:湖北16Z4
107038:湖北1609
107039:湖北1610
107040:湖北1611
107041:湖北1612
107042:甘肃1604
107043:山东16Z5
107044:山东16Z6
107045:山东16Z7
107046:山东16Z8
107047:山东16Z9
107048:山东1609
107049:山东1610
107050:山东1611
107051:重庆16Z3
107052:重庆16Z4
107053:重庆16Z5
107054:重庆16Z6
107055:重庆1605
107056:重庆1606
107057:重庆1607
107058:重庆1608
107059:江苏1605
107060:江苏1606
107061:江苏1607
107062:江苏1608
107063:江苏16Z5
107064:江苏16Z6
107065:江苏16Z7
107066:江苏16Z8
107067:浙江16Z1
107068:浙江16Z2
107069:浙江1605
107070:浙江1606
107071:浙江1607
107072:浙江1608
107073:广西1609
107074:广西1610
107075:广西1611
107076:广西1612
107077:广西16Z3
107078:广西16Z4
107079:广西16Z5
107080:新疆1609
107081:新疆1610
107082:新疆1611
107083:新疆1612
107084:广东16Z7
107085:广东16Z8
107086:广东16Z9
107087:广东1609
107088:广东1610
107089:广东1611
107090:广东1612
107091:宁夏16Z5
107092:宁夏16Z6
107093:宁夏16Z7
107094:宁夏1605
107095:宁夏1606
107096:宁夏1607
107097:宁夏1608
107098:福建16Z1
107099:福建16Z2
107100:福建1601
107101:福建1602
107102:福建1603
107103:福建1604
107104:四川1609
107106:四川1611
107107:四川1612
107108:四川16Z5
107110:四川16Z7
107111:四川16Z8
107112:吉林1601
107113:吉林1602
107114:吉林1603
107131:内蒙16Z1
107132:内蒙16Z2
107133:内蒙16Z3
107134:内蒙16Z4
107135:湖南16Z1
107136:湖南16Z2
107137:山西1605
107138:山西1606
107139:山西1607
107140:山西1608
107141:山西16Z1
107142:山西16Z2
107143:河南1609
107144:河南1610
107145:河南1611
107146:河南1612
107147:河南16Z5
107148:河南16Z7
107149:河南16Z6
107150:河南16Z8
107151:安徽1605
107152:安徽1606
107153:安徽1607
107154:安徽1608
107155:安徽16Z1
107156:安徽16Z2
107157:北京1601
107158:北京1602
107159:青海16Z5
107160:青海16Z6
107161:青海16Z7
107162:青海16Z8
107163:辽宁1605
107164:辽宁1606
107165:辽宁1607
107166:辽宁1608
107167:新疆16Z5
107168:新疆16Z6
107169:新疆16Z7
107170:新疆16Z8
107171:新疆1613
107172:新疆1614
107173:新疆1615
107174:新疆1616
107175:广东1613
107176:广东1614
107177:广东1615
107178:广东1616
107179:广东1617
107180:广东1618
107181:广东1619
107182:贵州1609
107183:贵州1610
107184:贵州1611
107185:贵州1612
107186:贵州16Z5
107187:贵州16Z6
108104:贴债1607
108108:贴债1611
108114:贴债1617
108117:贴债1619
108118:贴债1620
108119:贴债1622
108120:贴债1621
108121:贴债1623
108122:贴债1624
108123:贴债1625
108124:贴债1627
108125:贴债1626
108126:贴债1628
108127:贴债1629
108128:贴债1630
108129:贴债1632
108130:贴债1631
109064:地债1104
109066:地债1106
109068:地债1108
109070:上海1102
109072:广东1102
109074:浙江1102
109076:深圳1102
109078:地债1202
109080:地债1204
109082:地债1206
109084:地债1208
109085:上海1201
109086:上海1202
109087:广东1201
109088:广东1202
109090:地债1210
109091:浙江1201
109092:浙江1202
109093:深圳1201
109094:深圳1202
109096:地债1302
109098:地债1304
109099:地债1305
109100:地债1306
109101:地债1307
109102:地债1308
109103:山东1301
109104:山东1302
109105:地债1309
109106:上海1301
109107:上海1302
109108:地债1310
109109:广东1301
109110:广东1302
109111:江苏1301
109112:江苏1302
109113:地债1311
109114:地债1312
109115:浙江1301
109116:浙江1302
109117:深圳1301
109118:深圳1302
109119:地债1401
109120:地债1402
109121:广东1401
109122:广东1402
109123:广东1403
109124:地债1403
109125:地债1404
109126:地债1405
109127:山东1401
109128:山东1402
109129:山东1403
109130:地债1406
109131:地债1407
109132:地债1408
109133:江苏1401
109134:江苏1402
109135:江苏1403
109136:江西1401
109137:江西1402
109138:江西1403
109139:宁夏1401
109140:宁夏1402
109141:宁夏1403
109142:地债1409
109143:地债1410
109144:青岛1401
109145:青岛1402
109146:青岛1403
109147:浙江1401
109148:浙江1402
109149:浙江1403
109150:北京1401
109151:北京1402
109152:北京1403
109153:上海1401
109154:上海1402
109155:上海1403
109156:地债1411
109157:地债1412
109158:地债1413
109159:深圳1401
109160:深圳1402
109161:深圳1403
109162:江苏1501
109163:江苏1502
109164:江苏1503
109165:江苏1504
109166:新疆1501
109167:新疆1502
109168:新疆1503
109169:新疆1504
109170:湖北1501
109171:湖北1502
109172:湖北1503
109173:湖北1504
109174:广西1501
109175:广西1502
109176:广西1503
109177:广西1504
109178:山东1501
109179:山东1502
109180:山东1503
109181:山东1504
109182:重庆1501
109183:重庆1502
109184:重庆1503
109185:重庆1504
109186:贵州1501
109187:贵州1502
109188:贵州1503
109189:贵州1504
109190:安徽1501
109191:安徽1502
109192:安徽1503
109193:安徽1504
109194:天津1501
109195:天津1502
109196:天津1503
109197:天津1504
109198:湖北1505
109199:湖北1506
109200:湖北1507
109201:湖北1508
109202:浙江1501
109203:浙江1502
109204:浙江1503
109205:浙江1504
109206:河北1501
109207:河北1502
109208:河北1503
109209:河北1504
109210:吉林1501
109211:吉林1502
109212:吉林1503
109213:吉林1504
109214:山西1501
109215:山西1502
109216:山西1503
109217:山西1504
109218:广东1501
109219:广东1502
109220:广东1503
109221:广东1504
109222:江西1501
109223:江西1502
109224:江西1503
109225:江西1504
109226:宁夏1501
109227:宁夏1502
109228:宁夏1503
109229:宁夏1504
109230:新疆1505
109231:新疆1506
109232:新疆1507
109233:新疆1508
109234:四川1501
109235:四川1502
109236:四川1503
109237:四川1504
109238:河北15Z1
109239:河北15Z2
109240:河北15Z3
109241:河南1501
109242:河南1502
109243:河南1503
109244:河南1504
109245:辽宁1501
109246:辽宁1502
109247:辽宁1503
109248:辽宁1504
109249:云南1501
109250:云南1502
109251:云南1503
109252:云南1504
109253:海南1501
109254:海南1502
109255:海南1503
109256:海南1504
109257:青岛1501
109258:青岛1502
109259:青岛1503
109260:青岛1504
109261:江苏15Z1
109262:江苏15Z2
109263:江苏15Z3
109264:山东1505
109265:山东1506
109266:山东1507
109267:山东1508
109268:陕西1501
109269:陕西1502
109270:陕西1503
109271:陕西1504
109272:大连1501
109273:大连1502
109274:大连1503
109275:大连1504
109276:大连15Z1
109277:大连15Z2
109278:大连15Z3
109279:大连15Z4
109280:贵州1505
109281:贵州1506
109282:贵州1507
109283:贵州1508
109284:内蒙1501
109285:内蒙1502
109286:内蒙1503
109287:内蒙1504
109288:新疆15Z1
109289:新疆15Z2
109290:新疆15Z3
109291:新疆15Z4
109292:四川1505
109293:四川1506
109294:四川1507
109295:四川1508
109296:北京1501
109297:北京1502
109298:北京1503
109299:北京1504
109300:甘肃1501
109301:甘肃1502
109302:甘肃1503
109303:甘肃1504
109304:青海1501
109305:青海1502
109306:青海1503
109307:青海1504
109308:广东15Z1
109309:广东15Z2
109310:广东15Z3
109311:宁波1501
109312:宁波1502
109313:宁波1503
109314:宁波1504
109315:宁波15Z1
109316:宁波15Z2
109317:宁波15Z3
109318:宁波15Z4
109319:福建1501
109320:福建1502
109321:福建1503
109322:福建1504
109323:湖南1501
109324:湖南1502
109325:湖南1503
109326:湖南1504
109327:广西15Z1
109328:广西15Z2
109329:广西1505
109330:广西1506
109331:广西1507
109332:广西1508
109333:湖北15Z1
109334:湖北15Z2
109335:湖北15Z3
109336:湖北15Z4
109337:湖北1509
109338:湖北1510
109339:湖北1511
109340:湖北1512
109341:广东1505
109342:广东1506
109343:广东1507
109344:广东1508
109345:山东15Z1
109346:山东15Z2
109347:山东15Z3
109348:福建15Z1
109349:福建15Z2
109350:福建1505
109351:福建1506
109352:福建1507
109353:福建1508
109354:云南15Z1
109355:云南15Z2
109356:云南15Z3
109357:云南15Z4
109358:龙江15Z1
109359:龙江15Z2
109360:龙江15Z3
109361:龙江1501
109362:龙江1502
109363:龙江1503
109364:龙江1504
109365:重庆15Z1
109366:重庆15Z2
109367:重庆1505
109368:重庆1506
109369:重庆1507
109370:重庆1508
109371:新疆15Z5
109372:新疆15Z6
109373:新疆15Z7
109374:新疆15Z8
109375:新疆1509
109376:新疆1510
109377:新疆1511
109378:新疆1512
109379:上海15Z1
109380:上海1503
109381:上海1501
109382:上海1502
109383:上海1504
109384:上海15Z2
109385:辽宁15Z1
109386:辽宁15Z2
109387:辽宁1505
109388:辽宁1506
109389:辽宁1507
109390:辽宁1508
109391:青岛1505
109392:青岛1506
109393:青岛1507
109394:青岛1508
109395:青岛15Z1
109396:青岛15Z2
109397:青岛15Z3
109398:天津15Z1
109399:天津15Z2
109400:天津15Z3
109401:天津1505
109402:天津1506
109403:天津1507
109404:天津1508
109405:甘肃1505
109406:甘肃1506
109407:甘肃1507
109408:甘肃1508
109409:甘肃15Z2
109410:甘肃15Z1
109411:安徽1505
109412:安徽1506
109413:安徽1507
109414:安徽1508
109415:安徽1509
109416:安徽15Z1
109417:安徽15Z2
109418:厦门1501
109419:厦门1502
109420:厦门1503
109421:厦门1504
109422:厦门15Z1
109423:厦门15Z2
109424:青海1505
109425:青海1506
109426:青海1507
109427:青海1508
109428:青海15Z1
109429:青海15Z2
109430:青海15Z3
109431:青海15Z4
109432:北京15Z1
109433:北京15Z2
109434:北京15Z3
109435:北京15Z4
109436:陕西1505
109437:陕西1506
109438:陕西1507
109439:陕西1508
109440:陕西15Z1
109441:陕西15Z2
109442:陕西15Z3
109443:陕西15Z4
109444:陕西15Z5
109445:陕西15Z6
109446:陕西15Z7
109447:陕西15Z8
109448:河南15Z1
109449:河南15Z2
109450:河南15Z3
109451:河南15Z4
109452:河南1505
109453:河南1506
109454:河南1507
109455:河南1508
109456:内蒙1505
109457:内蒙1506
109458:内蒙1507
109459:内蒙1508
109460:内蒙15Z1
109461:内蒙15Z2
109462:内蒙15Z3
109463:内蒙15Z4
109464:宁夏1505
109465:宁夏1506
109466:宁夏1507
109467:宁夏1508
109468:江苏1505
109469:江苏1506
109470:江苏1507
109471:江苏1508
109472:江苏15Z4
109473:江苏15Z5
109474:江苏15Z6
109475:江苏15Z7
109476:山东15Z4
109477:山东15Z5
109478:山东15Z6
109479:山东15Z7
109480:山东1509
109481:山东1510
109482:山东1511
109483:山东1512
109484:新疆1513
109485:新疆1514
109486:新疆1515
109487:新疆1516
109488:新疆15Z9
109489:广西1509
109490:广西1510
109491:广西1511
109492:广西1512
109493:广西15Z3
109494:新疆1517
109495:新疆1518
109496:新疆1519
109497:广西15Z4
109498:浙江1505
109499:浙江1506
109500:浙江1507
109501:浙江1508
109502:浙江15Z1
109503:浙江15Z3
109504:浙江15Z4
109505:浙江15Z2
109506:贵州1509
109507:贵州1510
109508:贵州1511
109509:贵州1512
109510:河北15Z4
109511:河北15Z5
109512:河北1505
109513:河北1506
109514:河北1507
109515:河北1508
109516:云南1505
109517:云南1506
109518:云南1507
109519:云南1508
109520:云南15Z5
109521:云南15Z6
109522:云南15Z7
109523:云南15Z8
109524:福建1509
109525:福建1510
109526:福建1511
109527:福建1512
109528:福建15Z3
109529:福建15Z4
109530:青海1509
109531:青海1510
109532:青海1511
109533:青海1512
109534:四川1509
109535:四川1510
109536:四川1511
109537:四川1512
109538:广东15Z4
109539:广东15Z5
109540:广东15Z6
109541:广东1509
109542:广东1510
109543:广东1511
109544:广东1512
109545:海南15Z1
109546:海南15Z2
109547:海南15Z3
109548:海南1505
109549:海南1506
109550:海南1507
109551:海南1508
109552:湖北1513
109553:湖北1514
109554:湖北1515
109555:湖北1516
109556:湖北15Z5
109557:湖北15Z6
109558:湖北15Z7
109559:湖北15Z8
109560:浙江1509
109561:浙江1510
109562:浙江1511
109563:浙江1512
109564:浙江15Z5
109565:浙江15Z6
109566:浙江15Z7
109567:浙江15Z8
109568:江西15Z1
109569:江西15Z2
109570:江西15Z3
109571:江西15Z4
109572:江西1505
109573:江西1506
109574:江西1507
109575:江西1508
109576:江西15Z5
109577:江西15Z6
109578:江西15Z7
109579:江西15Z8
109580:甘肃15Z3
109581:甘肃15Z4
109582:甘肃1509
109583:甘肃1510
109584:甘肃1511
109585:甘肃1512
109586:上海1505
109587:上海1506
109588:上海1507
109589:上海15Z3
109590:上海15Z4
109591:上海15Z5
109592:上海15Z6
109593:四川15Z1
109594:四川15Z2
109595:四川15Z3
109596:四川15Z4
109597:福建1513
109598:福建1514
109599:福建1515
109600:福建1516
109601:福建15Z5
109602:福建15Z6
109603:福建15Z7
109604:福建15Z8
109605:安徽1510
109606:安徽1511
109607:安徽1512
109608:安徽1513
109609:安徽15Z3
109610:安徽15Z4
109611:宁夏15Z5
109612:宁夏15Z6
109613:宁夏1509
109614:宁夏1510
109615:宁夏1511
109616:宁夏1512
109617:宁夏15Z1
109618:宁夏15Z2
109619:宁夏15Z3
109620:宁夏15Z4
109621:天津15Z4
109622:天津15Z5
109623:天津15Z6
109624:天津1509
109625:天津1510
109626:天津1511
109627:天津1512
109628:广东1513
109629:广东1514
109630:广东1515
109631:广东1516
109632:山西15Z1
109633:山西15Z2
109634:山西1505
109635:山西1506
109636:山西1507
109637:山西1508
109638:河南1509
109639:河南1510
109640:河南1511
109641:河南1512
109642:河南15Z5
109643:河南15Z6
109644:河南15Z7
109645:河南15Z8
109646:云南1509
109647:云南1510
109648:云南1511
109649:云南1512
109650:云南15Z9
109651:云南1513
109652:云南1514
109653:云南1515
109654:内蒙15Z5
109655:内蒙15Z6
109656:内蒙15Z7
109657:内蒙15Z8
109658:内蒙1509
109659:内蒙1510
109660:内蒙1511
109661:内蒙1512
109662:厦门1505
109663:厦门1506
109664:厦门1507
109665:厦门1508
109666:厦门15Z3
109667:厦门15Z4
109668:宁波15Z5
109669:宁波15Z6
109670:宁波15Z7
109671:宁波15Z8
109672:贵州15Z1
109673:贵州15Z2
109674:贵州15Z3
109675:贵州15Z4
109676:江苏1509
109677:江苏1510
109678:江苏1511
109679:江苏1512
109680:江苏15Z8
109681:江苏15Z9
109682:江苏1513
109683:江苏1514
109684:陕西1510
109685:陕西1511
109686:陕西1512
109687:陕西15Z9
109688:陕西1513
109689:陕西1514
109690:陕西1515
109691:陕西1509
109692:龙江1505
109693:龙江1506
109694:龙江1507
109695:龙江1508
109696:龙江15Z4
109697:龙江15Z5
109698:宁波1505
109699:宁波1506
109700:宁波1507
109701:宁波1508
109702:大连15Z5
109703:大连15Z6
109704:大连15Z7
109705:大连15Z8
109706:大连1505
109707:大连1506
109708:大连1507
109709:大连1508
109710:吉林1505
109711:吉林1506
109712:吉林1507
109713:吉林1508
109714:吉林15Z1
109715:吉林15Z2
109716:吉林15Z3
109717:吉林15Z4
109718:北京1505
109719:北京1506
109720:北京1507
109721:北京1508
109722:北京15Z5
109723:北京15Z6
109724:北京15Z7
109725:北京15Z8
109726:北京15Z9
109727:湖南1505
109728:湖南1506
109729:湖南1507
109730:湖南1508
109731:上海1508
109732:上海1509
109733:贵州15Z8
109734:贵州15Z5
109735:贵州15Z6
109736:贵州15Z7
109737:贵州1513
109738:贵州1514
109739:贵州1515
109740:贵州1516
109741:山东1513
109742:山东1514
109743:山东1515
109744:山东1516
109745:浙江1513
109746:浙江1514
109747:浙江1515
109748:浙江1516
109749:福建1517
109750:福建1518
109751:福建1519
109752:福建1520
109753:福建15Z9
109754:福建1521
109755:青岛1509
109756:青岛1510
109757:青岛1511
109758:青岛1512
109759:内蒙1513
109760:内蒙1514
109761:内蒙1515
109762:内蒙1516
109763:内蒙15Z9
109764:内蒙1517
109765:辽宁1509
109766:辽宁1510
109767:辽宁1511
109768:辽宁1512
109769:甘肃1513
109770:甘肃1514
109771:甘肃1515
109772:甘肃1516
109773:山西1509
109774:山西1510
109775:山西1511
109776:山西1512
109777:贵州1517
109778:贵州1518
109779:贵州1519
109780:贵州1520
109781:湖北1601
109782:湖北1602
109783:湖北1603
109784:湖北1604
109785:广东16Z1
109786:广东16Z2
109787:广东16Z3
109788:广东1601
109789:广东1603
109790:广东1604
109791:广东1602
109792:浙江1601
109793:浙江1602
109794:浙江1603
109795:浙江1604
109796:山东1601
109797:山东1602
109798:山东1603
109799:山东1604
109800:山东16Z1
109801:山东16Z2
109802:山东16Z3
109803:山东16Z4
109804:内蒙1601
109805:内蒙1602
109806:内蒙1603
109807:内蒙1604
109808:江苏1601
109809:江苏1602
109810:江苏1603
109811:江苏1604
109812:江苏16Z1
109813:江苏16Z2
109814:江苏16Z3
109815:江苏16Z4
109816:重庆16Z1
109817:重庆16Z2
109818:重庆1601
109819:重庆1602
109820:重庆1603
109821:重庆1604
109822:天津1601
109823:天津1602
109824:天津16Z1
109825:天津16Z2
109826:天津16Z3
109827:云南1601
109828:云南1602
109829:云南16Z1
109830:云南16Z2
109831:新疆1601
109832:新疆1602
109833:新疆1603
109834:新疆1604
109835:宁夏1601
109836:宁夏1602
109837:宁夏1603
109838:江西1601
109839:江西1602
109840:江西1603
109841:江西1604
109842:江西16Z1
109843:江西16Z2
109844:江西16Z3
109845:江西16Z4
109846:广西16Z1
109847:广西16Z2
109848:广西1601
109849:广西1602
109850:广西1603
109851:广西1604
109852:辽宁1601
109853:辽宁1602
109854:辽宁1603
109855:辽宁1604
109856:四川1601
109857:四川1602
109858:四川1603
109859:四川1604
109860:宁夏1604
109861:安徽1601
109862:安徽1602
109863:安徽1603
109864:安徽1604
109865:青海1601
109866:青海1602
109867:青海1603
109868:青海1604
109869:广东1605
109870:广东1606
109871:广东1607
109872:广东1608
109873:广东16Z4
109874:广东16Z5
109875:广东16Z6
109876:广西1605
109877:广西1606
109878:广西1607
109879:广西1608
109880:贵州16Z1
109881:贵州16Z2
109882:贵州16Z3
109883:贵州16Z4
109884:贵州1601
109885:贵州1602
109886:贵州1603
109887:贵州1604
109888:新疆16Z1
109889:新疆16Z2
109890:新疆16Z3
109891:新疆16Z4
109892:新疆1605
109893:新疆1606
109894:新疆1607
109895:新疆1608
109896:龙江1601
109897:龙江1602
109898:龙江1603
109899:龙江1604
109900:龙江16Z1
109901:龙江16Z2
109902:龙江16Z3
109903:龙江16Z4
109904:湖南1601
109905:湖南1602
109906:河北16Z1
109907:河北16Z2
109908:河北16Z3
109909:河北16Z4
109910:河北1601
109911:河北1602
109912:河北1603
109913:河北1604
109914:河南1601
109915:河南1602
109916:河南1603
109917:河南1604
109918:湖北16Z1
109919:湖北16Z2
109920:湖北1605
109921:湖北1606
109922:湖北1607
109923:湖北1608
109924:甘肃1601
109925:甘肃1602
109926:甘肃1603
109927:甘肃16Z1
109928:甘肃16Z2
109929:山西1601
109930:山西1602
109931:山西1603
109932:山西1604
109933:山东1605
109934:山东1606
109935:山东1607
109936:山东1608
109937:陕西1601
109938:陕西1602
109939:陕西1603
109940:陕西1604
109941:陕西16Z1
109942:陕西16Z2
109943:陕西16Z3
109944:陕西16Z4
109945:湖南1603
109946:湖南1604
109947:海南1601
109948:海南1602
109949:海南1603
109950:宁波1601
109951:宁波1602
109952:宁波1603
109953:宁波1604
109954:宁波16Z1
109955:宁波16Z2
109956:宁波16Z3
109957:宁波16Z4
109958:青岛1601
109959:青岛1602
109960:青岛1603
109961:青岛1604
109962:青岛16Z1
109963:青岛16Z2
109964:青岛16Z3
109965:四川16Z1
109966:四川16Z2
109967:四川16Z3
109968:四川16Z4
109969:四川1605
109970:四川1606
109971:四川1607
109972:四川1608
109973:宁夏16Z1
109974:宁夏16Z2
109975:宁夏16Z3
109976:宁夏16Z4
109977:辽宁16Z1
109978:辽宁16Z2
109979:辽宁16Z3
109980:辽宁16Z4
109981:云南16Z3
109982:云南16Z4
109983:云南1603
109984:云南1604
109985:陕西1605
109986:陕西1606
109987:陕西1607
109988:陕西1608
109989:陕西16Z5
109990:陕西16Z6
109991:陕西16Z7
109992:陕西16Z8
109993:陕西16Z9
109994:陕西1609
109995:陕西1610
109996:陕西1611
109997:青海1605
109998:青海1606
109999:青海1607
111018:02电网15
111019:02广核债
111023:03中铁债
111026:05粤交通
111032:06鲁能债
111050:09华菱债
111051:09怀化债
111052:09哈城投
111057:09兖城投
111058:09黄城投
111059:09宿建投
111060:10佳城投
111062:10并高铁
111063:11富阳债
111064:11长交债
111065:11豫中小
111067:14邯中小
112004:08中粮债
112019:09宜化债
112022:10南玻02
112024:10煤气02
112025:11珠海债
112027:11新兴02
112030:11鲁西债
112033:11柳工02
112034:11陕气债
112035:11国脉债
112036:11三钢01
112037:11万方债
112038:11锡业债
112039:11黔轮债
112040:11建能债
112041:11冀东01
112043:11东控02
112044:11中泰01
112045:11宗申债
112046:11华孚01
112047:11海大债
112048:11凯迪债
112049:11安泰01
112050:11报喜01
112051:11报喜02
112052:11许继债
112053:11新筑债
112055:11徐工01
112056:11远兴债
112057:11东磁债
112058:11中粮01
112059:11联化债
112062:12新都债
112064:12隆平债
112065:12柳工债
112066:11西建债
112067:11冀东02
112068:11棕榈债
112069:12明牌债
112070:12中泰债
112071:11智光债
112073:11三钢02
112074:12华茂债
112075:12雅致01
112076:12雅致02
112077:12天沃债
112078:12太钢01
112079:12长安债
112080:12万向债
112081:11新野01
112083:11国星债
112084:11万家债
112086:12圣农01
112088:12富春01
112089:12万丰01
112092:12海药债
112093:11亚迪01
112094:11中利债
112095:12康盛债
112096:12国创债
112097:12亚厦债
112098:11新野02
112100:12盾安债
112101:12安泰债
112102:12大康债
112103:12海型债
112106:12太钢03
112107:12云内债
112109:12南糖债
112110:12东锆债
112112:12江泥01
112113:12冀东01
112114:12冀东02
112115:12冀东03
112116:12中桥债
112117:12福发债
112118:12金螳01
112119:12恒邦债
112120:12联发债
112121:12景兴债
112122:12建峰债
112123:12中山01
112124:11徐工02
112126:12科伦01
112127:12华西债
112128:12湘金01
112129:12华锦债
112130:12华包债
112132:12制药债
112133:12亚夏债
112134:12合兴债
112136:12勤上01
112137:12康得债
112138:12苏宁01
112139:12北新债
112140:12基地债
112141:12金王债
112142:12格林债
112143:12粤电01
112144:12晨鸣债
112145:13荣信01
112146:12希努01
112148:12光电债
112149:12芭田债
112150:12银轮债
112153:12科伦02
112154:12盐湖01
112155:12正邦债
112156:12中顺债
112157:12科陆01
112160:12毅昌01
112161:13传化债
112162:12粤电债
112163:12黑牛01
112165:12德豪债
112166:12河钢02
112167:12莱士债
112168:12三维债
112169:12中财债
112170:12濮耐01
112171:12久联债
112172:13普邦债
112173:12南港债
112174:13广田01
112175:13三九01
112177:13围海债
112179:13南洋债
112180:12奥飞债
112181:13广发01
112182:13广发02
112183:13广发03
112184:13嘉寓债
112186:13国元01
112187:13国元02
112188:13渤租债
112189:12瑞泽债
112190:11亚迪02
112192:13赤湾01
112193:13美邦01
112194:13东北01
112195:13东北02
112196:13苏宁债
112197:13山证01
112198:14欧菲债
112199:14铁岭债
112200:14三聚债
112201:14机电01
112202:14嘉杰债
112203:14北农债
112204:14好想债
112205:14翰宇01
112207:14锦龙债
112208:14华邦01
112209:14雏鹰债
112210:14金禾债
112211:14搜特01
112212:14中山债
112213:14中超债
112214:14杰赛债
112215:14万马01
112216:14欣旺01
112217:14东江01
112218:14山证01
112219:14渝发债
112220:14福星01
112221:14万里债
112222:14荣盛债
112223:14江泥01
112224:14兴蓉01
112225:14恒运01
112226:14科陆01
112227:14利源债
112228:14怡亚债
112229:14白药01
112230:14司尔01
112231:14金贵债
112232:14长证债
112233:14漳发债
112234:14中弘债
112235:15福星01
112236:15本钢01
112237:15辉煌01
112238:15振业债
112239:15沃尔债
112240:15华联债
112241:15广田债
112242:15岭南债
112243:15东旭债
112244:15国海债
112245:15顺鑫01
112246:15金一债
112247:15华东债
112248:15翰宇01
112249:15博彦债
112250:15云旅债
112251:15恒运债
112252:15鄂能01
112253:15荣盛01
112254:15湘金01
112255:15濮耐01
112256:15天保01
112257:15荣盛02
112258:15荣盛03
112259:15甘电债
112260:15阳房01
112261:15互动债
112262:15荣安债
112263:15中房债
112264:15亚迪01
112265:15中环债
112266:15利尔01
112267:15阳房02
112268:15东华01
112269:15涪陵01
112270:15华邦债
112271:15中粮01
112272:15金科01
112273:15金街01
112274:15金街02
112275:15搜特债
112276:15金鸿债
112277:15金街03
112278:15东莞债
112279:15渤租01
112280:15东华02
112281:15中洲债
112282:15西部01
112283:15西部02
112284:15渤租02
112285:15万科01
112286:15海伟01
112287:15海投债
112288:15证通01
112289:15顺鑫02
112290:15证通02
112291:15渝外贸
112292:16冀中01
112293:15深爱01
112294:15海伟02
112295:15司尔债
112296:15东北债
112297:15粤科债
112298:15新证债
112299:15中利债
112300:15立业债
112301:15中武债
112302:15东网债
112303:15京威债
112304:15长影01
112305:15长影02
112306:15泛控01
112307:15投资01
112308:15银亿01
112309:15花样02
112310:16美的债
112311:16昆仑01
112312:16徐工01
112313:16铁汉01
112314:16华南01
112315:16宝龙债
112316:16航空债
112317:16太安01
112318:16白药01
112319:16一创01
112320:16蓝标债
112321:16龙基01
112322:16涪陵01
112323:16投资01
112324:16高鸿债
112325:16中南01
112326:16中弘01
112328:16曲文01
112329:16太安02
112330:16巨轮01
112331:16宝德01
112332:16劲嘉01
112333:16鲁焦01
112334:16国购01
112335:16海资01
112336:16太安债
112337:16双星01
112338:16迪科01
112339:16中航城
112340:16泛控01
112341:16宝龙02
112342:16厦贸01
112343:16魏桥01
112344:16魏桥02
112345:16劲嘉02
112346:16惠誉01
112347:16惠誉02
112348:16华美01
112349:16红楼债
112350:16软控01
112351:16步高01
112352:16TCL01
112353:16TCL02
112354:16国购02
112355:16桂资01
112357:16海资02
112358:16BOE01
112359:16魏桥03
112361:16新大02
112362:16泛控02
112363:16三湘债
112364:16云白01
112365:16广业01
112366:16宝安01
112367:16华西01
112369:16峨旅01
112370:16新纶债
112371:16太阳01
112372:16棕榈01
112373:16奥瑞金
112375:16昆投01
112376:16侨城01
112377:16侨城02
112378:16盛润债
112379:16新国都
112380:16东林01
112381:16华联债
112382:16南都01
112383:16当代债
112384:16歌尔01
112385:16奥燃01
112386:16申宏01
112387:16华南02
112388:16聚龙债
112389:16华西02
112390:16三聚债
112392:16掌趣01
112393:16万维01
112394:16泰禾02
112395:16泰禾03
112397:16胜通01
112400:16中南债
112402:16华股01
112403:16农四01
116001:广交投A2
116002:广交投A3
116003:广交投A4
116004:广交投A5
116005:广交投A6
116015:民生2A3
116016:民生2A4
116017:民生3A1
116018:民生3A2
116019:民生3A3
116021:合热02
116022:合热03
116023:合热04
116024:合热05
116026:凯迪2优1
116027:凯迪2优2
116028:凯迪2优3
116029:凯迪2优4
116030:凯迪2优5
116031:凯迪2优6
116033:鑫桥01
116034:鑫桥02
116035:鑫桥03
116036:鑫桥04
116037:鑫桥05
116038:鑫桥06
116039:鑫桥07
116040:鑫桥08
116042:金科优1
116043:金科优2
116044:金科优3
116045:金科优4
116046:金科优5
116047:金科次级
116048:民生4A1
116049:民生4A2
116050:民生4A3
116051:民生5A1
116052:华发01
116053:华发02
116054:华发03
116055:华发04
116056:华发05
116057:华发06
116058:华发07
116060:长宇1优
116062:世联1A
116063:世联1B
116065:赢时通1A
116067:15蚂蚁7A
116068:15蚂蚁7B
116070:15星河A1
116071:15星河A2
116072:15星河A3
116073:15星河A4
116074:15星河A5
116076:京东2优1
116077:京东2优2
116079:泰达01
116080:泰达02
116081:泰达03
116082:泰达04
116083:泰达05
116085:民生6A1
116086:民生6A2
116088:民生7A2
116089:民生7A3
116090:中南1优1
116091:中南1优2
116092:中南1优3
116093:中南1优4
116094:中南1优5
116096:三电优01
116097:三电优02
116098:三电优03
116099:三电优04
116100:三电优05
116101:三电优06
116102:三电优07
116104:大地优01
116105:大地优02
116106:大地优03
116108:民生8A1
116109:民生8A2
116110:鸿飞01
116111:鸿飞02
116112:鸿飞03
116113:鸿飞04
116115:周燃01
116116:周燃02
116117:周燃03
116118:周燃04
116119:周燃05
116121:徐工优01
116122:徐工优02
116125:深南优02
116126:深南优03
116127:深南优04
116128:深南优05
116129:深南优06
116130:深南优07
116131:深南优08
116132:深南优09
116133:深南优10
116135:邹电优01
116136:邹电优02
116137:邹电优03
116140:易汇一A2
116141:易汇一B
116143:信择161A
116144:信择161B
116146:招商创业
116147:华热01
116148:华热02
116149:华热03
116150:华热04
116152:民生9A1
116153:民生9A2
116155:民生10A2
116156:民生10A3
116157:徐工优先
116159:京东3优1
116160:京东3优2
116161:京东3优3
116163:蛟川汽01
116164:蛟川汽02
116165:蛟川汽03
116166:蛟川汽04
116167:蛟川汽05
116171:16太盟A3
116172:16太盟A4
116173:16太盟A5
116174:16太盟A6
116175:16太盟A7
116176:16太盟B1
116177:16太盟B2
116178:16太盟B3
116180:橙E1A1
116181:橙E1A2
116183:德基02
116184:德基03
116185:德基04
116186:德基05
116187:德基06
116189:16榆靖01
116190:16榆靖02
116191:16榆靖03
116192:16榆靖04
116193:16榆靖05
116194:16榆靖06
116195:16榆靖07
116196:16榆靖08
116199:16宜人A2
116200:16宜人A3
116201:16宜人A4
116202:16宜人A5
116203:16宜人A6
116204:16宜人B
116205:16宜人C
116207:康景01
116208:康景02
116209:康景03
116210:康景04
116211:康景05
116212:康景06
116214:16巴拉A1
116215:16巴拉A2
116216:16巴拉A3
116217:16巴拉A4
116224:镇安置1
116225:镇安置2
116226:镇安置3
116227:镇安置4
116228:镇安置5
116229:镇安置6
116231:京东1优A
116232:京东1优B
116253:16镇公01
116254:16镇公02
116255:16镇公03
116256:16镇公04
116257:16镇公05
116258:16镇公06
116263:16苏源01
116264:16苏源02
116265:16苏源03
116266:16苏源04
116267:16苏源05
117002:14海宁债
117003:14歌尔债
117005:14卡森01
117008:15东集01
117009:15九洲债
117010:15中基债
117011:15大重债
117013:15纳海债
117014:15大族01
117015:15雅本债
117016:16华泰01
117017:16山田01
117019:16卓越EB
117022:16配投01
117024:15宝利来
117538:广发1602
117539:广发1603
117541:西部1601
117542:西部1602
118119:13天喜01
118121:13福星门
118122:13利树债
118123:13华珠债
118124:13常机债
118129:13华春债
118134:13中磁债
118140:13新港债
118149:13八达01
118154:13共兴债
118155:13安期生
118156:13江苏院
118158:13百川债
118164:13昭信债
118168:13新天阳
118173:13合利债
118175:13丰投01
118180:13八达02
118187:13弘佳债
118191:13东宇债
118192:13华耐01
118194:13京天债
118195:13易博债
118198:13珍珠债
118199:13德和债
118205:13海岛02
118210:13光彩债
118211:13巨龙债
118212:13八达03
118213:13腾晋02
118214:13腾晋03
118220:13华光04
118226:14铭可达
118227:13华耐02
118228:14金硕01
118230:13昆旅02
118233:14金硕02
118239:14通江海
118242:13花王01
118249:14金鑫债
118250:14广亚债
118251:14泰鑫01
118252:14泰鑫02
118253:14佳源债
118255:14一品01
118256:14一品02
118259:14洋口债
118261:14衡拖债
118262:14讯鸟债
118266:14金鼎债
118267:14明兴债
118268:14众源债
118270:14易通债
118271:14佳捷债
118272:14黎明债
118274:14为海建
118275:14萌业01
118276:14萌业02
118277:14萌业03
118279:14天路通
118281:14苏污水
118282:14江仓债
118283:14金国际
118284:14叠石01
118285:14叠石02
118286:14银河债
118287:14科力01
118288:14信通债
118289:14申鑫债
118291:14南郊债
118292:14四方01
118295:14科力02
118297:14陕旅01
118298:14陕旅02
118299:14陕旅03
118300:14万思顿
118301:14常机债
118302:14平临债
118303:14鸿仪01
118304:14海格01
118305:14汤山债
118306:14华夏债
118307:14天安债
118308:15昆产投
118310:14汝城债
118311:14锦昉债
118312:14苏佳债
118313:14筑经01
118315:14华控债
118316:15四方债
118317:14邳水01
118318:15迎宾馆
118319:15富水债
118321:15漳龙债
118322:14筑经02
118323:14高新债
118324:15长城债
118325:15阳光01
118326:15宁证01
118327:15新华联
118328:15新能债
118329:15易成债
118330:14寿金海
118331:15豪美01
118332:15株国01
118333:15株国02
118334:15阳光02
118335:15临安01
118336:15昆投02
118337:15乐视01
118338:15长交债
118339:15泰禾债
118340:15株高01
118341:15株发01
118342:15雷山债
118343:15铁岭01
118344:15陕旅01
118345:15中南01
118346:15昆发01
118347:15翔控01
118348:15翔控02
118349:15中孚01
118350:15阳集01
118351:15贵高01
118352:15龙里债
118353:15翔控03
118354:15中利01
118355:15冀东01
118356:15扬化01
118357:15海控01
118358:15绵交发
118359:15东旭01
118360:15株发02
118361:15泰禾02
118362:15瑞泥01
118363:15荣发01
118364:15柯桥01
118365:15泰丰01
118366:15酉阳01
118367:15酉阳02
118368:15株国03
118369:15中城01
118370:15乐视02
118371:15柳建债
118372:15金国01
118373:15安城债
118374:15长经开
118375:15泰禾03
118376:15新郑债
118377:15金控01
118378:15渝高债
118379:15广投01
118380:15广投02
118381:15山投01
118382:15利得债
118383:15阳集02
118384:15东丽01
118385:15柯桥02
118386:15徐新债
118387:15中南02
118388:15荣发02
118389:15荣发03
118390:15阳光03
118391:15东方01
118392:15十四01
118393:15万通01
118394:15万通02
118395:15昆新01
118396:15长兴债
118397:15华控债
118398:15道桥债
118399:15恒贸01
118400:15福惠01
118401:15国安债
118402:15星海02
118403:15金一01
118404:15珠九债
118405:15昆钢债
118406:15瑞泥02
118407:15中城02
118408:15浙湖01
118409:15宁温泉
118410:15兴阳债
118411:15新华01
118412:15恒泰01
118413:15翔控04
118414:15翔控05
118415:15南国01
118416:15淮新01
118417:15淮新02
118418:15恒贸02
118419:15亿阳01
118420:15华控02
118421:15阳光04
118422:15沪宝01
118423:15和国资
118424:15南国02
118425:15东方02
118426:15昆经01
118427:15金债01
118428:15新沂01
118429:15新沂02
118430:15国广债
118431:15桂投01
118432:15聚丰01
118433:15临安02
118434:15玉舍债
118435:15铜仁01
118436:15文山债
118437:15泰禾04
118438:15泰禾05
118439:15邳水债
118440:15铜发01
118441:15荣发04
118442:15荣发05
118443:15中南03
118444:15桂投02
118445:15天易债
118446:15城南债
118447:15盛运01
118448:15中科01
118449:15中科02
118450:15中城03
118451:15中城04
118452:15中置01
118453:16恒地01
118454:16高创01
118455:16高创02
118456:15晋交02
118457:15福惠02
118458:15鸿业01
118459:15鸿业02
118460:15长租01
118461:15富通01
118462:16桂金投
118463:16紫光01
118464:16新光01
118465:15泛海债
118466:16蓝盾债
118467:16蔡家01
118468:16时代01
118470:16融地01
118471:16泰达01
118472:16泰达02
118473:16宝城债
118474:16金圆01
118475:16大兴债
118476:16东泰债
118477:16娄底债
118478:15暴风债
118479:16皋沿01
118480:16皋沿02
118481:16融投01
118482:16华控01
118483:16银宝债
118484:16十四01
118485:16苏宁01
118486:16科创01
118487:16铜仁01
118488:16新野债
118489:16奥园01
118490:15阳光05
118491:16融投02
118492:16华控02
118493:16桂投01
118494:16大厂债
118495:16华福01
118496:16株高01
118497:16昆交01
118498:16隆地01
118499:16海控01
118500:16必康债
118501:16益阳01
118502:16昆新债
118503:16丰兴01
118504:16丰兴02
118505:15叠石债
118506:16万楼01
118508:16开源01
118509:16嘉建01
118510:16六师债
118511:16圣乙债
118512:16苏展01
118514:16福汽债
118515:16都兴01
118516:16莱钢01
118517:16怀化债
118518:16遵经投
118519:16德泰01
118520:16中城01
118521:16宜集01
118522:16皖新路
118523:16弘债01
118524:16红企01
118525:16鸿达债
118526:16中置01
118527:16荣发01
118528:16阜投01
118529:16常建01
118530:16仪水务
118531:16天房01
118532:16西投01
118533:16蓟城01
118534:16昆经01
118535:16雏鹰01
118536:16金贵01
118537:16泛海01
118538:16景峰债
118539:16临安01
118540:16株展01
118541:16金控01
118542:16中房私
118543:16临开债
118544:16湘高01
118545:16郑公01
118548:16彩生01
118549:16余投01
118550:16金贵02
118551:16沿江债
118552:16邳润城
118553:16桂建01
118554:16中城02
118555:16句福01
118556:16渝万盛
118558:16高投01
118559:16东方01
118560:16株展02
118561:16金科01
118562:16金科02
118563:16天房02
118564:16新光02
118566:16化纤债
118567:16国泰01
118568:16桂铁投
118569:16内江01
118570:16铁岭01
118571:16南宁债
118572:16中城03
118573:16中科债
118574:16昆经02
118575:16镇城01
118576:16环城01
118577:16漳龙债
118578:16新华01
118579:16环保债
118581:16沿江02
118582:16铜仁02
118583:16华美S1
118584:16江油01
118585:16泰禾01
118586:16银亿01
118587:16道桥01
118588:16天翔01
118589:16袍工01
118590:16昆交02
118591:16精功01
118592:16华福02
118593:16泛海02
118597:16美的01
118598:16聚丰01
118599:16福星01
118602:16万通03
118603:16国控债
118604:16东丽债
118605:16润银01
118607:16紫光02
118608:16三鼎01
118609:16阳集01
118610:16连房政
118611:16金控02
118612:16怡亚01
118613:16皋沿03
118616:16长租01
118617:16延安债
118618:16海空01
118619:16西彭债
118620:16东集01
118624:16宜集02
118625:16仁保债
118626:16荣发02
118628:16花木债
118629:16阜投02
118630:16华汽01
118633:16津金01
118635:16渝迈01
118636:16东投01
118639:16航基01
118641:16春语01
118642:16华控03
118643:16万楼02
118649:16中建01
118651:16五洋01
118652:16郑公02
118653:16广田01
118654:16广田02
118655:16洪山债
118656:16银亿02
118658:16遵汇01
118659:16维尔01
118664:16淮水01
118667:16汇源01
118668:16雏鹰02
118669:16天恒02
118671:16金智债
118672:16东建债
118679:16正商01
118682:16新交债
118686:16高投02
118688:16阳集02
118692:16泾河01
118703:16禹地产
118902:13一创01
118903:13一创02
118905:14广发02
118906:14国信01
118907:14长城债
118908:14南京债
118910:14东北债
118913:15东北01
118915:15国信01
118916:15一创01
118917:15国海01
118918:15南京01
118919:15英大01
118920:15国信02
118922:15国信03
118923:15国海02
118924:15南京02
118926:15国信05
118928:15东北02
118929:15长江01
118930:15一创02
118932:15国元01
118933:15广发05
118935:15国信06
118936:15广发07
118938:15国信Y1
118939:15中山01
118940:15中山02
118941:15西证01
118942:16广发01
119021:宁公控5
119026:侨城04
119027:侨城05
119032:澜沧江4
119033:澜沧江5
119053:启航优先
119057:国泰一04
119066:14浦发05
119067:14浦发06
119068:14浦发07
119069:14浦发08
119070:14浦发09
119071:14浦发10
119072:14浦发11
119078:建发房2
119079:建发房3
119080:建发房4
119083:海印02
119084:海印03
119085:海印04
119086:海印05
119093:14长隆03
119094:14长隆04
119095:14长隆05
119096:14长隆06
119097:14长隆07
119098:14长隆08
119104:14水务02
119105:14水务03
119106:14水务04
119107:14水务05
119110:徐新盛02
119111:徐新盛03
119112:徐新盛04
119113:徐新盛05
119116:大都市A2
119117:大都市B
119119:14中和1A
119122:15金通A2
119123:15金通B
119126:14长热02
119127:14长热03
119128:14长热04
119129:14长热05
119130:14长热06
119138:15狮桥07
119141:15濮水02
119142:15濮水03
119143:15濮水04
119144:15濮水05
119147:15蚂蚁1B
119150:湘衡高02
119151:湘衡高03
119152:湘衡高04
119153:湘衡高05
119154:湘衡高06
119155:湘衡高07
119156:湘衡高08
119157:湘衡高09
119158:湘衡高10
119160:15蚂蚁2A
119161:15蚂蚁2B
119163:15中和1A
119164:15中和1B
119167:15远东A2
119168:15远东A3
119169:15远东B
119173:科慧02
119174:科慧03
119175:科慧04
119176:科慧05
119179:星美02
119180:星美03
119181:星美04
119182:星美05
119184:畅行专项
119186:凯迪02
119187:凯迪03
119188:凯迪04
119189:凯迪05
119192:15远东02
119193:15远东03
119195:15远东05
119196:15远东06
119197:15远东次
119199:陕交通02
119200:陕交通03
119201:陕交通04
119202:陕交通05
119204:15蚂蚁3A
119205:15蚂蚁3B
119207:15蚂蚁4A
119208:15蚂蚁4B
119210:长经01
119211:长经02
119212:长经03
119213:长经04
119214:长经05
119215:长经06
119224:美兰03
119225:美兰04
119226:美兰05
119227:美兰06
119228:美兰07
119229:美兰08
119230:美兰09
119231:美兰10
119233:扬保障3
119234:扬保障4
119235:扬保障5
119237:南方A1
119238:南方A2
119239:南方A3
119240:南方A4
119243:房信2
119244:房信3
119245:房信4
119246:房信5
119247:房信6
119249:京东优01
119250:京东优02
119252:15蚂蚁5A
119253:15蚂蚁5B
119255:15蚂蚁6A
119256:15蚂蚁6B
119258:银山1优
119260:瀚华1优
119262:济钢1优
119265:15龙桥02
119266:15龙桥03
119268:民生1A1
119269:民生1A2
119270:民生1A3
119271:15宁北01
119272:15宁北02
119273:15宁北03
119274:15宁北04
119275:15宁北05
119276:15宁北06
119277:15宁北07
119279:中腾信优
119281:15海技A
119282:15海技B
119284:狮桥02A
119285:狮桥02B
119287:茂庸01
119288:茂庸02
119289:茂庸03
119290:茂庸04
119291:茂庸05
119292:茂庸06
119293:茂庸07
119294:茂庸08
119296:15安信A
119297:15安信B
119401:启航次级
119402:14苏宁A
119403:14苏宁B
119404:151苏宁A
119405:151苏宁B
119406:天虹优先
119407:天虹次级
120001:16以岭EB
123001:蓝标转债
128009:歌尔转债
128010:顺昌转债
128011:汽模转债
128012:辉丰转债
131800:Ｒ-003
131801:Ｒ-007
131802:Ｒ-014
131803:Ｒ-028
131805:Ｒ-091
131806:Ｒ-182
131809:Ｒ-004
131810:Ｒ-001
131811:Ｒ-002
150008:瑞和小康
150009:瑞和远见
150012:中证100A
150013:中证100B
150016:合润A
150017:合润B
150018:银华稳进
150019:银华锐进
150020:汇利A
150021:汇利B
150022:深成指A
150023:深成指B
150028:中证500A
150029:中证500B
150030:中证90A
150031:中证90B
150032:多利优先
150033:多利进取
150036:建信稳健
150037:建信进取
150039:鼎利A
150040:鼎利B
150047:消费A
150048:消费B
150049:消费收益
150050:消费进取
150051:沪深300A
150052:沪深300B
150053:泰达500A
150054:泰达500B
150055:500A
150056:500B
150057:中小300A
150058:中小300B
150059:资源A级
150060:资源B级
150064:同瑞A
150065:同瑞B
150066:互利A
150067:互利B
150073:诺安稳健
150075:诺安进取
150076:浙商稳健
150077:浙商进取
150083:深证100A
150084:深证100B
150085:中小板A
150086:中小板B
150088:金鹰500A
150089:金鹰500B
150090:成长A
150091:成长B
150092:诺德300A
150093:诺德300B
150094:泰信400A
150095:泰信400B
150096:商品A
150097:商品B
150100:资源A
150101:资源B
150104:HS300A
150105:HS300B
150106:中小A
150107:中小B
150108:同辉100A
150109:同辉100B
150112:深100A
150113:深100B
150117:房地产A
150118:房地产B
150121:银河优先
150122:银河进取
150123:建信50A
150124:建信50B
150130:医药A
150131:医药B
150133:德信A
150134:德信B
150135:国富100A
150136:国富100B
150138:中证800A
150139:中证800B
150140:国金300A
150141:国金300B
150142:互利债B
150143:转债A级
150144:转债B级
150145:高贝塔A
150146:高贝塔B
150147:同利B
150148:医药800A
150149:医药800B
150150:有色800A
150151:有色800B
150152:创业板A
150153:创业板B
150154:惠丰B
150156:中银互B
150157:金融A
150158:金融B
150160:通福B
150161:惠鑫B
150164:可转债A
150165:可转债B
150167:银华300A
150168:银华300B
150169:恒生A
150170:恒生B
150171:证券A
150172:证券B
150173:TMT中证A
150174:TMT中证B
150175:H股A
150176:H股B
150177:证保A
150178:证保B
150179:信息A
150180:信息B
150181:军工A
150182:军工B
150184:环保A
150185:环保B
150186:军工A级
150187:军工B级
150188:转债优先
150189:转债进取
150190:NCF环保A
150191:NCF环保B
150192:地产A
150193:地产B
150194:互联网A
150195:互联网B
150196:有色A
150197:有色B
150198:食品A
150199:食品B
150200:券商A
150201:券商B
150203:传媒A
150204:传媒B
150205:国防A
150206:国防B
150207:地产A端
150208:地产B端
150209:国企改A
150210:国企改B
150211:新能车A
150212:新能车B
150213:成长A级
150214:成长B级
150215:TMTA
150216:TMTB
150217:新能源A
150218:新能源B
150219:健康A
150220:健康B
150221:中航军A
150222:中航军B
150223:证券A级
150224:证券B级
150225:证保A级
150226:证保B级
150227:银行A
150228:银行B
150229:酒A
150230:酒B
150231:电子A
150232:电子B
150233:传媒业A
150234:传媒业B
150235:券商A级
150236:券商B级
150237:环保A级
150238:环保B级
150241:银行A级
150242:银行B级
150243:创业A
150244:创业B
150245:互联A
150246:互联B
150247:传媒A级
150248:传媒B级
150249:银行A端
150250:银行B端
150251:煤炭A
150252:煤炭B
150255:银行业A
150256:银行业B
150257:生物A
150258:生物B
150259:重组A
150260:重组B
150261:医疗A
150262:医疗B
150263:1000A
150264:1000B
150265:一带A
150266:一带B
150267:银行A类
150268:银行B类
150269:白酒A
150270:白酒B
150271:生物药A
150272:生物药B
150273:带路A
150274:带路B
150275:一带一A
150276:一带一B
150277:高铁A
150278:高铁B
150279:新能A
150280:新能B
150281:金融地A
150282:金融地B
150283:SW医药A
150284:SW医药B
150287:钢铁A
150288:钢铁B
150289:煤炭A级
150290:煤炭B级
150291:银行A份
150292:银行B份
150293:高铁A级
150294:高铁B级
150295:改革A
150296:改革B
150297:互联A级
150298:互联B级
150299:银行股A
150300:银行股B
150301:证券股A
150302:证券股B
150303:创业股A
150304:创业股B
150305:养老A
150306:养老B
150307:体育A
150308:体育B
150309:信息安A
150310:信息安B
150311:智能A
150312:智能B
150315:工业4A
150316:工业4B
150317:E金融A
150318:E金融B
150321:煤炭A基
150322:煤炭B基
150323:环保A端
150324:环保B端
150325:高铁A端
150326:高铁B端
150327:新能A级
150328:新能B级
150329:保险A
150330:保险B
150331:网金融A
150332:网金融B
150335:军工股A
150336:军工股B
150343:证券A基
150344:证券B基
159001:保证金
159003:招商快线
159005:添富快钱
159901:深100ETF
159902:中 小 板
159903:深成ETF
159905:深红利
159906:深成长
159907:中小300
159908:深F200
159909:深TMT
159910:深F120
159911:民营ETF
159912:深300ETF
159913:深价值
159915:创业板
159916:深F60
159918:中创400
159919:300ETF
159920:恒生ETF
159921:中小等权
159922:500ETF
159923:100ETF
159924:300等权
159925:南方300
159926:国债ETF
159927:A300ETF
159928:消费ETF
159929:医药ETF
159930:能源ETF
159931:金融ETF
159932:500深ETF
159933:金地ETF
159934:黄金ETF
159935:景顺500
159936:可选消费
159937:博时黄金
159938:广发医药
159939:信息技术
159940:全指金融
159941:纳指100
159942:中创100
159943:深证ETF
159944:全指材料
159945:全指能源
159946:全指消费
159948:创业板EF
159949:创业板50
160105:南方积配
160106:南方高增
160119:南方500
160123:南方50债
160125:南方香港
160127:南方消费
160128:南方金利
160130:南方永利
160131:南方聚利
160133:南方天元
160135:高铁基金
160136:改革基金
160137:互联基金
160211:国泰小盘
160212:国泰估值
160215:国泰价值
160216:国泰商品
160217:国泰互利
160218:国泰地产
160219:国泰医药
160220:国泰民益
160221:国泰有色
160222:国泰食品
160224:国泰TMT
160225:新汽车
160311:华夏蓝筹
160314:华夏行业
160415:华安S300
160416:石油基金
160417:华安300
160418:银行股基
160419:证券股基
160420:创业50
160505:博时主题
160512:博时卓越
160513:稳健债A
160515:安丰18
160516:博时证保
160517:博时银行
160518:博时睿远
160522:博时睿益
160603:鹏华收益
160605:鹏华５０
160607:鹏华价值
160610:鹏华动力
160611:鹏华治理
160613:鹏华创新
160615:鹏华300
160616:鹏华500
160617:鹏华丰润
160618:鹏华丰泽
160620:资源分级
160621:鹏华丰和
160622:鹏华丰利
160624:鹏华领先
160625:证保分级
160626:信息分级
160627:鹏华策略
160628:房地产
160629:传媒分级
160630:国防分级
160631:银行指基
160632:酒分级
160633:券商指基
160634:环保分级
160635:医药基金
160636:互联网
160637:创业指基
160638:带路分级
160639:高铁分级
160640:新能源
160706:嘉实 300
160716:嘉实50
160717:恒生H股
160718:嘉实多利
160719:嘉实黄金
160720:中期企债
160805:长盛同智
160806:长盛同庆
160807:长盛300
160808:长盛同瑞
160809:长盛同辉
160810:长盛同丰
160812:长盛同益
160813:长盛同盛
160814:金融地产
160910:大成创新
160915:大成景丰
160916:优选LOF
160918:大成小盘
160919:大成产业
160921:大成定增
161005:富国天惠
161010:富国天丰
161014:富国汇利
161015:富国天盈
161017:富国500
161019:富国天锋
161022:创业分级
161024:军工分级
161025:移动互联
161026:国企改革
161027:证券分级
161028:新能源车
161029:银行分级
161030:体育分级
161031:工业4
161032:煤炭基金
161033:智能汽车
161115:易基岁丰
161116:易基黄金
161117:易基永旭
161118:易基中小
161119:易基综债
161121:银行业
161122:生物分级
161123:重组分级
161207:瑞和300
161210:国投新兴
161213:国投消费
161216:双债A
161217:国投资源
161219:国投产业
161222:国投瑞利
161223:国投成长
161224:国投丝路
161225:国投瑞盈
161226:白银基金
161227:深证100
161229:国投中国
161230:丰利A类
161505:银河通利
161507:银河增强
161607:融通巨潮
161610:融通领先
161614:融通添利
161628:融通军工
161629:融通证券
161630:融通农业
161706:招商成长
161713:招商信用
161714:招商金砖
161715:大宗商品
161716:招商双债
161718:高贝分级
161719:转债分级
161720:券商分级
161721:地产分级
161722:招商丰泰
161723:银行基金
161724:煤炭分级
161725:白酒分级
161726:生物医药
161727:招商增荣
161810:银华内需
161811:300分级
161812:100分级
161813:银华信用
161815:银华通胀
161816:90分级
161818:消费分级
161819:中证资源
161820:银华纯债
161821:银华50A
161823:银华永兴
161825:800分级
161826:中证转债
161831:H股分级
161903:万家优选
161907:万家红利
161908:万家添利
161910:成长分级
161911:万家强债
162006:长城久富
162010:长城久兆
162105:持久增利
162107:金鹰500
162108:元盛债券
162207:泰达效率
162215:泰达聚利
162216:泰达500
162307:海富100
162308:海富增利
162411:华宝油气
162412:医疗分级
162413:1000分级
162414:新机遇
162415:美国消费
162509:双禧100
162510:国安双力
162511:国安双佳
162605:景顺鼎益
162607:景顺资源
162703:广发小盘
162711:广发500L
162712:广发聚利
162714:深100基
162715:广发聚源
162907:泰信400
163001:长信医疗
163003:长信利鑫
163005:长信利众
163109:申万深成
163110:申万量化
163111:申万中小
163113:申万证券
163114:申万环保
163115:申万军工
163116:申万电子
163117:申万传媒
163118:医药生物
163208:诺安油气
163209:诺安中创
163210:诺安纯债
163302:大摩资源
163402:兴全趋势
163406:兴全合润
163407:兴全300
163409:兴全绿色
163412:兴全轻资
163415:兴全模式
163503:天治核心
163801:中银中国
163803:中银增长
163804:中银收益
163805:中银策略
163806:中银增利
163807:中银优选
163808:中银100
163809:中银蓝筹
163810:中银价值
163819:中银信用
163821:中银300E
163824:中银盛利
163827:中银产债
163907:中海惠裕
164105:华富强债
164206:天弘添利
164208:天弘丰利
164304:新华环保
164401:健康分级
164402:中航军工
164403:农业精选
164508:国富100
164606:信用增利
164701:添富贵金
164702:添富季红
164705:添富恒生
164808:工银四季
164809:工银500
164810:工银纯债
164811:100母基
164812:工银增利
164814:工银双债
164815:工银资源
164818:传媒母基
164819:环保母基
164820:高铁母基
164821:新能母基
164902:交银添利
164905:交银新能
164906:中国互联
164907:E金融
164908:环境治理
165309:建信300
165310:建信双利
165311:建信信用
165312:建信50
165313:建信优势
165315:网金融
165316:有色金属
165508:信诚深度
165509:信诚增强
165510:信诚四国
165511:信诚500
165512:信诚机遇
165513:信诚商品
165515:信诚300
165516:信诚周期
165517:信诚双盈
165519:信诚医药
165520:信诚有色
165521:信诚金融
165522:信诚TMT
165523:信息安全
165524:智能家居
165525:基建工程
165526:信诚新旺
165705:诺德双翼
165707:诺德S300
165806:东吴100
165807:东吴鼎利
165809:东吴转债
166001:中欧趋势
166002:中欧蓝筹
166003:中欧稳A
166004:中欧稳C
166005:中欧价值
166006:中欧成长
166007:中欧300
166008:中欧强债
166009:中欧动力
166010:中欧鼎利
166011:中欧盛世
166012:中欧信用
166016:中欧纯债
166105:信达增利
166401:浦银增利
166802:浙商300
166902:民生增利
166904:民生添利
167301:保险分级
167501:安信宝利
167503:一带分级
167601:国金300
167701:德邦德信
168001:国寿养老
168101:九泰锐智
168102:九泰锐富
168103:九泰锐益
168201:一带一路
168203:钢铁母基
168204:煤炭母基
168205:银行母基
169101:东证睿丰
169102:东证睿阳
184721:基金丰和
184722:基金久嘉
184728:基金鸿阳
184801:鹏华前海
200011:深物业B
200012:南 玻Ｂ
200016:深康佳Ｂ
200017:深中华B
200018:神州B
200019:深深宝Ｂ
200020:深华发Ｂ
200022:深赤湾Ｂ
200025:特 力Ｂ
200026:飞亚达Ｂ
200028:一致Ｂ
200029:深深房Ｂ
200030:富奥B
200037:*ST南电B
200045:深纺织Ｂ
200053:深基地Ｂ
200054:建摩B
200055:方大Ｂ
200056:皇庭B
200058:深赛格B
200152:山 航Ｂ
200160:南江B
200168:舜喆B
200413:东旭B
200418:小天鹅Ｂ
200429:粤高速Ｂ
200468:*ST宁通B
200488:晨 鸣Ｂ
200505:*ST珠江B
200512:闽灿坤Ｂ
200521:皖美菱Ｂ
200530:大 冷Ｂ
200539:粤电力Ｂ
200541:粤照明Ｂ
200550:江 铃Ｂ
200553:沙隆达Ｂ
200570:苏常柴Ｂ
200581:苏威孚Ｂ
200596:古井贡Ｂ
200613:大东海B
200625:长 安Ｂ
200706:*ST瓦轴B
200725:京东方Ｂ
200726:鲁 泰Ｂ
200761:本钢板Ｂ
200771:杭汽轮Ｂ
200869:张 裕Ｂ
200986:粤华包Ｂ
200992:中 鲁Ｂ
300001:特锐德
300002:神州泰岳
300003:乐普医疗
300004:南风股份
300005:探路者
300006:莱美药业
300007:汉威电子
300008:天海防务
300009:安科生物
300010:立思辰
300011:鼎汉技术
300012:华测检测
300013:新宁物流
300014:亿纬锂能
300015:爱尔眼科
300016:北陆药业
300017:网宿科技
300018:中元股份
300019:硅宝科技
300020:银江股份
300021:大禹节水
300022:吉峰农机
300023:宝德股份
300024:机器人
300025:华星创业
300026:红日药业
300027:华谊兄弟
300028:金亚科技
300029:天龙光电
300030:阳普医疗
300031:宝通科技
300032:金龙机电
300033:同花顺
300034:钢研高纳
300035:中科电气
300036:超图软件
300037:新宙邦
300038:梅泰诺
300039:上海凯宝
300040:九洲电气
300041:回天新材
300042:朗科科技
300043:互动娱乐
300044:赛为智能
300045:华力创通
300046:台基股份
300047:天源迪科
300048:合康变频
300049:福瑞股份
300050:世纪鼎利
300051:三五互联
300052:中青宝
300053:欧比特
300054:鼎龙股份
300055:万邦达
300056:三维丝
300057:万顺股份
300058:蓝色光标
300059:东方财富
300061:康耐特
300062:中能电气
300063:天龙集团
300064:豫金刚石
300065:海兰信
300066:三川智慧
300067:安诺其
300068:南都电源
300069:金利华电
300070:碧水源
300071:华谊嘉信
300072:三聚环保
300073:当升科技
300074:华平股份
300075:数字政通
300076:GQY视讯
300077:国民技术
300078:思创医惠
300079:数码视讯
300080:易成新能
300081:恒信移动
300082:奥克股份
300083:劲胜精密
300084:海默科技
300085:银之杰
300086:康芝药业
300087:荃银高科
300088:长信科技
300089:文化长城
300090:盛运环保
300091:金通灵
300092:科新机电
300093:金刚玻璃
300094:国联水产
300095:华伍股份
300096:易联众
300097:智云股份
300098:高新兴
300099:尤洛卡
300100:双林股份
300101:振芯科技
300102:乾照光电
300103:达刚路机
300104:乐视网
300105:龙源技术
300106:西部牧业
300107:建新股份
300108:双龙股份
300109:新开源
300110:华仁药业
300111:向日葵
300112:万讯自控
300113:顺网科技
300114:中航电测
300115:长盈精密
300116:坚瑞消防
300117:嘉寓股份
300118:东方日升
300119:瑞普生物
300120:经纬电材
300121:阳谷华泰
300122:智飞生物
300123:太阳鸟
300124:汇川技术
300125:易世达
300126:锐奇股份
300127:银河磁体
300128:锦富新材
300129:泰胜风能
300130:新国都
300131:英唐智控
300132:青松股份
300133:华策影视
300134:大富科技
300135:宝利国际
300136:信维通信
300137:先河环保
300138:晨光生物
300139:晓程科技
300140:启源装备
300141:和顺电气
300142:沃森生物
300143:星河生物
300144:宋城演艺
300145:中金环境
300146:汤臣倍健
300147:香雪制药
300148:天舟文化
300149:量子高科
300150:世纪瑞尔
300151:昌红科技
300152:科融环境
300153:科泰电源
300154:瑞凌股份
300155:安居宝
300156:神雾环保
300157:恒泰艾普
300158:振东制药
300159:新研股份
300160:秀强股份
300161:华中数控
300162:雷曼股份
300163:先锋新材
300164:通源石油
300165:天瑞仪器
300166:东方国信
300167:迪威视讯
300168:万达信息
300169:天晟新材
300170:汉得信息
300171:东富龙
300172:中电环保
300173:智慧松德
300174:元力股份
300175:朗源股份
300176:鸿特精密
300177:中海达
300178:腾邦国际
300179:四方达
300180:华峰超纤
300181:佐力药业
300182:捷成股份
300183:东软载波
300184:力源信息
300185:通裕重工
300187:永清环保
300188:美亚柏科
300189:神农基因
300190:维尔利
300191:潜能恒信
300192:科斯伍德
300193:佳士科技
300194:福安药业
300195:长荣股份
300196:长海股份
300197:铁汉生态
300198:纳川股份
300199:翰宇药业
300200:高盟新材
300201:海伦哲
300202:聚龙股份
300203:聚光科技
300204:舒泰神
300205:天喻信息
300206:理邦仪器
300207:欣旺达
300208:恒顺众昇
300209:天泽信息
300210:森远股份
300211:亿通科技
300212:易华录
300213:佳讯飞鸿
300214:日科化学
300215:电科院
300216:千山药机
300217:东方电热
300218:安利股份
300219:鸿利智汇
300220:金运激光
300221:银禧科技
300222:科大智能
300223:北京君正
300224:正海磁材
300225:金力泰
300226:上海钢联
300227:光韵达
300228:富瑞特装
300229:拓尔思
300230:永利股份
300231:银信科技
300232:洲明科技
300233:金城医药
300234:开尔新材
300235:方直科技
300236:上海新阳
300237:美晨科技
300238:冠昊生物
300239:东宝生物
300240:飞力达
300241:瑞丰光电
300242:明家联合
300243:瑞丰高材
300244:迪安诊断
300245:天玑科技
300246:宝莱特
300247:乐金健康
300248:新开普
300249:依米康
300250:初灵信息
300251:光线传媒
300252:金信诺
300253:卫宁健康
300254:仟源医药
300255:常山药业
300256:星星科技
300257:开山股份
300258:精锻科技
300259:新天科技
300260:新莱应材
300261:雅本化学
300262:巴安水务
300263:隆华节能
300264:佳创视讯
300265:通光线缆
300266:兴源环境
300267:尔康制药
300268:万福生科
300269:联建光电
300270:中威电子
300271:华宇软件
300272:开能环保
300273:和佳股份
300274:阳光电源
300275:梅安森
300276:三丰智能
300277:海联讯
300278:华昌达
300279:和晶科技
300280:南通锻压
300281:金明精机
300282:汇冠股份
300283:温州宏丰
300284:苏交科
300285:国瓷材料
300286:安科瑞
300287:飞利信
300288:朗玛信息
300289:利德曼
300290:荣科科技
300291:华录百纳
300292:吴通控股
300293:蓝英装备
300294:博雅生物
300295:三六五网
300296:利亚德
300297:蓝盾股份
300298:三诺生物
300299:富春通信
300300:汉鼎宇佑
300301:长方集团
300302:同有科技
300303:聚飞光电
300304:云意电气
300305:裕兴股份
300306:远方光电
300307:慈星股份
300308:中际装备
300309:吉艾科技
300310:宜通世纪
300311:任子行
300312:邦讯技术
300313:天山生物
300314:戴维医疗
300315:掌趣科技
300316:晶盛机电
300317:珈伟股份
300318:博晖创新
300319:麦捷科技
300320:海达股份
300321:同大股份
300322:硕贝德
300323:华灿光电
300324:旋极信息
300325:德威新材
300326:凯利泰
300327:中颖电子
300328:宜安科技
300329:海伦钢琴
300330:华虹计通
300331:苏大维格
300332:天壕环境
300333:兆日科技
300334:津膜科技
300335:迪森股份
300336:新文化
300337:银邦股份
300338:开元仪器
300339:润和软件
300340:科恒股份
300341:麦迪电气
300342:天银机电
300343:联创互联
300344:太空板业
300345:红宇新材
300346:南大光电
300347:泰格医药
300348:长亮科技
300349:金卡股份
300350:华鹏飞
300351:永贵电器
300352:北信源
300353:东土科技
300354:东华测试
300355:蒙草生态
300356:光一科技
300357:我武生物
300358:楚天科技
300359:全通教育
300360:炬华科技
300362:天翔环境
300363:博腾股份
300364:中文在线
300365:恒华科技
300366:创意信息
300367:东方网力
300368:汇金股份
300369:绿盟科技
300370:安控科技
300371:汇中股份
300372:*欣泰
300373:扬杰科技
300374:恒通科技
300375:鹏翎股份
300376:易事特
300377:赢时胜
300378:鼎捷软件
300379:东方通
300380:安硕信息
300381:溢多利
300382:斯莱克
300383:光环新网
300384:三联虹普
300385:雪浪环境
300386:飞天诚信
300387:富邦股份
300388:国祯环保
300389:艾比森
300390:天华超净
300391:康跃科技
300392:腾信股份
300393:中来股份
300394:天孚通信
300395:菲利华
300396:迪瑞医疗
300397:天和防务
300398:飞凯材料
300399:京天利
300400:劲拓股份
300401:花园生物
300402:宝色股份
300403:地尔汉宇
300404:博济医药
300405:科隆精化
300406:九强生物
300407:凯发电气
300408:三环集团
300409:道氏技术
300410:正业科技
300411:金盾股份
300412:迦南科技
300413:快乐购
300414:中光防雷
300415:伊之密
300416:苏试试验
300417:南华仪器
300418:昆仑万维
300419:浩丰科技
300420:五洋科技
300421:力星股份
300422:博世科
300423:鲁亿通
300424:航新科技
300425:环能科技
300426:唐德影视
300427:红相电力
300428:四通新材
300429:强力新材
300430:诚益通
300431:暴风集团
300432:富临精工
300433:蓝思科技
300434:金石东方
300435:中泰股份
300436:广生堂
300437:清水源
300438:鹏辉能源
300439:美康生物
300440:运达科技
300441:鲍斯股份
300442:普丽盛
300443:金雷风电
300444:双杰电气
300445:康斯特
300446:乐凯新材
300447:全信股份
300448:浩云科技
300449:汉邦高科
300450:先导智能
300451:创业软件
300452:山河药辅
300453:三鑫医疗
300455:康拓红外
300456:耐威科技
300457:赢合科技
300458:全志科技
300459:浙江金科
300460:惠伦晶体
300461:田中精机
300462:华铭智能
300463:迈克生物
300464:星徽精密
300465:高伟达
300466:赛摩电气
300467:迅游科技
300468:四方精创
300469:信息发展
300470:日机密封
300471:厚普股份
300472:新元科技
300473:德尔股份
300474:景嘉微
300475:聚隆科技
300476:胜宏科技
300477:合纵科技
300478:杭州高新
300479:神思电子
300480:光力科技
300481:濮阳惠成
300482:万孚生物
300483:沃施股份
300484:蓝海华腾
300485:赛升药业
300486:东杰智能
300487:蓝晓科技
300488:恒锋工具
300489:中飞股份
300490:华自科技
300491:通合科技
300492:山鼎设计
300493:润欣科技
300494:盛天网络
300495:美尚生态
300496:中科创达
300497:富祥股份
300498:温氏股份
300499:高澜股份
300500:苏州设计
300501:海顺新材
300502:新易盛
300503:昊志机电
300505:川金诺
300506:名家汇
300507:苏奥传感
300508:维宏股份
300509:新美星
300510:金冠电气
300511:雪榕生物
300512:中亚股份
300513:恒泰实达
300515:三德科技
300516:久之洋
300517:海波重科
300518:盛讯达
300519:新光药业
300520:科大国创
300521:爱司凯
300522:世名科技
300527:华舟应急
395001:主板Ａ股
395002:主板Ｂ股
395003:中小板
395004:创业板
395011:封闭基金
395012:ＬＯＦｓ
395013:ＥＴＦｓ
395014:分级基金
395021:可 转 债
395022:企业债
395024:公司债
395031:国债
395032:债券回购
395041:股票权证
395099:总 成 交
399001:深证成指
399002:深成指R
399003:成份Ｂ指
399004:深证100R
399005:中小板指
399006:创业板指
399007:深证300
399008:中小300
399009:深证200
399010:深证700
399011:深证1000
399012:创业300
399013:深市精选
399015:中小创新
399100:新 指 数
399101:中小板综
399102:创业板综
399103:乐富指数
399106:深证综指
399107:深证Ａ指
399108:深证Ｂ指
399231:农林指数
399232:采矿指数
399233:制造指数
399234:水电指数
399235:建筑指数
399236:批零指数
399237:运输指数
399238:餐饮指数
399239:IT指数
399240:金融指数
399241:地产指数
399242:商务指数
399243:科研指数
399244:公共指数
399248:文化指数
399249:综企指数
399298:深信中高
399299:深信中低
399300:沪深300
399301:深信用债
399302:深公司债
399303:国证2000
399305:基金指数
399306:深证ETF
399307:深证转债
399310:国证50
399311:国证1000
399312:国证300
399313:巨潮100
399314:巨潮大盘
399315:巨潮中盘
399316:巨潮小盘
399317:国证Ａ指
399318:国证Ｂ指
399319:资源优势
399320:国证服务
399321:国证红利
399322:国证治理
399324:深证红利
399326:成长40
399328:深证治理
399330:深证100
399332:深证创新
399333:中小板R
399335:深证央企
399337:深证民营
399339:深证科技
399341:深证责任
399344:深证300R
399346:深证成长
399348:深证价值
399350:皖江30
399351:深报指数
399352:深报综指
399353:国证物流
399354:分析师指数
399355:长三角
399356:珠三角
399357:环渤海
399358:泰达指数
399359:国证基建
399360:新硬件
399361:国证商业
399362:国证民营
399363:计算机指
399364:中金消费
399365:国证农业
399366:国证大宗
399367:巨潮地产
399368:国证军工
399369:CBN-兴全
399370:国证成长
399371:国证价值
399372:大盘成长
399373:大盘价值
399374:中盘成长
399375:中盘价值
399376:小盘成长
399377:小盘价值
399378:南方低碳
399379:国证基金
399380:国证ETF
399381:1000能源
399382:1000材料
399383:1000工业
399384:1000可选
399385:1000消费
399386:1000医药
399387:1000金融
399388:1000信息
399389:国证通信
399390:1000公用
399391:投资时钟
399392:国证新兴
399393:国证地产
399394:国证医药
399395:国证有色
399396:国证食品
399397:OCT文化
399398:绩效指数
399399:中经GDP
399400:大中盘
399401:中小盘
399402:周期100
399403:防御100
399404:大盘低波
399405:大盘高贝
399406:中盘低波
399407:中盘高贝
399408:小盘低波
399409:小盘高贝
399410:苏州率先
399411:红利100
399412:国证新能
399413:国证转债
399415:I100
399416:I300
399417:新能源车
399418:国证国安
399419:国证高铁
399420:国证保证
399423:中关村50
399427:专利领先
399428:国证定增
399429:新丝路
399431:国证银行
399432:国证汽车
399433:国证交运
399434:国证传媒
399435:国证农牧
399436:国证煤炭
399437:国证证券
399438:国证电力
399439:国证油气
399440:国证钢铁
399441:生物医药
399481:企债指数
399550:央视50
399551:央视创新
399552:央视成长
399553:央视回报
399554:央视治理
399555:央视责任
399556:央视生态
399557:央视文化
399602:中小成长
399604:中小价值
399606:创业板R
399608:科技100
399610:TMT50
399611:中创100R
399612:中创100
399613:深证能源
399614:深证材料
399615:深证工业
399616:深证可选
399617:深证消费
399618:深证医药
399619:深证金融
399620:深证信息
399621:深证电信
399622:深证公用
399623:中小基础
399624:中创400
399625:中创500
399626:中创成长
399627:中创价值
399628:700成长
399629:700价值
399630:1000成长
399631:1000价值
399632:深100EW
399633:深300EW
399634:中小板EW
399635:创业板EW
399636:深证装备
399637:深证地产
399638:深证环保
399639:深证大宗
399640:创业基础
399641:深证新兴
399642:中小新兴
399643:创业新兴
399644:深证时钟
399645:100低波
399646:深消费50
399647:深医药50
399648:深证GDP
399649:中小红利
399650:中小治理
399651:中小责任
399652:中创高新
399653:深证龙头
399654:深证文化
399655:深证绩效
399656:100绩效
399657:300绩效
399658:中小绩效
399659:深成指EW
399660:中创EW
399661:深证低波
399662:深证高贝
399663:中小低波
399664:中小高贝
399665:中创低波
399666:中创高贝
399667:创业板G
399668:创业板V
399669:深证农业
399670:深周期50
399671:深防御50
399672:深红利50
399673:创业板50
399674:深A医药
399675:深互联网
399676:深医药EW
399677:深互联EW
399678:深次新股
399679:深证200R
399680:深成能源
399681:深成材料
399682:深成工业
399683:深成可选
399684:深成消费
399685:深成医药
399686:深成金融
399687:深成信息
399688:深成电信
399689:深成公用
399690:中小专利
399691:创业专利
399693:安防产业
399701:深证F60
399702:深证F120
399703:深证F200
399704:深证上游
399705:深证中游
399706:深证下游
399707:CSSW证券
399802:500深市
399803:工业4.0
399804:中证体育
399805:互联金融
399806:环境治理
399807:高铁产业
399808:中证新能
399809:保险主题
399810:CSSW传媒
399811:CSSW电子
399812:养老产业
399813:中证国安
399814:大农业
399817:生态100
399901:小康指数
399903:中证100
399904:中证 200
399905:中证 500
399908:300 能源
399909:300 材料
399910:300 工业
399911:300 可选
399912:300 消费
399913:300 医药
399914:300 金融
399917:300 公用
399918:300 成长
399919:300 价值
399922:中证红利
399925:基本面50
399928:中证能源
399931:中证可选
399932:中证消费
399933:中证医药
399934:中证金融
399935:中证信息
399939:民企200
399944:内地资源
399950:300基建
399951:300银行
399952:300地产
399957:300运输
399958:创业成长
399959:军工指数
399961:中证上游
399963:中证下游
399964:中证新兴
399965:800地产
399966:800非银
399967:中证军工
399969:300非周
399970:移动互联
399971:中证传媒
399972:300深市
399973:中证国防
399974:国企改革
399975:证券公司
399976:CS新能车
399977:内地低碳
399978:医药100
399979:大宗商品
399982:500等权
399983:地产等权
399986:中证银行
399987:中证酒
399989:中证医疗
399990:煤炭等权
399991:一带一路
399992:CSWD并购
399993:CSWD生科
399994:信息安全
399995:基建工程
399996:智能家居
399997:中证白酒
399998:中证煤炭
06808:高鑫零售
00151:中国旺旺
01033:中石化油服
00489:东风集团股份
00719:山东新华制药股份
01169:海尔电器
00709:佐丹奴国际
00941:中国移动
00322:康师傅控股
03969:中国通号
00291:华润啤酒
00220:统一企业中国
01044:恒安国际
01347:华虹半导体
00933:光汇石油
00330:思捷环球
00002:中电控股
00607:丰盛控股
01136:台泥国际集团
02678:天虹纺织
00867:康哲药业
02328:中国财险
03311:中国建筑国际
03308:金鹰商贸集团
01606:国银租赁
01302:先健科技
00885:仁天科技控股
00699:神州租车
00680:南海控股
00468:纷美包装
00011:恒生银行
00019:太古股份公司Ａ
00006:电能实业
00116:周生生
01658:邮储银行
00008:电讯盈科
00836:华润电力
02877:神威药业
00853:微创医疗
01030:新城发展控股
01636:中国金属利用
00315:数码通电讯
00341:大家乐集团
02199:维珍妮
00023:东亚银行
03908:中金公司
00551:裕元集团
01196:伟禄集团
00142:第一太平
00198:星美控股
00101:恒隆地产
01052:越秀交通基建
03698:徽商银行
03380:龙光地产
00548:深圳高速公路股份
01112:Ｈ＆Ｈ国际控股
00256:冠城钟表珠宝
01115:西藏水资源
02238:广汽集团
02298:都市丽人
00976:齐合环保
00883:中国海洋石油
00386:中国石油化工股份
00316:东方海外国际
01666:同仁堂科技
00874:白云山
00525:广深铁路股份
01513:丽珠医药
00728:中国电信
00003:香港中华煤气
01886:汇源果汁
02386:中石化炼化工程
00384:中国燃气
01883:中信国际电讯
00586:海螺创业
00345:VITASOY INT'L
00511:电视广播
01458:周黑鸭
00368:中外运航运
01811:中广核新能源
01816:中广核电力
00257:中国光大国际
00279:民众金融科技
02319:蒙牛乳业
01310:香港宽频
00321:德永佳集团
01988:民生银行
01999:敏华控股
00494:利丰
00012:恒基地产
00737:合和公路基建
00493:国美电器
00083:信和置业
00014:希慎兴业
00939:建设银行
00042:东北电气
01628:禹洲地产
01528:红星美凯龙
01635:大众公用
03800:保利协鑫能源
00968:信义光能
03396:联想控股
01766:中国中车
00010:恒隆集团
02111:超盈国际控股
03823:德普科技
00245:中国民生金融
00669:创科实业
01238:宝龙地产
00168:青岛啤酒股份
02329:国瑞置业
01038:长江基建集团
00857:中国石油股份
02607:上海医药
01211:比亚迪股份
00016:新鸿基地产
03331:维达国际
01071:华电国际电力股份
00992:联想集团
00590:六福集团
01610:中粮肉食
01972:太古地产
03988:中国银行
00308:香港中旅
01970:IMAX CHINA
00598:中国外运
01432:中国圣牧
00787:利标品牌
00579:京能清洁能源
01117:现代牧业
01288:农业银行
00636:嘉里物流
00178:莎莎国际
00371:北控水务集团
01958:北京汽车
01363:中滔环保
01060:阿里影业
00996:嘉年华国际
00270:粤海投资
03799:达利食品
00868:信义玻璃
00004:九龙仓集团
01382:互太纺织
01199:中远海运港口
02768:佳源国际控股
00659:新创建集团
00293:国泰航空
03998:波司登
02380:中国电力
00173:嘉华国际
01963:重庆银行
00895:东江环保
03958:东方证券
00995:安徽皖通高速公路
00017:新世界发展
02356:大新银行集团
01308:海丰国际
00811:新华文轩
00658:中国高速传动
00069:香格里拉（亚洲）
03328:交通银行
01800:中国交通建设
01193:华润燃气
01299:友邦保险
00775:长江生命科技
00683:嘉里建设
00553:南京熊猫电子股份
00038:第一拖拉机股份
01381:粤丰环保
00215:和记电讯香港
00041:鹰君
01778:彩生活
01186:中国铁建
00267:中信股份
06178:光大证券
06066:中信建投证券
01109:华润置地
00317:中船防务
00392:北京控股
00576:浙江沪杭甬
00902:华能国际电力股份
00390:中国中铁
00656:复星国际
00107:四川成渝高速公路
01113:长实地产
02314:理文造纸
02196:复星医药
02777:富力地产
01368:特步国际
00338:上海石油化工股份
00177:江苏宁沪高速公路
03818:中国动向
03933:联邦制药
03618:重庆农村商业银行
01099:国药控股
00916:龙源电力
02202:万科企业
00751:创维数码
01980:天鸽互动
02601:中国太保
01776:广发证券
01234:中国利郎
01530:三生制药
00855:中国水务
00001:长和
02888:渣打集团
01072:东方电气
01848:中国飞机租赁
00005:汇丰控股
02880:大连港
01230:雅士利国际
00378:五龙动力
00035:远东发展
01361:３６１度
01966:中骏置业
01375:中州证券
03808:中国重汽
06881:中国银河
01813:合景泰富
01339:中国人民保险集团
01508:中国再保险
01387:人和商业
00179:德昌电机控股
00425:敏实集团
01818:招金矿业
03320:华润医药
06099:招商证券
03301:融信中国
02588:中银航空租赁
02388:中银香港
02333:长城汽车
02313:申洲国际
00570:中国中药
00934:中石化冠德
02611:国泰君安
00165:中国光大控股
02338:潍柴动力
01398:工商银行
01928:金沙中国有限公司
00998:中信银行
00813:世茂房地产
01929:周大福
00546:阜丰集团
00914:海螺水泥
00777:网龙
00410:ＳＯＨＯ中国
01357:美图公司
06818:中国光大银行
01316:耐世特
02318:中国平安
01448:福寿园
00288:万洲国际
06837:海通证券
01515:华润凤凰医疗
00688:中国海外发展
00884:旭辉控股集团
03898:中车时代电气
02666:环球医疗
01282:中国金洋
02869:绿城服务
02727:上海电气
00819:天能动力
01066:威高股份
00606:中国粮油控股
00604:深圳控股
02233:西部水泥
01177:中国生物制药
00753:中国国航
00588:北京北辰实业股份
01070:ＴＣＬ多媒体
00440:大新金融
02186:绿叶制药
02883:中海油田服务
01618:中国中冶
00272:瑞安房地产
01828:大昌行集团
06030:中信证券
00966:中国太平
00735:中国电力清洁能源
01317:枫叶教育
00152:深圳国际
00439:光启科学
01128:永利澳门
00880:澳博控股
00732:信利国际
00563:上实城市开发
00960:龙湖地产
00670:中国东方航空股份
00991:大唐发电
00981:中芯国际
03377:远洋集团
03360:远东宏信
02005:石四药集团
02128:中国联塑
00135:昆仑能源
00694:北京首都机场股份
00363:上海实业控股
02688:新奥能源
00388:香港交易所
01108:洛阳玻璃股份
01888:建滔积层板
00522:ASM PACIFIC
02357:中航科工
00144:招商局港口
03968:招商银行
00242:信德集团
03382:天津港发展
00419:华谊腾讯娱乐
01055:中国南方航空股份
00095:绿景中国地产
03333:中国恒大
00762:中国联通
02628:中国人寿
00354:中国软件国际
00806:惠理集团
00506:中国食品
00175:吉利汽车
02689:玖龙纸业
00665:海通国际
02282:美高梅中国
02020:安踏体育
01882:海天国际
02208:金风科技
00027:银河娱乐
06886:HTSC
00020:会德丰
00552:中国通信服务
01065:天津创业环保股份
02038:富智康集团
00639:首钢资源
03899:中集安瑞科
01057:浙江世宝
00123:越秀地产
00081:中国海外宏洋集团
02009:金隅股份
02331:李宁
01668:华南城
00303:VTECH HOLDINGS
02899:紫金矿业
01083:港华燃气
00958:华能新能源
00268:金蝶国际
00066:港铁公司
01680:澳门励骏
00696:中国民航信息网络
00400:科通芯城
00978:招商局置地
02018:瑞声科技
01138:中远海能
00460:四环医药
02382:舜宇光学科技
00698:通达集团
01359:中国信达
00951:超威动力
00451:协鑫新能源
01313:华润水泥控股
01088:中国神华
03606:福耀玻璃
00817:中国金茂
01788:国泰君安国际
00700:腾讯控股
01336:新华保险
00148:建滔化工
03888:金山软件
00564:郑煤机
00297:中化化肥
00336:华宝国际
00163:英皇国际
02866:中远海发
01093:石药集团
03323:中国建材
00861:神州控股
00729:五龙电动车
02007:碧桂园
02039:中集集团
01315:允升国际
01333:中国忠旺
01918:融创中国
03339:中国龙工
01898:中煤能源
00547:数字王国
01812:晨鸣纸业
03383:雅居乐集团
01114:BRILLIANCE CHI
00119:保利置业集团
00358:江西铜业股份
01777:花样年控股
00763:中兴通讯
01157:中联重科
00721:中国金融国际
01608:伟能集团
01919:中远海控
03900:绿城中国
00241:阿里健康
01205:中信资源
03886:康健国际医疗
00337:绿地香港
00881:中升控股
00200:新濠国际发展
03836:和谐汽车
03813:宝胜国际
01171:兖州煤业股份
00799:IGG
01293:广汇宝信
00285:比亚迪电子
00327:百富环球
03993:洛阳钼业
00323:马鞍山钢铁股份
00347:鞍钢股份
01728:正通汽车
02600:中国铝业
01208:五矿资源
00921:海信科龙
00931:中国天然气";
        #endregion
    }
}
