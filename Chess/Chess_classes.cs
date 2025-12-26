namespace Chess;

public class Prev_move_memo
{
    private const int SIZE = 12;
    private Move[] moves = new Move[SIZE];

    public Prev_move_memo()
    {
        for (int i = 0; i < SIZE; i++)
            moves[i] = new();
    }

    private Prev_move_memo(Move[] moves)
    {
        for (int i = 0; i < SIZE; i++)
            this.moves[i] = moves[i].copy();
    }

    public void Push(Move move)
    {
        for (int i = SIZE - 1; i > 0; i--)
            moves[i] = moves[i - 1];

        moves[0] = move.copy();
    }

    public bool is_repeated()
    {
        for (int i = 0; i < SIZE - 2; i++)
            if (moves[i].from != moves[i + 2].to || moves[i + 2].is_None())
                return false;
        return true;
    }

    public Prev_move_memo Push_get(Move move)
    {
        Prev_move_memo prev = new(moves);
        prev.Push(move);
        return prev;
    }
}

public class Draw_data
{
    private const int HALF_MOES_FOR_DRAW = 100;

    private Prev_move_memo prev_moves = new();
    private int half_moves = 0;

    public Draw_data() { }

    private Draw_data(Prev_move_memo prev_moves, int half_moves)
    {
        this.prev_moves = prev_moves;
        this.half_moves = half_moves;
    }

    public void next(Move move)
    {
        half_moves++;
        prev_moves.Push(move);
    }

    public Draw_data next_get(Move move) => new(prev_moves.Push_get(move), half_moves + 1);

    public bool is_draw() => half_moves >= HALF_MOES_FOR_DRAW || prev_moves.is_repeated();
}

public record struct Move_bools(bool is_king_moved, bool is_left_rook_moved, bool is_right_rook_moved)
{
    public bool is_king_moved = is_king_moved;
    public bool is_left_rook_moved = is_left_rook_moved;
    public bool is_right_rook_moved = is_right_rook_moved;

    public Move_bools(bool b) : this(b, b, b) { }
};
