namespace ChemSolution_re_API.Entities
{
    public class ElementValence
    {
        public Valence Valence { get; set; }
        public int ElementId { get; set; }
        public Element Element { get; set; } = new();
    }
}
