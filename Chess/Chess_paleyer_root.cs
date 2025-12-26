namespace Chess;

using static Chess_game;

public class Chess_player_root(Turns player_color)
{
    protected Chess_cell[,] _board = InitializeBoard();

    protected Draw_data draw_data = new();

    protected Move_bools white_move_bools = new(false);
    protected Move_bools black_move_bools = new(false);

    public Turns turn { get; protected set; } = Turns.white;

    public Turns color_of_this { get; init; } = player_color;
    public Turns color_of_opponent { get; init; } = reverse(player_color);

    public Loose_type end_game_type { get; protected set; } = Loose_type.game_gos;

    public Piece_name change_pawn_to = Piece_name.None;

    protected Move_bools get_move_bools(Turns color) => (color == Turns.white) ? white_move_bools : black_move_bools;

    protected bool set_move(Move move, bool is_bot = false, bool is_opponent_color = false)
    {
        Turns color = is_opponent_color ? color_of_opponent : turn;

        if (end_game_type != Loose_type.game_gos || move.is_None() || color != turn)
            return false;

        Move_bools is_moved = get_move_bools(color);

        if (!get_all_moves(_board, color, is_moved, draw_data).Contains(move))
            return false;

        else if (_board[move.to.row, move.to.col].is_None() && _board[move.from.row, move.from.col].name == Piece_name.pawn)
        {
            Pos temp = new(move.to.row + ((color == Turns.white) ? 1 : -1), move.to.col);
            if (temp.isin_board_range() && _board[temp.row, temp.col].name == Piece_name.pawn && _board[temp.row, temp.col].color != color)
                _board[temp.row, temp.col].name = Piece_name.None;
        }

        _board[move.from.row, move.from.col].move_to(ref _board[move.to.row, move.to.col]);

        for (int row = 0; row < 8; row++)
            for (int column = 0; column < 8; column++)
                _board[row, column].is_pawn_double_moved = false;

        switch (_board[move.to.row, move.to.col].name)
        {
            case Piece_name.pawn:
                if (move.to.row == 0 || move.to.row == 7)
                    _board[move.to.row, move.to.col].name = is_bot ? Chess_bot.find_best_pawn_transformation(_board, move.to, color, draw_data) : change_pawn_to;
                else
                {
                    int start_position = (color == Turns.white) ? 6 : 1;
                    int end_position = (color == Turns.white) ? 4 : 3;

                    if (move.from.row == start_position && move.to.row == end_position)
                        _board[move.to.row, move.to.col].is_pawn_double_moved = true;
                }
                break;

            case Piece_name.rook:
                if (move.from.is_on(0, 0) || _board[0, 0].name != Piece_name.rook)
                    black_move_bools.is_left_rook_moved = true;
                else if (move.from.is_on(0, 7) || _board[0, 7].name != Piece_name.rook)
                    black_move_bools.is_right_rook_moved = true;

                else if (move.from.is_on(7, 0) || _board[7, 0].name != Piece_name.rook)
                    white_move_bools.is_left_rook_moved = true;
                else if (move.from.is_on(7, 7) || _board[7, 7].name != Piece_name.rook)
                    white_move_bools.is_right_rook_moved = true;
                break;

            case Piece_name.king:
                {
                    if (is_moved.is_king_moved) break;

                    Move rook_castling_moves = attempt_castling(_board[move.to.row, move.to.col], color);

                    if (!rook_castling_moves.is_None())
                    {
                        _board[rook_castling_moves.from.row, rook_castling_moves.from.col]
                            .move_to(ref _board[rook_castling_moves.to.row, rook_castling_moves.to.col]);
                    }

                    if (color == Turns.white)
                        white_move_bools = new(true);
                    else black_move_bools = new(true);

                    break;
                }

            default: break;
        }

        turn = reverse(turn);
        draw_data.next(move);

        if (is_this_color_in_checkmate(_board, turn, draw_data))
            end_game_type = Loose_type.checkmate;

        else if (is_draw(_board, turn, draw_data))
            end_game_type = Loose_type.draw;

        return true;
    }
}
