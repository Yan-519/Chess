namespace Chess.Chess_code;

public class Chess_player(Chess_tools.Turns player_color) : Chess_player_root(player_color)
{
    public Chess_cell[,] board => (Chess_cell[,])_board.Clone();

    public bool make_bot_move(Move move) => check_and_make_move(move, true, true);

    public bool make_move(Move move) => check_and_make_move(move);

    public HashSet<Pos> get_moves(Pos pos)
    {
        if (game_state != Chess_tools.Game_stats.gos || _board[pos.row, pos.col].color != turn)
            return [];

        HashSet<Pos> possible_moves = [];
        Chess_cell current = _board[pos.row, pos.col];
        Move_bools move_bools = get_move_bools(turn);

        possible_moves.UnionWith(current.get_moves(_board).Where(m => Chess_tools.is_valid_move(_board, m, current.color, move_bools, draw_data)).Select(m => m.to));


        if (current.name == Chess_tools.Piece_name.king)
            possible_moves.UnionWith(Chess_tools.get_possible_castling_positions(_board, turn, move_bools));

        return possible_moves;
    }
}
