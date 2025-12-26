using Bot_levels = Chess.Chess_tools.Bot_levels;
using Turns = Chess.Chess_tools.Turns;

namespace Chess
{
    public partial class Start_Page : Form
    {
        private static readonly Random random = new();

        private enum Switch_player_color {white, black, random}

        private bool is_bot = false;
        private Bot_levels bot_level = Bot_levels.easy;
        private Turns player_turn = new Turns[2] { Turns.black, Turns.white }[ random.Next(2)];

        public Start_Page()
        {
            InitializeComponent();
            this.Padding = new Padding(5);

            easy_bar.Tag = Bot_levels.easy;
            normal_bar.Tag = Bot_levels.normal;
            hard_bar.Tag = Bot_levels.hard;

            random_bar.Tag =  Switch_player_color.random;
            white_bar.Tag = Switch_player_color.white;
            black_bar.Tag = Switch_player_color.black;
        }

        private void start_button_Click(object sender, EventArgs e)
        {
            Game_form game_Layout = is_bot ? new(this, player_turn, bot_level) : new(this);
            game_Layout.Show();
            this.Hide();
        }

        private void bot_level_menu_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem menuItem && menuItem.Tag is Bot_levels new_level)
            {
                bot_level = new_level;

                easy_bar.Checked = new_level == Bot_levels.easy;
                normal_bar.Checked = new_level == Bot_levels.normal;
                hard_bar.Checked = new_level == Bot_levels.hard;
            }
        }

        private void color_menu_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem menuItem && menuItem.Tag is Switch_player_color new_turn)
            {
                player_turn = Switch_player_color.white == new_turn ? Turns.white :
                    Switch_player_color.black == new_turn ? Turns.black :
                    new Turns[2] { Turns.black, Turns.white }[random.Next(2)];

                random_bar.Checked = Switch_player_color.random == new_turn;
                white_bar.Checked = Switch_player_color.white == new_turn;
                black_bar.Checked = Switch_player_color.black == new_turn;
            }
        }

        private void bot_switch(object sender, EventArgs e)
        {
            is_bot = !is_bot;

            top_bar_levels.Visible = is_bot;
            top_bar_player_color.Visible = is_bot;

            random_bar.Checked = true;
            white_bar.Checked = false;
            black_bar.Checked = false;
            player_turn = new Turns[2] { Turns.black, Turns.white }[random.Next(2)];

            easy_bar.Checked = true;
            normal_bar.Checked = false;
            hard_bar.Checked = false;
            bot_level = Bot_levels.easy;
        }

    }
}
