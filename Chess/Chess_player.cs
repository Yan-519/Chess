namespace Chess;

using Loose_type = Chess_game.Loose_type;
using Turns = Chess_game.Turns;

public class Chess_player(Turns player_color) : Chess_player_root(player_color)
{
    public Piece_characteristic[,] board => (Piece_characteristic[,])_board.Clone();

    public bool make_bot_move(Move move) => set_move(move, Chess_game.reverse(color_of_this), true);

    public bool make_move(Move move) => set_move(move, turn);

    public IEnumerable<Pos> get_moves(Pos pos)
    {
        if (end_game_type != Loose_type.game_gos || _board[pos.row, pos.col].color != turn)
            return [];

        return Chess_game.get_all_moves_for(_board, pos, is_king_moved_g(turn), is_left_rook_moved_g(turn), is_right_rook_moved_g(turn)).Select(m => m.to);
    }

}
