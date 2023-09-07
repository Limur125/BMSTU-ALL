using BusinesLogic;
using Microsoft.Extensions.Logging;

namespace GameTime
{
    public partial class TimeRecordForm : Form
    {
        Form1 mainForm;
        int recordPages;
        List<TimeRecord> records;
        int pageIndex;
        UserMode userMode;
        public TimeRecordForm(Form1 mainForm, TimeMode mode, List<TimeRecord> timeRecords, UserMode uMode)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            records = timeRecords;
            userMode = uMode;
            switch (mode)
            {
                case TimeMode.ADD:
                    SaveTimeRecordGroupBox.Visible = true;
                    AllTimeRecordsGroupBox.Visible = false;
                    break;
                case TimeMode.ALL:
                    SaveTimeRecordGroupBox.Visible = false;
                    AllTimeRecordsGroupBox.Visible = true;
                    recordPages = records.Count / 9;
                    recordPages += records.Count % 9 != 0 ? 1 : 0;
                    vScrollBar1.Maximum = recordPages;
                    pageIndex = 0;
                    DisableRecords();
                    int n = pageIndex < recordPages - 1 ? 9 : records.Count % 9;
                    n = n == 0 ? 9 : n;
                    for (int i = 1; i <= n; i++)
                        EnableRecord(records[pageIndex * 9 + i - 1], i);
                    if (userMode != UserMode.ADMIN)
                        DisableDeleteButtons();
                    break;
                default:
                    break;
            }
        }

        void DisableRecords()
        {
            for (int i = 1; i <= 9; i++)
            {
                AllTimeRecordsGroupBox.Controls[$"User{i}"].Visible = false;
                AllTimeRecordsGroupBox.Controls[$"Type{i}"].Visible = false;
                AllTimeRecordsGroupBox.Controls[$"Time{i}"].Visible = false;
                AllTimeRecordsGroupBox.Controls[$"button{i}"].Visible = false;
            }
        }

        void DisableDeleteButtons()
        {
            for (int i = 1; i <= 4; i++)
                AllTimeRecordsGroupBox.Controls[$"button{i}"].Visible = false;
        }

        void EnableRecord(TimeRecord record, int i)
        {
            string trt = record.Type switch
            {
                TimeRecordType.NORMAL => "обычное",
                TimeRecordType.FULL => "полное",
                _ => "обычное",
            };
            AllTimeRecordsGroupBox.Controls[$"User{i}"].Visible = true;
            AllTimeRecordsGroupBox.Controls[$"Type{i}"].Visible = true;
            AllTimeRecordsGroupBox.Controls[$"Time{i}"].Visible = true;
            AllTimeRecordsGroupBox.Controls[$"User{i}"].Text = record.UserLogin;
            AllTimeRecordsGroupBox.Controls[$"Type{i}"].Text = trt;
            AllTimeRecordsGroupBox.Controls[$"Time{i}"].Text = record.TimeStamp.ToString();
            AllTimeRecordsGroupBox.Controls[$"button{i}"].Visible = true;
        }

        private void SaveTimeRecordButton_Click(object sender, EventArgs e)
        { 
            int type = SaveTimeRecordTypeCombobox.SelectedIndex;
            string hours = SaveTimeRecordHoursTextbox.Text;
            string minutes = SaveTimeRecordMinutesTextbox.Text;
            mainForm.SaveTimeRecord(type, hours, minutes);
            Close();
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            DisableRecords();
            pageIndex += (e.NewValue - e.OldValue);
            int n = pageIndex < recordPages - 1 ? 9 : records.Count % 9;
            n = n == 0 ? 9 : n;
            for (int i = 1; i <= n; i++)
                EnableRecord(records[pageIndex * 4 + i - 1], i);
            if (userMode != UserMode.ADMIN)
                DisableDeleteButtons();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mainForm.DeleteRecord(records[pageIndex * 9]);
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            mainForm.DeleteRecord(records[pageIndex * 9 + 1]);
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            mainForm.DeleteRecord(records[pageIndex * 9 + 2]);
            Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            mainForm.DeleteRecord(records[pageIndex * 9 + 3]);
            Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            mainForm.DeleteRecord(records[pageIndex * 9 + 4]);
            Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            mainForm.DeleteRecord(records[pageIndex * 9 + 5]);
            Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            mainForm.DeleteRecord(records[pageIndex * 9 + 6]);
            Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            mainForm.DeleteRecord(records[pageIndex * 9 + 7]);
            Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            mainForm.DeleteRecord(records[pageIndex * 9 + 8]);
            Close();
        }
    }
    public enum TimeMode
    {
        ADD,
        ALL,
    }
}
