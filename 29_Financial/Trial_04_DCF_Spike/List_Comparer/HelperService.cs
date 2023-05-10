﻿using System;
using System.Collections.Generic;

namespace List_Comparer
{
    internal class HelperService
    {
        public HelperService() { }

        public Dictionary<string, decimal> GetIndexComponents()
        {
            return new Dictionary<string, decimal>()
            {
                { "AAPL", 7.08145M },
                { "MSFT", 6.194952M },
                { "AMZN", 2.639443M },
                { "NVDA", 1.91415M },
                { "GOOGL", 1.81879M },
                { "BRK.B", 1.681112M },
                { "GOOG", 1.59227M },
                { "TSLA", 1.449605M },
                { "META", 1.403751M },
                { "XOM", 1.361118M },
                { "UNH", 1.360649M },
                { "JNJ", 1.248174M },
                { "JPM", 1.181919M },
                { "V", 1.093291M },
                { "PG", 1.026882M },
                { "MA", 0.906303M },
                { "CVX", 0.876197M },
                { "HD", 0.867573M },
                { "LLY", 0.846214M },
                { "MRK", 0.840285M },
                { "ABBV", 0.821403M },
                { "AVGO", 0.75548M },
                { "PEP", 0.732046M },
                { "KO", 0.711745M },
                { "PFE", 0.666118M },
                { "TMO", 0.665232M },
                { "COST", 0.631584M },
                { "BAC", 0.610823M },
                { "MCD", 0.610614M },
                { "WMT", 0.604223M },
                { "CSCO", 0.595226M },
                { "CRM", 0.56792M },
                { "DIS", 0.528006M },
                { "ABT", 0.525153M },
                { "LIN", 0.514267M },
                { "ACN", 0.508081M },
                { "ADBE", 0.498566M },
                { "DHR", 0.483079M },
                { "VZ", 0.477573M },
                { "TXN", 0.470075M },
                { "CMCSA", 0.461731M },
                { "WFC", 0.4535M },
                { "NKE", 0.45297M },
                { "NEE", 0.449748M },
                { "PM", 0.44646M },
                { "RTX", 0.435281M },
                { "BMY", 0.43256M },
                { "NFLX", 0.426991M },
                { "ORCL", 0.423345M },
                { "AMD", 0.417562M },
                { "UPS", 0.40651M },
                { "T", 0.406499M },
                { "QCOM", 0.384331M },
                { "INTC", 0.383157M },
                { "AMGN", 0.381714M },
                { "HON", 0.380951M },
                { "COP", 0.372459M },
                { "SBUX", 0.358714M },
                { "LOW", 0.357224M },
                { "INTU", 0.356868M },
                { "UNP", 0.352719M },
                { "CAT", 0.337778M },
                { "MS", 0.336171M },
                { "ELV", 0.333287M },
                { "IBM", 0.333025M },
                { "BA", 0.332248M },
                { "GS", 0.331472M },
                { "SPGI", 0.33109M },
                { "PLD", 0.327037M },
                { "LMT", 0.320585M },
                { "MDT", 0.314779M },
                { "DE", 0.305015M },
                { "GE", 0.303249M },
                { "BLK", 0.301874M },
                { "GILD", 0.300521M },
                { "BKNG", 0.298667M },
                { "SYK", 0.284459M },
                { "AXP", 0.281502M },
                { "CVS", 0.280489M },
                { "AMT", 0.279746M },
                { "ADI", 0.276884M },
                { "C", 0.276881M },
                { "MDLZ", 0.276084M },
                { "NOW", 0.275269M },
                { "AMAT", 0.27159M },
                { "ISRG", 0.271505M },
                { "ADP", 0.260491M },
                { "TJX", 0.259215M },
                { "TMUS", 0.256861M },
                { "REGN", 0.253944M },
                { "PYPL", 0.252116M },
                { "MMC", 0.248528M },
                { "VRTX", 0.242802M },
                { "CB", 0.236668M },
                { "MO", 0.234613M },
                { "ZTS", 0.23414M },
                { "PGR", 0.232992M },
                { "SCHW", 0.231948M },
                { "SO", 0.226574M },
                { "CI", 0.222633M },
                { "DUK", 0.218037M },
                { "TGT", 0.215435M },
                { "FISV", 0.213933M },
                { "BSX", 0.213212M },
                { "SLB", 0.213056M },
                { "BDX", 0.209518M },
                { "EOG", 0.204091M },
                { "CME", 0.198836M },
                { "NOC", 0.197762M },
                { "MU", 0.196652M },
                { "AON", 0.196275M },
                { "LRCX", 0.19156M },
                { "EQIX", 0.188478M },
                { "ITW", 0.187199M },
                { "ETN", 0.186875M },
                { "HUM", 0.186837M },
                { "CSX", 0.184183M },
                { "APD", 0.182632M },
                { "CL", 0.182197M },
                { "WM", 0.177728M },
                { "ATVI", 0.17547M },
                { "ICE", 0.173909M },
                { "FCX", 0.173487M },
                { "MMM", 0.169805M },
                { "MPC", 0.169162M },
                { "EL", 0.169072M },
                { "CDNS", 0.168499M },
                { "SNPS", 0.166987M },
                { "HCA", 0.165364M },
                { "CCI", 0.16466M },
                { "ORLY", 0.161575M },
                { "SHW", 0.156776M },
                { "PXD", 0.156169M },
                { "FDX", 0.15344M },
                { "EW", 0.149588M },
                { "GD", 0.1488M },
                { "GIS", 0.148775M },
                { "KLAC", 0.148502M },
                { "PNC", 0.145147M },
                { "AZO", 0.144106M },
                { "F", 0.143553M },
                { "MCK", 0.143362M },
                { "USB", 0.142886M },
                { "EMR", 0.142556M },
                { "VLO", 0.142317M },
                { "CMG", 0.141252M },
                { "GM", 0.141224M },
                { "D", 0.140906M },
                { "MSI", 0.140177M },
                { "SRE", 0.14016M },
                { "PSX", 0.139552M },
                { "AEP", 0.137908M },
                { "NSC", 0.137655M },
                { "DG", 0.137562M },
                { "MCO", 0.137212M },
                { "MRNA", 0.136827M },
                { "ROP", 0.135148M },
                { "KMB", 0.13483M },
                { "APH", 0.134625M },
                { "PSA", 0.134141M },
                { "DXCM", 0.131848M },
                { "OXY", 0.13166M },
                { "MAR", 0.131406M },
                { "TFC", 0.130772M },
                { "NXPI", 0.130088M },
                { "ADM", 0.129911M },
                { "CTVA", 0.129202M },
                { "MCHP", 0.127153M },
                { "FTNT", 0.1263M },
                { "AJG", 0.124199M },
                { "MSCI", 0.123691M },
                { "ADSK", 0.122644M },
                { "EXC", 0.122191M },
                { "BIIB", 0.121418M },
                { "PH", 0.120277M },
                { "A", 0.119389M },
                { "ECL", 0.117074M },
                { "TT", 0.116924M },
                { "MET", 0.116683M },
                { "ANET", 0.116551M },
                { "HES", 0.116461M },
                { "TEL", 0.116223M },
                { "MNST", 0.115874M },
                { "DOW", 0.115195M },
                { "CTAS", 0.114986M },
                { "JCI", 0.114976M },
                { "IDXX", 0.114971M },
                { "TRV", 0.114069M },
                { "TDG", 0.112759M },
                { "HLT", 0.111453M },
                { "YUM", 0.111088M },
                { "O", 0.111081M },
                { "LHX", 0.111M },
                { "AIG", 0.110987M },
                { "NEM", 0.110865M },
                { "XEL", 0.110705M },
                { "SYY", 0.109958M },
                { "PCAR", 0.109954M },
                { "HSY", 0.109707M },
                { "CNC", 0.109033M },
                { "AFL", 0.10835M },
                { "IQV", 0.108293M },
                { "CARR", 0.108039M },
                { "COF", 0.107669M },
                { "NUE", 0.107664M },
                { "STZ", 0.107228M },
                { "WMB", 0.105914M },
                { "CHTR", 0.105081M },
                { "SPG", 0.10491M },
                { "ROST", 0.104393M },
                { "ILMN", 0.104066M },
                { "DVN", 0.103816M },
                { "WELL", 0.102292M },
                { "MTD", 0.10228M },
                { "PAYX", 0.101113M },
                { "KMI", 0.101062M },
                { "ED", 0.099923M },
                { "OTIS", 0.09914M },
                { "FIS", 0.099089M },
                { "ON", 0.097377M },
                { "EA", 0.096994M },
                { "CMI", 0.09549M },
                { "CPRT", 0.095451M },
                { "AMP", 0.095385M },
                { "VICI", 0.095041M },
                { "RMD", 0.095027M },
                { "PPG", 0.094962M },
                { "DD", 0.09393M },
                { "BK", 0.093251M },
                { "WBD", 0.092528M },
                { "PRU", 0.09219M },
                { "AME", 0.092135M },
                { "ROK", 0.091528M },
                { "PEG", 0.091457M },
                { "KHC", 0.090286M },
                { "CTSH", 0.090083M },
                { "KR", 0.089621M },
                { "DHI", 0.089572M },
                { "ODFL", 0.088829M },
                { "DLTR", 0.088765M },
                { "ENPH", 0.088097M },
                { "FAST", 0.088071M },
                { "ALL", 0.087655M },
                { "WEC", 0.087572M },
                { "HAL", 0.087363M },
                { "VRSK", 0.086485M },
                { "GEHC", 0.086403M },
                { "KDP", 0.086192M },
                { "GWW", 0.085785M },
                { "OKE", 0.085398M },
                { "BKR", 0.083979M },
                { "APTV", 0.083883M },
                { "AWK", 0.083322M },
                { "GPN", 0.083203M },
                { "SBAC", 0.081878M },
                { "RSG", 0.081662M },
                { "CSGP", 0.081543M },
                { "ZBH", 0.080574M },
                { "ANSS", 0.080372M },
                { "ES", 0.079434M },
                { "DLR", 0.079258M },
                { "EIX", 0.079197M },
                { "DFS", 0.078986M },
                { "KEYS", 0.078961M },
                { "ULTA", 0.078702M },
                { "PCG", 0.078097M },
                { "ABC", 0.077998M },
                { "LEN", 0.077508M },
                { "WST", 0.077324M },
                { "HPQ", 0.077034M },
                { "TSCO", 0.076765M },
                { "FANG", 0.076581M },
                { "URI", 0.076454M },
                { "GLW", 0.076207M },
                { "ACGL", 0.075502M },
                { "WBA", 0.074222M },
                { "WTW", 0.074087M },
                { "CDW", 0.073709M },
                { "TROW", 0.073572M },
                { "STT", 0.0731M },
                { "ALGN", 0.072548M },
                { "IT", 0.072478M },
                { "LYB", 0.070962M },
                { "IFF", 0.070664M },
                { "CEG", 0.070587M },
                { "AVB", 0.07005M },
                { "ALB", 0.069389M },
                { "EFX", 0.069266M },
                { "PWR", 0.068709M },
                { "FTV", 0.068513M },
                { "EBAY", 0.068347M },
                { "GPC", 0.067647M },
                { "WY", 0.067107M },
                { "AEE", 0.066745M },
                { "VMC", 0.065727M },
                { "CBRE", 0.065706M },
                { "IR", 0.065438M },
                { "PODD", 0.064425M },
                { "DAL", 0.063917M },
                { "ETR", 0.063874M },
                { "FE", 0.06359M },
                { "HIG", 0.063495M },
                { "MLM", 0.063264M },
                { "CHD", 0.063212M },
                { "DTE", 0.06292M },
                { "BAX", 0.062492M },
                { "FSLR", 0.062436M },
                { "MPWR", 0.062208M },
                { "MKC", 0.061661M },
                { "MTB", 0.061302M },
                { "PPL", 0.06055M },
                { "CAH", 0.060374M },
                { "EXR", 0.060326M },
                { "EQR", 0.060183M },
                { "HOLX", 0.060081M },
                { "DOV", 0.059817M },
                { "TDY", 0.05917M },
                { "LH", 0.059011M },
                { "HPE", 0.058809M },
                { "CTRA", 0.058525M },
                { "VRSN", 0.058099M },
                { "TTWO", 0.056828M },
                { "CLX", 0.056292M },
                { "OMC", 0.05616M },
                { "ARE", 0.056091M },
                { "CNP", 0.055584M },
                { "LUV", 0.055197M },
                { "INVH", 0.054842M },
                { "LVS", 0.054788M },
                { "XYL", 0.054359M },
                { "NDAQ", 0.054201M },
                { "FITB", 0.053953M },
                { "STE", 0.053702M },
                { "DRI", 0.053337M },
                { "RJF", 0.053257M },
                { "WAT", 0.053133M },
                { "COO", 0.052827M },
                { "WAB", 0.052185M },
                { "CMS", 0.051485M },
                { "NTRS", 0.051484M },
                { "TSN", 0.051442M },
                { "VTR", 0.051189M },
                { "RF", 0.051121M },
                { "EXPD", 0.050891M },
                { "CAG", 0.050834M },
                { "SWKS", 0.050825M },
                { "SEDG", 0.050762M },
                { "STLD", 0.05064M },
                { "FICO", 0.050484M },
                { "PFG", 0.050452M },
                { "MAA", 0.04998M },
                { "K", 0.049801M },
                { "TRGP", 0.049539M },
                { "PKI", 0.049485M },
                { "BR", 0.04944M },
                { "NVR", 0.049359M },
                { "MOH", 0.0492M },
                { "CINF", 0.04906M },
                { "EPAM", 0.048464M },
                { "HBAN", 0.048175M },
                { "AMCR", 0.047869M },
                { "IEX", 0.047666M },
                { "SJM", 0.047195M },
                { "FLT", 0.047126M },
                { "ATO", 0.047071M },
                { "DGX", 0.046875M },
                { "AES", 0.046831M },
                { "MOS", 0.046484M },
                { "BALL", 0.046141M },
                { "FDS", 0.045909M },
                { "HWM", 0.045394M },
                { "MRO", 0.045309M },
                { "LW", 0.045072M },
                { "FMC", 0.044895M },
                { "ZBRA", 0.044595M },
                { "IRM", 0.044559M },
                { "TER", 0.044111M },
                { "CF", 0.044032M },
                { "GRMN", 0.044026M },
                { "TYL", 0.043433M },
                { "PAYC", 0.042967M },
                { "J", 0.042799M },
                { "CFG", 0.042759M },
                { "IPG", 0.042563M },
                { "BBY", 0.042414M },
                { "NTAP", 0.042317M },
                { "JBHT", 0.042185M },
                { "AVY", 0.042113M },
                { "TXT", 0.041988M },
                { "CBOE", 0.041657M },
                { "BG", 0.041428M },
                { "RE", 0.041181M },
                { "EVRG", 0.04105M },
                { "LKQ", 0.041014M },
                { "BRO", 0.040513M },
                { "MGM", 0.040226M },
                { "PHM", 0.040172M },
                { "INCY", 0.04007M },
                { "EXPE", 0.039983M },
                { "UAL", 0.039854M },
                { "RCL", 0.039654M },
                { "PTC", 0.039591M },
                { "LNT", 0.039584M },
                { "ESS", 0.039305M },
                { "TECH", 0.038628M },
                { "PKG", 0.038574M },
                { "POOL", 0.037898M },
                { "AKAM", 0.037749M },
                { "SYF", 0.037731M },
                { "IP", 0.037685M },
                { "ETSY", 0.037194M },
                { "APA", 0.036974M },
                { "SNA", 0.036869M },
                { "MKTX", 0.036703M },
                { "WRB", 0.036554M },
                { "LDOS", 0.036405M },
                { "UDR", 0.036338M },
                { "STX", 0.035946M },
                { "TFX", 0.035232M },
                { "TRMB", 0.034782M },
                { "VTRS", 0.034631M },
                { "EQT", 0.034312M },
                { "HST", 0.034039M },
                { "DPZ", 0.033928M },
                { "PEAK", 0.033876M },
                { "CPT", 0.033824M },
                { "NDSN", 0.033824M },
                { "SWK", 0.033711M },
                { "KIM", 0.033607M },
                { "WYNN", 0.033509M },
                { "WDC", 0.033144M },
                { "KEY", 0.033108M },
                { "BWA", 0.033082M },
                { "BF.B", 0.033051M },
                { "HRL", 0.033037M },
                { "JKHY", 0.03291M },
                { "NI", 0.032812M },
                { "CHRW", 0.032429M },
                { "HSIC", 0.032311M },
                { "KMX", 0.032202M },
                { "CPB", 0.032036M },
                { "L", 0.031986M },
                { "PARA", 0.031959M },
                { "MAS", 0.031937M },
                { "CE", 0.031611M },
                { "JNPR", 0.03099M },
                { "TAP", 0.030681M },
                { "CRL", 0.030574M },
                { "CDAY", 0.030175M },
                { "FOXA", 0.02988M },
                { "GEN", 0.02966M },
                { "BIO", 0.029395M },
                { "MTCH", 0.02905M },
                { "EMN", 0.028756M },
                { "TPR", 0.028729M },
                { "GL", 0.028265M },
                { "CCL", 0.028156M },
                { "LYV", 0.027866M },
                { "QRVO", 0.027722M },
                { "CZR", 0.027021M },
                { "REG", 0.026851M },
                { "ALLE", 0.026605M },
                { "ROL", 0.02595M },
                { "PNW", 0.02556M },
                { "XRAY", 0.025391M },
                { "UHS", 0.025141M },
                { "PNR", 0.024978M },
                { "AOS", 0.024861M },
                { "FFIV", 0.024767M },
                { "AAL", 0.024562M },
                { "HII", 0.024094M },
                { "RHI", 0.024046M },
                { "NRG", 0.023687M },
                { "CTLT", 0.023576M },
                { "BBWI", 0.023168M },
                { "IVZ", 0.022413M },
                { "WRK", 0.022333M },
                { "BEN", 0.022067M },
                { "AAP", 0.022027M },
                { "BXP", 0.021402M },
                { "WHR", 0.021389M },
                { "VFC", 0.021362M },
                { "FRT", 0.02059M },
                { "SEE", 0.020126M },
                { "HAS", 0.019393M },
                { "NWSA", 0.019373M },
                { "GNRC", 0.018672M },
                { "AIZ", 0.01818M },
                { "OGN", 0.017397M },
                { "CMA", 0.016994M },
                { "DXC", 0.016904M },
                { "NCLH", 0.015389M },
                { "ALK", 0.015149M },
                { "MHK", 0.014597M },
                { "RL", 0.013948M },
                { "NWL", 0.013681M },
                { "DVA", 0.013392M },
                { "ZION", 0.013368M },
                { "FOX", 0.012513M },
                { "LNC", 0.009741M },
                { "FRC", 0.006882M },
                { "NWS", 0.006138M },
                { "DISH", 0.005774M },
            };
        }

        internal Dictionary<string, string> GetDcfUrlDictionary(List<CompanyModel> companyList, string apiKey)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            foreach (CompanyModel company in companyList)
            {
                dict.Add(company.Code, "https://financialmodelingprep.com/api/v3/discounted-cash-flow/" + company.Code + "?apikey=" + apiKey);
            }

            return dict;
        }
    }
}
