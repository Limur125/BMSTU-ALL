using BusinesLogic;

namespace GameTime
{
    public partial class ReviewForm : Form
    {
        List<Review> _reviews;
        MainForm _mainForm;
        int pageIndex;
        int reviewPages;
        UserMode _userMode;
        public ReviewForm(MainForm mainForm, List<Review> reviews, ReviewMode mode, UserMode userMode)
        {
            _mainForm = mainForm;
            _reviews = reviews;
            _userMode = userMode;
            InitializeComponent();
            switch (mode)
            {
                case ReviewMode.SHOW:
                    SaveReviewGroupBox.Visible = false;
                    AllReviewsGroupBox1.Visible = true;
                    reviewPages = _reviews.Count / 4;
                    reviewPages += _reviews.Count % 4 != 0 ? 1 : 0;
                    vScrollBar1.Maximum = reviewPages;
                    pageIndex = 0;
                    DisableReviews();
                    int n = pageIndex < reviewPages - 1 ? 4 : _reviews.Count % 4;
                    n = n == 0 ? 4 : n;
                    for (int i = 1; i <= n ; i++)
                        EnableReview(_reviews[pageIndex * 4 + i - 1], i);
                    break;
                case ReviewMode.ADD:
                    SaveReviewGroupBox.Visible = true;
                    AllReviewsGroupBox1.Visible = false;
                    break;
                default:
                    break;
            }
            if (userMode != UserMode.ADMIN)
                DisableDeleteButtons();
        }

        void DisableDeleteButtons()
        {
            for (int i = 1; i <= 4; i++)
                AllReviewsGroupBox1.Controls[$"DeleteReview{i}Button"].Visible = false;
        }

        void DisableReviews()
        {
            for (int i = 1; i <= 4; i++)
            {
                AllReviewsGroupBox1.Controls[$"Review{i}Date"].Visible = false;
                AllReviewsGroupBox1.Controls[$"Review{i}Text"].Visible = false;
                AllReviewsGroupBox1.Controls[$"Review{i}User"].Visible = false;
                AllReviewsGroupBox1.Controls[$"Review{i}Score"].Visible = false;
                AllReviewsGroupBox1.Controls[$"DeleteReview{i}Button"].Visible = false;
            }
        }

        void EnableReview(Review r, int i)
        {
            AllReviewsGroupBox1.Controls[$"Review{i}Date"].Visible = true;
            AllReviewsGroupBox1.Controls[$"Review{i}Text"].Visible = true;
            AllReviewsGroupBox1.Controls[$"Review{i}User"].Visible = true;
            AllReviewsGroupBox1.Controls[$"Review{i}Score"].Visible = true;
            AllReviewsGroupBox1.Controls[$"Review{i}User"].Text = r.UserLogin;
            AllReviewsGroupBox1.Controls[$"Review{i}Date"].Text = r.PublicationDate.Date.ToShortDateString();
            AllReviewsGroupBox1.Controls[$"Review{i}Text"].Text = r.Text;
            AllReviewsGroupBox1.Controls[$"Review{i}Score"].Text = r.Score.ToString();
            AllReviewsGroupBox1.Controls[$"DeleteReview{i}Button"].Visible = true;
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            DisableReviews();
            pageIndex += (e.NewValue - e.OldValue);
            int n = pageIndex < reviewPages - 1 ? 4 : _reviews.Count % 4;
            n = n == 0 ? 4 : n;
            for (int i = 1; i <= n; i++)
                EnableReview(_reviews[pageIndex * 4 + i - 1], i);
            if (_userMode != UserMode.ADMIN)
                DisableDeleteButtons();
        }

        private void DeleteReview1Button_Click(object sender, EventArgs e)
        {
            _mainForm.DeleteReview(_reviews[pageIndex * 4]);
            Close();
        }

        private void DeleteReview2Button_Click(object sender, EventArgs e)
        {
            _mainForm.DeleteReview(_reviews[pageIndex * 4 + 1]);
            Close();
        }

        private void DeleteReview3Button_Click(object sender, EventArgs e)
        {
            _mainForm.DeleteReview(_reviews[pageIndex * 4 + 2]);
            Close();
        }

        private void DeleteReview4Button_Click(object sender, EventArgs e)
        {
            _mainForm.DeleteReview(_reviews[pageIndex * 4 + 3]);
            Close();
        }

        private void SaveReviewButton_Click(object sender, EventArgs e)
        {
            uint score = decimal.ToUInt32(SaveReviewScoreNumericUpDown.Value);
            string text = SaveReviewTextBox.Text;
            _mainForm.SaveReview(text, score);
            Close();
        }
    }
    public enum ReviewMode
    {
        SHOW,
        ADD,
    }
}
