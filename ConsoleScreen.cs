namespace Epsi.MazeCs
{
    public class ConsoleScreen : IGridDisplay
    {
        public int OffsetX { get; }
        public int OffsetY { get; }
        public int MarginYMessage { get; }
        public int MessageHeight { get; }

        public ConsoleColor SuccessColor { get; }
        public ConsoleColor DangerColor { get; }
        public ConsoleColor InfoColor { get; }
        public ConsoleColor InstructionColor { get; }
        public ConsoleColor WallColor { get; }
        public ConsoleColor CorridorColor { get; }
        public ConsoleColor PlayerColor { get; }
        public ConsoleColor ExitColor { get; }

        public string Instructions { get; }
        public string WinText { get; }
        public string CanceledText { get; }
        public string PressKey { get; }
        public string HeaderText { get; }

        public ConsoleScreen()
        {
            OffsetX = 0;
            OffsetY = 3;
            MarginYMessage = 3;
            MessageHeight = 5;

            SuccessColor     = ConsoleColor.Green;
            DangerColor      = ConsoleColor.Red;
            InfoColor        = ConsoleColor.Cyan;
            InstructionColor = ConsoleColor.DarkCyan;
            WallColor        = ConsoleColor.DarkGray;
            CorridorColor    = ConsoleColor.DarkBlue;
            PlayerColor      = ConsoleColor.Yellow;
            ExitColor        = ConsoleColor.Green;

            Instructions  = "  [Z/↑] Haut   [S/↓] Bas   [Q/←] Gauche   [D/→] Droite   [Échap] Quitter";
            WinText       = "🎉  FÉLICITATIONS !  🎉\nVous avez trouvé la sortie !";
            CanceledText  = "Partie abandonnée. À bientôt !";
            PressKey      = "Appuyez sur une key pour quitter...";
            HeaderText    = "🏃 LABYRINTHE ASCII  C#  🏃";
        }

        public void DrawTextXY(int x, int y, string text, ConsoleColor? color = null)
        {
            Console.SetCursorPosition(x, y);
            if (color.HasValue)
            {
                Console.ForegroundColor = color.Value;
            }
            Console.Write(text);
            Console.ResetColor();
        }

        public void DrawTextColorXY(int x, int y, (string text, ConsoleColor color) info) =>
            DrawTextXY(x, y, info.text, info.color);

        public void DrawFramedText(int x, int y, string text, ConsoleColor? color = null)
        {
            var lines = text.Split('\n');
            int width = 0;
            foreach (var line in lines)
                if (line.Length > width) width = line.Length;

            string top = "╔" + new string('═', width + 2) + "╗";
            string bottom = "╚" + new string('═', width + 2) + "╝";
            DrawTextXY(x, y, top, color);
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i].PadRight(width);
                DrawTextXY(x, y + 1 + i, $"║ {line} ║", color);
            }
            DrawTextXY(x, y + 1 + lines.Length, bottom, color);
        }

        public void DrawCell(int cx, int cy, Cell cell)
        {
            var (symbol, color) = cell.GetDisplayInfo(this);
            DrawTextColorXY(OffsetX + cx, OffsetY + cy, (symbol, color));
        }
    }
}