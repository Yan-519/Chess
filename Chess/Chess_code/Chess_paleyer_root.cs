namespace Chess.Chess_code;

using static Chess_tools;

public class Chess_player_root(Turns player_color)
{
    protected Chess_cell[,] _board = InitializeBoard();

    protected Draw_data draw_data = new();

    protected Move_bools white_move_bools = new(false);
    protected Move_bools black_move_bools = new(false);

    public Turns turn { get; protected set; } = Turns.white;

    public Turns color_of_this { get; init; } = player_color;
    public Turns color_of_opponent { get; init; } = reverse(player_color);

    public Game_stats game_state = Game_stats.gos;

    public Piece_name change_pawn_to = Piece_name.None;

    protected Move_bools get_move_bools(Turns color) => (color == Turns.white) ? white_move_bools : black_move_bools;

    protected bool check_and_make_move(Move move, bool is_bot = false, bool is_opponent_color = false)
    {
        Turns color = is_opponent_color ? color_of_opponent : turn;

        if (game_state != Game_stats.gos || move.is_None() || color != turn)
            return false;

        Move_bools move_bools = get_move_bools(color);

        if (!get_all_moves(_board, color, move_bools, draw_data).Contains(move))
            return false;

        (_board, move_bools, draw_data) = generate_future_board(_board, move, move_bools, draw_data, color, change_pawn_to, is_bot);
        if (color == Turns.white)
            white_move_bools = move_bools;
        else black_move_bools = move_bools;

        turn = reverse(turn);

        if (is_this_color_in_checkmate(_board, turn, draw_data))
            game_state = Game_stats.checkmate;

        else if (is_draw(_board, turn, draw_data))
            game_state = Game_stats.draw;

        return true;
    }
}
