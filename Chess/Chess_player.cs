namespace Chess;

using Loose_type = Chess_game.Loose_type;
using Piece_name = Chess_game.Piece_name;
using Turns = Chess_game.Turns;

public class Chess_player(Turns player_color) : Chess_player_root(player_color)
{
    public Piece_characteristic[,] board => (Piece_characteristic[,])_board.Clone();

    public bool make_bot_move(Move move) => set_move(move, Chess_game.reverse(color_of_this), true);

    public bool make_move(Move move) => set_move(move, turn);

    public HashSet<Pos> get_moves(Pos pos)
    {
        if (end_game_type != Loose_type.game_gos || _board[pos.row, pos.col].is_None() || _board[pos.row, pos.col].color != turn)
            return [];

        HashSet<Pos> possible_moves = [];
        Piece_characteristic current = _board[pos.row, pos.col];

        bool is_king_moved = is_king_moved_g(turn);
        bool is_left_rook_moved = is_left_rook_moved_g(turn);
        bool is_right_rook_moved = is_right_rook_moved_g(turn);

        possible_moves.UnionWith(current.get_moves(_board).Where(m => Chess_game.is_valid_move(_board, m, current.color, is_king_moved)).Select(m => m.to));


        if (current.name == Piece_name.king)
            possible_moves.UnionWith(Chess_game.get_possible_castling_positions(_board, turn, is_king_moved, is_left_rook_moved, is_right_rook_moved));

        return possible_moves;
    }

}
