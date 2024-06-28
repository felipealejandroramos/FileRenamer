namespace FindAndRename
{

    internal class FileList
    {

        private string[,] files = null!;
        private readonly string path = "C:";
        private string extension = "";
        /// <summary>
        /// costruttore di oggetti FileList richiede il path della cartella di cui fare la lista file 
        /// </summary>
        /// <param name="directoryPath"></param>

        public FileList(string directoryPath)
        {
            try
            {
                path = directoryPath;
                files = new string[2, Directory.GetFiles(path).Length];
                for (int i = 0; i < Directory.GetFiles(path).Length; i++)
                {
                    files[0, i] = Directory.GetFiles(path)[i];
                    files[1, i] = "unrenamed";
                }

            }
            catch (Exception exception)
            {
                switch (exception)
                {
                    case DirectoryNotFoundException:
                        Console.WriteLine($"nessuna cartella nel path {path} è stata trovata");
                        break;
                    case PathTooLongException:
                        Console.WriteLine("il path inserito è toppo lungo");
                        break;
                    case UnauthorizedAccessException:
                        Console.WriteLine("non ha il permesso di accedere al path inserito");

                        break;
                    case IOException:
                        Console.WriteLine("il valore inserito non è un path valido");

                        break;
                    case ArgumentNullException:
                        Console.WriteLine("nessun path inserito");

                        break;
                    default:
                        Console.WriteLine("errore: " + exception.Message);

                        break;

                }

            }
        }
        /// <summary>
        /// controlla se il file esiste ancora nel path associato
        /// </summary>
        /// <param name="file"></param>
        private static void CheckExistence(string file)
        {
            if (!System.IO.Path.Exists(file) && file != null)
            {
                Console.WriteLine($"il file {Path.GetFileName(file)} è stato cancellato o spostato continuare con la esecuzione? \n Si            No ");
                bool resolved = false;
                while (!resolved)
                {
                    var responce = Console.ReadLine();
                    switch (responce)
                    {
                        case "exit":
                        case "No":
                        case "no":
                        case "n":
                            Environment.Exit(0);
                            break;
                        case "Si":
                        case "si":
                        case "s":
                            resolved = true;
                            break;
                        default:
                            Console.WriteLine($"risposta invalida riprovare \n il file {Path.GetFileName(file)} è stato cancellato o spostato continuare con la esecuzione? \n Si            No ");
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// restituisce un array di stringhe che rapresentano i percorsi dei file. 
        /// </summary>
        /// <returns> un array di stringhe o null se non e stato inizializato files</returns>
        public string[,] GetFiles()
        {
            if (files != null)
                return files;
            string[,] vuoto = new string[0, 0];
            return vuoto;
        }
        /// <summary>
        /// modifica il campo extension che seve per filrare i file
        /// </summary>
        /// <param name="extension"></param>
        public void SetExtension(string extension)
        {
            for (int i = 0; i < Directory.GetFiles(path).Length; i++)
            {
                files[0, i] = Directory.GetFiles(path)[i];
                files[1, i] = "unrenamed";
            }
            this.extension = extension;
        }
        /// <summary>
        /// filtra l' array files del oggetto FileList sulla base di una estensione passata come parametro 
        /// </summary>
        /// <param name="extension"> stringa rapresentante una estensione file</param>
        public void FilterFiles()
        {
            if (files != null)
            {
                string[,] filteredFiles = new string[2, files.Length / 2];
                try
                {
                    for (int i = 0; i < files.Length / 2; i++)
                    {
                        CheckExistence(files[0, i]);
                        if (extension.Equals(System.IO.Path.GetExtension(files[0, i])) || extension.Equals(System.IO.Path.GetExtension(files[0, i])[1..]) || extension == "")
                        {
                            filteredFiles[0, i] = files[0, i];
                            filteredFiles[1, i] = files[1, i];
                        }
                    }
                    files = filteredFiles;
                }
                catch (Exception exception)
                {
                    switch (exception)
                    {
                        case ArgumentNullException:
                            Console.WriteLine("non è stato trovato il valore da filtrare");
                            break;
                        case ArgumentException:
                            Console.WriteLine("non è arrivata una risposta in tempo");
                            break;
                        default:
                            Console.WriteLine("errore: " + exception);
                            break;
                    }

                    Environment.Exit(1);
                }
            }
        }
        /// <summary>
        /// aggiunge la data alla parte finale del nome a tutti i file referenziati nella array files di un oggetto FileList
        /// </summary>
        public void RenameFiles()
        {
            do
            {
                if (files != null)
                {
                    for (int i = 0; i < files.Length / 2; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(files[0, i]) && files[1, i] != "renamed")
                        {
                            CheckExistence(files[0, i]);
                            string newName = System.IO.Path.GetFileNameWithoutExtension(files[0, i]) + "_" + DateTime.Now.ToString("yyyy-MM-dd") + System.IO.Path.GetExtension(files[0, i]);
                            try
                            {
                                if (!Regexp.CheckIfDated(files[0, i]))
                                {
                                    if (System.IO.Path.Exists(System.IO.Path.Combine([System.IO.Path.GetDirectoryName(files[0, i])!, newName])))
                                    {
                                        AskIfFileAlreadyExist(i, newName);
                                    }
                                    else
                                    {
                                        File.Move(files[0, i], System.IO.Path.Combine([System.IO.Path.GetDirectoryName(files[0, i])!, newName]));
                                        Console.WriteLine(System.IO.Path.GetFileName(files[0, i]) + " è ora " + newName);
                                        files[0, i] = System.IO.Path.Combine([System.IO.Path.GetDirectoryName(files[0, i])!, newName]);
                                        files[1, i] = "renamed";
                                    }
                                }
                                else
                                {
                                    DateUpdater(i);
                                }
                            }
                            catch (Exception exception)
                            {

                                switch (exception)
                                {
                                    case ArgumentException:
                                        Console.WriteLine($"nessun valore");
                                        break;
                                    case FileNotFoundException:
                                        Console.WriteLine($"il file {files[0, i]} non è stato trovato in fase di rinomina");
                                        break;
                                    case UnauthorizedAccessException:
                                        Console.WriteLine($"non ci sono i permessi per rinominare {files[0, i]}");
                                        break;
                                    case IOException:
                                        Console.WriteLine($"il valore {files[0, i]} non è valido");
                                        break;
                                    default:
                                        Console.WriteLine("errore " + exception.Message);
                                        break;
                                }



                            }
                        }

                    }
                }
            } while (CheckFolder());
        }

        /// <summary>
        /// ti chiede come agire se il file che prova a creare esiste già
        /// </summary>
        /// <param name="file"></param>
        /// <param name="newName"></param>
        private void AskIfFileAlreadyExist(int filePositon, string newName)
        {
            string file = files[0, filePositon];
            CheckExistence(file);
            Console.WriteLine($" Il file {newName} esite già come devo proseguire? \n 1) Interrompi la esecuzione \n 2) Ignora il file {System.IO.Path.GetFileName(file)} \n 3) Sovrascrivi il file {newName} ");
            bool resolved = false;
            while (!resolved)
            {
                var responce = Console.ReadLine();
                switch (responce)
                {
                    case "exit":
                    case "interrompi":
                    case "1":
                        Environment.Exit(0);
                        break;
                    case "ignora":
                    case "2":
                        resolved = true;
                        break;
                    case "sovrascrivi":
                    case "3":
                        try
                        {
                            File.Move(file, System.IO.Path.Combine([System.IO.Path.GetDirectoryName(file)!, newName]), true);
                            Console.WriteLine($" Il file {newName} è stato sovrascritto ");
                            for (int i = 0; i < files.Length / 2; i++)
                            {
                                if (files[0, i] == System.IO.Path.Combine([System.IO.Path.GetDirectoryName(files[0, i])!, newName]))
                                {
                                    files[0, i] = "";
                                }
                            }
                            files[0, filePositon] = newName;
                            files[1, filePositon] = "rinominato";
                            resolved = true;
                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine($"non è stato possibile sovrascrivere {newName} per il seguente motivo:");
                            switch (exception)
                            {
                                case FileNotFoundException:
                                    Console.WriteLine($"il file {files[0, filePositon]} non è stato trovato in fase di sovrascrizione di {newName}");
                                    break;
                                case UnauthorizedAccessException:
                                    Console.WriteLine($"non ci sono i permessi per esseguire questa operazione di sovrascrizione");
                                    break;
                                default:
                                    Console.WriteLine("errore " + exception.Message);
                                    break;
                            }
                        }

                        break;
                    default:
                        Console.WriteLine($"risposta invalida riprovare \n  Il file {newName} esite già come devo proseguire? \n 1) Interrompi la esecuzione \n 2) Ignora il file {System.IO.Path.GetFileName(file)} \n 3) Sovrascrivi il file {newName}");
                        break;
                }
            }
        }
        /// <summary>
        /// aggiorna la data alla fine del nome del file a quella odierna
        /// </summary>
        /// <param name="file"></param>
        private void DateUpdater(int filePosition)
        {
            string file = files[0, filePosition];
            CheckExistence(file);
            int startDate = file.LastIndexOf('_');
            string date = file.Substring(startDate, 11);
            if (date == "_" + DateTime.Now.ToString("yyyy-MM-dd"))
                return;

            Console.WriteLine($"il file {file} ha gia una data vuole aggiornarla? \n Si            No");

            bool resolved = false;
            while (!resolved)
            {
                var responce = Console.ReadLine();
                switch (responce)
                {
                    case "exit":
                        Environment.Exit(0);
                        break;
                    case "No":
                    case "no":
                    case "n":
                        resolved = true;
                        break;
                    case "Si":
                    case "si":
                    case "s":
                        string newName = file.Replace(date, "_" + DateTime.Now.ToString("yyyy-MM-dd"));
                        try
                        {
                            File.Move(file, System.IO.Path.Combine([System.IO.Path.GetDirectoryName(file)!, newName]));
                            Console.WriteLine(System.IO.Path.GetFileName(file) + " è ora " + newName);
                            files[0, filePosition] = newName;
                            files[1, filePosition] = "renamed";
                            resolved = true;
                        }
                        catch (Exception exception)
                        {
                            if (System.IO.Path.Exists(System.IO.Path.Combine([System.IO.Path.GetDirectoryName(file)!, newName])))
                            {
                                AskIfFileAlreadyExist(filePosition, newName);
                                resolved = true;
                            }
                            else
                            {

                                switch (exception)
                                {
                                    case FileNotFoundException:
                                        Console.WriteLine($"il file {file} non è stato trovato in fase di rinomina");
                                        break;
                                    case UnauthorizedAccessException:
                                        Console.WriteLine($"non ci sono i permessi per rinominare {file}");
                                        break;
                                    case IOException:
                                        Console.WriteLine($"il valore {file} non è valido");

                                        break;
                                    default:
                                        Console.WriteLine("errore " + exception.Message);
                                        break;
                                }
                            }
                        }
                        break;
                    default:
                        Console.WriteLine($"risposta invalida riprovare \n il file {file} ha gia una data vuole aggiornarla? \n Si            No");
                        break;
                }
            }
        }
        /// <summary>
        /// mostra la lista file con x elementi alla volta
        /// </summary>
        public void LayoutList()
        {
            bool satisfied = false;
            int pageStart = 0;
            int pageLenght = 4;
            int elementQuantity = files.Length / 2;
            int filesNumber = 0;
            int pageNumber = 1;
            int fileNumber;
            int nulls = 0;
            for (int i = 0; i < elementQuantity; i++)
            {
                if (files[0, i] != null)
                {
                    filesNumber++;
                }
            }
            do
            {

                int pageNulls = pageLenght;
                Console.Clear();
                for (fileNumber = 0; fileNumber < pageNulls; fileNumber++)
                {
                    if (fileNumber + pageStart < elementQuantity)
                    {
                        if (string.IsNullOrWhiteSpace(files[0, fileNumber + pageStart]))
                        {
                            pageNulls++;
                        }
                        else if (fileNumber + pageStart < elementQuantity)
                        {
                            Console.WriteLine(files[0, fileNumber + pageStart]);
                        }
                    }


                }
                if (pageStart == files.Length / 2)
                {
                    Console.WriteLine("      nessun file trovato    ");
                }

                Console.WriteLine($"Sono stati trovati {filesNumber} Numero Pagina {pageNumber}/{(filesNumber + pageLenght - 1) / pageLenght} lunghezza pagina {pageLenght}");
                Console.WriteLine($"L: aumenta la dimensione della pagina   ->/giu/enter: passare alla prossima    <-/su/bakspace: per tornare alla precedente   Q/esc: per uscire");
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.Q:
                    case ConsoleKey.Escape:
                        satisfied = true;
                        break;
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.Enter:
                        nulls = CalculateNulls(true, pageLenght, pageStart);
                        if (fileNumber + pageStart + 1 <= elementQuantity && nulls + pageStart + 1 != elementQuantity)
                        {
                            pageStart += fileNumber;
                            pageNumber++;
                        }
                        else
                        {
                            pageNumber = (filesNumber + pageLenght - 1) / pageLenght;

                        }
                        break;
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.Backspace:
                        nulls = CalculateNulls(false, pageLenght, pageStart);
                        if (pageStart - (nulls + pageLenght) <= 0)
                        {
                            pageNumber = 1;
                            pageStart = 0;
                        }
                        else
                        {
                            pageNumber--;
                            pageStart -= pageLenght;
                        }

                        break;
                    case ConsoleKey.L:
                        Console.WriteLine("inserica la lunghezza desiderata");
                        int number;
                        string? input = Console.ReadLine();
                        if (int.TryParse(input, out number))
                        {
                            if (number >= elementQuantity && number < 0)
                            {
                                number = filesNumber;
                            }
                            pageNumber = 1;
                            pageStart = 0;
                            pageLenght = number;
                        }
                        break;
                }

            } while (!satisfied);
        }
        /// <summary>
        /// controlla quanti valori nulli ci saranno nella prossima pagina e li restituisce come un valore int si danno come valori la direzione come bool (true avanti false dietro), la lunghezza della pagina e il punto di partenza
        /// </summary>
        /// <param name="foward"></param>
        /// <param name="pageLenght"></param>
        /// <param name="pageStart"></param>
        /// <returns></returns>
        private int CalculateNulls(bool foward, int pageLenght, int pageStart)
        {
            int nulls = 0;

            int elementQuantity = files.Length / 2;
            pageStart = foward ? pageStart + pageLenght : pageStart;
            for (int fileNumber = 0; fileNumber < (nulls + pageLenght); fileNumber++)
            {
                if (pageStart >= 0 && pageStart < elementQuantity && ((fileNumber + pageStart < elementQuantity && string.IsNullOrWhiteSpace(files[0, fileNumber + pageStart]) && foward) || (pageStart - 1 - fileNumber > 0 && string.IsNullOrWhiteSpace(files[0, pageStart - 1 - fileNumber]) && !foward)))
                {
                    nulls++;
                }

            }
            return nulls;
        }
        /// <summary>
        /// controlla se ce stata qualche modifica alla cartella a cui fa riferimento la lista se ce stata restituisce true se no false
        /// </summary>
        /// <returns></returns>
        private bool CheckFolder()
        {
            bool found, updated = false;
            FileList listChecker = new(path);
            listChecker.SetExtension(extension);
            listChecker.FilterFiles();
            string[,] listCheckerFiles = listChecker.GetFiles();

            for (int i = 0; i < listCheckerFiles.Length / 2; i++)
            {
                found = false;
                for (int j = 0; j < files.Length / 2; j++)
                {

                    if (listCheckerFiles[0, i] == files[0, j])
                    {
                        listCheckerFiles[0, i] = files[0, j];
                        listCheckerFiles[1, i] = files[1, j];
                        found = true;
                        break;
                    }
                    else if (j == (files.Length / 2) - 1 && !found)
                    {
                        updated = true;
                    }

                }
            }
            files = listCheckerFiles;
            return updated;
        }

    }
}
