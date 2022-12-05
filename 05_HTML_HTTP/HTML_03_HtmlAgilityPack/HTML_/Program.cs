using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace HTML_
{
    class Program
    {
        static void Main(string[] args)
        {
            DoSomething();
        }

        private static void DoSomething()
        {
            VesselModel vessel = ScrapSingleVesselAsync(312869000, 8741325);
        }

        private static VesselModel ScrapSingleVesselAsync(int mmsi, int imo)
        {
            try
            {
                VesselModel vessel = new VesselModel();
                HtmlDocument doc;
                HtmlWeb web = new HtmlWeb
                {
                    OverrideEncoding = Encoding.UTF8,
                    UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3703.0 Safari/537.36"
                };

                //VF vessel page
                doc = web.Load("url" + mmsi);

                string detailedTypeVF = "";
                string vesselImoVF = "";
                string vesselMmsiVF = "";

                HtmlNode subtitle = doc.DocumentNode.SelectSingleNode("/html/body/div[1]/div/main/div/div[1]/div/div/h2");
                if (subtitle != null && subtitle.OuterHtml.ToString().Contains("<h2 class=\"subtitle\">"))
                {
                    string subtitleText = subtitle.OuterHtml.ToString().Split(new string[] { "<h2 class=\"subtitle\">" }, StringSplitOptions.None)[1].Trim();
                    if (subtitleText.Contains(",")) detailedTypeVF = System.Net.WebUtility.HtmlDecode(subtitleText.Split(',')[0]);
                    if (subtitleText.Contains("IMO: ")) vesselImoVF = System.Net.WebUtility.HtmlDecode(subtitleText.Split(new string[] { "IMO: " }, StringSplitOptions.None)[1].Split(',')[0]).Trim();
                    if (subtitleText.Contains("MMSI: ")) vesselMmsiVF = System.Net.WebUtility.HtmlDecode(subtitleText.Split(new string[] { "MMSI: " }, StringSplitOptions.None)[1].Split('<')[0]).Trim();
                }

                string aisTypeVF = ""; //1
                string flagVF = ""; //2
                string destinationVF = ""; //3
                string etaVF = ""; //4
                //string imo_mmsi = ""; //5
                string callsignVF = ""; //6
                string loa_beamVF = ""; //7
                string draughtVF = ""; //8
                string course_speedVF = ""; //9
                string lat_lonVF = ""; //10

                for (int i = 1; i < 11; i++)
                {
                    HtmlNode row = doc.DocumentNode.SelectSingleNode("/html/body/div[1]/div/main/div/section[2]/div/div[1]/table/tbody/tr[" + i + "]/td[2]");
                    if (row != null && row.OuterHtml.ToString().Contains("<td class=\"v3\">"))
                    {
                        string rowText = row.OuterHtml.ToString().Split(new string[] { "<td class=\"v3\">" }, StringSplitOptions.None)[1].Split('<')[0];

                        switch (i)
                        {
                            case 1:
                                aisTypeVF = System.Net.WebUtility.HtmlDecode(rowText).Trim();
                                break;
                            case 2:
                                flagVF = System.Net.WebUtility.HtmlDecode(rowText).Trim();
                                break;
                            case 3:
                                destinationVF = System.Net.WebUtility.HtmlDecode(rowText).Trim();
                                break;
                            case 4:
                                etaVF = System.Net.WebUtility.HtmlDecode(rowText).Trim();
                                break;
                            case 6:
                                callsignVF = System.Net.WebUtility.HtmlDecode(rowText).Trim();
                                break;
                            case 7:
                                loa_beamVF = System.Net.WebUtility.HtmlDecode(rowText).Trim();
                                break;
                            case 8:
                                draughtVF = System.Net.WebUtility.HtmlDecode(rowText).Trim();
                                break;
                            case 9:
                                course_speedVF = System.Net.WebUtility.HtmlDecode(rowText).Trim();
                                break;
                            case 10:
                                lat_lonVF = System.Net.WebUtility.HtmlDecode(rowText).Trim();
                                break;
                            default:
                                break;
                        }
                    }
                }

                etaVF = ParseEta(etaVF);
                draughtVF = draughtVF.Replace(" m", "");

                string aisFeed = "";
                HtmlNode feedTime = doc.DocumentNode.SelectSingleNode("/html/body/div[1]/div/main/div/section[2]/div/div[1]/table/tbody/tr[11]/td[2]");
                if (feedTime != null && feedTime.OuterHtml.ToString().Contains(" UTC") && feedTime.OuterHtml.ToString().Contains("<td class=\"v3 tooltip expand\" data-title=\""))
                {
                    aisFeed = feedTime.OuterHtml.ToString().Split(new string[] { "<td class=\"v3 tooltip expand\" data-title=\"" }, StringSplitOptions.None)[1]
                        .Split(new string[] { " UTC" }, StringSplitOptions.None)[0].Trim();
                    aisFeed = System.Net.WebUtility.HtmlDecode(aisFeed);
                }

                string loaVF = "";
                string beamVF = "";
                string courseVF = "";
                string speedVF = "";
                string lat = "";
                string lon = "";

                if (loa_beamVF != "-" && !string.IsNullOrEmpty(loa_beamVF))
                {
                    loaVF = loa_beamVF.Split('/')[0].Trim();
                    beamVF = loa_beamVF.Split('/')[1].Replace(" m", "").Trim();
                }
                if (course_speedVF != "-" && !string.IsNullOrEmpty(course_speedVF))
                {
                    courseVF = course_speedVF.Split('°')[0].Trim();
                    speedVF = course_speedVF.Split('/')[1].Split('k')[0].Trim();
                }
                if (lat_lonVF != "-" && !string.IsNullOrEmpty(lat_lonVF))
                {
                    lat = lat_lonVF.Split('/')[0];
                    lon = lat_lonVF.Split('/')[1];
                }
                if (lat.Contains("N")) lat = lat.Split(' ')[0].Trim(); else if (lat.Contains("S")) lat = "-" + lat.Split(' ')[0].Trim();
                if (lon.Contains("E")) lon = lon.Split(' ')[0].Trim(); else if (lon.Contains("W")) lon = "-" + lon.Split(' ')[0].Trim();

                string grtVF = "";
                string dwtVF = "";
                string yobVF = "";

                for (int i = 1; i < 12; i++)
                {
                    HtmlNode row = doc.DocumentNode.SelectSingleNode("/html/body/div[1]/div/main/div/section[3]/table/tbody/tr[" + i + "]/td[2]");

                    if (row != null && row.OuterHtml.ToString().Contains("<td class=\"v3\">"))
                    {
                        string rowText = row.OuterHtml.ToString().Split(new string[] { "<td class=\"v3\">" }, StringSplitOptions.None)[1].Split('<')[0];

                        switch (i)
                        {
                            case 6:
                                grtVF = System.Net.WebUtility.HtmlDecode(rowText).Trim();
                                break;
                            case 7:
                                dwtVF = System.Net.WebUtility.HtmlDecode(rowText).Trim();
                                break;
                            case 11:
                                yobVF = System.Net.WebUtility.HtmlDecode(rowText).Trim();
                                break;
                            default:
                                break;
                        }
                    }
                }

                //SP vessel page
                doc = web.Load("url" + imo);

                string detailedTypeSP = "";

                HtmlNode detailedType = doc.DocumentNode.SelectSingleNode("/html/body/center/table");

                if (detailedType != null && detailedType.OuterHtml.ToString().Contains("Vessel type:") && detailedType.OuterHtml.ToString().Contains("<br>"))
                {
                    detailedTypeSP = detailedType.OuterHtml.ToString().Split(new string[] { "<b>Vessel type:</b>" }, StringSplitOptions.None)[1].Split(new string[] { "<br>" }, StringSplitOptions.None)[0].Trim();
                    detailedTypeSP = System.Net.WebUtility.HtmlDecode(detailedTypeSP);
                }

                //VT vessel page
                doc = web.Load("url" + imo + ".html");

                string naviStatusVT = "";

                HtmlNode naviStatus = doc.DocumentNode.SelectSingleNode("/html/body");

                if (naviStatus != null && naviStatus.OuterHtml.ToString().Contains("Navigational status:"))
                {
                    naviStatusVT = naviStatus.OuterHtml.ToString().Split(new string[] { "Navigational status:</div><div class=" }, StringSplitOptions.None)[1].Split('>')[1].Split('<')[0].Trim();
                    naviStatusVT = System.Net.WebUtility.HtmlDecode(naviStatusVT);
                }

                //PARSE

                int integer;
                double d;
                DateTime date;
                List<string> detailedTypes = new List<string>();

                vessel.IMO = imo;
                vessel.MMSI = mmsi;
                if (!string.IsNullOrEmpty(detailedTypeSP)) detailedTypes.Add(detailedTypeSP.ToLower());
                if (!string.IsNullOrEmpty(detailedTypeVF)) detailedTypes.Add(detailedTypeVF.ToLower());
                if (!string.IsNullOrEmpty(aisTypeVF)) vessel.AisVesselType = aisTypeVF;
                if (!string.IsNullOrEmpty(naviStatusVT)) vessel.AISStatus = naviStatusVT;
                if (!string.IsNullOrEmpty(destinationVF)) vessel.Destination = destinationVF;
                if (!string.IsNullOrEmpty(callsignVF)) vessel.CallSign = callsignVF;
                if (!string.IsNullOrEmpty(flagVF)) vessel.Flag = flagVF;
                if (!string.IsNullOrEmpty(grtVF)) if (int.TryParse(grtVF, out integer)) vessel.GRT = int.Parse(grtVF);
                if (!string.IsNullOrEmpty(dwtVF)) if (int.TryParse(dwtVF, out integer)) vessel.DWT = int.Parse(dwtVF);
                if (!string.IsNullOrEmpty(loaVF))
                    if (double.TryParse(loaVF, NumberStyles.Number, CultureInfo.CreateSpecificCulture("en-US"), out d))
                        vessel.LOA = double.Parse(loaVF, CultureInfo.InvariantCulture);
                if (!string.IsNullOrEmpty(beamVF))
                    if (double.TryParse(beamVF, NumberStyles.Number, CultureInfo.CreateSpecificCulture("en-US"), out d))
                        vessel.Breadth = double.Parse(beamVF, CultureInfo.InvariantCulture);
                if (!string.IsNullOrEmpty(lat))
                    if (double.TryParse(lat, NumberStyles.Number, CultureInfo.CreateSpecificCulture("en-US"), out d))
                        vessel.Lat = double.Parse(lat, CultureInfo.InvariantCulture);
                if (!string.IsNullOrEmpty(lon))
                    if (double.TryParse(lon, NumberStyles.Number, CultureInfo.CreateSpecificCulture("en-US"), out d))
                        vessel.Lon = double.Parse(lon, CultureInfo.InvariantCulture);
                if (!string.IsNullOrEmpty(speedVF))
                    if (double.TryParse(speedVF, NumberStyles.Number, CultureInfo.CreateSpecificCulture("en-US"), out d))
                        vessel.Speed = double.Parse(speedVF, CultureInfo.InvariantCulture);
                if (!string.IsNullOrEmpty(courseVF))
                    if (double.TryParse(courseVF, NumberStyles.Number, CultureInfo.CreateSpecificCulture("en-US"), out d))
                        vessel.Course = double.Parse(courseVF, CultureInfo.InvariantCulture);
                if (!string.IsNullOrEmpty(draughtVF))
                    if (double.TryParse(draughtVF, NumberStyles.Number, CultureInfo.CreateSpecificCulture("en-US"), out d))
                        vessel.Draught = double.Parse(draughtVF, CultureInfo.InvariantCulture);
                if (!string.IsNullOrEmpty(aisFeed))
                    if (DateTime.TryParse(aisFeed, out date))
                        vessel.AISLatestActivity = DateTime.Parse(aisFeed);
                if (!string.IsNullOrEmpty(etaVF))
                    if (DateTime.TryParse(etaVF, out date))
                        vessel.ETA = DateTime.Parse(etaVF);
                if (detailedTypes.Count > 0) vessel.DetailedType = detailedTypes.Distinct().ToArray();
                vessel.AISStatus = VerifyAisStatus(vessel.AISStatus, vessel.Speed);

                return vessel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static string ParseEta(string etaVF)
        {
            string month = etaVF.Split(' ')[0].Trim();
            switch (month)
            {
                case "Jan":
                    etaVF = etaVF.Split(' ')[1].Replace(",", " 01");
                    break;
                case "Feb":
                    etaVF = etaVF.Split(' ')[1].Replace(",", " 02");
                    break;
                case "Mar":
                    etaVF = etaVF.Split(' ')[1].Replace(",", " 03");
                    break;
                case "Apr":
                    etaVF = etaVF.Split(' ')[1].Replace(",", " 04");
                    break;
                case "May":
                    etaVF = etaVF.Split(' ')[1].Replace(",", " 05");
                    break;
                case "Jun":
                    etaVF = etaVF.Split(' ')[1].Replace(",", " 06");
                    break;
                case "Jul":
                    etaVF = etaVF.Split(' ')[1].Replace(",", " 07");
                    break;
                case "Aug":
                    etaVF = etaVF.Split(' ')[1].Replace(",", " 08");
                    break;
                case "Sep":
                    etaVF = etaVF.Split(' ')[1].Replace(",", " 09");
                    break;
                case "Oct":
                    etaVF = etaVF.Split(' ')[1].Replace(",", " 10");
                    break;
                case "Nov":
                    etaVF = etaVF.Split(' ')[1].Replace(",", " 11");
                    break;
                case "Dec":
                    etaVF = etaVF.Split(' ')[1].Replace(",", " 12");
                    break;
                default:
                    break;
            }

            return etaVF;
        }

        private static string VerifyAisStatus(string aISStatus, double? speed)
        {
            string finalStatus = aISStatus;

            if (speed != null)
            {
                if ((aISStatus == "Moving" || aISStatus == "Sailing" || aISStatus == "Fishing") && speed.Value < 0.5)
                {
                    aISStatus = "Not Moving";
                }
                else if ((aISStatus == "Moored" || aISStatus == "Anchored") && speed.Value > 0.5)
                {
                    aISStatus = "Moving";
                }
                else return aISStatus;
            }

            return aISStatus;
        }
    }
}
