namespace AssemblyImport.Tests
{
    public class ImportProperties
    {
        public int Prop1 { get; set; }
        public int Prop2 { get; }
        public int Prop3 { get; private set; }

        public int Prop4
        {
            set { }
        }

        public int this[int fst, int snd]
        {
            get { return 0; }
        }
    }
}
