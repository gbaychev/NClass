namespace AssemblyImport.Tests
{
    public class ImportMethods
    {
        static ImportMethods() {}

        ImportMethods(int val) {}
        private ImportMethods(int value1, int value2) { }

        int Method1(System.Tuple<int, int?>[] array, out int value) 
        {
            value = 0;
            return 1;
        }

        System.Collections.Generic.IEnumerable<System.Tuple<System.ConsoleKey, System.ConsoleKey?>> Method2()
        {
            return null;
        }

        void Method3(System.Collections.Generic.IEnumerable<System.Tuple<System.ConsoleKey, System.ConsoleKey?>> src)
        { }

        ~ImportMethods() { }
    }
}
