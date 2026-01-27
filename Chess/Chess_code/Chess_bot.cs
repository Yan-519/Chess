namespace Chess;

using static Board_values;
using static Chess_tools;

public class Chess_bot : Chess_player_root
{
    private static readonly Random random = new();

    private readonly Bot_levels bot_level;

    public Chess_bot(Turns bot_color, Bot_levels bot_level, ref Move first_move) :
        base(bot_color)
    {
        this.bot_level = bot_level;

        if (color_of_this == turn)
        {
            first_move = get_move();
            check_and_make_move(first_move);
        }
        else first_move = new();
    }

    public Chess_bot() : base(Turns.black) => bot_level = default;

    private static double calculate_move_value(Chess_cell[,] board, Move move, Turns color, Move_bools bools, Draw_data draw_data)
    {
        (Chess_cell[,] future, _, Draw_data new_draw_data) = generate_future_board(board, move, bools, draw_data, color);
        return calculate_move_value(future, color, new_draw_data, board[move.from.row, move.from.col].name == Piece_name.king);
    }

    private static double calculate_move_value(Chess_cell[,] future, Turns color, Draw_data draw_data, bool is_king_move)
    {
        static double calculate_board_value_diff(Chess_cell[,] board, Turns color, bool is_king_move)
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
                        Piece_name.pawn => pawn_modifiers_to_down[(color == Turns.white) ? pawn_modifiers_to_down.Length - row - 1 : row],
                        Piece_name.rook => rook_modifier[row, column],
                        _ => piece_position_modifiers[row, column]
                    }) * ((current.color == color) ? 1 : -1);
                }
            }
            if (is_king_move)
                score += AVOID_KING_MOVES_COST;

            return score;
        }

        if (is_this_color_in_checkmate(future, reverse(color), draw_data))
            return double.MaxValue;

        if (is_draw(future, reverse(color), draw_data))
            return double.MinValue;

        if (is_this_color_in_checkmate(future, color, draw_data))
            return double.NegativeInfinity;

        return calculate_board_value_diff(future, color, is_king_move);
    }


    private static Move get_easy_move(Chess_cell[,] board, Turns color, Move_bools move_bools, Draw_data draw_data)
        => get_easy_move(board, color, move_bools, draw_data, [], []);

    private static Move get_easy_move(Chess_cell[,] board, Turns color, Move_bools move_bools, Draw_data draw_data,
        HashSet<Move> bot_moves, Dictionary<Move, (Chess_cell[,], Move_bools, Draw_data)> future_board_from_original)
    {
        if (bot_moves.Count == 0)
        {
            bot_moves = get_all_moves(board, color, move_bools, draw_data);

            if (bot_moves.Count == 0)
                return new();
            else if (bot_moves.Count == 1)
                return bot_moves.ToArray().First();

            future_board_from_original = [];

            foreach (Move bot_move in bot_moves)
            {
                (Chess_cell[,] future_board, Move_bools is_moved_second, Draw_data new_draw_data) = generate_future_board(board, bot_move, move_bools, draw_data, reverse(color));
                future_board_from_original[bot_move] = (future_board, is_moved_second, new_draw_data);

                if (is_this_color_in_checkmate(future_board, reverse(color), new_draw_data))
                    return bot_move;
            }
        }

        Move best_move = new();

        double best_score = double.MinValue;

        foreach (Move bot_move in bot_moves)
        {
            (Chess_cell[,] future_board_first, _, Draw_data new_draw_data) = future_board_from_original[bot_move];
            double current_score = calculate_move_value(future_board_first, color, new_draw_data, board[bot_move.from.row, bot_move.from.col].name == Piece_name.king);

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

    private static Move get_normal_move(Chess_cell[,] board, Turns color, Move_bools bot_move_bools, Move_bools player_move_bools, Draw_data draw_data,
        HashSet<Move> bot_moves, Dictionary<Move, (Chess_cell[,], Move_bools, Draw_data)> future_board_from_original)
    {
        if (bot_moves.Count == 0)
        {
            bot_moves = get_all_moves(board, color, bot_move_bools, draw_data);

            if (bot_moves.Count == 0)
                return new();
            else if (bot_moves.Count == 1)
                return bot_moves.ToArray().First();

            future_board_from_original = [];

            foreach (Move bot_move in bot_moves)
            {
                (Chess_cell[,] future_board, Move_bools is_moved_second, Draw_data new_draw_data) = generate_future_board(board, bot_move, bot_move_bools, draw_data, reverse(color));
                future_board_from_original[bot_move] = (future_board, is_moved_second, new_draw_data);

                if (is_this_color_in_checkmate(future_board, reverse(color), new_draw_data))
                    return bot_move;
            }
        }

        Move best_move = new();

        double best_score = double.MinValue;

        foreach (Move bot_move in bot_moves)
        {
            (Chess_cell[,] future_board_first, _, Draw_data new_draw_data) = future_board_from_original[bot_move];
            if (new_draw_data.is_draw)
                continue;

            Move best_player_move = get_easy_move(future_board_first, reverse(color), player_move_bools, new_draw_data);
            if (best_player_move.is_None())
                continue;

            double current_score = calculate_move_value(future_board_first, best_player_move, color, bot_move_bools, new_draw_data);

            if (current_score == double.MaxValue)
                return bot_move;

            else if (current_score > best_score || (current_score == best_score && random.Next(2) == 0))
            {
                best_move = bot_move;
                best_score = current_score;
            }
        }

        if (best_move.is_None())
            return get_easy_move(board, color, bot_move_bools, draw_data, bot_moves, future_board_from_original);

        return best_move;
    }

    private static Move get_best_move(Chess_cell[,] board, Turns color, Move_bools bot_move_bools, Move_bools player_move_bools, Draw_data draw_data)
    {
        HashSet<Move> bot_moves = get_all_moves(board, color, bot_move_bools, draw_data);

        if (bot_moves.Count == 0)
            return new();
        else if (bot_moves.Count == 1)
            return bot_moves.ToArray().First();

        Dictionary<Move, (Chess_cell[,], Move_bools, Draw_data)> future_board_from_original = [];

        foreach (Move bot_move in bot_moves)
        {
            (Chess_cell[,] future_board, Move_bools is_moved_second, Draw_data new_draw_data) = generate_future_board(board, bot_move, bot_move_bools, draw_data, reverse(color));
            future_board_from_original[bot_move] = (future_board, is_moved_second, new_draw_data);

            if (is_this_color_in_checkmate(future_board, reverse(color), new_draw_data))
                return bot_move;
        }

        Move best_move = new();

        double best_score = double.MinValue;

        foreach (Move bot_move in bot_moves)
        {
            (Chess_cell[,] future_board_first, Move_bools is_moved_second, Draw_data new_draw_data) = future_board_from_original[bot_move];
            if (new_draw_data.is_draw)
                continue;

            Move best_player_move = get_easy_move(future_board_first, reverse(color), player_move_bools, new_draw_data);

            (Chess_cell[,] future_board_second, _, new_draw_data) = generate_future_board(future_board_first, best_player_move, is_moved_second, new_draw_data, reverse(color));
            if (new_draw_data.is_draw)
                continue;

            Move best_bot_move = get_easy_move(future_board_second, color, is_moved_second, new_draw_data);
            if (best_player_move.is_None())
                continue;

            double current_score = calculate_move_value(future_board_second, best_player_move, color, is_moved_second, new_draw_data);

            if (current_score == double.MaxValue)
                return bot_move;

            else if (current_score > best_score || (current_score == best_score && random.Next(2) == 0))
            {
                best_move = bot_move;
                best_score = current_score;
            }
        }

        if (best_move.is_None())
            return get_normal_move(board, color, bot_move_bools, player_move_bools, draw_data, bot_moves, future_board_from_original);

        return best_move;
    }


    public static Piece_name find_best_pawn_transformation(Chess_cell[,] board, Pos pos_of_pawn, Turns color, Draw_data draw_data)
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

            double current_score = calculate_move_value(temp, color, draw_data, false);
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
        if (turn == color_of_this || is_game_over || player_move.is_None())
            return new();

        check_and_make_move(player_move);

        if (is_game_over)
            return new();

        Move response = get_move();

        check_and_make_move(response, true);

        return response;
    }

    public Move get_move() => bot_level switch
    {
        Bot_levels.easy => get_easy_move(_board, color_of_this, get_move_bools(color_of_this), draw_data),
        Bot_levels.normal => get_normal_move(_board, color_of_this, get_move_bools(color_of_this), get_move_bools(color_of_opponent), draw_data, [], []),
        Bot_levels.hard => get_best_move(_board, color_of_this, get_move_bools(color_of_this), get_move_bools(color_of_opponent), draw_data),
        _ => new(),
    };
}
