namespace Chess
{
    public class Pos
    {
        public int row, col;

        public Pos(int row, int column) => (this.row, col) = (row, column);

        public Pos() : this(-1, -1) { }


        public static bool operator ==(Pos a, Pos b) => a.is_on(b.row, b.col);

        public static bool operator !=(Pos a, Pos b) => !(a == b);


        public bool is_None() => !isin_board_range();

        public bool is_on(int row, int column) => this.row == row && col == column;

        public Pos copy() => new(row, col);

        public bool isin_board_range()
            => row >= 0 && row <= 7 &&
               col >= 0 && col <= 7;

        public override string ToString() => $"({row},{col})";

        public override bool Equals(object? obj) => obj is Pos other && this == other;

        public override int GetHashCode() => HashCode.Combine(row, col);
    }
}
