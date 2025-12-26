namespace Chess;

using Piece_name = Chess_tools.Piece_name;
using Turns = Chess_tools.Turns;

public partial class Change_pawn_window : Form
{
    private Game_form game_Window;

    public Change_pawn_window(Turns turn, Game_form game_Window)
    {
        this.game_Window = game_Window;

        InitializeComponent();

        this.FormClosing += (sender, e) => Environment.Exit(0);

        Button[] buttons = [queen_button, rook_button, knight_button, bishop_button];
        Piece_name[] piece_names = [Piece_name.queen, Piece_name.rook, Piece_name.knight, Piece_name.bishop];

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].Tag = piece_names[i];
            buttons[i].Image = Game_form.get_picture(turn, piece_names[i]);
        }
    }

    public void change_pawn_click(object sender, EventArgs e)
    {
        if (sender is Button current_button && current_button.Tag is Piece_name name)
        {
            game_Window.set_board_enabled(true);
            game_Window.after_choosing_pawn_transformation(name);
            this.Hide();
        }
    }
}
