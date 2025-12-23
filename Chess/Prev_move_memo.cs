namespace Chess
{
    public class Prev_move_memo
    {
        private const int SIZE = 12;
        private Move[] moves = new Move[SIZE];

        public Prev_move_memo()
        {
            for (int i = 0; i < SIZE; i++)
                moves[i] = new();
        }

        protected Prev_move_memo(Move[] moves)
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
}
