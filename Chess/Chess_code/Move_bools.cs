namespace Chess
{
    public readonly record struct Move_bools(bool is_king_moved, bool is_left_rook_moved, bool is_right_rook_moved)
    {
        public bool is_king_moved { get; init; } = is_king_moved;
        public bool is_left_rook_moved { get; init; } = is_left_rook_moved;
        public bool is_right_rook_moved { get; init; } = is_right_rook_moved;

        public Move_bools(bool b) : this(b, b, b) { }
    };
}
