
namespace _20240723_SqlDb_Gai.Models
{
    /// <summary>
    /// entity that summarizes dates from Car, Mark, Color
    /// </summary>
    public class CarMarkPaint
    {
        public string number { get; set; } = null!;
        public string mark{ get; set; } = null!;
        public string model { get; set; } = null!;
        public float paintThk { get; set; }
        public int ral { get; set; }
        public string paintType { get; set; } = null!;

        public CarMarkPaint() : this("XX0000XX", "none", "none", 0, 0, "none") { }

        public CarMarkPaint(string number, string mark, string model, float paintThk, int ral, string paintType)
        {
            this.number = number.ToUpper();
            this.mark = mark.ToUpper();
            this.model = model.ToUpper();
            this.paintThk = paintThk;
            this.ral = ral;
            this.paintType = paintType;
        }
    }
}
