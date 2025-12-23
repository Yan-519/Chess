namespace Chess;

public static class Chess_game
{
    public record struct Move_bools(bool is_king_moved, bool is_left_rook_moved, bool is_right_rook_moved)
    {
        public bool is_king_moved = is_king_moved;
        public bool is_left_rook_moved = is_left_rook_moved;
        public bool is_right_rook_moved = is_right_rook_moved;

        public Move_bools(bool b) : this(b, b, b) { }
    };

    public enum Piece_name { rook, knight, bishop, queen, king, pawn, None };
    public enum Turns { white, black };
    public enum Bot_levels { easy, normal, hard };
    public enum Loose_type { checkmate, draw, game_gos, time_ended, not_started };


    public readonly static Func<Turns, Turns> reverse = color => (color == Turns.white) ? Turns.black : Turns.white;

    public static Chess_cell[,] InitializeBoard()
    {
        Chess_cell[,] board = new Chess_cell[8, 8];

        Piece_name[] line_of_figures =
        [
            Piece_name.rook, Piece_name.knight, Piece_name.bishop,
            Piece_name.queen, Piece_name.king,
            Piece_name.bishop, Piece_name.knight, Piece_name.rook
        ];

        Piece_name current_figures;
        Turns current_color_of_figure = Turns.black;

        for (int row = 0; row < 8; row++)
        {
            for (int column = 0; column < 8; column++)
            {
                if (row == 0 || row == 7)
                    current_figures = line_of_figures[column];

                else if (row == 1 || row == 6)
                    current_figures = Piece_name.pawn;

                else current_figures = Piece_name.None;

                board[row, column] = new(current_figures, current_color_of_figure, new(row, column));
            }
            if (row == 1) current_color_of_figure = Turns.white;
        }
        return board;
    }

    private static HashSet<Move> get_all_moves(Chess_cell[,] board, Turns color) => get_all_moves(board, color, new(false));

    public static HashSet<Move> get_all_moves(Chess_cell[,] board, Turns color, Move_bools move_bools)
    {
        HashSet<Move> moves = [];

        for (int row = 0; row < 8; row++)
        {
            for (int column = 0; column < 8; column++)
            {
                Chess_cell current = board[row, column];

                if (current.color != color)
                    continue;

                moves.UnionWith(current.get_moves(board).Where(m => is_valid_move(board, m, color, move_bools.is_king_moved)));

                if (current.name == Piece_name.king && !move_bools.is_king_moved)
                    moves.UnionWith(get_possible_castling_positions(board, color, move_bools).Select(p => new Move(to: p, from: current.pos)));

            }
        }

        return moves;
    }

    public static HashSet<Pos> get_possible_castling_positions(Chess_cell[,] board, Turns color, Move_bools move_bools)
    {
        static bool is_able_to_castling_her(Chess_cell[,] board, Pos[] is_safe, Pos[] is_clear, bool is_rook_moved, Turns color, Pos corner)
        {
            static bool is_path_clear(Chess_cell[,] board, Pos[] positions)
            {
                foreach (Pos pos in positions)
                    if (!board[pos.row, pos.col].is_None())
                        return false;

                return true;
            }

            static bool is_path_safe(Chess_cell[,] board, Pos[] positions, Turns color)
            {
                HashSet<Pos> attacks = get_attack_range_of(board, reverse(color));

                foreach (Pos pos in positions)
                    if (attacks.Contains(pos))
                        return false;

                return true;
            }

            if (is_rook_moved || board[corner.row, corner.col].name != Piece_name.rook)
                return false;

            return is_path_safe(board, is_safe, color) && is_path_clear(board, is_clear);
        }

        if (move_bools.is_left_rook_moved && move_bools.is_right_rook_moved || move_bools.is_king_moved)
            return [];

        else if (is_this_color_in_check(board, color))
            return [];

        HashSet<Pos> possible_castling_positions = [];

        if (color == Turns.white)
        {
            if (is_able_to_castling_her(board, [new(7, 2), new(7, 3)], [new(7, 1), new(7, 2), new(7, 3)], move_bools.is_left_rook_moved, color, new(7, 0)))
                possible_castling_positions.Add(new(7, 2));

            if (is_able_to_castling_her(board, [new(7, 5), new(7, 6)], [new(7, 5), new(7, 6)], move_bools.is_right_rook_moved, color, new(7, 7)))
                possible_castling_positions.Add(new(7, 6));
        }
        else
        {
            if (is_able_to_castling_her(board, [new(0, 2), new(0, 3)], [new(0, 1), new(0, 2), new(0, 3)], move_bools.is_left_rook_moved, color, new(0, 0)))
                possible_castling_positions.Add(new(0, 2));

            if (is_able_to_castling_her(board, [new(0, 5), new(0, 6)], [new(0, 5), new(0, 6)], move_bools.is_right_rook_moved, color, new(0, 7)))
                possible_castling_positions.Add(new(0, 6));
        }

        return possible_castling_positions;
    }

