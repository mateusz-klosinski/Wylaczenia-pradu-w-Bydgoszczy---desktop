using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EneaWPF
{
    class Controller : INotifyPropertyChanged
    {

        private string downloadedData;
        private List<string> downloadedDataList = new List<string>();
        private ObservableCollection<string> test;

        public ObservableCollection<string> DataList { get { return test; } }

        public event PropertyChangedEventHandler PropertyChanged;



        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler property = PropertyChanged;
            if (property != null)
            {
                property(this, new PropertyChangedEventArgs(propertyName));
            }
        }




        private async Task downloadString()
        {
            WebClient client = new WebClient();
            Uri uri = new Uri("http://www.wylaczenia-eneaoperator.pl/index.php?rejon=17");
            try
            {
                downloadedData = await client.DownloadStringTaskAsync(uri);
            }
            catch (WebException)
            {
                MessageBox.Show("Nie można pobrać danych ze strony Enea", "Błąd sieci");
            }

        }




        public async void UpdateData()
        {
            test = null;
            downloadedData = null;
            downloadedDataList.Clear();
            await downloadString();
            await createFileFromString();
            await createListFromFile();
            makeMagicWithList();
            removeUnnecesaryInformationFromList();
            removeBlanksFromList();
            test = createListToShow();
            OnPropertyChanged("DataList");
        }





        private async Task createFileFromString()
        {
            await downloadString();

            using (StreamWriter writer = new StreamWriter(new FileStream(@"enea.html", FileMode.OpenOrCreate, FileAccess.ReadWrite), Encoding.Default))
            {
                await writer.WriteLineAsync(downloadedData);
            }

        }




        private async Task createListFromFile()
        {
            using (StreamReader reader = new StreamReader(new FileStream(@"enea.html", FileMode.OpenOrCreate, FileAccess.ReadWrite)))
            {
                while (!reader.EndOfStream)
                {
                    downloadedDataList.Add(await reader.ReadLineAsync());
                }
            }

        }



        private void makeMagicWithList()
        {
            List<string> copiedList = new List<string>();
            copiedList.AddRange(downloadedDataList);
            downloadedDataList.Clear();


            foreach(string line in copiedList)
            {
                downloadedDataList.Add(TagRemoveHelper.CutTagsFromHtml(line));
            }
        }


        private void removeUnnecesaryInformationFromList()
        {
            string startingLine = string.Empty;
            string finishLine = string.Empty;
            foreach (string line in downloadedDataList)
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
            downloadedDataList.RemoveRange(0, downloadedDataList.IndexOf(startingLine));
            downloadedDataList.RemoveRange(downloadedDataList.IndexOf(finishLine), downloadedDataList.Count - 1 - downloadedDataList.IndexOf(finishLine));
        }


        private void removeBlanksFromList()
        {
            for (int i = 0; i < downloadedDataList.Count; i++)
            {
                downloadedDataList[i] = downloadedDataList[i].Trim();

                if (String.IsNullOrEmpty(downloadedDataList[i])) downloadedDataList.RemoveAt(i);
            }
        }

        public void TestList()
        {
            foreach (string line in downloadedDataList)
            {
                Console.WriteLine(line);
            }

        }


        private ObservableCollection<string> createListToShow()
        {
            ObservableCollection<string> listToShow = new ObservableCollection<string>();
            for (int i = 0; i < downloadedDataList.Count; i++)
            {
                if (downloadedDataList[i].Contains("dzisiaj"))
                    listToShow.Add(downloadedDataList[i - 1] + downloadedDataList[i]);

            }

            return listToShow;
        }

    }
}
