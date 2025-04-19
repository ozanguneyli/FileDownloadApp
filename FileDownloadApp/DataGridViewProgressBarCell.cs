// DataGridViewProgressBarCell.cs
// Custom DataGridView cell for progress bar

using System.Drawing;
using System.Windows.Forms;

namespace FileDownloadApp
{
    /// <summary>
    /// Custom DataGridView cell to display a progress bar.
    /// </summary>
    public class DataGridViewProgressBarCell : DataGridViewTextBoxCell
    {
        public DataGridViewProgressBarCell()
        {
            ValueType = typeof(int);
        }

        protected override void Paint(Graphics graphics,
                                      Rectangle clipBounds,
                                      Rectangle cellBounds,
                                      int rowIndex,
                                      DataGridViewElementStates cellState,
                                      object value,
                                      object formattedValue,
                                      string errorText,
                                      DataGridViewCellStyle cellStyle,
                                      DataGridViewAdvancedBorderStyle advancedBorderStyle,
                                      DataGridViewPaintParts paintParts)
        {
            // Draw cell background and border
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState,
                       null, null, errorText,
                       cellStyle, advancedBorderStyle,
                       paintParts & ~DataGridViewPaintParts.ContentForeground);

            int progressVal = value is int v ? v : 0;
            float pct = progressVal / 100f;
            var barRect = new Rectangle(
                cellBounds.X + 2,
                cellBounds.Y + 2,
                (int)((cellBounds.Width - 4) * pct),
                cellBounds.Height - 4
            );

            graphics.FillRectangle(Brushes.LightGreen, barRect);

            string text = progressVal + "%";
            var textSize = graphics.MeasureString(text, cellStyle.Font);
            var textLoc = new PointF(
                cellBounds.X + (cellBounds.Width - textSize.Width) / 2,
                cellBounds.Y + (cellBounds.Height - textSize.Height) / 2
            );
            graphics.DrawString(text, cellStyle.Font, Brushes.Black, textLoc);
        }
    }

    /// <summary>
    /// Custom DataGridView column for progress bar cells.
    /// </summary>
    public class DataGridViewProgressBarColumn : DataGridViewColumn
    {
        public DataGridViewProgressBarColumn()
            : base(new DataGridViewProgressBarCell())
        {
            ValueType = typeof(int);
        }
    }
}
