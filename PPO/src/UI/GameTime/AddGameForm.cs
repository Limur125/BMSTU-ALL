using BusinesLogic;
using SqlServerDatabase;

namespace GameTime
{
    public partial class AddGameForm : Form
    {
        MainForm mainForm;
        Game currentGame;
        GameMode _mode;
        public AddGameForm(MainForm form, GameMode mode, Game game)
        {
            mainForm = form;
            currentGame = game;
            _mode = mode;
            InitializeComponent();
            switch (mode)
            {
                case GameMode.ADD:
                    GameButton.Text = "Добавить игру";
                    break;
                case GameMode.UPDATE:
                    GameButton.Text = "Изменить игру";
                    GameTitle.Text = game.Title;
                    GameDeveloper.Text = game.Developer;
                    GamePublisher.Text = game.Publisher;
                    GamePlatform.Text = game.Platform;
                    GameReleaseDate.Value = game.ReleaseDate;
                    break;
                default:
                    break;
            }
        }

        private void GameButton_Click(object sender, EventArgs e)
        {
            string title = GameTitle.Text;
            string developer = GameDeveloper.Text;
            string publisher = GamePublisher.Text;
            string platform = GamePlatform.Text;
            DateTime releaseDate = GameReleaseDate.Value.Date;
            int id = currentGame == null ? 0 : currentGame.Id;
            Game g = new Game(id, title, releaseDate, developer, publisher, platform);
            switch (_mode)
            {
                case GameMode.ADD:
                    mainForm.AddGame(g);
                    break;
                case GameMode.UPDATE:
                    mainForm.UpdateGame(g);
                    break;
                default:
                    break;
            }
            Close();
        }
    }

    public enum GameMode
    {
        ADD,
        UPDATE,
    }
}
