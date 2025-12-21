namespace Chess;

public class Chess_player(Chess_game.Turns player_color) : Chess_player_root(player_color)
{
    public Game_cell[,] board => (Game_cell[,])_board.Clone();

    public bool make_bot_move(Move move) => set_move(move, true, true);

    public bool make_move(Move move) => set_move(move);

    public HashSet<Pos> get_moves(Pos pos)
    {
        if (end_game_type != Chess_game.Loose_type.game_gos || _board[pos.row, pos.col].color != turn)
            return [];

        HashSet<Pos> possible_moves = [];
        Game_cell current = _board[pos.row, pos.col];
        Chess_game.Move_bools move_bools = get_move_bools(turn);

        possible_moves.UnionWith(current.get_moves(_board).Where(m => Chess_game.is_valid_move(_board, m, current.color, move_bools.is_king_moved)).Select(m => m.to));


        if (current.name == Chess_game.Piece_name.king)
            possible_moves.UnionWith(Chess_game.get_possible_castling_positions(_board, turn, move_bools));

        return possible_moves;
    }
}
