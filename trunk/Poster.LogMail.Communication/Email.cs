using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Net.Mail;
using System.Text;
using ViewModel.LogMail.Business;

namespace Poster.LogMail.Communication
{
    public class Email
    {
        private NameValueCollection Config;
        private String Password;

        private const String NOTHING = "暂无";
        public const String SendedFileExtName = ".sended";

        public Email(string password)
        {
            this.Config = ConfigurationManager.AppSettings;
            this.Password = password;
        }

        public void Send(DirectoryInfo archiveDirectory, bool backup = false)
        {
            if (archiveDirectory == null || !archiveDirectory.Exists)
                return;

            FileInfo[] files = archiveDirectory.GetFiles("*" + FormViewModel.ExtName);
            if (files != null && files.Length > 0)
            {

                string smtpHost = this.Config["Email.SMTP"],
                   from = this.Config["Email.From"],
                   to = this.Config["Email.To"],
                   user = this.Config["Global.User"],
                   project = this.Config["Global.Project"];

                string startDateStr = files[0].Name.Replace(FormViewModel.ExtName, string.Empty),
                    endDateStr = files[files.Length - 1].Name.Replace(FormViewModel.ExtName, string.Empty);

                string key = null;
                FileInfo item = null;
                FormViewModel vm = new FormViewModel();
                DateTime dateKey;

                StringBuilder summaryContent = new StringBuilder();
                summaryContent.AppendFormat("姓名: {0}    负责项目: {1}    日期: {2} 至 {3}\r\n", user, project, startDateStr, endDateStr);

                StringBuilder weekWorkContent = new StringBuilder("\r\n\r\n一、本周工作内容\r\n");
                StringBuilder questionContent = new StringBuilder("\r\n\r\n二、本周所遇问题\r\n");
                StringBuilder studyContent = new StringBuilder("\r\n\r\n三、本周学习内容\r\n");
                StringBuilder scheduleContent = new StringBuilder("\r\n\r\n四、下周工作计划\r\n");
                StringBuilder opinionContent = new StringBuilder("\r\n\r\n五、意见和建议\r\n");

                int numberWeekWork = 0,
                    numberQuestion = 0,
                    numberStudy = 0,
                    numberSchedule = 0,
                    numberOpinion = 0;

                for (int i = 0, len = files.Length; i < len; i++)
                {
                    item = files[i];

                    if (!item.Exists)
                        continue;

                    key = item.Name.Replace(FormViewModel.ExtName, "");
                    if (!DateTime.TryParse(key, out dateKey))
                        continue;

                    vm.DateKey = dateKey;
                    vm.Load();

                    // 拼接邮件内容
                    this.ProcessText(weekWorkContent, vm.WorkContent, ref numberWeekWork);
                    this.ProcessText(questionContent, vm.WorkQuestion, ref numberQuestion);
                    this.ProcessText(studyContent, vm.StudyContent, ref numberStudy);
                    this.ProcessText(scheduleContent, vm.ScheduleNextWeek, ref numberSchedule);
                    this.ProcessText(opinionContent, vm.Opinion, ref numberOpinion);

                    // 标记已读取过的内容文件
                    item.MoveTo(Path.Combine(archiveDirectory.FullName, item.Name + SendedFileExtName));
                }

                if (numberWeekWork == 0)
                    weekWorkContent.AppendLine(NOTHING);
                if (numberQuestion == 0)
                    questionContent.AppendLine(NOTHING);
                if (numberStudy == 0)
                    studyContent.AppendLine(NOTHING);
                if (numberSchedule == 0)
                    scheduleContent.AppendLine(NOTHING);
                if (numberOpinion == 0)
                    opinionContent.AppendLine(NOTHING);

                String messageBody = summaryContent.ToString()
                    + weekWorkContent
                    + questionContent
                    + studyContent
                    + scheduleContent
                    + opinionContent;

                SmtpClient client = new SmtpClient(smtpHost);
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(from, this.Password);

                MailMessage message = message = new MailMessage();
                message.From = new MailAddress(from);
                message.To.Add(to);
                message.CC.Add(from);   // 抄送给自己一份
                message.Subject = string.Format("{0}至{1} 工作周报 来自:{2}", startDateStr, endDateStr, user);
                message.Body = messageBody;

                client.Send(message);

                if (backup)
                {
                    string historyPath = Path.Combine(archiveDirectory.FullName, "History");
                    if (!Directory.Exists(historyPath))
                    {
                        Directory.CreateDirectory(historyPath);
                    }

                    File.AppendAllText(Path.Combine(historyPath, string.Format("{0}至{1}周报{2}", startDateStr, endDateStr, ".report")), messageBody);
                }
            }
        }

        private void ProcessText(StringBuilder sb, string str, ref int startupNumber)
        {
            string[] contents = str.Split('\r', '\n');
            if (contents != null && contents.Length > 0)
            {
                foreach (string item in contents)
                {
                    if (!string.IsNullOrEmpty(item))
                        sb.AppendLine(string.Format("\t{0}.\t{1}", ++startupNumber, item));
                }
            }
        }
    }
}
