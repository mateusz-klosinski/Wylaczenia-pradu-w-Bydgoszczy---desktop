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
    class AppManager : INotifyPropertyChanged
    {

        private string downloadedData;
        private List<string> downloadedDataList = new List<string>();
        private ObservableCollection<Disconnection> test;

        public ObservableCollection<Disconnection> DataList { get { return test; } }

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
            formatList();
            test = createListToShow();
            OnPropertyChanged("DataList");
        }


        private void formatList()
        {
            StringHelper.removeHtmlTagsFromList(ref downloadedDataList);
            StringHelper.removeUnnecesaryInformationFromList(ref downloadedDataList);
            StringHelper.removeBlanksFromList(ref downloadedDataList);
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



        public void TestList()
        {
            foreach (string line in downloadedDataList)
            {
                Console.WriteLine(line);
            }

        }


        private ObservableCollection<Disconnection> createListToShow()
        {
            ObservableCollection<Disconnection> listToShow = new ObservableCollection<Disconnection>();
            for (int i = 0; i < downloadedDataList.Count; i+=3)
            {
                listToShow.Add(new Disconnection(downloadedDataList[i], downloadedDataList[i + 1], downloadedDataList[i + 2]));
            }

            return listToShow;
        }

    }
}
