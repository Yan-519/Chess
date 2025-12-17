namespace Chess;

using Bot_levels = Chess_game.Bot_levels;
using Loose_type = Chess_game.Loose_type;
using Piece_name = Chess_game.Piece_name;
using Turns = Chess_game.Turns;

public class Chess_bot : Chess_player_root
{
    private static readonly Random random = new();

    private static readonly double[,] piece_position_modifiers = new double[8, 8]
{
            {-0.5, -0.4, -0.4, -0.4, -0.4, -0.4, -0.4, -0.5},
            {-0.4, -0.2,  0.0,  0.0,  0.0,  0.0, -0.2, -0.4},
            {-0.4,  0.0,  0.1,  0.2,  0.2,  0.1,  0.0, -0.4},
            {-0.4,  0.0,  0.2,  0.25, 0.25, 0.2,  0.0, -0.4},
            {-0.4,  0.0,  0.2,  0.25, 0.25, 0.2,  0.0, -0.4},
            {-0.4,  0.0,  0.1,  0.2,  0.2,  0.1,  0.0, -0.4},
            {-0.4, -0.2,  0.0,  0.0,  0.0,  0.0, -0.2, -0.4},
            {-0.5, -0.4, -0.4, -0.4, -0.4, -0.4, -0.4, -0.5}
};

