using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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
            downloadedData = await client.DownloadStringTaskAsync(uri);
        }




        public async void UpdateData()
        {
            test = null;
            downloadedData = null;
            downloadedDataList.Clear();
            await downloadString();
            await createFile();
            await createListFromFile();
            test = createListToShow();
            OnPropertyChanged("DataList");
        }





        private async Task createFile()
        {
            await downloadString();

            using (StreamWriter writer = new StreamWriter(new FileStream(@"c:\enea\enea.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite), Encoding.Default))
            {
                await writer.WriteLineAsync(downloadedData);
            }

        }




        private async Task createListFromFile()
        {
            using (StreamReader reader = new StreamReader(new FileStream(@"c:\enea\enea.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite)))
            {
                while (!reader.EndOfStream)
                {
                    downloadedDataList.Add(await reader.ReadLineAsync());
                }
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
