using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EneaWPF
{
    static class StringHelper
    {
        static Regex htmlRegex = new Regex("<.*?>", RegexOptions.Compiled);


        public static string CutTagsFromHtml(string source)
        {
            return htmlRegex.Replace(source, string.Empty);
        }




        public static void removeHtmlTagsFromList(ref List<string> dataList)
        {
            List<string> copiedList = new List<string>();
            copiedList.AddRange(dataList);
            dataList.Clear();


            foreach (string line in copiedList)
            {
                dataList.Add(StringHelper.CutTagsFromHtml(line));
            }
        }


        public static void removeUnnecesaryInformationFromList(ref List<string> dataList)
        {
            string startingLine = string.Empty;
            string finishLine = string.Empty;
            foreach (string line in dataList)
            {
                if (line.Contains("Wyłączenia w Rejonie Dystrybucji Bydgoszcz"))
                {
                    startingLine = line;
                }
                if (line.Contains("Kim jesteśmy"))
                {
                    finishLine = line;
                }
            }
            dataList.RemoveRange(0, dataList.IndexOf(startingLine) + 1);
            dataList.RemoveRange(dataList.IndexOf(finishLine), dataList.Count - 1 - dataList.IndexOf(finishLine));
        }


        public static void removeBlanksFromList(ref List<string> dataList)
        {
            List<string> copiedList = new List<string>();
            copiedList.AddRange(dataList);

            foreach (string line in copiedList)
            {
                if (string.IsNullOrWhiteSpace(line)) dataList.Remove(line);
            }

            for (int i = 0; i < dataList.Count; i++)
            {
                dataList[i] = dataList[i].Trim();
            }
        }

        public static string getProperEndingInPolish(int number, bool isDay=false)
        {
            if (!isDay)
            {
                int numberToCheck = number;

                if (numberToCheck == 1) return "a";
                if (numberToCheck > 10)
                {
                    string cutNumber = number.ToString().Remove(0, 1);
                    int.TryParse(cutNumber, out numberToCheck);
                }

                if (numberToCheck >= 2 && numberToCheck <= 4) return "y";
                else return "";

            }
            else
            {
                if (number == 1) return "dzień";
                else return "dni";
            }
        }

    }
}

