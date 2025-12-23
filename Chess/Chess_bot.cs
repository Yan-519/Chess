namespace Chess;

using static Chess_game;

public class Chess_bot : Chess_player_root
{
    private static readonly Random random = new();

    private static readonly double[,] piece_position_modifiers = new double[8, 8]
    {
            { -0.5, -0.4, -0.4, -0.4, -0.4, -0.4, -0.4, -0.5},
            { -0.4, -0.2,  0.0,  0.0,  0.0,  0.0, -0.2, -0.4},
            { -0.4,  0.0,  0.1,  0.2,  0.2,  0.1,  0.0, -0.4},
            { -0.4,  0.0,  0.2,  0.25, 0.25, 0.2,  0.0, -0.4},
            { -0.4,  0.0,  0.2,  0.25, 0.25, 0.2,  0.0, -0.4},
            { -0.4,  0.0,  0.1,  0.2,  0.2,  0.1,  0.0, -0.4},
            { -0.4, -0.2,  0.0,  0.0,  0.0,  0.0, -0.2, -0.4},
            { -0.5, -0.4, -0.4, -0.4, -0.4, -0.4, -0.4, -0.5}
    };

    private static readonly double[,] king_position_modifiers = new double[8, 8]
    {
            { 0.25, 0.2,  0.2,  0.2,  0.2,  0.2, 0.2, 0.25},
            { 0.2,  0.1,  0.0,  0.0,  0.0,  0.0, 0.1, 0.2 },
            { 0.2,  0.0, -0.2, -0.4, -0.4, -0.2, 0.0, 0.2 },
            { 0.2,  0.0, -0.4, -0.5, -0.5, -0.4, 0.0, 0.2 },
            { 0.2,  0.0, -0.4, -0.5, -0.5, -0.4, 0.0, 0.2 },
            { 0.2,  0.0, -0.2, -0.4, -0.4, -0.2, 0.0, 0.2 },
            { 0.2,  0.1,  0.0,  0.0,  0.0,  0.0, 0.1, 0.2 },
            { 0.25, 0.2,  0.2,  0.2,  0.2,  0.2, 0.2, 0.25}
    };

    private static readonly double[,] pawn_modifiers_to_up = new double[8, 8]
    {
            { 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00 },
            { 0.05, 0.05, 0.05, 0.05, 0.05, 0.05, 0.05, 0.05 },
            { 0.10, 0.10, 0.10, 0.10, 0.10, 0.10, 0.10, 0.10 },
            { 0.15, 0.15, 0.15, 0.15, 0.15, 0.15, 0.15, 0.15 },
            { 0.20, 0.20, 0.20, 0.20, 0.20, 0.20, 0.20, 0.20 },
            { 0.25, 0.25, 0.25, 0.25, 0.25, 0.25, 0.25, 0.25 },
            { 0.30, 0.30, 0.30, 0.30, 0.30, 0.30, 0.30, 0.30 },
            { 0.35, 0.35, 0.35, 0.35, 0.35, 0.35, 0.35, 0.35 }
    };

    private static readonly double[,] pawn_modifiers_to_down = new double[8, 8]
    {
            { 0.35, 0.35, 0.35, 0.35, 0.35, 0.35, 0.35, 0.35 },
            { 0.30, 0.30, 0.30, 0.30, 0.30, 0.30, 0.30, 0.30 },
            { 0.25, 0.25, 0.25, 0.25, 0.25, 0.25, 0.25, 0.25 },
            { 0.20, 0.20, 0.20, 0.20, 0.20, 0.20, 0.20, 0.20 },
            { 0.15, 0.15, 0.15, 0.15, 0.15, 0.15, 0.15, 0.15 },
            { 0.10, 0.10, 0.10, 0.10, 0.10, 0.10, 0.10, 0.10 },
            { 0.05, 0.05, 0.05, 0.05, 0.05, 0.05, 0.05, 0.05 },
            { 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00 }
    };

    private static readonly double[,] rook_modifier = new double[8, 8]
    {
            { 0    , -0.10, 0.00, 0.00, 0.00, 0.00, -0.10, 0     },
            { -0.10, 0.05 , 0.05, 0.05, 0.05, 0.05, 0.05 , -0.10 },
            { 0    , 0.10 , 0.10, 0.10, 0.10, 0.10, 0.10 , 0     },
            { 0.20 , 0.20 , 0.20, 0.20, 0.20, 0.20, 0.20 , 0.20  },
            { 0.15 , 0.15 , 0.15, 0.15, 0.15, 0.15, 0.15 , 0.15  },
            { 0    , 0.10 , 0.10, 0.10, 0.10, 0.10, 0.10 , 0     },
            { -0.10, 0.05 , 0.05, 0.05, 0.05, 0.05, 0.05 , -0.10 },
            { 0    , -0.10, 0.00, 0.00, 0.00, 0.00, -0.10, 0     }
    };

