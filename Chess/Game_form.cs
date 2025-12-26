namespace Chess
{
    using Bot_levels = Chess_tools.Bot_levels;
    using Loose_type = Chess_tools.Loose_type;
    using Piece_name = Chess_tools.Piece_name;
    using Turns = Chess_tools.Turns;
    public partial class Game_form : Form
    {
        private Button[,] board_buttons = new Button[8, 8];

        private Start_Page start_Page;

        private readonly Color potential = Color.Red;
        private readonly Color selected = Color.Blue;

        private readonly Color board_color_dark = Color.DarkGreen;
        private readonly Color board_color_light = Color.White;

        public enum Game_type { vz_bot, two_players }
        private Game_type game_type { init; get; }

        private Chess_player player;
        private Chess_bot bot;

        private Move selected_move = new();

        private Pos move_pieces = new();

        public Game_form(Start_Page start_Page)
        {
            InitializeComponent();
            this.FormClosing += (sender, e) => Environment.Exit(0);

            game_type = Game_type.two_players;
            this.start_Page = start_Page;

            player = new Chess_player(Turns.white);
            bot = new();

            InitBoard(Turns.white);
        }

        public Game_form(Start_Page start_Page, Turns player_color, Bot_levels bot_level)
        {
            InitializeComponent();
            this.FormClosing += (sender, e) => Environment.Exit(0);

            game_type = Game_type.vz_bot;
            this.start_Page = start_Page;

            player = new Chess_player(player_color);
            InitBoard(player_color);

            bot = new Chess_bot(Chess_tools.reverse(player_color), bot_level, ref selected_move);
            if (!selected_move.is_None())
                player.make_bot_move(selected_move);

            refresh_board();
        }

        private void InitBoard(Turns color)
        {
            TableLayoutPanel main_grid = new()
            {
                Dock = DockStyle.Fill,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
            };

            for (int i = 0; i < 8; i++)
            {
                main_grid.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5f));
                main_grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5f));
            }

            this.Controls.Add(main_grid);

            Func<int, int> indexing = i => color == Turns.white ? i : 7 - i;

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Button button = new()
                    {
                        Dock = DockStyle.Fill,
                        BackColor = (row + col) % 2 == 0 ? board_color_light : board_color_dark
                    };
                    Pos pos = new(indexing(row), indexing(col));
                    button.Click += (sender, e) => Button_Click(pos);

                    board_buttons[indexing(row), indexing(col)] = button;
                    main_grid.Controls.Add(button, col, row);
                }
            }

            refresh_board();
        }

        public static Bitmap? get_picture(Turns color, Piece_name name)
        {
            if (name == Piece_name.None)
                return null;

            string path = Application.StartupPath + $@"/../../../Images/{color}_{name}.png";
            try
            {
                return (Bitmap)Image.FromFile(path);
            } catch { }
            return null;
        }

        private void Button_Click(Pos pos)
        {
            if (game_type == Game_type.vz_bot && player.turn != player.color_of_this && game_type != Game_type.two_players || player.end_game_type != Loose_type.game_gos)
                return;

            if (!pos.isin_board_range())
            {
                MessageBox.Show($"You clicked outside the board range: {pos}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (board_buttons[pos.row, pos.col].BackColor != potential)
            {
                clean_red_from_board();
                move_pieces = pos;

                foreach (Pos p in player.get_moves(pos))
                    board_buttons[p.row, p.col].BackColor = potential;

                check_for_game_end();
                return;
            }

            selected_move = new(to: pos, from: move_pieces);
            clean_red_from_board();

            if (player.board[move_pieces.row, move_pieces.col].name == Piece_name.pawn && (pos.row == 0 || pos.row == 7))
            {
                player.change_pawn_to = Piece_name.None;
                set_board_enabled(false);
                new Change_pawn_window(player.turn, this).Show();
                return;
            }
            make_the_move();
        }

        public void after_choosing_pawn_transformation(Piece_name name)
        {
            player.change_pawn_to = name;
            set_board_enabled(true);
            make_the_move();
        }

        private void make_the_move()
        {
            if (!player.make_move(selected_move))
            {
                check_for_game_end();
                return;
            }
            refresh_board();
            if (game_type == Game_type.vz_bot && player.end_game_type == Loose_type.game_gos)
            {
                selected_move = bot.get_response_for(selected_move);
                if (!selected_move.is_None() && player.make_bot_move(selected_move))
                {
                    refresh_board();
                    clean_red_from_board();
                }
            }
            check_for_game_end();
        }

        public void set_board_enabled(bool v)
        {
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    board_buttons[i, j].Enabled = v;
        }

        private void check_for_game_end()
        {
            if (player.end_game_type != Loose_type.game_gos)
            {
                if (MessageBox.Show($"The game is over: {player.end_game_type} on the {player.turn} turn \n" +
                                                        "Do you want to go to the start page?", "Game finish",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.start_Page.Show();
                    this.Hide();
                }
                else Environment.Exit(0);
            }
        }

        private void refresh_board()
        {
            this.Text = $"The turn of the {player.turn} player";
            Chess_cell[,] board = player.board;

            for (int row = 0; row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    Chess_cell current_piece = board[row, column];
                    board_buttons[row, column].Image = get_picture(current_piece.color, current_piece.name);
                }
            }

            if (!selected_move.is_None())
            {
                board_buttons[selected_move.from.row, selected_move.from.col].BackColor = selected;
                board_buttons[selected_move.to.row, selected_move.to.col].BackColor = selected;
            }
        }

        private void clean_red_from_board()
        {
            for (int row = 0; row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    Button current_button = board_buttons[row, column];

                    if (current_button.BackColor == selected || current_button.BackColor == potential)
                        current_button.BackColor = (row + column) % 2 == 0 ? board_color_light : board_color_dark;

                    if (selected_move.to.is_on(row, column) || selected_move.from.is_on(row, column))
                        current_button.BackColor = selected;
                }
            }
        }
    }
}
