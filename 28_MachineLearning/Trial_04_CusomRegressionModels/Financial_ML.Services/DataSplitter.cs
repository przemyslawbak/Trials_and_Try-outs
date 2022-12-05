using Financial_ML.Models;
using Skender.Stock.Indicators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Financial_ML.Services
{
    public class DataSplitter : IDataSplitter
    {
        private List<DateTime> GetAllDates(List<Quote> quotesDax, List<Quote> quotesBrent)
        {
            List<DateTime> dates = new List<DateTime>();
            dates.AddRange(quotesDax.Select(p => p.Date).ToList());
            dates.AddRange(quotesBrent.Select(p => p.Date).ToList());

            return dates.Distinct().OrderBy(d => d).ToList();
        }

        public List<TotalQuote> GetAllTotalQuotes(List<Quote> quotesDax, List<Quote> quotesBrent)
        {
            List<DateTime> allDates = GetAllDates(quotesDax, quotesBrent);
            List<TotalQuote> allQuotes = new List<TotalQuote>();
            List<SmaResult> smaBrentList = quotesBrent.GetSma(5).ToList();
            List<SmaResult> smaDaxList = quotesDax.GetSma(5).ToList();
            float lastSmaBrent = 0;
            float lastSmaDax = 0;
            float lastBrentClose = 0;
            float lastDaxClose = 0;
            float lastSmaDeltaBrent = 0;
            float lastSmaDeltaDax = 0;

            foreach (DateTime date in allDates)
            {
                float? closeBrent = (float?)quotesBrent.Where(q => q.Date == date).Select(q => q.Close).FirstOrDefault();
                float? closeDax = (float?)quotesDax.Where(q => q.Date == date).Select(q => q.Close).FirstOrDefault();
                float? smaBrent = (float?)smaBrentList.Where(q => q.Date == date).Select(q => q.Sma).FirstOrDefault();
                float? smaDax = (float?)smaDaxList.Where(q => q.Date == date).Select(q => q.Sma).FirstOrDefault();

                if (closeBrent == 0)
                {
                    closeBrent = (float)lastBrentClose;
                }
                if (closeDax == 0)
                {
                    closeDax = (float)lastDaxClose;
                }
                if (smaBrent == 0)
                {
                    smaBrent = (float)lastSmaBrent;
                }
                if (smaDax == 0)
                {
                    smaDax = (float)lastSmaDax;
                }

                allQuotes.Add(new TotalQuote()
                {
                    SmaDeltaBrent = lastSmaBrent != 0 && smaBrent.HasValue ?
                        ((lastSmaBrent - smaBrent.Value) > 0 ? 0 : 1) :
                        lastSmaDeltaBrent,
                    SmaDeltaDax = lastSmaDax != 0 && smaDax.HasValue ?
                        ((lastSmaDax - smaDax.Value) > 0 ? 0 : 1) :
                        lastSmaDeltaDax,
                    SmaBrent = smaBrent.HasValue ?
                        smaBrent.Value :
                       lastSmaBrent,
                    SmaDax = smaDax.HasValue ?
                        smaDax.Value :
                        lastSmaDax,
                    CloseBrent = closeBrent.HasValue ?
                        closeBrent.Value :
                        lastBrentClose,
                    CloseDax = closeDax.HasValue ?
                        closeDax.Value :
                        lastDaxClose,
                    Date = date
                });

                lastSmaBrent = allQuotes.Last().SmaBrent;
                lastSmaDax = allQuotes.Last().SmaDax;
                lastSmaDeltaBrent = allQuotes.Last().SmaDeltaBrent;
                lastSmaDeltaDax = allQuotes.Last().SmaDeltaDax;
                lastBrentClose = allQuotes.Last().CloseBrent;
                lastDaxClose = allQuotes.Last().CloseDax;

                int count = allQuotes.Count();

                if (allQuotes.Count() > 2)
                {
                    allQuotes[count - 2].NextDayCloseDax = lastDaxClose;
                    allQuotes[count - 2].NextDayCloseDaxBoolean = allQuotes[count - 2].NextDayCloseDax - allQuotes[count - 2].CloseDax > 0 ? true : false;
                }
            }

            return allQuotes;
        }
    }
}
