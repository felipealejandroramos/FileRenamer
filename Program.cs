namespace FindAndRename
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string directorypath = AskForDirectoryPath();
            FileList filelist = new(directorypath);
            while (filelist.GetFiles() == null)
            {
                directorypath = AskForDirectoryPath();
                filelist = new(directorypath);
            }

            bool unresolved = true;
            filelist.LayoutList();
            while (unresolved)
            {
            Console.WriteLine("inserica la estensione desiderata:  ");
            string? extension = Console.ReadLine();
            while (extension==null)
            {
                Console.WriteLine("nessun valore estensione \n inserica la estensione desiderata o exit per uscire");
                extension = Console.ReadLine();
                if (extension == "exit")
                    Environment.Exit(0);
            }
            filelist.SetExtension(extension);
            filelist.FilterFiles();
            Console.WriteLine("i file con la estensione desiderata sono i seguenti: ");
            filelist.LayoutList();
        
                Console.WriteLine("Proseguire con la rinomina di questi file: \n si   no");

                string? answer = Console.ReadLine();

                switch (answer)
                {
                    case "exit":
                        Environment.Exit(0);
                        break;
                    case "si":
                    case "s":
                        filelist.RenameFiles();
                        unresolved = false;
                        break;
                    case "no":
                    case "n":
                        Console.WriteLine("quale tipo di file vuoi rinominare");
                        break;
                }
            }
        }
        private static string AskForDirectoryPath()
        {
            Console.Write("Inserisca il path della cartella o scriva exit per uscire: ");
            string? directorypath = Console.ReadLine();
            if (directorypath == "exit")
                Environment.Exit(0);
            while (string.IsNullOrWhiteSpace(directorypath) || !Path.Exists(directorypath))
            {
                Console.WriteLine("nessun  path trovato \n Inserisca il path della cartella o scriva exit per uscire");
                directorypath = Console.ReadLine();
                if (directorypath == "exit")
                    Environment.Exit(0);
            }
            return directorypath;
        }
    }
}
