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
using System.Windows.Threading;

namespace EneaWPF
{
    class AppManager : INotifyPropertyChanged
    {

        DispatcherTimer timer = new DispatcherTimer();
        HandleEmail email = new HandleEmail("smtp.gmail.com", 587);

        private string downloadedData;
        private List<string> downloadedDataList = new List<string>();


        private ObservableCollection<Disconnection> todayDisconnectionList = new ObservableCollection<Disconnection>();
        private ObservableCollection<Disconnection> tomorrowDisconnectionList = new ObservableCollection<Disconnection>();
        private ObservableCollection<Disconnection> elseDisconnectionList = new ObservableCollection<Disconnection>();

        public ObservableCollection<Disconnection> TodayList { get { return todayDisconnectionList; } }
        public ObservableCollection<Disconnection> TommorowList { get { return tomorrowDisconnectionList; } }
        public ObservableCollection<Disconnection> ElseList { get { return elseDisconnectionList; } }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Location { get; set; }
        public string Date { get;  set; }
        public string Time { get;  set; }
        public string Details { get; set; }

        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }

        private DateTime lastUpdate;
        private TimeSpan toNextUpdate;
        public string LastUpdate { get { return lastUpdate.ToLongDateString(); } }
        public string ToNextUpdate {
            get
            {
                return toNextUpdate.Days + " " + StringHelper.getProperEndingInPolish(toNextUpdate.Days, true) + " " +
                toNextUpdate.Hours + " godzin" + StringHelper.getProperEndingInPolish(toNextUpdate.Hours) + " " +
                toNextUpdate.Minutes + " minut" + StringHelper.getProperEndingInPolish(toNextUpdate.Minutes);
            }
        }

        public bool isSMSSubscriptionRunning = false;
        public bool isEmailSubscriptionRunning = false;


        public AppManager()
        {
            UpdateData();
            timer.Interval = TimeSpan.FromMinutes(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            toNextUpdate -= TimeSpan.FromMinutes(1);
            OnPropertyChanged("ToNextUpdate");
        }

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
            todayDisconnectionList.Clear();
            tomorrowDisconnectionList.Clear();
            elseDisconnectionList.Clear();

            downloadedData = null;
            downloadedDataList.Clear();


            await downloadString();
            await createFileFromString();
            await createListFromFile();
            formatList();

            makeDisconnectionLists();

            lastUpdate = DateTime.Now;
            toNextUpdate = TimeSpan.FromHours(24);

            OnPropertyChanged("LastUpdate");
            OnPropertyChanged("ToNextUpdate");

            OnPropertyChanged("TodayDisconnectionList");
            OnPropertyChanged("TommorowDisconnectionList");
            OnPropertyChanged("ElseDisconnectionList");

            if (isEmailSubscriptionRunning)
            {
                email.sendMail(generateEmailSubject(), generateEmailString(), EmailAddress);
            }
        }

        public void updateDetails(Disconnection disconnection)
        {
            Location = disconnection.ToString();
            Date = disconnection.ConvertDate().ToLongDateString();
            Time = disconnection.Time;
            Details = disconnection.Details;
            OnPropertyChanged("Location");
            OnPropertyChanged("Date");
            OnPropertyChanged("Time");
            OnPropertyChanged("Details");
        }




        private void formatList()
        {
            StringHelper.removeHtmlTagsFromList(ref downloadedDataList);
            StringHelper.removeUnnecesaryInformationFromList(ref downloadedDataList);
            StringHelper.removeBlanksFromList(ref downloadedDataList);
        }




        private async Task createFileFromString()
        {
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


        private void makeDisconnectionLists()
        {
            ObservableCollection<Disconnection> DisconnectionsList = new ObservableCollection<Disconnection>();
            for (int i = 0; i < downloadedDataList.Count; i+=3)
            {
                DisconnectionsList.Add(new Disconnection(downloadedDataList[i], downloadedDataList[i + 1], downloadedDataList[i + 2]));
            }

            foreach (Disconnection disconnection in DisconnectionsList)
            {
                if (disconnection.ConvertDate().Date == DateTime.Now.Date)
                {
                    todayDisconnectionList.Add(disconnection);
                }
                else if (disconnection.ConvertDate().Date == DateTime.Now.Date + TimeSpan.FromDays(1))
                {
                    tomorrowDisconnectionList.Add(disconnection);
                }
                else
                    elseDisconnectionList.Add(disconnection);
            }

        }

        private string generateEmailSubject()
        {
            return "Planowane wyłączenia w dniach " + DateTime.Now.ToShortDateString() + ", "
                + (DateTime.Now + TimeSpan.FromDays(1)).ToShortDateString() + ".";
        }


        private string generateEmailString()
        {
            string email = "Planowane wyłączenia w dniach " + DateTime.Now.ToShortDateString() + ", "
                + (DateTime.Now + TimeSpan.FromDays(1)).ToShortDateString() + Environment.NewLine + Environment.NewLine;

            if (todayDisconnectionList.Count == 0)
            {
                email += "Brak wyłączeń w dniu dzisiejszym." + Environment.NewLine + Environment.NewLine;
            }
            else
            {
                foreach (var disconnection in todayDisconnectionList)
                {
                    email += disconnection.Area + Environment.NewLine;
                    email += disconnection.Time + Environment.NewLine;
                    email += disconnection.Details + Environment.NewLine + Environment.NewLine;
                }
            }

            if (tomorrowDisconnectionList.Count == 0)
            {
                email += "Brak wyłączeń w dniu jutrzejszym." + Environment.NewLine + Environment.NewLine;
            }
            else
            {
                foreach (var disconnection in tomorrowDisconnectionList)
                {
                    email += disconnection.Area + Environment.NewLine;
                    email += disconnection.Time + Environment.NewLine;
                    email += disconnection.Details + Environment.NewLine + Environment.NewLine;
                }
            }
            email += Environment.NewLine + Environment.NewLine + Environment.NewLine;
            email += "E-mail wygenerowany automatycznie przez program, prosimy nie odpowiadać!";

            return email;
        }

        private string generateSMSString()
        {
            string sms = "Planowane wylaczenia jutro w okolicach: " + Environment.NewLine;
            foreach (var disconnection in tomorrowDisconnectionList)
            {
                sms += disconnection.Area + Environment.NewLine;
            }
            sms += "Po więcej szczegolow zapraszamy na ekran aplikacji.";
            return sms;
        }

    }
}
