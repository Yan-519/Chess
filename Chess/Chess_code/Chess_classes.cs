namespace Chess;

public class Draw_data
{
    internal class Prev_move_memo
    {
        private const int SIZE = 9;
        private Move[] moves = new Move[SIZE];
        public bool is_repeated { get; private set; } = false;
        private readonly Func<int, int> mod_that_works = n => ((n % SIZE) + SIZE) % SIZE;

        private int head_index = 0;
        private bool is_made_loop = false;

        public Prev_move_memo()
        {
            for (int i = 0; i < SIZE; i++)
                moves[i] = new();
        }

        private Prev_move_memo(Prev_move_memo prev)
        {
            head_index = prev.head_index;
            is_made_loop = prev.is_made_loop;

            for (int i = 0; i < SIZE; i++)
                moves[i] = prev.moves[i].copy();
        }

        public void Push(Move move)
        {
            head_index++;
            is_made_loop = head_index == SIZE || is_made_loop;
            head_index %= SIZE;
            moves[head_index] = move;
            if (is_made_loop)
                is_repeated = repeated_check();
        }

        private bool repeated_check()
        {
            for (int i = head_index; mod_that_works(i - 2) != head_index; i = mod_that_works(i - 1))
                if (moves[i].from != moves[mod_that_works(i - 2)].to)
                    return false;
            return true;
        }

        public Prev_move_memo Push_get(Move move)
        {
            Prev_move_memo prev = new(this);
            prev.Push(move);
            return prev;
        }
    }

    private const int HALF_MOVES_FOR_DRAW = 100;

    private Prev_move_memo prev_moves = new();
    private int half_moves = 0;
    public bool is_draw { get; private set; } = false;

    public Draw_data() { }

    private Draw_data(Prev_move_memo prev_moves, int half_moves)
    {
        this.prev_moves = prev_moves;
        this.half_moves = half_moves;
        is_draw = half_moves >= HALF_MOVES_FOR_DRAW || prev_moves.is_repeated;
    }

    public void next(Move move)
    {
        half_moves++;
        prev_moves.Push(move);
        is_draw = half_moves >= HALF_MOVES_FOR_DRAW || prev_moves.is_repeated;
    }

    public Draw_data next_get(Move move) => new(prev_moves.Push_get(move), half_moves + 1);
}

public readonly record struct Move_bools(bool is_king_moved, bool is_left_rook_moved, bool is_right_rook_moved)
{
    public bool is_king_moved { get; init; } = is_king_moved;
    public bool is_left_rook_moved { get; init; } = is_left_rook_moved;
    public bool is_right_rook_moved { get; init; } = is_right_rook_moved;

    public Move_bools(bool b) : this(b, b, b) { }
};
