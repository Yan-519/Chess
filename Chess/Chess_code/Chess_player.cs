using static Chess.Chess_tools;

namespace Chess;

public class Chess_player(Turns player_color = default) : Chess_player_root(player_color)
{
    public Chess_cell[,] board => (Chess_cell[,])_board.Clone();

    public bool make_bot_move(Move move) => check_and_make_move(move, true, true);

    public bool make_move(Move move) => check_and_make_move(move);

    public HashSet<Pos> get_moves(Pos pos)
    {
        if (is_game_over || _board[pos.row, pos.col].color != turn)
            return [];

        HashSet<Pos> possible_moves = [];
        Chess_cell current = _board[pos.row, pos.col];
        Move_bools move_bools = get_move_bools(turn);

        possible_moves.UnionWith(current.get_moves(_board).Where(m => is_valid_move(_board, m, current.color, move_bools, draw_data)).Select(m => m.to));


        if (current.name == Piece_name.king)
            possible_moves.UnionWith(get_possible_castling_positions(_board, turn, move_bools));

        return possible_moves;
    }

    public void end_game(Game_stats state)
    {
        if(state == Game_stats.draw || state == Game_stats.surrender)
            game_state = state;
    }
}
