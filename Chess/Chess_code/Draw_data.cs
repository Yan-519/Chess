namespace Chess
{
    public class Draw_data
    {
        private const int HALF_MOVES_FOR_DRAW = 100;

        private Prev_move_memo prev_moves = new();
        private int half_moves = 0;

        public bool is_draw => prev_moves.is_repeated;
        public bool is_a_lot_of_moves => half_moves >= HALF_MOVES_FOR_DRAW;

        public Draw_data() { }

        private Draw_data(Prev_move_memo prev_moves, int half_moves)
        {
            this.prev_moves = prev_moves;
            this.half_moves = half_moves;
        }

        public Draw_data next_get(Move move) => new(prev_moves.Push_get(move), half_moves + 1);

        public void reset() => half_moves = 0;
    }
}