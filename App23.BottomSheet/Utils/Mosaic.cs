using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace App23.BottomSheet.Utils;

public abstract class Mosaic
{
    public static void Draw(Rectangle rect)
    {
        var mosaicBrush = new DrawingBrush();
        var drawingGroup = new DrawingGroup();

        const double tileSize = 20; // 每个马赛克块的大小

        for (var y = 0; y < 2; ++y)
        {
            for (var x = 0; x < 2; ++x)
            {
                var isWhite = (x + y) % 2 == 0;
                var tile = new GeometryDrawing
                {
                    Brush = isWhite ? Brushes.White : Brushes.LightGray,
                    Geometry = new RectangleGeometry(new Rect(x * tileSize, y * tileSize, tileSize, tileSize))
                };
                drawingGroup.Children.Add(tile);
            }
        }

        mosaicBrush.Drawing = drawingGroup;
        mosaicBrush.TileMode = TileMode.Tile;
        mosaicBrush.Viewport = new Rect(0, 0, tileSize * 2, tileSize * 2);
        mosaicBrush.ViewportUnits = BrushMappingMode.Absolute;

        rect.Fill = mosaicBrush;
    }
}