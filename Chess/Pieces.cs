namespace Chess;

using Turns = Chess_game.Turns;

class None_piece
{
    public int cost { get; init; }

    public None_piece() => cost = 0;
    protected None_piece(int cost) => this.cost = cost;

    public virtual HashSet<Move> get_list_of_moves(Game_cell[,] board, Pos pos, Turns color, bool is_range) => [];
}

class Pawn : None_piece
{
    public Pawn() : base(1) { }

    public override HashSet<Move> get_list_of_moves(Game_cell[,] board, Pos pos, Turns color, bool is_range)
        => is_range ? get_range_attack(pos, color) : get_moves(board, pos, color);

    private static HashSet<Move> get_moves(Game_cell[,] board, Pos pos, Turns color)
    {
        if (pos.row == ((color == Turns.white) ? 0 : 7))
            return [];

        HashSet<Move> list = [];

        int control_num = (color == Turns.white) ? -1 : 1;

        Game_cell current_class = board[pos.row + control_num, pos.col];

        if (current_class.is_None())
        {
            list.Add(new(new(pos.row + control_num, pos.col), pos));

            if (pos.row == ((color == Turns.white) ? 6 : 1))
            {
                current_class = board[pos.row + control_num * 2, pos.col];

                if (current_class.is_None())
                    list.Add(new(new(pos.row + control_num * 2, pos.col), pos));
            }
        }

        foreach (int[] i in new int[][] { [0, -1], [7, 1] })
        {
            if (pos.col != i[0])
            {
                current_class = board[pos.row + control_num, pos.col + i[1]];

                if (!current_class.is_None() && current_class.color != color)
                    list.Add(new(new(pos.row + control_num, pos.col + i[1]), pos));

                if (pos.row == ((board[pos.row + control_num, pos.col].color == Turns.white) ? 3 : 4) && board[pos.row, pos.col + i[1]].is_pawn_double_moved)
                {
                    list.Add(new(new(pos.row + control_num, pos.col + i[1]), pos));
                }
            }
        }

        return list;
    }

    private static HashSet<Move> get_range_attack(Pos pos, Turns color)
    {
        if (pos.row == ((color == Turns.white) ? 0 : 7))
            return [];

        HashSet<Move> list = [];

        int control_num = (color == Turns.white) ? -1 : 1;

        foreach (int[] i in new int[][] { [0, -1], [7, 1] })
            if (pos.col != i[0])
                list.Add(new(new(pos.row + control_num, pos.col + i[1]), pos));

        return list;
    }
}

class Bishop : None_piece
{
    private static readonly int[] row_deltas = [-1, 1, -1, 1];
    private static readonly int[] col_deltas = [-1, -1, 1, 1];

    public Bishop() : base(3) { }
    public override HashSet<Move> get_list_of_moves(Game_cell[,] board, Pos pos, Turns color, bool is_range)
    {
        HashSet<Move> list = [];

        int max_steps;

        for (int d = 0; d < 4; d++)
        {
            switch (d)
            {
                case 0: max_steps = Math.Min(pos.col, pos.row); break;

                case 1: max_steps = Math.Min(pos.col, 7 - pos.row); break;

                case 2: max_steps = Math.Min(7 - pos.col, pos.row); break;

                default: max_steps = Math.Min(7 - pos.col, 7 - pos.row); break;
            }

            int new_row = pos.row, new_col = pos.col;

            for (int i = 0; i < max_steps; i++)
            {
                new_row += row_deltas[d];
                new_col += col_deltas[d];

                if (is_range)
                {
                    list.Add(new(new(new_row, new_col), pos));

                    if (!board[new_row, new_col].is_None())
                        break;
                }
                else
                {
                    Game_cell current_class = board[new_row, new_col];

                    if (current_class.is_None() || current_class.color != color)
                        list.Add(new(new(new_row, new_col), pos));

                    if (!current_class.is_None())
                        break;
                }
            }
        }

        return list;
    }
}

class Rook : None_piece
{
    private static readonly int[] row_deltas = [-1, 1, 0, 0];
    private static readonly int[] col_deltas = [0, 0, 1, -1];

    public Rook() : base(5) { }
    public override HashSet<Move> get_list_of_moves(Game_cell[,] board, Pos pos, Turns color, bool is_range)
    {
        HashSet<Move> list = [];

        int[] max_steps = [pos.row, 7 - pos.row, 7 - pos.col, pos.col];


        for (int d = 0; d < 4; d++)
        {
            int new_row = pos.row, new_col = pos.col;

            for (int i = 0; i < max_steps[d]; i++)
            {
                new_row += row_deltas[d];
                new_col += col_deltas[d];

                if (is_range)
                {
                    list.Add(new(new(new_row, new_col), pos));

                    if (!board[new_row, new_col].is_None())
                        break;
                }
                else
                {
                    Game_cell current_class = board[new_row, new_col];

                    if (current_class.is_None() || current_class.color != color)
                        list.Add(new(new(new_row, new_col), pos));

                    if (!current_class.is_None())
                        break;
                }
            }
        }

        return list;

    }
}

class Knight : None_piece
{
    public Knight() :base(3) { }

    public override HashSet<Move> get_list_of_moves(Game_cell[,] board, Pos pos, Turns color, bool is_range)
    {
        HashSet<Move> list = [];

        for (int delta_row = -2; delta_row <= 2; delta_row++)
        {
            for (int delta_column = -2; delta_column <= 2; delta_column++)
            {
                if (Math.Abs(delta_row) + Math.Abs(delta_column) != 3)
                    continue;

                Pos new_pos = new(pos.row + delta_row, pos.col + delta_column);

                if (new_pos.isin_board_range())
                {
                    if (is_range)
                        list.Add(new(new_pos, pos));

                    else if (board[new_pos.row, new_pos.col] is Game_cell current_class && (current_class.is_None() || current_class.color != color))
                        list.Add(new(new_pos, pos));
                }
            }
        }

        return list;
    }
}

class Queen : None_piece
{
    public Queen() : base(9) { }

    public override HashSet<Move> get_list_of_moves(Game_cell[,] board, Pos pos, Turns color, bool is_range)
        =>  new Rook().get_list_of_moves(board, pos, color, is_range).Union(
            new Bishop().get_list_of_moves(board, pos, color, is_range)).ToHashSet();
}

class King : None_piece
{
    private static readonly int[] row_offsets = [-1, -1, -1, 0, 1, 1, 1, 0];
    private static readonly int[] col_offsets = [0, -1, 1, -1, 0, -1, 1, 1];

    public King() : base(100) { }

    public override HashSet<Move> get_list_of_moves(Game_cell[,] board, Pos pos, Turns color, bool is_range)
    {
        HashSet<Move> list = [];

        for (int index = 0; index < 8; index++)
        {
            Pos new_pos = new(pos.row + row_offsets[index], pos.col + col_offsets[index]);

            if (new_pos.isin_board_range())
            {
                if (is_range)
                    list.Add(new(new_pos, pos));

                else if (board[new_pos.row, new_pos.col] is Game_cell current_class && (current_class.is_None() || current_class.color != color))
                    list.Add(new(new_pos, pos));
            }
        }

        return list;
    }
}