    private readonly Bot_levels bot_level;

    public Chess_bot(Turns bot_color, Bot_levels bot_Level, ref Move first_move) :
        base(bot_color)
    {
        this.bot_level = bot_Level;

        if (color_of_this == turn)
        {
            first_move = get_move();
            set_move(first_move);
        }
        else first_move = new();
    }

    public Chess_bot() : base(Turns.black) => bot_level = Bot_levels.easy;

    private static double calculate_move_value(Chess_cell[,] board, Move move, Turns color, bool is_king_moved)
        => calculate_move_value(generate_future_board(board, move, is_king_moved), color);

    private static double calculate_move_value(Chess_cell[,] future, Turns color)
    {
        static double calculate_board_value_diff(Chess_cell[,] board, Turns color)
        {
            double score = 0;

            for (int row = 0; row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    Chess_cell current = board[row, column];

                    if (current.is_None()) continue;

                    score += (current.cost + current.name switch
                    {
                        Piece_name.knight => king_position_modifiers[row, column],
                        Piece_name.pawn => (color == Turns.white) ? pawn_modifiers_to_up[row, column] : pawn_modifiers_to_down[row, column],
                        Piece_name.rook => rook_modifier[row, column],
                        _ => piece_position_modifiers[row, column]
                    }) * ((current.color == color) ? 1 : -1);
                }
            }
            return score;
        }

        if (is_this_color_in_checkmate(future, reverse(color)))
            return double.MaxValue;

        if (is_draw(future, reverse(color)))
            return double.MinValue;

        if (is_this_color_in_checkmate(future, color))
            return double.NegativeInfinity;

