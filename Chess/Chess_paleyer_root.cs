namespace Chess;

using Loose_type = Chess_game.Loose_type;
using Piece_name = Chess_game.Piece_name;
using Turns = Chess_game.Turns;

public class Chess_player_root(Turns player_color)
{
    protected Piece_characteristic[,] _board = Chess_game.InitializeBoard();

    protected bool is_white_left_rook_moved = false, is_white_right_rook_moved = false;
    protected bool is_black_left_rook_moved = false, is_black_right_rook_moved = false;

    protected bool is_white_king_moved = false, is_black_king_moved = false;

    protected bool is_king_moved_g(Turns color) => (color == Turns.white) ? is_white_king_moved : is_black_king_moved;
    protected bool is_left_rook_moved_g(Turns color) => (color == Turns.white) ? is_white_left_rook_moved : is_black_left_rook_moved;
    protected bool is_right_rook_moved_g(Turns color) => (color == Turns.white) ? is_white_right_rook_moved : is_black_right_rook_moved;


    public Turns turn { get; protected set; } = Turns.white;

    public Turns color_of_this { get; protected set; } = player_color;
    public Turns color_of_opponent => Chess_game.reverse(color_of_this);

    public Loose_type end_game_type { get; protected set; } = Loose_type.game_gos;


    public Piece_name change_pawn_to = Piece_name.None;

    protected bool set_move(Move move, Turns color, bool is_bot = false)
    {
        if (end_game_type != Loose_type.game_gos || move.is_None() || color != turn)
            return false;

        bool is_king_moved = is_king_moved_g(color);
        bool is_left_rook_moved = is_left_rook_moved_g(color);
        bool is_right_rook_moved = is_right_rook_moved_g(color);

        if (!Chess_game.get_all_moves(_board, color, is_king_moved, is_left_rook_moved, is_right_rook_moved).Contains(move))
            return false;

        if (_board[move.to.row, move.to.col].is_None() && _board[move.from.row, move.from.col].name == Piece_name.pawn)
        {
            int en_passant_row = (color == Turns.white) ? 1 : -1;
            if (new Pos(move.to.row + en_passant_row, move.to.col).isin_board_range())
                if (_board[move.to.row + en_passant_row, move.to.col].name == Piece_name.pawn &&
                    _board[move.to.row + en_passant_row, move.to.col].color != color)
                    _board[move.to.row + en_passant_row, move.to.col].name = Piece_name.None;
        }

        _board[move.from.row, move.from.col].move_to(ref _board[move.to.row, move.to.col]);

        for (int row = 0; row < 8; row++)
            for (int column = 0; column < 8; column++)
                _board[row, column].is_pawn_double_moved = false;


        switch (_board[move.to.row, move.to.col].name)
        {
            case Piece_name.pawn:
                if (move.to.row == 0 || move.to.row == 7)
                {
                    _board[move.to.row, move.to.col].name = is_bot ? Chess_bot.find_best_pawn_transformation(_board, move.to, color) : change_pawn_to;
                }
                else
                {
                    int start_position = (color == Turns.white) ? 6 : 1;
                    int end_position = (color == Turns.white) ? 4 : 3;

                    if (move.from.row == start_position && move.to.row == end_position)
                        _board[move.to.row, move.to.col].is_pawn_double_moved = true;
                }
                break;

            case Piece_name.rook:
                if (move.from.is_on(0, 0) || _board[0, 0].name != Piece_name.rook) is_black_left_rook_moved = true;
                if (move.from.is_on(0, 7) || _board[0, 7].name != Piece_name.rook) is_black_right_rook_moved = true;

                if (move.from.is_on(7, 0) || _board[7, 0].name != Piece_name.rook) is_white_left_rook_moved = true;
                if (move.from.is_on(7, 7) || _board[7, 7].name != Piece_name.rook) is_white_right_rook_moved = true;
                break;

            case Piece_name.king:
                {
                    if ((color == Turns.white) ? is_white_king_moved : is_black_king_moved)
                        break;

                    Move rook_castling_moves = Chess_game.attempt_castling(_board[move.to.row, move.to.col], color);

                    if (!rook_castling_moves.is_None())
                    {
                        _board[rook_castling_moves.from.row, rook_castling_moves.from.col]
                            .move_to(ref _board[rook_castling_moves.to.row, rook_castling_moves.to.col]);
                    }


                    if (color == Turns.white)
                    {
                        is_white_king_moved = true;

                        is_white_right_rook_moved = true;
                        is_white_left_rook_moved = true;
                    }
                    else
                    {
                        is_black_left_rook_moved = true;

                        is_black_right_rook_moved = true;
                        is_black_king_moved = true;
                    }

                    break;
                }

            default: break;
        }

        turn = Chess_game.reverse(turn);

        if (Chess_game.is_this_color_in_checkmate(_board, turn))
            end_game_type = Loose_type.checkmate;

        else if (Chess_game.is_draw(_board, turn))
            end_game_type = Loose_type.draw;


        return true;

    }
}
