namespace Chess.Chess_code
{
    public class Move
    {
        public Pos from, to;

        public Move(Pos to, Pos from)
            => (this.from, this.to) = (from.copy(), to.copy());

        public Move() : this(new(), new())
        { }

        public static bool operator ==(Move a, Move b) => a.from == b.from && a.to == b.to;
        public static bool operator !=(Move a, Move b) => !(a == b);

        public bool is_None() => !from.isin_board_range() || !to.isin_board_range();

        public Move copy() => new(to.copy(), from.copy());

        public override string ToString() => $"[from: {from} to: {to}]";

        public override bool Equals(object? obj)
            => obj is Move other && this == other;

        public override int GetHashCode()
            => HashCode.Combine(to.GetHashCode(), from.GetHashCode());
    }
}