    private static HashSet<Pos> get_attack_range_of(Chess_cell[,] board, Turns color)
    {
        HashSet<Pos> attack_positions = [];

        for (int row = 0; row < 8; row++)
        {
            for (int column = 0; column < 8; column++)
            {
                Chess_cell current_button = board[row, column];

                if (current_button.color == color)
                    attack_positions.UnionWith(current_button.get_range_attack(board).Select(m => m.to));
            }
        }
        return attack_positions;
    }

    public static bool is_this_color_in_check(Chess_cell[,] board, Turns color)
    {
        for (int row = 0; row < 8; row++)
        {
            for (int column = 0; column < 8; column++)
            {
                Chess_cell current_button = board[row, column];

                if (current_button.name == Piece_name.king && current_button.color == color)
                    return get_attack_range_of(board, reverse(color)).Contains(new(row, column));
            }
        }
        return true;
    }

    public static bool is_this_color_in_checkmate(Chess_cell[,] board, Turns color)
        => is_this_color_in_check(board, color) && get_all_moves(board, color).Count == 0;

    private static bool is_board_contains_only(Chess_cell[,] board, HashSet<Piece_name> names, Turns color)
    {
        for (int row = 0; row < 8; row++)
        {
            for (int column = 0; column < 8; column++)
            {
                Chess_cell temp = board[row, column];

                if (temp.is_None() || temp.color != color)
                    continue;

                else if (names.Contains(temp.name))
                    names.Remove(temp.name);

                else return false;
            }
        }
        return names.Count == 0;
    }

    public static bool is_draw(Chess_cell[,] board, Turns color)
        => get_all_moves(board, color).Count == 0 && !is_this_color_in_check(board, color) ||

            (is_board_contains_only(board, [Piece_name.king, Piece_name.bishop], Turns.white) ||
             is_board_contains_only(board, [Piece_name.king, Piece_name.knight], Turns.white) ||
             is_board_contains_only(board, [Piece_name.king], Turns.white)) &&

            (is_board_contains_only(board, [Piece_name.king, Piece_name.bishop], Turns.black) ||
             is_board_contains_only(board, [Piece_name.king, Piece_name.knight], Turns.black) ||
             is_board_contains_only(board, [Piece_name.king], Turns.black));

    public static bool is_valid_move(Chess_cell[,] board, Move move, Turns color, bool is_king_moved)
        => !is_this_color_in_check(generate_future_board(board, move, is_king_moved), color);

    public static Move attempt_castling(Chess_cell new_king_pos, Turns color)
    {
        if (color == Turns.white && new_king_pos.pos.row == 7)
        {
            if (new_king_pos.pos.col == 6)
                return new(new(7, 5), new(7, 7));

            else if (new_king_pos.pos.col == 2)
                return new(new(7, 3), new(7, 0));
        }
        else if (color == Turns.black && new_king_pos.pos.row == 0)
        {
            if (new_king_pos.pos.col == 6)
                return new(new(0, 5), new(0, 7));

            else if (new_king_pos.pos.col == 2)
                return new(new(0, 3), new(0, 0));
        }

        return new();
    }

    public static Chess_cell[,] generate_future_board(Chess_cell[,] board, Move move, bool is_king_moved)
    {
        Chess_cell[,] future_board = new Chess_cell[8, 8];

        for (int row = 0; row < 8; row++)
            for (int column = 0; column < 8; column++)
                future_board[row, column] = board[row, column].copy();

        Turns color = future_board[move.from.row, move.from.col].color;

        if (future_board[move.to.row, move.to.col].is_None() && future_board[move.from.row, move.from.col].name == Piece_name.pawn)
        {
            int en_passant_row = (color == Turns.white) ? 1 : -1;
            if (new Pos(move.to.row + en_passant_row, move.to.col).isin_board_range())
                if (future_board[move.to.row + en_passant_row, move.to.col].name == Piece_name.pawn &&
                    future_board[move.to.row + en_passant_row, move.to.col].color != color)
                    future_board[move.to.row + en_passant_row, move.to.col].name = Piece_name.None;
        }

        future_board[move.from.row, move.from.col].move_to(ref future_board[move.to.row, move.to.col]);

        switch (future_board[move.to.row, move.to.col].name)
        {
            case Piece_name.pawn:
                if (move.to.row == 0 || move.to.row == 7)
                    future_board[move.to.row, move.to.col].name = Chess_bot.find_best_pawn_transformation(future_board, move.to, color);
                else
                {
                    int start_position = (color == Turns.white) ? 6 : 1;
                    int end_position = (color == Turns.white) ? 4 : 3;

                    if (move.from.row == start_position && move.to.row == end_position)
                        future_board[move.to.row, move.to.col].is_pawn_double_moved = true;
                }
                break;

            case Piece_name.king:
                {
                    if (is_king_moved) break;

                    Move rook_castling_moves = attempt_castling(future_board[move.to.row, move.to.col], color);

                    if (rook_castling_moves.is_None()) break;

                    future_board[rook_castling_moves.from.row, rook_castling_moves.from.col]
                        .move_to(ref future_board[rook_castling_moves.to.row, rook_castling_moves.to.col]);

                    break;
                }

            default: break;
        }

        return future_board;
    }
}