    private static readonly double[,] king_position_modifiers = new double[8, 8]
        {
            {0.25, 0.2,  0.2,  0.2,  0.2,  0.2, 0.2, 0.25},
            {0.2,  0.1,  0.0,  0.0,  0.0,  0.0, 0.1, 0.2 },
            {0.2,  0.0, -0.2, -0.4, -0.4, -0.2, 0.0, 0.2 },
            {0.2,  0.0, -0.4, -0.5, -0.5, -0.4, 0.0, 0.2 },
            {0.2,  0.0, -0.4, -0.5, -0.5, -0.4, 0.0, 0.2 },
            {0.2,  0.0, -0.2, -0.4, -0.4, -0.2, 0.0, 0.2 },
            {0.2,  0.1,  0.0,  0.0,  0.0,  0.0, 0.1, 0.2 },
            {0.25, 0.2,  0.2,  0.2,  0.2,  0.2, 0.2, 0.25}
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
            set_move(first_move, turn);
        }
        else first_move = new();
    }

    public Chess_bot() : base(Turns.black) => bot_level = Bot_levels.easy;

    private static double calculate_move_value(Piece_characteristic[,] board, Move move, Turns color, bool is_king_moved)
        => calculate_board_value_diff(Chess_game.generate_future_board(board, move, is_king_moved), color);

    private static double calculate_move_value(Piece_characteristic[,] future, Turns color)
    {
        if (Chess_game.is_this_color_in_checkmate(future, Chess_game.reverse(color)))
            return double.MaxValue;

        if (Chess_game.is_draw(future, Chess_game.reverse(color)))
            return double.MinValue;

        if (Chess_game.is_this_color_in_checkmate(future, color))
            return double.NegativeInfinity;

        return calculate_board_value_diff(future, color);
    }

    private static double calculate_board_value_diff(Piece_characteristic[,] board, Turns color)
    {
        double score = 0;

        for (int row = 0; row < 8; row++)
        {
            for (int column = 0; column < 8; column++)
            {
                Piece_characteristic current = board[row, column];

                if (current.is_None()) continue;

                int sign = (current.color == color) ? 1 : -1;

                score += current.cost * sign;

                switch (current.name)
                {
                    case Piece_name.king:
                        score += king_position_modifiers[row, column] * sign; break;

                    case Piece_name.pawn:
                        score += ((color == Turns.white) ? pawn_modifiers_to_up[row, column] : pawn_modifiers_to_down[row, column]) * sign;
                        break;

                    case Piece_name.rook:
                        score += rook_modifier[row, column] * sign; break;

                    default:
                        score += piece_position_modifiers[row, column] * sign; break;
                }
            }
        }

        return score;
    }

    private static Move get_easy_move(Piece_characteristic[,] board, Turns color,
        bool is_king_moved_bot, bool is_left_rook_moved_bot, bool is_right_rook_moved_bot)

        => get_easy_move(board, color, is_king_moved_bot, is_left_rook_moved_bot, is_right_rook_moved_bot, [], []);

    private static Move get_easy_move(Piece_characteristic[,] board, Turns color,
        bool is_king_moved_bot, bool is_left_rook_moved_bot, bool is_right_rook_moved_bot,
        HashSet<Move> bot_moves, Dictionary<Move, Piece_characteristic[,]> future_board_from_original)
    {
        if (bot_moves.Count == 0)
        {
            bot_moves = Chess_game.get_all_moves(board, color, is_king_moved_bot, is_left_rook_moved_bot, is_right_rook_moved_bot);

            if (bot_moves.Count == 0)
                return new();
            else if (bot_moves.Count == 1)
                return bot_moves.ToArray().First();

            future_board_from_original = [];

            foreach (Move bot_move in bot_moves)
            {
                Piece_characteristic[,] future_board = Chess_game.generate_future_board(board, bot_move, is_king_moved_bot);
                future_board_from_original[bot_move] = future_board;

                if (Chess_game.is_this_color_in_checkmate(future_board, Chess_game.reverse(color)))
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

    private static Move get_normal_move(Piece_characteristic[,] board, Turns color,
        bool is_king_moved_bot, bool is_left_rook_moved_bot, bool is_right_rook_moved_bot,
        bool is_king_moved_player, bool is_left_rook_moved_player, bool is_right_rook_moved_player)

        => get_normal_move(board, color, is_king_moved_bot, is_left_rook_moved_bot, is_right_rook_moved_bot,
            is_king_moved_player, is_left_rook_moved_player, is_right_rook_moved_player, [], []);

    private static Move get_normal_move(Piece_characteristic[,] board, Turns color,
        bool is_king_moved_bot, bool is_left_rook_moved_bot, bool is_right_rook_moved_bot,
        bool is_king_moved_player, bool is_left_rook_moved_player, bool is_right_rook_moved_player,
        HashSet<Move> bot_moves, Dictionary<Move, Piece_characteristic[,]> future_board_from_original)
    {
        if (bot_moves.Count == 0)
        {
            bot_moves = Chess_game.get_all_moves(board, color, is_king_moved_bot, is_left_rook_moved_bot, is_right_rook_moved_bot);

            if (bot_moves.Count == 0)
                return new();
            else if (bot_moves.Count == 1)
                return bot_moves.ToArray().First();

            future_board_from_original = [];

            foreach (Move bot_move in bot_moves)
            {
                Piece_characteristic[,] future_board = Chess_game.generate_future_board(board, bot_move, is_king_moved_bot);
                future_board_from_original[bot_move] = future_board;

                if (Chess_game.is_this_color_in_checkmate(future_board, Chess_game.reverse(color)))
                    return bot_move;
            }
        }

        Move best_move = new();

        double best_score = double.MinValue;

        foreach (Move bot_move in bot_moves)
        {
            Piece_characteristic[,] future_board_first = future_board_from_original[bot_move];

            Move best_player_move = get_easy_move(future_board_first, Chess_game.reverse(color),
                is_king_moved_player, is_left_rook_moved_player, is_right_rook_moved_player);
            if (best_player_move.is_None())
                continue;

            double current_score = calculate_move_value(future_board_first, best_player_move, color, is_king_moved_bot);

            if (current_score == double.MaxValue)
                return bot_move;

            else if (current_score > best_score || (current_score == best_score && random.Next(2) == 0))
            {
                best_move = bot_move;
                best_score = current_score;
            }
        }

        if (best_move.is_None())
            return get_easy_move(board, color, is_king_moved_bot, is_left_rook_moved_bot, is_right_rook_moved_bot, bot_moves, future_board_from_original);

        return best_move;
    }

    private static Move get_best_move(Piece_characteristic[,] board, Turns color,
                                bool is_king_moved_bot, bool is_left_rook_moved_bot, bool is_right_rook_moved_bot,
                                bool is_king_moved_player, bool is_left_rook_moved_player, bool is_right_rook_moved_player)
    {
        HashSet<Move> bot_moves = Chess_game.get_all_moves(board, color, is_king_moved_bot, is_left_rook_moved_bot, is_right_rook_moved_bot);

        if (bot_moves.Count == 0)
            return new();
        else if (bot_moves.Count == 1)
            return bot_moves.ToArray().First();

        Dictionary<Move, Piece_characteristic[,]> future_board_from_original = [];

        foreach (Move bot_move in bot_moves)
        {
            Piece_characteristic[,] future_board = Chess_game.generate_future_board(board, bot_move, is_king_moved_bot);
            future_board_from_original[bot_move] = future_board;

            if (Chess_game.is_this_color_in_checkmate(future_board, Chess_game.reverse(color)))
                return bot_move;
        }

        Move best_move = new();

        double best_score = double.MinValue;

        foreach (Move bot_move in bot_moves)
        {
            Piece_characteristic[,] future_board_first = future_board_from_original[bot_move];

            bool is_king_moved_second = is_king_moved_bot,
                is_left_rook_moved_second = is_left_rook_moved_bot,
                is_right_rook_moved_second = is_right_rook_moved_bot;

            Piece_name piece = future_board_first[bot_move.from.row, bot_move.from.col].name;

            if (piece == Piece_name.king)
                is_king_moved_second = true;
            else if (piece == Piece_name.rook)
            {
                if (bot_move.from.col == 0)
                    is_left_rook_moved_second = true;
                else if (bot_move.from.col == 6)
                    is_right_rook_moved_second = true;
            }

            Move best_player_move = get_easy_move(future_board_first, Chess_game.reverse(color),
                is_king_moved_player, is_left_rook_moved_player, is_right_rook_moved_player);
            //, is_king_moved_second, is_left_rook_moved_second, is_right_rook_moved_second);
            if (best_player_move.is_None())
                continue;

            //bool is_player_king_moved_second = is_king_moved_player,
            //    is_player_left_rook_moved_second = is_left_rook_moved_player,
            //    is_player_right_rook_moved_second = is_right_rook_moved_player;
            //Piece_name player_piece = future_board_first[best_bot_move.from.row, best_bot_move.from.col].name;

            //if (player_piece == Piece_name.king)
            //    is_player_king_moved_second = true;
            //else if (player_piece == Piece_name.rook)
            //{
            //    if (best_bot_move.from.col == 0)
            //        is_player_left_rook_moved_second = true;
            //    else if (best_bot_move.from.col == 6)
            //        is_player_right_rook_moved_second = true;
            //}


            Piece_characteristic[,] future_board_second = Chess_game.generate_future_board(future_board_first, best_player_move, is_king_moved_second);

            Move best_bot_move = get_easy_move(future_board_second, color, is_king_moved_second, is_left_rook_moved_second, is_right_rook_moved_second);
            //, is_player_king_moved_second, is_player_left_rook_moved_second, is_player_right_rook_moved_second);
            if (best_player_move.is_None())
                continue;

            double current_score = calculate_move_value(future_board_second, best_player_move, color, is_king_moved_second);

            if (current_score == double.MaxValue)
                return bot_move;

            else if (current_score > best_score || (current_score == best_score && random.Next(2) == 0))
            {
                best_move = bot_move;
                best_score = current_score;
            }
        }

        if (best_move.is_None())
            return get_normal_move(board, color,
                is_king_moved_bot, is_left_rook_moved_bot, is_right_rook_moved_bot,
                is_king_moved_player, is_left_rook_moved_player, is_right_rook_moved_player,
                bot_moves, future_board_from_original);

        return best_move;
    }


    public static Piece_name find_best_pawn_transformation(Piece_characteristic[,] board, Pos pos_of_pawn, Turns color)
    {
        double best_score = double.MinValue;
        Piece_name best_transformation = Piece_name.None;

        foreach (Piece_name name in new[] { Piece_name.queen, Piece_name.rook, Piece_name.bishop, Piece_name.knight })
        {
            Piece_characteristic[,] temp = (Piece_characteristic[,])board.Clone();
            temp[pos_of_pawn.row, pos_of_pawn.col].name = name;

            if (Chess_game.is_this_color_in_check(temp, color))
                continue;

            else if (Chess_game.is_this_color_in_check(temp, Chess_game.reverse(color)))
                return name;

            double current_score = calculate_board_value_diff(temp, color) - calculate_board_value_diff(temp, Chess_game.reverse(color));

            Random random = new();

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

        set_move(player_move, turn);

        if (end_game_type != Loose_type.game_gos)
            return new();

        Move response = get_move();

        set_move(response, turn, true);

        return response;
    }

    public Move get_move() => bot_level switch
    {
        Bot_levels.easy => get_easy_move(_board, color_of_this, is_king_moved_g(color_of_this), is_left_rook_moved_g(color_of_this), is_right_rook_moved_g(color_of_this)),
        Bot_levels.normal => get_normal_move(_board, color_of_this,
                is_king_moved_g(color_of_this), is_left_rook_moved_g(color_of_this), is_right_rook_moved_g(color_of_this),
                is_king_moved_g(color_of_opponent), is_left_rook_moved_g(color_of_opponent), is_right_rook_moved_g(color_of_opponent)),
        Bot_levels.hard => get_best_move(_board, color_of_this,
                is_king_moved_g(color_of_this), is_left_rook_moved_g(color_of_this), is_right_rook_moved_g(color_of_this),
                is_king_moved_g(color_of_opponent), is_left_rook_moved_g(color_of_opponent), is_right_rook_moved_g(color_of_opponent)),
        _ => new(),
    };
}
