using System.Diagnostics;
namespace CrlfChanger;

class Program
{
    const string UNIX2DOS_EXE = "UNIX2DOS.exe";
    const string UNIX2DOS_C = "UNIX2DOS.C";
    const string ArgFormat = "for /f %f IN ( 'dir /b /s {0}' ) DO @unix2dos %f";

    static string CurrentDir = string.Empty;
    static string WorkingDir = string.Empty;

    static void Main(string[] args)
    {
        WorkingDir = AppDomain.CurrentDomain.BaseDirectory;
        CurrentDir = Directory.GetCurrentDirectory();

        string fileToChange = ParseArguments(args);
        var formattedArg = string.Format(ArgFormat, fileToChange);

        CopyExeFile();
        Execute(formattedArg);
        DeleteExeFile();
    }

    static void Execute(string arg)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            WorkingDirectory = CurrentDir
        };

        var process = new Process { StartInfo = startInfo };

        process.Start();
        process.StandardInput.WriteLine(arg);
        process.StandardInput.WriteLine("exit");

        Console.WriteLine(process.StandardOutput.ReadToEnd());

        process.WaitForExit();
    }

    static string ParseArguments(string[] args)
    {
        var fileToChange = "*.htm *.html *.css *.js *.txt *.cs *.aspx *.svg *.cshtml";
        if (args.Length > 0)
        {
            fileToChange = "";
            foreach (var a in args)
            {
                fileToChange += a + " ";
            }
            fileToChange = fileToChange.Remove(fileToChange.Length - 1);
        }

        return fileToChange; 
    }

    static void CopyExeFile()
    {
        if (!File.Exists(CurrentDir + "/" + UNIX2DOS_C))
        {
            File.Copy(WorkingDir + "/" + UNIX2DOS_C, CurrentDir + "/" + UNIX2DOS_C);
        }

        if (!File.Exists(CurrentDir + "/" + UNIX2DOS_EXE))
        {
            File.Copy(WorkingDir + "/" + UNIX2DOS_EXE, CurrentDir + "/" + UNIX2DOS_EXE);
        }
    }

    static void DeleteExeFile()
    {
        if (WorkingDir == CurrentDir)
            return;

        if (File.Exists(CurrentDir + "/" + UNIX2DOS_C))
        {
            File.Delete(CurrentDir + "/" + UNIX2DOS_C);
        }

        if (File.Exists(CurrentDir + "/" + UNIX2DOS_EXE))
        {
            File.Delete(CurrentDir + "/" + UNIX2DOS_EXE);
        }
    }
}