        return calculate_board_value_diff(future, color);
    }


    private static Move get_easy_move(Chess_cell[,] board, Turns color, Move_bools move_bools)
        => get_easy_move(board, color, move_bools, [], []);

    private static Move get_easy_move(Chess_cell[,] board, Turns color, Move_bools move_bools,
        HashSet<Move> bot_moves, Dictionary<Move, Chess_cell[,]> future_board_from_original)
    {
        if (bot_moves.Count == 0)
        {
            bot_moves = get_all_moves(board, color, move_bools);

            if (bot_moves.Count == 0)
                return new();
            else if (bot_moves.Count == 1)
                return bot_moves.ToArray().First();

            future_board_from_original = [];

            foreach (Move bot_move in bot_moves)
            {
                Chess_cell[,] future_board = generate_future_board(board, bot_move, move_bools.is_king_moved);
                future_board_from_original[bot_move] = future_board;

                if (is_this_color_in_checkmate(future_board, reverse(color)))
                    return bot_move;
            }
        }

        Move best_move = new();

        double best_score = double.MinValue;

        foreach (Move bot_move in bot_moves)
        {
            double current_score = calculate_move_value(future_board_from_original[bot_move], color);

            if (current_score == double.MaxValue)
                return bot_move;

            if (current_score > best_score || (current_score == best_score && random.Next(2) == 0))
            {
                best_move = bot_move;
                best_score = current_score;
            }
        }

        if (best_move.is_None())
            return bot_moves.ToArray()[random.Next(bot_moves.Count)];

        return best_move;
    }

    private static Move get_normal_move(Chess_cell[,] board, Turns color, Move_bools bot_move_bools, Move_bools player_move_bools,
        HashSet<Move> bot_moves, Dictionary<Move, Chess_cell[,]> future_board_from_original)
    {
        if (bot_moves.Count == 0)
        {
            bot_moves = get_all_moves(board, color, bot_move_bools);

            if (bot_moves.Count == 0)
                return new();
            else if (bot_moves.Count == 1)
                return bot_moves.ToArray().First();

            future_board_from_original = [];

            foreach (Move bot_move in bot_moves)
            {
                Chess_cell[,] future_board = generate_future_board(board, bot_move, bot_move_bools.is_king_moved);
                future_board_from_original[bot_move] = future_board;

                if (is_this_color_in_checkmate(future_board, reverse(color)))
                    return bot_move;
            }
        }

        Move best_move = new();

        double best_score = double.MinValue;

        foreach (Move bot_move in bot_moves)
        {
            Chess_cell[,] future_board_first = future_board_from_original[bot_move];

            Move best_player_move = get_easy_move(future_board_first, reverse(color), player_move_bools);
            if (best_player_move.is_None())
                continue;

            double current_score = calculate_move_value(future_board_first, best_player_move, color, bot_move_bools.is_king_moved);

            if (current_score == double.MaxValue)
                return bot_move;

            else if (current_score > best_score || (current_score == best_score && random.Next(2) == 0))
            {
                best_move = bot_move;
                best_score = current_score;
            }
        }

        if (best_move.is_None())
            return get_easy_move(board, color, bot_move_bools, bot_moves, future_board_from_original);

        return best_move;
    }

    private static Move get_best_move(Chess_cell[,] board, Turns color, Move_bools bot_move_bools, Move_bools player_move_bools)
    {
        HashSet<Move> bot_moves = get_all_moves(board, color, bot_move_bools);

        if (bot_moves.Count == 0)
            return new();
        else if (bot_moves.Count == 1)
            return bot_moves.ToArray().First();

        Dictionary<Move, Chess_cell[,]> future_board_from_original = [];

        foreach (Move bot_move in bot_moves)
        {
            Chess_cell[,] future_board = generate_future_board(board, bot_move, bot_move_bools.is_king_moved);
            future_board_from_original[bot_move] = future_board;

            if (is_this_color_in_checkmate(future_board, reverse(color)))
                return bot_move;
        }

        Move best_move = new();

        double best_score = double.MinValue;

        foreach (Move bot_move in bot_moves)
        {
            Chess_cell[,] future_board_first = future_board_from_original[bot_move];

            Move_bools is_moved_second = bot_move_bools with { };

            Piece_name piece = future_board_first[bot_move.from.row, bot_move.from.col].name;

            if (piece == Piece_name.king)
                is_moved_second = bot_move_bools with { is_king_moved = true };
            else if (piece == Piece_name.rook)
            {
                if (bot_move.from.col == 0)
                    is_moved_second = bot_move_bools with { is_left_rook_moved = true };
                else if (bot_move.from.col == 6)
                    is_moved_second = bot_move_bools with { is_right_rook_moved = true };
            }

            Move best_player_move = get_easy_move(future_board_first, reverse(color), player_move_bools);// is_moved_second, bot_moves, future_board_from_original);
            if (best_player_move.is_None())
                continue;

            //Piece_name player_piece = future_board_first[best_player_move.from.row, best_player_move.from.col].name;
            //Move_bools is_player_moved_second = player_move_bools with { };

            //if (player_piece == Piece_name.king)
            //    is_player_moved_second.is_king_moved = true;
            //else if (player_piece == Piece_name.rook)
            //{
            //    if (best_player_move.from.col == 0)
            //        is_player_moved_second.is_left_rook_moved = true;
            //    else if (best_player_move.from.col == 6)
            //        is_player_moved_second.is_right_rook_moved = true;
            //}

            Chess_cell[,] future_board_second = generate_future_board(future_board_first, best_player_move, is_moved_second.is_king_moved);

            Move best_bot_move = get_easy_move(future_board_second, color, is_moved_second); //, is_player_moved_second);
            if (best_player_move.is_None())
                continue;

            double current_score = calculate_move_value(future_board_second, best_player_move, color, is_moved_second.is_king_moved);

            if (current_score == double.MaxValue)
                return bot_move;

            else if (current_score > best_score || (current_score == best_score && random.Next(2) == 0))
            {
                best_move = bot_move;
                best_score = current_score;
            }
        }

        if (best_move.is_None())
            return get_normal_move(board, color, bot_move_bools, player_move_bools, bot_moves, future_board_from_original);

        return best_move;
    }


    public static Piece_name find_best_pawn_transformation(Chess_cell[,] board, Pos pos_of_pawn, Turns color)
    {
        double best_score = double.MinValue;
        Piece_name best_transformation = Piece_name.None;

        foreach (Piece_name name in new[] { Piece_name.queen, Piece_name.rook, Piece_name.bishop, Piece_name.knight })
        {
            Chess_cell[,] temp = (Chess_cell[,])board.Clone();
            temp[pos_of_pawn.row, pos_of_pawn.col].name = name;

            if (is_this_color_in_check(temp, color))
                continue;

            else if (is_this_color_in_check(temp, reverse(color)))
                return name;

            double current_score = calculate_move_value(temp, color);
            if (current_score == double.MaxValue)
                return name;

            if (current_score > best_score || (current_score == best_score && random.Next(2) == 0))
            {
                best_transformation = name;
                best_score = current_score;
            }
        }

        return best_transformation;
    }

    public Move get_response_for(Move player_move)
    {
        if (turn == color_of_this || end_game_type != Loose_type.game_gos || player_move.is_None())
            return new();

        set_move(player_move);

        if (end_game_type != Loose_type.game_gos)
            return new();

        Move response = get_move();

        set_move(response, true);

        return response;
    }

    public Move get_move() => bot_level switch
    {
        Bot_levels.easy => get_easy_move(_board, color_of_this, get_move_bools(color_of_this)),
        Bot_levels.normal => get_normal_move(_board, color_of_this, get_move_bools(color_of_this), get_move_bools(color_of_opponent), [], []),
        Bot_levels.hard => get_best_move(_board, color_of_this, get_move_bools(color_of_this), get_move_bools(color_of_opponent)),
        _ => new(),
    };
}
