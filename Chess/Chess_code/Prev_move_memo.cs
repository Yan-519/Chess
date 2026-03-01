namespace Chess
{
    public class Prev_move_memo
    {
        private const int SIZE = 9;
        private Move[] moves = new Move[SIZE];
        public bool is_repeated { get; private set; } = false;
        private readonly Func<int, int> Mod = n => ((n % SIZE) + SIZE) % SIZE;

        private int head_index = 0;
        private bool is_made_loop = false;

        public Prev_move_memo()
        {
            for (int i = 0; i < SIZE; i++)
                moves[i] = new Move();
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
            head_index = ++head_index % SIZE;

            is_made_loop |= head_index == 0;

            moves[head_index] = move;
            if (is_made_loop)
                is_repeated = repeated_check();
        }

        private bool repeated_check()
        {
            for (int i = head_index; Mod(i - 2) != head_index; i = Mod(i - 1))
                if (moves[i].from != moves[Mod(i - 2)].to)
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
}
