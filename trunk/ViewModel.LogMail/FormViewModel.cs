using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.LogMail.Business
{
    [DataContract(Name="LogFile")]
    public class FormViewModel : INotifyPropertyChanged
    {
        public static readonly String StartupPath = AppDomain.CurrentDomain.BaseDirectory;
        public const String ArchiveDirectory = "Archive";
        public const String ExtName = ".lm2";
        public const String Sended = ".sended";

        private DateTime? _DateKey;

        [DataMember]
        public DateTime? DateKey
        {
            get { return _DateKey; }
            set
            {
                _DateKey = value;
                this.FormPropertyChange("DateKey");
            }
        }

        private string _WorkContent;

        [DataMember]
        public string WorkContent
        {
            get { return _WorkContent; }
            set
            {
                _WorkContent = value;
                this.FormPropertyChange("WorkContent");
            }
        }
        private string _WorkQuestion;

        [DataMember]
        public string WorkQuestion
        {
            get { return _WorkQuestion; }
            set
            {
                _WorkQuestion = value;
                this.FormPropertyChange("WorkQuestion");
            }
        }
        private string _StudyContent;

        [DataMember]
        public string StudyContent
        {
            get { return _StudyContent; }
            set
            {
                _StudyContent = value;
                this.FormPropertyChange("StudyContent");
            }
        }
        private string _ScheduleNextWeek;

        [DataMember]
        public string ScheduleNextWeek
        {
            get { return _ScheduleNextWeek; }
            set
            {
                _ScheduleNextWeek = value;
                this.FormPropertyChange("ScheduleNextWeek");
            }
        }
        private string _Opinion;

        [DataMember]
        public string Opinion
        {
            get { return _Opinion; }
            set
            {
                _Opinion = value;
                this.FormPropertyChange("Opinion");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void FormPropertyChange(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void Clear()
        {
            this.Opinion = string.Empty;
            this.ScheduleNextWeek = string.Empty;
            this.StudyContent = string.Empty;
            this.WorkContent = string.Empty;
            this.WorkQuestion = string.Empty;
        }

        public void Load()
        {
            string fileName = this.DateKey.Value.ToString("yyyy-MM-dd");
            string targetFile = Path.Combine(StartupPath, ArchiveDirectory, fileName + ExtName);

            Debug.WriteLine(targetFile);
            FileInfo file = new FileInfo(targetFile);
            FileInfo fileSended = new FileInfo(targetFile + Sended);
            FormViewModel instance = null;

            if (fileSended.Exists)
            {
                using (FileStream fs = fileSended.OpenRead())
                {
                    DataContractSerializer serializer = new DataContractSerializer(this.GetType());
                    instance = serializer.ReadObject(fs) as FormViewModel;

                    fs.Close();
                }
            }
            else if (file.Exists)
            {
                using (FileStream fs = file.OpenRead())
                {
                    DataContractSerializer serializer = new DataContractSerializer(this.GetType());
                    instance = serializer.ReadObject(fs) as FormViewModel;
                    
                    fs.Close();
                }
            }

            if (instance != null)
            {
                this.Opinion = instance.Opinion;
                this.ScheduleNextWeek = instance.ScheduleNextWeek;
                this.StudyContent = instance.StudyContent;
                this.WorkContent = instance.WorkContent;
                this.WorkQuestion = instance.WorkQuestion;
            }
            else
            {
                this.Opinion = string.Empty;
                this.ScheduleNextWeek = string.Empty;
                this.StudyContent = string.Empty;
                this.WorkContent = string.Empty;
                this.WorkQuestion = string.Empty;
            }
        }

        public void Save()
        {
            string fileName = this.DateKey.Value.ToString("yyyy-MM-dd");
            string targetFile = Path.Combine(StartupPath, ArchiveDirectory, fileName + ExtName);

            Debug.WriteLine(targetFile);
            FileInfo file = new FileInfo(targetFile);

            if (!file.Directory.Exists)
                file.Directory.Create();

            if (!File.Exists(targetFile + Sended)) { 
                using (FileStream fs = file.Create())
                {
                    DataContractSerializer serializer = new DataContractSerializer(this.GetType());
                    serializer.WriteObject(fs, this);

                    fs.Flush();
                    fs.Close();
                }
            }
            else
            {
                using (FileStream fs = File.Create(targetFile + Sended))
                {
                    DataContractSerializer serializer = new DataContractSerializer(this.GetType());
                    serializer.WriteObject(fs, this);

                    fs.Flush();
                    fs.Close();
                }
            }
        }
    }
}
